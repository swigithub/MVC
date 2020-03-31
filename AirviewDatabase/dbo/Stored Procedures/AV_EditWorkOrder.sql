CREATE PROCEDURE [dbo].[AV_EditWorkOrder]
	@SiteId NUMERIC(18,0)
	,@SiteCode NVARCHAR(50)
	,@Latitude FLOAT
	,@Longitude FLOAT
	,@Description NVARCHAR(500)=NULL
	,@CityId NUMERIC(18,0)=NULL
	,@UserId INT=0
	,@SiteAddress NVARCHAR(500)=NULL
	,@Sectors Tb_AV_Sectors READONLY
AS
BEGIN
	
	
	IF (SELECT x.[Status] FROM AV_Sites x WHERE x.SiteId=@SiteId) IN(90,91,450,92)
	BEGIN
		
	DECLARE @SectorId numeric
	DECLARE @SectorCode NVARCHAR(50)
	DECLARE @NetworkModeId NUMERIC
	DECLARE @BandId NUMERIC
	DECLARE @CarrierId NUMERIC
	DECLARE @ScopeId NUMERIC
	DECLARE @Antenna NVARCHAR(50)
	DECLARE @BeamWidth FLOAT
	DECLARE @Azimuth FLOAT
	DECLARE @PCI INT
	DECLARE @sSiteId NUMERIC
	DECLARE @MRBTS NVARCHAR(50)
	
	DECLARE @TransactionDetail NVARCHAR(1000)=''	
	
	DECLARE @ClusterId AS NUMERIC
	DECLARE @ClusterCode AS NVARCHAR(50)
	DECLARE @CityName AS NVARCHAR(150)=(SELECT ad.DefinationName FROM AD_Definations AS ad WHERE ad.DefinationId=@CityId)
	DECLARE @Region AS NVARCHAR(150)=(SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=(SELECT ad.PDefinationId FROM AD_Definations AS ad WHERE ad.DefinationId=@CityId))
	DECLARE @RegionID AS numeric=(SELECT ad.PDefinationId FROM AD_Definations AS ad WHERE ad.DefinationId=@CityId)

	SELECT TOP 1 @ClusterId=ac.ClusterId, @ClusterCode=ac.ClusterCode
	FROM AV_Clusters AS ac
	WHERE ac.CityId=@CityId

	UPDATE AV_Sites
	SET ClusterId = @ClusterId, CityId = @CityId, [Description] = @Description
	WHERE SiteId=@SiteId

	UPDATE AV_SiteTestSummary
	SET ClusterId = @ClusterId,
	CityId = @cityId,
	City = @CityName,
	RegionId = @RegionID,
	Region = @Region
	WHERE SiteId=@SiteId

	UPDATE AV_SiteTestLog
	SET ClusterId = @ClusterId, CityId = @cityId, City = @CityName,RegionId = @RegionID, Region = @Region
	WHERE SiteId=@SiteId
	
	--IF NOT EXISTS(SELECT SiteCode FROM AV_Sites WHERE SiteCode=@SiteCode)
	--BEGIN
		UPDATE AV_Sites
		SET Latitude = @Latitude, Longitude = @Longitude
		WHERE SiteId=@SiteId;
		
		UPDATE AV_SiteTestSummary
		SET Latitude = @Latitude, Longitude = @Longitude
		WHERE SiteId=@SiteId;
		
		--UPDATE AV_SiteTestLog
		--SET Site = @SiteCode
		--WHERE SiteId=@SiteId;
		
		DECLARE @woStatus AS INT=0;
		SELECT @woStatus=as1.[Status] FROM AV_Sites AS as1 WHERE as1.SiteId=@SiteId;
		
		SET @TransactionDetail='Site:'+@SiteCode
		
		DECLARE db_sector CURSOR FOR  
		SELECT x.SectorId, x.SectorCode, x.NetworkModeId, x.ScopeId, x.BandId, x.CarrierId, x.Antenna, x.BeamWidth, x.Azimuth, x.PCI, x.SiteId, x.MRBTS
		FROM @Sectors x
		OPEN db_sector   
		FETCH NEXT FROM db_sector INTO @SectorId, @SectorCode, @NetworkModeId, @ScopeId, @BandId, @CarrierId, @Antenna, @BeamWidth, @Azimuth, @PCI, @sSiteId, @MRBTS

		WHILE @@FETCH_STATUS = 0   
		BEGIN  
			IF ISNULL(@SectorId,0)>0
			BEGIN 
				DECLARE @ColorHexCode as nvarchar(10)=(SELECT x.ColorCode FROM AD_Definations x WHERE x.DefinationName=@SectorCode AND x.DefinationTypeId=20)	
				DECLARE @oldPCI AS NVARCHAR(10)=(SELECT s.PCI FROM AV_Sectors AS s WHERE s.SiteId=@SiteId AND s.SectorId=@SectorId)
				
				IF (SELECT COUNT(*) FROM AV_Sectors AS as1 WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND Pci=@oldPCI)=0
				BEGIN
					UPDATE AV_SiteTestLog
					SET pciColor = '#87898A'
					WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@oldPCI AND TestType IN('CW','CCW')
				END
				ELSE
				BEGIN
					UPDATE AV_SiteTestLog
					--SET pciColor = (SELECT DISTINCT as1.sectorColor FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteId AND as1.NetworkModeId=@NetworkModeId AND as1.BandId=@BandId AND as1.CarrierId=@CarrierId AND as1.Pci=@oldPCI)
					SET pciColor = (SELECT DISTINCT as1.sectorColor FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteId AND as1.SectorId=@SectorId)
					WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@oldPCI AND TestType IN('CW','CCW')
				END
				
				UPDATE AV_Sectors
				SET BeamWidth = @BeamWidth,
				Azimuth = @Azimuth,
				PCI = @PCI,
				SectorCode = @SectorCode,
				sectorColor = @ColorHexCode,
				MRBTS=@MRBTS
				--CarrierId = @CarrierId
				WHERE SiteId=@SiteId AND sectorId=@SectorId;
			
				UPDATE AV_SiteTestSummary
				SET BeamWidth = @BeamWidth,
				Azimuth = @Azimuth,
				PciId = @PCI,
				Sector = @SectorCode
				--CarrierId = @CarrierId, Carrier = (SELECT ad.DefinationName FROM AD_Definations AS ad WHERE ad.DefinationId=@CarrierId)				
				WHERE SiteId=@SiteId AND sectorId=@SectorId;			
							
				
				UPDATE AV_SiteTestLog
				SET pciColor = @ColorHexCode
				WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@PCI AND TestType IN('CW','CCW')
				
				SET @TransactionDetail=''
				SET @TransactionDetail = 'Site:'+@SiteCode+', Sector:'+CAST(@SectorCode AS NVARCHAR(15))+', BW:'+CAST(@BeamWidth AS NVARCHAR(15))+', Azimuth:'+CAST(@Azimuth AS NVARCHAR(15))+', CID:'+CAST(@PCI AS NVARCHAR(15))+', Carrier:'+(SELECT ad.DefinationName FROM AD_Definations AS ad WHERE ad.DefinationId=@CarrierId)
				
				INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource,Description)
				VALUES(@SiteId,@NetworkModeId,@BandId,@CarrierId,NULL, @woStatus,@UserId,HOST_NAME(),GETDATE(),'AV_EditWorkOrder',@TransactionDetail);
				
				IF (@woStatus NOT IN(90,450,91,92,451,93,89))
				BEGIN
					UPDATE AV_Sectors
					SET CarrierId = @CarrierId
					WHERE SiteId=@SiteId AND sectorId=@SectorId;
			
					UPDATE AV_SiteTestSummary
					SET CarrierId = @CarrierId,
					Carrier = (SELECT ad.DefinationName FROM AD_Definations AS ad WHERE ad.DefinationId=@CarrierId)				
					WHERE SiteId=@SiteId AND sectorId=@SectorId;
					
					
				
					DELETE FROM AV_NetLayerStatus
					WHERE SiteId=@SiteId 
					--AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND ScopeId=@ScopeId
					AND CarrierId NOT IN(SELECT DISTINCT as1.CarrierId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteId)
				END
			END
			--ELSE
			--BEGIN
			--	IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID AND as1.NetworkModeId=@NetworkModeId AND as1.ScopeId=@ScopeId AND as1.BandId=@BandId AND as1.CarrierId=@CarrierId AND as1.SectorCode=@SectorCode AND as1.Azimuth=@Azimuth AND as1.PCI=@PCI)
			--	BEGIN
			--		INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId)
			--		VALUES(@SectorCode,@NetworkModeId,@ScopeId,@BandId,@CarrierId,@Antenna,@BeamWidth,@Azimuth,@PCI,@SiteId)
				
			--		IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId AND x.CarrierId=@CarrierId AND x.ScopeId=@ScopeId)
			--		BEGIN	
			--			DECLARE @WOStatusID AS INT=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.KeyCode='PENDING_SCHEDULED')						
			--			--INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive)
			--			--VALUES(@SiteID,@NetworkModeId,@ScopeId,@BandId,@CarrierId,@ReceivedOn,GETDATE(),@SubmittedById,@WOStatusID,CAST(1 AS BIT));	
						
			--			INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,UploadedOn,UploadedById,[Status],IsActive)
			--			VALUES(@SiteID,@NetworkModeId,@ScopeId,@BandId,@CarrierId,GETDATE(),@UserId,@WOStatusID,CAST(1 AS BIT));					
			--		END
			--	END
			--END					   
		FETCH NEXT FROM db_sector INTO @SectorId, @SectorCode, @NetworkModeId, @ScopeId, @BandId, @CarrierId, @Antenna, @BeamWidth, @Azimuth, @PCI, @sSiteId, @MRBTS
		END   
		CLOSE db_sector   
		DEALLOCATE db_sector
		
		--UPDATE AV_SiteTestLog
		--SET pciColor = '#87898A'
		--WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND TestType IN('CW','CCW')
		
		
		UPDATE AV_SiteTestLog
		SET pciColor = ISNULL((SELECT x.sectorColor FROM AV_Sectors x WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId AND x.CarrierId=@CarrierId AND x.PCI=AV_SiteTestLog.PciId),'#87898A')
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND TestType IN('CW','CCW')
	--END
	--ELSE
	--BEGIN
	--	RAISERROR('Site Code Already Exists!',16,1)
	--END
	
	END
	ELSE
	BEGIN
		RAISERROR('Site Edit not Allowed!',16,1)
	END	
	
END