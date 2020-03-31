


-- [dbo].[AV_GetNetLayerReport] 648660,74,3163,15,11,'2018-04-01'
CREATE PROCEDURE [dbo].[AV_GetNetLayerReport]
	@SiteId int,
	@BandId int,
	@Carrier nvarchar(50),
	@NetworkMode nvarchar(50),
	@UserId NUMERIC(18,0),
	@ReportDate DATE
AS
BEGIN
	
	 EXEC [dbo].[AV_GetNetLayerSummary] @SiteId,@BandId,@Carrier,@NetworkMode,@UserId
	
	--Select DISTINCT * from AV_SiteTestLog
	--Where SiteId = @SiteId and BandId=@BandId and ActualCarrier=@Carrier and SubNetworkMode=@NetworkMode
	--AND TestStatus=1 AND TestType IN('CW','CCW')
	--order by TIMESTAMP
	
	DECLARe @SiteCode AS NVARCHAR(50)=''
	DECLARe @BandCode AS NVARCHAR(50)=''
	DECLARE @UploadDate AS DATETIME
	DECLARE @ScopeId as numeric=0
		
	SELECT @SiteCode=as1.SiteCode, @UploadDate=as1.SubmittedOn,@ScopeId=as1.ScopeId
		FROM AV_Sites AS as1
	WHERE as1.SiteId=@SiteId
	
	DECLARE @Scope AS NVARCHAR(50)= (SELECT ad.DefinationName FROM AV_Sites AS x INNER JOIN AD_Definations AS ad ON x.ScopeId=ad.DefinationId WHERE x.SiteId=@SiteId)
	
	SELECT @BandCode=ad.DefinationName
	FROM AD_Definations AS ad
	Where ad.DefinationId=@BandId
		
	
	IF CAST(@UploadDate AS DATE)<CAST(@ReportDate AS DATE)
	BEGIN	
		DECLARE @DrivePlots TABLE
		([Timestamp] DATETIME,
		TestType NVARCHAR(100), Site NVARCHAR(50), PciId NVARCHAR(20), Latitude FLOAT, Longitude FLOAT, NetworkMode NVARCHAR(100), Band NVARCHAR(100),
		Carrier NVARCHAR(100),GsmRssi INT, GsmRxQual INT, WcdmaRssi INT, WcdmaRscp INT, WcdmaEcio FLOAT, LteRssi INT, LteRsrp INT, LteRsrq INT, LteRsnr FLOAT,
		LteCqi FLOAT, TestStatus BIT, IsHandover BIT, SubNetworkMode NVARCHAR(100), ActualBand NVARCHAR(100), ActualCarrier NVARCHAR(100),CellId NVARCHAR(20),serverTimeStamp DATETIME,
		pciColor nvarchar(50), rsrpColor nvarchar(50), rsrqColor nvarchar(50), sinrColor nvarchar(50),ChColor NVARCHAR(50))


		INSERT INTO @DrivePlots([Timestamp],TestType, [Site], PciId, Latitude, Longitude,
		NetworkMode, Band, Carrier, GsmRssi, GsmRxQual,WcdmaRssi, WcdmaRscp,
		WcdmaEcio, LteRssi, LteRsrp, LteRsrq, LteRsnr, LteCqi,
		TestStatus, IsHandover, SubNetworkMode, ActualBand,
		ActualCarrier,CellId,serverTimestamp,pciColor,rsrpColor,rsrqColor,sinrColor,chColor)
		Select DISTINCT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
		x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
		x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
		x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
		x.ActualCarrier,x.CellId,x.serverTimestamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor,x.chColor
		from AV_SiteTestLog x
		Where x.SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
		AND TestStatus=1 AND TestType IN('CCW','CW')
		AND x.isActive=1
		--AND logid%2=0
		ORDER BY x.TestType,x.[TimeStamp] DESC


		SELECT sit.Sector,sit.PciId
		INTO #Sectors
		FROM AV_SiteTestSummary AS sit
		INNER JOIN AV_Sectors AS sec ON sec.SiteId=sit.SiteId AND sec.NetworkModeId=sit.NetworkModeId AND sec.BandId=sit.BandId AND sec.CarrierId=sit.CarrierId AND sec.SectorId=sit.SectorId	
		where sit.SiteId=@SiteId and sit.BandId=@BandId and sit.CarrierId=@Carrier and sit.NetworkModeId=@NetworkMode
		AND sec.isActive=1

		SELECT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude,
		x.NetworkMode, x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi,
		x.WcdmaRscp, x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr,
		x.LteCqi, x.TestStatus, x.IsHandover, x.SubNetworkMode, x.ActualBand,
		x.ActualCarrier,y.Sector,x.CellId,x.serverTimestamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor,0 'FloorId'
		FROM @DrivePlots x LEFT OUTER JOIN #Sectors y ON x.PciId=y.PciId
		ORDER BY x.TestType
	END
	ELSE
	BEGIN		
		IF @Scope IN('SSV','NI')
		BEGIN
			--SELECT @SiteId,@SiteCode,@NetworkMode,@BandId,@Carrier,@ScopeId;
			EXEC [AV_GetKmlLayer]  'NetLayerReport', @SiteId,@SiteCode,@NetworkMode,@BandId,@Carrier,@ScopeId
		END
		ELSE IF @Scope IN('IND')
		BEGIN
			SELECT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude,
			x.NetworkMode, x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi,
			x.WcdmaRscp, x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr,
			x.LteCqi, x.TestStatus, x.IsHandover, x.SubNetworkMode, x.ActualBand,
			x.ActualCarrier,x.Sector,x.CellId,x.serverTimestamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor,x.FloorId,x.ChColor
			FROM AV_SiteTestLog x
			WHERE x.SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
		END
	END
	
	--SELECT rpt.*
	--FROM AD_ReportConfiguration rpt
	--INNER JOIN AV_Sites sit ON sit.ClientId=rpt.clientId AND sit.CityId=rpt.CityId
	--WHERE reportID=444 AND sit.SiteId=@SiteId;
	
	 DECLARE @CitiId AS NUMERIC(18,0)
	 DECLARE @ClientId AS NUMERIC(18,0)	 
	 SELECT @CitiId=CityId , @ClientId=ClientId,
	 @SiteCode=(CASE WHEN charindex('_',SiteCode)=0 AND charindex('-',SiteCode)=0 THEN SiteCode WHEN charindex('-',SiteCode)>0 THEN LEFT(SiteCode,charindex('-',SiteCode)-1) ELSE LEFT(SiteCode,charindex('_',SiteCode)-1) END) 	 
	   FROM AV_Sites  WHERE SiteId=@SiteId
	   
	 EXEC [AD_GetReportConfiguration]  'byCityId_ClientId', @CitiId, @ClientId,@ScopeId


	 IF @UploadDate<@ReportDate
	 BEGIN
		SELECT DISTINCT x.PciId,x.CellId,x.pciColor,x.TestType
		FROM @DrivePlots x --LEFT OUTER JOIN #Sectors y ON x.PciId=y.PciId		
	 END
	 ELSE
	 BEGIN
	 	--SELECT 0;
	 	SELECT DISTINCT x.PciId,x.pciColor,x.TestType
		FROM AV_SiteTestLog x
	 	WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
	 	AND x.TestType IN('CW','CCW') AND x.IsActive=1 AND x.pciColor IS NOT NULL
	 	AND x.PciId NOT IN(SELECT asts.PciId FROM AV_SiteTestSummary AS asts WHERE asts.SiteId = @SiteId and asts.BandId=@BandId and asts.CarrierId=@Carrier and asts.NetworkModeId=@NetworkMode)
	 	UNION ALL
	 	SELECT DISTINCT (CASE WHEN x.NetworkMode='NR' then x.NRCarrier else x.Carrier END) 'PciId',x.ChColor 'pciColor','Carrier' TestType
		FROM AV_SiteTestLog x
	 	WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
	 	AND x.TestType IN('CW','CCW') AND x.ChColor IS NOT NULL
	 --	UNION ALL
	 --	SELECT DISTINCT x.NRCarrier 'PciId',x.ChColor 'pciColor','Carrier' TestType
		--FROM AV_SiteTestLog x
	 --	WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
	 --	AND x.TestType IN('CW','CCW')
	 	ORDER BY x.TestType

	--select r.PciId,r.pciColor,r.TestType
	--from 	
	--(SELECT DISTINCT x.PciId,x.pciColor,x.TestType 
	--	FROM AV_SiteTestLog x
	-- 	WHERE x.SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
			
	-- 	AND x.TestType IN('CW','CCW')
	
	-- 	UNION ALL
	-- 	SELECT DISTINCT x.Carrier 'PciId',x.ChColor 'pciColor','Carrier' TestType
	--	FROM AV_SiteTestLog x
	-- 	WHERE x.SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
	-- 	AND x.TestType IN('CW','CCW')) r

	

		
	 END	 

