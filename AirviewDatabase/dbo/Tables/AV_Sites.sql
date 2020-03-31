CREATE TABLE [dbo].[AV_Sites] (
    [SiteId]             NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteCode]           NVARCHAR (50)  NOT NULL,
    [Latitude]           FLOAT (53)     NOT NULL,
    [Longitude]          FLOAT (53)     NOT NULL,
    [ClusterId]          NUMERIC (18)   NOT NULL,
    [ClientId]           NUMERIC (18)   NOT NULL,
    [TesterId]           NUMERIC (18)   CONSTRAINT [DF_AV_Sites_TesterId] DEFAULT ((0)) NOT NULL,
    [Description]        NVARCHAR (500) NULL,
    [Status]             NUMERIC (18)   NOT NULL,
    [SubmittedOn]        DATETIME       CONSTRAINT [DF_AV_Sites_SubmittedOn] DEFAULT (getdate()) NULL,
    [AssignedOn]         DATETIME       NULL,
    [CompletedOn]        DATETIME       NULL,
    [ScheduledOn]        DATETIME       NULL,
    [DriveCompletedOn]   DATETIME       NULL,
    [DownloadedOn]       DATETIME       NULL,
    [SubmittedById]      NUMERIC (18)   NOT NULL,
    [TesterAssignedById] NCHAR (10)     CONSTRAINT [DF_AV_Sites_TesterAssignedById] DEFAULT ((0)) NOT NULL,
    [IsActive]           BIT            CONSTRAINT [DF_AV_Sites_IsActive] DEFAULT ((1)) NOT NULL,
    [IsPublished]        BIT            CONSTRAINT [DF_AV_Sites_IsPublished] DEFAULT ((0)) NULL,
    [PublishedOn]        DATETIME       NULL,
    [IsDownloaded]       BIT            CONSTRAINT [DF_AV_Sites_IsDownloaded] DEFAULT ((0)) NULL,
    [WoCode]             NUMERIC (18)   NULL,
    [WoRefId]            NVARCHAR (100) NULL,
    [CityId]             NUMERIC (18)   NULL,
    [ReceivedOn]         DATETIME       CONSTRAINT [DF_AV_Sites_ReceivedOn] DEFAULT (getdate()) NULL,
    [ReportSubmittedOn]  DATETIME       NULL,
    [RedriveCount]       INT            NULL,
    [InactiveById]       INT            NULL,
    [ApprovedComments]   NVARCHAR (500) NULL,
    [HoldById]           INT            NULL,
    [SiteName]           NVARCHAR (50)  NULL,
    [SiteTypeId]         NUMERIC (18)   NULL,
    [SiteClassId]        NUMERIC (18)   NULL,
    [RevisionId]         INT            NULL,
    [ScopeId]            NUMERIC (18)   NULL,
    [SiteAddress]        NVARCHAR (500) NULL,
    [ProjectId]          NUMERIC (18)   NULL,
    [IsSpecialAccess]    BIT            NULL,
    [AccessDateTime]     DATETIME       NULL,
    [AccessInstructions] NVARCHAR (500) NULL,
    CONSTRAINT [PK_AV_Sites] PRIMARY KEY CLUSTERED ([SiteId] ASC),
    CONSTRAINT [FK_AV_Sites_AD_Clients] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[AD_Clients] ([ClientId]),
    CONSTRAINT [FK_AV_Sites_AV_Clusters] FOREIGN KEY ([ClusterId]) REFERENCES [dbo].[AV_Clusters] ([ClusterId])
);


GO
CREATE TRIGGER [dbo].[AV_loadSiteTestSummary]
       ON dbo.AV_Sites
