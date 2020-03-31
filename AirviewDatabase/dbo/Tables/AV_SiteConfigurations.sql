CREATE TABLE [dbo].[AV_SiteConfigurations] (
    [Id]                NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ClientId]          NUMERIC (18)  NULL,
    [CityId]            NUMERIC (18)  NULL,
    [SiteId]            NUMERIC (18)  NULL,
    [RevisionId]        NUMERIC (18)  NULL,
    [TestTypeId]        NUMERIC (18)  NULL,
    [KpiId]             NUMERIC (18)  NULL,
    [KpiValue]          NVARCHAR (50) NULL,
    [TestCategoryId]    NUMERIC (18)  NULL,
    [ConfigurationDate] DATETIME      CONSTRAINT [DF_AV_SiteConfigurations_ConfigurationDate] DEFAULT (getdate()) NULL,
    [NetworkModeId]     NUMERIC (18)  NULL,
    [BandId]            NUMERIC (18)  NULL,
    CONSTRAINT [PK_AV_SiteConfigurations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AV_SiteConfigurations_AV_Sites] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[AV_Sites] ([SiteId])
);

