CREATE TYPE [dbo].[Tbl_AD_Definations] AS TABLE (
    [DefinationName]   VARCHAR (100) NOT NULL,
    [PDefinationId]    NUMERIC (18)  NULL,
    [DefinationTypeId] NUMERIC (18)  NOT NULL,
    [KeyCode]          NVARCHAR (50) NULL,
    [DisplayType]      NVARCHAR (50) NULL,
    [ColorCode]        NVARCHAR (50) NULL,
    [InputType]        NVARCHAR (50) NULL,
    [MaxLength]        INT           NULL,
    [SortOrder]        INT           NOT NULL,
    [IsActive]         BIT           DEFAULT ((1)) NOT NULL,
    [DisplayText]      NVARCHAR (50) NULL);

