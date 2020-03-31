CREATE TABLE [dbo].[PM_WorkGroup] (
    [PMWGId]      INT IDENTITY (1, 1) NOT NULL,
    [WorkGroupId] INT NULL,
    [ProjectId]   INT NULL,
    CONSTRAINT [PK_PM_WorkGroup] PRIMARY KEY CLUSTERED ([PMWGId] ASC)
);

