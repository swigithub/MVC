
CREATE PROCEDURE [dbo].[AV_Filter] 
	-- Add the parameters for the stored procedure here
	@FROMDATE DATETIME='2015-01-01'
,@TODATE DATETIME='2017-12-01'
,@FilterOption as nvarchar(50)='Tester'
,@UserID as int=11
AS
BEGIN
SELECT cty.CityId
INTO #Cities
FROM Sec_UserCities cty
WHERE cty.UserId=@UserID;

SELECT clt.ClientId
INTO #Clients
FROM UserClients clt
WHERE clt.UserId=@UserID;


IF @FilterOption='Region'
BEGIN
	--REGIONAL VIEW
	SELECT rgn.RegionId, rgn.Region, ISNULL(ste.RegionCompletedSites,0) RegionCompletedSites, ISNULL(ste.RegionInProcessSites,0) RegionInProcessSites, ISNULL(ste.RegionPendingSites,0) RegionPendingSites
	, ISNULL(ste.RegionTotalSites,0) RegionTotalSites
	FROM 
	(
		SELECT rgn.DefinationId 'RegionId',cty.DefinationId 'CityId',cty.DefinationName + ' ' + rgn.DefinationName 'Region'
		FROM AD_Definations rgn
		INNER JOIN AD_Definations cty on rgn.DefinationId=cty.PDefinationId
		WHERE rgn.DefinationTypeId=6
		AND cty.DefinationId IN(Select x.CityId FROM #Cities x)
	) rgn LEFT OUTER JOIN
	(
		SELECT  rgn.DefinationId 'RegionId',cty.DefinationId 'CityId', cty.DefinationName + ' ' + rgn.DefinationName 'Region',
			SUM(CASE WHEN Status = 'Completed' THEN 1 ELSE 0 END) AS RegionCompletedSites,
			SUM(CASE WHEN Status = 'InProcess' THEN 1 ELSE 0 END) AS RegionInProcessSites ,
			SUM(CASE WHEN Status='Pending' THEN 1 ELSE 0 END) AS RegionPendingSites, 
			COUNT(*) AS RegionTotalSites 
		FROM AD_Definations rgn
		INNER JOIN AD_Definations cty on rgn.DefinationId=cty.PDefinationId
		INNER JOIN AV_Clusters cls ON cls.CityId = cty.DefinationId
		INNER JOIN AV_Sites ste ON ste.ClusterId = cls.ClusterId
		WHERE rgn.DefinationTypeId=6
		AND cty.DefinationId IN(Select x.CityId FROM #Cities x)
		AND (CAST(ste.SubmittedOn AS date) BETWEEN @FROMDATE AND @TODATE)
		GROUP BY cty.DefinationId,cty.DefinationName, rgn.DefinationId, rgn.DefinationName
	) ste ON rgn.RegionID=ste.RegionID AND rgn.CityId=ste.CityId
END
ELSE IF @FilterOption='Market'
BEGIN
	--MARKET VIEW
	SELECT rgn.RegionId, rgn.Region, ISNULL(ste.RegionCompletedSites,0) RegionCompletedSites, ISNULL(ste.RegionInProcessSites,0) RegionInProcessSites, ISNULL(ste.RegionPendingSites,0) RegionPendingSites
	, ISNULL(ste.RegionTotalSites,0) RegionTotalSites
	FROM 
	(
		SELECT cty.DefinationId 'RegionId',cty.DefinationName 'Region'
		FROM AD_Definations cty
		WHERE cty.DefinationTypeId=7
		AND cty.DefinationId IN(Select x.CityId FROM #Cities x)
	) rgn LEFT OUTER JOIN
	(
		SELECT  cty.DefinationId 'RegionId', cty.DefinationName 'Region',
			SUM(CASE WHEN Status = 'Completed' THEN 1 ELSE 0 END) AS RegionCompletedSites,
			SUM(CASE WHEN Status = 'InProcess' THEN 1 ELSE 0 END) AS RegionInProcessSites ,
			SUM(CASE WHEN Status='Pending' THEN 1 ELSE 0 END) AS RegionPendingSites, 
			COUNT(*) AS RegionTotalSites 
		FROM AD_Definations cty
		INNER JOIN AV_Clusters cls ON cls.CityId = cty.DefinationId
		INNER JOIN AV_Sites ste ON ste.ClusterId = cls.ClusterId
		WHERE cty.DefinationTypeId=7
		AND cty.DefinationId IN(Select x.CityId FROM #Cities x)
		AND (CAST(ste.SubmittedOn AS date) BETWEEN @FROMDATE AND @TODATE)
		GROUP BY cty.DefinationId,cty.DefinationName
	) ste ON rgn.RegionId=ste.RegionId
END
ELSE IF @FilterOption='Client'
BEGIN
--CLIENT VIEW
SELECT x.RegionId, x.Region, ISNULL(y.RegionCompletedSites,0) RegionCompletedSites, ISNULL(y.RegionInProcessSites,0) RegionInProcessSites, ISNULL(y.RegionPendingSites,0) RegionPendingSites
	, ISNULL(y.RegionTotalSites,0) RegionTotalSites
FROM
(
	SELECT clt.ClientId 'RegionId', clt.ClientName 'Region'
	FROM AD_Clients clt
	WHERE clt.IsActive=1
	AND clt.ClientId IN(Select x.ClientId FROM #Clients x)
) x LEFT OUTER JOIN
(
	SELECT  clt.ClientId 'RegionId', clt.ClientName 'Region',
			SUM(CASE WHEN Status = 'Completed' THEN 1 ELSE 0 END) AS RegionCompletedSites,
			SUM(CASE WHEN Status = 'InProcess' THEN 1 ELSE 0 END) AS RegionInProcessSites ,
			SUM(CASE WHEN Status='Pending' THEN 1 ELSE 0 END) AS RegionPendingSites, 
			COUNT(*) AS RegionTotalSites 
		FROM AD_Clients clt		
		INNER JOIN AV_Sites ste ON ste.ClientId = clt.ClientId
		WHERE (CAST(ste.SubmittedOn AS date) BETWEEN @FROMDATE AND @TODATE)
		AND clt.ClientId IN(Select x.ClientId FROM #Clients x)
		GROUP BY clt.ClientId, clt.ClientName
) y ON x.RegionId=y.RegionId
END

IF @FilterOption='Tester'
BEGIN
	--DRIVE TESTER VIEW
	select u.UserId 'TesterId', (u.FirstName + ' ' +u.LastName) AS TesterName, u.Picture, ISNULL(Temp.TesterCompletedSites,0) TesterCompletedSites,
	ISNULL(temp.TesterInProcessSites,0) TesterInProcessSites, ISNULL(temp.TesterPendingSites,0) TesterPendingSites, ISNULL(temp.TesterTotalSites,0) TesterTotalSites
	FROM Sec_Users u 
	LEFT join Sec_UserRoles ur on u.UserId=ur.UserId
	left outer join
	( select usr.UserId 'TesterId',
	SUM(CASE WHEN st.Status='Pending' THEN 1 ELSE 0 END) AS TesterPendingSites, 
	SUM(CASE WHEN st.Status = 'InProcess' THEN 1 ELSE 0 END) AS TesterInProcessSites ,
	SUM(CASE WHEN st.Status = 'Completed' THEN 1 ELSE 0 END) AS TesterCompletedSites,
	COUNT(st.SiteId) AS TesterTotalSites 
	from Sec_Users usr
	LEFT join Sec_UserRoles ur on usr.UserId=ur.UserId
	left outer join AV_Sites st ON st.TesterId=usr.UserId
	LEFT JOIN AV_Clusters cls ON cls.ClusterId = st.ClusterId
	where UR.RoleId=13 AND (CAST(ST.SubmittedOn AS date) BETWEEN @FROMDATE AND @TODATE)
	AND cls.CityId IN(Select x.CityId FROM #Cities x)
	group by usr.UserId) TEMP 
	ON Temp.TesterId = u.UserId
	Where ur.RoleId = 13 
END
ELSE IF @FilterOption='ClientPOC'
BEGIN
	SELECT usr.UserId
	INTO #ClientUsers
	FROM Sec_Users usr
	INNER JOIN AD_ClientContacts cnt ON cnt.UserId=usr.UserId
	WHERE cnt.ClientId=(SELECT x.ClientId FROM AD_ClientContacts x WHERE x.UserId=@UserID)

	--DRIVE TESTER VIEW
	select u.UserId 'TesterId', (u.FirstName + ' ' +u.LastName) AS TesterName, u.Picture, ISNULL(Temp.TesterCompletedSites,0) TesterCompletedSites,
	ISNULL(temp.TesterInProcessSites,0) TesterInProcessSites, ISNULL(temp.TesterPendingSites,0) TesterPendingSites, ISNULL(temp.TesterTotalSites,0) TesterTotalSites
	FROM Sec_Users u 
	LEFT join Sec_UserRoles ur on u.UserId=ur.UserId
	left outer join
	( select usr.UserId 'TesterId',
	SUM(CASE WHEN st.Status='Pending' THEN 1 ELSE 0 END) AS TesterPendingSites, 
	SUM(CASE WHEN st.Status = 'InProcess' THEN 1 ELSE 0 END) AS TesterInProcessSites ,
	SUM(CASE WHEN st.Status = 'Completed' THEN 1 ELSE 0 END) AS TesterCompletedSites,
	COUNT(st.SiteId) AS TesterTotalSites 
	from Sec_Users usr
	LEFT join Sec_UserRoles ur on usr.UserId=ur.UserId
	left outer join AV_Sites st ON st.TesterId=usr.UserId
	LEFT JOIN AV_Clusters cls ON cls.ClusterId = st.ClusterId
	where UR.RoleId=4 AND (CAST(ST.SubmittedOn AS date) BETWEEN @FROMDATE AND @TODATE)
	AND cls.CityId IN(Select x.CityId FROM #Cities x)
	AND usr.UserId IN(Select x.UserId FROM #ClientUsers x)
	group by usr.UserId) TEMP 
	ON Temp.TesterId = u.UserId
	Where ur.RoleId = 4 

	DROP TABLE #ClientUsers
END

DROP TABLE #Cities
DROP TABLE #Clients
END