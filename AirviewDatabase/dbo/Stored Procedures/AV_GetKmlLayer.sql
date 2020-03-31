CREATE PROCEDURE [dbo].[AV_GetKmlLayer]
	 @Filter nvarchar(50)
	,@SiteId numeric(18,0)=0
	,@SiteCode nvarchar(50)=null
	,@NetworkModeId numeric(18,0)=0
	,@BandId numeric(18,0)=0
	,@CarrierId numeric(18,0)=0
	,@ScopeId numeric(18,0)=0
AS
BEGIN
	DECLARE @ClientId AS NUMERIC(18,0)=0
	
	if @Filter='NetLayerReport' AND @ClientId=0
	begin
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
			'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId AND x.TestType IN('CW','CCW')
		AND x.IsActive=1 --AND (x.LogId%2)=0
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' pciPlot,
		
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
		'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
		'</LineString>'+
		'</Placemark>'
		WHEN ISNULL(x.IsHandover,0)=1 THEN 
		'<Style id="hoMarker'+ CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar) + '">' +
		'<IconStyle><Icon><scale>0.6</scale><href>' + CASE WHEN x.City='Chicago' THEN 'http://96.57.107.148:82/Content/Images/Common/hoHand.png' ELSE 'http://96.57.107.148:82/Content/Images/Colors/handover_x16.png'  END + '</href></Icon></IconStyle>'+
		'</Style>'+		
		'<Placemark>'+
		'<name>'+x.PciId+'</name>'+
		'<description>'+x.PciId+'</description>'+
		'<styleUrl>#hoMarker'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
		'<Point>'+
		'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
		'</Point>'+
		'</Placemark>'		
		END + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId  AND x.TestType IN('CW','CCW')
		AND x.IsActive=1 --AND (x.LogId%2)=0
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' hoPlot,
		
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
		'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
		'</LineString>'+
		'</Placemark>'
		WHEN ISNULL(x.IsHandover,0)=1 AND x.NetworkMode='NR' THEN 
		'<Style id="hoMarker'+ CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar) + '">' +
		'<IconStyle><Icon><href>' + CASE WHEN x.City='Chicago' THEN 'http://96.57.107.148:82/Content/Images/Common/hoHand.png' ELSE 'http://96.57.107.148:82/Content/Images/Colors/handover_x16.png'  END + '</href></Icon></IconStyle>'+
		'</Style>'+		
		'<Placemark>'+
		'<name>'+x.PciId+'</name>'+
		'<description>This is a new point to learn KML format.</description>'+
		'<styleUrl>#hoMarker'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
		'<Point>'+
		'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
		'</Point>'+
		'</Placemark>'		
		END + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId AND x.TestType IN('CW')
		AND x.IsActive=1 --AND (x.LogId%2)=0
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
		'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
		'</LineString>'+
		'</Placemark>'
		WHEN ISNULL(x.IsHandover,0)=1 AND x.NetworkMode='NR' THEN 
		'<Style id="hoMarker'+ CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar) + '">' +
		'<IconStyle><Icon><href>' + CASE WHEN x.City='Chicago' THEN 'http://96.57.107.148:82/Content/Images/Common/hoHand.png' ELSE 'http://96.57.107.148:82/Content/Images/Colors/handover_x16.png'  END + '</href></Icon></IconStyle>'+
		'</Style>'+		
		'<Placemark>'+
		'<name>'+x.PciId+'</name>'+
		'<description>This is a new point to learn KML format.</description>'+
		'<styleUrl>#hoMarker'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
		'<Point>'+
		'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
		'</Point>'+
		'</Placemark>'		
		END + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId AND x.TestType IN('CCW')
		AND x.IsActive=1 --AND (x.LogId%2)=0
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
			'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId AND x.TestType IN('CW','CCW')
		AND x.IsActive=1 --AND (x.LogId%2)=0
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
			'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId AND x.TestType IN('CW','CCW')
		AND x.IsActive=1 --AND (x.LogId%2)=0
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
			'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId AND x.TestType IN('CW','CCW')
		AND x.IsActive=1 --AND (x.LogId%2)=0
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' sinrPlot,
		
		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'">'+
			'<LineStyle>'+
			'<color>ff' + SUBSTRING(RIGHT(ChColor,6),5,2)+ SUBSTRING(RIGHT(ChColor,6),3,2) +SUBSTRING(RIGHT(ChColor,6),1,2)+ '</color>'+
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
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId AND x.TestType IN('CW','CCW')
		AND x.IsActive=1 --AND (x.LogId%2)=0
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' chPlot,
		
		
		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'">'+
			'<LineStyle>'+
			'<color>ff' + SUBSTRING(RIGHT(cellColor,6),5,2)+ SUBSTRING(RIGHT(cellColor,6),3,2) +SUBSTRING(RIGHT(cellColor,6),1,2)+ '</color>'+
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
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId AND x.TestType IN('CW','CCW')
		AND x.IsActive=1 --AND (x.LogId%2)=0
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' cellPlot,

		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY BTLogId) as nvarchar)+'">'+
			'<LineStyle>'+
			'<color>ff' + SUBSTRING(RIGHT(BMColor,6),5,2)+ SUBSTRING(RIGHT(BMColor,6),3,2) +SUBSTRING(RIGHT(BMColor,6),1,2)+ '</color>'+
			'<width>10</width>'+
			'</LineStyle>'+
			'</Style>'+
			'<Placemark>'+
			'<name>cw1</name>'+
			'<styleUrl>#Id'+CAST(ROW_NUMBER() OVER (ORDER BY BTLogId) as nvarchar)+'</styleUrl>'+
			'<LineString>'+
			'<altitudeMode>relative</altitudeMode>'+
			'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_BeamTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId --AND x.TestType IN('CW','CCW')
		--AND x.IsActive=1 --AND (x.LogId%2)=0
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' beamPlot,

		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY BTLogId) as nvarchar)+'">'+
			'<LineStyle>'+
			'<color>ff' + SUBSTRING(RIGHT(BMGColor,6),5,2)+ SUBSTRING(RIGHT(BMGColor,6),3,2) +SUBSTRING(RIGHT(BMGColor,6),1,2)+ '</color>'+
			'<width>10</width>'+
			'</LineStyle>'+
			'</Style>'+
			'<Placemark>'+
			'<name>cw1</name>'+
			'<styleUrl>#Id'+CAST(ROW_NUMBER() OVER (ORDER BY BTLogId) as nvarchar)+'</styleUrl>'+
			'<LineString>'+
			'<altitudeMode>relative</altitudeMode>'+
			'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
		FROM AV_BeamTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId --AND x.TestType IN('CW','CCW')
		--AND x.IsActive=1 --AND (x.LogId%2)=0
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' beamGroupPlot,

		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+@SiteCode+'</name><description>'+@SiteCode+'</description>'+
		REPLACE(REPLACE(REPLACE(
		(SELECT  
		'<Style id="hoMarker'+ CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar) + '">' +
		'<IconStyle><Icon><href>' + CASE WHEN x.StackTrace LIKE 'EVENT%Release' THEN 'http://96.57.107.148:82/Content/Images/Colors/rHO_x16.png' ELSE 'http://96.57.107.148:82/Content/Images/Colors/dHO_x16.png'  END + '</href></Icon></IconStyle>'+
		'</Style>'+		
		'<Placemark>'+
		'<name>'+x.PciId+'</name>'+
		'<description>This is a new point to learn KML format.</description>'+
		'<styleUrl>#hoMarker'+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+'</styleUrl>'+
		'<Point>'+
		'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
		'</Point>'+
		'</Placemark>'		
		+ ' '
		FROM AV_SiteTestLog x
		WHERE SiteId = @SiteId and BandId=@BandId and CarrierId=@CarrierId and x.NetworkModeId=@NetworkModeId AND x.TestType IN('CW','CCW')
		AND (x.StackTrace LIKE 'EVENT%Release' OR x.StackTrace LIKE 'EVENT%0:0')
		AND x.IsActive=1 --AND (x.LogId%2)=0
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' brRelPlot,

		'' brDropPlot
	END
	ELSE if @Filter='NetLayerReport_Sprint'
	BEGIN
		DECLARE @Cell AS NVARCHAR(100)=''
		SELECT TOP 1 @Cell=x.Band
		FROM AV_NemoSiteLogs  x INNER JOIN AV_LogsInfo AS lg ON x.fileID_Fk=lg.fileID
		WHERE lg.SiteId = @SiteId and lg.BandId=@BandId and lg.CarrierId=@CarrierId and lg.NetworkModeId=@NetworkModeId AND lg.fileType IN('CW','CCW')
		AND x.Cell='Serving'
		
		SELECT
		'' pciPlot,
		
		'' hoPlot,
		
		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+'SPR002'+'</name><description>'+'SPR002'+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(
				SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY x.id) as nvarchar)+'">'+
				'<LineStyle>'+
				'<color>ff' + SUBSTRING(RIGHT(x.[Color],6),5,2)+ SUBSTRING(RIGHT(x.[Color],6),3,2) +SUBSTRING(RIGHT(x.[Color],6),1,2)+ '</color>'+
				'<width>10</width>'+
				'</LineStyle>'+
				'</Style>'+
				'<Placemark>'+
				'<name>cw1</name>'+
				'<styleUrl>#Id'+CAST(ROW_NUMBER() OVER (ORDER BY x.id) as nvarchar)+'</styleUrl>'+
				'<LineString>'+
				'<altitudeMode>relative</altitudeMode>'+
				'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
				'</LineString>'+
				'</Placemark>'  + ' '
				FROM
				(
					SELECT x.id,x.Latitude,
					x.Longitude, x.PCI,
					ISNULL((SELECT as1.sectorColor FROM AV_Sectors AS as1 WHERE as1.SiteId=x.siteID AND as1.NetworkModeId=x.networkModeID AND as1.BandId=x.bandID AND as1.CarrierId=x.carrierID AND as1.PCI=x.PCI),'#87898A') 'Color'
					FROM
					(
						SELECT ROW_NUMBER() OVER (ORDER BY x.id ASC) 'id', lg.siteID, lg.networkModeID, lg.bandID, lg.carrierID, lg.scopeID, x.[Time],x.Latitude,x.Longitude
						,ISNULL((SELECT TOP 1 a.PCI_PN FROM AV_NemoSiteLogs AS a WHERE a.Id>x.id AND a.Cell='Serving' AND a.Band=@Cell  ORDER  BY x.Id DESC),'') 'PCI'
						FROM AV_NemoSiteLogs  x INNER JOIN AV_LogsInfo AS lg ON x.fileID_Fk=lg.fileID
						WHERE lg.SiteId = @SiteId and lg.BandId=@BandId and lg.CarrierId=@CarrierId and lg.NetworkModeId=@NetworkModeId AND lg.fileType='CW'
						AND x.Latitude IS NOT NULL --AND x.Cell='Serving'
					) x
					WHERE x.PCI!=0					
				) x
				FOR XML PATH('')
			),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' cwPlot,
		
		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+'SPR002'+'</name><description>'+'SPR002'+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(
				SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY x.id) as nvarchar)+'">'+
				'<LineStyle>'+
				'<color>ff' + SUBSTRING(RIGHT(x.[Color],6),5,2)+ SUBSTRING(RIGHT(x.[Color],6),3,2) +SUBSTRING(RIGHT(x.[Color],6),1,2)+ '</color>'+
				'<width>10</width>'+
				'</LineStyle>'+
				'</Style>'+
				'<Placemark>'+
				'<name>cw1</name>'+
				'<styleUrl>#Id'+CAST(ROW_NUMBER() OVER (ORDER BY x.id) as nvarchar)+'</styleUrl>'+
				'<LineString>'+
				'<altitudeMode>relative</altitudeMode>'+
				'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
				'</LineString>'+
				'</Placemark>'  + ' '
				FROM
				(
					SELECT x.id,x.Latitude,
					x.Longitude, x.PCI,
					ISNULL((SELECT as1.sectorColor FROM AV_Sectors AS as1 WHERE as1.SiteId=x.siteID AND as1.NetworkModeId=x.networkModeID AND as1.BandId=x.bandID AND as1.CarrierId=x.carrierID AND as1.PCI=x.PCI),'#87898A') 'Color'
					FROM
					(
						SELECT ROW_NUMBER() OVER (ORDER BY x.id ASC) 'id', lg.siteID, lg.networkModeID, lg.bandID, lg.carrierID, lg.scopeID, x.[Time],x.Latitude,x.Longitude
						,ISNULL((SELECT TOP 1 a.PCI_PN FROM AV_NemoSiteLogs AS a WHERE a.Id>x.id AND a.Cell='Serving' AND a.Band=@Cell  ORDER  BY x.Id DESC),'') 'PCI'
						FROM AV_NemoSiteLogs  x INNER JOIN AV_LogsInfo AS lg ON x.fileID_Fk=lg.fileID
						WHERE lg.SiteId = @SiteId and lg.BandId=@BandId and lg.CarrierId=@CarrierId and lg.NetworkModeId=@NetworkModeId AND lg.fileType='CCW'
						AND x.Latitude IS NOT NULL --AND x.Cell='Serving'
					) x
					WHERE x.PCI!=0				
				) x
				FOR XML PATH('')
			),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' ccwPlot,

		'' rsrpPlot,

		'' rsrqPlot,

		'' sinrPlot,
		
		'' chPlot,
		
		
		'' cellPlot
		

	end
END