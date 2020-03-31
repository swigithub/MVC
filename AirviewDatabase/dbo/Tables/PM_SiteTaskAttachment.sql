CREATE TABLE [dbo].[PM_SiteTaskAttachment] (
    [AttachmentId] NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteTaskId]   NUMERIC (18)   NOT NULL,
    [FileName]     NVARCHAR (150) NOT NULL,
    [Description]  NVARCHAR (500) NULL,
    [CategoryId]   NUMERIC (18)   NOT NULL,
    [Tags]         NVARCHAR (150) NULL,
    [CreatedOn]    DATETIME       NOT NULL,
    [CreateBy]     NUMERIC (18)   NOT NULL,
    [ModifiedOn]   DATETIME       NOT NULL,
    [ModifiedBy]   NUMERIC (18)   NOT NULL,
    [IsDeleted]    BIT            NOT NULL,
    CONSTRAINT [PK_PM_SiteTaskAttachment] PRIMARY KEY CLUSTERED ([AttachmentId] ASC)
);

