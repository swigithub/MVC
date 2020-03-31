CREATE TABLE [dbo].[Sec_Permissions] (
    [Id]         INT           NOT NULL,
    [ParentId]   INT           NULL,
    [Title]      VARCHAR (50)  NULL,
    [URL]        VARCHAR (150) NULL,
    [IsMenuItem] BIT           NULL,
    [Code]       VARCHAR (50)  NULL,
    [Icon]       VARCHAR (20)  NULL,
    [IsUsed]     BIT           NULL,
    [ModuleId]   NUMERIC (18)  NULL,
    [SortOrder]  INT           NULL,
    [IsModule]   BIT           NULL,
    CONSTRAINT [PK_Sec_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

