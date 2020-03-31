CREATE TABLE [dbo].[PM_ProjectEntity] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Title]     NVARCHAR (50) NULL,
    [Plural]    NVARCHAR (50) NULL,
    [CreatedOn] DATETIME      CONSTRAINT [DF_PM_ProjectEntity_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_PM_ProjectEntity] PRIMARY KEY CLUSTERED ([Id] ASC)
);

