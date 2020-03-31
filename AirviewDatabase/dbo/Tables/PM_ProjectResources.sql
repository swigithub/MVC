CREATE TABLE [dbo].[PM_ProjectResources] (
    [ProjectAssignId] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ProjectId]       NUMERIC (18) NULL,
    [TaskId]          NUMERIC (18) NULL,
    [AssignedById]    NUMERIC (18) NULL,
    [AssignToId]      NUMERIC (18) NULL,
    [AssignedDate]    DATETIME     NULL,
    [ForecastDate]    DATETIME     NULL,
    [PlanDate]        DATETIME     NULL,
    [IsActive]        BIT          NULL,
    [RatePerUnit]     FLOAT (53)   NULL,
    CONSTRAINT [PK_PM_ProjectResources] PRIMARY KEY CLUSTERED ([ProjectAssignId] ASC)
);

