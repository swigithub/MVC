CREATE TYPE [dbo].[Tb_Sectors] AS TABLE (
    [SectorCode]    VARCHAR (50) NULL,
    [NetworkModeId] NUMERIC (18) NULL,
    [ScopeId]       NUMERIC (18) NULL,
    [BandId]        NUMERIC (18) NULL,
    [CarrierId]     NUMERIC (18) NULL,
    [Antenna]       VARCHAR (50) NULL,
    [BeamWidth]     FLOAT (53)   NULL,
    [Azimuth]       FLOAT (53)   NULL,
    [PCI]           INT          NULL,
    [SiteId]        NUMERIC (18) NULL,
    [Client]        VARCHAR (50) NULL,
    [City]          VARCHAR (50) NULL);

