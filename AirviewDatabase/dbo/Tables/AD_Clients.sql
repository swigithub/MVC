CREATE TABLE [dbo].[AD_Clients] (
    [ClientId]     NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ClientName]   VARCHAR (50)   NOT NULL,
    [IsActive]     BIT            CONSTRAINT [DF_AD_Clients_IsActive] DEFAULT ((1)) NOT NULL,
    [Logo]         NVARCHAR (MAX) NULL,
    [ClientTypeId] NUMERIC (18)   NULL,
    [PClientId]    NUMERIC (18)   NULL,
    [ClientPrefix] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_AD_Clients] PRIMARY KEY CLUSTERED ([ClientId] ASC)
);

