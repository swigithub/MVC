
CREATE PROCEDURE [dbo].[AV_rptNetLayer]
@DateFilter NVARCHAR(50)=NULL,
	@fromDate DATETIME,
	@toDate DATETIME,
	@woStatus NVARCHAR(50)=null,
	@Panel1Filter NVARCHAR(50)=null,
	@Panel1Value NVARCHAR(50)=null,
	@Panel2Filter NVARCHAR(50)=null,
	@Panel2Value NVARCHAR(50)=null,
	@ReportFilter NVARCHAR(50)='NetLayerStatus',
	@UserId numeric(18,0)=null
AS



DECLARE @DATE1 AS DATETIME
DECLARE @DATE2 AS DATETIME

--IF @DateFilter = 'Today'
--BEGIN
--SET @DATE1 = CAST(GETDATE() AS date)
--SET @DATE2 = CAST(GETDATE() AS date)
--END
--IF @DateFilter = 'ThisWeek'
--BEGIN
--SET @DATE1 = DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
--SET @DATE2 = DATEADD(DAY, 7 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
--END
--IF @DateFilter = 'ThisMonth'
--BEGIN
--SET @DATE1 = CAST(DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) AS date)
--SET @DATE2 = CAST(DATEADD(DAY, -(DAY(DATEADD(MONTH, 1, GETDATE()))),DATEADD(MONTH, 1, GETDATE())) AS date)



--END
--IF @DateFilter = 'Total'
--BEGIN
--SET @DATE1 = CAST(@FROMDATE AS DATE)
--SET @DATE2 = CAST(@TODATE AS DATE)
--END
	IF DATEDIFF(DAY,@FROMDATE,@TODATE)>=0 AND DATEDIFF(DAY,@FROMDATE,@TODATE)<=7
	BEGIN
		SET @DATE1 = CAST(@FROMDATE AS DATE)
		SET @DATE2 = CAST(@TODATE AS DATE)
	END
	ELSE IF DATEDIFF(DAY,@FROMDATE,@TODATE)>7
	BEGIN
		--IF @CHILDFILTER='DateRange'
		--BEGIN
		--	SET @DATE1 = CAST(@FROMDATE AS DATE)
		--	SET @DATE2 = CAST(@TODATE AS DATE)
		--END
		--ELSE 
		--BEGIN
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
		--END
	END
	
	DECLARE @UserScopeId AS NVARCHAR(500)=''
	DECLARE @UserCityId AS NVARCHAR(500)=''
	
	SELECT @UserScopeId = COALESCE(@UserScopeId + ', ', '') + CAST(x.DefinationId AS varchar(15))
	FROM AD_Definations x INNER JOIN Sec_UserScopes AS sus ON x.DefinationId=sus.ScopeId
	WHERE x.DefinationTypeId= 12 AND sus.UserId=@UserId

	SET @UserScopeId+=','
	
	SELECT @UserCityId = COALESCE(@UserCityId + ', ', '') + CAST(x.DefinationId AS varchar(15))
	FROM AD_Definations x
	INNER JOIN Sec_UserCities y ON y.CityId=x.DefinationId
	WHERE x.DefinationTypeId= 7 AND y.UserId=@UserId
	
	SET @UserCityId+=','

