CREATE TABLE [dbo].[AV_TestConfigurations] (
    [Id]                NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ClientId]          NUMERIC (18)  NULL,
    [CityId]            NUMERIC (18)  NULL,
    [RevisionId]        NUMERIC (18)  NULL,
    [ConfigurationDate] DATE          CONSTRAINT [DF_AV_TestConfigurations_ConfigurationDate] DEFAULT (getdate()) NULL,
    [TestTypeId]        NUMERIC (18)  NULL,
    [KpiId]             NUMERIC (18)  NULL,
    [KpiValue]          NVARCHAR (50) NULL,
    [IsActive]          BIT           NULL,
    [TestCategoryId]    NUMERIC (18)  NULL,
    [NetworkModeId]     NUMERIC (18)  NULL,
    [BandId]            NUMERIC (18)  NULL,
    [CarrierId]         NUMERIC (18)  NULL,
    CONSTRAINT [PK_AV_TestConfigurations] PRIMARY KEY CLUSTERED ([Id] ASC)
);

