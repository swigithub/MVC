CREATE TABLE [dbo].[PM_ToDo] (
    [TodoId]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ToDoTitle]     NVARCHAR (100) NULL,
    [Description]   NVARCHAR (500) NULL,
    [Type]          NVARCHAR (50)  NULL,
    [Status]        NVARCHAR (50)  NULL,
    [CreatedOn]     DATETIME       CONSTRAINT [DF_PM_ToDo_CreatedOn] DEFAULT (getdate()) NULL,
    [CreatedById]   NUMERIC (18)   NULL,
    [ToDoDateTime]  DATETIME       NULL,
    [ProjectId]     NVARCHAR (255) NULL,
    [SiteId]        NUMERIC (18)   NULL,
    [TaskId]        NUMERIC (18)   NULL,
    [AssignedToIds] NVARCHAR (100) NULL,
    CONSTRAINT [PK_PM_ToDo] PRIMARY KEY CLUSTERED ([TodoId] ASC)
);

