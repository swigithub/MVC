CREATE TABLE [dbo].[AD_ClientContacts] (
    [ContactId]     NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ContactPerson] VARCHAR (100) NULL,
    [Designation]   VARCHAR (100) NULL,
    [Gender]        VARCHAR (10)  NULL,
    [Title]         VARCHAR (10)  NULL,
    [ContactNo]     NUMERIC (18)  NULL,
    [ContactType]   VARCHAR (10)  NULL,
    [IsPrimary]     BIT           NOT NULL,
    [ClientId]      NUMERIC (18)  NOT NULL,
    [UserId]        NUMERIC (18)  NULL,
    [RegionId]      NUMERIC (18)  NULL,
    [CityId]        NUMERIC (18)  NULL,
    [IsActive]      BIT           CONSTRAINT [DF_AD_ClientContacts_IsActive] DEFAULT ((1)) NOT NULL,
    [ReportToId]    NUMERIC (18)  NULL,
    CONSTRAINT [PK_AD_ClientContacts] PRIMARY KEY CLUSTERED ([ContactId] ASC)
);

