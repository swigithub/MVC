CREATE PROCEDURE AV_GetSitesToSchedule
	@CHILDFILTER varchar(max)
	,@PARENTFILTER VARCHAR(MAX)
	,@FROMDATE DATETIME
	,@TODATE DATETIME	
	,@UserID as int
	,@Panel1Option nvarchar(100)
	,@Panel1Value numeric(18,0) 
	,@Panel2Option nvarchar(100)
	,@Panel2Value numeric(18,0)
	,@CountryId varchar(50)=null
	,@Client varchar(50)=null
	,@Scopes varchar(50) =N'0'
	,@Markets varchar(50) =null
AS
BEGIN
	
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
		WHERE x.RoleId=13 and su.homeLatitude<>0 and su.homeLongitude<>0
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
		WHERE x.RoleId=13 and su.homeLatitude<>0 and su.homeLongitude<>0 --AND su.IsActive=1 
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
		WHERE x.RoleId=13 and su.homeLatitude<>0 and su.homeLongitude<>0--AND su.IsActive=1
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

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select DISTINCT Cast(WoRefId As nvarchar(100)) + '-' + Cast(WoCode as nvarchar(100)) As WoRefNo, AV_Sites.SiteId, AV_Sites.SiteCode, AV_Sites.Latitude, AV_Sites.Longitude,
	AV_Sites.ClusterId, AV_Sites.ClientId, AV_Sites.Description, 
	--anls.Status, anls.SubmittedOn, anls.AssignedOn, anls.ScheduledOn, anls.CompletedOn,anls.ScheduledOn,AV_Sites.ReceivedOn,
	AV_Sites.Status, AV_Sites.SubmittedOn 'SubmittedOn', AV_Sites.ScheduledOn 'AssignedOn', AV_Sites.ScheduledOn, AV_Sites.CompletedOn,AV_Sites.ScheduledOn, AV_Sites.ReceivedOn,
	AD_Definations.DefinationName AS Market, DEF.DefinationName AS Region, users.FirstName + ' ' + users.LastName AS Tester, users.Picture TesterPicture,AD_Clients.ClientName,users.UserId 'TesterId',
	CAST(0 AS BIT) 'IsDownloaded',
	sts.DefinationName 'StatusName',AV_Sites.DriveCompletedOn,sts.KeyCode 'StatusKeyCode'
	 from AV_Sites
	LEFT JOIN AV_Clusters ON AV_Clusters.ClusterId = AV_Sites.ClusterId
	LEFT JOIN AD_Clients ON AD_Clients.ClientId = AV_Sites.ClientId
	LEFT JOIN AD_Definations ON AD_Definations.DefinationId = AV_Clusters.CityId
	LEFT JOIN AD_Definations As DEF ON DEF.DefinationId = AD_Definations.PDefinationId
	INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=AV_Sites.SiteId
	LEFT Join Sec_Users users ON users.UserId = anls.TesterId
	LEFT JOIN Sec_UserScopes AS sus1 ON sus1.UserId=users.UserId
	LEFT JOIN AD_Definations AS sts ON sts.DefinationId=AV_Sites.[Status]
	WHERE
	AV_Sites.CityId IN (Select x.CityId FROM @Cities x)
	AND  (CAST(AV_Sites.SubmittedOn AS date) BETWEEN @DATE1 AND @DATE2)
	AND AV_Sites.ClientId IN(Select x.ClientId FROM @Clients x) 
	--AND anls.TesterId IN(SELECT x.TesterId FROM @Testers x)	 
	AND anls.Status IN ('90','91','92','93','450')	
	AND AV_Sites.IsActive=1
	AND AV_Sites.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
	UNION ALL
	SELECT '' WoRefNo,   0 SiteId, '' SiteCode, su.homeLatitude 'Latitude', su.homeLongitude 'Longitude', 0 'ClusterId', 0 'ClientId', '' 'Description', 0 'Status', NULL 'SubmittedOn',
	NULL 'AssignedOn', NULL 'ScheduledOn', NULL 'CompletedOn',NULL 'ScheduledOn', NULL 'ReceivedOn',
	'' AS Market, '' AS Region,su.FirstName + ' ' + su.LastName AS Tester, 
	--REPLACE(su.Picture,'thumb','home') TesterPicture,
	su.Picture TesterPicture,
	'' 'ClientName',su.UserId 'TesterId',
	CAST(0 AS BIT) 'IsDownloaded','N/A' 'StatusName',NULL 'DriveCompletedOn', 'N/A' 'StatusKeyCode'
	FROM Sec_Users AS su INNER JOIN Sec_UserRoles AS sur ON su.UserId=sur.UserId
	INNER JOIN Sec_UserScopes AS sus2 ON sus2.UserId=su.UserId
	WHERE sur.RoleId=13 AND su.homeLatitude IS NOT NULL AND su.IsActive=1 and su.homeLatitude<>0 and su.homeLongitude<>0
	AND sus2.ScopeId IN(SELECT sus.ScopeId FROM Sec_UserScopes AS sus WHERE sus.UserId=@UserID AND Charindex(cast(sus.ScopeId as varchar(max))+',', @Scopes)>0)
	--Order By anls.SubmittedOn ASC 
END