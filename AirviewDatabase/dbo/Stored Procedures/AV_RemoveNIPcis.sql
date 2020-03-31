create PROCEDURE AV_RemoveNIPcis
    @Filter NVARCHAR(50),
	@SiteId NUMERIC(18,0) =NULL,
	@IsActive bit= 0,
	--@CarrierId NUMERIC(18,0) =NULL,
	@selectedLayers NVARCHAR(500)=NULL,
	@selectedPcis NVARCHAR(500)=NULL, 
	@AngleFrom NVARCHAR(500)=NULL, 
	@AngleTo NVARCHAR(500)=NULL, 
	@DistanceFrom NVARCHAR(500)=NULL, 
	@DistanceTo NVARCHAR(500)=NULL
	
AS
BEGIN

	

	IF @Filter='RemoveNIPcis'
	BEGIN
		SELECT 0;
	END
	
END