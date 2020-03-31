-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- [dbo].[AV_GetSiteReport] 'NIReport',112729
CREATE PROCEDURE [dbo].[AV_GetSiteReport]
 @Filter nvarchar(50)
,@SiteId numeric(18,0)
AS
BEGIN
	if @Filter='NIReport'
	begin
		Select smry.SummaryId, smry.ClientId, smry.RegionId, smry.Region,
		       smry.CityId, smry.City, smry.ClusterId, smry.Cluster, smry.SiteId,
		       smry.[Site], 
		       --(CASE WHEN charindex('_',smry.[Site])=0 AND charindex('-',smry.[Site])=0 THEN smry.[Site] WHEN charindex('-',smry.[Site])>0 THEN LEFT(smry.[Site],charindex('-',smry.[Site])-1) ELSE LEFT(smry.[Site],charindex('_',smry.[Site])-1) END) 'Site',
		       sec.SectorLatitude 'Latitude', sec.SectorLongitude 'Longitude', smry.SiteScheduleDate,
		       smry.SectorId, smry.Sector, smry.TestLatitude, smry.TestLongitude,
		       smry.ScopeId, smry.Scope, smry.NetworkModeId, smry.NetworkMode,
		       smry.BandId, smry.Band, smry.CarrierId, smry.Carrier, smry.Antenna,
		       smry.Azimuth, smry.PciId, smry.BeamWidth, smry.GsmRssi,
		       smry.GsmRxQual, smry.WcdmaRssi, smry.WcdmaRscp, smry.WcdmaEcio,
		       smry.LteRssi, smry.LteRsrp, smry.LteRsrq, smry.LteRsnr, smry.LteCqi,
		       smry.DistanceFromSite, smry.AngleToSite, smry.FtpStatus,
		       smry.PingHost, smry.LatencyRate, smry.PingIterations,
		       smry.PingMinResult, smry.PingMaxResult, smry.PingAverageResult,
		       smry.PingStatus, smry.DownlinkRate, smry.DownlinkMinResult,
		       smry.DownlinkMaxResult, smry.DownlinkAvgResult, smry.DownlinkStatus,
		       smry.UplinkRate, smry.UplinkMinResult, smry.UplinkMaxResult,
		       smry.UplinkAvgResult, smry.UplinkStatus, smry.ConnectionSetupTime,
		       smry.ConnectionSetupStatus, smry.MoMtCallNo, smry.MoMtCallDuration,
		       smry.MoStatus, smry.MtStatus, smry.VMoMtCallno,
		       smry.VMoMtCallDuration, smry.VMoStatus, smry.VMtStatus,
		       smry.CwHandoverStatus, smry.Ccwhandoverstatus,
		       smry.ICwHandoverStatus, smry.ICcwhandoverstatus, smry.FtpServerIp,
		       smry.FtpServerPort, smry.FtpServerPath, smry.FtpDownlinkFile,
		       smry.StationaryTestFilePath, smry.CwTestFilePath,
		       smry.CcwTestFilePath, smry.OoklaTestFilePath, smry.OoklaPingResult,
		       smry.OoklaDownlinkResult, smry.OoklaUplinkResult,
		       smry.OoklaLatitude, smry.OoklaLongitude, smry.OoklaRssi,
		       smry.OoklaSinr, smry.pciColor, smry.rsrpColor, smry.rsrqColor,
		       smry.sinrColor, smry.dlColor, smry.SMoStatus, smry.SMtStatus,smry.MimoStatus,smry.MimoTestFilePath,SpeedTestFilePath,CaActiveTestFilePath ,CaDeavticeTestFilePath ,CaSpeedTestFilePath ,LaaSpeedTestFilePath ,LaaSmTestFilePath,SMtStatus,SMoStatus,
		       smry.CarrierAggregationStatus, smry.E911Status,site.*,cli.Logo 'ClientLogo',ven.Logo 'VendorLogo',sec.sectorColor,cli.ClientPrefix,smry.IRATHandover,
		       '0' 'CellId','0' 'TCH',0 'SampleCount',CAST(1 AS BIT) 'FastReturnStatus',0.00 'CoverageDistance',smry.PhyDLSpeedMin,
		       smry.PhyDLSpeedMax, smry.PhyDLSpeedAvg, smry.PhyULSpeedMin,
		       smry.PhyULSpeedMax, smry.PhyULSpeedAvg, smry.IntraHOInteruptTime,
		       smry.callSetupTime, smry.IntreHOInteruptTime, smry.PhyDLStatus,
		       smry.PhyULStatus,smry.CADLSpeed,smry.CAULSpeed
		from AV_SiteTestSummary smry
		Inner Join AV_Sites site On Site.SiteId = smry.SiteId 
		left join AD_Clients cli on cli.ClientId=site.ClientId 
		left join AD_Clients ven on ven.ClientId=cli.PClientId
		inner join AV_Sectors sec on sec.SectorId=smry.SectorId
		INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=smry.SiteId AND anls.NetworkModeId=smry.NetworkModeId AND anls.BandId=smry.BandId AND anls.CarrierId=smry.CarrierId
		Where smry.SiteId = @SiteId AND anls.IsActive=1
		ORDER BY smry.NetworkModeId,smry.BandId,smry.CarrierId,
		CASE WHEN smry.Sector='Alpha' THEN 1
		 WHEN smry.Sector='Beta' THEN 2
		 WHEN smry.Sector='Gamma' THEN 3
		 WHEN smry.Sector='Delta' THEN 4
		 WHEN smry.Sector='Epsilon' THEN 5
		 WHEN smry.Sector='DiGamma' THEN 6
		 WHEN smry.Sector='Iota' THEN 7
		END



		--Legend 
		SELECT DISTINCT x.LogId, x.serverTimeStamp, x.PciId,x.pciColor,x.TestType
		FROM AV_SiteTestLog x
	 	WHERE SiteId = @SiteId AND x.IsActive=1
	 	AND x.TestType IN('CW','CCW')

		-- Configurations
		 DECLARE @CitiId AS NUMERIC(18,0)
		 DECLARE @ClientId AS NUMERIC(18,0)	 
		 DECLARE @SiteCode AS NVARCHAR(50)
		 DECLARE @ScopeId AS NUMERIC
		 SELECT @CitiId=CityId , @ClientId=ClientId, @SiteCode=SiteCode, @ScopeId=ScopeId
		   FROM AV_Sites  WHERE SiteId=@SiteId

		 EXEC [AD_GetReportConfiguration]  'byCityId_ClientId', @CitiId, @ClientId, @ScopeId
		 
		 SELECT y.SiteId, y.[Site], y.NetworkModeId, y.NetworkMode, y.BandId, y.Band, y.CarrierId, y.Carrier,

		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1">\n<Document>\n<name>'+''+'</name><description>'+''+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'">'+
			'<LineStyle>'+
			'<color>ff' + SUBSTRING(RIGHT(pciColor,6),5,2)+ SUBSTRING(RIGHT(pciColor,6),3,2) +SUBSTRING(RIGHT(pciColor,6),1,2)+ '</color>'+
			'<width>10</width>'+
			'</LineStyle>'+
			'</Style>'+
			'<Placemark>'+
			'<name>cw1</name>'+
			'<styleUrl>#Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
			'<LineString>'+
			'<altitudeMode>relative</altitudeMode>'+
			'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_SiteTestLog x
		WHERE x.SiteId = y.SiteId AND x.NetworkModeId=y.NetworkModeId AND x.BandId=y.BandId AND x.CarrierId=y.CarrierId AND x.TestType IN('CW') AND x.IsActive=1
		AND x.IsActive=1
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' 'pciPlot'
		FROM
		(
			SELECT DISTINCT asts.SiteId, asts.[Site], asts.NetworkModeId, asts.NetworkMode, asts.BandId, asts.Band, asts.CarrierId, asts.Carrier
			FROM AV_SiteTestSummary AS asts
			INNER JOIN AV_SiteTestLog AS astl ON astl.SiteId=asts.SiteId AND astl.NetworkModeId=asts.NetworkModeId AND astl.BandId=asts.BandId AND astl.CarrierId=asts.CarrierId
			INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=asts.SiteId AND anls.NetworkModeId=asts.NetworkModeId AND anls.BandId=asts.BandId AND anls.CarrierId=asts.CarrierId			
			WHERE asts.SiteId=@SiteId --AND asts.NetworkMode='LTE' AND astl.TestType='CW' --AND asts.NetworkModeId=15 AND asts.BandId=75 AND asts.CarrierId=3143
			
			--SELECT DISTINCT asts.SiteId, asts.[Site], asts.NetworkModeId, asts.NetworkMode, asts.BandId, asts.Band, asts.CarrierId, asts.Carrier
			--FROM AV_SiteTestSummary AS asts
			--WHERE asts.SiteId=@SiteId AND asts.NetworkMode='LTE'
		) y


		--serverTimeStamp Table 4
		SELECT DISTINCT x.SiteId, x.NetworkModeId,x.SubNetworkMode 'NetworkMode',x.BandId,x.ActualBand 'Band',x.CarrierId,x.ActualCarrier 'Carrier', x.serverTimeStamp,
		x.SubNetworkMode+'_'+x.ActualBand+'_'+x.ActualCarrier 'NetLayer', x.IsActive
		FROM AV_SiteTestLog x
		WHERE SiteId= @SiteId
	END

		---Sector Swap Charts
	ELSE IF @Filter='SectorSwapCharts'
	BEGIN
		SELECT asts.PCI
		INTO #tmpPCI
		FROM AV_Sectors AS asts
		WHERE asts.SiteId=@SiteId
	
		SELECT asts.[TimeStamp] 'serverTimeStamp' ,asts.PciId 'PCI',asts.NetworkModeId, asts.BandId, asts.CarrierId,asts.SectorId,sec.SectorCode,
		CASE WHEN asts.NetworkMode='LTE' then asts.LteRsrp
		WHEN asts.NetworkMode='WCDMA' then asts.WcdmaRscp
		WHEN asts.NetworkMode='GSM' then asts.GsmRssi
		ELSE 0
		END 'SignalStrength',ISNULL(sec.sectorColor,'#87898A') 'ColorCode',CAST(asts.NetworkModeId as nvarchar(15))+'_'+CAST(asts.BandId as nvarchar(15))+'_'+CAST(asts.CarrierId as nvarchar(15)) 'NetLayer','ColorCode',CAST(ntm.DefinationName as nvarchar(50))+'_'+CAST(bnd.DefinationName as nvarchar(50))+'_'+CAST(crr.DefinationName as nvarchar(50)) 'NetLayerTitle'
		, ntm.DefinationName 'NetworkMode'
		, bnd.DefinationName 'Band'
		, crr.DefinationName 'Carrier'
		FROM AV_SiteTestLog AS asts
		INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId AND sec.SectorId=asts.SectorId
		INNER JOIN AD_Definations ntm on ntm.DefinationId=sec.NetworkModeId
		INNER JOIN AD_Definations bnd on bnd.DefinationId=sec.BandId
		INNER JOIN AD_Definations crr on crr.DefinationId=sec.CarrierId
		WHERE asts.SiteId=@SiteId AND asts.TestType='IDLE'
		UNION ALL
		SELECT DISTINCT asts.[TimeStamp],asts.PCI1 'PCI',asts.NetworkModeId, asts.BandId, asts.CarrierId,asts.SectorId,sec.SectorCode,
		asts.SS1 'SignalStrength',ISNULL(sec.sectorColor,'#87898A') 'ColorCode',CAST(asts.NetworkModeId as nvarchar(15))+'_'+CAST(asts.BandId as nvarchar(15))+'_'+CAST(asts.CarrierId as nvarchar(15)) 'NetLayer','ColorCode',CAST(ntm.DefinationName as nvarchar(50))+'_'+CAST(bnd.DefinationName as nvarchar(50))+'_'+CAST(crr.DefinationName as nvarchar(50)) 'NetLayerTitle'
		, ntm.DefinationName 'NetworkMode'
		, bnd.DefinationName 'Band'
		, crr.DefinationName 'Carrier'
		FROM AV_NeighbourLogs AS asts
		INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId AND sec.SectorId=asts.SectorId --AND sec.PCI=asts.Pci1
		INNER JOIN AD_Definations ntm on ntm.DefinationId=sec.NetworkModeId
		INNER JOIN AD_Definations bnd on bnd.DefinationId=sec.BandId
		INNER JOIN AD_Definations crr on crr.DefinationId=sec.CarrierId
		WHERE asts.SiteId=@SiteId 
		AND asts.PCI1>0 AND asts.PCI1 IN(SELECT x.PCI FROM #tmpPCI x)
		--UNION ALL
		--SELECT asts.[TimeStamp],asts.PCI2 'PCI',asts.NetworkModeId, asts.BandId, asts.CarrierId,asts.SectorId,sec.SectorCode,
		--asts.SS2 'SignalStrength',ISNULL('#87898A','#87898A') 'ColorCode'
		--FROM AV_NeighbourLogs AS asts
		--INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId AND sec.SectorId=asts.SectorId
		--WHERE asts.SiteId=@SiteId 
		--AND asts.PCI2>0  AND sec.PCI IN(SELECT x.PCI FROM #tmpPCI x)
		--UNION ALL
		--SELECT asts.[TimeStamp],asts.PCI3 'PCI',asts.NetworkModeId, asts.BandId, asts.CarrierId,asts.SectorId,sec.SectorCode,
		--asts.SS3 'SignalStrength',ISNULL('#87898A','#87898A') 'ColorCode'
		--FROM AV_NeighbourLogs AS asts
		--INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId  AND sec.SectorId=asts.SectorId
		--WHERE asts.SiteId=@SiteId 
		--AND asts.PCI3>0 AND sec.PCI IN(SELECT x.PCI FROM #tmpPCI x)
		
		
		SELECT asts.SiteId,CAST(asts.NetworkModeId as nvarchar(15))+'_'+CAST(asts.BandId as nvarchar(15))+'_'+CAST(asts.CarrierId as nvarchar(15)) 'NetLayer',asts.SectorId,asts.Sector,asts.PciId 'PCI',
		COUNT(asts.LogId) 'SampleCount',0 'SamplePercentage',0 'ReselectionCount'
		FROM AV_SiteTestLog AS asts
		WHERE asts.SiteId=@SiteId AND asts.SectorId!=0
		GROUP BY asts.SiteId,asts.NetworkModeId,asts.BandId,asts.CarrierId,asts.SectorId,asts.Sector,asts.PciId

		DROP TABLE #tmpPCI
	END



	---------------------------------------------------------
	--SELECT asts.PCI
	--INTO #tmpPCI
	--FROM AV_Sectors AS asts
	--WHERE asts.SiteId=@SiteId

	--SELECT asts.[TimeStamp],asts.PciId 'PCI',asts.NetworkModeId, asts.BandId, asts.CarrierId,asts.SectorId,sec.SectorCode,
	--CASE WHEN asts.NetworkMode='LTE' then asts.LteRsrp
	--	 WHEN asts.NetworkMode='WCDMA' then asts.WcdmaRscp
	--	 WHEN asts.NetworkMode='GSM' then asts.GsmRssi
	--ELSE 0
	--END 'SignalStrength',ISNULL(asts.pciColor,'#87898A') 'ColorCode'
	--FROM AV_SiteTestLog AS asts
	--INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId AND sec.PCI=asts.PciId
	--WHERE asts.SiteId=@SiteId AND asts.TestType='IDLE'
	--UNION ALL
	--SELECT asts.[TimeStamp],asts.PCI1 'PCI',asts.NetworkModeId, asts.BandId, asts.CarrierId,asts.SectorId,sec.SectorCode,
	--asts.SS1 'SignalStrength',ISNULL('#87898A','#87898A') 'ColorCode'
	--FROM AV_NeighbourLogs AS asts
	--INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId --AND sec.PCI=asts.Pci1
	--WHERE asts.SiteId=@SiteId 
	--AND asts.PCI1>0 AND sec.PCI IN(SELECT x.PCI FROM #tmpPCI x)
	--UNION ALL
	--SELECT asts.[TimeStamp],asts.PCI2 'PCI',asts.NetworkModeId, asts.BandId, asts.CarrierId,asts.SectorId,sec.SectorCode,
	--asts.SS2 'SignalStrength',ISNULL('#87898A','#87898A') 'ColorCode'
	--FROM AV_NeighbourLogs AS asts
	--INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId --AND sec.PCI=asts.Pci1
	--WHERE asts.SiteId=@SiteId 
	--AND asts.PCI2>0  AND sec.PCI IN(SELECT x.PCI FROM #tmpPCI x)
	--UNION ALL
	--SELECT asts.[TimeStamp],asts.PCI3 'PCI',asts.NetworkModeId, asts.BandId, asts.CarrierId,asts.SectorId,sec.SectorCode,
	--asts.SS3 'SignalStrength',ISNULL('#87898A','#87898A') 'ColorCode'
	--FROM AV_NeighbourLogs AS asts
	--INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId --AND sec.PCI=asts.Pci1
	--WHERE asts.SiteId=@SiteId 
	--AND asts.PCI3>0 AND sec.PCI IN(SELECT x.PCI FROM #tmpPCI x)

	--DROP TABLE #tmpPCI

	--------------------------------------------

	
	ELSE IF @Filter='NILayersTimeStamp'
	BEGIN
		SELECT DISTINCT x.SiteId, x.NetworkModeId,x.SubNetworkMode 'NetworkMode',x.BandId,x.ActualBand 'Band',x.CarrierId,x.ActualCarrier 'Carrier',  x.serverTimeStamp,
		x.SubNetworkMode+'_'+x.ActualBand+'_'+x.ActualCarrier 'NetLayer', x.IsActive
		FROM AV_SiteTestLog x
		WHERE SiteId =@SiteId -- and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
		ORDER BY x.serverTimeStamp
	END

	ELSE IF @Filter='NIReportLayers'
	BEGIN
		Select DISTINCT   x.SiteId, x.NetworkModeId, x.SubNetworkMode 'NetworkMode',x.BandId,x.ActualBand 'Band',x.CarrierId,x.ActualCarrier 'Carrier', 
         x.SubNetworkMode +'_'+ x.ActualBand +'_'+ x.ActualCarrier 'NetLayer', x.PciId,
		 CAST(x.NetworkModeId as nvarchar(15))+'_'+CAST(x.BandId as nvarchar(15))+'_'+CAST(x.CarrierId as nvarchar(15)) 'NetLayerId'
		 --, x.pciColor, x.AngleToSite, x.DistanceFromSite
		 FROM AV_SiteTestLog x 
		 WHERE x.SiteId =@SiteId
	END


END