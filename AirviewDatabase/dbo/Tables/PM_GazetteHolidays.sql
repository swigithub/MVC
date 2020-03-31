CREATE TABLE [dbo].[PM_GazetteHolidays] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Title]     VARCHAR (50) NOT NULL,
    [Date]      DATE         NOT NULL,
    [IsOffday]  BIT          CONSTRAINT [DF_PM_GazetteHolidays_IsOffday] DEFAULT ((0)) NOT NULL,
    [ProjectId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_PM_GazetteHolidays] PRIMARY KEY CLUSTERED ([Id] ASC)
);

