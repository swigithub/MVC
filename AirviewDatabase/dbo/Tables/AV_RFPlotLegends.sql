CREATE TABLE [dbo].[AV_RFPlotLegends] (
    [srId]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ClientId]      NUMERIC (18)  NULL,
    [CityId]        NUMERIC (18)  NULL,
    [NetworkModeId] NUMERIC (18)  NULL,
    [PlotTypeId]    NUMERIC (18)  NULL,
    [rangeFrom]     FLOAT (53)    NULL,
    [rangeTo]       FLOAT (53)    NULL,
    [rangeColor]    NVARCHAR (10) NULL,
    [LegendTypeId]  INT           NULL,
    CONSTRAINT [PK_AV_RFPlotLegends] PRIMARY KEY CLUSTERED ([srId] ASC)
);

