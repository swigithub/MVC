CREATE TABLE [dbo].[AV_Clusters] (
    [ClusterId]   NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ClusterCode] VARCHAR (50) NOT NULL,
    [CityId]      NUMERIC (18) NOT NULL,
    [ClientId]    NUMERIC (18) NOT NULL,
    [IsActive]    BIT          CONSTRAINT [DF_AV_Clusters_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_AV_Clusters] PRIMARY KEY CLUSTERED ([ClusterId] ASC)
);