--SELECT @DATE1,@DATE2
IF @ReportFilter='NetLayerStatus'
BEGIN
	IF @woStatus='Total'
	BEGIN
	SELECT DISTINCT x.WoRefId 'WO_Ref', x.ClientName 'Client', x.Region 'Region', x.Market 'Market', x.SiteCode 'Site', x.NetworkMode 'Network_Mode',
		   x.Band, x.Carrier, x.Scope, x.ReceivedOn 'Received', x.SubmittedOn 'Submitted', x.AssignedOn 'Scheduled',
		   x.DriveCompletedOn 'Drive_Completed', x.CompletedOn 'Approved',
		   --CASE WHEN x.ReceivedOn IS NULL THEN '' ELSE CAST(x.ReceivedOn AS NVARCHAR(30)) END 'Received',
		   --CASE WHEN x.SubmittedOn IS NULL THEN '' ELSE CAST(x.SubmittedOn AS NVARCHAR(30)) END 'Submitted',
		   --CASE WHEN x.AssignedOn IS NULL THEN '' ELSE CAST(x.AssignedOn AS NVARCHAR(30)) END 'Assigned',
		   --CASE WHEN x.DriveCompletedOn IS NULL THEN '' ELSE CAST(x.DriveCompletedOn AS NVARCHAR(30)) END 'Drive Completed',
		   --CASE WHEN x.CompletedOn IS NULL THEN '' ELSE CAST(x.CompletedOn AS NVARCHAR(30)) END 'Completed',
		   x.[Status],x.[Description],x.ReportSubmittedOn,x.Drive_Tester,x.SiteType,x.Project
	FROM
	(
	SELECT DISTINCT sit.WoRefId, clt.ClientName, rgn.DefinationName 'Region', cty.DefinationName 'Market', sit.SiteCode,
	net.DefinationName 'NetworkMode',bnd.DefinationName 'Band',crr.DefinationName 'Carrier',scp.DefinationName 'Scope',
	sec.ReceivedOn, sec.UploadedOn 'SubmittedOn', sec.AssignedOn,sec.DriveCompletedOn, sec.CompletedOn,
	stt.DefinationName 'Status',sit.[Description],sit.SubmittedOn 'ReportSubmittedOn',
	su.FirstName + ' ' + su.LastName 'Drive_Tester',stp.DefinationName 'SiteType',pm.ProjectName 'Project'
	FROM AV_Sites sit
	INNER JOIN AV_NetLayerStatus sec ON sit.SiteId=sec.SiteId
	INNER JOIN AD_Clients AS clt ON clt.ClientId=sit.ClientId
	INNER JOIN AD_Definations AS cty ON cty.DefinationId=sit.CityId
	INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=cty.PDefinationId
	INNER JOIN AD_Definations AS stt ON stt.DefinationId=sec.[Status]
	INNER JOIN AD_Definations AS net ON net.DefinationId=sec.NetworkModeId
	INNER JOIN AD_Definations AS bnd ON bnd.DefinationId=sec.BandId
	INNER JOIN AD_Definations AS crr ON crr.DefinationId=sec.CarrierId
	INNER JOIN AD_Definations AS scp ON scp.DefinationId=sec.ScopeId
	LEFT OUTER JOIN Sec_Users AS su ON su.UserId=sec.TesterId
	INNER JOIN AD_Definations AS stp ON stp.DefinationId=sit.SiteTypeId
	INNER JOIN PM_Projects AS pm ON sit.ProjectId = pm.ProjectId
	WHERE sit.isActive=1 --AND sit.Status = @woStatus
	AND CAST(LEFT(sit.SubmittedOn,12) AS DATETIME) BETWEEN @DATE1 AND @DATE2
	AND Charindex(cast(sit.ScopeId as varchar(max))+',', @UserScopeId) > 0
	AND Charindex(cast(sit.CityId as varchar(max))+',', @UserCityId) > 0
	) x
	ORDER BY x.SiteCode,x.NetworkMode,x.Band
	END
	ELSE
	BEGIN
		SELECT DISTINCT x.WoRefId 'WO_Ref', x.ClientName 'Client', x.Region 'Region', x.Market 'Market', x.SiteCode 'Site', x.NetworkMode 'Network_Mode',
		   x.Band, x.Carrier, x.Scope, x.ReceivedOn 'Received', x.SubmittedOn 'Submitted', x.AssignedOn 'Scheduled',
		   x.DriveCompletedOn 'Drive_Completed', x.CompletedOn 'Approved',
		   --CASE WHEN x.ReceivedOn IS NULL THEN '' ELSE CAST(x.ReceivedOn AS NVARCHAR(30)) END 'Received',
		   --CASE WHEN x.SubmittedOn IS NULL THEN '' ELSE CAST(x.SubmittedOn AS NVARCHAR(30)) END 'Submitted',
		   --CASE WHEN x.AssignedOn IS NULL THEN '' ELSE CAST(x.AssignedOn AS NVARCHAR(30)) END 'Assigned',
		   --CASE WHEN x.DriveCompletedOn IS NULL THEN '' ELSE CAST(x.DriveCompletedOn AS NVARCHAR(30)) END 'Drive Completed',
		   --CASE WHEN x.CompletedOn IS NULL THEN '' ELSE CAST(x.CompletedOn AS NVARCHAR(30)) END 'Completed',
		   x.[Status],x.[Description],x.ReportSubmittedOn,x.Drive_Tester,x.SiteType,x.Project
	FROM
	(
	SELECT DISTINCT sit.WoRefId, clt.ClientName, rgn.DefinationName 'Region', cty.DefinationName 'Market', sit.SiteCode,
	net.DefinationName 'NetworkMode',bnd.DefinationName 'Band',crr.DefinationName 'Carrier',scp.DefinationName 'Scope',
	sec.ReceivedOn, sec.UploadedOn 'SubmittedOn', sec.AssignedOn,sec.DriveCompletedOn, sec.CompletedOn,
	stt.DefinationName 'Status',sit.[Description],sit.SubmittedOn 'ReportSubmittedOn',
	su.FirstName + ' ' + su.LastName 'Drive_Tester', stp.DefinationName 'SiteType',pm.ProjectName 'Project'
	FROM AV_Sites sit
	INNER JOIN AV_NetLayerStatus sec ON sit.SiteId=sec.SiteId
	INNER JOIN AD_Clients AS clt ON clt.ClientId=sit.ClientId
	INNER JOIN AD_Definations AS cty ON cty.DefinationId=sit.CityId
	INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=cty.PDefinationId
	INNER JOIN AD_Definations AS stt ON stt.DefinationId=sec.[Status]
	INNER JOIN AD_Definations AS net ON net.DefinationId=sec.NetworkModeId
	INNER JOIN AD_Definations AS bnd ON bnd.DefinationId=sec.BandId
	INNER JOIN AD_Definations AS crr ON crr.DefinationId=sec.CarrierId
	INNER JOIN AD_Definations AS scp ON scp.DefinationId=sec.ScopeId
	INNER JOIN AD_Definations AS stp ON stp.DefinationId=sit.SiteTypeId
	INNER JOIN PM_Projects AS pm ON sit.ProjectId = pm.ProjectId
	LEFT OUTER JOIN Sec_Users AS su ON su.UserId=sec.TesterId
	WHERE sit.isActive=1 AND sit.Status = @woStatus
	AND CAST(LEFT(sit.SubmittedOn,12) AS DATETIME) BETWEEN @DATE1 AND @DATE2
	AND Charindex(cast(sit.ScopeId as varchar(max))+',', @UserScopeId) > 0
	AND Charindex(cast(sit.CityId as varchar(max))+',', @UserCityId) > 0
	) x
	ORDER BY x.SiteCode,x.NetworkMode,x.Band,x.Scope
	END
