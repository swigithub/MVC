CREATE TABLE [dbo].[FM_WoVehicles] (
    [WoVehicleId] INT IDENTITY (1, 1) NOT NULL,
    [UserId]      INT NOT NULL,
    [VehicleId]   INT NULL,
    [SiteId]      INT NOT NULL,
    [NetworkId]   INT NOT NULL,
    [BandId]      INT NOT NULL,
    [CarrierId]   INT NOT NULL
);

