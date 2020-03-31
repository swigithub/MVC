CREATE TABLE [dbo].[PM_WorkLog] (
    [WLogId]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ProjectId]     NUMERIC (18)   NULL,
    [ProjectSiteId] NUMERIC (18)   NULL,
    [TaskId]        NUMERIC (18)   NULL,
    [LogType]       NVARCHAR (15)  NULL,
    [UserId]        NUMERIC (18)   NULL,
    [LogDate]       DATETIME       NULL,
    [LogHours]      FLOAT (53)     NULL,
    [RatePerUnit]   FLOAT (53)     NULL,
    [Description]   NVARCHAR (300) NULL,
    [IsApproved]    BIT            NULL,
    [ApprovedById]  NUMERIC (18)   NULL,
    [ApprovalDate]  DATETIME       NULL,
    [IsAttended]    BIT            CONSTRAINT [DF_PM_WorkLog_IsAttended] DEFAULT ((0)) NULL,
    [Comment]       NVARCHAR (500) NULL,
    CONSTRAINT [PK_PM_WorkLog] PRIMARY KEY CLUSTERED ([WLogId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Task or Issue', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_WorkLog', @level2type = N'COLUMN', @level2name = N'LogType';

