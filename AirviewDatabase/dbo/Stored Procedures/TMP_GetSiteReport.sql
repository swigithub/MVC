-- EXEC [dbo].[TMP_GetSiteReport] 10097, 73, 76, 15, 11, 'report'
CREATE PROCEDURE TMP_GetSiteReport
	@SiteId NUMERIC(18,0),
	@NetworkModeId NUMERIC(18,0),
	@BandId NUMERIC(18,0),
	@CarrierId NUMERIC(18,0),
	@ScopeId NUMERIC(18,0),
	@PlotId NUMERIC(18,0),
	@NodeId NUMERIC(18,0),
	@UserId NUMERIC(18,0) = 11,
	@ControlDetailType NVARCHAR(50) = null,
	@ChartType NVARCHAR(50) = null,
	@WhereClause NVARCHAR(MAX) = null,
	@Filter nvarchar(50)
AS
BEGIN
	DECLARE @Scope as nvarchar(50) = (SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=@ScopeId)
	DECLARE @CityId AS NUMERIC
	DECLARE @ClientId AS NUMERIC
	DECLARE @SiteCode AS NVARCHAR(50)
	
	DECLARE @strSQL AS NVARCHAR(MAX)=''
	DECLARE @strSQL2 AS NVARCHAR(MAX)=''


	SELECT @CityId=as1.CityId, @ClientId=as1.ClientId, @SiteCode=as1.SiteCode FROM AV_Sites AS as1
	WHERE as1.SiteId=@SiteId
	
	IF @Filter = 'GET_PAGE_DATA'
	BEGIN
		SET @strSQL =  'SELECT DISTINCT sts.Region,sts.City,sts.Site,sts.NetworkMode,sts.Band,sts.Carrier,sts.Scope, sts.SiteScheduleDate, sts.Sector
		FROM AV_SiteTestSummary sts
		WHERE sts.SiteId= ' + CAST(@SiteId AS NVARCHAR(15)) + ' AND sts.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND sts.BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID= ' + CAST(@CarrierId AS NVARCHAR(15)) + '
		AND sts.ScopeId= ' + CAST(@ScopeId AS NVARCHAR(15)) + ' AND ' + @WhereClause
		
		EXEC (@strSQL)

		EXEC [TMP_GetNodeSettings] 'GET_BY_NODEID', @NodeId

	END

	IF @Filter = 'GET_MAP_DATA'
	BEGIN

		IF @ControlDetailType = 'PCI'

			BEGIN
				
				SET @strSQL =  'SELECT ''<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</name><description>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</description>''+
					REPLACE(REPLACE(REPLACE(
						(SELECT ''<Style id="Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''">''+
						''<LineStyle>''+
						''<color>ff'' + SUBSTRING(RIGHT(rsrpColor,6),5,2)+ SUBSTRING(RIGHT(rsrpColor,6),3,2) +SUBSTRING(RIGHT(rsrpColor,6),1,2)+ ''</color>''+
						''<width>10</width>''+
						''</LineStyle>''+
						''</Style>''+
						''<Placemark>''+
						''<name>cw1</name>''+
						''<styleUrl>#Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''</styleUrl>''+
						''<LineString>''+
						''<altitudeMode>relative</altitudeMode>''+
						''<coordinates>''+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0 ''+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0''+''</coordinates>''+
						''</LineString>''+
						''</Placemark>''  + '' ''
					FROM AV_SiteTestLog x
					WHERE SiteId =  ' + CAST(@SiteId AS NVARCHAR(15)) + ' and BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' and CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + ' and x.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND x.TestType IN(''CW'',''CCW'') AND '+ @WhereClause +'
					AND x.IsActive=1 
					FOR XML PATH('''')),''&lt;/'',''</''),''&gt;'',''>''),''&lt;'',''<'')+
					''</Document></kml>'' Plot, ''PCI'' PlotName, ''' + CAST(@SiteCode AS NVARCHAR(15)) + ''' SiteCode	'
	
					EXEC (@strSQL)
				
			END

		 IF @ControlDetailType = 'RSRP'
		BEGIN


			SET @strSQL =  'SELECT ''<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</name><description>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</description>''+
					REPLACE(REPLACE(REPLACE(
						(SELECT ''<Style id="Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''">''+
						''<LineStyle>''+
						''<color>ff'' + SUBSTRING(RIGHT(rsrqColor,6),5,2)+ SUBSTRING(RIGHT(rsrqColor,6),3,2) +SUBSTRING(RIGHT(rsrqColor,6),1,2)+ ''</color>''+
						''<width>10</width>''+
						''</LineStyle>''+
						''</Style>''+
						''<Placemark>''+
						''<name>cw1</name>''+
						''<styleUrl>#Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''</styleUrl>''+
						''<LineString>''+
						''<altitudeMode>relative</altitudeMode>''+
						''<coordinates>''+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0 ''+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0''+''</coordinates>''+
						''</LineString>''+
						''</Placemark>''  + '' ''
					FROM AV_SiteTestLog x
					WHERE SiteId =  ' + CAST(@SiteId AS NVARCHAR(15)) + ' and BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' and CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + ' and x.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND x.TestType IN(''CW'',''CCW'') AND '+ @WhereClause +'
					AND x.IsActive=1 
					FOR XML PATH('''')),''&lt;/'',''</''),''&gt;'',''>''),''&lt;'',''<'')+
					''</Document></kml>'' Plot, ''RSRP'' PlotName, ''' + CAST(@SiteCode AS NVARCHAR(15)) + ''' SiteCode	'

					EXEC (@strSQL)

		
		END
		 IF @ControlDetailType = 'RSRQ'
		BEGIN
			

				SET @strSQL =  'SELECT ''<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</name><description>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</description>''+
					REPLACE(REPLACE(REPLACE(
						(SELECT ''<Style id="Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''">''+
						''<LineStyle>''+
						''<color>ff'' + SUBSTRING(RIGHT(rsrqColor,6),5,2)+ SUBSTRING(RIGHT(rsrqColor,6),3,2) +SUBSTRING(RIGHT(rsrqColor,6),1,2)+ ''</color>''+
						''<width>10</width>''+
						''</LineStyle>''+
						''</Style>''+
						''<Placemark>''+
						''<name>cw1</name>''+
						''<styleUrl>#Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''</styleUrl>''+
						''<LineString>''+
						''<altitudeMode>relative</altitudeMode>''+
						''<coordinates>''+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0 ''+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0''+''</coordinates>''+
						''</LineString>''+
						''</Placemark>''  + '' ''
					FROM AV_SiteTestLog x
					WHERE SiteId =  ' + CAST(@SiteId AS NVARCHAR(15)) + ' and BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' and CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + ' and x.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND x.TestType IN(''CW'',''CCW'') AND '+ @WhereClause +'
					AND x.IsActive=1 
					FOR XML PATH('''')),''&lt;/'',''</''),''&gt;'',''>''),''&lt;'',''<'')+
					''</Document></kml>'' Plot, ''RSRQ'' PlotName, ''' + CAST(@SiteCode AS NVARCHAR(15)) + ''' SiteCode	'
	
					EXEC (@strSQL)


		END
		 IF @ControlDetailType = 'SINR'
		BEGIN
			
				SET @strSQL =  'SELECT ''<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</name><description>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</description>''+
					REPLACE(REPLACE(REPLACE(
						(SELECT ''<Style id="Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''">''+
						''<LineStyle>''+
						''<color>ff'' + SUBSTRING(RIGHT(sinrColor,6),5,2)+ SUBSTRING(RIGHT(sinrColor,6),3,2) +SUBSTRING(RIGHT(sinrColor,6),1,2)+ ''</color>''+
						''<width>10</width>''+
						''</LineStyle>''+
						''</Style>''+
						''<Placemark>''+
						''<name>cw1</name>''+
						''<styleUrl>#Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''</styleUrl>''+
						''<LineString>''+
						''<altitudeMode>relative</altitudeMode>''+
						''<coordinates>''+ CAST(CAST(Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0 ''+CAST(CAST(Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0''+''</coordinates>''+
						''</LineString>''+
						''</Placemark>''  + '' ''
					FROM AV_SiteTestLog
					WHERE SiteId =  ' + CAST(@SiteId AS NVARCHAR(15)) + ' and BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' and CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + ' and NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND TestType IN(''CW'',''CCW'') AND '+ @WhereClause +'
					AND IsActive=1 
					FOR XML PATH('''')),''&lt;/'',''</''),''&gt;'',''>''),''&lt;'',''<'')+
					''</Document></kml>'' Plot, ''SINR'' PlotName, ''' + CAST(@SiteCode AS NVARCHAR(15)) + ''' SiteCode	'
	

					EXEC (@strSQL)

		END
		 IF @ControlDetailType = 'HO'
		BEGIN




		SET @strSQL =  'SELECT ''<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</name><description>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</description>''+
					REPLACE(REPLACE(REPLACE(
						(SELECT CASE WHEN ISNULL(x.IsHandover,0)=0 THEN
						''<Style id="Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''">''+
						''<LineStyle>''+
						''<color>ff'' + SUBSTRING(RIGHT(pciColor,6),5,2)+ SUBSTRING(RIGHT(pciColor,6),3,2) +SUBSTRING(RIGHT(pciColor,6),1,2)+ ''</color>''+
						''<width>10</width>''+
						''</LineStyle>''+
						''</Style>''+
						''<Placemark>''+
						''<name>cw1</name>''+
						''<styleUrl>#Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''</styleUrl>''+
						''<LineString>''+
						''<altitudeMode>relative</altitudeMode>''+
						''<coordinates>''+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0 ''+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0''+''</coordinates>''+
						''</LineString>''+
						''</Placemark>'' 

						WHEN ISNULL(x.IsHandover,0)=1 THEN 
						''<Style id="hoMarker''+ CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar) + ''">'' +
						''<IconStyle><Icon><scale>0.6</scale><href>'' + CASE WHEN x.City=''Chicago'' THEN ''http://96.57.107.148/Content/Images/Common/hoHand.png'' ELSE ''http://96.57.107.148/Content/Images/Colors/handover_x16.png''  END + ''</href></Icon></IconStyle>''+
						''</Style>''+       
						''<Placemark>''+
						''<name>''+x.PciId+''</name>''+
						''<description>''+x.PciId+''</description>''+
						''<styleUrl>#hoMarker''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''</styleUrl>''+
						''<Point>''+
						''<coordinates>''+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0 ''+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0''+''</coordinates>''+
						''</Point>''+
						''</Placemark>''        
						END +  + '' ''

					FROM AV_SiteTestLog x
					WHERE SiteId =  ' + CAST(@SiteId AS NVARCHAR(15)) + ' and BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' and CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + ' and x.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND x.TestType IN(''CW'',''CCW'') AND '+ @WhereClause +'
					AND x.IsActive=1 
					FOR XML PATH('''')),''&lt;/'',''</''),''&gt;'',''>''),''&lt;'',''<'')+
					''</Document></kml>'' Plot, ''HO'' PlotName, ''' + CAST(@SiteCode AS NVARCHAR(15)) + ''' SiteCode'

					EXEC (@strSQL)

		END
		 IF @ControlDetailType = 'CW'
		BEGIN





		SET @strSQL =  'SELECT ''<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</name><description>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</description>''+
					REPLACE(REPLACE(REPLACE(
						(SELECT CASE WHEN ISNULL(x.IsHandover,0)=0 THEN
						''<Style id="Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''">''+
						''<LineStyle>''+
						''<color>ff'' + SUBSTRING(RIGHT(pciColor,6),5,2)+ SUBSTRING(RIGHT(pciColor,6),3,2) +SUBSTRING(RIGHT(pciColor,6),1,2)+ ''</color>''+
						''<width>10</width>''+
						''</LineStyle>''+
						''</Style>''+
						''<Placemark>''+
						''<name>cw1</name>''+
						''<styleUrl>#Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''</styleUrl>''+
						''<LineString>''+
						''<altitudeMode>relative</altitudeMode>''+
						''<coordinates>''+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0 ''+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0''+''</coordinates>''+
						''</LineString>''+
						''</Placemark>'' 

						WHEN ISNULL(x.IsHandover,0)=1 THEN 
						''<Style id="hoMarker''+ CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar) + ''">'' +
						''<IconStyle><Icon><href>'' + CASE WHEN x.City=''Chicago'' THEN ''http://96.57.107.148/Content/Images/Common/hoHand.png'' ELSE ''http://96.57.107.148/Content/Images/Colors/handover_x16.png''  END + ''</href></Icon></IconStyle>''+
						''</Style>''+       
						''<Placemark>''+
						''<name>''+x.PciId+''</name>''+
						''<description>This is a new point to learn KML format.</description>''+
						''<styleUrl>#hoMarker''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''</styleUrl>''+
						''<Point>''+
						''<coordinates>''+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0 ''+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0''+''</coordinates>''+
						''</Point>''+
						''</Placemark>''        
						END +  + '' ''

					FROM AV_SiteTestLog x
					WHERE SiteId =  ' + CAST(@SiteId AS NVARCHAR(15)) + ' and BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' and CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + ' and x.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND x.TestType IN(''CW'') AND '+ @WhereClause +'
					AND x.IsActive=1 
					FOR XML PATH('''')),''&lt;/'',''</''),''&gt;'',''>''),''&lt;'',''<'')+
					''</Document></kml>'' Plot, ''CW'' PlotName, ''' + CAST(@SiteCode AS NVARCHAR(15)) + ''' SiteCode'




					EXEC (@strSQL)

		END
		 IF @ControlDetailType = 'CCW'
		BEGIN
			

			SET @strSQL =  'SELECT ''<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</name><description>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</description>''+
					REPLACE(REPLACE(REPLACE(
						(SELECT CASE WHEN ISNULL(x.IsHandover,0)=0 THEN
						''<Style id="Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''">''+
						''<LineStyle>''+
						''<color>ff'' + SUBSTRING(RIGHT(pciColor,6),5,2)+ SUBSTRING(RIGHT(pciColor,6),3,2) +SUBSTRING(RIGHT(pciColor,6),1,2)+ ''</color>''+
						''<width>10</width>''+
						''</LineStyle>''+
						''</Style>''+
						''<Placemark>''+
						''<name>cw1</name>''+
						''<styleUrl>#Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''</styleUrl>''+
						''<LineString>''+
						''<altitudeMode>relative</altitudeMode>''+
						''<coordinates>''+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0 ''+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0''+''</coordinates>''+
						''</LineString>''+
						''</Placemark>'' 

						WHEN ISNULL(x.IsHandover,0)=1 THEN 
						''<Style id="hoMarker''+ CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar) + ''">'' +
						''<IconStyle><Icon><scale>0.6</scale><href>'' + CASE WHEN x.City=''Chicago'' THEN ''http://96.57.107.148/Content/Images/Common/hoHand.png'' ELSE ''http://96.57.107.148/Content/Images/Colors/handover_x16.png''  END + ''</href></Icon></IconStyle>''+
						''</Style>''+       
						''<Placemark>''+
						''<name>''+x.PciId+''</name>''+
						''<description>''+x.PciId+''</description>''+
						''<styleUrl>#hoMarker''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''</styleUrl>''+
						''<Point>''+
						''<coordinates>''+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0 ''+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0''+''</coordinates>''+
						''</Point>''+
						''</Placemark>''        
						END +  + '' ''

					FROM AV_SiteTestLog x
					WHERE SiteId =  ' + CAST(@SiteId AS NVARCHAR(15)) + ' and BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' and CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + ' and x.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND x.TestType IN(''CCW'') AND '+ @WhereClause +'
					AND x.IsActive=1 
					FOR XML PATH('''')),''&lt;/'',''</''),''&gt;'',''>''),''&lt;'',''<'') +
					''</Document></kml>'' Plot, ''CCW'' PlotName, ''' + CAST(@SiteCode AS NVARCHAR(15)) + ''' SiteCode'


					EXEC (@strSQL)

		END
		IF @ControlDetailType = 'CH'
		BEGIN

				SET @strSQL =  'SELECT ''<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</name><description>'+ CAST(@SiteCode AS NVARCHAR(15)) +'</description>''+
					REPLACE(REPLACE(REPLACE(
						(SELECT ''<Style id="Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''">''+
						''<LineStyle>''+
						''<color>ff'' + SUBSTRING(RIGHT(ChColor,6),5,2)+ SUBSTRING(RIGHT(ChColor,6),3,2) +SUBSTRING(RIGHT(ChColor,6),1,2)+ ''</color>''+
						''<width>10</width>''+
						''</LineStyle>''+
						''</Style>''+
						''<Placemark>''+
						''<name>cw1</name>''+
						''<styleUrl>#Id''+CAST(ROW_NUMBER() OVER (ORDER BY logid) as nvarchar)+''</styleUrl>''+
						''<LineString>''+
						''<altitudeMode>relative</altitudeMode>''+
						''<coordinates>''+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0 ''+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',''+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+'',0''+''</coordinates>''+
						''</LineString>''+
						''</Placemark>''  + '' ''
					FROM AV_SiteTestLog x
					WHERE SiteId =  ' + CAST(@SiteId AS NVARCHAR(15)) + ' and BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' and CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + ' and x.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND x.TestType IN(''CW'',''CCW'') AND ' + @WhereClause + '
					AND x.IsActive=1 
					FOR XML PATH('''')),''&lt;/'',''</''),''&gt;'',''>''),''&lt;'',''<'')+
					''</Document></kml>'' Plot, ''CH'' PlotName, ''' + CAST(@SiteCode AS NVARCHAR(15)) + ''' SiteCode	'
	

					EXEC (@strSQL)

		END
		
		SET @strSQL =  ' SELECT CAST(arl.PCI AS NVARCHAR) ''PCI'', arl.sectorColor ''SectorColor'', arl.BeamWidth, arl.Azimuth, as1.SiteCode ''Site'', arl.SectorCode ''Sector'', arl.SectorLatitude ''Latitude'', arl.SectorLongitude ''Longitude''
				FROM AV_Sectors AS arl
				INNER JOIN AV_Sites AS as1 ON as1.SiteId=arl.SiteId
				WHERE arl.SiteId =  ' + CAST(@SiteId AS NVARCHAR(15)) + ' and BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' and CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + ' and NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' 
				ORDER BY arl.NetworkModeId,arl.BandId,arl.CarrierId,
				CASE WHEN arl.SectorCode=''Alpha'' THEN 1
					 WHEN arl.SectorCode=''Beta'' THEN 2
					 WHEN arl.SectorCode=''Gamma'' THEN 3
					 WHEN arl.SectorCode=''Delta'' THEN 4
					 WHEN arl.SectorCode=''Epsilon'' THEN 5
					 WHEN arl.SectorCode=''DiGamma'' THEN 6
					 WHEN arl.SectorCode=''Iota'' THEN 7
				END '

		EXEC (@strSQL)

		-- getting legends logic 
		IF @ControlDetailType IN('PCI')
		BEGIN
			SET @strSQL =  ' SELECT CAST(arl.PCI AS NVARCHAR) ''Legend'', arl.sectorColor ''Color''
					FROM AV_Sectors AS arl			
					WHERE SiteId = ' + CAST(@SiteId AS NVARCHAR(15)) + '  and BandId= ' + CAST(@BandId AS NVARCHAR(15)) + '  and CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + '  and NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + '
					ORDER BY arl.NetworkModeId,arl.BandId,arl.CarrierId,
					CASE WHEN arl.SectorCode=''Alpha'' THEN 1
							WHEN arl.SectorCode=''Beta'' THEN 2
							WHEN arl.SectorCode=''Gamma'' THEN 3
							WHEN arl.SectorCode=''Delta'' THEN 4
							WHEN arl.SectorCode=''Epsilon'' THEN 5
							WHEN arl.SectorCode=''DiGamma'' THEN 6
							WHEN arl.SectorCode=''Iota'' THEN 7
					END '

					EXEC (@strSQL)
		END
		ELSE IF @ControlDetailType IN('RSRP')
		BEGIN
			SET @strSQL =  'SELECT CAST(arl.rangeFrom AS NVARCHAR) + '' to '' +  CAST(arl.rangeTo AS NVARCHAR) ''Legend'', arl.rangeColor ''Color''
			FROM AV_RFPlotLegends AS arl
			WHERE arl.ClientId= ' + CAST(@ClientId AS NVARCHAR(15)) + ' AND arl.CityId=0 AND arl.PlotTypeId=485 AND arl.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + '
			AND arl.rangeFrom!=rangeTo'

			EXEC (@strSQL)
		END
		ELSE IF @ControlDetailType IN('RSRQ')
		BEGIN
			SET @strSQL =  ' SELECT CAST(arl.rangeFrom AS NVARCHAR) + '' to '' +  CAST(arl.rangeTo AS NVARCHAR) ''Legend'', arl.rangeColor ''Color''
			FROM AV_RFPlotLegends AS arl
			WHERE arl.ClientId= ' + CAST(@ClientId AS NVARCHAR(15)) + ' AND arl.CityId=0 AND arl.PlotTypeId=486 AND arl.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + '
			AND arl.rangeFrom!=rangeTo ' 

			EXEC (@strSQL)
		END
		ELSE IF @ControlDetailType IN('SINR')
		BEGIN
			SET @strSQL =  ' SELECT CAST(arl.rangeFrom AS NVARCHAR) + '' to '' +  CAST(arl.rangeTo AS NVARCHAR) ''Legend'', arl.rangeColor ''Color''
			FROM AV_RFPlotLegends AS arl
			WHERE arl.ClientId= ' + CAST(@ClientId AS NVARCHAR(15)) + ' AND arl.CityId=0 AND arl.PlotTypeId=487 AND arl.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + '
			AND arl.rangeFrom!=rangeTo '

			EXEC (@strSQL)
		END
		ELSE IF @ControlDetailType IN('SINR')
		BEGIN
			SET @strSQL =  ' SELECT CAST(arl.rangeFrom AS NVARCHAR) + '' to '' +  CAST(arl.rangeTo AS NVARCHAR) ''Legend'', arl.rangeColor ''Color''
			FROM AV_RFPlotLegends AS arl
			WHERE arl.ClientId= ' + CAST(@ClientId AS NVARCHAR(15)) + ' AND arl.CityId=0 AND arl.PlotTypeId=487 AND arl.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + '
			AND arl.rangeFrom!=rangeTo '

			EXEC (@strSQL)
		END
		ELSE IF @ControlDetailType IN('CH')
		BEGIN
			SET @strSQL =  'SELECT DISTINCT astl.Carrier ''Legend'', astl.ChColor ''Color''
			FROM AV_SiteTestLog AS astl
			WHERE astl.SiteId= ' + CAST(@SiteId AS NVARCHAR(15)) + ' AND astl.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND astl.BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' AND astl.CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + '
			AND astl.TestType IN(''CW'',''CCW'') '

			EXEC (@strSQL)
		END
		ELSE IF @ControlDetailType IN('PCI','CW','CCW','HO')
		BEGIN
			IF @Scope IN('SSV','NI')
			BEGIN
				SET @strSQL =  ' SELECT x.PCI ''Legend'',x.SectorColor ''Color''
				FROM AV_Sectors x
				WHERE x.SiteId =  ' + CAST(@SiteId AS NVARCHAR(15)) + ' and x.BandId = ' + CAST(@BandId AS NVARCHAR(15)) + ' and x.CarrierId = ' + CAST(@CarrierId AS NVARCHAR(15)) + ' and x.NetworkModeId =  ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' and x.isactive=1 '

				EXEC (@strSQL)
			END
		END


		EXEC [TMP_GetNodeSettings] 'GET_BY_NODEID', @NodeId

	END

	IF @Filter = 'GET_TABLE_DATA'
	BEGIN
		
		SET @strSQL =  'SELECT sts.Site ''Site'', sts.Sector ''Sector'', sts.NetworkMode ''Network Mode'',sts.Band,sts.Carrier, sts.Latitude, sts.Longitude, sts.Azimuth, sts.PciId ''PCI''
		FROM AV_SiteTestSummary sts
		WHERE sts.SiteId= ' + CAST(@SiteId AS NVARCHAR(15)) + ' AND sts.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' and sts.BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID= ' + CAST(@CarrierId AS NVARCHAR(15)) + '
		AND sts.ScopeId= ' + CAST(@ScopeId AS NVARCHAR(15)) + ' AND ' + @WhereClause

		EXEC (@strSQL)

		EXEC [TMP_GetNodeSettings] 'GET_BY_NODEID', @NodeId
		
	END


	IF @Filter = 'GET_OOKLA_DATA'
	BEGIN
		
		SET @strSQL =  ' SELECT sts.OoklaTestFilePath FROM AV_SiteTestSummary sts WHERE
		sts.SiteId= ' + CAST(@SiteId AS NVARCHAR(15)) + ' AND sts.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' and sts.BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID= ' + CAST(@CarrierId AS NVARCHAR(15)) + '
		AND sts.ScopeId= ' + CAST(@ScopeId AS NVARCHAR(15)) + ' AND ' + @WhereClause

		EXEC (@strSQL)

		EXEC [TMP_GetNodeSettings] 'GET_BY_NODEID', @NodeId
		
	END



	IF @Filter = 'GET_TABLE_WITH_MAP_DATA'
	BEGIN
		
		SET @strSQL = 'SELECT CAST(arl.PCI AS NVARCHAR) ''PCI'', arl.sectorColor ''SectorColor'', arl.BeamWidth, arl.Azimuth, as1.SiteCode ''Site'', arl.SectorCode ''Sector'', arl.SectorLatitude ''Latitude'', arl.SectorLongitude ''Longitude''
			FROM AV_Sectors AS arl
			INNER JOIN AV_Sites AS as1 ON as1.SiteId=arl.SiteId
			WHERE arl.SiteId =  ' + CAST(@SiteId AS NVARCHAR(15)) + ' and BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' and CarrierId= ' + CAST(@CarrierId AS NVARCHAR(15)) + ' and NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) +'
			ORDER BY arl.NetworkModeId,arl.BandId,arl.CarrierId,
			CASE WHEN arl.SectorCode=''Alpha'' THEN 1
				 WHEN arl.SectorCode=''Beta'' THEN 2
				 WHEN arl.SectorCode=''Gamma'' THEN 3
				 WHEN arl.SectorCode=''Delta'' THEN 4
				 WHEN arl.SectorCode=''Epsilon'' THEN 5
				 WHEN arl.SectorCode=''DiGamma'' THEN 6
				 WHEN arl.SectorCode=''Iota'' THEN 7
			END '
			
		--SET @strSQL2 = 'SELECT sts.Site, sts.Sector, sts.NetworkMode,sts.Band,sts.Carrier, sts.Latitude, sts.Longitude, sts.Azimuth, sts.PciId ''PCI''
		--FROM AV_SiteTestSummary sts INNER JOIN AV_SectorColors sc ON sts.SectorId = sc.SectorId
		--WHERE sts.SiteId= ' + CAST(@SiteId AS NVARCHAR(15)) + ' AND sts.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND sts.BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID= ' + CAST(@CarrierId AS NVARCHAR(15)) + '		--AND sts.ScopeId= ' + CAST(@ScopeId AS NVARCHAR(15)) + ' AND ' + @WhereClause


		SET @strSQL2 = 'SELECT DISTINCT sts.Site, sts.Sector, sts.NetworkMode AS ''Network Mode'',sts.Band,sts.Carrier, sts.Latitude, sts.Longitude, sts.Azimuth, sts.PciId ''PCI''
		FROM AV_SiteTestSummary sts INNER JOIN AV_SectorColors sc ON sts.Sector = sc.SectorCode
		WHERE sts.SiteId= ' + CAST(@SiteId AS NVARCHAR(15)) + ' AND sts.NetworkModeId= ' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' AND sts.BandId= ' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID= ' + CAST(@CarrierId AS NVARCHAR(15)) + '
		AND sts.ScopeId= ' + CAST(@ScopeId AS NVARCHAR(15)) --+ ' AND ' + @WhereClause

		EXEC (@strSQL)
		EXEC (@strSQL2)
		
		
		EXEC [TMP_GetNodeSettings] 'GET_BY_NODEID', @NodeId
		

	END

	--bar chart, pie chart, line chart, column chart, stacked column, stacked bar 
	-- @ChartType
	
	IF @Filter = 'GET_CHART_DATA'
	BEGIN 
		IF (@ControlDetailType='DL' OR @ControlDetailType='UL' OR @ControlDetailType='Ping')
		BEGIN
			IF @ChartType IN('bar chart', 'pie chart', 'line chart', 'column chart')
			BEGIN
				SET @strSQL = 'SELECT LEFT(CAST(CONVERT(TIME,sts.[TimeStamp],108) AS NVARCHAR(50)),12) AS  x,sts.TestResult y FROM AV_SiteTestLog AS sts WHERE sts.SiteId=' + CAST(@SiteId AS NVARCHAR(15))
				+ ' AND sts.NetworkModeId=' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' and sts.BandId=' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID=' + CAST(@CarrierId AS NVARCHAR(15))
				+ ' AND sts.ScopeId=' + CAST(@ScopeId AS NVARCHAR(15)) + ' AND sts.TestType=''' + @ControlDetailType + ''' AND ' + @WhereClause
			
			END
			ELSE IF @ChartType IN('stacked column', 'stacked bar')
			BEGIN

				SET @strSQL = 'SELECT sts.Sector AS  x, MIN(sts.TestResult) y, ''MIN'' AS Type '
				+ ' FROM AV_SiteTestLog AS sts '
				+ ' WHERE sts.SiteId= '+CAST(@SiteId AS NVARCHAR(15))+' AND sts.NetworkModeId= '+CAST(@NetworkModeId AS NVARCHAR(15))+' AND sts.BandId= '+CAST(@BandId AS NVARCHAR(15)) 
				+ ' AND sts.CarrierID= '+CAST(@CarrierId AS NVARCHAR(15))
				+ ' AND sts.ScopeId= '+CAST(@ScopeId AS NVARCHAR(15))+' AND sts.TestType= '''+CAST(@ControlDetailType AS NVARCHAR(15))+''' '
				+ ' GROUP BY sts.Sector'
				+ ' UNION ALL'
				+ ' SELECT sts.Sector AS  x,AVG(sts.TestResult) y, ''AVG'' AS Type'
				+ ' FROM AV_SiteTestLog AS sts '
				+ ' WHERE sts.SiteId='+CAST(@SiteId AS NVARCHAR(15))+' AND sts.NetworkModeId='+CAST(@NetworkModeId AS NVARCHAR(15))+' AND sts.BandId=' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID=' + CAST(@CarrierId AS NVARCHAR(15))
				+ ' AND sts.ScopeId='+CAST(@ScopeId AS NVARCHAR(15))+' AND sts.TestType= '''+CAST(@ControlDetailType AS NVARCHAR(15))+''' '
				+ ' GROUP BY sts.Sector'
				+ ' UNION ALL'
				+ ' SELECT sts.Sector AS  x,MAX(sts.TestResult) y, ''MAX'' AS Type'
				+ ' FROM AV_SiteTestLog AS sts '
				+ ' WHERE sts.SiteId='+CAST(@SiteId AS NVARCHAR(15))+' AND sts.NetworkModeId='+CAST(@NetworkModeId AS NVARCHAR(15))+' AND sts.BandId=' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID=' + CAST(@CarrierId AS NVARCHAR(15))
				+ ' AND sts.ScopeId='+CAST(@ScopeId AS NVARCHAR(15))+' AND sts.TestType= '''+CAST(@ControlDetailType AS NVARCHAR(15))+''' '
				+ ' AND ' + @WhereClause +' GROUP BY sts.Sector '


			END
		END		
		ELSE IF (@ControlDetailType='Sector_DL')
		BEGIN


			SET @strSQL = 'SELECT sts.Sector AS  x,MAX(sts.TestResult) y FROM AV_SiteTestLog AS sts WHERE sts.SiteId=' + CAST(@SiteId AS NVARCHAR(15))
				+ ' AND sts.NetworkModeId=' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' and sts.BandId=' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID=' + CAST(@CarrierId AS NVARCHAR(15))
				+ ' AND sts.ScopeId=' + CAST(@ScopeId AS NVARCHAR(15)) + ' AND sts.TestType=''DL'' AND ' + @WhereClause + 'GROUP BY sts.Sector'

		END	
		ELSE IF (@ControlDetailType='Sector_UL')
		BEGIN

			SET @strSQL = 'SELECT sts.Sector AS  x,MAX(sts.TestResult) y FROM AV_SiteTestLog AS sts WHERE sts.SiteId=' + CAST(@SiteId AS NVARCHAR(15))
				+ ' AND sts.NetworkModeId=' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' and sts.BandId=' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID=' + CAST(@CarrierId AS NVARCHAR(15))
				+ ' AND sts.ScopeId=' + CAST(@ScopeId AS NVARCHAR(15)) + ' AND sts.TestType=''UL'' AND ' + @WhereClause + 'GROUP BY sts.Sector'

		END	
		ELSE IF (@ControlDetailType='Sector_Ping')
		BEGIN

			SET @strSQL = 'SELECT sts.Sector AS  x,MAX(sts.TestResult) y FROM AV_SiteTestLog AS sts WHERE sts.SiteId=' + CAST(@SiteId AS NVARCHAR(15))
				+ ' AND sts.NetworkModeId=' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' and sts.BandId=' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID=' + CAST(@CarrierId AS NVARCHAR(15))
				+ ' AND sts.ScopeId=' + CAST(@ScopeId AS NVARCHAR(15)) + ' AND sts.TestType=''Ping'' AND ' + @WhereClause + 'GROUP BY sts.Sector'

		END	
		ELSE IF @ControlDetailType='RSRP'
		BEGIN

			SET @strSQL = 'SELECT LEFT(CAST(CONVERT(TIME,sts.[TimeStamp],108) AS NVARCHAR(50)),12) x,sts.LteRsrp y FROM AV_SiteTestLog AS sts WHERE sts.SiteId=' + CAST(@SiteId AS NVARCHAR(15))
				+ ' AND sts.NetworkModeId=' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' and sts.BandId=' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID=' + CAST(@CarrierId AS NVARCHAR(15))
				+ ' AND sts.ScopeId=' + CAST(@ScopeId AS NVARCHAR(15)) + ' AND sts.TestType IN(''CW'',''CCW'') AND ' + @WhereClause 

		END
		ELSE IF @ControlDetailType='RSRQ'
		BEGIN

			SET @strSQL = 'SELECT LEFT(CAST(CONVERT(TIME,sts.[TimeStamp],108) AS NVARCHAR(50)),12) x,sts.LteRsrq y FROM AV_SiteTestLog AS sts WHERE sts.SiteId=' + CAST(@SiteId AS NVARCHAR(15))
				+ ' AND sts.NetworkModeId=' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' and sts.BandId=' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID=' + CAST(@CarrierId AS NVARCHAR(15))
				+ ' AND sts.ScopeId=' + CAST(@ScopeId AS NVARCHAR(15)) + ' AND sts.TestType IN(''CW'',''CCW'') AND ' + @WhereClause

		END
		ELSE IF @ControlDetailType='SINR'
		BEGIN

			SET @strSQL = 'SELECT LEFT(CAST(CONVERT(TIME,sts.[TimeStamp],108) AS NVARCHAR(50)),12) x,sts.LteRsnr y FROM AV_SiteTestLog AS sts WHERE sts.SiteId=' + CAST(@SiteId AS NVARCHAR(15))
				+ ' AND sts.NetworkModeId=' + CAST(@NetworkModeId AS NVARCHAR(15)) + ' and sts.BandId=' + CAST(@BandId AS NVARCHAR(15)) + ' AND sts.CarrierID=' + CAST(@CarrierId AS NVARCHAR(15))
				+ ' AND sts.ScopeId=' + CAST(@ScopeId AS NVARCHAR(15)) + ' AND sts.TestType IN(''CW'',''CCW'') AND ' + @WhereClause

		END

		EXEC (@strSQL)

		EXEC [TMP_GetNodeSettings] 'GET_BY_NODEID', @NodeId
	END


END