CREATE TABLE [dbo].[tmpPSTasks] (
    [OProjectSiteId]     NUMERIC (18)   NULL,
    [OFACode]            NVARCHAR (25)  NULL,
    [NProjectSiteId]     NUMERIC (18)   NOT NULL,
    [NFACode]            NVARCHAR (25)  NULL,
    [TaskId]             NUMERIC (18)   NULL,
    [PTaskId]            NUMERIC (18)   NULL,
    [RevisionId]         INT            NULL,
    [StatusId]           NUMERIC (18)   NULL,
    [PriorityId]         NUMERIC (18)   NULL,
    [EstimatedStartDate] DATETIME       NULL,
    [EstimatedEndDate]   DATETIME       NULL,
    [PlannedDate]        DATETIME       NULL,
    [TargetDate]         DATETIME       NULL,
    [ActualStartDate]    DATETIME       NULL,
    [ActualEndDate]      DATETIME       NULL,
    [CompletionPercent]  FLOAT (53)     NULL,
    [BudgetCost]         FLOAT (53)     NULL,
    [ActualCost]         FLOAT (53)     NULL,
    [IsActive]           BIT            NULL,
    [ResourcesId]        NVARCHAR (500) NULL
);

