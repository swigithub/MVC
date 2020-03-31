CREATE TABLE [dbo].[PM_ProjectPlanHistory] (
    [RevisionId]   NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [RevisionType] NUMERIC (18) NULL,
    [CreatedOn]    DATETIME     NULL,
    [CreatedBy]    NUMERIC (18) NULL,
    [ProjectId]    NUMERIC (18) NULL,
    CONSTRAINT [PK_PM_ProjectPlanHistory] PRIMARY KEY CLUSTERED ([RevisionId] ASC)
);

