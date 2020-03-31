CREATE TABLE [dbo].[AD_Help] (
    [HelpId]      INT            IDENTITY (1, 1) NOT NULL,
    [ModuleId]    INT            NULL,
    [FeatureId]   INT            NULL,
    [Title]       NVARCHAR (150) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [IsActive]    BIT            NOT NULL,
    [ComponentId] INT            NULL,
    CONSTRAINT [PK_AD_Help] PRIMARY KEY CLUSTERED ([HelpId] ASC)
);

