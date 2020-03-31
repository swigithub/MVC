CREATE TABLE [dbo].[AD_MarketCenter] (
    [SrId]      INT          IDENTITY (1, 1) NOT NULL,
    [MarketID]  NUMERIC (18) NULL,
    [Latitude]  FLOAT (53)   NULL,
    [Longitude] FLOAT (53)   NULL,
    CONSTRAINT [PK_AD_MarketCenter] PRIMARY KEY CLUSTERED ([SrId] ASC)
);

