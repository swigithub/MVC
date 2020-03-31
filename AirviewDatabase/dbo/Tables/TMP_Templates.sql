CREATE TABLE [dbo].[TMP_Templates] (
    [TemplateId]      NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [TemplateTitle]   NVARCHAR (50)  NULL,
    [ProjectId]       NUMERIC (18)   NULL,
    [ScopeId]         NUMERIC (18)   NULL,
    [BackgroundColor] NVARCHAR (10)  NULL,
    [PageType]        NVARCHAR (50)  NULL,
    [Parameters]      NVARCHAR (MAX) NULL,
    [IsActive]        BIT            NOT NULL,
    [TemplateType]    NVARCHAR (50)  NULL,
    [IsDefault]       BIT            NULL,
    [ModuleId]        NUMERIC (18)   NULL,
    CONSTRAINT [PK_TMP_Templates] PRIMARY KEY CLUSTERED ([TemplateId] ASC)
);

