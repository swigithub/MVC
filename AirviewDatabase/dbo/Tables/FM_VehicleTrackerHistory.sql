CREATE TABLE [dbo].[FM_VehicleTrackerHistory] (
    [VehicleTrackerHistoryId] INT      IDENTITY (1, 1) NOT NULL,
    [VehicleId]               INT      NULL,
    [UEId]                    INT      NULL,
    [TrackerDate]             DATETIME NULL
);

