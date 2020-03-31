

CREATE PROCEDURE [dbo].[AV_Insert_SiteConfigurations] 

@List tbl_SiteConfiguration READONLY
AS
BEGIN
	DECLARE @PingKPI AS FLOAT=0
	DECLARE @DLKPI AS FLOAT=0
	DECLARE @ULKPI AS FLOAT=0
	DECLARE @SiteId AS NUMERIC=0
	DECLARE @NetworkModeId AS NUMERIC=0
	DECLARE @BandId AS NUMERIC=0
	DECLARE @CarrierId AS NUMERIC=0
	
	SELECT @SiteId=SiteId,@NetworkModeId=NetworkModeId,@BandId=BandId,
	@PingKPI=(SELECT x.KpiValue FROM @List x WHERE x.KpiId=51),
	@DLKPI=(SELECT x.KpiValue FROM @List x WHERE x.KpiId=62),
	@ULKPI=(SELECT x.KpiValue FROM @List x WHERE x.KpiId=63)FROM @List
	
	UPDATE AV_SiteTestSummary
	SET LatencyRate = @PingKPI, DownlinkRate = @DLKPI, UplinkRate = @ULKPI
	WHERE siteid=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId
	

SELECT @SiteId,@NetworkModeId,@BandId,@PingKPI,@ULKPI,@DLKPI

   
	INSERT INTO AV_SiteConfigurations(ClientId,[CityId],[TestTypeId],[KpiId],[KpiValue],[TestCategoryId],[SiteId],[RevisionId],NetworkModeId,BandId)
    SELECT * FROM @List
    
    
	
END