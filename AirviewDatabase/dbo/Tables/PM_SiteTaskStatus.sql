CREATE TABLE [dbo].[PM_SiteTaskStatus] (
    [TaskStatusId]  NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ProjectSiteId] NUMERIC (18)   NULL,
    [TaskId]        NUMERIC (18)   NULL,
    [SiteTaskId]    NUMERIC (18)   NULL,
    [StatusId]      NUMERIC (18)   NULL,
    [StatusDate]    DATETIME       NULL,
    [CreatedBy]     NUMERIC (18)   NULL,
    [CreatedOn]     DATETIME       NULL,
    [Description]   NVARCHAR (500) NULL,
    CONSTRAINT [PK_PM_SiteTaskStatus] PRIMARY KEY CLUSTERED ([TaskStatusId] ASC)
);

