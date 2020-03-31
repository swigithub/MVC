CREATE TABLE [dbo].[AV_SiteClusters] (
    [SiteClusterId] NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteId]        NUMERIC (18)   NULL,
    [ClusterId]     NVARCHAR (50)  NULL,
    [ClusterName]   NVARCHAR (50)  NULL,
    [NetworkId]     NVARCHAR (50)  NULL,
    [CellFilePath]  NVARCHAR (250) NULL,
    [SiteCount]     NUMERIC (18)   NULL,
    CONSTRAINT [PK_AV_SiteCluster] PRIMARY KEY CLUSTERED ([SiteClusterId] ASC)
);

