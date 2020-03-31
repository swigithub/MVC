CREATE TABLE [dbo].[AV_WoTracker] (
    [WoTrackerId]      NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [SiteId]           NUMERIC (18)  NULL,
    [SectorId]         NUMERIC (18)  NULL,
    [NetworkModeId]    NUMERIC (18)  NULL,
    [BandId]           NUMERIC (18)  NULL,
    [CarrierId]        NUMERIC (18)  NULL,
    [WoRefId]          NVARCHAR (50) NULL,
    [Latitude]         FLOAT (53)    NULL,
    [Longitude]        FLOAT (53)    NULL,
    [TesterId]         NUMERIC (18)  NULL,
    [TestType]         NVARCHAR (50) NULL,
    [TrackerTimestamp] DATETIME      CONSTRAINT [DF_AV_WoTracker_TrackerTimestamp] DEFAULT (getdate()) NULL,
    [AppVersion]       NVARCHAR (50) NULL,
    [AndroidVersion]   NVARCHAR (50) NULL,
    CONSTRAINT [PK_AV_WoTracker] PRIMARY KEY CLUSTERED ([WoTrackerId] ASC)
);

