-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- [dbo].[AV_GetNetLayerReport] 11036,74,181,15,11,'2017-03-26'
CREATE PROCEDURE [dbo].[AV_GetNetLayerReport5]
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
	DECLARE @UploadDate AS DATETIME
		
	SELECT @SiteCode=as1.SiteCode, @UploadDate=as1.SubmittedOn
		FROM AV_Sites AS as1
	WHERE as1.SiteId=@SiteId
		
		
	
	IF CAST(@UploadDate AS DATE)<CAST(@ReportDate AS DATE)
	BEGIN	
	DECLARE @DrivePlots TABLE
	([Timestamp] DATETIME,
	TestType NVARCHAR(100), Site NVARCHAR(50), PciId NVARCHAR(20), Latitude FLOAT, Longitude FLOAT, NetworkMode NVARCHAR(100), Band NVARCHAR(100),
	 Carrier NVARCHAR(100),GsmRssi INT, GsmRxQual INT, WcdmaRssi INT, WcdmaRscp INT, WcdmaEcio FLOAT, LteRssi INT, LteRsrp INT, LteRsrq INT, LteRsnr FLOAT,
	 LteCqi FLOAT, TestStatus BIT, IsHandover BIT, SubNetworkMode NVARCHAR(100), ActualBand NVARCHAR(100), ActualCarrier NVARCHAR(100),CellId NVARCHAR(20),serverTimeStamp DATETIME,
	pciColor nvarchar(50), rsrpColor nvarchar(50), rsrqColor nvarchar(50), sinrColor nvarchar(50),ChColor NVARCHAR(50))


	IF (SELECT COUNT(logId) FROM AV_SiteTestLog Where SiteId = @SiteId and BandId=@BandId and ActualCarrier=@Carrier and SubNetworkMode=@NetworkMode
	AND TestStatus=1 AND TestType IN('CW'))>2000
	BEGIN
		INSERT INTO @DrivePlots([Timestamp],TestType, [Site], PciId, Latitude, Longitude,
		            NetworkMode, Band, Carrier, GsmRssi, GsmRxQual,WcdmaRssi, WcdmaRscp,
		            WcdmaEcio, LteRssi, LteRsrp, LteRsrq, LteRsnr, LteCqi,
		            TestStatus, IsHandover, SubNetworkMode, ActualBand,
		            ActualCarrier,CellId,serverTimeStamp,pciColor,rsrpColor,rsrqColor,sinrColor,chColor)
		SELECT * 
		FROM (
		SELECT DISTINCT  x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude,
       x.NetworkMode, x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi,
       x.WcdmaRscp, x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr,
       x.LteCqi, x.TestStatus, x.IsHandover, x.SubNetworkMode, x.ActualBand,
       x.ActualCarrier,x.CellId,x.serverTimestamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor,x.chColor
       
		FROM
		(
		Select DISTINCT ROW_NUMBER() OVER (ORDER BY x.logid) 'rowid',x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
	       x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
	       x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
	       x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
	       x.ActualCarrier,x.CellId,x.serverTimeStamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor,x.chColor
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
	       x.ActualCarrier,x.CellId,x.serverTimestamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor,x.chColor
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
		            ActualCarrier,CellId,serverTimestamp,pciColor,rsrpColor,rsrqColor,sinrColor,chColor)
	Select DISTINCT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
	       x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
	       x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
	       x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
	       x.ActualCarrier,x.CellId,x.serverTimestamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor,x.chColor
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
		            ActualCarrier,CellId,serverTimestamp,pciColor,rsrpColor,rsrqColor,sinrColor,chColor)
		SELECT * 
		FROM (
		SELECT DISTINCT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude,
       x.NetworkMode, x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi,
       x.WcdmaRscp, x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr,
       x.LteCqi, x.TestStatus, x.IsHandover, x.SubNetworkMode, x.ActualBand,
       x.ActualCarrier,x.CellId,x.serverTimestamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor,x.chColor
       
		FROM
		(
		Select DISTINCT ROW_NUMBER() OVER (ORDER BY x.logid) 'rowid',x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
	       x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
	       x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
	       x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
	       x.ActualCarrier,x.CellId,x.serverTimestamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor,x.chColor
		from AV_SiteTestLog x
		Where SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode
			AND IShandover=0 
		--AND logid%2=0
		AND TestStatus=1 AND TestType IN('CCW')
		AND isActive=1
		) x
		--WHERE x.rowid%2=0
		UNION ALL
		Select DISTINCT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
	       x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
	       x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
	       x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
	       x.ActualCarrier,x.CellId,x.serverTimestamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor,x.chColor
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
		            ActualCarrier,CellId,serverTimestamp,pciColor,rsrpColor,rsrqColor,sinrColor,chColor)
	Select DISTINCT x.[TimeStamp],x.TestType, x.[Site], x.PciId, x.Latitude, x.Longitude, x.NetworkMode,
	       x.Band, x.Carrier, x.GsmRssi, x.GsmRxQual, x.WcdmaRssi, x.WcdmaRscp,
	       x.WcdmaEcio, x.LteRssi, x.LteRsrp, x.LteRsrq, x.LteRsnr, x.LteCqi,
	       x.TestStatus, x.IsHandover,x.SubNetworkMode, x.ActualBand,
	       x.ActualCarrier,x.CellId,x.serverTimestamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor,x.chColor
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
       x.ActualCarrier,y.Sector,x.CellId,x.serverTimestamp,x.pciColor,x.rsrpColor,x.rsrqColor,x.sinrColor
	FROM @DrivePlots x LEFT OUTER JOIN #Sectors y ON x.PciId=y.PciId
	ORDER BY x.TestType
	END
	ELSE
	BEGIN	
		--<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>CH41662A-D1</name><description>CH41662A-D1\LTE_LTE 1900_1150</description> 
		--</Document></kml>
	

		SELECT
		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
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
			'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode AND x.TestType IN('CW','CCW')
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' pciPlot,
		
		--'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		--REPLACE(REPLACE(REPLACE(
		--	(SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'">'+
		--	'<LineStyle>'+
		--	'<color>ff' + SUBSTRING(RIGHT(pciColor,6),5,2)+ SUBSTRING(RIGHT(pciColor,6),3,2) +SUBSTRING(RIGHT(pciColor,6),1,2)+ '</color>'+
		--	'<width>10</width>'+
		--	'</LineStyle>'+
		--	'</Style>'+
		--	'<Placemark>'+
		--	'<name>cw1</name>'+
		--	'<styleUrl>#Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
		--	'<LineString>'+
		--	'<altitudeMode>relative</altitudeMode>'+
		--	'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
		--	'</LineString>'+
		--	'</Placemark>'  + ' '
		--FROM AV_SiteTestLog x
		--WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode AND x.TestType IN('CW','CCW')
		--FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		--'</Document></kml>' hoPlot,
		
		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
		(SELECT CASE WHEN ISNULL(x.IsHandover,0)=0 THEN
		'<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'">'+
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
		'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
		'</LineString>'+
		'</Placemark>'
		WHEN ISNULL(x.IsHandover,0)=1 THEN 
		'<Style id="hoMarker'+ CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar) + '">' +
		'<IconStyle><Icon><href>' + CASE WHEN x.City='Chicago' THEN 'http://96.57.107.148/Content/Images/Common/hoHand.png' ELSE 'http://96.57.107.148/Content/Images/Colors/handover_x16.png'  END + '</href></Icon></IconStyle>'+
		'</Style>'+		
		'<Placemark>'+
		'<name>'+x.PciId+'</name>'+
		'<description>This is a new point to learn KML format.</description>'+
		'<styleUrl>#hoMarker'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
		'<Point>'+
		'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
		'</Point>'+
		'</Placemark>'		
		END + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode  AND x.TestType IN('CW','CCW')
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' hoPlot,
		
		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
		(SELECT CASE WHEN ISNULL(x.IsHandover,0)=1 THEN
		'<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'">'+
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
		'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
		'</LineString>'+
		'</Placemark>'
		WHEN ISNULL(x.IsHandover,0)=1 THEN 
		'<Style id="hoMarker'+ CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar) + '">' +
		'<IconStyle><Icon><href>' + CASE WHEN x.City='Chicago' THEN 'http://96.57.107.148/Content/Images/Common/hoHand.png' ELSE 'http://96.57.107.148/Content/Images/Colors/handover_x16.png'  END + '</href></Icon></IconStyle>'+
		'</Style>'+		
		'<Placemark>'+
		'<name>'+x.PciId+'</name>'+
		'<description>This is a new point to learn KML format.</description>'+
		'<styleUrl>#hoMarker'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
		'<Point>'+
		'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
		'</Point>'+
		'</Placemark>'		
		END + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode AND x.TestType IN('CW')
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' cwPlot,
		
		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
		(SELECT CASE WHEN ISNULL(x.IsHandover,0)=0 THEN
		'<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'">'+
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
		'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
		'</LineString>'+
		'</Placemark>'
		WHEN ISNULL(x.IsHandover,0)=1 THEN 
		'<Style id="hoMarker'+ CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar) + '">' +
		'<IconStyle><Icon><href>' + CASE WHEN x.City='Chicago' THEN 'http://96.57.107.148/Content/Images/Common/hoHand.png' ELSE 'http://96.57.107.148/Content/Images/Colors/handover_x16.png'  END + '</href></Icon></IconStyle>'+
		'</Style>'+		
		'<Placemark>'+
		'<name>'+x.PciId+'</name>'+
		'<description>This is a new point to learn KML format.</description>'+
		'<styleUrl>#hoMarker'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
		'<Point>'+
		'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
		'</Point>'+
		'</Placemark>'		
		END + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode AND x.TestType IN('CCW')
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' ccwPlot,

		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'">'+
			'<LineStyle>'+
			'<color>ff' + SUBSTRING(RIGHT(rsrpColor,6),5,2)+ SUBSTRING(RIGHT(rsrpColor,6),3,2) +SUBSTRING(RIGHT(rsrpColor,6),1,2)+ '</color>'+
			'<width>10</width>'+
			'</LineStyle>'+
			'</Style>'+
			'<Placemark>'+
			'<name>cw1</name>'+
			'<styleUrl>#Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
			'<LineString>'+
			'<altitudeMode>relative</altitudeMode>'+
			'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode AND x.TestType IN('CW','CCW')
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' rsrpPlot,

		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'">'+
			'<LineStyle>'+
			'<color>ff' + SUBSTRING(RIGHT(rsrqColor,6),5,2)+ SUBSTRING(RIGHT(rsrqColor,6),3,2) +SUBSTRING(RIGHT(rsrqColor,6),1,2)+ '</color>'+
			'<width>10</width>'+
			'</LineStyle>'+
			'</Style>'+
			'<Placemark>'+
			'<name>cw1</name>'+
			'<styleUrl>#Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
			'<LineString>'+
			'<altitudeMode>relative</altitudeMode>'+
			'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode AND x.TestType IN('CW','CCW')
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' rsrqPlot,

		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'">'+
			'<LineStyle>'+
			'<color>ff' + SUBSTRING(RIGHT(sinrColor,6),5,2)+ SUBSTRING(RIGHT(sinrColor,6),3,2) +SUBSTRING(RIGHT(sinrColor,6),1,2)+ '</color>'+
			'<width>10</width>'+
			'</LineStyle>'+
			'</Style>'+
			'<Placemark>'+
			'<name>cw1</name>'+
			'<styleUrl>#Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
			'<LineString>'+
			'<altitudeMode>relative</altitudeMode>'+
			'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode AND x.TestType IN('CW','CCW')
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' sinrPlot,
		
		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'">'+
			'<LineStyle>'+
			'<color>ff' + SUBSTRING(RIGHT(sinrColor,6),5,2)+ SUBSTRING(RIGHT(sinrColor,6),3,2) +SUBSTRING(RIGHT(sinrColor,6),1,2)+ '</color>'+
			'<width>10</width>'+
			'</LineStyle>'+
			'</Style>'+
			'<Placemark>'+
			'<name>cw1</name>'+
			'<styleUrl>#Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
			'<LineString>'+
			'<altitudeMode>relative</altitudeMode>'+
			'<coordinates>'+ CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0 '+CAST(x.Longitude AS NVARCHAR)+','+CAST(x.Latitude AS NVARCHAR)+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@Carrier and x.NetworkModeId=@NetworkMode AND x.TestType IN('CW','CCW')
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' chPlot
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
	 EXEC [AD_GetReportConfiguration]  'byCityId_ClientId', @CitiId, @ClientId


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
	 	AND x.TestType IN('CW','CCW')
	 END	 
	
	IF (SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=@CitiId)='Chicago'
	BEGIN
		
		DECLARE @sitLat as float=0
		DECLARE @sitLng as float=0
		
		SELECT @sitLat=Latitude,@sitLng=Longitude
		FROM AV_Sites WHERE Siteid=@SiteId
		
		DECLARE @maxDistance as float=(SELECT MAX(DistanceFromSite) FROM AV_SiteTestLog WHERE Siteid=@SiteId and TestType IN('CW','CCW'))


		--SELECT @siteCode
		--[dbo].[AV_GetNetLayerReport] 11036,74,181,15,11

		SELECT x.SiteCode,x.Latitude,x.Longitude,x.Azimuth,x.PCI,x.SectorColor,x.SectorCode,x.BeamWidth
		--ACOS( SIN( RADIANS(x.latitude) ) * SIN( RADIANS( @sitLat ) ) + COS( RADIANS(x.latitude) )
		-- COS( RADIANS( @sitLat ))  COS( RADIANS(x.longitude) - RADIANS( @sitLong )) ) * 6380 AS distance
		FROM AV_marketSites x
		WHERE x.SiteCode!=@SiteCode AND
		ACOS( SIN( RADIANS(x.latitude) ) * SIN( RADIANS( @sitLat ) ) + COS( RADIANS(x.latitude) )
		* COS( RADIANS( @sitLat )) * COS( RADIANS(x.longitude) - RADIANS( @sitLng )) ) * 6380 < @maxDistance+10
		
	END
END