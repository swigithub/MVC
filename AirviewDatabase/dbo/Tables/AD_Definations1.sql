CREATE TABLE [dbo].[AD_Definations1] (
    [DefinationId]     NUMERIC (18)   NOT NULL,
    [DefinationName]   VARCHAR (100)  NOT NULL,
    [PDefinationId]    NUMERIC (18)   NULL,
    [DefinationTypeId] NUMERIC (18)   NOT NULL,
    [KeyCode]          NVARCHAR (50)  NULL,
    [DisplayType]      NVARCHAR (50)  NULL,
    [ColorCode]        NVARCHAR (50)  NULL,
    [InputType]        NVARCHAR (50)  NULL,
    [MaxLength]        INT            NULL,
    [SortOrder]        INT            NULL,
    [IsActive]         BIT            CONSTRAINT [DF_AD_Definations_IsActive1] DEFAULT ((1)) NOT NULL,
    [DisplayText]      NVARCHAR (50)  NULL,
    [MapColumn]        NVARCHAR (50)  NULL,
    [URL]              NVARCHAR (500) NULL,
    [Band]             NVARCHAR (50)  NULL
);

