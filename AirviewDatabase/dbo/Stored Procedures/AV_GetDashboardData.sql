
CREATE PROCEDURE [dbo].[AV_GetDashboardData] 
	@CHILDFILTER varchar(max)
	,@PARENTFILTER VARCHAR(MAX)
	,@FROMDATE DATETIME
	,@TODATE DATETIME
	,@CITEIES Tb_Data READONLY
	,@UserID as int 
	,@Panel1Option nvarchar(100)
	,@Panel1Value numeric(18,0) 
	,@Panel2Option nvarchar(100)
	,@Panel2Value numeric(18,0)
	,@CountryId varchar(50)=null
	,@Client varchar(50)=null
	,@Scopes varchar(50) =N'0'
	,@Markets varchar(50) =null
	,@Projects varchar(50) =null
AS

--SELECT @PARENTFILTER 'parentFilter', @CHILDFILTER 'childFilter', @FROMDATE 'fromDate', @TODATE 'toDate', @UserID 'UserID', @Panel1Option 'Panel1Option', @Panel1Value 'Panel1Value',
--@Panel2Option 'Panel2Option', @Panel2Value 'Panel2Value'
--INTO tmpVariables

DECLARE @DATE1 AS DATETIME
DECLARE @DATE2 AS DATETIME

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


--IF @CHILDFILTER = 'Today'
--BEGIN
--SET @DATE1 =CAST(@FromDate AS date)
--SET @DATE2 = CAST(@TODATE AS date)
--END
--IF @CHILDFILTER = 'ThisWeeK'
--BEGIN
--SET @DATE1 = CAST(@FromDate AS date)--DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
--SET @DATE2 =  CAST(@TODATE AS date)--DATEADD(DAY, 7 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
--END
--IF @CHILDFILTER = 'ThisMonth'
--BEGIN
--SET @DATE1 =  CAST(@FromDate-31 AS date)--DATEADD(DAY,-31,CAST(DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) AS date))--CAST(DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) AS date)
--SET @DATE2 = CAST(@TODATE AS date)--CAST(DATEADD(DAY, -(DAY(DATEADD(MONTH, 1, GETDATE()))),DATEADD(MONTH, 1, GETDATE())) AS date)

--END
--IF @CHILDFILTER = 'Total'
--BEGIN
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
--END



DECLARE @FilterOption AS nvarchar(50)
DECLARE @clientId nvarchar(max)=''
DECLARE @cityId nvarchar(max)=''
DECLARE @testerId as nvarchar(max)=''

DECLARE @Cities TABLE(CityId int)
DECLARE @Clients TABLE(ClientId int)
DECLARE @Testers TABLE(TesterId int)

--SELECT @testerId = COALESCE(@testerId + ', ', '') + CAST(x.UserId AS varchar(15))
--FROM Sec_UserRoles x
--WHERE x.RoleId=13

--SET @testerId+=','

--INSERT INTO @Testers
--SELECT x.UserId
--FROM Sec_UserRoles x
--WHERE x.RoleId=13
--UNION ALL
--SELECT NULL
--UNION ALL
--SELECT 0


IF  (@Panel1Option='Regional View' AND @Panel2Option='Drive Tester View')
BEGIN	
	IF (@Panel1Value=0)
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
		AND PDefinationId=@Panel1Value;

		SET @cityId+=','
	END
END
ELSE IF  (@Panel1Option='Market View' AND @Panel2Option='Drive Tester View')
BEGIN
	IF (@Panel1Value=0)
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
		AND DefinationId=@Panel1Value;

		SET @cityId+=','

		--SELECT @listId
	END
