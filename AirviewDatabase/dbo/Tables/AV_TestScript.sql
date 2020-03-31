CREATE TABLE [dbo].[AV_TestScript] (
    [SrId]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ClientId]      NUMERIC (18)  NULL,
    [CityId]        NUMERIC (18)  NULL,
    [ScopeId]       NUMERIC (18)  NULL,
    [NetworkModeId] NUMERIC (18)  NULL,
    [BandId]        NUMERIC (18)  NULL,
    [RevisionId]    INT           CONSTRAINT [DF_AV_TestScript\_RevisionId] DEFAULT ((0)) NULL,
    [EventTypeId]   NUMERIC (18)  NULL,
    [EventValue]    NVARCHAR (50) NULL,
    [IsValue]       BIT           NULL,
    [IsL3Enabled]   BIT           NULL,
    [Color]         NVARCHAR (10) NULL,
    [SequenceId]    INT           NULL,
    [EventCommand]  NVARCHAR (50) NULL,
    [EventValue1]   NVARCHAR (50) NULL,
    [EventCommand1] NVARCHAR (50) NULL,
    CONSTRAINT [PK_AV_TestScript\] PRIMARY KEY CLUSTERED ([SrId] ASC)
);

