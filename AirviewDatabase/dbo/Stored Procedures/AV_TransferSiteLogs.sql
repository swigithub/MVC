-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--exec AV_TransferSiteLogs @Filter=N'Transfer_Testslogs',@SourceSiteId=648664,@LayerStatusId=674559,@Sectors=N'718748,718749',@Tests=N'CW,Ping',@DestSiteCode=N'Safi-11122',@Tasks=N'Summary'
--exec AV_TransferSiteLogs @Filter=N'Transfer_Testslogs',@SourceSiteId=648664,@LayerStatusId=674559,@Sectors=N'718748,718749',@Tests=N'96',@DestSiteCode=N'',@Tasks=N'Logs'
--exec AV_TransferSiteLogs @Filter=N'Transfer_Testslogs',@SourceSiteId=648664,@LayerStatusId=674559,@Sectors=N'718748,718749',@Tests=N'13175,13176',@DestSiteCode=N'Test123',@Tasks=N'Logs'
--exec AV_TransferSiteLogs @Filter=N'Count_Testslogs',@SourceSiteId=648664,@LayerStatusId=0,@Sectors=N'',@Tests=N'',@DestSiteCode=N'',@Tasks=N''
-- exec AV_TransferSiteLogs @Filter=N'Count_Testslogs',@SourceSiteId=668757,@LayerStatusId=0,@Sectors=N'',@Tests=N'',@SiteCode=N'',@Task=N''
-- exec AV_TransferSiteLogs @Filter=N'Transfer_Testslogs',@SourceSiteId=648737,@LayerStatusId=674718,@Sectors=N'719219,719220,719221',@Tests=N'CW',@DestSiteCode=N'Safi-1112211',@Tasks=N'Logs'
CREATE PROCEDURE AV_TransferSiteLogs
@Filter NVARCHAR(50)
,@SourceSiteId NUMERIC(18,0)
,@LayerStatusId NUMERIC(18,0)
,@Sectors NVARCHAR(50)
,@Tests varchar(150)
,@DestSiteCode NVARCHAR(50)
,@Tasks NVARCHAR(50)
AS
BEGIN

