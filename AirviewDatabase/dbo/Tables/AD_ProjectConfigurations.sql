CREATE TABLE [dbo].[AD_ProjectConfigurations] (
    [SrId]      NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ProjectId] NUMERIC (18) NOT NULL,
    [TypeId]    NUMERIC (18) NOT NULL,
    [TypeValue] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_AD_ProjectConfigurations] PRIMARY KEY CLUSTERED ([SrId] ASC)
);

