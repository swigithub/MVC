-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- [dbo].[AV_GetNetLayerSummary] 11360,73,76,15,11
CREATE PROCEDURE [dbo].[AV_GetNetLayerSummary1]
	@SiteId int,
	@BandId int,
	@Carrier nvarchar(50),
	@NetworkMode nvarchar(50),
	@UserId NUMERIC(18,0)
AS
BEGIN
		SELECT rpt.SummaryId, rpt.ClientId, rpt.RegionId, rpt.Region, rpt.CityId,rpt.City  , rpt.ClusterId, rpt.Cluster, rpt.SiteId, 
		  --CASE WHEN charindex('_',Site)=0 THEN Site ELSE LEFT(site,charindex('_',Site)-1) END 'Site',
		  CASE WHEN charindex('_',Site)=0 AND charindex('-',Site)=0 THEN Site WHEN charindex('-',Site)>0 THEN LEFT(site,charindex('-',Site)-1) ELSE LEFT(site,charindex('_',Site)-1) END 'Site',
	       rpt.Latitude, rpt.Longitude, rpt.SiteScheduleDate, rpt.SectorId,rpt.Sector, ISNULL(rpt.TestLatitude,0) 'TestLatitude', 
	       ISNULL(rpt.TestLongitude,0) 'TestLongitude', rpt.ScopeId, rpt.Scope,
	       rpt.NetworkModeId, rpt.NetworkMode, rpt.BandId, rpt.Band, rpt.CarrierId,rpt.Carrier, rpt.Antenna, rpt.Azimuth, rpt.PciId, rpt.BeamWidth,
	       ISNULL(rpt.GsmRssi,0) 'GsmRssi', ISNULL(rpt.GsmRxQual,0) 'GsmRxQual', ISNULL(rpt.WcdmaRssi,0) 'WcdmaRssi', ISNULL(rpt.WcdmaRscp,0) 'WcdmaRscp', 
	       ISNULL(rpt.WcdmaEcio,0) 'WcdmaEcio',ISNULL(rpt.LteRssi,0) 'LteRssi', ISNULL(rpt.LteRsrp,0) 'LteRsrp', ISNULL(rpt.LteRsrq,0) 'LteRsrq',
	       ISNULL(rpt.LteRsnr,0) 'LteRsnr', ISNULL(rpt.LteCqi,0) 'LteCqi',
	       ISNULL(rpt.DistanceFromSite,0) 'DistanceFromSite', ISNULL(rpt.AngleToSite,0) 'AngleToSite', rpt.FtpStatus, rpt.PingHost,rpt.LatencyRate, 
	       rpt.PingIterations, ISNULL(rpt.PingMinResult,0) 'PingMinResult',
	       ISNULL(rpt.PingMaxResult,0) 'PingMaxResult', ISNULL(rpt.PingAverageResult,0) 'PingAverageResult',
	       CASE WHEN ISNULL(rpt.PingAverageResult,0)>0 AND ISNULL(rpt.PingAverageResult,0)<=rpt.LatencyRate THEN CAST(1 AS BIT) WHEN ISNULL(rpt.PingAverageResult,0)>0 AND ISNULL(rpt.PingAverageResult,0)>rpt.LatencyRate THEN CAST(0 AS BIT) ELSE NULL END 'PingStatus',
	       rpt.DownlinkRate, ISNULL(rpt.DownlinkMinResult,0) 'DownlinkMinResult', ISNULL(rpt.DownlinkMaxResult,0) 'DownlinkMaxResult',
	       ISNULL(rpt.DownlinkAvgResult,0) 'DownlinkAvgResult',
	       CASE WHEN ISNULL(rpt.DownlinkMaxResult,0)>0 AND ISNULL(rpt.DownlinkMaxResult,0)>=rpt.DownlinkRate THEN CAST(1 AS BIT) WHEN ISNULL(rpt.DownlinkMaxResult,0)>0 AND ISNULL(rpt.DownlinkMaxResult,0)<rpt.DownlinkRate THEN CAST(0 AS BIT) ELSE NULL END 'DownlinkStatus',
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
		   CAST(ISNULL(rpt.ICcwhandoverstatus,0) AS BIT) 'ICcwhandoverstatus'
	FROM
	(
		select DISTINCT sit.SummaryId, sit.ClientId, sit.RegionId, sit.Region, sit.CityId,sit.City, sit.ClusterId, sit.Cluster, sit.SiteId, sit.[Site],
			   sit.Latitude, sit.Longitude, CASE WHEN anls.DriveCompletedOn IS NOT NULL THEN anls.DriveCompletedOn ELSE GETDATE() END  'SiteScheduleDate', sit.SectorId,sit.Sector, sit.TestLatitude, sit.TestLongitude, sit.ScopeId, sit.Scope,
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
			   sit.UplinkAvgResult,
			   sit.UplinkStatus, sit.ConnectionSetupTime, sit.ConnectionSetupStatus,sit.MoMtCallNo, sit.MoMtCallDuration, sit.MoStatus, sit.MtStatus,
			   sit.VMoMtCallno, sit.VMoMtCallDuration, sit.VMoStatus, sit.VMtStatus,sit.CwHandoverStatus, sit.Ccwhandoverstatus, sit.FtpServerIp,
			   sit.FtpServerPort, sit.FtpServerPath, sit.FtpDownlinkFile,sit.StationaryTestFilePath, sit.CwTestFilePath, sit.CcwTestFilePath,
			   sit.OoklaTestFilePath,sit.OoklaPingResult, sit.OoklaDownlinkResult,sit.OoklaUplinkResult,cli.Logo 'ClientLogo',ven.Logo 'VendorLogo',sit.ICwHandoverStatus,sit.ICcwhandoverstatus
		  from AV_SiteTestSummary sit 
		  INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=sit.SiteId AND anls.NetworkModeId=sit.NetworkModeId AND anls.BandId=sit.BandId AND anls.CarrierId=sit.CarrierId
		  INNER JOIN AV_Sectors AS sec ON sec.SiteId=sit.SiteId AND sec.NetworkModeId=sit.NetworkModeId AND sec.BandId=sit.BandId AND sec.CarrierId=sit.CarrierId AND sec.SectorId=sit.SectorId
		inner join AD_Clients cli on cli.ClientId=sit.ClientId 
		left join AD_Clients ven on ven.ClientId=cli.PClientId
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
END