END
ELSE IF @ReportFilter='WorkOrderStatus'
BEGIN
	IF @woStatus='Total'
	BEGIN
		SELECT x.WO_Ref, x.Client, x.Region, x.Market, x.[Site], x.Drive_Tester,
       x.Received, x.Submitted, x.Scheduled, x.Drive_Completed, x.Approved,
       x.Network_Layers, x.[Status], x.[Description], x.LayerCount, x.SectorCount,
       x.ReportSubmittedOn, x.DT_Minutes,SUM(x.[Distance_from_Site]) 'Distance_from_Site',x.Project,x.Scope,x.CheckList,
       CASE WHEN x.Drive_Completed IS NOT NULL AND (DATEPART(HOUR,x.Drive_Completed) BETWEEN 0 AND 6) THEN DATEADD(DAY,-1,x.Drive_Completed) ELSE x.Drive_Completed END 'Site_Completed',       
       CASE WHEN x.DT_First_Test IS NOT NULL AND (DATEPART(HOUR,x.DT_First_Test) BETWEEN 0 AND 6) THEN DATEADD(DAY,-1,x.DT_First_Test) ELSE x.DT_First_Test END 'Site_First_Test'
		FROM
		(
				SELECT DISTINCT x.WoRefId 'WO_Ref', x.ClientName 'Client', x.Region 'Region', x.Market 'Market', x.SiteCode 'Site', x.Drive_Tester,
				x.ReceivedOn 'Received', x.SubmittedOn 'Submitted', x.AssignedOn 'Scheduled',
				x.DriveCompletedOn 'Drive_Completed', x.CompletedOn 'Approved',
				x.Network_Layers,x.[Status],x.[Description], x.ProjectName 'Project',x.DefinationName 'Scope',x.CheckList,
				(SELECT COUNT(DISTINCT CAST(y.NetworkModeId AS NVARCHAR(15))+'-'+CAST(y.BandId AS NVARCHAR(15))+'-'+CAST(y.CarrierId AS NVARCHAR(15))) FROM AV_Sectors y WHERE y.SiteId=x.SiteId) 'LayerCount',
				(SELECT CASE WHEN r.sCount>=6 THEN 6 ELSE r.sCount END
				FROM
				(
					SELECT TOP 1 y.siteId,y.NetworkModeId, y.ScopeId, y.BandId, y.CarrierId,COUNT(y.SectorId) 'sCount'
					FROM AV_Sectors y
					WHERE y.SiteId=x.SiteId
					GROUP BY  y.siteId,y.NetworkModeId, y.ScopeId, y.BandId, y.CarrierId
					ORDER BY COUNT(y.SectorId) DESC
				) r) 'SectorCount',x.ReportSubmittedOn,x.DT_Minutes,x.[Distance_from_Site] 'Distance_from_Site', x.DT_First_Test,x.SiteType
				FROM
				(
				SELECT DISTINCT sit.SiteId,sit.WoRefId, clt.ClientName, rgn.DefinationName 'Region', cty.DefinationName 'Market', sit.SiteCode,sit.ScopeId,pm.ProjectName,scp.DefinationName,
				substring(
				(
					Select DISTINCT ', '+tsd.SurveyTitle AS [text()]
					From dbo.TSS_SurveyDocuments AS tsd
					INNER JOIN AV_NetLayerStatus AS avnls ON avnls.SiteId = sit.SiteId
					INNER JOIN TSS_SiteSurvey AS tss ON tss.SiteSurveyId = avnls.SiteSurveyId
					AND tss.SurveyId = tsd.SurveyId
					For XML PATH ('')
				), 2, 1000) 'CheckList',
				substring(
				(
					Select DISTINCT ', '+usr.FirstName + ' ' + usr.LastName AS [text()]
					From Sec_Users usr
					INNER JOIN AV_NetLayerStatus nls ON nls.SiteId=sit.SiteId AND nls.TesterId=usr.UserId            
					For XML PATH ('')
				), 2, 1000) 'Drive_Tester',		
				sit.ReceivedOn, sit.SubmittedOn, sit.AssignedOn,sit.DriveCompletedOn, sit.CompletedOn,
				stt.DefinationName 'Status',sit.[Description],
				substring(
				(
					Select DISTINCT ', '+bnd.DefinationName + ' ('+crr.DefinationName+')' AS [text()]
					From dbo.AV_NetLayerStatus AS sec
					INNER JOIN AD_Definations AS net ON net.DefinationId=sec.NetworkModeId
					INNER JOIN AD_Definations AS bnd ON bnd.DefinationId=sec.BandId
					INNER JOIN AD_Definations AS crr ON crr.DefinationId=sec.CarrierId
					Where sec.SiteId = sit.SiteId
					For XML PATH ('')
				), 2, 1000) 'Network_Layers',sit.ReportSubmittedOn 'ReportSubmittedOn',
				--CASE WHEN sit.[Status]<>91 AND (sit.Latitude IS NOT NULL AND sit.Longitude IS NOT NULL AND su.homeLatitude IS NOT NULL AND su.homeLongitude IS NOT NULL) THEN CAST(ISNULL(dbo.fnCalcDistanceMiles(sit.Latitude,sit.Longitude,ISNULL(su.homeLatitude,0),ISNULL(su.homeLongitude,0)),0) AS FLOAT) ELSE 0 END 'Distance_from_Site',
				CASE WHEN (sit.Latitude IS NOT NULL AND sit.Longitude IS NOT NULL AND mc.Latitude IS NOT NULL AND mc.Longitude IS NOT NULL) THEN CAST(ISNULL(dbo.fnCalcDistanceMiles(sit.Latitude,sit.Longitude,ISNULL(mc.Latitude,0),ISNULL(mc.Longitude,0)),0) AS FLOAT) ELSE 0 END 'Distance_from_Site',
				 CAST(ISNULL(
        				DATEDIFF(MINUTE,
						(SELECT MIN(awt.TrackerTimestamp) FROM AV_WoTracker AS awt WHERE awt.SiteId=sit.SiteId),
						(SELECT MAX(awt.TrackerTimestamp) FROM AV_WoTracker AS awt WHERE awt.SiteId=sit.SiteId)
        			),1) AS FLOAT) 'DT_Minutes',
        			(SELECT MIN(awt.TrackerTimestamp) FROM AV_WoTracker AS awt WHERE awt.SiteId=sit.SiteId) 'DT_First_Test',stp.DefinationName 'SiteType'
				--CAST('POINT('+CAST(sit.Latitude AS NVARCHAR(20))+' '+ CAST(sit.Longitude AS NVARCHAR(20))+')' AS geography).STDistance(CAST('POINT('+CAST(su.homeLatitude AS NVARCHAR(20))+' '+ CAST(su.homeLongitude AS NVARCHAR(20))+')' AS geography))/1609.34  'Distance_from_Site(mi)'
				FROM AV_Sites sit
				INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=sit.SiteId
				INNER JOIN AD_Clients AS clt ON clt.ClientId=sit.ClientId
				INNER JOIN AD_Definations AS cty ON cty.DefinationId=sit.CityId
				INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=cty.PDefinationId
				INNER JOIN AD_Definations AS stt ON stt.DefinationId=sit.[Status]
				LEFT OUTER JOIN Sec_Users AS su ON su.UserId=anls.TesterId
				LEFT OUTER JOIN AD_MarketCenter AS mc ON mc.MarketId=sit.CityId
				INNER JOIN AD_Definations AS stp ON stp.DefinationId=sit.SiteTypeId
				INNER JOIN PM_Projects AS pm ON sit.ProjectId = pm.ProjectId
				INNER JOIN AD_Definations AS scp ON scp.DefinationId=anls.ScopeId
				WHERE sit.isActive=1
				--AND sit.SiteCode='A2E0520A'
				AND CAST(LEFT(sit.SubmittedOn,12) AS DATETIME) BETWEEN @DATE1 AND @DATE2
				AND Charindex(cast(sit.ScopeId as varchar(max))+',', @UserScopeId) > 0
				AND Charindex(cast(sit.CityId as varchar(max))+',', @UserCityId) > 0
				) x
		) x
		GROUP BY  x.WO_Ref, x.Client, x.Region, x.Market, x.Site, x.Drive_Tester,
				x.Received, x.Submitted, x.Scheduled,
				x.Drive_Completed, x.Approved,
				x.Network_Layers,x.[Status],x.[Description],
				x.LayerCount,x.SectorCount,x.ReportSubmittedOn,x.DT_Minutes,x.DT_First_Test,x.Scope,x.Project,x.CheckList
		ORDER BY x.Site
	END
	ELSE
	BEGIN
		SELECT x.WO_Ref, x.Client, x.Region, x.Market, x.[Site], x.Drive_Tester,
			   x.Received, x.Submitted, x.Scheduled, x.Drive_Completed, x.Approved,
			   x.Network_Layers, x.[Status], x.[Description], x.LayerCount, x.SectorCount,
			   x.ReportSubmittedOn, x.DT_Minutes,SUM(x.[Distance_from_Site]) 'Distance_from_Site',x.Scope,x.Project,x.CheckList
		FROM
		(
				SELECT DISTINCT x.WoRefId 'WO_Ref', x.ClientName 'Client', x.Region 'Region', x.Market 'Market', x.SiteCode 'Site', x.Drive_Tester,
				x.ReceivedOn 'Received', x.SubmittedOn 'Submitted', x.AssignedOn 'Scheduled',
				x.DriveCompletedOn 'Drive_Completed', x.CompletedOn 'Approved',
				x.Network_Layers,x.[Status],x.[Description],x.DefinationName 'Scope',x.ProjectName 'Project',x.CheckList,
				(SELECT COUNT(DISTINCT CAST(y.NetworkModeId AS NVARCHAR(15))+'-'+CAST(y.BandId AS NVARCHAR(15))+'-'+CAST(y.CarrierId AS NVARCHAR(15))) FROM AV_Sectors y WHERE y.SiteId=x.SiteId) 'LayerCount',
				(SELECT CASE WHEN r.sCount>=6 THEN 6 ELSE r.sCount END
				FROM
				(
					SELECT TOP 1 y.siteId,y.NetworkModeId, y.ScopeId, y.BandId, y.CarrierId,COUNT(y.SectorId) 'sCount'
					FROM AV_Sectors y
					WHERE y.SiteId=x.SiteId
					GROUP BY  y.siteId,y.NetworkModeId, y.ScopeId, y.BandId, y.CarrierId
					ORDER BY COUNT(y.SectorId) DESC
				) r) 'SectorCount',x.ReportSubmittedOn,x.DT_Minutes,x.[Distance_from_Site] 'Distance_from_Site',x.SiteType
				FROM
				(
				SELECT DISTINCT sit.SiteId,sit.WoRefId, clt.ClientName, rgn.DefinationName 'Region', cty.DefinationName 'Market', sit.SiteCode,pm.ProjectName,scp.DefinationName,
				substring(
				(
					Select DISTINCT ', '+tsd.SurveyTitle AS [text()]
					From dbo.TSS_SurveyDocuments AS tsd
					INNER JOIN AV_NetLayerStatus AS avnls ON avnls.SiteId = sit.SiteId
					INNER JOIN TSS_SiteSurvey AS tss ON tss.SiteSurveyId = avnls.SiteSurveyId
					AND tss.SurveyId = tsd.SurveyId
					For XML PATH ('')
				), 2, 1000) 'CheckList',
				substring(
				(
					Select DISTINCT ', '+usr.FirstName + ' ' + usr.LastName AS [text()]
					From Sec_Users usr
					INNER JOIN AV_NetLayerStatus nls ON nls.SiteId=sit.SiteId AND nls.TesterId=usr.UserId            
					For XML PATH ('')
				), 2, 1000) 'Drive_Tester',		
				sit.ReceivedOn, sit.SubmittedOn, sit.AssignedOn,sit.DriveCompletedOn, sit.CompletedOn,
				stt.DefinationName 'Status',sit.[Description],
				substring(
				(
					Select DISTINCT ', '+bnd.DefinationName + ' ('+crr.DefinationName+')' AS [text()]
					From dbo.AV_NetLayerStatus AS sec
					INNER JOIN AD_Definations AS net ON net.DefinationId=sec.NetworkModeId
					INNER JOIN AD_Definations AS bnd ON bnd.DefinationId=sec.BandId
					INNER JOIN AD_Definations AS crr ON crr.DefinationId=sec.CarrierId
					Where sec.SiteId = sit.SiteId
					For XML PATH ('')
				), 2, 1000) 'Network_Layers',sit.ReportSubmittedOn 'ReportSubmittedOn',
				--CASE WHEN sit.[Status]<>91 AND (sit.Latitude IS NOT NULL AND sit.Longitude IS NOT NULL AND su.homeLatitude IS NOT NULL AND su.homeLongitude IS NOT NULL) THEN CAST(ISNULL(dbo.fnCalcDistanceMiles(sit.Latitude,sit.Longitude,ISNULL(su.homeLatitude,0),ISNULL(su.homeLongitude,0)),0) AS FLOAT) ELSE 0 END 'Distance_from_Site',
				CASE WHEN (sit.Latitude IS NOT NULL AND sit.Longitude IS NOT NULL AND mc.Latitude IS NOT NULL AND mc.Longitude IS NOT NULL) THEN CAST(ISNULL(dbo.fnCalcDistanceMiles(sit.Latitude,sit.Longitude,ISNULL(mc.Latitude,0),ISNULL(mc.Longitude,0)),0) AS FLOAT) ELSE 0 END 'Distance_from_Site',
				 CAST(ISNULL(
        				DATEDIFF(MINUTE,
						(SELECT MIN(awt.TrackerTimestamp) FROM AV_WoTracker AS awt WHERE awt.SiteId=sit.SiteId),
						(SELECT MAX(awt.TrackerTimestamp) FROM AV_WoTracker AS awt WHERE awt.SiteId=sit.SiteId)
        			),1) AS FLOAT) 'DT_Minutes',stp.DefinationName 'SiteType'
				--CAST('POINT('+CAST(sit.Latitude AS NVARCHAR(20))+' '+ CAST(sit.Longitude AS NVARCHAR(20))+')' AS geography).STDistance(CAST('POINT('+CAST(su.homeLatitude AS NVARCHAR(20))+' '+ CAST(su.homeLongitude AS NVARCHAR(20))+')' AS geography))/1609.34  'Distance_from_Site(mi)'
				FROM AV_Sites sit
				INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=sit.SiteId
				INNER JOIN AD_Clients AS clt ON clt.ClientId=sit.ClientId
				INNER JOIN AD_Definations AS cty ON cty.DefinationId=sit.CityId
				INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=cty.PDefinationId
				INNER JOIN AD_Definations AS stt ON stt.DefinationId=sit.[Status]
				LEFT OUTER JOIN Sec_Users AS su ON su.UserId=anls.TesterId
				LEFT OUTER JOIN AD_MarketCenter AS mc ON mc.MarketId=sit.CityId
				INNER JOIN AD_Definations AS stp ON stp.DefinationId=sit.SiteTypeId
				INNER JOIN PM_Projects AS pm ON sit.ProjectId = pm.ProjectId
				INNER JOIN AD_Definations AS scp ON scp.DefinationId=anls.ScopeId
				WHERE sit.isActive=1
				--AND sit.SiteCode='A2E0520A'
				AND CAST(LEFT(sit.SubmittedOn,12) AS DATETIME) BETWEEN @DATE1 AND @DATE2
				AND Charindex(cast(sit.ScopeId as varchar(max))+',', @UserScopeId) > 0
				AND Charindex(cast(sit.CityId as varchar(max))+',', @UserCityId) > 0
				AND anls.[Status]=@woStatus
				) x
		) x
		GROUP BY  x.WO_Ref, x.Client, x.Region, x.Market, x.Site, x.Drive_Tester,
				x.Received, x.Submitted, x.Scheduled,
				x.Drive_Completed, x.Approved,
				x.Network_Layers,x.[Status],x.[Description],
				x.LayerCount,x.SectorCount,x.ReportSubmittedOn,x.DT_Minutes,x.Scope,x.Project,x.CheckList
		ORDER BY x.Site
	END
END