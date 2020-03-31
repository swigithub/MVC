create PROCEDURE TMP_GetDashboardMap  
   @ScopeId INT = 0,
   @ProjectID INT = 0
AS  
BEGIN  
    SELECT s.SiteCode, s.SiteName, defination.DefinationName AS 'Market', s.SiteTypeId, s.Status, usr.FirstName + ' ' +  usr.LastName as 'Tester', s.Latitude, s.Longitude
	FROM AV_Sites s 
	LEFT JOIN dbo.AD_Definations defination ON s.CityId = defination.DefinationId
	LEFT JOIN dbo.Sec_Users usr ON s.TesterId = usr.UserId
	WHERE s.ProjectId = @ProjectID AND s.ScopeId = @ScopeId AND s.IsActive = 1
END