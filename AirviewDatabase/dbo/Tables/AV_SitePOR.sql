CREATE TABLE [dbo].[AV_SitePOR] (
    [ClientId]      NUMERIC (18)   NULL,
    [CityId]        NUMERIC (18)   NULL,
    [RingId]        NVARCHAR (255) NULL,
    [SiteId]        NVARCHAR (255) NULL,
    [SiteType]      NVARCHAR (255) NULL,
    [POR]           NVARCHAR (255) NULL,
    [PORToolCheck]  NVARCHAR (255) NULL,
    [Comments]      NVARCHAR (255) NULL,
    [MSA]           NVARCHAR (255) NULL,
    [L1900Capable]  NVARCHAR (255) NULL,
    [L1900POR]      NVARCHAR (255) NULL,
    [L1900OA]       NVARCHAR (255) NULL,
    [L1900Spectrum] NVARCHAR (255) NULL,
    [L700Capable]   NVARCHAR (255) NULL,
    [L700POR]       NVARCHAR (255) NULL,
    [L700OA]        NVARCHAR (255) NULL,
    [L700Spectrum]  NVARCHAR (255) NULL,
    [PORType]       NVARCHAR (255) NULL
);

