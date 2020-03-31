-- [dbo].[AV_GetSite] 'ById',0,0,0,0,'10159'
CREATE PROCEDURE AV_GetSite
    @Filter nvarchar(50),
	--@SiteCode nvarchar(100),
    @NetworkModeId NUMERIC(18,0)=NULL,
    @BandId NUMERIC(18,0)=NULL,
    @CarrierId NUMERIC(18,0)=NULL,
    @ScopeId NUMERIC(18,0)=NULL,
	@value nvarchar(50)=NULL
   ,@value2 nvarchar(50)=NULL
   ,@value3 nvarchar(50)=NULL
AS
BEGIN

    if @Filter='ById'
	 begin
		Select sit.SiteId, SiteCode, Latitude, Longitude, sit.ClusterId, sit.ClientId, Description, Status, SubmittedOn, AssignedOn, CompletedOn,ScheduledOn,
		AD_Definations.DefinationName AS Market, DEF.DefinationName AS Region,sec.NetworkModeId,net.DefinationName 'NetworkMode',sec.BandId,band.DefinationName 'Band',
		sec.CarrierId,car.DefinationName 'Carrier',sit.CityId,sec.ScopeId, scope.DefinationName as Scope
		from AV_Sites sit
		LEFT JOIN AV_Clusters ON AV_Clusters.ClusterId = sit.ClusterId
		LEFT JOIN AD_Clients ON AD_Clients.ClientId = sit.ClientId
		LEFT JOIN AD_Definations ON AD_Definations.DefinationId = AV_Clusters.CityId
		LEFT JOIN AD_Definations As DEF ON DEF.DefinationId = AD_Definations.PDefinationId
		left join AV_Sectors sec on sec.SiteId=sit.SiteId
		Inner Join AD_Definations As scope ON scope.DefinationId = sec.ScopeId
		left join AD_Definations net on net.DefinationId=sec.NetworkModeId
		left join AD_Definations band on band.DefinationId=sec.BandId
		left join AD_Definations car on car.DefinationId=sec.CarrierId
		 Where sit.SiteId = @value
	 end


--  [dbo].[AV_GetSite] 'ByStatusKeyCode',0,0,0,0,'93'
	ELSE if @Filter='ByStatusKeyCode'
	 begin	    
		select * from AV_Sites where Status=(select DefinationId from AD_Definations where KeyCode=@value)
	 END
	 ELSE if @Filter='SingleSitebyId'
	 begin	    
		select * from AV_Sites where SiteId=@value
	 END 
	 ELSE if @Filter='SingleSitebySiteCode'
	 begin	    
		select * from AV_Sites where SiteCode=@value
	 END
--  [dbo].[AV_GetSite] 'ByStatus',0,0,0,0,'93'
	ELSE if @Filter='ByStatus'
	 begin	    
		 select sit.SiteId,sit.WoRefId,sit.SiteCode,cl.ClientName 'Client',market.DefinationName 'Market',sit.Status
		 from AV_Sites sit
		 inner join AD_Clients cl on cl.ClientId=sit.ClientId
		 inner join AD_Definations market on market.DefinationId=sit.CityId
		  where sit.Status=@value and sit.ReceivedOn between @value2 and @value3
	 END
	 
--  [dbo].[AV_GetSite] 'BySiteCodeWithLayer',0,0,0,0,'CF001'
	ELSE if @Filter='BySiteCodeWithLayer'
	 begin	    
		select nls.LayerStatusId  'SiteId',sit.WoRefId,sit.SiteCode+': '+bnd.DefinationName+' ('+crr.DefinationName+')' 'SiteCode' ,cl.ClientName 'Client',market.DefinationName 'Market',sit.Status,cl.ClientPrefix
		 from AV_Sites sit
		 inner join AV_NetLayerStatus nls on nls.SiteId=sit.SiteId
		 inner join AD_Clients cl on cl.ClientId=sit.ClientId
		 inner join AD_Definations market on market.DefinationId=sit.CityId
		 inner join AD_Definations net on net.DefinationId=nls.NetworkModeId
		 inner join AD_Definations bnd on bnd.DefinationId=nls.BandId
		 inner join AD_Definations crr on crr.DefinationId=nls.CarrierId		 
		  where sit.SiteCode=@value AND sit.IsActive=1 --AND nls.[Status]=450
	 END


--   [dbo].[AV_GetSite] 'SiteWithSectors',162793
	 ELSE if @Filter='SiteWithSectors'
	 begin	    
		SELECT asi.*,asi.CityId 'Market',reg.PDefinationId 'Region',sta.PDefinationId 'State',cvc.ClusterCode 'ClusterCode'  FROM AV_Sites asi
		inner join AD_Definations reg on reg.DefinationId=asi.CityId
		inner join AD_Definations sta on sta.DefinationId=reg.PDefinationId
			left join AV_Clusters cvc on cvc.ClusterId=asi.ClusterId
		 WHERE SiteId=@value


		SELECT sec.*,sec.SectorLatitude 'Latitude',sec.SectorLongitude 'Longitude',avc.*,tss.SurveyId,tss.SiteSurveyId,avc.SiteClusterId FROM AV_Sectors sec
		left join AV_SiteClusters avc on avc.SiteId=sec.SiteId
		left join TSS_SiteSurvey tss on tss.SiteId=sec.SiteId
		 where  sec.SiteId=@value
		ORDER BY sec.NetworkModeId,sec.BandId,sec.CarrierId,sec.ScopeId,
		CASE WHEN sec.SectorCode='Alpha' THEN 1
			 WHEN sec.SectorCode='Beta' THEN 2
			 WHEN sec.SectorCode='Gamma' THEN 3
			 WHEN sec.SectorCode='Delta' THEN 4
			 WHEN sec.SectorCode='Epsilon' THEN 5
			 WHEN sec.SectorCode='DiGamma' THEN 6
			 WHEN sec.SectorCode='Iota' THEN 7
		END
		
		
	 END
	 
--	 [dbo].[AV_GetSite] 'SiteWithSectors',648664
	 ELSE if @Filter='By_NetLayer_SiteCode'
	 begin	    
	 --set @value=@value + '%';
		SELECT sit.*,sec.isRedrive
		FROM AV_Sites AS sit
		INNER JOIN AV_NetLayerStatus AS sec ON sec.SiteId=sit.SiteId
		WHERE sit.SiteCode like @value + '%'
		--sec.NetworkModeId=@NetworkModeId AND sec.BandId=@BandId AND sec.CarrierId=@CarrierId AND sec.ScopeId=@ScopeId AND sit.SiteCode like @value% 
		--AND ISNULL(sec.isRedrive,0)=0 
	 end
	 --	 [dbo].[AV_GetSite] 'Draw_Sectors',10096
	 ELSE if @Filter='Draw_Sectors'
	 begin	    
		SELECT DISTINCT sec.SectorCode,sec.Antenna,sec.BeamWidth,sec.Azimuth
		FROM AV_Sectors sec 
		where  sec.SiteId=@value
	 end
	 ELSE IF @Filter='GET_TEST_BY_LAYER'
	 begin
		SELECT ad.DefinationId, REPLACE(ad.DefinationName,'_PLOT','') 'DefinationName', 'UE37' 'UEId'
		FROM AD_Definations AS ad
		WHERE ad.DefinationTypeId=27 AND ad.PDefinationId=474 AND ad.DefinationName LIKE '%_PLOT'
		AND ad.DefinationName not iN('CH_PLOT','CW_PLOT','CCW_PLOT','HANDOVER_PLOT')
	 end

END