CREATE TABLE [dbo].[PM_TrackerGroup] (
    [TrackerGroupId] INT            IDENTITY (1, 1) NOT NULL,
    [ProjectId]      INT            NOT NULL,
    [TaskId]         INT            NOT NULL,
    [ParentId]       INT            NOT NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF_PM_TrackerGroup_CreatedOn] DEFAULT (getdate()) NULL,
    [Title]          NVARCHAR (100) NOT NULL,
    [GroupCode]      NVARCHAR (50)  NULL,
    CONSTRAINT [PK_PM_TrackerGroup] PRIMARY KEY CLUSTERED ([TrackerGroupId] ASC)
);

