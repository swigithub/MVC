CREATE TYPE [dbo].[Tb_AD_ReportConfiguration] AS TABLE (
    [ReportId]  NUMERIC (18)   NULL,
    [ClientId]  NUMERIC (18)   NULL,
    [CityId]    NUMERIC (18)   NULL,
    [KeyValue]  NVARCHAR (250) NULL,
    [KeyId]     NUMERIC (18)   NULL,
    [isActive]  BIT            NULL,
    [fontColor] NVARCHAR (10)  NULL,
    [ScopeId]   NUMERIC (18)   NULL);

