CREATE TABLE [dbo].[ExternalDatabase] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [TableName]    NVARCHAR (50) NOT NULL,
    [CompleteName] NVARCHAR (50) NOT NULL
);