END
ELSE IF  (@Panel1Option='Regional View' AND (@Panel2Option='ClientPOC View'  OR @Panel2Option='SwiView View'))
BEGIN	
	IF (@Panel1Value=0)
	BEGIN
		DECLARE @RegionId1 varchar(max)=''
		SELECT @RegionId1 = COALESCE(@RegionId1 + ', ', '') + CAST(DefinationId AS varchar(15))
		FROM AD_Definations rgn
		WHERE rgn.DefinationTypeId= 6

		SET @RegionId1+=','

		IF (@Panel2Value=0)
		BEGIN
			SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(x.DefinationId AS varchar(15))
			FROM AD_Definations x
			INNER JOIN Sec_UserCities y ON y.CityId=x.DefinationId
			WHERE x.DefinationTypeId= 7
			AND Charindex(cast(x.PDefinationId as varchar(max))+',', @RegionId1) > 0;
		END
		ELSE
		BEGIN
			SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(x.DefinationId AS varchar(15))
			FROM AD_Definations x
			INNER JOIN Sec_UserCities y ON y.CityId=x.DefinationId
			WHERE x.DefinationTypeId= 7 AND y.UserId=@Panel2Value
			AND Charindex(cast(x.PDefinationId as varchar(max))+',', @RegionId1) > 0;
		END
		
		SET @cityId+=','
	END
	ELSE
	BEGIN
		IF (@Panel2Value=0)
		BEGIN
			SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(x.DefinationId AS varchar(15))
			FROM AD_Definations x
			INNER JOIN Sec_UserCities y ON y.CityId=x.DefinationId
			WHERE x.DefinationTypeId= 7
			AND x.PDefinationId=@Panel1Value;	
		END
		ELSE
		BEGIN
			SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(x.DefinationId AS varchar(15))
			FROM AD_Definations x
			INNER JOIN Sec_UserCities y ON y.CityId=x.DefinationId
			WHERE x.DefinationTypeId= 7 AND y.UserId=@Panel2Value
			AND x.PDefinationId=@Panel1Value;		
		END

		SET @cityId+=','
	END
END
ELSE IF  (@Panel1Option='Market View' AND (@Panel2Option='ClientPOC View'  OR @Panel2Option='SwiView View'))
BEGIN
	IF (@Panel1Value=0)
	BEGIN
		IF (@Panel2Value=0)
		BEGIN
			SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(x.DefinationId AS varchar(15))
			FROM AD_Definations x
			INNER JOIN Sec_UserCities y ON y.CityId=x.DefinationId
			WHERE x.DefinationTypeId= 7
		END
		ELSE
		BEGIN
			SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(x.DefinationId AS varchar(15))
			FROM AD_Definations x
			INNER JOIN Sec_UserCities y ON y.CityId=x.DefinationId
			WHERE x.DefinationTypeId= 7	AND y.UserId=@Panel2Value
		END

		SET @cityId+=','
	END
	ELSE
	BEGIN
		IF (@Panel2Value=0)
		BEGIN
			SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(x.DefinationId AS varchar(15))
			FROM AD_Definations x
			INNER JOIN Sec_UserCities y ON y.CityId=x.DefinationId
			WHERE x.DefinationTypeId= 7
			AND x.DefinationId=@Panel1Value
		END
		ELSE
		BEGIN
			SELECT @cityId = COALESCE(@cityId + ', ', '') + CAST(x.DefinationId AS varchar(15))
			FROM AD_Definations x
			INNER JOIN Sec_UserCities y ON y.CityId=x.DefinationId
			WHERE x.DefinationTypeId= 7
			AND x.DefinationId=@Panel1Value AND y.UserId=@Panel2Value;
		END

		SET @cityId+=','

		--SELECT @listId
	END
END
ELSE IF  (@Panel1Option='Client View')
BEGIN
	IF (@Panel1Value=0)
	BEGIN
		SELECT @clientId = COALESCE(@clientId + ', ', '') + CAST(ClientId AS varchar(15))
		FROM AD_Clients

		SET @clientId+=','
	END
	ELSE
	BEGIN
		SELECT @clientId=','+CAST(@Panel1Value as nvarchar(15))+','
	END
END


