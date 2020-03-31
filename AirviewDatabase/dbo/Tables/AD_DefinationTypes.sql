CREATE TABLE [dbo].[AD_DefinationTypes] (
    [DefinationTypeId]  NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DefinationType]    VARCHAR (100) NOT NULL,
    [SortOrder]         INT           NOT NULL,
    [IsActive]          BIT           CONSTRAINT [DF_AD_DefinationTypes_IsActive] DEFAULT ((1)) NOT NULL,
    [PDefinationTypeId] NUMERIC (18)  NULL,
    CONSTRAINT [PK_AD_DefinationTypes] PRIMARY KEY CLUSTERED ([DefinationTypeId] ASC)
);

