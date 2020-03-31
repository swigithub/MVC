--  [dbo].[AV_GetSectors] 'GetSiteNetworkLayers','15'
CREATE PROCEDURE AV_GetSectors
@Filter nvarchar(100),
@SiteId NUMERIC(18,0) =0,
@NetworkModeId  NUMERIC(18,0) =0,
@BandId  NUMERIC(18,0) =0,
@CarrierId  NUMERIC(18,0) =0,
@ScopeId NUMERIC(18,0) =0,
@LayerId NUMERIC(18,0) =0
AS
BEGIN

--[dbo].[AV_GetSectors] 'GetSectors','648746'
	if @Filter='GetSectors'
	 BEGIN
		-- @value1 = BandId
		-- @valu2 = SiteId
		Select sec.SectorCode, sec.Azimuth,sec.SectorId, sec.PCI, sec.BeamWidth, sec.Antenna, NM.DefinationName NetworkMode, Scope.DefinationName Scope, Carrier.DefinationName Carrier,
		sts.PingStatus, sts.DownlinkStatus, sts.UplinkStatus, sts.MoStatus, sts.MtStatus, sts.CwHandoverStatus, sts.Ccwhandoverstatus, sts.VMoStatus, sts.VMtStatus,sts.SMoStatus,sts.SMtStatus,
		sts.CarrierAggregationStatus,sts.E911Status,sit.CityId,sit.ClientId,sec.SectorLatitude,sec.SectorLongitude,sec.sectorColor, sts.IRATHandover, sts.LatencyRate 'PingKpi',sts.DownlinkRate 'DLKpi',
		sts.UplinkRate 'ULKpi',sts.PingAverageResult 'AvgPing',sts.DownlinkMaxResult 'MaxDL',sts.UplinkMaxResult 'MaxUL'
		from AV_Sectors sec
		INNER JOIN AV_Sites AS sit ON sit.SiteId=sec.SiteId
		Inner Join AD_Definations NM On NM.DefinationId = sec.NetworkModeId
		Inner Join AD_Definations Scope On Scope.DefinationId = sec.ScopeId
		Inner Join AD_Definations Carrier On Carrier.DefinationId = sec.CarrierId
		left outer join AV_SiteTestSummary sts ON sts.SiteId=sec.SiteId AND sts.SectorId=sec.SectorId AND sts.NetworkModeId=sec.NetworkModeId AND sts.BandId=sec.BandId AND sts.CarrierId=sec.CarrierId
		Where  sec.SiteId = @SiteId AND sec.NetworkModeId=@NetworkModeId AND sec.CarrierId=@CarrierId and sec.BandId = @BandId AND sec.ScopeId=@ScopeId	 AND sec.isActive=1	
		ORDER BY sec.NetworkModeId,sec.BandId,sec.CarrierId,
		CASE WHEN sec.SectorCode='Alpha' THEN 1
		 WHEN sec.SectorCode='Beta' THEN 2
		 WHEN sec.SectorCode='Gamma' THEN 3
		 WHEN sec.SectorCode='Delta' THEN 4
		 WHEN sec.SectorCode='Epsilon' THEN 5
		 WHEN sec.SectorCode='DiGamma' THEN 6
		 WHEN sec.SectorCode='Iota' THEN 7
		END
		
	 END
	--	[dbo].[AV_GetSectors] 'BySiteId','398366'	 
	 IF	@Filter='BySiteId'
	 BEGIN
	 	SELECT * 
	 	FROM AV_Sectors AS sec
	 	WHERE sec.SiteId=@SiteId
	 	ORDER BY sec.NetworkModeId,sec.BandId,sec.CarrierId,
		CASE WHEN sec.SectorCode='Alpha' THEN 1
		 WHEN sec.SectorCode='Beta' THEN 2
		 WHEN sec.SectorCode='Gamma' THEN 3
		 WHEN sec.SectorCode='Delta' THEN 4
		 WHEN sec.SectorCode='Epsilon' THEN 5
		 WHEN sec.SectorCode='DiGamma' THEN 6
		 WHEN sec.SectorCode='Iota' THEN 7
		END
	 END

	--	[dbo].[AV_GetSectors] 'GetSiteNetworkLayers','398368'
	else if @Filter='GetSiteNetworkLayers' begin
		-- @value1 = SiteId
		select distinct sec.SiteId,sit.SiteCode, sec.NetworkModeId,net.DefinationName NetworkMode,sec.BandId,band.DefinationName Band,
		sec.CarrierId,crier.DefinationName Carrier,cli.ClientPrefix,sec.ScopeId,scop.DefinationName
		from AV_Sectors sec
		left join AD_Definations crier on crier.DefinationId =CarrierId 
		left join AD_Definations net on net.DefinationId =NetworkModeId
		left join AD_Definations band on band.DefinationId =sec.BandId
	    inner join AV_Sites sit on sit.SiteId=sec.SiteId
		inner join AD_Clients cli on cli.ClientId=sit.ClientId
		INNER JOIN AD_Definations scop ON scop.DefinationId=sec.ScopeId
		INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=sec.SiteId AND anls.NetworkModeId=sec.NetworkModeId AND anls.BandId=sec.BandId AND anls.CarrierId=sec.CarrierId
		where sec.SiteId=@SiteId AND anls.IsActive=1
	end
	--	[dbo].[AV_GetSectors] 'Draw_Sectors',10098
	 ELSE if @Filter='Draw_Sectors'
	 begin	    
		SELECT DISTINCT sec.SectorCode,sec.Antenna,sec.BeamWidth,sec.Azimuth,sit.Latitude,sit.Longitude		
		FROM AV_Sectors sec
		INNER JOIN AV_Sites AS sit ON sit.SiteId=sec.SiteId
		where  sec.SiteId=@SiteId	 AND sec.isActive=1
	 end
	  IF	@Filter='GetSectorsCLS'
	 BEGIN
	 	SELECT DISTINCT
     a.SequenceId,
     STUFF(
			 (SELECT DISTINCT ',' +def2.DefinationName +' ('+ def3.DefinationName+')'  
			  FROM AV_SiteScript b inner join  AD_Definations def2 on def2.DefinationId=b.EventTypeId
			  inner join AD_Definations def3 on def3.DefinationId=b.EventValue
			   WHERE b.SiteId = a.SiteId  and b.SequenceId=a.SequenceId
			   AND def2.DefinationName LIKE '%Lock%' and b.EventValue not like '%Lock%' and def3.DefinationId=b.EventValue
			 FOR XML PATH ('')
		 )
          , 1, 1, ''
		) AS pDefinationName,
		 STUFF(
			 (SELECT DISTINCT ',' +def2.DefinationName 
			  FROM AV_SiteScript b
			    inner join  AD_Definations def2 on def2.DefinationId=b.EventTypeId
			   inner join  AD_Definations pdef on pdef.DefinationId=def2.PDefinationId
			   WHERE b.SiteId = a.SiteId  and b.SequenceId=a.SequenceId and def2.DisplayText not like '%Lock%'
			 FOR XML PATH ('')
		 )
          , 1, 1, ''
		) AS DefinationName,a.NetLayerId,cs.DeviceScheduleId
