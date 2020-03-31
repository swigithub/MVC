CREATE PROCEDURE AV_GetMarketSites 
	@Filter NVARCHAR(50),
	@SiteId NVARCHAR(50)=NULL,
	@SiteCode NVARCHAR(50)=NULL,
	@Latitude FLOAT=NULL,
	@Longitude FLOAT=NULL,
	@CityId numeric(18, 0)=null
AS
BEGIN
	IF @Filter='bySiteCode'
	BEGIN		
		
		DECLARE @maxDistance as float=(SELECT MAX(DistanceFromSite) FROM AV_SiteTestLog WHERE Siteid=@SiteId and TestType IN('CW','CCW'))
		
		SELECT x.SiteCode,x.Latitude,x.Longitude,x.Azimuth,x.PCI,x.SectorColor,x.SectorCode,x.BeamWidth,x.NetworkModeId,x.BandId,x.CarrierId,x.SectorCode 'SectorId',0 'SiteId',0 'ScopeId',
		 1 'RecieverDistance', 1 'InnerDistance', 1 'OuterDistance'
		FROM AV_marketSites x
		WHERE x.SiteCode!=@SiteCode AND
		ACOS( SIN( RADIANS(x.latitude) ) * SIN( RADIANS( @Latitude ) ) + COS( RADIANS(x.latitude) )
		* COS( RADIANS( @Latitude )) * COS( RADIANS(x.longitude) - RADIANS( @Longitude )) ) * 6380 < @maxDistance+10
		UNION ALL		
		SELECT x.SiteCode,x.Latitude,x.Longitude,y.Azimuth,y.PCI,y.SectorColor,y.SectorCode,y.BeamWidth,y.NetworkModeId,y.BandId,y.CarrierId,y.SectorId,x.SiteId,x.ScopeId,
		 1 'RecieverDistance', 2 'InnerDistance', 3 'OuterDistance'
		FROM AV_Sites x inner join AV_Sectors y on x.SiteId=y.SiteId
		where y.siteid=@SiteId
	END
	--  [dbo].[AV_GetMarketSites] 'UniqueSiteCode',0,0,0,0,468
	If @Filter='UniqueSiteCode'
	begin
	 select SiteCode 
	 from AV_marketSites 
	 where CityId=@CityId
	 group by SiteCode
	end

	IF @Filter='AllbySiteCode'
	BEGIN
		SELECT *
		FROM AV_marketSites x
		WHERE x.SiteCode=@SiteCode
	END
END