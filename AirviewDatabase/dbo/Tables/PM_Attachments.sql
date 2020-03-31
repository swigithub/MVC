CREATE TABLE [dbo].[PM_Attachments] (
    [SiteDocId]     NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ProjectSiteId] NUMERIC (18)   NULL,
    [TaskId]        NUMERIC (18)   NULL,
    [DocFilepath]   NVARCHAR (500) NULL,
    [Description]   NVARCHAR (500) NULL,
    [UploadedById]  NUMERIC (18)   NULL,
    [UploadedOn]    DATETIME       NULL,
    CONSTRAINT [PK_PM_Attachments] PRIMARY KEY CLUSTERED ([SiteDocId] ASC)
);

