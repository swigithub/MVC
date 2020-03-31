CREATE TABLE [dbo].[PM_Colors] (
    [SrId]       NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ProjectId]  NUMERIC (18)  NULL,
    [PanelName]  NVARCHAR (50) NULL,
    [ChartName]  NVARCHAR (50) NULL,
    [ChartType]  NVARCHAR (50) NULL,
    [DataSeries] NVARCHAR (50) NULL,
    [ColorCode]  NVARCHAR (50) NULL,
    [SeriesType] NVARCHAR (50) NULL
);

