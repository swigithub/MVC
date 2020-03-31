

CREATE PROCEDURE [dbo].[AV_ManageClusters] 
	-- Add the parameters for the stored procedure here
	
	@ClustorCode varchar(max),
	@Region varchar(max),
	@Market varchar(max),
	@Client varchar(max)
	
	
AS
	DECLARE @RETURN_VALUE numeric(18,0) = 0 
	IF EXISTS(select * from AV_Clusters AVS Where AVS.CityId = (Select DefinationId from AD_Definations Where DefinationName = @Market))
	BEGIN
		SELECT @RETURN_VALUE = ClusterId from AV_Clusters WHERE CityId = (Select DefinationId from AD_Definations Where DefinationName = @Market)
		RETURN @RETURN_VALUE;
	END
	ELSE
	BEGIN
		INSERT INTO AV_Clusters(ClusterCode,CityId,ClientId) VALUES(@ClustorCode,(Select DefinationId from AD_Definations Where DefinationName = @Market), (Select ClientId from AD_Clients Where ClientName = @Client))

		SELECT @RETURN_VALUE = SCOPE_IDENTITY()
		RETURN @RETURN_VALUE;
	END