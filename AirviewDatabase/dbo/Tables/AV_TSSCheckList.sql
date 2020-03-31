CREATE TABLE [dbo].[AV_TSSCheckList] (
    [CheckListId]     NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteId]          NUMERIC (18)   NULL,
    [LayerId]         NUMERIC (18)   NULL,
    [CheckListTypeId] NUMERIC (18)   NULL,
    [CheckListPId]    NUMERIC (18)   NULL,
    [CheckCount]      INT            NULL,
    [Description]     NVARCHAR (200) NULL,
    [IsActive]        BIT            NULL,
    CONSTRAINT [PK_AV_TSSCheckList] PRIMARY KEY CLUSTERED ([CheckListId] ASC)
);

