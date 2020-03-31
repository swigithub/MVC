CREATE TYPE [dbo].[Tbl_Permissions] AS TABLE (
    [Id]       INT          NULL,
    [ParentId] INT          NULL,
    [Title]    VARCHAR (50) NULL,
    [URL]      VARCHAR (50) NULL,
    [Code]     VARCHAR (50) NULL,
    [Icon]     VARCHAR (50) NULL,
    [In_menu]  BIT          NULL,
    [IsUsed]   BIT          NULL);

