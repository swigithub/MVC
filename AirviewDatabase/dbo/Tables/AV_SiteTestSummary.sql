CREATE TABLE [dbo].[AV_SiteTestSummary] (
    [SummaryId]                NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ClientId]                 NUMERIC (18)   NULL,
    [RegionId]                 NUMERIC (18)   NULL,
    [Region]                   VARCHAR (100)  NULL,
    [CityId]                   NUMERIC (18)   NULL,
    [City]                     VARCHAR (100)  NULL,
    [ClusterId]                NUMERIC (18)   NULL,
    [Cluster]                  NVARCHAR (50)  NULL,
    [SiteId]                   NUMERIC (18)   NULL,
    [Site]                     NVARCHAR (50)  NULL,
    [Latitude]                 NVARCHAR (50)  NULL,
    [Longitude]                NVARCHAR (50)  NULL,
    [SiteScheduleDate]         DATETIME       NULL,
    [SectorId]                 NUMERIC (18)   NULL,
    [Sector]                   NVARCHAR (50)  NULL,
    [TestLatitude]             FLOAT (53)     NULL,
    [TestLongitude]            FLOAT (53)     NULL,
    [ScopeId]                  NUMERIC (18)   NULL,
    [Scope]                    VARCHAR (100)  NULL,
    [NetworkModeId]            NUMERIC (18)   NULL,
    [NetworkMode]              VARCHAR (100)  NULL,
    [BandId]                   NUMERIC (18)   NULL,
    [Band]                     VARCHAR (100)  NULL,
    [CarrierId]                NUMERIC (18)   NULL,
    [Carrier]                  VARCHAR (100)  NULL,
    [Antenna]                  NVARCHAR (50)  NULL,
    [Azimuth]                  FLOAT (53)     NULL,
    [PciId]                    NUMERIC (18)   NULL,
    [BeamWidth]                FLOAT (53)     NULL,
    [GsmRssi]                  INT            NULL,
    [GsmRxQual]                INT            NULL,
    [WcdmaRssi]                INT            NULL,
    [WcdmaRscp]                INT            NULL,
    [WcdmaEcio]                FLOAT (53)     NULL,
    [LteRssi]                  INT            NULL,
    [LteRsrp]                  INT            NULL,
    [LteRsrq]                  INT            NULL,
    [LteRsnr]                  FLOAT (53)     NULL,
    [LteCqi]                   FLOAT (53)     NULL,
    [DistanceFromSite]         FLOAT (53)     NULL,
    [AngleToSite]              FLOAT (53)     NULL,
    [FtpStatus]                BIT            NULL,
    [PingHost]                 NVARCHAR (50)  NULL,
    [LatencyRate]              FLOAT (53)     NULL,
    [PingIterations]           INT            NULL,
    [PingMinResult]            FLOAT (53)     NULL,
    [PingMaxResult]            FLOAT (53)     NULL,
    [PingAverageResult]        FLOAT (53)     NULL,
    [PingStatus]               BIT            NULL,
    [DownlinkRate]             FLOAT (53)     NULL,
    [DownlinkMinResult]        FLOAT (53)     NULL,
    [DownlinkMaxResult]        FLOAT (53)     NULL,
    [DownlinkAvgResult]        FLOAT (53)     NULL,
    [DownlinkStatus]           BIT            NULL,
    [UplinkRate]               FLOAT (53)     NULL,
    [UplinkMinResult]          FLOAT (53)     NULL,
    [UplinkMaxResult]          FLOAT (53)     NULL,
    [UplinkAvgResult]          FLOAT (53)     NULL,
    [UplinkStatus]             BIT            NULL,
    [ConnectionSetupTime]      FLOAT (53)     NULL,
    [ConnectionSetupStatus]    BIT            NULL,
    [MoMtCallNo]               NVARCHAR (50)  NULL,
    [MoMtCallDuration]         INT            NULL,
    [MoStatus]                 BIT            NULL,
    [MtStatus]                 BIT            NULL,
    [VMoMtCallno]              NVARCHAR (50)  NULL,
    [VMoMtCallDuration]        INT            NULL,
    [VMoStatus]                BIT            NULL,
    [VMtStatus]                BIT            NULL,
    [CwHandoverStatus]         BIT            NULL,
    [Ccwhandoverstatus]        BIT            NULL,
    [ICwHandoverStatus]        BIT            NULL,
    [ICcwhandoverstatus]       BIT            NULL,
    [FtpServerIp]              NVARCHAR (50)  NULL,
    [FtpServerPort]            NVARCHAR (50)  NULL,
    [FtpServerPath]            NVARCHAR (250) NULL,
    [FtpDownlinkFile]          NVARCHAR (250) NULL,
    [StationaryTestFilePath]   NVARCHAR (500) NULL,
    [CwTestFilePath]           NVARCHAR (500) NULL,
    [CcwTestFilePath]          NVARCHAR (500) NULL,
    [OoklaTestFilePath]        NVARCHAR (500) NULL,
    [OoklaPingResult]          FLOAT (53)     NULL,
    [OoklaDownlinkResult]      FLOAT (53)     NULL,
    [OoklaUplinkResult]        FLOAT (53)     NULL,
    [OoklaLatitude]            FLOAT (53)     NULL,
    [OoklaLongitude]           FLOAT (53)     NULL,
    [OoklaRssi]                FLOAT (53)     NULL,
    [OoklaSinr]                FLOAT (53)     NULL,
    [pciColor]                 NVARCHAR (10)  NULL,
    [rsrpColor]                NVARCHAR (10)  NULL,
    [rsrqColor]                NVARCHAR (10)  NULL,
    [sinrColor]                NVARCHAR (10)  NULL,
    [dlColor]                  NVARCHAR (10)  NULL,
    [SMoStatus]                BIT            NULL,
    [SMtStatus]                BIT            NULL,
    [CarrierAggregationStatus] BIT            NULL,
    [E911Status]               BIT            NULL,
    [IsE911Performed]          BIT            NULL,
    [TesterName]               NVARCHAR (50)  NULL,
    [Comments]                 NVARCHAR (500) NULL,
    [TechName]                 NVARCHAR (50)  NULL,
    [SwitchComment]            NVARCHAR (500) NULL,
    [ProjectId]                NUMERIC (18)   NULL,
    [MinConSetupTime]          FLOAT (53)     NULL,
    [MaxConSetupTime]          FLOAT (53)     NULL,
    [AvgConSetupTime]          FLOAT (53)     NULL,
    [HoInterruptionTime]       FLOAT (53)     NULL,
    [IRATHandover]             BIT            NULL,
    [CCWHoInterruptionTime]    FLOAT (53)     NULL,
    [MimoStatus]               BIT            NULL,
    [MimoTestFilePath]         NVARCHAR (500) NULL,
    [SpeedTestFilePath]        NVARCHAR (500) NULL,
    [CaActiveTestFilePath]     NVARCHAR (500) NULL,
    [CaDeavticeTestFilePath]   NVARCHAR (500) NULL,
    [CaSpeedTestFilePath]      NVARCHAR (500) NULL,
    [LaaSpeedTestFilePath]     NVARCHAR (500) NULL,
    [LaaSmTestFilePath]        NVARCHAR (500) NULL,
    [TCH]                      NVARCHAR (15)  NULL,
    [PhyDLSpeedMin]            FLOAT (53)     NULL,
    [PhyDLSpeedMax]            FLOAT (53)     NULL,
    [PhyDLSpeedAvg]            FLOAT (53)     NULL,
    [PhyULSpeedMin]            FLOAT (53)     NULL,
    [PhyULSpeedMax]            FLOAT (53)     NULL,
    [PhyULSpeedAvg]            FLOAT (53)     NULL,
    [IntraHOInteruptTime]      FLOAT (53)     NULL,
    [callSetupTime]            FLOAT (53)     NULL,
    [IntreHOInteruptTime]      FLOAT (53)     NULL,
    [PhyDLStatus]              BIT            NULL,
    [PhyULStatus]              BIT            NULL,
    [CADLSpeed]                FLOAT (53)     NULL,
    [CAULSpeed]                FLOAT (53)     NULL,
    CONSTRAINT [PK_AV_SiteTestSummery] PRIMARY KEY CLUSTERED ([SummaryId] ASC),
    CONSTRAINT [FK_AV_SiteTestSummary_AV_Sites] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[AV_Sites] ([SiteId])
);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestSummary_6]
    ON [dbo].[AV_SiteTestSummary]([ScopeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestSummary_5]
    ON [dbo].[AV_SiteTestSummary]([CarrierId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestSummary_4]
    ON [dbo].[AV_SiteTestSummary]([BandId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestSummary_3]
    ON [dbo].[AV_SiteTestSummary]([NetworkModeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestSummary_2]
    ON [dbo].[AV_SiteTestSummary]([SectorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestSummary_1]
    ON [dbo].[AV_SiteTestSummary]([Site] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AV_SiteTestSummary]
    ON [dbo].[AV_SiteTestSummary]([SiteId] ASC);

