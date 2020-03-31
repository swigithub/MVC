CREATE TABLE [dbo].[AV_SectorColors] (
    [SectorColorId] NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ClientId]      NUMERIC (18)  NOT NULL,
    [CityId]        NUMERIC (18)  NOT NULL,
    [ScopeId]       NUMERIC (18)  NOT NULL,
    [SectorId]      NUMERIC (18)  NULL,
    [SectorCode]    NVARCHAR (50) NOT NULL,
    [SectorColor]   NVARCHAR (15) NOT NULL,
    CONSTRAINT [PK_AV_SectorColors] PRIMARY KEY CLUSTERED ([SectorColorId] ASC)
);

