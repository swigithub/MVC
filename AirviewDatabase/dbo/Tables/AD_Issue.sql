CREATE TABLE [dbo].[AD_Issue] (
    [IssueId]          INT            IDENTITY (1, 1) NOT NULL,
    [IssueTitle]       NVARCHAR (150) NULL,
    [IssueDescription] TEXT           NULL,
    [IssueFeature]     NVARCHAR (50)  NULL,
    [IssueStatus]      NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([IssueId] ASC)
);

