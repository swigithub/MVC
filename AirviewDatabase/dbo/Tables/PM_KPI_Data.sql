CREATE TABLE [dbo].[PM_KPI_Data] (
    [KPI_Id]     NUMERIC (18)   NULL,
    [Date]       DATE           NOT NULL,
    [KPI_Value]  NVARCHAR (150) NULL,
    [KPIData_Id] NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [TaskId]     NUMERIC (18)   NULL,
    [Type]       NVARCHAR (50)  NULL,
    [Carrier]    NUMERIC (18)   NULL,
    [Sector]     NUMERIC (18)   NULL,
    [Site]       NUMERIC (18)   NULL,
    CONSTRAINT [PK_PM_KPI_Data] PRIMARY KEY CLUSTERED ([KPIData_Id] ASC)
);

