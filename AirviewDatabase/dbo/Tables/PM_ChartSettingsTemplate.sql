CREATE TABLE [dbo].[PM_ChartSettingsTemplate] (
    [SrId]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [PanelName]     NVARCHAR (50) NULL,
    [ChartName]     NVARCHAR (50) NULL,
    [ChartType]     NVARCHAR (50) NULL,
    [DataSeries]    NVARCHAR (50) NULL,
    [ColorCode]     NVARCHAR (50) NULL,
    [SeriesType]    NVARCHAR (50) NULL,
    [IsActive]      BIT           NULL,
    [IsClient]      BIT           NULL,
    [IsEndClient]   BIT           NULL,
    [IsUnavoidable] BIT           NULL,
    CONSTRAINT [PK_PM_ChartSettingsTemplate] PRIMARY KEY CLUSTERED ([SrId] ASC)
);

