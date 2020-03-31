CREATE TABLE [dbo].[PM_TaskStages] (
    [StageId]     INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (250) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [SortOrder]   INT            NOT NULL,
    [ProjectId]   NUMERIC (18)   NOT NULL,
    [IsDeleted]   BIT            NULL,
    [TaskId]      NUMERIC (18)   NULL,
    CONSTRAINT [PK_PM_TaskStages] PRIMARY KEY CLUSTERED ([StageId] ASC),
    CONSTRAINT [FK_PM_TaskStages_PM_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[PM_Projects] ([ProjectId]) ON DELETE CASCADE ON UPDATE CASCADE
);

