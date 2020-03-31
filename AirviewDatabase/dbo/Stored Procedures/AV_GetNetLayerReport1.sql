-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- [dbo].[AV_GetNetLayerReport] 11360,73,76,15,11
CREATE PROCEDURE [dbo].[AV_GetNetLayerReport1]
	@SiteId int,
	@BandId int,
	@Carrier nvarchar(50),
	@NetworkMode nvarchar(50),
	@UserId NUMERIC(18,0)
AS
BEGIN
	
	 EXEC [dbo].[AV_GetNetLayerSummary] @SiteId,@BandId,@Carrier,@NetworkMode,@UserId
	
	--Select DISTINCT * from AV_SiteTestLog
	--Where SiteId = @SiteId and BandId=@BandId and ActualCarrier=@Carrier and SubNetworkMode=@NetworkMode
	--AND TestStatus=1 AND TestType IN('CW','CCW')
	--order by TIMESTAMP
	
	DECLARE @DrivePlots TABLE
([Timestamp] DATETIME,
  TestType NVARCHAR(100), Site NVARCHAR(50), PciId NVARCHAR(20), Latitude FLOAT, Longitude FLOAT, NetworkMode NVARCHAR(100), Band NVARCHAR(100),
  Carrier NVARCHAR(100),GsmRssi INT, GsmRxQual INT, WcdmaRssi INT, WcdmaRscp INT, WcdmaEcio FLOAT, LteRssi INT, LteRsrp INT, LteRsrq INT, LteRsnr FLOAT,
  LteCqi FLOAT, TestStatus BIT, IsHandover BIT, SubNetworkMode NVARCHAR(100), ActualBand NVARCHAR(100), ActualCarrier NVARCHAR(100),CellId NVARCHAR(20),serverTimeStamp DATETIME
)

