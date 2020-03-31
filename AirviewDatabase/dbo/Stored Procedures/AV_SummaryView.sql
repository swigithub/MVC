
CREATE PROCEDURE [dbo].[AV_SummaryView] 
	-- Add the parameters for the stored procedure here
	@CHILDFILTER varchar(max)
	,@PARENTFILTER VARCHAR(MAX)
	,@FROMDATE DATETIME
	,@TODATE DATETIME
	,@FilterType as nvarchar(50)
	,@FilterValue as int
	,@UserID as INT
	,@FilterType2 AS NVARCHAR(50)=NULL
	,@FilterValue2 as INT=0
	,@CountryId varchar(50)=null
	,@Client varchar(50)=null
	,@Scopes varchar(50) =N'0'
	,@Projects varchar(50) = null
AS

IF @Scopes=N'0'
BEGIN
	SELECT @Scopes = COALESCE(@Scopes + ', ', '') + CAST(sus.ScopeId AS varchar(15))
	FROM Sec_UserScopes AS sus
	WHERE sus.UserId=@UserID
	
	SET @Scopes+=','
END
ELSE
BEGIN
	SET @Scopes+=','
END

DECLARE @FilterOption AS nvarchar(50)
DECLARE @Cities TABLE(CityId int)
DECLARE @cityId nvarchar(max)=''



IF  (@FilterType2='Regional View')
BEGIN
	IF (@FilterValue2=0)
	BEGIN
		DECLARE @RegionId varchar(max)=''
		SELECT @RegionId = COALESCE(@RegionId + ', ', '') + CAST(DefinationId AS varchar(15))
		FROM AD_Definations rgn
		WHERE rgn.DefinationTypeId= 6

		SET @RegionId+=','

		SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(DefinationId AS varchar(15))
		FROM AD_Definations
		WHERE DefinationTypeId= 7
		AND Charindex(cast(PDefinationId as varchar(max))+',', @RegionId) > 0;

		SET @cityId+=','
	END
	ELSE
	BEGIN
		SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(DefinationId AS varchar(15))
		FROM AD_Definations
		WHERE DefinationTypeId= 7
		AND PDefinationId=@FilterValue2;

		SET @cityId+=','
	END
	
	INSERT INTO @Cities
	SELECT cty.CityId	
	FROM Sec_UserCities cty
	WHERE cty.UserId=@UserID AND Charindex(cast(cty.CityId as varchar(max))+',', @CityId) > 0;
END
ELSE IF  (@FilterType2='Market View')
BEGIN
	IF (@FilterValue2=0)
	BEGIN
		SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(DefinationId AS varchar(15))
		FROM AD_Definations
		WHERE DefinationTypeId= 7		

		SET @cityId+=','
	END
	ELSE
	BEGIN
		SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(DefinationId AS varchar(15))
		FROM AD_Definations
		WHERE DefinationTypeId= 7
		AND DefinationId=@FilterValue2;

		SET @cityId+=','

		--SELECT @listId
	END
	
	INSERT INTO @Cities
	SELECT cty.CityId	
	FROM Sec_UserCities cty
	WHERE cty.UserId=@UserID AND Charindex(cast(cty.CityId as varchar(max))+',', @CityId) > 0;
END
ELSE
BEGIN
	INSERT INTO @Cities
	SELECT cty.CityId	
	FROM Sec_UserCities cty
	WHERE cty.UserId=@UserID;
END

PRINT @CityId


IF @FilterType = 'Regional View'
		BEGIN
			SET @FilterOption = 'Region'
		END
	ELSE IF @FilterType = 'Market View'
		BEGIN
			SET @FilterOption = 'Market'
		END
	ELSE IF @FilterType = 'Client View'
		BEGIN
			SET @FilterOption = 'Client'
		END
	ELSE IF @FilterType = 'Drive Tester View'
		BEGIN
			SET @FilterOption = 'Tester'
		END
	ELSE IF @FilterType = 'ClientPOC View'
		BEGIN
			SET @FilterOption = 'ClientPOC'
		END
	ELSE IF @FilterType = 'SwiView View'
		BEGIN
			SET @FilterOption = 'SwiPOC'
		END

