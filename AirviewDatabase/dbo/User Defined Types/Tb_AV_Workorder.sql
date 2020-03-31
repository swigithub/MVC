﻿CREATE TYPE [dbo].[Tb_AV_Workorder] AS TABLE (
    [clusterCode]       NVARCHAR (50)  NULL,
    [Region]            NVARCHAR (50)  NULL,
    [Market]            NVARCHAR (50)  NULL,
    [Client]            NVARCHAR (50)  NULL,
    [siteCode]          NVARCHAR (50)  NULL,
    [siteLatitude]      NVARCHAR (50)  NULL,
    [siteLongitude]     NVARCHAR (50)  NULL,
    [Description]       NVARCHAR (250) NULL,
    [sectorCode]        NVARCHAR (50)  NULL,
    [networkMode]       NVARCHAR (50)  NULL,
    [Scope]             NVARCHAR (50)  NULL,
    [Band]              NVARCHAR (50)  NULL,
    [Carrier]           NVARCHAR (50)  NULL,
    [Antenna]           NVARCHAR (50)  NULL,
    [BeamWidth]         NVARCHAR (50)  NULL,
    [Azimuth]           NVARCHAR (50)  NULL,
    [PCI]               NVARCHAR (50)  NULL,
    [ReceivedOn]        DATETIME       NULL,
    [BandWidth]         NVARCHAR (50)  NULL,
    [CellId]            NVARCHAR (50)  NULL,
    [RFHeight]          NVARCHAR (50)  NULL,
    [MTilt]             NVARCHAR (50)  NULL,
    [ETilt]             NVARCHAR (50)  NULL,
    [SiteName]          NVARCHAR (50)  NULL,
    [SiteTypeId]        NVARCHAR (50)  NULL,
    [SiteClassId]       NVARCHAR (50)  NULL,
    [SiteAddress]       NVARCHAR (50)  NULL,
    [MRBTS]             NVARCHAR (50)  NULL,
    [RevisionId]        INT            NULL,
    [RedriveCount]      INT            NULL,
    [SiteId]            NUMERIC (18)   NULL,
    [SectorLatitude]    NVARCHAR (50)  NULL,
    [SectorLongitude]   NVARCHAR (50)  NULL,
    [clusterName]       NVARCHAR (50)  NULL,
    [CellFilePath]      NVARCHAR (250) NULL,
    [SurveyId]          NVARCHAR (50)  NULL,
    [ProjectId]         NUMERIC (18)   NULL,
    [SiteSurveyId]      NUMERIC (18)   NULL,
    [SectorId]          NUMERIC (18)   NULL,
    [SiteClusterId]     NUMERIC (18)   NULL,
    [VerticalBeamwidth] NUMERIC (18)   NULL,
    [AntennaDownTilt]   NVARCHAR (50)  NULL,
    [Checklist]         NVARCHAR (500) NULL,
    [Project]           NVARCHAR (50)  NULL,
    [SiteClass]         NVARCHAR (50)  NULL,
    [SiteType]          NVARCHAR (50)  NULL);

