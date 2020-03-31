CREATE TABLE [dbo].[AV_SiteOoklaLogs] (
    [SrId]          NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteId]        NUMERIC (18)   NULL,
    [ScopeId]       NUMERIC (18)   NULL,
    [NetworkModeId] NUMERIC (18)   NULL,
    [BandId]        NUMERIC (18)   NULL,
    [CarrierId]     NUMERIC (18)   NULL,
    [SectorId]      NUMERIC (18)   NULL,
    [Latitude]      FLOAT (53)     NULL,
    [Longitude]     FLOAT (53)     NULL,
    [DownlinkSpeed] FLOAT (53)     NULL,
    [UplinkSpeed]   FLOAT (53)     NULL,
    [Latency]       FLOAT (53)     NULL,
    [OoklaFilePath] NVARCHAR (500) NULL,
    [NetworkMode]   NVARCHAR (10)  NULL,
    [RSRP]          FLOAT (53)     NULL,
    [RSRQ]          FLOAT (53)     NULL,
    [RSNR]          FLOAT (53)     NULL,
    [Timestamp]     DATETIME       NULL,
    CONSTRAINT [PK_AV_SiteOoklaLogs] PRIMARY KEY CLUSTERED ([SrId] ASC)
);

