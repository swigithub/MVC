CREATE TABLE [dbo].[AV_FloorPlan] (
    [PlanId]   NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteId]   NUMERIC (18)   NULL,
    [FloorId]  NUMERIC (18)   NULL,
    [PlanFile] NVARCHAR (500) NULL,
    [IsActive] BIT            CONSTRAINT [DF_AV_FloorPlan_IsActive] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_AV_FloorPlan] PRIMARY KEY CLUSTERED ([PlanId] ASC)
);

