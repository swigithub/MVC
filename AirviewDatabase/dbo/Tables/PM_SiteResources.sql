CREATE TABLE [dbo].[PM_SiteResources] (
    [TaskAssignId]  NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ProjectSiteId] NUMERIC (18)   NULL,
    [SiteTaskId]    NUMERIC (18)   NULL,
    [AssignedById]  NUMERIC (18)   NULL,
    [AssignToId]    NVARCHAR (250) NULL,
    [AssignedDate]  DATETIME       NULL,
    [ForecastDate]  DATETIME       NULL,
    [PlanDate]      DATETIME       NULL,
    [IsActive]      BIT            NULL,
    CONSTRAINT [PK_PM_SiteResources] PRIMARY KEY CLUSTERED ([TaskAssignId] ASC)
);

