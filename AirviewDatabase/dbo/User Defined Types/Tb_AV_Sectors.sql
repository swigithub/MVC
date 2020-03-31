CREATE TYPE [dbo].[Tb_AV_Sectors] AS TABLE (
    [SectorId]      NUMERIC (18)  NULL,
    [SectorCode]    VARCHAR (50)  NOT NULL,
    [NetworkModeId] NUMERIC (18)  NOT NULL,
    [ScopeId]       NUMERIC (18)  NOT NULL,
    [BandId]        NUMERIC (18)  NOT NULL,
    [CarrierId]     NUMERIC (18)  NOT NULL,
    [Antenna]       VARCHAR (50)  NOT NULL,
    [BeamWidth]     FLOAT (53)    NOT NULL,
    [Azimuth]       FLOAT (53)    NOT NULL,
    [PCI]           INT           NOT NULL,
    [SiteId]        NUMERIC (18)  NOT NULL,
    [MRBTS]         NVARCHAR (50) NULL,
    [CellId]        NVARCHAR (50) NULL,
    [RFHeight]      NVARCHAR (50) NULL,
    [MTilt]         NVARCHAR (50) NULL,
    [ETilt]         NVARCHAR (50) NULL);

