CREATE TABLE [dbo].[AD_Projects] (
    [ProjectID]      NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ProjectName]    NVARCHAR (150) NOT NULL,
    [ProjectScopeID] NVARCHAR (50)  NULL,
    [CompanyID]      NUMERIC (18)   NULL,
    [VendorID]       NUMERIC (18)   NULL,
    [StartDate]      DATETIME       NULL,
    [EndDate]        DATETIME       NULL,
    [StatusID]       NUMERIC (18)   NULL,
    [Color]          NVARCHAR (10)  NULL,
    [Description]    NVARCHAR (500) NULL,
    [IsActive]       BIT            CONSTRAINT [DF_AD_Projects_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_AD_Projects] PRIMARY KEY CLUSTERED ([ProjectID] ASC)
);

