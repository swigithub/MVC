CREATE TABLE [dbo].[PM_IssuesLog] (
    [IssueLogId]   NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [IssueId]      NUMERIC (18)   NULL,
    [StatusId]     NUMERIC (18)   NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [UserId]       NUMERIC (18)   NULL,
    [CreatedOn]    DATETIME       NULL,
    [PriorityId]   NUMERIC (18)   NULL,
    [SeverityId]   NUMERIC (18)   NULL,
    [AssignToId]   NVARCHAR (50)  NULL,
    [ItemFilePath] NVARCHAR (500) NULL,
    CONSTRAINT [PK_PM_IssuesLog] PRIMARY KEY CLUSTERED ([IssueLogId] ASC)
);

