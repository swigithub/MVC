

CREATE PROCEDURE [dbo].[AV_ManageSectors]
	@SECTORS Tb_Sectors READONLY
	,@Filter NVARCHAR(50)=NULL
	,@Value1 NVARCHAR(50)=NULL
	,@Value2 NVARCHAR(50)=NULL
AS

IF @Filter='Set_isActive'
BEGIN
	-- @Value1 = SectorId,@Value2=isActive
	UPDATE AV_Sectors
	SET isActive=@Value2
	WHERE SectorId=@Value1
END


IF NOT EXISTS (Select * from AV_Sectors Where SiteId = ( Select DISTINCT SiteId from @SECTORS))
BEGIN
	
	
	INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,SectorColor)
	SELECT x.SectorCode,x.NetworkModeId,x.ScopeId,x.BandId,x.CarrierId,x.Antenna,x.BeamWidth,x.Azimuth,x.PCI,x.SiteId,
	--(SELECT y.ColorCode FROM AD_Definations y WHERE y.DefinationName=x.SectorCode)
	(SELECT y.SectorColor FROM AV_SectorColors y WHERE y.ScopeId=x.ScopeId AND y.sectorCode=x.SectorCode)
	FROM @SECTORS x 
	WHERE CAST(x.siteId AS NVARCHAR(15))+'-'+x.SectorCode+'-'+CAST(x.networkModeId AS NVARCHAR(15))+'-'+CAST(x.BandId AS NVARCHAR(15))+'-'+CAST(x.CarrierId AS NVARCHAR(15))+'-'+CAST(x.Azimuth AS NVARCHAR(15))+'-'+CAST(x.PCI AS NVARCHAR(15))
	NOT IN
	(
		SELECT DISTINCT CAST(sec.siteId AS NVARCHAR(15))+'-'+sec.SectorCode+'-'+CAST(sec.networkModeId AS NVARCHAR(15))+'-'+CAST(sec.BandId AS NVARCHAR(15))+'-'+CAST(sec.CarrierId AS NVARCHAR(15))+'-'+CAST(sec.Azimuth AS NVARCHAR(15))+'-'+CAST(sec.PCI AS NVARCHAR(15))		
		FROM AV_Sectors sec
	)
END

INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive)
SELECT DISTINCT x.SiteId,x.NetworkModeId,x.ScopeId,x.BandId,x.CarrierId,GETDATE(),GETDATE(),29,90,CAST(1 AS BIT) FROM @SECTORS x 
WHERE CAST(x.siteId AS NVARCHAR(15))+'-'+CAST(x.networkModeId AS NVARCHAR(15))+'-'+CAST(x.BandId AS NVARCHAR(15))+'-'+CAST(x.CarrierId AS NVARCHAR(15))
NOT IN
(
	SELECT DISTINCT CAST(sec.siteId AS NVARCHAR(15))+'-'+CAST(sec.networkModeId AS NVARCHAR(15))+'-'+CAST(sec.BandId AS NVARCHAR(15))+'-'+CAST(sec.CarrierId AS NVARCHAR(15))
	FROM AV_NetLayerStatus sec
)



			
				
--IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID AND as1.NetworkModeId=@NetworkMode AND as1.BandId=@Band AND as1.CarrierId=@Carrier AND as1.SectorCode=@SectorCode AND as1.Azimuth=@Azimuth AND as1.PCI=@PCI)
--BEGIN
--	INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId)
--	VALUES(@SectorCode,@NetworkMode,@Scope,@Band,@Carrier,@Antenna,@Beamwidth,@Azimuth,@PCI,@SiteID);
--END
					
--IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID AND x.NetworkModeId=@NetworkMode AND x.BandId=@Band AND x.CarrierId=@Carrier)
--BEGIN							
	
--	VALUES(@SiteID,@NetworkMode,@Scope,@Band,@Carrier,@ReceivedOn,GETDATE(),@SubmittedById,@WOStatusID,CAST(1 AS BIT));						
--END