IF (SELECT COUNT(logId) FROM AV_SiteTestLog Where SiteId = @SiteId and BandId=@BandId and ActualCarrier=@Carrier and SubNetworkMode=@NetworkMode
	AND TestStatus=1 AND TestType IN('CW'))>2000
	BEGIN
		INSERT INTO @DrivePlots([Timestamp],TestType, [Site], PciId, Latitude, Longitude,
		            NetworkMode, Band, Carrier, GsmRssi, GsmRxQual,WcdmaRssi, WcdmaRscp,
		            WcdmaEcio, LteRssi, LteRsrp, LteRsrq, LteRsnr, LteCqi,
		            TestStatus, IsHandover, SubNetworkMode, ActualBand,
		            ActualCarrier,CellId,serverTimeStamp)
		SELECT * 
		FROM (
		SELECT DISTINCT  x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude,
       x.NetworkMode, x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi,
       x.WcdmaRscp, x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr,
       x.LteCqi, x.TestStatus, x.IsHandover, x.SubNetworkMode, x.ActualBand,
       x.ActualCarrier,x.CellId,x.serverTimestamp
       
		FROM
		(
		Select DISTINCT ROW_NUMBER() OVER (ORDER BY x.logid) 'rowid',x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
	       x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
	       x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
	       x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
	       x.ActualCarrier,x.CellId,x.serverTimeStamp
		from AV_SiteTestLog x
		Where SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
			AND IShandover=0 
		AND logid%2=0 AND TestStatus=1 AND TestType IN('CW')
		AND isActive=1
		) x		
		--WHERE x.rowid%2=0
		UNION ALL
		Select DISTINCT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
	       x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
	       x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
	       x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
	       x.ActualCarrier,x.CellId,x.serverTimestamp
		from AV_SiteTestLog x
		Where SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode 
		AND IShandover=1 AND TestStatus=1 AND TestType IN('CW')
		AND isActive=1		
		) x
		ORDER BY x.TestType,x.[TimeStamp] DESC
	END
	ELSE
	BEGIN
		INSERT INTO @DrivePlots([Timestamp],TestType, [Site], PciId, Latitude, Longitude,
		            NetworkMode, Band, Carrier, GsmRssi, GsmRxQual,WcdmaRssi, WcdmaRscp,
		            WcdmaEcio, LteRssi, LteRsrp, LteRsrq, LteRsnr, LteCqi,
		            TestStatus, IsHandover, SubNetworkMode, ActualBand,
		            ActualCarrier,CellId,serverTimestamp)
	Select DISTINCT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
	       x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
	       x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
	       x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
	       x.ActualCarrier,x.CellId,x.serverTimestamp
	  from AV_SiteTestLog x
	Where SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
	AND TestStatus=1 AND TestType IN('CW')
	AND isActive=1
	ORDER BY x.TestType,x.[TimeStamp] DESC
	END
	
	IF (SELECT COUNT(logId) FROM AV_SiteTestLog Where SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and NetworkModeId=@NetworkMode
	AND TestStatus=1 AND TestType IN('CCW'))>2000
	BEGIN
		INSERT INTO @DrivePlots([Timestamp],TestType, [Site], PciId, Latitude, Longitude,
		            NetworkMode, Band, Carrier, GsmRssi, GsmRxQual,WcdmaRssi, WcdmaRscp,
		            WcdmaEcio, LteRssi, LteRsrp, LteRsrq, LteRsnr, LteCqi,
		            TestStatus, IsHandover, SubNetworkMode, ActualBand,
		            ActualCarrier,CellId,serverTimestamp)
		SELECT * 
		FROM (
		SELECT DISTINCT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude,
       x.NetworkMode, x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi,
       x.WcdmaRscp, x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr,
       x.LteCqi, x.TestStatus, x.IsHandover, x.SubNetworkMode, x.ActualBand,
       x.ActualCarrier,x.CellId,x.serverTimestamp
       
		FROM
		(
		Select DISTINCT ROW_NUMBER() OVER (ORDER BY x.logid) 'rowid',x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
	       x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
	       x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
	       x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
	       x.ActualCarrier,x.CellId,x.serverTimestamp
		from AV_SiteTestLog x
		Where SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
			AND IShandover=0 
		AND logid%2=0 AND TestStatus=1 AND TestType IN('CCW')
		AND isActive=1
		) x
		--WHERE x.rowid%2=0
		UNION ALL
		Select DISTINCT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
	       x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
	       x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
	       x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
	       x.ActualCarrier,x.CellId,x.serverTimestamp
		from AV_SiteTestLog x
		Where SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode 
		AND IShandover=1 AND TestStatus=1 AND TestType IN('CCW')
		AND isActive=1		
		) x
		ORDER BY x.TestType,x.[TimeStamp] DESC
	END
	ELSE
	BEGIN
		INSERT INTO @DrivePlots([Timestamp],TestType, [Site], PciId, Latitude, Longitude,
		            NetworkMode, Band, Carrier, GsmRssi, GsmRxQual,WcdmaRssi, WcdmaRscp,
		            WcdmaEcio, LteRssi, LteRsrp, LteRsrq, LteRsnr, LteCqi,
		            TestStatus, IsHandover, SubNetworkMode, ActualBand,
		            ActualCarrier,CellId,serverTimestamp)
	Select DISTINCT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
	       x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
	       x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
	       x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
	       x.ActualCarrier,x.CellId,x.serverTimestamp
	  from AV_SiteTestLog x
	Where SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
	AND TestStatus=1 AND TestType IN('CCW')
	AND isActive=1
	ORDER BY x.TestType,x.[TimeStamp] DESC
	END


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
       x.ActualCarrier,y.Sector,x.CellId,x.serverTimestamp
	FROM @DrivePlots x LEFT OUTER JOIN #Sectors y ON x.PciId=y.PciId
	ORDER BY x.TestType
	
	
	--SELECT rpt.*
	--FROM AD_ReportConfiguration rpt
	--INNER JOIN AV_Sites sit ON sit.ClientId=rpt.clientId AND sit.CityId=rpt.CityId
	--WHERE reportID=444 AND sit.SiteId=@SiteId;
	
	 DECLARE @CitiId AS NUMERIC(18,0)
	 DECLARE @ClientId AS NUMERIC(18,0)
	 SELECT @CitiId=CityId , @ClientId=ClientId FROM AV_Sites  WHERE SiteId=@SiteId
	 EXEC [AD_GetReportConfiguration]  'byCityId_ClientId', @CitiId, @ClientId

	 SELECT DISTINCT y.Sector,x.PciId,x.CellId
	 FROM @DrivePlots x LEFT OUTER JOIN #Sectors y ON x.PciId=y.PciId
END