AFTER UPDATE
AS
BEGIN
		DECLARE @SiteId INT=0
		DECLARE @NetworkModeId INT=0
		DECLARE @BandId INT=0
		DECLARE @IsPublished BIT=0
		DECLARE @RevisionId int=1		
		

		SELECT @SiteId = INSERTED.SiteId, @IsPublished=CAST(INSERTED.IsPublished AS BIT)
		FROM INSERTED;
	   
		SET @RevisionId=ISNULL((SELECT MAX(x.RevisionId) FROM AV_SiteConfigurations x WHERE x.SiteId=@SiteId),1)
		
 
       IF @IsPublished=1 AND (SELECT COUNT(x.SummaryId) FROM AV_SiteTestSummary x WHERE x.SiteId=@SiteId)=0
       BEGIN
		    INSERT INTO AV_SiteTestSummary(ClientId,RegionId,Region,CityId,City,ClusterId,Cluster,siteId,[Site],SiteScheduleDate,SectorId,Sector,ScopeId,Scope,NetworkModeId,NetworkMode,
					BandId,Band,CarrierId,Carrier,Antenna,Azimuth,
					PciId,BeamWidth,PingHost,LatencyRate,PingIterations,DownlinkRate,UplinkRate,ConnectionSetupTime,MoMtCallNo,MoMtCallDuration,VMoMtCallNo,VMoMtCallDuration,FtpServerIp,
					FtpServerPort,FtpServerPath,FtpDownlinkFile, Latitude, Longitude,MoStatus,MtStatus,VMoStatus,VMtStatus)
			SELECT sit.ClientId,rgn.DefinationId 'RegionId',rgn.DefinationName 'Region',cty.DefinationId 'CityId',cty.DefinationName 'City',
			cls.ClusterId,cls.ClusterCode 'Cluster',sit.SiteId,sit.SiteCode 'Site',sit.AssignedOn 'SiteScheduleDate',sec.SectorId,sec.SectorCode 'Sector',
			scp.DefinationId 'ScopeId',scp.DefinationName 'Scope',ntm.DefinationId 'NetworkModeId',ntm.DefinationName 'NetworkMode', bnd.DefinationId 'BandId', bnd.DefinationName 'Band', crr.DefinationId 'CarrierId',crr.DefinationName 'Carrier',
			sec.Antenna, sec.Azimuth,sec.PCI 'PciId',sec.BeamWidth,
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId AND x.RevisionId=@RevisionId AND y.KeyCode='PING_HOST') 'PingHost',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId  AND x.RevisionId=@RevisionId AND y.KeyCode='PING_RATE') 'LatencyRate',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId  AND x.RevisionId=@RevisionId AND y.KeyCode='PING_ITERATION') 'PingIterations',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId  AND x.RevisionId=@RevisionId AND y.KeyCode='DL_RATE') 'DownlinkRate',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId  AND x.RevisionId=@RevisionId AND y.KeyCode='UL_RATE') 'UplinkRate',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId  AND x.RevisionId=@RevisionId AND y.KeyCode='SETUP_TIME') 'ConnectionSetupTime',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId  AND x.RevisionId=@RevisionId AND y.KeyCode='MO_CALL_NO') 'MoMtCallNo',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId  AND x.RevisionId=@RevisionId AND y.KeyCode='MO_CALL_DURATION') 'MoMtCallDuration',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId  AND x.RevisionId=@RevisionId AND y.KeyCode='VMO_CALL_NO') 'VMoMtCallNo',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId  AND x.RevisionId=@RevisionId AND y.KeyCode='VMO_CALL_DURATION') 'VMoMtCallDuration',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId  AND x.RevisionId=@RevisionId AND y.KeyCode='FTP_IP_ADDRESS') 'FtpServerIp',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.NetworkModeId=ntm.DefinationId AND x.BandId=bnd.DefinationId  AND x.RevisionId=@RevisionId AND y.KeyCode='FTP_PORT') 'FtpServerPort',
			(SELECT TOP 1 CAST(x.KpiValue as nvarchar(200)) FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.RevisionId=@RevisionId AND y.KeyCode='FTP_FOLDER_PATH') 'FtpServerPath',
			(SELECT TOP 1 x.KpiValue FROM AV_SiteConfigurations x INNER JOIN AD_Definations y ON x.KpiId=y.DefinationId WHERE x.SiteId=@SiteId AND x.RevisionId=@RevisionId AND y.KeyCode='FTP_DOWNLOAD_FILE') 'FtpDownlinkFile',
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
			WHERE sit.SiteId=@SiteId              
       END
END
GO
DISABLE TRIGGER [dbo].[AV_loadSiteTestSummary]
    ON [dbo].[AV_Sites];


GO

CREATE TRIGGER [dbo].[AV_TransferSiteTestLogs]
       ON [dbo].[AV_Sites]
AFTER UPDATE
AS
BEGIN
		DECLARE @SiteId INT=0
		DECLARE @Status AS INT=0		
		

		SELECT @SiteId = INSERTED.SiteId
		FROM INSERTED;
	   
		SET @Status =(SELECT x.[Status] FROM AV_Sites x WHERE x.SiteId=@SiteId)
		
		IF @Status=89
		BEGIN
			INSERT INTO AirView_DW_Approved.dbo.AV_SiteTestLog
			(
				LogId,RegionId,Region,CityId,City,TestCategoryId,TestCategory,TestTypeId,TestType,[TimeStamp],ClusterId,Cluster,SiteId,
				[Site],SectorId,Sector,CellId,LacId,PciId,MccId,MncId,Latitude,Longitude,ScopeId,Scope,NetworkModeId,NetworkMode,BandId,Band,CarrierId,Carrier,GsmRssi,GsmRxQual,WcdmaRssi,
				WcdmaRscp,WcdmaEcio,LteRssi,LteRsrp,LteRsrq,LteRsnr,LteCqi,DistanceFromSite,AngleToSite,FtpStatus,StackTrace,TestResult,MoStatus,MtStatus,VolteMoStatus,VolteMtStatus,ConnectionSetupTime,
				SubNetworkMode,ActualBand,ActualCarrier,TestStatus,IsHandover,IsActive,serverTimeStamp,pciColor,rsrpColor,rsrqColor,sinrColor,dlColor,ChColor
			)
			SELECT * FROM AV_SiteTestLog x
			WHERE x.SiteId=@SiteId
			
			DELETE FROM AV_SiteTestLog
			WHERE SiteId=@SiteId
		END
     
END
GO
DISABLE TRIGGER [dbo].[AV_TransferSiteTestLogs]
    ON [dbo].[AV_Sites];