--  serverTimeStamp
	 SELECT DISTINCT x.serverTimeStamp
	FROM AV_SiteTestLog x
	WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
	AND x.TestType IN('CW','CCW')-- AND x.IsActive=1
	ORDER BY x.serverTimeStamp

	 --  Lagend
	--ECLARE @RFCityId AS INT=ISNULL((SELECT TOP 1 arl.CityId FROM AV_RFPlotLegends AS arl WHERE arl.ClientId=@ClientId AND arl.CityId=@CitiId),0)
	SELECT arl.NetworkModeId, arl.PlotTypeId, arl.rangeFrom, arl.rangeTo, arl.rangeColor,def.KeyCode
	FROM AV_RFPlotLegends AS arl	
	inner join AD_Definations def on def.DefinationId=arl.PlotTypeId
	WHERE arl.ClientId=@ClientId AND arl.LegendTypeId=(CASE WHEN @CitiId IN(468,3151,3159,13174,13210,13211,13212,13213,13214,13215) THEN 1 ELSE 0 END) 
	AND arl.NetworkModeId=@NetworkMode
	AND arl.rangeFrom<>arl.rangeTo
	-- IN
	--(
	--	SELECT anls.NetworkModeId FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId
	--)


	--IF (SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=5)='Tampa'
	--IF (SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=5)='Chicago'
	--IF(SELECT x.ClientName  FROM AD_Clients x where ClientId=(SELECT clientid FROM AV_Sites WHERE SiteId=@SiteId))='Sprint'
	--BEGIN
		
		DECLARE @sitLat as float=0
		DECLARE @sitLng as float=0
		DECLARE @xNetMode AS NVARCHAR(10)=''
		
		SELECT @xNetMode=ad.DefinationName
		FROM AD_Definations AS ad
		WHERE ad.DefinationId=@NetworkMode
		
		SELECT @sitLat=Latitude,@sitLng=Longitude
		FROM AV_Sites WHERE Siteid=@SiteId

		DECLARE @maxDistance as float=0
		SET @maxDistance =ISNULL((SELECT MAX(ROUND(DistanceFromSite,0)) FROM AV_SiteTestLog WHERE Siteid=@SiteId and TestType IN('CW','CCW') AND IsActive=1),0)
	
		
		
		SELECT x.SiteCode,x.Latitude,x.Longitude,x.Azimuth,x.PCI,x.SectorColor,x.SectorCode,x.BeamWidth,ad.DefinationName 'Band',
		'' 'SiteType','' 'PORCheck','' 'RingId','' 'POR','' 'BandCapable','' 'BandPOR','' 'BandOA','' 'Comments',
		CAST(0 AS BIT) 'IsPOR'
		--ACOS( SIN( RADIANS(x.latitude) ) * SIN( RADIANS( @sitLat ) ) + COS( RADIANS(x.latitude) )
		-- COS( RADIANS( @sitLat ))  COS( RADIANS(x.longitude) - RADIANS( @sitLong )) ) * 6380 AS distance
		FROM AV_marketSites x
		LEFT OUTER JOIN AD_Definations AS ad ON ad.DefinationId=x.BandId
		WHERE x.SiteCode!=@SiteCode AND x.BandId=@BandId		
		AND ACOS( SIN( RADIANS(x.latitude) ) * SIN( RADIANS( @sitLat ) ) + COS( RADIANS(x.latitude) )
		* COS( RADIANS( @sitLat )) * COS( RADIANS(x.longitude) - RADIANS( @sitLng )) ) * 6380 < @maxDistance+5
		UNION ALL
		SELECT DISTINCT x.SiteCode,x.Latitude,x.Longitude,x.Azimuth,x.PCI,x.SectorColor,x.SectorCode,x.BeamWidth,ad.DefinationName 'Band',
		spo.SiteType 'SiteType',spo.PORToolCheck 'PORCheck',spo.RingId 'RingId',spo.POR 'POR',
		CASE WHEN @BandCode='LTE 700' THEN spo.L700Capable
			 WHEN @BandCode='LTE 1900' THEN spo.L1900Capable
		ELSE ''
		END 'BandCapable',
		CASE WHEN @BandCode='LTE 700' THEN spo.L700POR
			 WHEN @BandCode='LTE 1900' THEN spo.L1900POR
		ELSE ''
		END 'BandPOR',
		CASE WHEN @BandCode='LTE 700' THEN spo.L700OA
			 WHEN @BandCode='LTE 1900' THEN spo.L1900OA
		ELSE ''
		END 'BandOA',
		spo.Comments 'Comments',
		CASE WHEN (SELECT COUNT(pr.SiteId) FROM AV_SitePOR pr WHERE pr.SiteId=x.SiteCode AND pr.PORType=@BandCode)>0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END 'IsPOR'
		--ACOS( SIN( RADIANS(x.latitude) ) * SIN( RADIANS( @sitLat ) ) + COS( RADIANS(x.latitude) )
		-- COS( RADIANS( @sitLat ))  COS( RADIANS(x.longitude) - RADIANS( @sitLong )) ) * 6380 AS distance
		FROM AV_marketSites x
		LEFT OUTER JOIN AD_Definations AS ad ON ad.DefinationId=x.BandId
		LEFT OUTER JOIN AV_SitePOR spo ON spo.SiteId=x.SiteCode
		WHERE x.SiteCode!=@SiteCode AND spo.SiteId!=@SiteCode
		AND x.SiteCode NOT IN(SELECT ams.SiteCode FROM AV_MarketSites AS ams WHERE ams.BandId=@BandId)
		AND aCOS( SIN( RADIANS(x.latitude) ) * SIN( RADIANS( @sitLat ) ) + COS( RADIANS(x.latitude) )
		* COS( RADIANS( @sitLat )) * COS( RADIANS(x.longitude) - RADIANS( @sitLng )) ) * 6380 < @maxDistance+5
		
		--Site Data Location & RF Param Test Type Wise
		SELECT x.SiteId, x.NetworkModeId, x.BandId, x.CarrierId, x.ScopeId,
		       x.SectorId, x.TestType, x.TestLatitude, x.TestLongitude,
		       x.SignalStrength, x.SignalPower, x.SignalQuality, x.SignalNoise
		FROM AV_SectorTestLog x
		WHERE SiteId = @SiteId AND  NetworkModeId=@NetworkMode and BandId=@BandId and CarrierId=@Carrier
		
		--MO/MT Call Table
		SELECT DISTINCT lgi.siteID, lgi.networkModeID, lgi.bandID, lgi.carrierID,
		       lgi.scopeID, lgi.sectorID, lgi.fileType 'TestType',
		       (SELECT MIN(xy.[Time]) FROM AV_NemoSiteLogs AS xy INNER JOIN AV_LogsInfo AS lg ON xy.fileId_Fk=lgi.fileID) 'Timestamp',
		       lgi.fileType+': '+ nsl.[Event] 'Event','In' 'DirectionIN','Out' 'DirectionOUT',
		       nsl.EventFields + (CASE WHEN nsl.[Event] LIKE '%Success%' THEN '. PN: '+ISNULL((SELECT CAST(xx.PCI_PN AS NVARCHAR(50)) FROM AV_NemoSiteLogs AS xx WHERE xx.Id=nsl.Id-1),'')+'. Current System: '+ISNULL((SELECT xx.Current_System FROM AV_NemoSiteLogs AS xx WHERE xx.Id=nsl.Id-1),'') ELSE '' END) 'Description'
		FROM AV_NemoSiteLogs AS nsl INNER JOIN AV_LogsInfo AS lgi ON nsl.fileId_Fk=lgi.fileID
		INNER JOIN AV_Sectors AS sec ON sec.SectorId=lgi.sectorID
		WHERE lgi.SiteId = @SiteId AND  lgi.NetworkModeId=@NetworkMode and lgi.BandId=@BandId AND lgi.CarrierId=@Carrier AND lgi.fileType IN('MO','MT')
		AND
		nsl.[Event] IS NOT NULL AND nsl.[Event]!=''
		--GROUP BY lgi.siteID, lgi.networkModeID, lgi.bandID, lgi.carrierID,lgi.scopeID, lgi.sectorID, lgi.fileType, nsl.[Event],nsl.EventFields
		
		--CW/CCW Table
		--SELECT top 2 lgi.siteID, lgi.networkModeID, lgi.bandID, lgi.carrierID,
		--       lgi.scopeID, lgi.sectorID, lgi.fileType 'TestType',nsl.[Time] 'Timestamp',nsl.Latitude,nsl.Longitude,nsl.HO_Type 'HandoverType',
		--       nsl.Current_Channel 'FromCarrier', nsl.Current_Band 'FromBand', nsl.Attempted_System 'ToCarrier',
		--       nsl.Attempted_Band 'ToBand', nsl.Attempted_PCI 'ToPCI', nsl.HO_Duration_ms 'HoDuration',
		--       nsl.HO_Uplane_interruption_ms 'HoInterruptionTime', nsl.Current_PCI 'FromPCI',
		--       nsl.Attempted_Channel 'ToChannel'
		--FROM AV_NemoSiteLogs AS nsl INNER JOIN AV_LogsInfo AS lgi ON nsl.fileId_Fk=lgi.fileID
		--INNER JOIN AV_Sectors AS sec ON sec.SectorId=lgi.sectorID
		--WHERE lgi.SiteId = @SiteId AND  lgi.NetworkModeId=@NetworkMode and lgi.BandId=@BandId and lgi.CarrierId=@Carrier AND lgi.fileType IN('CW','CCW')
		--AND (nsl.Attempted_PCI IS NOT NULL OR nsl.Current_PCI IS NOT NULL)
		
		SELECT x.Id, x.siteID, x.networkModeID, x.bandID, x.carrierID, x.scopeID,
		       x.sectorID, x.TestType, MIN(x.[Timestamp]) 'Timestamp', x.Latitude, x.Longitude,
		       x.HandoverType, x.FromCarrier, x.FromBand, x.ToCarrier, x.ToBand,
		       x.ToPCI, MIN(x.HoDuration) 'HoDuration', MIN(x.HoInterruptionTime) 'HoInterruptionTime', x.FromPCI, x.ToChannel,x.FromRAT,x.ToRAT
		FROM
		(
			SELECT x.LogId 'Id',x.siteID, x.networkModeID, x.bandID, x.carrierID,
				   x.scopeID, x.sectorID, x.TestType 'TestType',x.[TimeStamp] 'Timestamp',
				   x.Latitude 'Latitude',
				   x.Longitude 'Longitude',
				   x.TestType 'HandoverType',
				   CASE WHEN (SELECT y.NetworkMode FROM AV_SiteTestLog y WHERE y.LogId=x.LogId-1)='LTE' THEN (SELECT y.Carrier FROM AV_SiteTestLog y WHERE y.LogId=x.LogId-1)
				   WHEN (SELECT y.NetworkMode FROM AV_SiteTestLog y WHERE y.LogId=x.LogId-1)='NR' THEN (SELECT y.NRCarrier FROM AV_SiteTestLog y WHERE y.LogId=x.LogId-1) 
				   ELSE '' END 'FromCarrier',
				   CASE WHEN (SELECT y.NetworkMode FROM AV_SiteTestLog y WHERE y.LogId=x.LogId-1)='LTE' THEN (SELECT y.Band FROM AV_SiteTestLog y WHERE y.LogId=x.LogId-1)
				   WHEN (SELECT y.NetworkMode FROM AV_SiteTestLog y WHERE y.LogId=x.LogId-1)='NR' THEN (SELECT y.NRBand FROM AV_SiteTestLog y WHERE y.LogId=x.LogId-1) 
				   ELSE '' END 'FromBand',
				   CASE WHEN (SELECT y.NetworkMode FROM AV_SiteTestLog y WHERE y.LogId=x.LogId)='LTE' THEN (SELECT y.Carrier FROM AV_SiteTestLog y WHERE y.LogId=x.LogId)
				   WHEN (SELECT y.NetworkMode FROM AV_SiteTestLog y WHERE y.LogId=x.LogId)='NR' THEN (SELECT y.NRCarrier FROM AV_SiteTestLog y WHERE y.LogId=x.LogId) 
				   ELSE '' END 'ToCarrier',
				   CASE WHEN (SELECT y.NetworkMode FROM AV_SiteTestLog y WHERE y.LogId=x.LogId)='LTE' THEN (SELECT y.Band FROM AV_SiteTestLog y WHERE y.LogId=x.LogId)
				   WHEN (SELECT y.NetworkMode FROM AV_SiteTestLog y WHERE y.LogId=x.LogId)='NR' THEN (SELECT y.NRBand FROM AV_SiteTestLog y WHERE y.LogId=x.LogId) 
				   ELSE '' END 'ToBand', x.ToPCI 'ToPCI',
				   0 'HoDuration',
				   0 'HoInterruptionTime',
				   x.FromPCI 'FromPCI',
				   x.Carrier 'ToChannel',
				   (SELECT y.NetworkMode FROM AV_SiteTestLog y WHERE y.LogId=x.LogId-1) 'FromRAT', x.NetworkMode 'ToRAT'
			FROM AV_SiteTestLog x
			WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
	 		AND x.TestType IN('CW','CCW') AND x.IsActive=1
	 		AND x.FromPCI<>x.ToPCI
	 		AND (x.FromPCI IN(SELECT CAST(asts.PciId AS NVARCHAR(15)) FROM AV_SiteTestSummary AS asts WHERE asts.SiteId = @SiteId and asts.BandId=@BandId and asts.CarrierId=@Carrier and asts.NetworkModeId=@NetworkMode)
	 		AND x.ToPCI IN(SELECT CAST(asts.PciId AS NVARCHAR(15)) FROM AV_SiteTestSummary AS asts WHERE asts.SiteId = @SiteId and asts.BandId=@BandId and asts.CarrierId=@Carrier and asts.NetworkModeId=@NetworkMode))
	 		AND x.NetworkMode='NR'
			
			/* FOR NEMO HO LOGS
			SELECT nsl.Id,lgi.siteID, lgi.networkModeID, lgi.bandID, lgi.carrierID,
				   lgi.scopeID, lgi.sectorID, lgi.fileType 'TestType',nsl.[Time] 'Timestamp',
				   (SELECT TOP 1 nm.Latitude FROM AV_NemoSiteLogs nm WHERE nm.Id<=nsl.Id AND nm.Latitude IS NOT NULL ORDER BY id DESC) 'Latitude',
				   (SELECT TOP 1 nm.Longitude FROM AV_NemoSiteLogs nm WHERE nm.Id<=nsl.Id AND nm.Latitude IS NOT NULL ORDER BY id DESC) 'Longitude',
				   nsl.HO_Type 'HandoverType',
				   nsl.Current_Channel 'FromCarrier', nsl.Current_Band 'FromBand', nsl.Attempted_System 'ToCarrier',
				   nsl.Attempted_Band 'ToBand', nsl.Attempted_Channel 'ToPCI',
				   (SELECT nm.HO_Duration_ms FROM AV_NemoSiteLogs nm WHERE nm.Id=nsl.Id+1) 'HoDuration',
				   (SELECT nm.HO_Uplane_interruption_ms FROM AV_NemoSiteLogs nm WHERE nm.Id=nsl.Id+1) 'HoInterruptionTime', nsl.Current_PCI 'FromPCI',
				   nsl.Attempted_Channel 'ToChannel'
			FROM AV_NemoSiteLogs AS nsl INNER JOIN AV_LogsInfo AS lgi ON nsl.fileId_Fk=lgi.fileID
			INNER JOIN AV_Sectors AS sec ON sec.SectorId=lgi.sectorID
			WHERE lgi.SiteId = @SiteId AND  lgi.NetworkModeId=@NetworkMode and lgi.BandId=@BandId and lgi.CarrierId=@Carrier 
			AND lgi.fileType IN('CW','CCW')
			AND (nsl.Current_PCI IS NOT NULL AND nsl.Current_PCI!='')
			*/
		) x
		WHERE x.FromRAT=@xNetMode AND x.ToRAT=@xNetMode
		GROUP BY  x.Id, x.siteID, x.networkModeID, x.bandID, x.carrierID, x.scopeID,
		       x.sectorID, x.TestType, x.[Timestamp], x.Latitude, x.Longitude,
		       x.HandoverType, x.FromCarrier, x.FromBand, x.ToCarrier, x.ToBand,
		       x.ToPCI,  x.FromPCI, x.ToChannel,x.FromRAT,x.ToRAT
		
		
		
		--SELECT * FROM AV_NemoSiteLogs AS ansl
		
		--FROM AV_marketSites x
		--LEFT OUTER JOIN AD_Definations AS ad ON ad.DefinationId=x.BandId
		--LEFT OUTER JOIN AV_SitePOR spo ON spo.SiteId=x.SiteCode
		--CROSS APPLY (SELECT cos(radians(@sitLat)) * cos(radians(x.Latitude)) * cos(radians(x.Longitude) - radians(@sitLng)) + sin(radians(@sitLat)) * sin(radians(x.Latitude))) T(ACosInput)
		--CROSS APPLY (SELECT ((3959 * acos(CASE WHEN ABS(ACosInput) > 1 THEN SIGN(ACosInput)*1 ELSE ACosInput END)))) T2(distance)
		--WHERE x.SiteCode!=@SiteCode AND spo.SiteId!=@SiteCode
		--AND x.SiteCode NOT IN(SELECT ams.SiteCode FROM AV_MarketSites AS ams WHERE ams.BandId=@BandId)
		--AND distance * 6380 < @maxDistance+5

		--EXEC [AV_GetKmlLayer]  'NetLayerReport', @SiteId,@SiteCode,@NetworkMode,@BandId,@Carrier,36
	--END
	
	
	SELECT afp.*,def.DefinationName 'FloorName' FROM AV_FloorPlan AS afp inner join AD_Definations def on afp.floorid=def.DefinationId
	WHERE afp.SiteId=@SiteId

	-- Getting Net Layer Observations (Comments)
	SELECT * FROM AV_NetLayerObservation x 
	INNER JOIN AV_NetLayerStatus y on x.LayerStatusId=y.LayerStatusId
	WHERE y.SiteId=@SiteId and NetworkModeId=@NetworkMode and BandId=@BandId and CarrierId=@Carrier
	
	
	-- Get AV_BeamTestLog
	SELECT Distinct BeamGroupId,BeamId,BMGColor,BMColor 
	FROM AV_BeamTestLog y
	WHERE y.SiteId=@SiteId and NetworkModeId=@NetworkMode and BandId=@BandId and CarrierId=@Carrier
	Group By BeamGroupId,BeamId,BMGColor,BMColor

	-- Legends For Beam Test Log		
	SELECT BeamGroupId as Id ,BMGColor as Color ,Count(BeamId) [Count] ,'BMG' as [Type]
	FROM AV_BeamTestLog y
	WHERE y.SiteId=@SiteId and NetworkModeId=@NetworkMode and BandId=@BandId and CarrierId=@Carrier
	GROUP BY BeamGroupId ,BMGColor
	UNION 
	SELECT BeamId as Id,BMColor as Color,Count(BeamId) as [Count]  ,'BM' as [Type]
	FROM AV_BeamTestLog  y
	WHERE y.SiteId=@SiteId and NetworkModeId=@NetworkMode and BandId=@BandId and CarrierId=@Carrier
	GROUP BY BeamId,BMColor

	SELECT y.TimeStamp,y.StackTrace
	FROM AV_SiteTestLog y
	WHERE SiteId=@SiteId and NetworkModeId=@NetworkMode and BandId=@BandId and CarrierId=@Carrier
	AND y.TestType NOT IN('CW','CCW')
	AND (y.StackTrace like 'Rach%' OR y.StackTrace LIKE 'HO%')
	
	--SELECT x.SiteId 'SiteId',x.NetworkModeId 'NetworkModeId',x.BandId 'BandId',x.CarrierId 'CarrierId',x.SectorId 'SectorId',x.ScopeId 'ScopeId',x.OoklaFilePath 'OoklaFilePath'
	--FROM AV_SiteOoklaLogs x
	--WHERE SiteId=@SiteId and NetworkModeId=@NetworkMode and BandId=@BandId and CarrierId=@Carrier;

;WITH CTE AS
(SELECT x.SiteId 'SiteId',x.NetworkModeId 'NetworkModeId',x.BandId 'BandId',x.CarrierId 'CarrierId',
x.SectorId 'SectorId',x.ScopeId 'ScopeId',x.OoklaFilePath 'OoklaFilePath', x.DownlinkSpeed , x.UplinkSpeed, x.Latency , x.NetworkMode
, ROW_NUMBER() Over (Partition By x.NetworkMode ,x.Sectorid , x.NetworkModeId, x.BandId, x.CarrierId Order By x.DownlinkSpeed Desc) RN
	FROM AV_SiteOoklaLogs x
	WHERE SiteId=@SiteId and NetworkModeId=@NetworkMode and BandId=@BandId and CarrierId=@Carrier)
	Select SiteId 'SiteId', NetworkModeId 'NetworkModeId',BandId 'BandId',
	CarrierId 'CarrierId',SectorId 'SectorId',ScopeId 'ScopeId', OoklaFilePath 'OoklaFilePath' , NetworkMode 'NetWorkMode', DownlinkSpeed 'DownLinkSpeed',
	UplinkSpeed 'UpLinkSpeed' from CTE Where RN =  1;
	
	
END