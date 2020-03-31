﻿CREATE TABLE [dbo].[ATSites] (
    [FA_Code]                                   FLOAT (53)     NULL,
    [USID]                                      FLOAT (53)     NULL,
    [Common ID]                                 NVARCHAR (255) NULL,
    [REGION]                                    NVARCHAR (255) NULL,
    [MARKET]                                    NVARCHAR (255) NULL,
    [SUB Market]                                NVARCHAR (255) NULL,
    [Site Name]                                 NVARCHAR (255) NULL,
    [Street_Address]                            NVARCHAR (255) NULL,
    [CITY]                                      NVARCHAR (255) NULL,
    [State]                                     NVARCHAR (255) NULL,
    [ZIP]                                       FLOAT (53)     NULL,
    [COUNTY]                                    NVARCHAR (255) NULL,
    [Latitude]                                  FLOAT (53)     NULL,
    [Longitude]                                 FLOAT (53)     NULL,
    [vMME]                                      FLOAT (53)     NULL,
    [Controlled Introduction]                   FLOAT (53)     NULL,
    [Super Bowl]                                FLOAT (53)     NULL,
    [Site_Type]                                 NVARCHAR (255) NULL,
    [DAS_or_Inbuilding]                         NVARCHAR (255) NULL,
    [FirstNet RAN]                              NVARCHAR (255) NULL,
    [iPlan Job]                                 NVARCHAR (255) NULL,
    [iPlan Status]                              NVARCHAR (255) NULL,
    [iPlan Issue Date]                          DATETIME       NULL,
    [PACE Number]                               NVARCHAR (255) NULL,
    [TSS_Plan]                                  NVARCHAR (255) NULL,
    [TSS_Forecast]                              NVARCHAR (255) NULL,
    [TSS_Submitted]                             NVARCHAR (255) NULL,
    [Site_Specific_Material_Available_Forecast] NVARCHAR (255) NULL,
    [Site_Specific_Material_Available_Actual]   NVARCHAR (255) NULL,
    [Pre_Install_Planned]                       NVARCHAR (255) NULL,
    [Pre_Install_Fcst]                          DATETIME       NULL,
    [Pre_Install_Actual]                        NVARCHAR (255) NULL,
    [Mig_Date_Planned]                          NVARCHAR (255) NULL,
    [Mig_Date_Forecast]                         DATETIME       NULL,
    [Migration Date]                            DATETIME       NULL,
    [EPL Ordered]                               FLOAT (53)     NULL,
    [EPL Called Out]                            FLOAT (53)     NULL,
    [EPL Delivered]                             FLOAT (53)     NULL,
    [EPL Status]                                NVARCHAR (255) NULL
);

