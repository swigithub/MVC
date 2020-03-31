CREATE TABLE [dbo].[AV_SiteScript] (
    [SrId]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [SiteId]        NUMERIC (18)  NULL,
    [NetLayerId]    NUMERIC (18)  NULL,
    [RevisionId]    INT           CONSTRAINT [DF_AV_SiteScript_RevisionId] DEFAULT ((0)) NULL,
    [EventTypeId]   NUMERIC (18)  NULL,
    [EventValue]    NVARCHAR (50) NULL,
    [IsValue]       BIT           NULL,
    [IsL3Enabled]   BIT           NULL,
    [Color]         NVARCHAR (10) NULL,
    [SequenceId]    INT           NULL,
    [EventCommand]  NVARCHAR (50) NULL,
    [EventValue1]   NVARCHAR (50) NULL,
    [EventCommand1] NVARCHAR (50) NULL,
    [SortOrder]     NUMERIC (18)  NULL,
    CONSTRAINT [PK_AV_SiteScript] PRIMARY KEY CLUSTERED ([SrId] ASC)
);

