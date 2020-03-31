CREATE TABLE [dbo].[NISites] (
    [ClusterCode]   NVARCHAR (50) NULL,
    [Region]        NVARCHAR (50) NULL,
    [Market]        NVARCHAR (50) NULL,
    [Client]        NVARCHAR (50) NULL,
    [SiteCode]      NVARCHAR (50) NULL,
    [SiteLatitude]  FLOAT (53)    NULL,
    [SiteLongitude] FLOAT (53)    NULL,
    [Description]   NVARCHAR (50) NULL,
    [SectorCode]    NVARCHAR (50) NULL,
    [NetworkMode]   NVARCHAR (50) NULL,
    [Scope]         NVARCHAR (50) NULL,
    [Band]          NVARCHAR (50) NULL,
    [Carrier]       NVARCHAR (50) NULL,
    [Antenna]       NVARCHAR (50) NULL,
    [BeamWidth]     NVARCHAR (50) NULL,
    [Azimuth]       FLOAT (53)    NULL,
    [PCI]           INT           NULL,
    [ReceivedOn]    DATETIME      NULL
);

