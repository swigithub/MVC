CREATE TABLE [dbo].[INV_UEIssues] (
    [IssueId]     NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [UEId]        NUMERIC (18)   NULL,
    [Description] NVARCHAR (500) NULL,
    [IssueUserId] NUMERIC (18)   NULL,
    [ReportDate]  DATETIME       NULL,
    CONSTRAINT [PK_INV_UEIssues] PRIMARY KEY CLUSTERED ([IssueId] ASC)
);

