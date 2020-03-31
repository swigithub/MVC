CREATE TABLE [dbo].[PM_IssuesTrend] (
    [SrId]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ProjectId]     NUMERIC (18) NULL,
    [RegionId]      NUMERIC (18) NULL,
    [MarketId]      NUMERIC (18) NULL,
    [ReportDate]    DATETIME     NULL,
    [IssueOwnerId]  NUMERIC (18) NULL,
    [IsUnAvoidable] BIT          NULL,
    [IssueCount]    INT          CONSTRAINT [DF_PM_IssuesTrend_IssueCount] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_PM_IssuesTrend] PRIMARY KEY CLUSTERED ([SrId] ASC)
);

