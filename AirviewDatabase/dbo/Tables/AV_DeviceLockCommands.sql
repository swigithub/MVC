CREATE TABLE [dbo].[AV_DeviceLockCommands] (
    [CmdId]         NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [MenuType]      NVARCHAR (50)  NULL,
    [DeviceModel]   NVARCHAR (50)  NULL,
    [RATCode]       INT            NULL,
    [NetworkModeId] NUMERIC (18)   NULL,
    [BandCode]      INT            NULL,
    [BandId]        NUMERIC (18)   NULL,
    [RATText]       NVARCHAR (250) NULL,
    [BandText]      NVARCHAR (250) NULL,
    [CommandCode]   NVARCHAR (50)  NULL,
    CONSTRAINT [PK_AV_DeviceLockCommands] PRIMARY KEY CLUSTERED ([CmdId] ASC)
);

