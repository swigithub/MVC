
CREATE PROCEDURE [dbo].[AV_GetNetLayerSummary]
	@SiteId int,
	@BandId int,
	@Carrier nvarchar(50),
	@NetworkMode nvarchar(50),
	@UserId NUMERIC(18,0)
AS
BEGIN
		DECLARE @BandNo AS NVARCHAR(50)= (SELECT ad.DisplayText FROM AD_Definations AS ad WHERE ad.DefinationId=@Carrier)
		DECLARE @Scope AS NVARCHAR(50)= (SELECT ad.DefinationName FROM AV_Sites AS x INNER JOIN AD_Definations AS ad ON x.ScopeId=ad.DefinationId WHERE x.SiteId=@SiteId)
		 

		 
		IF @Scope IN('SSV','IND','NI')
		BEGIN
		SELECT rpt.SummaryId, rpt.ClientId, rpt.RegionId, rpt.Region, rpt.CityId,rpt.City  , rpt.ClusterId, rpt.Cluster, rpt.SiteId, 
		  --CASE WHEN charindex('_',Site)=0 THEN Site ELSE LEFT(site,charindex('_',Site)-1) END 'Site',
		  CASE WHEN charindex('_',Site)=0 AND charindex('-',Site)=0 THEN Site WHEN charindex('-',Site)>0 THEN LEFT(site,charindex('-',Site)-1) ELSE LEFT(site,charindex('_',Site)-1) END 'Site',
	       rpt.Latitude, rpt.Longitude, rpt.SiteScheduleDate, rpt.SectorId, rpt.Sector,rpt.MimoStatus,rpt.MimoTestFilePath,rpt.SpeedTestFilePath,rpt.CaActiveTestFilePath,rpt.CaDeavticeTestFilePath,rpt.CaSpeedTestFilePath,rpt.LaaSpeedTestFilePath,rpt.LaaSmTestFilePath,
	       --CASE WHEN rpt.City='Chicago' THEN 
	       --		(CASE	WHEN rpt.Band='LTE 700' THEN 'D'
	       --				WHEN rpt.Band='LTE 1900' THEN 'B'
	       --				WHEN rpt.Band='LTE 2100' THEN 'L'
	       --		ELSE ''
	       --		END)+rpt.[Site]+
	       --		(
	       --			CASE WHEN rpt.Sector='Alpha' THEN '11'
	       --				 WHEN rpt.Sector='Beta' THEN '21'
	       --				 WHEN rpt.Sector='Gamma' THEN '31'
	       --				 WHEN rpt.Sector='Delta' THEN '41'
	       --				 WHEN rpt.Sector='Epsilon' THEN '51'
	       --				 WHEN rpt.Sector='DiGamma' THEN '61'
	       --				 WHEN rpt.Sector='Iota' THEN '71'
	       --			ELSE '' 
	       --			END
	       --		)
	       --ELSE	rpt.Sector
	       --END 'Sector',
	       CASE WHEN rpt.NetworkMode='NR' THEN rpt.OoklaLatitude ELSE ISNULL(rpt.TestLatitude,0) END 'TestLatitude', 
	       CASE WHEN rpt.NetworkMode='NR' THEN rpt.OoklaLongitude ELSE ISNULL(rpt.TestLongitude,0) END 'TestLongitude', rpt.ScopeId, rpt.Scope,
	       rpt.NetworkModeId, rpt.NetworkMode, rpt.BandId,
			rpt.Band,
	       rpt.CarrierId,rpt.Carrier, rpt.Antenna, rpt.Azimuth, rpt.PciId, rpt.BeamWidth,
	       ISNULL(rpt.GsmRssi,0) 'GsmRssi', ISNULL(rpt.GsmRxQual,0) 'GsmRxQual', ISNULL(rpt.WcdmaRssi,0) 'WcdmaRssi', ISNULL(rpt.WcdmaRscp,0) 'WcdmaRscp', 
	       ISNULL(rpt.WcdmaEcio,0) 'WcdmaEcio',ISNULL(rpt.LteRssi,0) 'LteRssi', ISNULL(rpt.LteRsrp,0) 'LteRsrp', ISNULL(rpt.LteRsrq,0) 'LteRsrq',
	       ISNULL(rpt.LteRsnr,0) 'LteRsnr', ISNULL(rpt.LteCqi,0) 'LteCqi',
	       ISNULL(rpt.DistanceFromSite,0) 'DistanceFromSite', ISNULL(rpt.AngleToSite,0) 'AngleToSite', rpt.FtpStatus, rpt.PingHost,rpt.LatencyRate, 
	       rpt.PingIterations, ISNULL(rpt.PingMinResult,0) 'PingMinResult',
	       ISNULL(rpt.PingMaxResult,0) 'PingMaxResult', ISNULL(rpt.PingAverageResult,0) 'PingAverageResult',
	       CASE WHEN ISNULL(rpt.PingAverageResult,0)>0 AND ISNULL(rpt.PingAverageResult,0)<=rpt.LatencyRate THEN CAST(1 AS BIT) WHEN ISNULL(rpt.PingAverageResult,0)>0 AND ISNULL(rpt.PingAverageResult,0)>rpt.LatencyRate THEN CAST(0 AS BIT) ELSE NULL END 'PingStatus',
	       rpt.DownlinkRate, ISNULL(rpt.DownlinkMinResult,0) 'DownlinkMinResult', ISNULL(rpt.DownlinkMaxResult,0) 'DownlinkMaxResult',
	       ISNULL(rpt.DownlinkAvgResult,0) 'DownlinkAvgResult',
	       CASE WHEN rpt.Scope='NI' THEN rpt.DownlinkStatus ELSE CASE WHEN ISNULL(rpt.DownlinkMaxResult,0)>0 AND ISNULL(rpt.DownlinkMaxResult,0)>=rpt.DownlinkRate THEN CAST(1 AS BIT) WHEN ISNULL(rpt.DownlinkMaxResult,0)>0 AND ISNULL(rpt.DownlinkMaxResult,0)<rpt.DownlinkRate THEN CAST(0 AS BIT) ELSE NULL END END 'DownlinkStatus',
	       rpt.UplinkRate,rpt.UplinkMinResult, rpt.UplinkMaxResult, rpt.UplinkAvgResult,
	       CASE WHEN ISNULL(rpt.UplinkMaxResult,0)>0 AND ISNULL(rpt.UplinkMaxResult,0)>=rpt.UplinkRate THEN CAST(1 AS BIT) WHEN ISNULL(rpt.UplinkMaxResult,0)>0 AND ISNULL(rpt.UplinkMaxResult,0)<rpt.UplinkRate THEN CAST(0 AS BIT) ELSE NULL END 'UplinkStatus',
	       rpt.ConnectionSetupTime, rpt.ConnectionSetupStatus,rpt.MoMtCallNo, rpt.MoMtCallDuration, rpt.MoStatus, rpt.MtStatus,
	       rpt.VMoMtCallno, rpt.VMoMtCallDuration, rpt.VMoStatus, rpt.VMtStatus,CAST(ISNULL(rpt.CwHandoverStatus,0) AS BIT) 'CwHandoverStatus', CAST(ISNULL(rpt.Ccwhandoverstatus,0) AS BIT) 'Ccwhandoverstatus',
	       rpt.FtpServerIp,rpt.callSetupTime,rpt.IntraHOInteruptTime,rpt.IntreHOInteruptTime,rpt.PhyDLSpeedMin,rpt.PhyDLSpeedMax,rpt.PhyDLSpeedAvg,rpt.PhyULSpeedMin,rpt.PhyULSpeedMax,rpt.PhyULSpeedAvg,
	       rpt.FtpServerPort, rpt.FtpServerPath, rpt.FtpDownlinkFile,rpt.StationaryTestFilePath, rpt.CwTestFilePath, rpt.CcwTestFilePath,rpt.PhyDLStatus,rpt.PhyULStatus,
	       ISNULL(rpt.OoklaTestFilePath,0) 'OoklaTestFilePath', ISNULL(rpt.OoklaPingResult,0) 'OoklaPingResult', ISNULL(rpt.OoklaDownlinkResult,0) 'OoklaDownlinkResult',
	       ISNULL(rpt.OoklaUplinkResult,0) 'OoklaUplinkResult',ISNULL(rpt.TestCount,0) 'TestCount',ISNULL( rpt.TestDuration,0) 'TestDuration',
	       rpt.ClientLogo, rpt.VendorLogo,rpt.CADLSpeed,rpt.CAULSpeed,
		   --CASE WHEN ISNULL(rpt.ICwHandoverStatus,0)=0 THEN CAST(0 AS BIT) ELSE CAST(ISNULL(rpt.ICwHandoverStatus,0) AS BIT) END 'ICwHandoverStatus',
		   CASE WHEN ISNULL(rpt.ICwHandoverStatus,0) IS NULL AND ISNULL(rpt.ICcwhandoverstatus,0) IS NULL THEN NULL
				WHEN ISNULL(rpt.ICwHandoverStatus,0)=CAST(0 as bit) AND ISNULL(rpt.ICcwhandoverstatus,0)=CAST(0 as bit) THEN NULL
				WHEN ISNULL(rpt.ICwHandoverStatus,0)=CAST(1 as bit) AND ISNULL(rpt.ICcwhandoverstatus,0)=CAST(0 as bit) THEN CAST(1 as bit)
				WHEN ISNULL(rpt.ICwHandoverStatus,0)=CAST(0 as bit) AND ISNULL(rpt.ICcwhandoverstatus,0)=CAST(1 as bit) THEN CAST(1 as bit)
				WHEN ISNULL(rpt.ICwHandoverStatus,0)=CAST(1 as bit) AND ISNULL(rpt.ICcwhandoverstatus,0)=CAST(1 as bit) THEN CAST(1 as bit)
				ELSE NULL
		   END 'ICwHandoverStatus',
		   CAST(ISNULL(rpt.ICcwhandoverstatus,0) AS BIT) 'ICcwhandoverstatus',Site 'ActualSiteCode',
		   rpt.SiteName, rpt.SiteType, rpt.SiteClass, rpt.LayerStatus,rpt.RFHeight,rpt.ETilt,rpt.MTilt,rpt.CellId,rpt.BandWidth,rpt.SiteUploadDate,@BandNo 'BandNo',
		   CASE WHEN ISNULL(rpt.CwHandoverStatus,0) IS NULL AND ISNULL(rpt.Ccwhandoverstatus,0) IS NULL THEN NULL
				WHEN ISNULL(rpt.CwHandoverStatus,0)=CAST(0 as bit) AND ISNULL(rpt.Ccwhandoverstatus,0)=CAST(0 as bit) THEN NULL
				WHEN ISNULL(rpt.CwHandoverStatus,0)=CAST(1 as bit) AND ISNULL(rpt.Ccwhandoverstatus,0)=CAST(0 as bit) THEN CAST(1 as bit)
				WHEN ISNULL(rpt.CwHandoverStatus,0)=CAST(0 as bit) AND ISNULL(rpt.Ccwhandoverstatus,0)=CAST(1 as bit) THEN CAST(1 as bit)
				WHEN ISNULL(rpt.CwHandoverStatus,0)=CAST(1 as bit) AND ISNULL(rpt.Ccwhandoverstatus,0)=CAST(1 as bit) THEN CAST(1 as bit)
				ELSE NULL
		   END 'MobHandoverStatus', rpt.Project, rpt.ClientPrefix, rpt.SiteAddress, rpt.SMoStatus, rpt.SMtStatus, rpt.CarrierAggregationStatus, rpt.E911Status, rpt.SectorColor, rpt.MRBTS,
			   rpt.SectorLatitude,rpt.SectorLongitude,  rpt.MinConSetupTime 'MinConSetupTime', rpt.MaxConSetupTime 'MaxConSetupTime', rpt.AvgConSetupTime 'AvgConSetupTime', ROUND(rpt.HoInterruptionTime*1000,2) 'HoInterruptionTime',ROUND(rpt.CcwHoInterruptionTime*1000,2) 'CcwHoInterruptionTime',CAST (1 as bit) IRATHandover,
		       '0' 'CellId','0' 'TCH',0 'SampleCount',CAST(1 AS BIT) 'FastReturnStatus',0.00 'CoverageDistance',
		       (SELECT astl.TestLatitude FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='DL') 'DLLat',
		       (SELECT astl.TestLongitude FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='DL') 'DLLng',
		       (SELECT astl.SignalStrength FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='DL') 'DLSignalStrength',
		       (SELECT astl.SignalQuality FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='DL') 'DLSignalQuality',
		       (SELECT astl.SignalNoise FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='DL') 'DLSignalNoise',
		        (SELECT astl.TestLatitude FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='CONNECTION_SETUP') 'CSLat',
		       (SELECT astl.TestLongitude FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='CONNECTION_SETUP') 'CSLng',
		       (SELECT astl.SignalStrength FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='CONNECTION_SETUP') 'CSSignalStrength',
		       (SELECT astl.SignalQuality FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='CONNECTION_SETUP') 'CSSignalQuality',
		       (SELECT astl.SignalNoise FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='CONNECTION_SETUP') 'CSSignalNoise',
		       (SELECT astl.TestLatitude FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='PING') 'RttLat',
		       (SELECT astl.TestLongitude FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='PING') 'RttLng',
		       (SELECT astl.SignalStrength FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='PING') 'RttSignalStrength',
		       (SELECT astl.SignalQuality FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='PING') 'RttSignalQuality',
		       (SELECT astl.SignalNoise FROM AV_SectorTestLog AS astl WHERE astl.SiteId=rpt.SiteId AND astl.NetworkModeId=rpt.NetworkModeId AND astl.BandId=rpt.BandId AND astl.CarrierId=rpt.CarrierId AND astl.SectorId=rpt.SectorId AND astl.TestType='PING') 'RttSignalNoise'
	FROM
	(
		select DISTINCT sit.SummaryId, sit.ClientId, sit.RegionId, sit.Region, sit.CityId,sit.City, sit.ClusterId, sit.Cluster, sit.SiteId, sit.[Site],
			   -- sit.Latitude, sit.Longitude, 
			   sec.SectorLatitude 'Latitude', sec.SectorLongitude 'Longitude',
			   sit.OoklaLatitude,
			   sit.OoklaLongitude, 
			   CASE WHEN anls.DriveCompletedOn IS NOT NULL THEN anls.DriveCompletedOn ELSE GETDATE() END  'SiteScheduleDate', sit.SectorId,sit.Sector,
			   sit.TestLatitude, --5G_LAT
			   sit.TestLongitude, --5G_LONG
			   sit.ScopeId, sit.Scope,
			   sit.NetworkModeId, sit.NetworkMode, sit.BandId, sit.Band, sit.CarrierId,sit.Carrier, sit.Antenna, sit.Azimuth, sit.PciId, sit.BeamWidth,
			   CASE WHEN sit.City='Birmingham' AND sit.NetworkMode='GSM' AND ISNULL(sit.OoklaRssi,0)<>0 THEN ISNULL(sit.OoklaRssi,0) ELSE sit.GsmRssi END 'GsmRssi',
			   CASE WHEN sit.City='Birmingham' AND sit.NetworkMode='GSM' AND ISNULL(sit.OoklaSinr,0)<>0 THEN ISNULL(sit.OoklaSinr,0) ELSE sit.GsmRxQual END 'GsmRxQual',
			   --(SELECT AVG(sol.LteRsrq) FROM AV_SiteTestLog sol WHERE sol.siteid=sit.SiteId AND sol.NetworkModeId=sit.NetworkModeId AND sol.BandId=sit.BandId AND sol.CarrierId=sit.CarrierId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE' AND sol.LteRsrp<>0) 'WcdmaRssi', --4G_RSRQ
			   (SELECT TOP 1 sol.RSRQ FROM AV_SiteOoklalogs sol WHERE sol.siteid=sit.SiteId AND sol.NetworkModeId=sit.NetworkModeId AND sol.BandId=sit.BandId AND sol.CarrierId=sit.CarrierId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'WcdmaRssi', --4G_RSRQ
			   CASE WHEN sit.City='Birmingham' AND sit.NetworkMode='WCDMA' AND ISNULL(sit.OoklaRssi,0)<>0 THEN ISNULL(sit.OoklaRssi,0) ELSE sit.WcdmaRscp END 'WcdmaRscp',
			   --(SELECT TOP 1 sol.Latitude FROM AV_SiteTestLog sol WHERE sol.siteid=sit.SiteId AND sol.NetworkModeId=sit.NetworkModeId AND sol.BandId=sit.BandId AND sol.CarrierId=sit.CarrierId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE' AND sol.LteRsrq<>0) 'WcdmaEcio',  --4G_LAT 
			   (SELECT TOP 1 sol.Latitude FROM AV_SiteOoklalogs sol WHERE sol.siteid=sit.SiteId AND sol.NetworkModeId=sit.NetworkModeId AND sol.BandId=sit.BandId AND sol.CarrierId=sit.CarrierId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'WcdmaEcio', --4G_LTE
			   
			   --(SELECT AVG(sol.LteRsrp) FROM AV_SiteTestLog sol WHERE sol.siteid=sit.SiteId AND sol.NetworkModeId=sit.NetworkModeId AND sol.BandId=sit.BandId AND sol.CarrierId=sit.CarrierId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'LteRssi', --4G_RSRP
			   (SELECT TOP 1 sol.RSRP FROM AV_SiteOoklalogs sol WHERE sol.siteid=sit.SiteId AND sol.NetworkModeId=sit.NetworkModeId AND sol.BandId=sit.BandId AND sol.CarrierId=sit.CarrierId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'LteRssi', --4G_RSRP
			   CASE WHEN sit.City='Birmingham' AND sit.NetworkMode='LTE' AND ISNULL(sit.OoklaRssi,0)<>0 THEN ISNULL(sit.OoklaRssi,0) ELSE sit.LteRsrp END 'LteRsrp',			   
			   sit.LteRsrq,
			   CASE WHEN sit.City='Birmingham' AND sit.NetworkMode='LTE' AND ISNULL(sit.OoklaSinr,0)<>0 THEN ISNULL(sit.OoklaSinr,0) ELSE sit.LteRsnr END 'LteRsnr',
			    --(SELECT TOP 1 sol.Longitude FROM AV_SiteTestLog sol WHERE sol.siteid=sit.SiteId AND sol.NetworkModeId=sit.NetworkModeId AND sol.BandId=sit.BandId AND sol.CarrierId=sit.CarrierId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'LteCqi', --4G_LONG
			    (SELECT TOP 1 sol.Longitude FROM AV_SiteOoklalogs sol WHERE sol.siteid=sit.SiteId AND sol.NetworkModeId=sit.NetworkModeId AND sol.BandId=sit.BandId AND sol.CarrierId=sit.CarrierId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'LteCqi', --4G_LONG
			   (SELECT AVG(sol.DownlinkSpeed) FROM AV_SiteOoklaLogs sol WHERE sol.siteid=sit.SiteId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'CADLSpeed', --4G_AVG_DL
			   (SELECT MAX(sol.DownlinkSpeed) FROM AV_SiteOoklaLogs sol WHERE sol.siteid=sit.SiteId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'CAULSpeed', --4G_AVG_UL
			   sit.DistanceFromSite, sit.AngleToSite, sit.FtpStatus, sit.PingHost,sit.LatencyRate, sit.PingIterations, 
			   (SELECT AVG(sol.Latency) FROM AV_SiteOoklaLogs sol WHERE sol.siteid=sit.SiteId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'PingMinResult', --4G_AVG_PING
			   sit.PingMaxResult,sit.CcwHoInterruptionTime,
			   CASE WHEN sit.City='Birmingham' THEN ISNULL(sit.OoklaPingResult,0)
			   ELSE (CASE WHEN sit.NetworkMode='NR' THEN (SELECT AVG(sol.Latency) FROM AV_SiteOoklaLogs sol WHERE sol.siteid=sit.SiteId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='NR') ELSE ISNULL(sit.PingAverageResult,0) END)
			   END 'PingAverageResult', --5G_AVG_PING
			   sit.PingStatus,sit.DownlinkRate, 
			   (SELECT MAX(sol.DownlinkSpeed) FROM AV_SiteOoklaLogs sol WHERE sol.siteid=sit.SiteId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'DownlinkMinResult', --4G_MAX_DL

			   CASE WHEN sit.City='Birmingham' THEN ISNULL(sit.OoklaDownlinkResult,0)
			   ELSE (CASE WHEN sit.NetworkMode='NR' THEN (SELECT MAX(sol.DownlinkSpeed) FROM AV_SiteOoklaLogs sol WHERE sol.siteid=sit.SiteId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='NR') ELSE ISNULL(sit.DownlinkMaxResult,0) END)
			   END 'DownlinkMaxResult', --5G_MAX_DL
			   CASE WHEN sit.City='Birmingham' THEN ISNULL(sit.OoklaDownlinkResult,0)
			   ELSE (CASE WHEN sit.NetworkMode='NR' THEN (SELECT AVG(sol.DownlinkSpeed) FROM AV_SiteOoklaLogs sol WHERE sol.siteid=sit.SiteId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='NR') ELSE ISNULL(sit.DownlinkAvgResult,0) END)
			   END 'DownlinkAvgResult',  --5G_AVG_DL
			   sit.DownlinkStatus, sit.UplinkRate,
			   (SELECT MAX(sol.DownlinkSpeed) FROM AV_SiteOoklaLogs sol WHERE sol.siteid=sit.SiteId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'UplinkMinResult', --4G_MAX_UL
			   
			   
			   CASE WHEN sit.City='Birmingham' THEN ISNULL(sit.OoklaUplinkResult,0)
			   ELSE (CASE WHEN sit.NetworkMode='NR' THEN (SELECT MAX(sol.UplinkSpeed) FROM AV_SiteOoklaLogs sol WHERE sol.siteid=sit.SiteId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='NR') ELSE ISNULL(sit.UplinkMaxResult,0) END)
			   END 'UplinkMaxResult', --5G_MAX_UL
			   CASE WHEN sit.City='Birmingham' THEN ISNULL(sit.OoklaUplinkResult,0)
			   ELSE (CASE WHEN sit.NetworkMode='NR' THEN (SELECT AVG(sol.UplinkSpeed) FROM AV_SiteOoklaLogs sol WHERE sol.siteid=sit.SiteId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='NR') ELSE ISNULL(sit.UplinkAvgResult,0) END)
			   END 'UplinkAvgResult', --5G_AVG_UL
			   --CASE WHEN sit.City='Birmingham' THEN ISNULL(sit.OoklaUplinkResult,0) ELSE ISNULL(sit.UplinkMaxResult,0) END 'UplinkMaxResult',
			   
			   --CASE WHEN ISNULL(sit.UplinkMaxResult,0)>=ISNULL(sit.OoklaUplinkResult,0) THEN ISNULL(sit.UplinkMaxResult,0) ELSE ISNULL(sit.OoklaUplinkResult,0) END 'UplinkMaxResult',
			   --sit.UplinkAvgResult,
			   sit.callSetupTime,sit.IntraHOInteruptTime,sit.IntreHOInteruptTime,
			   --(SELECT AVG(sol.LteRsnr) FROM AV_SiteTestLog sol WHERE sol.siteid=sit.SiteId AND sol.NetworkModeId=sit.NetworkModeId AND sol.BandId=sit.BandId AND sol.CarrierId=sit.CarrierId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE' AND sol.LteRsnr<>0) 'PhyDLSpeedMin', --4G_RSNR
			   (SELECT TOP 1 sol.RSNR FROM AV_SiteOoklalogs sol WHERE sol.siteid=sit.SiteId AND sol.NetworkModeId=sit.NetworkModeId AND sol.BandId=sit.BandId AND sol.CarrierId=sit.CarrierId AND sol.sectorid=sit.sectorid AND sol.NetworkMode='LTE') 'PhyDLSpeedMin', --4G_RSNR
			   
			   sit.PhyDLSpeedMax,sit.PhyDLSpeedAvg,sit.PhyULSpeedMin,sit.PhyULSpeedMax,sit.PhyULSpeedAvg,
			   sit.UplinkStatus, sit.ConnectionSetupTime, sit.ConnectionSetupStatus,sit.MoMtCallNo, sit.MoMtCallDuration, sit.MoStatus, sit.MtStatus,
			   sit.VMoMtCallno, sit.VMoMtCallDuration, sit.VMoStatus, sit.VMtStatus,sit.CwHandoverStatus, sit.Ccwhandoverstatus, sit.FtpServerIp,
			   sit.FtpServerPort, sit.FtpServerPath, sit.FtpDownlinkFile,sit.StationaryTestFilePath, sit.CwTestFilePath, sit.CcwTestFilePath,
			   sit.OoklaTestFilePath,sit.OoklaPingResult, sit.OoklaDownlinkResult,sit.OoklaUplinkResult,cli.Logo 'ClientLogo',ven.Logo 'VendorLogo',sit.ICwHandoverStatus,sit.ICcwhandoverstatus,
			   ast.SiteName, stt.DefinationName 'SiteType', stc.DefinationName 'SiteClass', sts.KeyCode 'LayerStatus',sec.RFHeight,sec.ETilt,sec.MTilt,sec.CellId,sec.BandWidth, CAST(ast.SubmittedOn AS DATE) 'SiteUploadDate','L700' 'Project',			   
			   cli.ClientPrefix,ast.SiteAddress, sit.SMoStatus, sit.SMtStatus, sit.CarrierAggregationStatus, sit.E911Status, scc.SectorColor, sec.MRBTS,
			   sec.SectorLatitude,sec.SectorLongitude, 
			   (SELECT COUNT(s.SrId) FROM AV_SiteOoklaLogs s WHERE s.SiteId=sit.SiteId AND s.NetworkModeId=sit.NetworkModeId AND s.BandId=sit.BandId AND s.CarrierId=sit.CarrierId AND s.SectorId=sit.SectorId AND s.NetworkMode='LTE') 'MinConSetupTime', --4G_TEST_COUNT
			   (SELECT DATEDIFF(minute,MIN(st.[TimeStamp]),MAX(st.[TimeStamp])) FROM AV_SiteTestLog AS st WHERE st.SiteId=sit.SiteId AND st.NetworkModeId=sit.NetworkModeId AND st.BandId=sit.BandId AND st.CarrierId=sit.CarrierId AND st.SectorId=sit.SectorId AND st.TestType IN('Ookla') AND st.NetworkMode='LTE') 'MaxConSetupTime', --4G_TEST_DURATION
			   sit.AvgConSetupTime, sit.HoInterruptionTime,
			   sit.MimoStatus,sit.MimoTestFilePath,sit.SpeedTestFilePath,sit.CaActiveTestFilePath,
			   sit.CaDeavticeTestFilePath,sit.CaSpeedTestFilePath,sit.LaaSpeedTestFilePath,sit.LaaSmTestFilePath,sit.PhyDLStatus,sit.PhyULStatus,
			   (SELECT COUNT(s.SrId) FROM AV_SiteOoklaLogs s WHERE s.SiteId=sit.SiteId AND s.NetworkModeId=sit.NetworkModeId AND s.BandId=sit.BandId AND s.CarrierId=sit.CarrierId AND s.SectorId=sit.SectorId AND s.NetworkMode='NR') 'TestCount', --5G_TEST_COUNT
			   (SELECT DATEDIFF(minute,MIN(st.[TimeStamp]),MAX(st.[TimeStamp])) FROM AV_SiteTestLog AS st WHERE st.SiteId=sit.SiteId AND st.NetworkModeId=sit.NetworkModeId AND st.BandId=sit.BandId AND st.CarrierId=sit.CarrierId AND st.SectorId=sit.SectorId AND st.TestType IN('Ookla') AND st.NetworkMode='NR') 'TestDuration' --5G_TEST_DURATION			   
		  from AV_SiteTestSummary sit 
		  INNER JOIN AV_Sites ast on ast.SiteId=sit.SiteId		  
		  INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=sit.SiteId AND anls.NetworkModeId=sit.NetworkModeId AND anls.BandId=sit.BandId AND anls.CarrierId=sit.CarrierId
		  INNER JOIN AV_Sectors AS sec ON sec.SiteId=sit.SiteId AND sec.NetworkModeId=sit.NetworkModeId AND sec.BandId=sit.BandId AND sec.CarrierId=sit.CarrierId AND sec.SectorId=sit.SectorId
		  --LEFT OUTER JOIN AV_SiteOoklaLogs AS sol ON sol.SiteId=sit.SiteId AND sol.NetworkModeId=sit.NetworkModeId AND sol.BandId=sit.BandId AND sol.CarrierId=sit.CarrierId AND sol.SectorId=sit.SectorId
		inner join AD_Clients cli on cli.ClientId=sit.ClientId 
		left join AD_Clients ven on ven.ClientId=cli.PClientId
		LEFT OUTER JOIN AD_Definations stt on stt.DefinationId=ast.SiteTypeId
		LEFT OUTER JOIN AD_Definations stc on stc.DefinationId=ast.SiteClassId
		INNER JOIN AD_Definations sts on sts.DefinationId=anls.Status
		INNER JOIN AV_SectorColors AS scc ON scc.ScopeId=sit.ScopeId AND scc.SectorCode=sit.Sector
		--LEFT OUTER JOIN AV_FloorPlan AS afp ON afp.SiteId=sit.SiteId
		where sit.SiteId=@SiteId and sit.BandId=@BandId and sit.CarrierId=@Carrier and sit.NetworkModeId=@NetworkMode
		AND sec.isActive=1
	) rpt
	ORDER BY rpt.NetworkModeId,rpt.BandId,rpt.CarrierId,
	CASE WHEN rpt.Sector='Alpha' THEN 1
		 WHEN rpt.Sector='Beta' THEN 2
		 WHEN rpt.Sector='Gamma' THEN 3
		 WHEN rpt.Sector='Delta' THEN 4
		 WHEN rpt.Sector='Epsilon' THEN 5
		 WHEN rpt.Sector='DiGamma' THEN 6
		 WHEN rpt.Sector='Iota' THEN 7
	END
	   
	   SELECT tt.PCI INTO #tmpPCI FROM AV_Sectors AS tt	   
	   WHERE tt.SiteId =@SiteId and  tt.BandId=@BandId and tt.CarrierId=CONVERT(numeric(18,0),@Carrier) and tt.NetworkModeId=CONVERT(numeric(18,0),@NetworkMode)
	   
	   select * from Av_TestLocation tt
	   where  tt.SiteId =@SiteId and  tt.BandId=@BandId and tt.CarrierId=CONVERT(numeric(18,0),@Carrier) and tt.NetworkModeId=CONVERT(numeric(18,0),@NetworkMode)
	   AND tt.TestType NOT IN('CW','CCW','UDPDL','UDPUL','CA_DL','CA_UL')
	   UNION ALL	   
	    SELECT Row_Number()  OVER(Partition By SectorId order by SectorId )TestLocationId, SiteId, NetworkModeId, BandId, CarrierId, ScopeId, TestCategory, TestType, TestResult, 
	   Latitude, Longitude, SignalStrength, SignalQuality, SignalNoiseRatio, TestComments, TestDate, TestStatus, TestKPI,Dense_Rank() Over(order by SectorId) SectorId, 
                  SourceCell, TargetCell, PRBPercent, MCS, RB, Modulation, ModPercent, TM, RI, PDSCH, CAPDSCH, PUSCH, CAPU
FROM     AV_TestLocation
 tt where  tt.SiteId =@SiteId and  tt.BandId=@BandId and tt.CarrierId=CONVERT(numeric(18,0),@Carrier) and tt.NetworkModeId=CONVERT(numeric(18,0),@NetworkMode)
	   AND tt.TestType IN('UDPDL')
	   UNION ALL	   
	    SELECT Row_Number()  OVER(Partition By SectorId order by SectorId )TestLocationId, SiteId, NetworkModeId, BandId, CarrierId, ScopeId, TestCategory, TestType, TestResult, 
	   Latitude, Longitude, SignalStrength, SignalQuality, SignalNoiseRatio, TestComments, TestDate, TestStatus, TestKPI,Dense_Rank() Over(order by SectorId) SectorId, 
                  SourceCell, TargetCell, PRBPercent, MCS, RB, Modulation, ModPercent, TM, RI, PDSCH, CAPDSCH, PUSCH, CAPU
FROM     AV_TestLocation
 tt where  tt.SiteId =@SiteId and  tt.BandId=@BandId and tt.CarrierId=CONVERT(numeric(18,0),@Carrier) and tt.NetworkModeId=CONVERT(numeric(18,0),@NetworkMode)
	   AND tt.TestType IN('UDPUL')
	   UNION ALL	   
	    SELECT Row_Number()  OVER(Partition By SectorId order by SectorId )TestLocationId, SiteId, NetworkModeId, BandId, CarrierId, ScopeId, TestCategory, TestType, TestResult, 
	   Latitude, Longitude, SignalStrength, SignalQuality, SignalNoiseRatio, TestComments, TestDate, TestStatus, TestKPI,Dense_Rank() Over(order by SectorId) SectorId, 
                  SourceCell, TargetCell, PRBPercent, MCS, RB, Modulation, ModPercent, TM, RI, PDSCH, CAPDSCH, PUSCH, CAPU
FROM     AV_TestLocation
 tt where  tt.SiteId =@SiteId and  tt.BandId=@BandId and tt.CarrierId=CONVERT(numeric(18,0),@Carrier) and tt.NetworkModeId=CONVERT(numeric(18,0),@NetworkMode)
	   AND tt.TestType IN('CA_DL')
	   UNION ALL	   
	    SELECT Row_Number()  OVER(Partition By SectorId order by SectorId )TestLocationId, SiteId, NetworkModeId, BandId, CarrierId, ScopeId, TestCategory, TestType, TestResult, 
	   Latitude, Longitude, SignalStrength, SignalQuality, SignalNoiseRatio, TestComments, TestDate, TestStatus, TestKPI,Dense_Rank() Over(order by SectorId) SectorId, 
                  SourceCell, TargetCell, PRBPercent, MCS, RB, Modulation, ModPercent, TM, RI, PDSCH, CAPDSCH, PUSCH, CAPU
FROM     AV_TestLocation
 tt where  tt.SiteId =@SiteId and  tt.BandId=@BandId and tt.CarrierId=CONVERT(numeric(18,0),@Carrier) and tt.NetworkModeId=CONVERT(numeric(18,0),@NetworkMode)
	   AND tt.TestType IN('CA_UL')
	   UNION ALL
	   select tt.TestLocationId, tt.SiteId, tt.NetworkModeId, tt.BandId,
	          tt.CarrierId, tt.ScopeId, tt.TestCategory, tt.TestType,
	          ROUND(tt.TestResult*1000,2) 'TestResult', tt.Latitude, tt.Longitude, tt.SignalStrength,
	          tt.SignalQuality, tt.SignalNoiseRatio, tt.TestComments, tt.TestDate,
	          tt.TestStatus, tt.TestKPI, tt.SectorId, tt.SourceCell, tt.TargetCell, tt.PRBPercent,
	          tt.MCS, tt.RB, tt.Modulation, tt.ModPercent, tt.TM, tt.RI, tt.PDSCH,
	          tt.CAPDSCH, tt.PUSCH, tt.CAPU
	     from Av_TestLocation tt
	   where  tt.SiteId =@SiteId and  tt.BandId=@BandId and tt.CarrierId=CONVERT(numeric(18,0),@Carrier) and tt.NetworkModeId=CONVERT(numeric(18,0),@NetworkMode)
	   AND tt.TestType IN('CW','CCW')
	   AND (tt.SourceCell IN(SELECT PCI FROM #tmpPCI) AND tt.TargetCell IN(SELECT PCI FROM #tmpPCI))
	   ORDER BY tt.SectorId
	END
	ELSE IF @Scope='NI'
		BEGIN
		SELECT rpt.SummaryId, rpt.ClientId, rpt.RegionId, rpt.Region, rpt.CityId,rpt.City  , rpt.ClusterId, rpt.Cluster, rpt.SiteId, 
		  --CASE WHEN charindex('_',Site)=0 THEN Site ELSE LEFT(site,charindex('_',Site)-1) END 'Site',
		  CASE WHEN charindex('_',Site)=0 AND charindex('-',Site)=0 THEN Site WHEN charindex('-',Site)>0 THEN LEFT(site,charindex('-',Site)-1) ELSE LEFT(site,charindex('_',Site)-1) END 'Site',
	       rpt.Latitude, rpt.Longitude, rpt.SiteScheduleDate, rpt.SectorId, rpt.Sector,rpt.PhyDLStatus,rpt.PhyULStatus,
	       --CASE WHEN rpt.City='Chicago' THEN 
	       --		(CASE	WHEN rpt.Band='LTE 700' THEN 'D'
	       --				WHEN rpt.Band='LTE 1900' THEN 'B'
	       --				WHEN rpt.Band='LTE 2100' THEN 'L'
	       --		ELSE ''
	       --		END)+rpt.[Site]+
	       --		(
	       --			CASE WHEN rpt.Sector='Alpha' THEN '11'
	       --				 WHEN rpt.Sector='Beta' THEN '21'
	       --				 WHEN rpt.Sector='Gamma' THEN '31'
	       --				 WHEN rpt.Sector='Delta' THEN '41'
	       --				 WHEN rpt.Sector='Epsilon' THEN '51'
	       --				 WHEN rpt.Sector='DiGamma' THEN '61'
	       --				 WHEN rpt.Sector='Iota' THEN '71'
	       --			ELSE '' 
	       --			END
	       --		)
	       --ELSE	rpt.Sector
	       --END 'Sector',
	       ISNULL(rpt.TestLatitude,0) 'TestLatitude', 
	       ISNULL(rpt.TestLongitude,0) 'TestLongitude', rpt.ScopeId, rpt.Scope,
	       rpt.NetworkModeId, rpt.NetworkMode, rpt.BandId,
			rpt.Band,rpt.callSetupTime,rpt.IntraHOInteruptTime,rpt.IntreHOInteruptTime,rpt.PhyDLSpeedMin,rpt.PhyDLSpeedMax,rpt.PhyDLSpeedAvg,rpt.PhyULSpeedMin,rpt.PhyULSpeedMax,rpt.PhyULSpeedAvg,
	       rpt.CarrierId,rpt.Carrier, rpt.Antenna, rpt.Azimuth, rpt.PciId, rpt.BeamWidth,
	       ISNULL(rpt.GsmRssi,0) 'GsmRssi', ISNULL(rpt.GsmRxQual,0) 'GsmRxQual', ISNULL(rpt.WcdmaRssi,0) 'WcdmaRssi', ISNULL(rpt.WcdmaRscp,0) 'WcdmaRscp', 
	       ISNULL(rpt.WcdmaEcio,0) 'WcdmaEcio',ISNULL(rpt.LteRssi,0) 'LteRssi', ISNULL(rpt.LteRsrp,0) 'LteRsrp', ISNULL(rpt.LteRsrq,0) 'LteRsrq',
	       ISNULL(rpt.LteRsnr,0) 'LteRsnr', ISNULL(rpt.LteCqi,0) 'LteCqi',
	       ISNULL(rpt.DistanceFromSite,0) 'DistanceFromSite', ISNULL(rpt.AngleToSite,0) 'AngleToSite', rpt.FtpStatus, rpt.PingHost,rpt.LatencyRate, 
	       rpt.PingIterations, ISNULL(rpt.PingMinResult,0) 'PingMinResult',
	       ISNULL(rpt.PingMaxResult,0) 'PingMaxResult', ISNULL(rpt.PingAverageResult,0) 'PingAverageResult',
	       CASE WHEN ISNULL(rpt.PingAverageResult,0)>0 AND ISNULL(rpt.PingAverageResult,0)<=rpt.LatencyRate THEN CAST(1 AS BIT) WHEN ISNULL(rpt.PingAverageResult,0)>0 AND ISNULL(rpt.PingAverageResult,0)>rpt.LatencyRate THEN CAST(0 AS BIT) ELSE NULL END 'PingStatus',
	       rpt.DownlinkRate, ISNULL(rpt.DownlinkMinResult,0) 'DownlinkMinResult', ISNULL(rpt.DownlinkMaxResult,0) 'DownlinkMaxResult',
	       ISNULL(rpt.DownlinkAvgResult,0) 'DownlinkAvgResult',
	       CASE WHEN rpt.Scope='NI' THEN rpt.DownlinkStatus ELSE CASE WHEN ISNULL(rpt.DownlinkMaxResult,0)>0 AND ISNULL(rpt.DownlinkMaxResult,0)>=rpt.DownlinkRate THEN CAST(1 AS BIT) WHEN ISNULL(rpt.DownlinkMaxResult,0)>0 AND ISNULL(rpt.DownlinkMaxResult,0)<rpt.DownlinkRate THEN CAST(0 AS BIT) ELSE NULL END END 'DownlinkStatus',
	       rpt.UplinkRate,rpt.UplinkMinResult, rpt.UplinkMaxResult, rpt.UplinkAvgResult,
	       CASE WHEN ISNULL(rpt.UplinkMaxResult,0)>0 AND ISNULL(rpt.UplinkMaxResult,0)>=rpt.UplinkRate THEN CAST(1 AS BIT) WHEN ISNULL(rpt.UplinkMaxResult,0)>0 AND ISNULL(rpt.UplinkMaxResult,0)<rpt.UplinkRate THEN CAST(0 AS BIT) ELSE NULL END 'UplinkStatus',
	       rpt.ConnectionSetupTime, rpt.ConnectionSetupStatus,rpt.MoMtCallNo, rpt.MoMtCallDuration, rpt.MoStatus, rpt.MtStatus,
	       rpt.VMoMtCallno, rpt.VMoMtCallDuration, rpt.VMoStatus, rpt.VMtStatus,CAST(ISNULL(rpt.CwHandoverStatus,0) AS BIT) 'CwHandoverStatus', CAST(ISNULL(rpt.Ccwhandoverstatus,0) AS BIT) 'Ccwhandoverstatus',
	       rpt.FtpServerIp,
	       rpt.FtpServerPort, rpt.FtpServerPath, rpt.FtpDownlinkFile,rpt.StationaryTestFilePath, rpt.CwTestFilePath, rpt.CcwTestFilePath,
	       ISNULL(rpt.OoklaTestFilePath,0) 'OoklaTestFilePath', ISNULL(rpt.OoklaPingResult,0) 'OoklaPingResult', ISNULL(rpt.OoklaDownlinkResult,0) 'OoklaDownlinkResult',
	       ISNULL(rpt.OoklaUplinkResult,0) 'OoklaUplinkResult',
	       rpt.ClientLogo, rpt.VendorLogo,
		   --CASE WHEN ISNULL(rpt.ICwHandoverStatus,0)=0 THEN CAST(0 AS BIT) ELSE CAST(ISNULL(rpt.ICwHandoverStatus,0) AS BIT) END 'ICwHandoverStatus',
		   CASE WHEN ISNULL(rpt.ICwHandoverStatus,0) IS NULL AND ISNULL(rpt.ICcwhandoverstatus,0) IS NULL THEN NULL
				WHEN ISNULL(rpt.ICwHandoverStatus,0)=CAST(0 as bit) AND ISNULL(rpt.ICcwhandoverstatus,0)=CAST(0 as bit) THEN NULL
				WHEN ISNULL(rpt.ICwHandoverStatus,0)=CAST(1 as bit) AND ISNULL(rpt.ICcwhandoverstatus,0)=CAST(0 as bit) THEN CAST(1 as bit)
				WHEN ISNULL(rpt.ICwHandoverStatus,0)=CAST(0 as bit) AND ISNULL(rpt.ICcwhandoverstatus,0)=CAST(1 as bit) THEN CAST(1 as bit)
				WHEN ISNULL(rpt.ICwHandoverStatus,0)=CAST(1 as bit) AND ISNULL(rpt.ICcwhandoverstatus,0)=CAST(1 as bit) THEN CAST(1 as bit)
				ELSE NULL
		   END 'ICwHandoverStatus',
		   CAST(ISNULL(rpt.ICcwhandoverstatus,0) AS BIT) 'ICcwhandoverstatus',Site 'ActualSiteCode',
		   rpt.SiteName, rpt.SiteType, rpt.SiteClass, rpt.LayerStatus,rpt.RFHeight,rpt.ETilt,rpt.MTilt,rpt.CellId,rpt.BandWidth,rpt.SiteUploadDate,@BandNo 'BandNo',
		   CASE WHEN ISNULL(rpt.CwHandoverStatus,0) IS NULL AND ISNULL(rpt.Ccwhandoverstatus,0) IS NULL THEN NULL
				WHEN ISNULL(rpt.CwHandoverStatus,0)=CAST(0 as bit) AND ISNULL(rpt.Ccwhandoverstatus,0)=CAST(0 as bit) THEN NULL
				WHEN ISNULL(rpt.CwHandoverStatus,0)=CAST(1 as bit) AND ISNULL(rpt.Ccwhandoverstatus,0)=CAST(0 as bit) THEN CAST(1 as bit)
				WHEN ISNULL(rpt.CwHandoverStatus,0)=CAST(0 as bit) AND ISNULL(rpt.Ccwhandoverstatus,0)=CAST(1 as bit) THEN CAST(1 as bit)
				WHEN ISNULL(rpt.CwHandoverStatus,0)=CAST(1 as bit) AND ISNULL(rpt.Ccwhandoverstatus,0)=CAST(1 as bit) THEN CAST(1 as bit)
				ELSE NULL
		   END 'MobHandoverStatus', rpt.Project, rpt.ClientPrefix, rpt.SiteAddress, rpt.SMoStatus, rpt.SMtStatus, rpt.CarrierAggregationStatus, rpt.E911Status, rpt.SectorColor, rpt.MRBTS,
			   rpt.SectorLatitude,rpt.SectorLongitude,CAST (1 as bit) IRATHandover,
		       '0' 'CellId','0' 'TCH',0 'SampleCount',CAST(1 AS BIT) 'FastReturnStatus',0.00 'CoverageDistance'
	FROM
	(
		select DISTINCT sit.SummaryId, sit.ClientId, sit.RegionId, sit.Region, sit.CityId,sit.City, sit.ClusterId, sit.Cluster, sit.SiteId, sit.[Site],
			   -- sit.Latitude, sit.Longitude, 
			   sec.SectorLatitude 'Latitude', sec.SectorLongitude 'Longitude',
			   CASE WHEN anls.DriveCompletedOn IS NOT NULL THEN anls.DriveCompletedOn ELSE GETDATE() END  'SiteScheduleDate', sit.SectorId,sit.Sector, sit.TestLatitude, sit.TestLongitude, sit.ScopeId, sit.Scope,
			   sit.NetworkModeId, sit.NetworkMode, sit.BandId, sit.Band, sit.CarrierId,sit.Carrier, sit.Antenna, sit.Azimuth, sit.PciId, sit.BeamWidth,
			   CASE WHEN sit.City='Birmingham' AND sit.NetworkMode='GSM' AND ISNULL(sit.OoklaRssi,0)<>0 THEN ISNULL(sit.OoklaRssi,0) ELSE sit.GsmRssi END 'GsmRssi',
			   CASE WHEN sit.City='Birmingham' AND sit.NetworkMode='GSM' AND ISNULL(sit.OoklaSinr,0)<>0 THEN ISNULL(sit.OoklaSinr,0) ELSE sit.GsmRxQual END 'GsmRxQual',
			   sit.WcdmaRssi,
			   CASE WHEN sit.City='Birmingham' AND sit.NetworkMode='WCDMA' AND ISNULL(sit.OoklaRssi,0)<>0 THEN ISNULL(sit.OoklaRssi,0) ELSE sit.WcdmaRscp END 'WcdmaRscp',
			   CASE WHEN sit.City='Birmingham' AND sit.NetworkMode='WCDMA' AND ISNULL(sit.OoklaSinr,0)<>0 THEN ISNULL(sit.OoklaSinr,0) ELSE sit.WcdmaEcio END 'WcdmaEcio', 
			   
			   sit.LteRssi,
			   CASE WHEN sit.City='Birmingham' AND sit.NetworkMode='LTE' AND ISNULL(sit.OoklaRssi,0)<>0 THEN ISNULL(sit.OoklaRssi,0) ELSE sit.LteRsrp END 'LteRsrp',			   
			   sit.LteRsrq,
			   CASE WHEN sit.City='Birmingham' AND sit.NetworkMode='LTE' AND ISNULL(sit.OoklaSinr,0)<>0 THEN ISNULL(sit.OoklaSinr,0) ELSE sit.LteRsnr END 'LteRsnr',
			   sit.LteCqi,
			   sit.DistanceFromSite, sit.AngleToSite, sit.FtpStatus, sit.PingHost,sit.LatencyRate, sit.PingIterations, sit.PingMinResult,
			   sit.PingMaxResult,
			   CASE WHEN sit.City='Birmingham' THEN ISNULL(sit.OoklaPingResult,0)
			   ELSE
			   	   CASE WHEN (ISNULL(sit.OoklaPingResult,0)>0 AND ISNULL(sit.PingAverageResult,0)>0) AND ISNULL(sit.PingAverageResult,0)<=ISNULL(sit.OoklaPingResult,0) THEN ISNULL(sit.PingAverageResult,0)
				   WHEN (ISNULL(sit.OoklaPingResult,0)>0 AND ISNULL(sit.PingAverageResult,0)>0) AND ISNULL(sit.PingAverageResult,0)>ISNULL(sit.OoklaPingResult,0) THEN ISNULL(sit.OoklaPingResult,0)
				   WHEN ISNULL(sit.OoklaPingResult,0)=0 AND ISNULL(sit.PingAverageResult,0)>0 THEN ISNULL(sit.PingAverageResult,0)
				   WHEN ISNULL(sit.OoklaPingResult,0)>0 AND ISNULL(sit.PingAverageResult,0)=0 THEN ISNULL(sit.OoklaPingResult,0)
				   WHEN ISNULL(sit.OoklaPingResult,0)=0 AND ISNULL(sit.PingAverageResult,0)=0 THEN 0
				   ELSE ISNULL(sit.OoklaPingResult,0) END
			   END 'PingAverageResult',
			   sit.PingStatus,sit.DownlinkRate, sit.DownlinkMinResult,

			   CASE WHEN sit.City='Birmingham' THEN ISNULL(sit.OoklaDownlinkResult,0)
			   ELSE
				   CASE WHEN (ISNULL(sit.OoklaDownlinkResult,0)>0 AND ISNULL(sit.DownlinkMaxResult,0)>0) AND ISNULL(sit.DownlinkMaxResult,0)>=ISNULL(sit.OoklaDownlinkResult,0) THEN ISNULL(sit.DownlinkMaxResult,0)
				   WHEN (ISNULL(sit.OoklaDownlinkResult,0)>0 AND ISNULL(sit.DownlinkMaxResult,0)>0) AND ISNULL(sit.DownlinkMaxResult,0)<ISNULL(sit.OoklaDownlinkResult,0) THEN ISNULL(sit.OoklaDownlinkResult,0)
				   WHEN (ISNULL(sit.OoklaDownlinkResult,0)=0 AND ISNULL(sit.DownlinkMaxResult,0)>0) THEN ISNULL(sit.DownlinkMaxResult,0)
				   WHEN (ISNULL(sit.OoklaDownlinkResult,0)>0 AND ISNULL(sit.DownlinkMaxResult,0)=0) THEN ISNULL(sit.OoklaDownlinkResult,0)
				   WHEN (ISNULL(sit.OoklaDownlinkResult,0)=0 AND ISNULL(sit.DownlinkMaxResult,0)=0) THEN 0
				   ELSE ISNULL(sit.OoklaDownlinkResult,0) END
			   END 'DownlinkMaxResult',
			   sit.DownlinkAvgResult, sit.DownlinkStatus, sit.UplinkRate,sit.UplinkMinResult, 
			   
			   CASE WHEN sit.City='Birmingham' THEN ISNULL(sit.OoklaUplinkResult,0)
			   ELSE
				   CASE WHEN (ISNULL(sit.OoklaUplinkResult,0)>0 AND ISNULL(sit.UplinkMaxResult,0)>0) AND ISNULL(sit.UplinkMaxResult,0)>=ISNULL(sit.OoklaUplinkResult,0) THEN ISNULL(sit.UplinkMaxResult,0)
				   WHEN (ISNULL(sit.OoklaUplinkResult,0)>0 AND ISNULL(sit.UplinkMaxResult,0)>0) AND ISNULL(sit.UplinkMaxResult,0)<ISNULL(sit.OoklaUplinkResult,0) THEN ISNULL(sit.OoklaUplinkResult,0)
				   WHEN (ISNULL(sit.OoklaUplinkResult,0)=0 AND ISNULL(sit.UplinkMaxResult,0)>0) THEN ISNULL(sit.UplinkMaxResult,0)
				   WHEN (ISNULL(sit.OoklaUplinkResult,0)>0 AND ISNULL(sit.UplinkMaxResult,0)=0) THEN ISNULL(sit.OoklaUplinkResult,0)
				   WHEN (ISNULL(sit.OoklaUplinkResult,0)=0 AND ISNULL(sit.UplinkMaxResult,0)=0) THEN 0
				   ELSE ISNULL(sit.OoklaUplinkResult,0) END
			   END 'UplinkMaxResult',
			   
			   --CASE WHEN ISNULL(sit.UplinkMaxResult,0)>=ISNULL(sit.OoklaUplinkResult,0) THEN ISNULL(sit.UplinkMaxResult,0) ELSE ISNULL(sit.OoklaUplinkResult,0) END 'UplinkMaxResult',
			   sit.UplinkAvgResult,sit.callSetupTime,sit.IntraHOInteruptTime,sit.IntreHOInteruptTime,sit.PhyDLSpeedMin,sit.PhyDLSpeedMax,sit.PhyDLSpeedAvg,sit.PhyULSpeedMin,sit.PhyULSpeedMax,sit.PhyULSpeedAvg,
			   sit.UplinkStatus, sit.ConnectionSetupTime, sit.ConnectionSetupStatus,sit.MoMtCallNo, sit.MoMtCallDuration, sit.MoStatus, sit.MtStatus,
			   sit.VMoMtCallno, sit.VMoMtCallDuration, sit.VMoStatus, sit.VMtStatus,sit.CwHandoverStatus, sit.Ccwhandoverstatus, sit.FtpServerIp,
			   sit.FtpServerPort, sit.FtpServerPath, sit.FtpDownlinkFile,sit.StationaryTestFilePath, sit.CwTestFilePath, sit.CcwTestFilePath,sit.PhyDLStatus,sit.PhyULStatus,
			   sit.OoklaTestFilePath,sit.OoklaPingResult, sit.OoklaDownlinkResult,sit.OoklaUplinkResult,cli.Logo 'ClientLogo',ven.Logo 'VendorLogo',sit.ICwHandoverStatus,sit.ICcwhandoverstatus,
			   ast.SiteName, stt.DefinationName 'SiteType', stc.DefinationName 'SiteClass', sts.KeyCode 'LayerStatus',sec.RFHeight,sec.ETilt,sec.MTilt,sec.CellId,sec.BandWidth, CAST(ast.SubmittedOn AS DATE) 'SiteUploadDate','L700' 'Project',			   
			   cli.ClientPrefix,ast.SiteAddress, sit.SMoStatus, sit.SMtStatus, sit.CarrierAggregationStatus, sit.E911Status, scc.SectorColor, sec.MRBTS,
			   sec.SectorLatitude,sec.SectorLongitude
		  from AV_SiteTestSummary sit 
		  INNER JOIN AV_Sites ast on ast.SiteId=sit.SiteId		  
		  INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=sit.SiteId AND anls.NetworkModeId=sit.NetworkModeId AND anls.BandId=sit.BandId AND anls.CarrierId=sit.CarrierId
		  INNER JOIN AV_Sectors AS sec ON sec.SiteId=sit.SiteId AND sec.NetworkModeId=sit.NetworkModeId AND sec.BandId=sit.BandId AND sec.CarrierId=sit.CarrierId AND sec.SectorId=sit.SectorId
		inner join AD_Clients cli on cli.ClientId=sit.ClientId 
		left join AD_Clients ven on ven.ClientId=cli.PClientId
		LEFT OUTER JOIN AD_Definations stt on stt.DefinationId=ast.SiteTypeId
		LEFT OUTER JOIN AD_Definations stc on stc.DefinationId=ast.SiteClassId
		INNER JOIN AD_Definations sts on sts.DefinationId=anls.Status
		INNER JOIN AV_SectorColors AS scc ON scc.ScopeId=sit.ScopeId AND scc.SectorCode=sit.Sector
		where sit.SiteId=@SiteId --and sit.BandId=@BandId and sit.CarrierId=@Carrier and sit.NetworkModeId=@NetworkMode
		AND sec.isActive=1
	) rpt
	ORDER BY rpt.NetworkModeId,rpt.BandId,rpt.CarrierId,
	CASE WHEN rpt.Sector='Alpha' THEN 1
		 WHEN rpt.Sector='Beta' THEN 2
		 WHEN rpt.Sector='Gamma' THEN 3
		 WHEN rpt.Sector='Delta' THEN 4
		 WHEN rpt.Sector='Epsilon' THEN 5
		 WHEN rpt.Sector='DiGamma' THEN 6
		 WHEN rpt.Sector='Iota' THEN 7
	END
	select * from Av_TestLocation tt where  tt.SiteId =@SiteId and  tt.BandId=@BandId and tt.CarrierId=@Carrier  and tt.NetworkModeId=@NetworkMode
	END	
END