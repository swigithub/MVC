CREATE TABLE [dbo].[PM_SiteTaskTracker] (
    [SiteTaskTrackerId] INT           IDENTITY (1, 1) NOT NULL,
    [GroupTrackerId]    INT           NULL,
    [TaskTrackerId]     INT           NULL,
    [Actual]            NVARCHAR (50) NULL,
    [CreatedOn]         DATETIME      CONSTRAINT [DF_PM_SiteTaskTracker_CreatedOn] DEFAULT (getdate()) NULL,
    [CreatedById]       INT           NULL,
    [SiteId]            NUMERIC (18)  NULL,
    CONSTRAINT [PK_PM_SiteTaskTracker] PRIMARY KEY CLUSTERED ([SiteTaskTrackerId] ASC)
);

