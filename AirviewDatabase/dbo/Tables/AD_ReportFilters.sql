CREATE TABLE [dbo].[AD_ReportFilters] (
    [ReportId] NUMERIC (18) NOT NULL,
    [FilterId] NUMERIC (18) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_AD_ReportFilters_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_AD_ReportFilters] PRIMARY KEY CLUSTERED ([ReportId] ASC, [FilterId] ASC)
);