BEGIN

SELECT clt.ClientId
INTO #Clients
FROM UserClients clt
WHERE clt.UserId=@UserID -- AND Charindex(cast(clt.ClientId as varchar(max))+',', @listId) > 0;;

DECLARE @DATE1 AS DATETIME
DECLARE @DATE2 AS DATETIME

	IF DATEDIFF(DAY,@FROMDATE,@TODATE)>=0 AND DATEDIFF(DAY,@FROMDATE,@TODATE)<=7
	BEGIN
		SET @DATE1 = CAST(@FROMDATE AS DATE)
		SET @DATE2 = CAST(@TODATE AS DATE)
	END
	ELSE IF DATEDIFF(DAY,@FROMDATE,@TODATE)>7
	BEGIN
		IF @CHILDFILTER='DateRange'
		BEGIN
			SET @DATE1 = CAST(@FROMDATE AS DATE)
			SET @DATE2 = CAST(@TODATE AS DATE)
		END
		ELSE 
		BEGIN
			DECLARE @MonthStart AS DATE = CAST(DATEADD(month, DATEDIFF(month, 0, @TODATE), 0) AS DATE);
			DECLARE @diffFromCurr AS INT = 0;
		
			SET @DATE2 = DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, @TODATE) + 1, 0))--CAST(@TODATE AS DATE)
			IF MONTH(@TODATE)=MONTH(GETDATE())
			BEGIN
				SET @diffFromCurr =DATEDIFF(DAY,@MonthStart,CAST(GETDATE() AS DATE));
				SET @DATE1 = DATEADD(mm, DATEDIFF(mm, 0, @FROMDATE) - 1, 0)+@diffFromCurr
			END
			ELSE
			BEGIN
				SET @diffFromCurr =DATEDIFF(DAY,@MonthStart,CAST(@TODATE AS DATE));
				SET @DATE1 = @FROMDATE
			END			
		END
	END
	--PRINT @DATE1
	--PRINT @DATE2

IF @FilterOption='Region'
BEGIN
	--REGIONAL VIEW
	SELECT rgn.RegionId, rgn.Region, ISNULL(ste.RegionCompletedSites,0) RegionCompletedSites, ISNULL(ste.RegionInProcessSites,0) RegionInProcessSites, ISNULL(ste.RegionPendingSites,0) RegionPendingSites
	, ISNULL(ste.RegionTotalSites,0) RegionTotalSites, ISNULL(ste.RegionDriveCompletedSites,0) RegionDriveCompletedSites, ISNULL(ste.RegionPendingWithIssuesSites,0) RegionPendingWithIssuesSites
	,ISNULL(ste.RegionInProgress,0) RegionInProgress,ISNULL(ste.RegionReportSubmitted,0) RegionReportSubmitted
	FROM 
	(
		SELECT DISTINCT rgn.DefinationId 'RegionId',stt.DefinationId 'CityId',stt.DefinationName + ' ' + rgn.DefinationName 'Region'
		FROM AD_Definations rgn
		INNER JOIN AD_Definations cty on rgn.DefinationId=cty.PDefinationId
		INNER JOIN AD_Definations stt on stt.DefinationId=rgn.PDefinationId
		WHERE rgn.DefinationTypeId=6
		AND cty.DefinationId IN(Select x.CityId FROM @Cities x)
	) rgn LEFT OUTER JOIN
	(
		SELECT  rgn.DefinationId 'RegionId',stt.DefinationId 'CityId', stt.DefinationName + ' ' + rgn.DefinationName 'Region',
			SUM(CASE WHEN Status = 89 THEN 1 ELSE 0 END) AS RegionCompletedSites,
			SUM(CASE WHEN Status = 91 THEN 1 ELSE 0 END) AS RegionInProcessSites ,
			SUM(CASE WHEN Status = 90 THEN 1 ELSE 0 END) AS RegionPendingSites,
			SUM(CASE WHEN Status = 92 THEN 1 ELSE 0 END) AS RegionDriveCompletedSites,
			SUM(CASE WHEN Status = 93 THEN 1 ELSE 0 END) AS RegionPendingWithIssuesSites, 
			SUM(CASE WHEN Status = 450 THEN 1 ELSE 0 END) AS RegionInProgress, 
			SUM(CASE WHEN Status = 451 THEN 1 ELSE 0 END) AS RegionReportSubmitted,
			COUNT(*) AS RegionTotalSites 
		FROM AD_Definations rgn
		INNER JOIN AD_Definations cty on rgn.DefinationId=cty.PDefinationId
		INNER JOIN AD_Definations stt on stt.DefinationId=rgn.PDefinationId
		INNER JOIN AV_Clusters cls ON cls.CityId = cty.DefinationId
		INNER JOIN AV_Sites ste ON ste.ClusterId = cls.ClusterId
		WHERE rgn.DefinationTypeId=6
		AND ste.CityId IN(Select x.CityId FROM @Cities x)
		AND (CAST(ste.SubmittedOn AS date) BETWEEN @DATE1 AND @DATE2)
		AND ste.IsActive=1
		AND ste.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
		AND (@Projects='0' or CHARINDEX(CAST(ste.ProjectId as nvarchar(20)),(',' + @Projects + ',')) > 0)
		GROUP BY stt.DefinationId,stt.DefinationName, rgn.DefinationId, rgn.DefinationName
	) ste ON rgn.RegionID=ste.RegionID AND rgn.CityId=ste.CityId
