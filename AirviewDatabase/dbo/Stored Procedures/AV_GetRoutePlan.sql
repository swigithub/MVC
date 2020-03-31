CREATE PROCEDURE [dbo].[AV_GetRoutePlan]
	@TesterId NUMERIC,
	@FilterOption NVARCHAR(50)
AS
BEGIN
	IF @FilterOption='GET_TESTER_ROUTE'
	BEGIN
		SELECT DISTINCT sit.SiteId,sit.SiteCode,nl.TesterId,sit.Latitude,sit.Longitude 
		FROM AV_Sites sit
		INNER JOIN AV_NetLayerStatus nl ON nl.SiteId=sit.SiteId
		WHERE sit.IsActive=1 AND nl.IsActive=1 AND nl.TesterId=@TesterId AND CAST(nl.ScheduledOn AS DATE)=CAST(GETDATE() AS DATE)
		UNION ALL
		SELECT NULL,NULL,usr.UserId 'TesterId',usr.homeLatitude,usr.homeLongitude 
		FROM Sec_Users usr		
		WHERE usr.UserId=@TesterId AND (usr.homeLatitude IS NOT NULL AND usr.homeLongitude IS NOT NULL)
	END
END