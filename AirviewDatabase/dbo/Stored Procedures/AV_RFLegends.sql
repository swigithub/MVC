CREATE PROCEDURE [dbo].[AV_RFLegends]
	@SiteId numeric,
	@NetworkModeId numeric,
	@Filter nvarchar(50)
AS
BEGIN
	DECLARE @ClientId as numeric
	DECLARE @CityId as numeric

	SELECT @ClientId=sit.ClientId, @CityId=sit.CityId
	FROM AV_Sites sit WHERE sit.SiteId=@SiteId;

	IF @Filter='RFLegends_by_NetMode'
	BEGIN
		select rf.*,nm.DefinationName 'NetworkMode', pt.DefinationName 'PlotType'
		from AV_RFPlotLegends rf
		inner join AD_Definations nm on nm.DefinationId=rf.NetworkModeId
		inner join AD_Definations pt on pt.DefinationId=rf.PlotTypeId
		WHERE rf.NetworkModeId=@networkModeId and CityId=0 and rf.ClientId=@ClientId
		and rf.rangeFrom<>rf.rangeTo
	END
END