END
ELSE IF @FilterOption='Market'
BEGIN
	--MARKET VIEW
	SELECT rgn.RegionId, rgn.Region, ISNULL(ste.RegionCompletedSites,0) RegionCompletedSites, ISNULL(ste.RegionInProcessSites,0) RegionInProcessSites, ISNULL(ste.RegionPendingSites,0) RegionPendingSites
	, ISNULL(ste.RegionTotalSites,0) RegionTotalSites, ISNULL(ste.RegionDriveCompletedSites,0) RegionDriveCompletedSites, ISNULL(ste.RegionPendingWithIssuesSites,0) RegionPendingWithIssuesSites
	,ISNULL(ste.RegionInProgress,0) RegionInProgress,ISNULL(ste.RegionReportSubmitted,0) RegionReportSubmitted
	
	FROM 
	(
		SELECT cty.DefinationId 'RegionId',cty.DefinationName 'Region'
		FROM AD_Definations cty
		WHERE cty.DefinationTypeId=7
		AND cty.DefinationId IN(Select x.CityId FROM @Cities x)
	) rgn LEFT OUTER JOIN
	(
		SELECT  cty.DefinationId 'RegionId', cty.DefinationName 'Region',
			SUM(CASE WHEN Status = 89 THEN 1 ELSE 0 END) AS RegionCompletedSites,
			SUM(CASE WHEN Status = 91 THEN 1 ELSE 0 END) AS RegionInProcessSites ,
			SUM(CASE WHEN Status = 90 THEN 1 ELSE 0 END) AS RegionPendingSites, 
			SUM(CASE WHEN Status = 92 THEN 1 ELSE 0 END) AS RegionDriveCompletedSites,
			SUM(CASE WHEN Status = 93 THEN 1 ELSE 0 END) AS RegionPendingWithIssuesSites,
			SUM(CASE WHEN Status = 450 THEN 1 ELSE 0 END) AS RegionInProgress, 
			SUM(CASE WHEN Status = 451 THEN 1 ELSE 0 END) AS RegionReportSubmitted,
			COUNT(*) AS RegionTotalSites 
		FROM AD_Definations cty
		INNER JOIN AV_Clusters cls ON cls.CityId = cty.DefinationId
		INNER JOIN AV_Sites ste ON ste.ClusterId = cls.ClusterId
		WHERE cty.DefinationTypeId=7
		AND ste.CityId IN(Select x.CityId FROM @Cities x)
		AND (CAST(ste.SubmittedOn AS date) BETWEEN @DATE1 AND @DATE2)
		AND ste.IsActive=1
		AND ste.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
		AND (@Projects='0' or CHARINDEX(CAST(ste.ProjectId as nvarchar(20)),(',' + @Projects + ',')) > 0)
		GROUP BY cty.DefinationId,cty.DefinationName
	) ste ON rgn.RegionId=ste.RegionId
