CREATE TABLE [dbo].[AD_ReportConfiguration] (
    [SrId]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ReportId]    NUMERIC (18)   NULL,
    [ClientId]    NUMERIC (18)   NULL,
    [CityId]      NUMERIC (18)   NULL,
    [PageId]      INT            NULL,
    [KeyCode]     NVARCHAR (150) NULL,
    [KeyValue]    NVARCHAR (250) NULL,
    [isActive]    BIT            CONSTRAINT [DF_AD_ReportConfiguration_isActive] DEFAULT ((1)) NULL,
    [fontColor]   NVARCHAR (10)  NULL,
    [headerColor] NVARCHAR (10)  NULL,
    [KeyId]       NUMERIC (18)   NULL,
    [IsPanelItem] BIT            CONSTRAINT [DF_AD_ReportConfiguration_IsPanelItem] DEFAULT ((0)) NULL,
    [ScopeId]     NUMERIC (18)   NULL,
    CONSTRAINT [PK_AD_ReportConfiguration] PRIMARY KEY CLUSTERED ([SrId] ASC)
);

