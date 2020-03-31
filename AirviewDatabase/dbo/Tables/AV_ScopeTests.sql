CREATE TABLE [dbo].[AV_ScopeTests] (
    [ScopeTestId]   NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ClientId]      NUMERIC (18) NULL,
    [CityId]        NUMERIC (18) NULL,
    [NetworkModeId] NUMERIC (18) NULL,
    [ScopeId]       NUMERIC (18) NULL,
    [TestTypeId]    NUMERIC (18) NULL,
    CONSTRAINT [PK_AV_ScopeTests] PRIMARY KEY CLUSTERED ([ScopeTestId] ASC)
);

