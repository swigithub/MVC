CREATE TABLE [dbo].[AL_AlertCategory] (
    [AlertCategoryId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]            VARCHAR (50)  NOT NULL,
    [IsEnabled]       BIT           NOT NULL,
    [IsDeleted]       BIT           CONSTRAINT [dfc_IsDeleted] DEFAULT ((0)) NULL,
    [KeyCode]         VARCHAR (MAX) NULL,
    [ParentId]        INT           NULL,
    CONSTRAINT [PK_AL_AlertCategory] PRIMARY KEY CLUSTERED ([AlertCategoryId] ASC)
);

