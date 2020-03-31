
CREATE PROCEDURE [dbo].[AV_GetNetworkLayerProcessed]
	@SiteId int,
	@BandId int,
	@Carrier nvarchar(50),
	@NetworkMode nvarchar(50)
AS
BEGIN
	SELECT rpt.SummaryId, rpt.ClientId, rpt.RegionId, rpt.Region, rpt.CityId,rpt.City  , rpt.ClusterId, rpt.Cluster, rpt.SiteId,		  
		  rpt.[Site],
	       rpt.Latitude, rpt.Longitude, rpt.SiteScheduleDate, rpt.SectorId,rpt.Sector, rpt.TestLatitude, rpt.TestLongitude, rpt.ScopeId, rpt.Scope,
	       rpt.NetworkModeId, rpt.NetworkMode, rpt.BandId, rpt.Band, rpt.CarrierId,rpt.Carrier, rpt.Antenna, rpt.Azimuth, rpt.PciId, rpt.BeamWidth,
	       rpt.GsmRssi, rpt.GsmRxQual, rpt.WcdmaRssi, rpt.WcdmaRscp, rpt.WcdmaEcio,rpt.LteRssi, rpt.LteRsrp, rpt.LteRsrq, rpt.LteRsnr, rpt.LteCqi,
	       rpt.DistanceFromSite, rpt.AngleToSite, rpt.FtpStatus, rpt.PingHost,rpt.LatencyRate, rpt.PingIterations, rpt.PingMinResult,
	       rpt.PingMaxResult, rpt.PingAverageResult,rpt.MimoStatus,rpt.SMoStatus,rpt.SMtStatus,rpt.MimoTestFilePath,rpt.SpeedTestFilePath,rpt.CaActiveTestFilePath,rpt.CaDeavticeTestFilePath,rpt.CaSpeedTestFilePath,rpt.LaaSpeedTestFilePath,rpt.LaaSmTestFilePath,
	       rpt.PingStatus,
	       rpt.DownlinkRate, rpt.DownlinkMinResult, rpt.DownlinkMaxResult,
	       rpt.DownlinkAvgResult,
	       rpt.DownlinkStatus,rpt.CADLSpeed,rpt.CAULSpeed,
	       rpt.UplinkRate,rpt.UplinkMinResult, rpt.UplinkMaxResult, rpt.UplinkAvgResult,
	       rpt.UplinkStatus,rpt.PhyDLStatus,rpt.PhyULStatus,
	       rpt.ConnectionSetupTime, rpt.ConnectionSetupStatus,rpt.MoMtCallNo, rpt.MoMtCallDuration, rpt.MoStatus, rpt.MtStatus,
	       rpt.VMoMtCallno, rpt.VMoMtCallDuration, rpt.VMoStatus, rpt.VMtStatus
	       ,CAST(ISNULL(rpt.CwHandoverStatus,0) AS BIT) 'CwHandoverStatus', CAST(ISNULL(rpt.Ccwhandoverstatus,0) AS BIT) 'Ccwhandoverstatus',
	       rpt.FtpServerIp,rpt.callSetupTime,rpt.IntraHOInteruptTime,rpt.IntreHOInteruptTime,rpt.PhyDLSpeedMin,rpt.PhyDLSpeedMax,rpt.PhyDLSpeedAvg,rpt.PhyULSpeedMin,rpt.PhyULSpeedMax,rpt.PhyULSpeedAvg,
	       rpt.FtpServerPort, rpt.FtpServerPath, rpt.FtpDownlinkFile,rpt.StationaryTestFilePath, rpt.CwTestFilePath, rpt.CcwTestFilePath,
	       rpt.OoklaTestFilePath, rpt.OoklaPingResult, rpt.OoklaDownlinkResult,rpt.OoklaUplinkResult, rpt.ClientLogo, rpt.VendorLogo,
		   CAST(ISNULL(rpt.ICwHandoverStatus,0) AS BIT) 'ICwHandoverStatus',		  
		   CAST(ISNULL(rpt.ICcwhandoverstatus,0) AS BIT) 'ICcwhandoverstatus',OoklaRssi,OoklaSinr,
		   CAST(CASE WHEN rpt.NetworkMode='GSM' THEN rpt.GsmRssi
				WHEN rpt.NetworkMode='WCDMA' THEN rpt.WcdmaRscp
				WHEN rpt.NetworkMode='LTE' THEN rpt.LteRsrp
		   ELSE 0
		   END AS FLOAT) 'TestRssi',
		   CAST(CASE WHEN rpt.NetworkMode='GSM' THEN rpt.GsmRxQual
				WHEN rpt.NetworkMode='WCDMA' THEN rpt.WcdmaEcio
				WHEN rpt.NetworkMode='LTE' THEN rpt.LteRsnr
		   ELSE 0
		   END AS FLOAT) 'TestSinr',rpt.E911Status,rpt.IsE911Performed
	FROM
	(
		select sit.SummaryId, sit.ClientId, sit.RegionId, sit.Region, sit.CityId,sit.City, sit.ClusterId, sit.Cluster, sit.SiteId, sit.[Site],
			   sit.Latitude, sit.Longitude, sit.SiteScheduleDate, sit.SectorId,sit.Sector, sit.TestLatitude, sit.TestLongitude, sit.ScopeId, sit.Scope,
			   sit.NetworkModeId, sit.NetworkMode, sit.BandId, sit.Band, sit.CarrierId,sit.Carrier, sit.Antenna, sit.Azimuth, sit.PciId, sit.BeamWidth,
			   sit.GsmRssi, sit.GsmRxQual, sit.WcdmaRssi, sit.WcdmaRscp, sit.WcdmaEcio,sit.LteRssi, sit.LteRsrp, sit.LteRsrq, sit.LteRsnr, sit.LteCqi,
			   sit.DistanceFromSite, sit.AngleToSite, sit.FtpStatus, sit.PingHost,sit.LatencyRate, sit.PingIterations, sit.PingMinResult,
			   sit.PingMaxResult,sit.MimoStatus,sit.MimoTestFilePath,
			   ISNULL(sit.PingAverageResult,0) 'PingAverageResult',
			   sit.PingStatus,sit.DownlinkRate, sit.DownlinkMinResult,sit.CADLSpeed,sit.CAULSpeed,
			   ISNULL(sit.DownlinkMaxResult,0) 'DownlinkMaxResult',
			   sit.DownlinkAvgResult, sit.DownlinkStatus, sit.UplinkRate,sit.UplinkMinResult, 
			   ISNULL(sit.UplinkMaxResult,0) 'UplinkMaxResult',
			   sit.UplinkAvgResult,sit.PhyDLStatus,sit.PhyULStatus,
			   sit.UplinkStatus, sit.ConnectionSetupTime, sit.ConnectionSetupStatus,sit.MoMtCallNo, sit.MoMtCallDuration, sit.MoStatus, sit.MtStatus,
			   sit.VMoMtCallno, sit.VMoMtCallDuration, sit.VMoStatus, sit.VMtStatus,sit.CwHandoverStatus, sit.Ccwhandoverstatus, sit.FtpServerIp,
			   sit.FtpServerPort, sit.FtpServerPath, sit.FtpDownlinkFile,sit.StationaryTestFilePath, sit.CwTestFilePath, sit.CcwTestFilePath,
			   sit.OoklaTestFilePath,ISNULL(sit.OoklaPingResult,0) 'OoklaPingResult', ISNULL(sit.OoklaDownlinkResult,0) 'OoklaDownlinkResult',ISNULL(sit.OoklaUplinkResult,0) 'OoklaUplinkResult',cli.Logo 'ClientLogo',ven.Logo 'VendorLogo',sit.ICwHandoverStatus,sit.ICcwhandoverstatus
			   ,OoklaRssi,OoklaSinr,sit.E911Status,sit.IsE911Performed,sit.SMoStatus,sit.SMtStatus,sit.SpeedTestFilePath,sit.CaActiveTestFilePath,sit.CaDeavticeTestFilePath,sit.CaSpeedTestFilePath,sit.LaaSpeedTestFilePath,sit.LaaSmTestFilePath,sit.callSetupTime,sit.IntraHOInteruptTime,sit.IntreHOInteruptTime,sit.PhyDLSpeedMin,sit.PhyDLSpeedMax,sit.PhyDLSpeedAvg,sit.PhyULSpeedMin,sit.PhyULSpeedMax,sit.PhyULSpeedAvg
			   
		  from AV_SiteTestSummary sit 
		inner join AD_Clients cli on cli.ClientId=sit.ClientId 
		left join AD_Clients ven on ven.ClientId=cli.PClientId
		where sit.SiteId=@SiteId and sit.BandId=@BandId and sit.Carrier=@Carrier and sit.NetworkMode=@NetworkMode
	) rpt
	ORDER BY rpt.NetworkModeId,rpt.BandId,rpt.CarrierId,rpt.ScopeId,
		CASE WHEN rpt.Sector='Alpha' THEN 1
			 WHEN rpt.Sector='Beta' THEN 2
			 WHEN rpt.Sector='Gamma' THEN 3
			 WHEN rpt.Sector='Delta' THEN 4
			 WHEN rpt.Sector='Epsilon' THEN 5
			 WHEN rpt.Sector='DiGamma' THEN 6
			 WHEN rpt.Sector='Iota' THEN 7
		END
	
END

--au01292d