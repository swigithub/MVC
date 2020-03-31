CREATE TABLE [dbo].[AV_SiteTestLog] (
    [LogId]               NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [RegionId]            NUMERIC (18)   NULL,
    [Region]              VARCHAR (100)  NULL,
    [CityId]              NUMERIC (18)   NULL,
    [City]                VARCHAR (100)  NULL,
    [TestCategoryId]      NUMERIC (18)   NULL,
    [TestCategory]        VARCHAR (100)  NULL,
    [TestTypeId]          NUMERIC (18)   NULL,
    [TestType]            VARCHAR (100)  NULL,
    [TimeStamp]           DATETIME       NULL,
    [ClusterId]           NUMERIC (18)   NULL,
    [Cluster]             NVARCHAR (50)  NULL,
    [SiteId]              NUMERIC (18)   NULL,
    [Site]                NVARCHAR (50)  NULL,
    [SectorId]            NUMERIC (18)   NULL,
    [Sector]              NVARCHAR (50)  NULL,
    [CellId]              NVARCHAR (20)  NULL,
    [LacId]               NVARCHAR (20)  NULL,
    [PciId]               NVARCHAR (20)  NULL,
    [MccId]               NVARCHAR (20)  NULL,
    [MncId]               NVARCHAR (20)  NULL,
    [Latitude]            FLOAT (53)     NULL,
    [Longitude]           FLOAT (53)     NULL,
    [ScopeId]             NUMERIC (18)   NULL,
    [Scope]               VARCHAR (100)  NULL,
    [NetworkModeId]       NUMERIC (18)   NULL,
    [NetworkMode]         VARCHAR (100)  NULL,
    [BandId]              NUMERIC (18)   NULL,
    [Band]                VARCHAR (100)  NULL,
    [CarrierId]           NUMERIC (18)   NULL,
    [Carrier]             VARCHAR (100)  NULL,
    [GsmRssi]             FLOAT (53)     NULL,
    [GsmRxQual]           FLOAT (53)     NULL,
    [WcdmaRssi]           INT            NULL,
    [WcdmaRscp]           FLOAT (53)     NULL,
    [WcdmaEcio]           FLOAT (53)     NULL,
    [LteRssi]             FLOAT (53)     NULL,
    [LteRsrp]             FLOAT (53)     NULL,
    [LteRsrq]             FLOAT (53)     NULL,
    [LteRsnr]             FLOAT (53)     NULL,
    [LteCqi]              FLOAT (53)     NULL,
    [DistanceFromSite]    FLOAT (53)     NULL,
    [AngleToSite]         FLOAT (53)     NULL,
    [FtpStatus]           BIT            NULL,
    [StackTrace]          NVARCHAR (MAX) NULL,
    [TestResult]          FLOAT (53)     NULL,
    [MoStatus]            BIT            NULL,
    [MtStatus]            BIT            NULL,
    [VolteMoStatus]       BIT            NULL,
    [VolteMtStatus]       BIT            NULL,
    [ConnectionSetupTime] FLOAT (53)     NULL,
    [SubNetworkMode]      VARCHAR (100)  NULL,
    [ActualBand]          VARCHAR (100)  NULL,
    [ActualCarrier]       VARCHAR (100)  NULL,
    [TestStatus]          BIT            NULL,
    [IsHandover]          BIT            CONSTRAINT [DF_AV_SiteTestLog_IsHandover] DEFAULT ((0)) NULL,
    [IsActive]            BIT            CONSTRAINT [DF_AV_SiteTestLog_IsActive] DEFAULT ((1)) NULL,
    [serverTimeStamp]     DATETIME       CONSTRAINT [DF_AV_SiteTestLog_serverTimeStamp] DEFAULT (getdate()) NULL,
    [pciColor]            NVARCHAR (10)  NULL,
    [rsrpColor]           NVARCHAR (10)  NULL,
    [rsrqColor]           NVARCHAR (10)  NULL,
    [sinrColor]           NVARCHAR (10)  NULL,
    [dlColor]             NVARCHAR (10)  NULL,
    [ChColor]             NVARCHAR (10)  NULL,
    [FloorId]             NUMERIC (18)   NULL,
    [RRCState]            NVARCHAR (50)  NULL,
    [cellColor]           NVARCHAR (10)  NULL,
    [TCH]                 NVARCHAR (15)  NULL,
    [NRBand]              VARCHAR (100)  NULL,
    [NRCarrier]           VARCHAR (100)  NULL,
    [NRRsrp]              FLOAT (53)     NULL,
    [NRRsrq]              FLOAT (53)     NULL,
    [NRRsnr]              FLOAT (53)     NULL,
    [NRCqi]               FLOAT (53)     NULL,
    [FromPCI]             NVARCHAR (20)  NULL,
    [ToPCI]               NVARCHAR (20)  NULL,
    [NRPci]               INT            NULL,
    CONSTRAINT [PK_AV_SiteTestLog] PRIMARY KEY CLUSTERED ([LogId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestLog_7]
    ON [dbo].[AV_SiteTestLog]([Sector] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestLog_6]
    ON [dbo].[AV_SiteTestLog]([SectorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestLog_5]
    ON [dbo].[AV_SiteTestLog]([Site] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestLog_4]
    ON [dbo].[AV_SiteTestLog]([ScopeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestLog_3]
    ON [dbo].[AV_SiteTestLog]([CarrierId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestLog_2]
    ON [dbo].[AV_SiteTestLog]([BandId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestLog_1]
    ON [dbo].[AV_SiteTestLog]([NetworkModeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestLog]
    ON [dbo].[AV_SiteTestLog]([SiteId] ASC);


GO
create TRIGGER AV_updateSiteTestLog
       ON AV_SiteTestLog
AFTER UPDATE
AS
BEGIN

    DECLARE @LogId int
	DECLARE @Site nvarchar(50)
       
	SELECT @LogId = INSERTED.LogId, @Site=INSERTED.Site
    FROM INSERTED;


	UPDATE AV_SiteTestLog
	SET SiteId=@LogId
	WHERE LogId=@LogId
END
GO
DISABLE TRIGGER [dbo].[AV_updateSiteTestLog]
    ON [dbo].[AV_SiteTestLog];