IF  (@Panel2Option='Drive Tester View')
BEGIN
	SET @testerId=''
	IF (@Panel2Value=0)
	BEGIN
		--SELECT @testerId = COALESCE(@testerId + ', ', '') + CAST(x.UserId AS varchar(15))
		--FROM Sec_UserRoles x
		--WHERE x.RoleId=13

		--SET @testerId+=','
		INSERT INTO @Testers
		SELECT x.UserId
		FROM Sec_UserRoles x INNER JOIN Sec_Users AS su ON x.UserId=su.UserId
		WHERE x.RoleId=13 
		--AND su.IsActive=1
		UNION ALL
		SELECT NULL
		UNION ALL
		SELECT 0
	END
	ELSE
	BEGIN
		INSERT INTO @Testers
		SELECT x.UserId
		FROM Sec_UserRoles x INNER JOIN Sec_Users AS su ON x.UserId=su.UserId
		WHERE x.RoleId=13 --AND su.IsActive=1
		AND x.UserId=@Panel2Value
		
		
		--SELECT * FROM @Testers
		
		--SELECT @testerId=','+CAST(@Panel2Value as nvarchar(15))+','
	END
END
ELSE IF  (@Panel2Option='ClientPOC View' OR @Panel2Option='SwiView View')
BEGIN	
		INSERT INTO @Testers
		SELECT x.UserId
		FROM Sec_UserRoles x
		INNER JOIN Sec_Users AS su ON x.UserId=su.UserId
		--INNER JOIN Sec_UserCities AS suc ON suc.UserId=su.UserId
		WHERE x.RoleId=13 --AND su.IsActive=1
		--AND Charindex(cast(suc.CityId as varchar(max))+',', @cityId) > 0;
		UNION ALL
		SELECT NULL
		UNION ALL
		SELECT 0
END




IF @Panel1Option='Client View'
BEGIN
	INSERT INTO @Cities
	SELECT cty.CityId
	FROM Sec_UserCities cty
	WHERE cty.UserId=@UserID;
	
	INSERT INTO @Clients
	SELECT clt.ClientId
	FROM UserClients clt
	WHERE clt.UserId=@UserID AND Charindex(cast(clt.ClientId as varchar(max))+',', @clientId) > 0;
END
ELSE
BEGIN
	INSERT INTO @Cities
	SELECT cty.CityId
	FROM Sec_UserCities cty
	WHERE cty.UserId=@UserID  AND Charindex(cast(cty.CityId as varchar(max))+',', @cityId) > 0;

	INSERT INTO @Clients
	SELECT clt.ClientId
	FROM UserClients clt
	WHERE clt.UserId=@UserID
	
	PRINT @cityId
END
PRINT @DATE1
PRINT @DATE2

