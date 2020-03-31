CREATE TABLE [dbo].[PM_TaskResources] (
    [PMTRId]      INT IDENTITY (1, 1) NOT NULL,
    [ResourceId]  INT NOT NULL,
    [GroupId]     INT NOT NULL,
    [RACIId]      INT NOT NULL,
    [ProjectId]   INT NOT NULL,
    [TaskId]      INT NOT NULL,
    [RatePerHour] INT NULL,
    [IsDeleted]   INT CONSTRAINT [DF_PM_TaskResources_IsDeleted] DEFAULT ((0)) NOT NULL
);

