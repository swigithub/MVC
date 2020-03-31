﻿CREATE TABLE [dbo].[AV_NemoSiteLogs] (
    [Id]                        INT             IDENTITY (1, 1) NOT NULL,
    [Time]                      NVARCHAR (50)   NULL,
    [Current_System]            NVARCHAR (50)   NULL,
    [Longitude]                 DECIMAL (18, 5) NULL,
    [Latitude]                  DECIMAL (18, 5) NULL,
    [fileID_Fk]                 INT             NOT NULL,
    [Cell]                      NVARCHAR (50)   NULL,
    [Band]                      NVARCHAR (50)   NULL,
    [Channel]                   NVARCHAR (50)   NULL,
    [PCI_PN]                    BIGINT          NULL,
    [RSSI_dBm]                  DECIMAL (18, 5) NULL,
    [RSRQ_Ec_I0_db]             NVARCHAR (50)   NULL,
    [RSRP_RSCP_dBm]             NVARCHAR (50)   NULL,
    [Cell1]                     NVARCHAR (50)   NULL,
    [Band1]                     NVARCHAR (50)   NULL,
    [Channel1]                  NVARCHAR (50)   NULL,
    [PCI_PN1]                   BIGINT          NULL,
    [RSSI_dBm1]                 DECIMAL (18, 5) NULL,
    [RSRQ_Ec_I0_db1]            NVARCHAR (50)   NULL,
    [RSRP_RSCP_dBm1]            NVARCHAR (50)   NULL,
    [Cell2]                     NVARCHAR (50)   NULL,
    [Band2]                     NVARCHAR (50)   NULL,
    [Channel2]                  NVARCHAR (50)   NULL,
    [PCI_PN2]                   BIGINT          NULL,
    [RSSI_dBm2]                 DECIMAL (18, 5) NULL,
    [RSRQ_Ec_I0_db2]            NVARCHAR (50)   NULL,
    [RSRP_RSCP_dBm2]            NVARCHAR (50)   NULL,
    [Cell3]                     NVARCHAR (50)   NULL,
    [Band3]                     NVARCHAR (50)   NULL,
    [Channel3]                  NVARCHAR (50)   NULL,
    [PCI_PN3]                   BIGINT          NULL,
    [RSSI_dBm3]                 DECIMAL (18, 5) NULL,
    [RSRQ_Ec_I0_db3]            NVARCHAR (50)   NULL,
    [RSRP_RSCP_dBm3]            NVARCHAR (50)   NULL,
    [Cell4]                     NVARCHAR (50)   NULL,
    [Band4]                     NVARCHAR (50)   NULL,
    [Channel4]                  NVARCHAR (50)   NULL,
    [PCI_PN4]                   BIGINT          NULL,
    [RSSI_dBm4]                 DECIMAL (18, 5) NULL,
    [RSRQ_Ec_I0_db4]            NVARCHAR (50)   NULL,
    [RSRP_RSCP_dBm4]            NVARCHAR (50)   NULL,
    [Cell5]                     NVARCHAR (50)   NULL,
    [Band5]                     NVARCHAR (50)   NULL,
    [Channel5]                  NVARCHAR (50)   NULL,
    [PCI_PN5]                   BIGINT          NULL,
    [RSSI_dBm5]                 DECIMAL (18, 5) NULL,
    [RSRQ_Ec_I0_db5]            NVARCHAR (50)   NULL,
    [RSRP_RSCP_dBm5]            NVARCHAR (50)   NULL,
    [Cell6]                     NVARCHAR (50)   NULL,
    [Band6]                     NVARCHAR (50)   NULL,
    [Channel6]                  NVARCHAR (50)   NULL,
    [PCI_PN6]                   BIGINT          NULL,
    [RSSI_dBm6]                 DECIMAL (18, 5) NULL,
    [RSRQ_Ec_I0_db6]            NVARCHAR (50)   NULL,
    [RSRP_RSCP_dBm6]            NVARCHAR (50)   NULL,
    [Cell7]                     NVARCHAR (50)   NULL,
    [Band7]                     NVARCHAR (50)   NULL,
    [Channel7]                  NVARCHAR (50)   NULL,
    [PCI_PN7]                   BIGINT          NULL,
    [RSSI_dBm7]                 DECIMAL (18, 5) NULL,
    [RSRQ_Ec_I0_db7]            NVARCHAR (50)   NULL,
    [RSRP_RSCP_dBm7]            NVARCHAR (50)   NULL,
    [Cell8]                     NVARCHAR (50)   NULL,
    [Band8]                     NVARCHAR (50)   NULL,
    [Channel8]                  NVARCHAR (50)   NULL,
    [PCI_PN8]                   BIGINT          NULL,
    [RSSI_dBm8]                 DECIMAL (18, 5) NULL,
    [RSRQ_Ec_I0_db8]            NVARCHAR (50)   NULL,
    [RSRP_RSCP_dBm8]            NVARCHAR (50)   NULL,
    [Cell9]                     NVARCHAR (50)   NULL,
    [Band9]                     NVARCHAR (50)   NULL,
    [Channel9]                  NVARCHAR (50)   NULL,
    [PCI_PN9]                   BIGINT          NULL,
    [RSSI_dBm9]                 DECIMAL (18, 5) NULL,
    [RSRQ_Ec_I0_db9]            NVARCHAR (50)   NULL,
    [RSRP_RSCP_dBm9]            NVARCHAR (50)   NULL,
    [HO_Type]                   NVARCHAR (50)   NULL,
    [Current_Channel]           NVARCHAR (50)   NULL,
    [Current_Band]              NVARCHAR (50)   NULL,
    [Attempted_System]          NVARCHAR (50)   NULL,
    [Attempted_PCI]             NVARCHAR (50)   NULL,
    [Attempted_Band]            NVARCHAR (50)   NULL,
    [HO_Duration_ms]            NVARCHAR (50)   NULL,
    [HO_Uplane_interruption_ms] NVARCHAR (50)   NULL,
    [Event]                     NVARCHAR (250)  NULL,
    [Call_Type]                 NVARCHAR (50)   NULL,
    [Call_Direction]            NVARCHAR (50)   NULL,
    [MO_MTPhone_Number]         NVARCHAR (50)   NULL,
    [Call_Connection_Status]    NVARCHAR (50)   NULL,
    [Ping_Size_bytes]           NVARCHAR (50)   NULL,
    [RTT_ms]                    NVARCHAR (50)   NULL,
    [Connection_Setup_Time_ms]  NVARCHAR (50)   NULL,
    [Current_PCI]               NVARCHAR (50)   NULL,
    [Attempted_Channel]         NVARCHAR (50)   NULL,
    [PDSCH_DL_Throughput_bits]  NVARCHAR (50)   NULL,
    [PUSCH_UL_Throughput_bits]  NVARCHAR (50)   NULL,
    [Direction]                 NVARCHAR (50)   NULL,
    [EventFields]               NVARCHAR (250)  NULL,
    [SNR]                       DECIMAL (18, 5) NULL,
    [SNR1]                      DECIMAL (18, 5) NULL,
    [SNR2]                      DECIMAL (18, 5) NULL,
    [SNR3]                      DECIMAL (18, 5) NULL,
    [SNR4]                      DECIMAL (18, 5) NULL,
    [SNR5]                      DECIMAL (18, 5) NULL,
    [SNR6]                      DECIMAL (18, 5) NULL,
    [SNR7]                      DECIMAL (18, 5) NULL,
    [SNR8]                      DECIMAL (18, 5) NULL,
    CONSTRAINT [PK_tsvFileInformation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_tsvFileInformation_flieInformation] FOREIGN KEY ([fileID_Fk]) REFERENCES [dbo].[AV_LogsInfo] ([fileID])
);

