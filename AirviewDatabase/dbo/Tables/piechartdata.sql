CREATE TABLE [dbo].[piechartdata] (
    [SrId]       INT           IDENTITY (1, 1) NOT NULL,
    [TaskId]     NUMERIC (18)  NULL,
    [Status]     NVARCHAR (50) NULL,
    [ColorCode]  NVARCHAR (50) NULL,
    [TotalSites] INT           NULL,
    CONSTRAINT [PK_piechartdata] PRIMARY KEY CLUSTERED ([SrId] ASC)
);

