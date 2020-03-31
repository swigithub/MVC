-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--  [dbo].[AV_GetDriveRoutes] 'GetSelectedBySiteId',183394,1,'Mobility'
CREATE PROCEDURE AV_GetDriveRoutes 
	@Filter NVARCHAR(50)
   ,@Value NVARCHAR(50)=NULL
   ,@Value1 NVARCHAR(50)=NULL
   ,@Value2 NVARCHAR(50)=NULL
AS
BEGIN
	IF @filter='BySiteId'
	BEGIN
		SELECT dr.RouteId,dr.RoutePath,dr.SiteId ,dr.CreatedDate,dr.ScopeId,dr.TestType,dr.IsSelected,RoutePath +'/route-'+CONVERT(nvarchar(255), RouteId)+'.kml' 'FileName', us.FirstName+' '+us.LastName 'UserName' ,us.FirstName+' '+us.LastName 'UserName' ,ad.DefinationName 'Scope'
		FROM AV_DriveRoutes AS dr
		INNER JOIN Sec_Users AS us ON us.UserId= dr.CreatedBy
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=dr.ScopeId
		
		WHERE SiteId=@Value 
	END
	
	else IF @filter='GetSelectedBySiteId'
	BEGIN
		 -- @Value SiteId , @Value1 IsSelected 0,1, @Value2=RouteType
		SELECT DISTINCT dr.RouteId,dr.RoutePath+'//route-'+cast (dr.RouteId AS NVARCHAR) +'.kml' 'RoutePath',dr.SiteId ,dr.CreatedDate,dr.ScopeId,dr.TestType,dr.IsSelected ,us.FirstName+' '+us.LastName 'UserName' ,ad.DefinationName 'Scope','route-'+cast (dr.RouteId AS NVARCHAR) 'FileName'
		FROM AV_DriveRoutes AS dr
		INNER JOIN Sec_Users AS us ON us.UserId= dr.CreatedBy
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=dr.ScopeId
		WHERE dr.SiteId=@Value AND dr.IsSelected=1 AND
		(dr.TestType=(CASE WHEN @Value2='CW' then 'CW' WHEN @Value2='CCW' THEN 'CCW' WHEN @Value2='Cluster' THEN 'Cluster' WHEN @Value2='Mobility' THEN 'Mobility' END)
		OR dr.TestType='CW,CCW'
		)

	END
END