IF @Filter='Transfer_Testslogs'
begin
declare @DestSiteId NUMERIC(18,0)
	declare @BandId NUMERIC(18,0)
	--declare @val nvarchar(180)
	set @DestSiteId = (select SiteId from AV_Sites where SiteCode =@DestSiteCode)
	set @BandId = (select BandId from AV_NetLayerStatus where LayerStatusId =@LayerStatusId)
	set @Tests=@Tests + ',';
	IF EXISTS (SELECT y.Item FROM SplitString(@Tasks, ',') y WHERE y.Item IN('Logs'))
	Begin
		if Charindex(CAST('CW' AS NVARCHAR(MAX))+',', @Tests) > 0
		begin
		INSERT INTO AV_SiteTestLog(
		RegionId,Region,CityId,City,TestCategoryId,TestCategory,TestTypeId,TestType,[TimeStamp],ClusterId,Cluster,SiteId,[Site],SectorId,Sector,CellId,LacId,PciId,MccId,MncId,
			Latitude,Longitude,ScopeId,Scope,NetworkModeId,NetworkMode,BandId,Band,CarrierId,Carrier,GsmRssi,GsmRxQual,WcdmaRssi,WcdmaRscp,WcdmaEcio,LteRssi,LteRsrp,LteRsrq,LteRsnr,
			LteCqi,DistanceFromSite,AngleToSite,FtpStatus,StackTrace,TestResult,MoStatus,MtStatus,VolteMoStatus,VolteMtStatus,ConnectionSetupTime,SubNetworkMode,ActualBand,
	  ActualCarrier,TestStatus,IsHandover,IsActive,serverTimeStamp)
	
		SELECT x.RegionId, x.Region, x.CityId, x.City, x.TestCategoryId, x.TestCategory,
			   x.TestTypeId, x.TestType, x.[TimeStamp], x.ClusterId, x.Cluster, @DestSiteId 'SiteId',
			   @DestSiteCode 'Site', x.SectorId, x.Sector, x.CellId, x.LacId, x.PciId, x.MccId,
			   x.MncId, x.Latitude, x.Longitude, x.ScopeId, x.Scope, x.NetworkModeId,
			   x.NetworkMode, x.BandId, x.Band, x.CarrierId, x.Carrier, x.GsmRssi,
			   x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp, x.WcdmaEcio, x.LteRssi, x.LteRsrp,
			   x.LteRsrq, x.LteRsnr, x.LteCqi, x.DistanceFromSite, x.AngleToSite,
			   x.FtpStatus, x.StackTrace, x.TestResult, x.MoStatus, x.MtStatus,
			   x.VolteMoStatus, x.VolteMtStatus, x.ConnectionSetupTime, x.SubNetworkMode,
			   x.ActualBand, x.ActualCarrier, x.TestStatus, x.IsHandover, x.IsActive,GETDATE()
		  FROM AV_SiteTestLog x
		WHERE x.[SiteId] = @SourceSiteId And  x.BandId=BandId AND
		Charindex(CAST('CW'AS NVARCHAR(MAX))+',', @Tests) > 0 or x.[SiteId] = @SourceSiteId And  x.BandId=BandId AND
		Charindex(CAST('CCW'AS NVARCHAR(MAX))+',', @Tests ) >0
		end
		
		 INSERT INTO AV_SiteTestLog(
			RegionId,Region,CityId,City,TestCategoryId,TestCategory,TestTypeId,TestType,[TimeStamp],ClusterId,Cluster,SiteId,[Site],SectorId,Sector,CellId,LacId,PciId,MccId,MncId,
		Latitude,Longitude,ScopeId,Scope,NetworkModeId,NetworkMode,BandId,Band,CarrierId,Carrier,GsmRssi,GsmRxQual,WcdmaRssi,WcdmaRscp,WcdmaEcio,LteRssi,LteRsrp,LteRsrq,LteRsnr,
			LteCqi,DistanceFromSite,AngleToSite,FtpStatus,StackTrace,TestResult,MoStatus,MtStatus,VolteMoStatus,VolteMtStatus,ConnectionSetupTime,SubNetworkMode,ActualBand,
			ActualCarrier,TestStatus,IsHandover,IsActive,serverTimeStamp)
	
	
		SELECT x.RegionId, x.Region, x.CityId, x.City, x.TestCategoryId, x.TestCategory,
			   x.TestTypeId, x.TestType, x.[TimeStamp], x.ClusterId, x.Cluster, @DestSiteId 'SiteId',
			   @DestSiteCode 'Site', x.SectorId, x.Sector, x.CellId, x.LacId, x.PciId, x.MccId,
			   x.MncId, x.Latitude, x.Longitude, x.ScopeId, x.Scope, x.NetworkModeId,
			   x.NetworkMode, x.BandId, x.Band, x.CarrierId, x.Carrier, x.GsmRssi,
			   x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp, x.WcdmaEcio, x.LteRssi, x.LteRsrp,
			   x.LteRsrq, x.LteRsnr, x.LteCqi, x.DistanceFromSite, x.AngleToSite,
			   x.FtpStatus, x.StackTrace, x.TestResult, x.MoStatus, x.MtStatus,
			   x.VolteMoStatus, x.VolteMtStatus, x.ConnectionSetupTime, x.SubNetworkMode,
			   x.ActualBand, x.ActualCarrier, x.TestStatus, x.IsHandover, x.IsActive,GETDATE()
		  FROM AV_SiteTestLog x
			WHERE x.[SiteId] = @SourceSiteId And  x.BandId=BandId AND
		Charindex(CAST(x.TestType AS NVARCHAR(MAX))+',', @Tests) > 0 And Charindex(CAST(x.SectorId AS NVARCHAR(MAX))+',', @Sectors) > 0
		--select *from AV_SiteTestLog where SiteId =648664
