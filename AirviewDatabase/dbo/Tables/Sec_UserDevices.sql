CREATE TABLE [dbo].[Sec_UserDevices] (
    [DeviceId]     NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [UserId]       NUMERIC (18)  NULL,
    [IMEI]         NVARCHAR (50) NULL,
    [MAC]          NVARCHAR (50) NULL,
    [Manufacturer] NVARCHAR (50) NULL,
    [Model]        NVARCHAR (50) NULL,
    [isActive]     BIT           CONSTRAINT [DF_Sec_UserDevices_isActive] DEFAULT ((1)) NULL,
    [UEId]         NUMERIC (18)  NULL,
    CONSTRAINT [PK_Sec_UserDevices] PRIMARY KEY CLUSTERED ([DeviceId] ASC),
    CONSTRAINT [FK_Sec_UserDevices_Sec_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Sec_Users] ([UserId])
);

