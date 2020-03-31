CREATE TABLE [dbo].[Sec_UserDefinationType] (
    [Id]               INT IDENTITY (1, 1) NOT NULL,
    [UserId]           INT NULL,
    [DefinationTypeId] INT NULL,
    CONSTRAINT [PK_Sec_UserDefinationType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

