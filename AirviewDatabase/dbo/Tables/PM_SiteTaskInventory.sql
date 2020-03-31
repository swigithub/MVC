CREATE TABLE [dbo].[PM_SiteTaskInventory] (
    [SiteTaskInventoryId] NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteId]              NUMERIC (18)   NULL,
    [SiteTaskId]          NUMERIC (18)   NOT NULL,
    [CategoryId]          NUMERIC (18)   NOT NULL,
    [ItemId]              VARCHAR (50)   NULL,
    [Quantity]            INT            NULL,
    [BarCode]             NVARCHAR (100) NULL,
    [Description]         VARCHAR (300)  NULL,
    [CreatedBy]           NUMERIC (18)   NULL,
    [CreatedOn]           DATETIME       NULL,
    [ModifiedBy]          NUMERIC (18)   NULL,
    [ModifiedOn]          DATETIME       NULL,
    CONSTRAINT [PK_PM_SiteTaskInventory] PRIMARY KEY CLUSTERED ([SiteTaskInventoryId] ASC)
);

