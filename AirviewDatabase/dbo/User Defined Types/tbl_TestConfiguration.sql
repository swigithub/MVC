CREATE TYPE [dbo].[tbl_TestConfiguration] AS TABLE (
    [ClientId]       NUMERIC (18)   NULL,
    [CityId]         NUMERIC (18)   NULL,
    [TestTypeId]     NUMERIC (18)   NULL,
    [KpiId]          NUMERIC (18)   NULL,
    [KpiValue]       NVARCHAR (100) NULL,
    [TestCatogoryId] NUMERIC (18)   NULL,
    [NetworkModeId]  NUMERIC (18)   NULL,
    [BandId]         NUMERIC (18)   NULL);

