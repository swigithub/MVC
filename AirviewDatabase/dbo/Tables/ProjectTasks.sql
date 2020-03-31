CREATE TABLE [dbo].[ProjectTasks] (
    [TaskId]      NUMERIC (18)  NULL,
    [PTaskId]     NUMERIC (18)  NULL,
    [ProjectId]   NUMERIC (18)  NULL,
    [Task]        NVARCHAR (50) NULL,
    [ActualSites] INT           NULL,
    [TotalSites]  INT           NULL,
    [Color]       NVARCHAR (50) NULL,
    [TaskType]    VARCHAR (100) NOT NULL
);

