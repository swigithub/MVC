CREATE TABLE [dbo].[AV_DriveRoutes] (
    [RouteId]     NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [CreatedDate] DATETIME       CONSTRAINT [DF_AV_DriveRoutes_CreatedDate] DEFAULT (getdate()) NULL,
    [RoutePath]   NVARCHAR (100) NULL,
    [CreatedBy]   NUMERIC (18)   NULL,
    [SiteId]      NUMERIC (18)   NULL,
    [ScopeId]     NUMERIC (18)   NULL,
    [TestType]    NVARCHAR (50)  NULL,
    [IsSelected]  BIT            NULL,
    CONSTRAINT [PK_AV_DriveRoutes] PRIMARY KEY CLUSTERED ([RouteId] ASC)
);

