CREATE TABLE [dbo].[PM_ProjectPlanHistory_Details] (
    [ProjectPlanHistoryId] NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteTaskId]           NUMERIC (18)   NULL,
    [ProjectSiteId]        NUMERIC (18)   NULL,
    [RevisionId]           NUMERIC (18)   NOT NULL,
    [Status]               NVARCHAR (100) NULL,
    [Priority]             NVARCHAR (100) NULL,
    [EstimatedStartDate]   DATETIME       NULL,
    [EstimatedEndDate]     DATETIME       NULL,
    [PlanDate]             DATETIME       NULL,
    [TargetDate]           DATETIME       NULL,
    [ActualStartDate]      DATETIME       NULL,
    [ActualEndDate]        DATETIME       NULL,
    [Completion]           FLOAT (53)     NULL,
    [FACode]               NVARCHAR (200) NULL,
    [SiteName]             NVARCHAR (200) NULL,
    [SiteType]             NVARCHAR (100) NULL,
    [SiteRevisionType]     NVARCHAR (50)  NULL,
    [TaskRevisionType]     NVARCHAR (50)  NULL,
    CONSTRAINT [PK_PM_ProjectPlanHistory_Details] PRIMARY KEY CLUSTERED ([ProjectPlanHistoryId] ASC)
);

