CREATE TABLE [dbo].[PM_SiteLog] (
    [SiteLogId]      NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ProjectSiteId]  NUMERIC (18)   NULL,
    [StatusId]       NUMERIC (18)   NULL,
    [MSWindowId]     NUMERIC (18)   NULL,
    [AlarmId]        NUMERIC (18)   NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [UserId]         NUMERIC (18)   NULL,
    [CreatedOn]      DATETIME       NULL,
    [ActivityTypeId] NUMERIC (18)   NULL,
    [GngId]          NUMERIC (18)   NULL,
    [ItemTypeId]     NUMERIC (18)   NULL,
    [ItemFilePath]   NVARCHAR (250) NULL,
    [IsAdditional]   BIT            CONSTRAINT [DF_PM_SiteLog_IsAdditional] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_PM_SiteLog] PRIMARY KEY CLUSTERED ([SiteLogId] ASC)
);

