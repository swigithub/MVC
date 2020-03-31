CREATE TABLE [dbo].[AV_SiteScriptFormEntry] (
    [FormId]       NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [NodeTypeId]   NUMERIC (18)   NOT NULL,
    [Title]        NVARCHAR (50)  NOT NULL,
    [DataType]     NVARCHAR (50)  NULL,
    [DefaultValue] NVARCHAR (MAX) NULL,
    [MaxLength]    NVARCHAR (50)  NULL,
    [Required]     NVARCHAR (50)  NULL,
    [SortOrder]    NUMERIC (18)   NULL,
    [IsDeleted]    BIT            NULL,
    [ActualValue]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_PM_SiteScriptFormEntry] PRIMARY KEY CLUSTERED ([FormId] ASC)
);

