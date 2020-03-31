CREATE TABLE [dbo].[AD_ClientAddress] (
    [AddressId]    NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Address]      VARCHAR (500) NULL,
    [Street]       VARCHAR (200) NULL,
    [CityId]       NUMERIC (18)  NOT NULL,
    [StateId]      NUMERIC (18)  NOT NULL,
    [CountryId]    NUMERIC (18)  NOT NULL,
    [ZipCode]      NUMERIC (18)  NULL,
    [IsHeadOffice] BIT           NOT NULL,
    [ClientId]     NUMERIC (18)  NULL,
    [IsActive]     BIT           CONSTRAINT [DF_AD_ClientAddress_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_AD_ClientAddress] PRIMARY KEY CLUSTERED ([AddressId] ASC)
);