End

	IF EXISTS (SELECT x.Item FROM SplitString(@Tasks, ',') x WHERE x.Item IN('Summary'))
		Begin

			UPDATE AV_SiteTestSummary
			SET
				TestLatitude = (SELECT asts.TestLatitude FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				TestLongitude = (SELECT asts.TestLongitude FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				GsmRssi = (SELECT asts.GsmRssi FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				GsmRxQual = (SELECT asts.GsmRxQual FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				WcdmaRssi = (SELECT asts.WcdmaRssi FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				WcdmaRscp =(SELECT asts.WcdmaRscp FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				WcdmaEcio = (SELECT asts.WcdmaEcio FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				LteRssi = (SELECT asts.LteRssi FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				LteRsrp =(SELECT asts.LteRsrp FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				LteRsrq = (SELECT asts.LteRsrq FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				LteRsnr = (SELECT asts.LteRsnr FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				LteCqi = (SELECT asts.LteCqi FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				DistanceFromSite = (SELECT asts.DistanceFromSite FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				AngleToSite = (SELECT asts.AngleToSite FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				FtpStatus = (SELECT asts.FtpStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				PingHost = (SELECT asts.PingHost FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				LatencyRate = (SELECT asts.LatencyRate FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				PingIterations = (SELECT asts.PingIterations FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				PingMinResult = (SELECT asts.PingMinResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				PingMaxResult = (SELECT asts.PingMaxResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				PingAverageResult = (SELECT asts.PingAverageResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				PingStatus = (SELECT asts.PingStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				DownlinkRate = (SELECT asts.DownlinkRate FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				DownlinkMinResult = (SELECT asts.DownlinkMinResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				DownlinkMaxResult = (SELECT asts.DownlinkMaxResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				DownlinkAvgResult = (SELECT asts.DownlinkAvgResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				DownlinkStatus = (SELECT asts.DownlinkStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				UplinkRate = (SELECT asts.UplinkRate FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				UplinkMinResult = (SELECT asts.UplinkMinResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				UplinkMaxResult =(SELECT asts.UplinkMaxResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				UplinkAvgResult = (SELECT asts.UplinkAvgResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				UplinkStatus = (SELECT asts.UplinkStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				ConnectionSetupTime = (SELECT asts.ConnectionSetupTime FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				ConnectionSetupStatus = (SELECT asts.ConnectionSetupStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				MoMtCallNo = (SELECT asts.MoMtCallNo FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				MoMtCallDuration = (SELECT asts.MoMtCallDuration FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				MoStatus = (SELECT asts.MoStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				MtStatus = (SELECT asts.MtStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				VMoMtCallno = (SELECT asts.VMoMtCallno FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				VMoMtCallDuration = (SELECT asts.VMoMtCallDuration FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				VMoStatus = (SELECT asts.VMoStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				VMtStatus = (SELECT asts.VMtStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				CwHandoverStatus = (SELECT asts.CwHandoverStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				Ccwhandoverstatus = (SELECT asts.Ccwhandoverstatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				ICwHandoverStatus = (SELECT asts.ICwHandoverStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				ICcwhandoverstatus = (SELECT asts.ICcwhandoverstatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				FtpServerIp = (SELECT asts.FtpServerIp FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				FtpServerPort = (SELECT asts.FtpServerPort FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				FtpServerPath = (SELECT asts.FtpServerPath FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				FtpDownlinkFile = (SELECT asts.FtpDownlinkFile FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				StationaryTestFilePath = (SELECT asts.StationaryTestFilePath FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				CwTestFilePath = (SELECT asts.CwTestFilePath FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				CcwTestFilePath =(SELECT asts.CcwTestFilePath FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				OoklaTestFilePath = (SELECT asts.OoklaTestFilePath FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				OoklaPingResult = (SELECT asts.OoklaPingResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				OoklaDownlinkResult = (SELECT asts.OoklaDownlinkResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				OoklaUplinkResult = (SELECT asts.OoklaUplinkResult FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				OoklaLatitude = (SELECT asts.OoklaLatitude FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				OoklaLongitude = (SELECT asts.OoklaLongitude FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				OoklaRssi = (SELECT asts.OoklaRssi FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				OoklaSinr =(SELECT asts.OoklaSinr FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				pciColor = (SELECT asts.pciColor FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				rsrpColor = (SELECT asts.rsrpColor FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				rsrqColor = (SELECT asts.rsrqColor FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				sinrColor =(SELECT asts.sinrColor FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				dlColor =(SELECT asts.dlColor FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				SMoStatus = (SELECT asts.SMoStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				SMtStatus = (SELECT asts.SMtStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				CarrierAggregationStatus = (SELECT asts.CarrierAggregationStatus FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				E911Status = (SELECT asts.E911Status FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				IsE911Performed = (SELECT asts.IsE911Performed FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				TesterName = (SELECT asts.TesterName FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				Comments = (SELECT asts.Comments FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				TechName = (SELECT asts.TechName FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				SwitchComment = (SELECT asts.SwitchComment FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector),
				ProjectId = (SELECT asts.ProjectId FROM AV_SiteTestSummary AS asts WHERE asts.SiteId=@SourceSiteId AND asts.NetworkMode=AV_SiteTestSummary.NetworkMode AND asts.Band=AV_SiteTestSummary.Band AND asts.Carrier=AV_SiteTestSummary.Carrier AND asts.Sector=AV_SiteTestSummary.Sector)
			WHERE SiteId=@SourceSiteId And  BandId=@BandId AND
				 Charindex(CAST(SectorId AS NVARCHAR(MAX))+',', @Sectors) > 0


				 --select * from AV_SiteTestSummary where Site='Safi-111'
		End

End
End