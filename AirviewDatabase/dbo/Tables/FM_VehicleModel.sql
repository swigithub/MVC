CREATE TABLE [dbo].[FM_VehicleModel] (
    [ModelId]  INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    [ParentId] INT           CONSTRAINT [FM_VehicleModel_ParentId] DEFAULT ((0)) NOT NULL,
    [ManuId]   INT           NULL,
    [TypeId]   INT           NULL
);

