-- =============================================
-- Author:		/*----MoB!----*/
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_ManageReDrive]
	@Filter NVARCHAR(MAX),
	@ReDriveId NUMERIC(18, 0) = NULL,
	@ReDriveTypeId NUMERIC(18, 0) = NULL,
	@ReasonId NUMERIC(18, 0) = NULL,
	@Description NVARCHAR(MAX),
	@SiteId NUMERIC(18, 0) = NULL,	
	@NetworkModeId NUMERIC(18, 0) = NULL,
	@BandId NUMERIC(18, 0)= NULL,
	@CarrierId NUMERIC(18, 0) = NULL,
	@ScopId NUMERIC(18, 0) = NULL,
	@UserId NUMERIC(18, 0) = NULL
AS
BEGIN
		
	IF @Filter='Insert'
	BEGIN
		DECLARE @orgWoRefId AS NVARCHAR(70)=''
		DECLARE @orgSiteId AS INT=0
		DECLARE @orgSiteCode AS NVARCHAR(50)=''
		DECLARE @orgScopeId AS NUMERIC=0
		
		SET @orgWoRefId=ISNULL((SELECT DISTINCT anls.PWoRefID FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.NetworkModeId=@NetworkModeId AND anls.BandId=@BandId AND anls.CarrierId=@CarrierId),'')
		
		IF (ISNULL(@orgWoRefId,'')='')
		BEGIN
			SET @orgWoRefId=ISNULL((SELECT as1.WoRefId FROM AV_Sites AS as1 WHERE as1.SiteId=@SiteId),'')
		END
		
		SELECT @orgSiteId=ISNULL(as1.SiteId,0), @orgSiteCode=ISNULL(as1.SiteCode,''),@orgScopeId=ISNULL(as1.ScopeId,0)
		FROM AV_Sites AS as1 WHERE as1.WoRefId=@orgWoRefId
		
		DECLARE @tmpScope AS NVARCHAR(10)=(SELECT ad.DefinationName FROM AD_Definations AS ad WHERE ad.DefinationId=@orgScopeId)
		
		DECLARE @maxWoID as int=0
		DECLARE @RedriveSiteID as int=0
		DECLARE @WOStatusID AS INT=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.KeyCode='PENDING_SCHEDULED')
		DECLARE @RedriveCount AS INT=ISNULL((SELECT as1.RedriveCount FROM AV_Sites AS as1 WHERE as1.SiteId=@orgSiteId),0)
		DECLARE @Client AS INT=(select ClientId from AV_Sites Where SiteId=@SiteId)
		SET @maxWoID=(Select ISNULL(MAX(WoCode),0) + 1 from AV_Sites WHERE ClientId=@Client AND (YEAR(AV_Sites.SubmittedOn)* 100 + MONTH(AV_Sites.SubmittedOn) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))
		DECLARE @woRefID as nvarchar(25)
		SET @woRefID = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientId=@Client)+'-'+@tmpScope+'-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
		RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@maxWoID as nvarchar(15)),5))		
			
		INSERT INTO AV_Sites(SiteCode,Latitude,Longitude,ClusterId,ClientId,[Description],[Status],SubmittedOn,SubmittedById,IsActive,IsPublished,IsDownloaded,WoCode,WoRefId,CityId,ReceivedOn,RevisionId,ScopeId,siteAddress,SiteName,
		            SiteTypeId, SiteClassId)
		SELECT @orgSiteCode+'-R'+CAST(@RedriveCount+1 AS NVARCHAR(15)),
		sit.Latitude,sit.Longitude,sit.ClusterId,sit.ClientId,'Re-Drive Type: '+ad.DefinationName + ', Description: '+@Description,@WOStatusID,GETDATE(),@UserId,CAST(1 AS BIT),CAST(0 AS BIT),CAST(0 AS BIT),
		@maxWoID,@woRefID,sit.CityId,sit.ReceivedOn,sit.RevisionId,sit.ScopeId,sit.siteAddress, sit.SiteName, sit.SiteTypeId, sit.SiteClassId
		FROM AV_Sites sit
		LEFT OUTER JOIN AD_Definations AS ad ON ad.DefinationId=@ReDriveTypeId
		WHERE sit.SiteId=@SiteId;
				
		SELECT @RedriveSiteID=@@IDENTITY;
		
		UPDATE AV_Sites
		SET RedriveCount=@RedriveCount+1
		WHERE SiteId=@orgSiteId;
		
		UPDATE AV_NetLayerStatus 
		SET isRedrive = 1,
		redriveTypeId = @ReDriveTypeId,
		redriveComments = @Description,
		redriveReasonId = (SELECT anls.StatusReason FROM AV_NetLayerStatus AS anls WHERE CAST(anls.SiteId AS NVARCHAR(15))+'-'+CAST(anls.NetworkModeId AS NVARCHAR(15))+'-'+CAST(anls.BandId AS NVARCHAR(15))+'-'+CAST(anls.CarrierId AS NVARCHAR(15))+'-'+CAST(anls.ScopeId AS NVARCHAR(15))=CAST(AV_NetLayerStatus.SiteId AS NVARCHAR(15))+'-'+CAST(AV_NetLayerStatus.NetworkModeId AS NVARCHAR(15))+'-'+CAST(AV_NetLayerStatus.BandId AS NVARCHAR(15))+'-'+CAST(AV_NetLayerStatus.CarrierId AS NVARCHAR(15))+'-'+CAST(AV_NetLayerStatus.ScopeId AS NVARCHAR(15)))	
		WHERE CAST(SiteId AS NVARCHAR(15))+'-'+CAST(NetworkModeId AS NVARCHAR(15))+'-'+CAST(BandId AS NVARCHAR(15))+'-'+CAST(CarrierId AS NVARCHAR(15))+'-'+CAST(ScopeId AS NVARCHAR(15)) IN
		(
			SELECT CAST(SiteId AS NVARCHAR(15))+'-'+CAST(NetworkModeId AS NVARCHAR(15))+'-'+CAST(BandId AS NVARCHAR(15))+'-'+CAST(CarrierId AS NVARCHAR(15))+'-'+CAST(ScopeId AS NVARCHAR(15))
			FROM AV_NetLayerStatus AS anls
			WHERE anls.SiteId=@SiteId AND anls.[Status]=93 AND anls.IsActive=1
		)
		AND [Status]=93
		
		IF @RedriveSiteID>0
		BEGIN
			INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,sectorColor,
			RFHeight,MTilt, ETilt, BandWidth, CellId, MRBTS)
			SELECT DISTINCT sec.SectorCode,sec.NetworkModeId, sec.ScopeId, sec.BandId, sec.CarrierId, sec.Antenna, sec.BeamWidth, sec.Azimuth, sec.PCI,@RedriveSiteID 'SiteId', sec.sectorColor,
			sec .RFHeight, sec.MTilt, sec.ETilt, sec.BandWidth, sec.CellId, sec.MRBTS
			FROM AV_Sectors sec
			INNER JOIN AV_NetLayerStatus nls ON nls.SiteId=sec.SiteId AND nls.NetworkModeId=sec.NetworkModeId AND nls.BandId=sec.BandId AND nls.CarrierId=sec.CarrierId AND nls.ScopeId=sec.ScopeId
			WHERE sec.SiteId=@SiteId AND nls.[Status]=93 AND sec.isActive=1 AND nls.IsActive=1
			
			--SELECT @maxWoID,@RedriveSiteID,@WOStatusID,@RedriveCount,@Client,@woRefID
										
			INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],isRedrive,
			            redriveTypeId, redriveReasonId, PWoRefID, redriveComments,IsActive)
			SELECT @RedriveSiteID,nls.NetworkModeId, nls.ScopeId, nls.BandId,nls.CarrierId,sit.ReceivedOn,GETDATE(),@UserId,@WOStatusID,
			CAST(0 AS BIT),@ReDriveTypeId,nls.StatusReason,sit.WoRefId,@Description,CAST(1 AS BIT)
			FROM AV_NetLayerStatus nls
			INNER JOIN AV_Sites sit ON nls.SiteId=sit.SiteId 
			WHERE nls.SiteId=@SiteId AND sit.SiteId=@SiteId AND nls.[Status]=93 AND nls.IsActive=1
			AND CAST(nls.SiteId AS NVARCHAR(15))+'-'+CAST(nls.NetworkModeId AS NVARCHAR(15))+'-'+CAST(nls.BandId AS NVARCHAR(15))+'-'+CAST(nls.CarrierId AS NVARCHAR(15))+'-'+CAST(nls.ScopeId AS NVARCHAR(15)) IN
			(
				SELECT CAST(SiteId AS NVARCHAR(15))+'-'+CAST(NetworkModeId AS NVARCHAR(15))+'-'+CAST(BandId AS NVARCHAR(15))+'-'+CAST(CarrierId AS NVARCHAR(15))+'-'+CAST(ScopeId AS NVARCHAR(15))
				FROM AV_NetLayerStatus AS anls
				WHERE anls.SiteId=@SiteId AND anls.[Status]=93 AND anls.IsActive=1
			)
		END		
	END
END