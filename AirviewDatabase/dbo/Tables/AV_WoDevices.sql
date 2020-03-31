CREATE TABLE [dbo].[AV_WoDevices] (
    [WoDeviceId]       NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [BandId]           NUMERIC (18)   NULL,
    [CarrierId]        NUMERIC (18)   NULL,
    [DownloadDate]     DATETIME       NULL,
    [IsDownlaoded]     BIT            CONSTRAINT [DF_AV_WoDevices_IsDownlaoded] DEFAULT ((0)) NULL,
    [NetworkId]        NUMERIC (18)   NULL,
    [UserId]           NUMERIC (18)   NULL,
    [UserDeviceId]     NUMERIC (18)   NULL,
    [SiteId]           NUMERIC (18)   NULL,
    [ScopeId]          NUMERIC (18)   NULL,
    [WoRefId]          NVARCHAR (50)  NULL,
    [TestTypes]        NVARCHAR (100) NULL,
    [DeviceScheduleId] NUMERIC (18)   NULL,
    [NetLayerStatusId] NUMERIC (18)   NULL,
    CONSTRAINT [PK_AV_WoDevices] PRIMARY KEY CLUSTERED ([WoDeviceId] ASC)
);

