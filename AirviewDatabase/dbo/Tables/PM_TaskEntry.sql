CREATE TABLE [dbo].[PM_TaskEntry] (
    [EntryId]       NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ProjectId]     NUMERIC (18)   NOT NULL,
    [ProjectSiteId] NUMERIC (18)   NULL,
    [TaskId]        NUMERIC (18)   NOT NULL,
    [FormId]        NUMERIC (18)   NOT NULL,
    [FormValue]     NVARCHAR (500) NULL,
    [CreatedById]   NUMERIC (18)   NULL,
    [CreatedOn]     DATETIME       NULL,
    [Revision]      NUMERIC (18)   NULL,
    [Comments]      NVARCHAR (200) NULL,
    CONSTRAINT [PK_PM_TaskEntry] PRIMARY KEY CLUSTERED ([EntryId] ASC)
);

