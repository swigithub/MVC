CREATE TABLE [dbo].[AV_ScriptTestKPI] (
    [ScriptTestKpiId] NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteScriptId]    NUMERIC (18)   NULL,
    [ScriptKpiId]     NVARCHAR (350) NULL,
    [IsEnabled]       BIT            NULL,
    [SiteId]          NUMERIC (18)   NULL,
    [NetLayerId]      NUMERIC (18)   NULL,
    CONSTRAINT [PK_AV_ScriptTestKPI] PRIMARY KEY CLUSTERED ([ScriptTestKpiId] ASC)
);

