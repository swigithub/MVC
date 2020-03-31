CREATE TABLE [dbo].[dashboarddata] (
    [TaskId]      NUMERIC (18)  NULL,
    [PTaskId]     NUMERIC (18)  NULL,
    [ProjectID]   NUMERIC (18)  NULL,
    [Task]        NVARCHAR (50) NULL,
    [ActualSites] INT           NULL,
    [TotalSites]  INT           NULL,
    [Color]       NVARCHAR (50) NULL,
    [TaskType]    NVARCHAR (50) NULL
);

