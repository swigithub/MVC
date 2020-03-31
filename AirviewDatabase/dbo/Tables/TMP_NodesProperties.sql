CREATE TABLE [dbo].[TMP_NodesProperties] (
    [FormId]       NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [NodeTypeId]   NUMERIC (18)   NOT NULL,
    [Title]        NVARCHAR (50)  NOT NULL,
    [ControlType]  NVARCHAR (50)  NULL,
    [DataType]     NVARCHAR (50)  NULL,
    [DefaultValue] NVARCHAR (MAX) NULL,
    [MaxLength]    NVARCHAR (50)  NULL,
    [Required]     NVARCHAR (50)  NULL,
    [IsAttachment] NVARCHAR (50)  NULL,
    [SortOrder]    INT            NOT NULL,
    [IsDeleted]    BIT            NULL,
    [Comments]     NVARCHAR (50)  NULL,
    CONSTRAINT [PK_TMP_FormTemplate] PRIMARY KEY CLUSTERED ([FormId] ASC)
);

