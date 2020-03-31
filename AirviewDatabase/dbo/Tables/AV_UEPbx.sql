CREATE TABLE [dbo].[AV_UEPbx] (
    [UEId]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [UEName]      NVARCHAR (50)  NULL,
    [IMEI]        NVARCHAR (50)  NULL,
    [IsIdle]      BIT            NULL,
    [DeviceToken] NVARCHAR (250) NULL,
    [IsForCall]   BIT            NULL,
    CONSTRAINT [PK_AV_UEPbx] PRIMARY KEY CLUSTERED ([UEId] ASC)
);

