CREATE TABLE [dbo].[AL_AlertConfig] (
    [AlertConfigId]   INT           IDENTITY (1, 1) NOT NULL,
    [AlertCategoryId] INT           NOT NULL,
    [Name]            VARCHAR (50)  NOT NULL,
    [Description]     VARCHAR (MAX) NOT NULL,
    [IsEnabled]       BIT           NOT NULL,
    [IsDeleted]       BIT           CONSTRAINT [dfac_IsDeleted] DEFAULT ((0)) NULL,
    [KeyCode]         VARCHAR (50)  NULL,
    CONSTRAINT [PK_AL_AlertConfig] PRIMARY KEY CLUSTERED ([AlertConfigId] ASC)
);

