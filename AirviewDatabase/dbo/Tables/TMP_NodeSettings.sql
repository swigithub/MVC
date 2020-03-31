CREATE TABLE [dbo].[TMP_NodeSettings] (
    [NodeSettingsId]   NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [NodeId]           NUMERIC (18)   NOT NULL,
    [DefinationId]     NUMERIC (18)   NOT NULL,
    [KeyName]          NVARCHAR (50)  NOT NULL,
    [MappedId]         NVARCHAR (50)  NULL,
    [Value]            NVARCHAR (100) NOT NULL,
    [Settings]         NVARCHAR (MAX) NULL,
    [SortOrder]        INT            NULL,
    [QueryWhereClause] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_TMP_NodeSettings] PRIMARY KEY CLUSTERED ([NodeSettingsId] ASC)
);