END
ELSE IF @FilterOption='Client'
BEGIN
--CLIENT VIEW
SELECT x.RegionId, x.Region, ISNULL(y.RegionCompletedSites,0) RegionCompletedSites, ISNULL(y.RegionInProcessSites,0) RegionInProcessSites, ISNULL(y.RegionPendingSites,0) RegionPendingSites
	, ISNULL(y.RegionTotalSites,0) RegionTotalSites, ISNULL(y.RegionDriveCompletedSites,0) RegionDriveCompletedSites, ISNULL(y.RegionPendingWithIssuesSites,0) RegionPendingWithIssuesSites
	,ISNULL(y.RegionInProgress,0) RegionInProgress,ISNULL(y.RegionReportSubmitted,0) RegionReportSubmitted

FROM
(
	SELECT clt.ClientId 'RegionId', clt.ClientName 'Region'
	FROM AD_Clients clt
	WHERE clt.IsActive=1
	AND clt.ClientId IN(Select x.ClientId FROM #Clients x)
) x LEFT OUTER JOIN
(
	SELECT  clt.ClientId 'RegionId', clt.ClientName 'Region',
			SUM(CASE WHEN Status = 89 THEN 1 ELSE 0 END) AS RegionCompletedSites,
			SUM(CASE WHEN Status = 91 THEN 1 ELSE 0 END) AS RegionInProcessSites ,
			SUM(CASE WHEN Status = 90 THEN 1 ELSE 0 END) AS RegionPendingSites, 
			SUM(CASE WHEN Status = 92 THEN 1 ELSE 0 END) AS RegionDriveCompletedSites,
			SUM(CASE WHEN Status = 93 THEN 1 ELSE 0 END) AS RegionPendingWithIssuesSites, 
			SUM(CASE WHEN Status = 450 THEN 1 ELSE 0 END) AS RegionInProgress, 
			SUM(CASE WHEN Status = 451 THEN 1 ELSE 0 END) AS RegionReportSubmitted,
			COUNT(*) AS RegionTotalSites 
		FROM AD_Clients clt		
		INNER JOIN AV_Sites ste ON ste.ClientId = clt.ClientId
		WHERE (CAST(ste.SubmittedOn AS date) BETWEEN @DATE1 AND @DATE2)
		AND clt.ClientId IN(Select x.ClientId FROM #Clients x)
		AND ste.IsActive=1
		AND ste.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
		AND (@Projects='0' or CHARINDEX(CAST(ste.ProjectId as nvarchar(20)),(',' + @Projects + ',')) > 0)
		GROUP BY clt.ClientId, clt.ClientName
) y ON x.RegionId=y.RegionId
END

IF @FilterOption='Tester'
BEGIN
	--DRIVE TESTER VIEW
	select DISTINCT u.UserId 'TesterId', (ISNULL(u.FirstName,'') + ' ' +ISNULL(u.LastName,'')) AS TesterName, REPLACE(u.Picture,'u-','thumb-') 'Picture', ISNULL(Temp.TesterCompletedSites,0) TesterCompletedSites,
	ISNULL(temp.TesterInProcessSites,0) TesterInProcessSites, ISNULL(temp.TesterPendingSites,0) TesterPendingSites, ISNULL(temp.TesterTotalSites,0) TesterTotalSites
	, ISNULL(temp.TesterDriveCompletedSites,0) TesterDriveCompletedSites, ISNULL(temp.TesterPendingWithIssuesSites,0) TesterPendingWithIssuesSites,
	(SELECT COUNT(DISTINCT x.SiteId) FROM AV_NetLayerStatus x WHERE x.TesterId=u.UserId AND CAST(LEFT(x.ScheduledOn,12) AS DATETIME)=CAST(LEFT(GETDATE(),12) AS DATETIME)) 'DtWoCount'
	,ISNULL(TEMP.TesterInProgress,0) TesterInProgress,ISNULL(TEMP.TesterReportSubmitted,0) TesterReportSubmitted
	
	FROM Sec_Users u 
	INNER JOIN Sec_UserScopes AS sus ON sus.UserId=u.UserId
	LEFT join Sec_UserRoles ur on u.UserId=ur.UserId
	LEFT OUTER JOIN Sec_UserCities AS suc ON suc.UserId=u.UserId
	left outer join
	(
		SELECT x.TesterId,
			SUM(CASE WHEN x.Status=90 THEN 1 ELSE 0 END) AS TesterPendingSites, 
			SUM(CASE WHEN x.Status = 91 THEN 1 ELSE 0 END) AS TesterInProcessSites ,
			SUM(CASE WHEN x.Status = 89 THEN 1 ELSE 0 END) AS TesterCompletedSites,
			SUM(CASE WHEN x.Status = 92 THEN 1 ELSE 0 END) AS TesterDriveCompletedSites,
			SUM(CASE WHEN x.Status = 93 THEN 1 ELSE 0 END) AS TesterPendingWithIssuesSites,
			SUM(CASE WHEN x.Status = 450 THEN 1 ELSE 0 END) AS TesterInProgress, 
			SUM(CASE WHEN x.Status = 451 THEN 1 ELSE 0 END) AS TesterReportSubmitted,
			COUNT(x.SiteId) AS TesterTotalSites 
		FROM
		(
			SELECT DISTINCT AV_Sites.SiteId,AV_Sites.[Status],anls.TesterId	
			FROM AV_Sites
			INNER Join AD_Definations ON AD_Definations.DefinationId = AV_Sites.CityId	
			INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=av_sites.SiteId			
			WHERE 
			(CAST(AV_Sites.SubmittedOn AS date) BETWEEN @DATE1 AND @DATE2)
			AND AV_Sites.CityId IN(Select x.CityId FROM @Cities x)
			AND AV_Sites.ClientId IN(Select x.ClientId FROM #Clients x)
			AND AV_Sites.IsActive=1
			AND AV_Sites.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
			AND (@Projects='0' or CHARINDEX(CAST(AV_Sites.ProjectId as nvarchar(20)),(',' + @Projects + ',')) > 0)
		) x
		GROUP BY x.TesterId
	) TEMP 
	ON Temp.TesterId = u.UserId
	Where ur.RoleId = 13 AND u.IsActive=1	
	AND sus.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
	
	AND suc.CityId IN(Select x.CityId FROM @Cities x)
	
END
ELSE IF @FilterOption='ClientPOC'
BEGIN
	--SELECT usr.UserId
	--INTO #ClientUsers
	--FROM Sec_Users usr
	--INNER JOIN AD_ClientContacts cnt ON cnt.UserId=usr.UserId
	--WHERE cnt.ClientId=(SELECT x.ClientId FROM AD_ClientContacts x WHERE x.UserId=@UserID)
	
	SELECT usr.TesterId, usr.TesterName, usr.Picture,
	SUM(CASE WHEN sit.Status = 89 THEN 1 ELSE 0 END) AS TesterCompletedSites,	
	SUM(CASE WHEN sit.Status = 91 THEN 1 ELSE 0 END) AS TesterInProcessSites,
	SUM(CASE WHEN sit.Status=90 THEN 1 ELSE 0 END) AS TesterPendingSites,	
	COUNT(sit.SiteId) AS TesterTotalSites,
	SUM(CASE WHEN sit.Status = 92 THEN 1 ELSE 0 END) AS TesterDriveCompletedSites,
	SUM(CASE WHEN sit.Status = 93 THEN 1 ELSE 0 END) AS TesterPendingWithIssuesSites,
	0 'DtWoCount',
	SUM(CASE WHEN sit.Status = 450 THEN 1 ELSE 0 END) AS TesterInProgress, 
	SUM(CASE WHEN sit.Status = 451 THEN 1 ELSE 0 END) AS TesterReportSubmitted
	FROM
	(
		SELECT su.UserId 'TesterId', suc.CityId,
		(su.FirstName + ' ' +su.LastName) AS TesterName, REPLACE(su.Picture,'u-','thumb-') 'Picture'
		FROM Sec_Users AS su
		INNER JOIN Sec_UserRoles AS sur ON su.UserId=sur.UserId
		INNER JOIN Sec_UserCities AS suc ON suc.UserId=su.UserId
		WHERE sur.RoleId=10018 AND su.IsActive=1
		AND suc.CityId IN(Select x.CityId FROM @Cities x)
		--AND su.UserId IN(Select x.UserId FROM #ClientUsers x)
	) usr INNER JOIN 
	(
		SELECT sit.SiteId, sit.cityID, sit.[Status]
		FROM AV_Sites sit
		WHERE sit.IsActive=1
		AND (CAST(sit.SubmittedOn AS date) BETWEEN @DATE1 AND @DATE2)
		AND sit.CityId IN(Select x.CityId FROM @Cities x)
		AND sit.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
		AND (@Projects='0' or CHARINDEX(CAST(sit.ProjectId as nvarchar(20)),(',' + @Projects + ',')) > 0)
	) sit ON sit.cityID=usr.CityId
	GROUP BY usr.TesterId, usr.TesterName, usr.Picture

	--DROP TABLE #ClientUsers
END

ELSE IF @FilterOption='SwiPOC'
BEGIN
	--SELECT usr.UserId
	--INTO #SwiUsers
	--FROM Sec_Users usr
	--INNER JOIN AD_ClientContacts cnt ON cnt.UserId=usr.UserId
	--WHERE cnt.ClientId=(SELECT x.ClientId FROM AD_ClientContacts x WHERE x.UserId=@UserID)
	
	SELECT usr.TesterId, usr.TesterName, usr.Picture,
	SUM(CASE WHEN sit.Status = 89 THEN 1 ELSE 0 END) AS TesterCompletedSites,	
	SUM(CASE WHEN sit.Status = 91 THEN 1 ELSE 0 END) AS TesterInProcessSites,
	SUM(CASE WHEN sit.Status=90 THEN 1 ELSE 0 END) AS TesterPendingSites,	
	COUNT(sit.SiteId) AS TesterTotalSites,
	SUM(CASE WHEN sit.Status = 92 THEN 1 ELSE 0 END) AS TesterDriveCompletedSites,
	SUM(CASE WHEN sit.Status = 93 THEN 1 ELSE 0 END) AS TesterPendingWithIssuesSites,
	0 'DtWoCount',
	SUM(CASE WHEN sit.Status = 450 THEN 1 ELSE 0 END) AS TesterInProgress, 
	SUM(CASE WHEN sit.Status = 451 THEN 1 ELSE 0 END) AS TesterReportSubmitted
	FROM
	(
		SELECT su.UserId 'TesterId', suc.CityId,
		(su.FirstName + ' ' +su.LastName) AS TesterName, REPLACE(su.Picture,'u-','thumb-') 'Picture'
		FROM Sec_Users AS su
		INNER JOIN Sec_UserRoles AS sur ON su.UserId=sur.UserId
		INNER JOIN Sec_UserCities AS suc ON suc.UserId=su.UserId
		WHERE sur.RoleId=10020 AND su.IsActive=1
		AND suc.CityId IN(Select x.CityId FROM @Cities x)
		--AND su.UserId IN(Select x.UserId FROM #ClientUsers x)
	) usr INNER JOIN 
	(
		SELECT sit.SiteId, sit.cityID, sit.[Status]
		FROM AV_Sites sit
		WHERE sit.IsActive=1
		AND (CAST(sit.SubmittedOn AS date) BETWEEN @DATE1 AND @DATE2)
		AND sit.CityId IN(Select x.CityId FROM @Cities x)
		AND sit.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
		AND (@Projects='0' or CHARINDEX(CAST(sit.ProjectId as nvarchar(20)),(',' + @Projects + ',')) > 0)
	) sit ON sit.cityID=usr.CityId
	GROUP BY usr.TesterId, usr.TesterName, usr.Picture

	--DROP TABLE #SwiUsers
END

--DROP TABLE @Cities
DROP TABLE #Clients
END