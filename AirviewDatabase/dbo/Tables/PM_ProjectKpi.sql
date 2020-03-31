CREATE TABLE [dbo].[PM_ProjectKpi] (
    [KPI]           NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [Technology]    NUMERIC (18)   NULL,
    [DataType]      NUMERIC (18)   NULL,
    [Level]         NUMERIC (18)   NULL,
    [Kpi_Name]      NVARCHAR (150) NULL,
    [Kpi_Type]      NUMERIC (18)   NULL,
    [TaskId]        NUMERIC (18)   NULL,
    [IsActive]      BIT            NULL,
    [Formula]       NVARCHAR (250) NULL,
    [ComputedValue] NUMERIC (18)   NULL,
    [Weightage]     NUMERIC (18)   NULL,
    [BandId]        NUMERIC (18)   NULL,
    CONSTRAINT [PK_PM_ProjectKpi] PRIMARY KEY CLUSTERED ([KPI] ASC)
);

