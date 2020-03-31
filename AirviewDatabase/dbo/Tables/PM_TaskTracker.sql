CREATE TABLE [dbo].[PM_TaskTracker] (
    [TrackerId]      INT           IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (50) NULL,
    [Budget]         NVARCHAR (50) NULL,
    [Actual]         NVARCHAR (50) NULL,
    [Unit]           NVARCHAR (50) NULL,
    [TrackerGroupId] INT           NULL,
    [CreatedOn]      DATETIME      CONSTRAINT [DF_PM_TaskTracker_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_PM_TaskTracker] PRIMARY KEY CLUSTERED ([TrackerId] ASC)
);

