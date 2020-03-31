CREATE TABLE [dbo].[AD_UserEquipments] (
    [UEId]         NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [UETypeId]     NUMERIC (18)   NULL,
    [Manufacturer] NVARCHAR (50)  NULL,
    [Model]        NVARCHAR (50)  NULL,
    [SerialNo]     NVARCHAR (50)  NULL,
    [MAC]          NVARCHAR (50)  NULL,
    [UENumber]     NVARCHAR (50)  NULL,
    [IsActive]     BIT            NULL,
    [UEStatusId]   NUMERIC (18)   NULL,
    [Token]        NVARCHAR (250) NULL,
    [UERefNo]      NVARCHAR (50)  NULL,
    [UEOwnerId]    NUMERIC (18)   NULL,
    CONSTRAINT [PK_AD_UserEquipments] PRIMARY KEY CLUSTERED ([UEId] ASC)
);