FROM AV_SiteScript  AS a
inner join AV_Sites asa on asa.SiteId=a.SiteId 
inner join AV_ClusterSchedule cs on cs.SiteId=a.SiteId
WHERE a.SiteId=@SiteId and a.NetLayerId=@LayerId and cs.SequenceId=a.SequenceId order by a.SequenceId
	 END
	 IF	@Filter='GetSectorsCLSSchedule'
	 BEGIN
	 	SELECT DISTINCT
     a.SequenceId,
     STUFF(
			 (SELECT DISTINCT ',' +def2.DefinationName +' ('+ def3.DefinationName+')'  
			  FROM AV_SiteScript b inner join  AD_Definations def2 on def2.DefinationId=b.EventTypeId
			   inner join AD_Definations def3 on def3.DefinationId=b.EventValue
			   WHERE b.SiteId = a.SiteId  and b.SequenceId=a.SequenceId
			   AND def2.DefinationName LIKE '%Lock%' and b.EventValue not like '%Lock%'
			 FOR XML PATH ('')
		 )
          , 1, 1, ''
		) AS pDefinationName,
		 STUFF(
			 (SELECT DISTINCT ',' +def2.DisplayText   
			  FROM AV_SiteScript b
			   inner join  AD_Definations def2 on def2.DefinationId=b.EventTypeId
			   inner join  AD_Definations pdef on pdef.DefinationId=def2.PDefinationId
			   WHERE b.SiteId = a.SiteId  and b.SequenceId=a.SequenceId AND a.NetLayerId=a.NetLayerId and def2.DisplayText not like '%Lock%'
			    FOR XML PATH ('')
		 )
          , 1, 1, ''
		) AS DefinationName,a.NetLayerId,cs.DeviceScheduleId
FROM AV_SiteScript  AS a
inner join AV_Sites asa on asa.SiteId=a.SiteId 
inner join AV_ClusterSchedule cs on cs.SiteId=a.SiteId
WHERE a.SiteId=@SiteId and cs.SequenceId=a.SequenceId ORDER BY a.SequenceId
	 END
	IF	@Filter='ParentTester'
	 BEGIN
	 select top 1 * from AV_ClusterSchedule where SiteId=@SiteId and IsMaster=1 and IsDownloaded=0
	 END
END