IF @PARENTFILTER = 'Total' --AND @CHILDFILTER='Total'
BEGIN
	--WORK ORDER VIEW
	EXEC [AV_GetDashboardSites]  @CHILDFILTER, @PARENTFILTER, @FROMDATE, @TODATE, @UserID, @Panel1Option, @Panel1Value, @Panel2Option, @Panel2Value, 'Dashboard_Sites',0,5, @CountryId, @Client, @Scopes,@Projects

	
	SELECT SUM(CASE WHEN x.Status= 90 THEN 1 ELSE NULL END) AS PendingSites, 
    SUM(CASE WHEN x.Status = 91 THEN 1 ELSE NULL END) AS InProcessSites ,
	SUM(CASE WHEN x.Status = 89 THEN 1 ELSE NULL END) AS CompletedSites,
	SUM(CASE WHEN x.Status = 92 THEN 1 ELSE 0 END) AS DriveCompletedSites,
	SUM(CASE WHEN x.Status = 93 THEN 1 ELSE 0 END) AS PendingWithIssuesSites,
	SUM(CASE WHEN x.Status = 450 THEN 1 ELSE 0 END) AS InProgress, 
	SUM(CASE WHEN x.Status = 451 THEN 1 ELSE 0 END) AS ReportSubmitted,
	COUNT(x.SiteId) AS TotalSites
	FROM
	(
		SELECT DISTINCT sit.SiteId, sit.[Status]
		FROM
		(	
			SELECT DISTINCT AV_Sites.SiteId,AV_Sites.[Status]--ISNULL(anls.TesterId,0) 'TesterId'	
			FROM AV_Sites
			INNER Join AD_Definations ON AD_Definations.DefinationId = AV_Sites.CityId	
			--INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=av_sites.SiteId
			Where 
			AV_Sites.CityId IN (Select x.CityId FROM @Cities x)
			AND  (CAST(AV_Sites.SubmittedOn AS date) BETWEEN @DATE1 AND @DATE2)
			AND AV_Sites.ClientId IN(Select x.ClientId FROM @Clients x)	
			AND AV_Sites.IsActive=1
			AND AV_Sites.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
			AND (@Projects='0' or CHARINDEX(CAST(AV_Sites.ProjectId as nvarchar(20)),(',' + @Projects + ',')) > 0)
		) sit LEFT OUTER JOIN
		(
			SELECT DISTINCT anls.SiteId, ISNULL(anls.TesterId,0) 'TesterId'
			FROM AV_Sites
			INNER Join AD_Definations ON AD_Definations.DefinationId = AV_Sites.CityId	
			INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=av_sites.SiteId
			WHERE  
			AV_Sites.CityId IN (Select x.CityId FROM @Cities x)
			AND  (CAST(AV_Sites.SubmittedOn AS date) BETWEEN @DATE1 AND @DATE2)
			--AND AV_Sites.ClientId IN(Select x.ClientId FROM @Clients x)	
			AND AV_Sites.IsActive=1 AND anls.IsActive=1
			AND AV_Sites.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
			AND (@Projects='0' or CHARINDEX(CAST(AV_Sites.ProjectId as nvarchar(20)),(',' + @Projects + ',')) > 0)
		) tst ON sit.SiteId=tst.SiteId
		WHERE tst.TesterId IN(SELECT x.TesterId FROM @Testers x) --OR tst.TesterId=0
	) x
	

	--REGION SUMMARY
	Exec AV_SummaryView @PARENTFILTER, @CHILDFILTER, @FROMDATE, @TODATE, @Panel1Option, @Panel1Value , @UserID, @Panel2Option, @Panel2Value, @CountryId, @Client, @Scopes,@Projects

	--Tester SUMMARY
	Exec AV_SummaryView @PARENTFILTER, @CHILDFILTER,@FROMDATE, @TODATE, @Panel2Option,@Panel2Value, @UserID, @Panel1Option, @Panel1Value, @CountryId, @Client, @Scopes,@Projects

	----Drive Tester View SUMMARY
	--Exec AV_SummaryView @PARENTFILTER, @CHILDFILTER,@FROMDATE, @TODATE, @Panel2Option,@Panel2Value, @UserID
