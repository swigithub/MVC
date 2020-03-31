﻿CREATE TYPE [dbo].[Tb_AV_SiteTestLog] AS TABLE (
    [TestType]         VARCHAR (100)  NULL,
    [TimeStamp]        VARCHAR (100)  NULL,
    [Latitude]         FLOAT (53)     NULL,
    [Longitude]        FLOAT (53)     NULL,
    [MccId]            NVARCHAR (20)  NULL,
    [MncId]            NVARCHAR (20)  NULL,
    [Site]             NVARCHAR (50)  NULL,
    [Sector]           NVARCHAR (50)  NULL,
    [SubNetworkMode]   NVARCHAR (100) NULL,
    [ActualBand]       NVARCHAR (100) NULL,
    [ActualCarrier]    NVARCHAR (100) NULL,
    [Scope]            NVARCHAR (100) NULL,
    [Band]             VARCHAR (100)  NULL,
    [Carrier]          VARCHAR (100)  NULL,
    [NetworkMode]      VARCHAR (100)  NULL,
    [CellId]           NVARCHAR (20)  NULL,
    [LacId]            NVARCHAR (20)  NULL,
    [PciId]            NVARCHAR (20)  NULL,
    [GsmRssi]          FLOAT (53)     NULL,
    [GsmRxQual]        FLOAT (53)     NULL,
    [WcdmaRscp]        FLOAT (53)     NULL,
    [WcdmaEcio]        FLOAT (53)     NULL,
    [LteRssi]          FLOAT (53)     NULL,
    [LteRsrp]          FLOAT (53)     NULL,
    [LteRsrq]          FLOAT (53)     NULL,
    [LteRsnr]          FLOAT (53)     NULL,
    [LteCqi]           FLOAT (53)     NULL,
    [DistanceFromSite] FLOAT (53)     NULL,
    [AngleToSite]      FLOAT (53)     NULL,
    [StackTrace]       NVARCHAR (MAX) NULL,
    [TestResult]       FLOAT (53)     NULL,
    [FtpStatus]        NVARCHAR (50)  NULL,
    [IsHandover]       INT            NULL,
    [SiteRefId]        INT            NULL,
    [SectorRefId]      INT            NULL,
    [NetworkModeId]    INT            NULL,
    [BandId]           INT            NULL,
    [CarrierId]        INT            NULL,
    [ScopeId]          INT            NULL,
    [WoRefId]          NVARCHAR (100) NULL,
    [FloorId]          NUMERIC (18)   NOT NULL,
    [RRCState]         NVARCHAR (50)  NULL,
    [NetMode1]         NVARCHAR (15)  NULL,
    [Band1]            NVARCHAR (15)  NULL,
    [CH1]              NVARCHAR (15)  NULL,
    [PCI1]             INT            NULL,
    [CI1]              FLOAT (53)     NULL,
    [SS1]              FLOAT (53)     NULL,
    [SP1]              FLOAT (53)     NULL,
    [SQ1]              FLOAT (53)     NULL,
    [SNR1]             FLOAT (53)     NULL,
    [NetMode2]         NVARCHAR (15)  NULL,
    [Band2]            NVARCHAR (15)  NULL,
    [CH2]              NVARCHAR (15)  NULL,
    [PCI2]             INT            NULL,
    [CI2]              FLOAT (53)     NULL,
    [SS2]              FLOAT (53)     NULL,
    [SP2]              FLOAT (53)     NULL,
    [SQ2]              FLOAT (53)     NULL,
    [SNR2]             FLOAT (53)     NULL,
    [NetMode3]         NVARCHAR (15)  NULL,
    [Band3]            NVARCHAR (15)  NULL,
    [CH3]              NVARCHAR (15)  NULL,
    [PCI3]             INT            NULL,
    [CI3]              FLOAT (53)     NULL,
    [SS3]              FLOAT (53)     NULL,
    [SP3]              FLOAT (53)     NULL,
    [SQ3]              FLOAT (53)     NULL,
    [SNR3]             FLOAT (53)     NULL,
    [NetMode4]         NVARCHAR (15)  NULL,
    [Band4]            NVARCHAR (15)  NULL,
    [CH4]              NVARCHAR (15)  NULL,
    [PCI4]             INT            NULL,
    [CI4]              FLOAT (53)     NULL,
    [SS4]              FLOAT (53)     NULL,
    [SP4]              FLOAT (53)     NULL,
    [SQ4]              FLOAT (53)     NULL,
    [SNR4]             FLOAT (53)     NULL,
    [NetMode5]         NVARCHAR (15)  NULL,
    [Band5]            NVARCHAR (15)  NULL,
    [CH5]              NVARCHAR (15)  NULL,
    [PCI5]             INT            NULL,
    [CI5]              FLOAT (53)     NULL,
    [SS5]              FLOAT (53)     NULL,
    [SP5]              FLOAT (53)     NULL,
    [SQ5]              FLOAT (53)     NULL,
    [SNR5]             FLOAT (53)     NULL,
    [NetMode6]         NVARCHAR (15)  NULL,
    [Band6]            NVARCHAR (15)  NULL,
    [CH6]              NVARCHAR (15)  NULL,
    [PCI6]             INT            NULL,
    [CI6]              FLOAT (53)     NULL,
    [SS6]              FLOAT (53)     NULL,
    [SP6]              FLOAT (53)     NULL,
    [SQ6]              FLOAT (53)     NULL,
    [SNR6]             FLOAT (53)     NULL,
    [NetMode7]         NVARCHAR (15)  NULL,
    [Band7]            NVARCHAR (15)  NULL,
    [CH7]              NVARCHAR (15)  NULL,
    [PCI7]             INT            NULL,
    [CI7]              FLOAT (53)     NULL,
    [SS7]              FLOAT (53)     NULL,
    [SP7]              FLOAT (53)     NULL,
    [SQ7]              FLOAT (53)     NULL,
    [SNR7]             FLOAT (53)     NULL,
    [NetMode8]         NVARCHAR (15)  NULL,
    [Band8]            NVARCHAR (15)  NULL,
    [CH8]              NVARCHAR (15)  NULL,
    [PCI8]             INT            NULL,
    [CI8]              FLOAT (53)     NULL,
    [SS8]              FLOAT (53)     NULL,
    [SP8]              FLOAT (53)     NULL,
    [SQ8]              FLOAT (53)     NULL,
    [SNR8]             FLOAT (53)     NULL,
    [TCH]              NVARCHAR (15)  NULL,
    [FromPCI]          NVARCHAR (15)  NULL,
    [ToPCI]            NVARCHAR (15)  NULL,
    [PRBPercent]       FLOAT (53)     NULL,
    [MCS]              NVARCHAR (100) NULL,
    [RB]               INT            NULL,
    [Modulation]       NVARCHAR (100) NULL,
    [ModPercent]       NVARCHAR (100) NULL,
    [TM]               NVARCHAR (100) NULL,
    [RI]               INT            NULL,
    [PCPDSCH]          FLOAT (53)     NULL,
    [PCPUSCH]          FLOAT (53)     NULL,
    [SCPDSCH]          FLOAT (53)     NULL,
    [SCPUSCH]          FLOAT (53)     NULL,
    [NRBand]           VARCHAR (100)  NULL,
    [NRCarrier]        VARCHAR (100)  NULL,
    [NRRsrp]           FLOAT (53)     NULL,
    [NRRsrq]           FLOAT (53)     NULL,
    [NRRsnr]           FLOAT (53)     NULL,
    [NRCqi]            FLOAT (53)     NULL,
    [NRPci]            INT            NULL);

