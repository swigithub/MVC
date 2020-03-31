
CREATE PROCEDURE [dbo].[AV_ScheduleSite_O]
	@SiteId numeric(18,0)
	,@TesterId numeric(18,0)
	,@TesterassignedById numeric(18,0)
	,@SchduledOn datetime
	,@Status varchar(50),
	 @NetworkModeId varchar(50),
	 @BandId varchar(50),
	 @CarrierId varchar(50)
	 ,@UserDeviceId numeric(18,0)
	 ,@TestTypes varchar(50)=null
	
AS
BEGIN
	DECLARE @ClientId AS INT=0;
	DECLARE @CityId AS INT=0;
	DECLARE @ScopeId AS INT=0
	DECLARE @WoRefId AS NVARCHAR(50)=''
	
	SELECT @ClientId=as1.ClientId, @CityId=as1.CityId,@WoRefId=as1.WoRefId, @ScopeId=as1.ScopeId
	FROM AV_Sites AS as1 WHERE as1.SiteId=@SiteId;
	
IF CAST(LEFT(@SchduledOn,12) AS DATETIME)>=CAST(LEFT(GETDATE(),12) AS DATETIME)
BEGIN
	IF NOT EXISTS(SELECT x.Id FROM AV_SiteConfigurations x WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId)
	BEGIN
		INSERT INTO AV_SiteConfigurations(ClientId,CityId,SiteId,RevisionId,TestTypeId,KpiId,KpiValue,TestCategoryId,ConfigurationDate,NetworkModeId,BandId)
		SELECT atc.ClientId, atc.CityId,@SiteId,1,atc.TestTypeId, atc.KpiId,atc.KpiValue, atc.TestCategoryId,GETDATE(),atc.NetworkModeId,atc.BandId
		FROM AV_TestConfigurations AS atc
		WHERE atc.ClientId=@ClientId AND atc.CityId=@CityId AND atc.NetworkModeId=@NetworkModeId AND atc.BandId=@BandId;
	END
	--Update AV_Sites
	--SET TesterId = @TesterId,ScheduledOn = @SchduledOn,TesterAssignedById = @TesterassignedById, Status = @Status,IsPublished=CAST(1 as bit), AssignedOn = GETDATE(),
	--PublishedOn=GETDATE()
	--Where SiteId = @SiteId
	
	--Update AV_Sites
	--SET TesterId = @TesterId,
	--ScheduledOn = @SchduledOn,
	--TesterAssignedById = @TesterassignedById,
	--Status = @Status,IsPublished=CAST(1 as bit),
	--AssignedOn = GETDATE(),
	--PublishedOn=GETDATE()
	--Where SiteId = @SiteId
	
	
	IF ISNULL((SELECT TOP 1 x.UserId FROM AV_WODevices x WHERE x.SiteId=@SiteId AND x.NetworkId=@NetworkModeId AND x.BandId=@BandId AND x.CarrierId=@CarrierId),0)>0 AND ISNULL(@TesterId,0)>0
	BEGIN
		UPDATE AV_WoDevices
		SET UserId = @TesterId,
			UserDeviceId = @UserDeviceId, IsDownlaoded = CAST(0 AS BIT)
		WHERE SiteId=@SiteId AND NetworkId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId;
		
		UPDATE AV_NetLayerStatus
		SET TesterId = @TesterId, ScheduledOn = @SchduledOn, AssignedOn = GETDATE(),[Status] = 91
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId;
	END
	ELSE IF ISNULL(@TesterId,0)>0
	BEGIN
		IF (SELECT TOP 1 x.[Status] FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId AND x.CarrierId=@CarrierId)=90
		BEGIN
			UPDATE AV_NetLayerStatus
			SET TesterId = @TesterId,
			AssignedOn = GETDATE(),
			ScheduledOn = @SchduledOn,
			ScheduledById = @TesterassignedById,
			[Status] = @Status,
			IsPublished=CAST(1 as bit),PublishedOn=GETDATE()	
			Where SiteId = @SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId;
		
			IF @UserDeviceId>0
			BEGIN
				INSERT INTO [dbo].[AV_WoDevices]([BandId],[CarrierId],[NetworkId],[UserId],[UserDeviceId],[SiteId],ScopeId,WoRefId,TestTypes)
				VALUES(@BandId,@CarrierId,@NetworkModeId,@TesterId,@UserDeviceId,@SiteId,36,@WoRefId,@TestTypes)
			END
			ELSE
			BEGIN
				INSERT INTO [dbo].[AV_WoDevices]([BandId],[CarrierId],[NetworkId],[UserId],[UserDeviceId],[SiteId],ScopeId,WoRefId,TestTypes)
				SELECT @BandId,@CarrierId,@NetworkModeId,sud.UserId,sud.DeviceId,@SiteId,36,@WoRefId,@TestTypes
				FROM Sec_UserDevices AS sud
				WHERE sud.isActive=1 AND sud.UserId=@TesterId
			END
		
		
			DECLARE @IsPublished BIT=1;
			DECLARE @RevisionId int=1

			SET @IsPublished=CAST(1 AS BIT)
			SET @RevisionId=ISNULL((SELECT MAX(x.RevisionId) FROM AV_SiteConfigurations x WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId),1)
		
 
			   IF @IsPublished=1 AND (SELECT COUNT(x.SummaryId) FROM AV_SiteTestSummary x WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId AND x.CarrierId=@CarrierId)=0
			   BEGIN
					INSERT INTO AV_SiteTestSummary(ClientId,RegionId,Region,CityId,City,ClusterId,Cluster,siteId,[Site],SiteScheduleDate,SectorId,Sector,ScopeId,Scope,NetworkModeId,NetworkMode,
							BandId,Band,CarrierId,Carrier,Antenna,Azimuth,
							PciId,BeamWidth,PingHost,LatencyRate,PingIterations,DownlinkRate,UplinkRate,ConnectionSetupTime,MoMtCallNo,MoMtCallDuration,VMoMtCallNo,VMoMtCallDuration,FtpServerIp,
							FtpServerPort,FtpServerPath,FtpDownlinkFile, Latitude, Longitude,MoStatus,MtStatus,VMoStatus,VMtStatus)
					SELECT sit.ClientId,rgn.DefinationId 'RegionId',rgn.DefinationName 'Region',cty.DefinationId 'CityId',cty.DefinationName 'City',
					cls.ClusterId,cls.ClusterCode 'Cluster',sit.SiteId,sit.SiteCode 'Site',GETDATE() 'SiteScheduleDate',sec.SectorId,sec.SectorCode 'Sector',
					scp.DefinationId 'ScopeId',scp.DefinationName 'Scope',ntm.DefinationId 'NetworkModeId',ntm.DefinationName 'NetworkMode', bnd.DefinationId 'BandId', bnd.DefinationName 'Band', crr.DefinationId 'CarrierId',crr.DefinationName 'Carrier',
					sec.Antenna, sec.Azimuth,sec.PCI 'PciId',sec.BeamWidth,
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId AND x.RevisionId=@RevisionId AND y.KeyCode='PING_HOST') 'PingHost',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId  AND x.RevisionId=@RevisionId AND y.KeyCode='PING_RATE') 'LatencyRate',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId  AND x.RevisionId=@RevisionId AND y.KeyCode='PING_ITERATION') 'PingIterations',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId  AND x.RevisionId=@RevisionId AND y.KeyCode='DL_RATE') 'DownlinkRate',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId  AND x.RevisionId=@RevisionId AND y.KeyCode='UL_RATE') 'UplinkRate',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId  AND x.RevisionId=@RevisionId AND y.KeyCode='SETUP_TIME') 'ConnectionSetupTime',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId  AND x.RevisionId=@RevisionId AND y.KeyCode='MO_CALL_NO') 'MoMtCallNo',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId  AND x.RevisionId=@RevisionId AND y.KeyCode='MO_CALL_DURATION') 'MoMtCallDuration',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId  AND x.RevisionId=@RevisionId AND y.KeyCode='VMO_CALL_NO') 'VMoMtCallNo',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId  AND x.RevisionId=@RevisionId AND y.KeyCode='VMO_CALL_DURATION') 'VMoMtCallDuration',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId  AND x.RevisionId=@RevisionId AND y.KeyCode='FTP_IP_ADDRESS') 'FtpServerIp',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId AND x.RevisionId=@RevisionId AND y.KeyCode='FTP_PORT') 'FtpServerPort',
					(SELECT TOP 1 CAST(x.KpiValue as nvarchar(200)) FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId AND x.RevisionId=@RevisionId AND y.KeyCode='FTP_FOLDER_PATH') 'FtpServerPath',
					(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId AND x.RevisionId=@RevisionId AND y.KeyCode='FTP_DOWNLOAD_FILE') 'FtpDownlinkFile',
					sit.Latitude, sit.Longitude, NULL 'MoStatus', NULL 'MtStatus',NULL 'VMoStatus', NULL 'VMtStatus'
					--CAST(1 as bit) 'MoStatus', CAST(1 as bit) 'MtStatus', CAST(1 as bit) 'VMoStatus', CAST(1 as bit) 'VMtStatus'
					FROM AV_Sites sit
					INNER JOIN AV_Clusters cls on sit.ClusterId=cls.ClusterId
					INNER JOIN AV_Sectors sec on sec.SiteId=sit.SiteId
					INNER JOIN AD_Definations cty ON cty.DefinationId=cls.CityId
					INNER JOIN AD_Definations rgn ON rgn.DefinationId=cty.PDefinationId
					INNER JOIN AD_Definations scp ON scp.DefinationId=sec.ScopeId
					INNER JOIN AD_Definations ntm ON ntm.DefinationId=sec.NetworkModeId
					INNER JOIN AD_Definations bnd ON bnd.DefinationId=sec.BandId
					INNER JOIN AD_Definations crr ON crr.DefinationId=sec.CarrierId
					WHERE sit.SiteId=@SiteId AND sec.NetworkModeId=@NetworkModeId AND sec.BandId=@BandId AND sec.CarrierId=@CarrierId;
			END	
		END
	END
END
ELSE
BEGIN
	RAISERROR('Site Schedule Not Allowed in Previous Dates!',16,1)
END
END