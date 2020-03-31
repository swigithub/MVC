CREATE TABLE [dbo].[AV_SectorTestLog] (
    [SrId]           NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [SiteId]         NUMERIC (18)  NULL,
    [NetworkModeId]  NUMERIC (18)  NULL,
    [BandId]         NUMERIC (18)  NULL,
    [CarrierId]      NUMERIC (18)  NULL,
    [ScopeId]        NUMERIC (18)  NULL,
    [SectorId]       NUMERIC (18)  NULL,
    [TestType]       NVARCHAR (50) NULL,
    [TestLatitude]   FLOAT (53)    NULL,
    [TestLongitude]  FLOAT (53)    NULL,
    [SignalStrength] FLOAT (53)    NULL,
    [SignalPower]    FLOAT (53)    NULL,
    [SignalQuality]  FLOAT (53)    NULL,
    [SignalNoise]    FLOAT (53)    NULL,
    CONSTRAINT [PK_AV_SectorTestLog] PRIMARY KEY CLUSTERED ([SrId] ASC)
);

