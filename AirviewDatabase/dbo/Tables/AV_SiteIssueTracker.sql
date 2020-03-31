CREATE TABLE [dbo].[AV_SiteIssueTracker] (
    [TrackingId]    NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteId]        NUMERIC (18)   NULL,
    [TesterId]      NUMERIC (18)   NULL,
    [NetworkModeId] NUMERIC (18)   NULL,
    [BandId]        NUMERIC (18)   NULL,
    [CarrierId]     NUMERIC (18)   NULL,
    [ScopeId]       NUMERIC (18)   NULL,
    [Description]   NVARCHAR (500) NULL,
    [Status]        NVARCHAR (50)  NULL,
    [ReportDate]    DATETIME       CONSTRAINT [DF_AV_SiteIssueTracker_ReportDate] DEFAULT (getdate()) NULL,
    [PTrakingId]    NUMERIC (18)   NULL,
    [ResolvedDate]  DATETIME       NULL,
    [ImagePath]     NVARCHAR (500) NULL,
    [IssueType]     NUMERIC (18)   NULL,
    CONSTRAINT [PK_AV_SiteIssueTracker] PRIMARY KEY CLUSTERED ([TrackingId] ASC)
);