END
ELSE
BEGIN 
	--WORK ORDER VIEW
	EXEC [AV_GetDashboardSites]  @CHILDFILTER, @PARENTFILTER, @FROMDATE, @TODATE, @UserID, @Panel1Option, @Panel1Value, @Panel2Option, @Panel2Value, 'Dashboard_Sites', 0,5, @CountryId, @Client, @Scopes,@Projects
	
	--SELECT SUM(CASE WHEN x.Status= 90 THEN 1 ELSE NULL END) AS PendingSites, 
 --   SUM(CASE WHEN x.Status = 91 THEN 1 ELSE NULL END) AS InProcessSites ,
	--SUM(CASE WHEN x.Status = 89 THEN 1 ELSE NULL END) AS CompletedSites,
	--SUM(CASE WHEN x.Status = 92 THEN 1 ELSE 0 END) AS DriveCompletedSites,
	--SUM(CASE WHEN x.Status = 93 THEN 1 ELSE 0 END) AS PendingWithIssuesSites,
	--SUM(CASE WHEN x.Status = 450 THEN 1 ELSE 0 END) AS InProgress, 
	--SUM(CASE WHEN x.Status = 451 THEN 1 ELSE 0 END) AS ReportSubmitted,
	--COUNT(x.SiteId) AS TotalSites
	--FROM
	--(
	--	SELECT DISTINCT AV_Sites.SiteId,AV_Sites.[Status]--ISNULL(anls.TesterId,0) 'TesterId'
	--	FROM AV_Sites
	--	INNER Join AD_Definations ON AD_Definations.DefinationId = AV_Sites.CityId	
	--	--INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=av_sites.SiteId
	--	Where 
	--	AV_Sites.CityId IN (Select x.CityId FROM @Cities x)
	--	AND  (CAST(AV_Sites.SubmittedOn AS date) BETWEEN @DATE1 AND @DATE2)
	--	AND AV_Sites.ClientId IN(Select x.ClientId FROM @Clients x)	
	--	AND AV_Sites.IsActive=1
	--) x
	--WHERE x.TesterId IN(SELECT x.TesterId FROM @Testers x) OR x.TesterId=0
	
	SELECT SUM(CASE WHEN x.Status= 90 THEN 1 ELSE NULL END) AS PendingSites, 
    SUM(CASE WHEN x.Status = 91 THEN 1 ELSE NULL END) AS InProcessSites ,
	SUM(CASE WHEN x.Status = 89 THEN 1 ELSE NULL END) AS CompletedSites,
	SUM(CASE WHEN x.Status = 92 THEN 1 ELSE 0 END) AS DriveCompletedSites,
	SUM(CASE WHEN x.Status = 93 THEN 1 ELSE 0 END) AS PendingWithIssuesSites,
	SUM(CASE WHEN x.Status = 450 THEN 1 ELSE 0 END) AS InProgress, 
	SUM(CASE WHEN x.Status = 451 THEN 1 ELSE 0 END) AS ReportSubmitted,
	COUNT(x.SiteId) AS TotalSites
	FROM
	(
		SELECT DISTINCT sit.SiteId, sit.[Status]
		FROM
		(	
			SELECT DISTINCT AV_Sites.SiteId,AV_Sites.[Status]--ISNULL(anls.TesterId,0) 'TesterId'	
			FROM AV_Sites
			INNER Join AD_Definations ON AD_Definations.DefinationId = AV_Sites.CityId	
			--INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=av_sites.SiteId
			Where 
			AV_Sites.CityId IN (Select x.CityId FROM @Cities x)
			AND  (CAST(AV_Sites.SubmittedOn AS date) BETWEEN @DATE1 AND @DATE2)
			AND AV_Sites.ClientId IN(Select x.ClientId FROM @Clients x)	
			AND AV_Sites.IsActive=1
			AND AV_Sites.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
			AND (@Projects='0' or CHARINDEX(CAST(AV_Sites.ProjectId as nvarchar(20)),(',' + @Projects + ',')) > 0)
		) sit LEFT OUTER JOIN
		(
			SELECT anls.SiteId, ISNULL(anls.TesterId,0) 'TesterId'
			FROM AV_NetLayerStatus AS anls
			WHERE anls.IsActive=1
		) tst ON sit.SiteId=tst.SiteId
		WHERE tst.TesterId IN(SELECT x.TesterId FROM @Testers x) --OR tst.TesterId=0
	) x
	
	--SELECT ISNULL(x.TesterId,0) FROM @Testers x
	--REGION SUMMARY
	Exec AV_SummaryView @PARENTFILTER, @CHILDFILTER, @FROMDATE, @TODATE, @Panel1Option, @Panel1Value , @UserID, @Panel2Option, @Panel2Value, @CountryId, @Client, @Scopes,@Projects

	--Tester SUMMARY
	Exec AV_SummaryView @PARENTFILTER, @CHILDFILTER,@FROMDATE, @TODATE, @Panel2Option,@Panel2Value, @UserID, @Panel1Option, @Panel1Value, @CountryId, @Client, @Scopes,@Projects

	----Drive Tester View SUMMARY
	--Exec AV_SummaryView @PARENTFILTER, @CHILDFILTER,@FROMDATE, @TODATE, @Panel2Option,@Panel2Value, @UserID
END 

--Status color and names
--select x.DefinationId 'StatusId', x.DefinationName 'Status', x.KeyCode, x.ColorCode
--  from AD_Definations x where DefinationTypeId=17

 EXEC [AV_GetDashboardSites]  @CHILDFILTER, @PARENTFILTER, @FROMDATE, @TODATE, @UserID, @Panel1Option, @Panel1Value, @Panel2Option, @Panel2Value, 'Dashboard_Markers',0,5, @CountryId, @Client, @Scopes,@Projects