CREATE TABLE [dbo].[PM_SiteTaskInventoryAttachments] (
    [SiteTaskInventoryAttachmentId] NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [FileNameWithExtension]         NVARCHAR (200) NULL,
    [SubDirectory]                  VARCHAR (50)   NULL,
    [ContentLength]                 INT            NULL,
    [SiteTaskInventoryId]           NUMERIC (18)   NOT NULL,
    [CreatedOn]                     DATETIME       CONSTRAINT [DF_PM_SiteTaskInventoryAttachments_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_PM_SiteTasksInventoryAttachments] PRIMARY KEY CLUSTERED ([SiteTaskInventoryAttachmentId] ASC)
);

