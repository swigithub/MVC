CREATE TABLE [dbo].[InActiveDevices] (
    [DeviceId]     NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [UserId]       NUMERIC (18)  NULL,
    [IMEI]         NVARCHAR (50) NULL,
    [MAC]          NVARCHAR (50) NULL,
    [Manufacturer] NVARCHAR (50) NULL,
    [Model]        NVARCHAR (50) NULL,
    [isActive]     BIT           NULL,
    [UEId]         NUMERIC (18)  NULL
);

