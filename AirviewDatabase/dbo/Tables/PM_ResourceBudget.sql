CREATE TABLE [dbo].[PM_ResourceBudget] (
    [UserCostId]  NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [UserId]      NUMERIC (18) NULL,
    [HourlyCost]  FLOAT (53)   NULL,
    [HoursPerDay] FLOAT (53)   NULL,
    [CreatedOn]   DATETIME     NULL,
    [CreatedBy]   NUMERIC (18) NULL,
    CONSTRAINT [PK_PM_ResourceBudget] PRIMARY KEY CLUSTERED ([UserCostId] ASC)
);

