CREATE TABLE [dbo].[PM_KPI_Threshold] (
    [Threshold]      NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [KPI]            NUMERIC (18)   NULL,
    [Condition]      NUMERIC (18)   NULL,
    [Threshold_Name] NVARCHAR (150) NULL,
    [Color]          NVARCHAR (50)  NULL,
    [Action]         NUMERIC (18)   NULL,
    [ThresholdTo]    NVARCHAR (150) NULL,
    [IsActive]       BIT            NULL,
    CONSTRAINT [PK_PM_KPI_Threshold] PRIMARY KEY CLUSTERED ([Threshold] ASC)
);

