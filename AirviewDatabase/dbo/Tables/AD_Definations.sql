CREATE TABLE [dbo].[AD_Definations] (
    [DefinationId]     NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [DefinationName]   VARCHAR (100)  NOT NULL,
    [PDefinationId]    NUMERIC (18)   NULL,
    [DefinationTypeId] NUMERIC (18)   NOT NULL,
    [KeyCode]          NVARCHAR (50)  NULL,
    [DisplayType]      NVARCHAR (50)  NULL,
    [ColorCode]        NVARCHAR (50)  NULL,
    [InputType]        NVARCHAR (50)  NULL,
    [MaxLength]        INT            NULL,
    [SortOrder]        INT            NULL,
    [IsActive]         BIT            CONSTRAINT [DF_AD_Definations_IsActive] DEFAULT ((1)) NOT NULL,
    [DisplayText]      NVARCHAR (50)  NULL,
    [MapColumn]        NVARCHAR (50)  NULL,
    [URL]              NVARCHAR (500) NULL,
    CONSTRAINT [PK_AD_Definations] PRIMARY KEY CLUSTERED ([DefinationId] ASC),
    CONSTRAINT [FK_AD_Definations_AD_DefinationTypes] FOREIGN KEY ([DefinationTypeId]) REFERENCES [dbo].[AD_DefinationTypes] ([DefinationTypeId])
);

