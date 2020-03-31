CREATE TABLE [dbo].[SiteTaskTracker] (
    [PM_SiteTaskTrackerId] INT           IDENTITY (1, 1) NOT NULL,
    [TrackerId]            INT           NULL,
    [SiteTaskId]           INT           NULL,
    [Actual]               NVARCHAR (50) NULL,
    [CreatedOn]            DATETIME      NULL,
    [CreatedById]          INT           NULL,
    CONSTRAINT [PK_SiteTaskTracker] PRIMARY KEY CLUSTERED ([PM_SiteTaskTrackerId] ASC)
);

