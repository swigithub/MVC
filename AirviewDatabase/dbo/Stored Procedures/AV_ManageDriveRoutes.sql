-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE AV_ManageDriveRoutes
 @Filter NVARCHAR(50) 
,@RouteId NUMERIC(18,0)=NULL
,@RoutePath NVARCHAR(100) =NULL
,@CreatedBy NUMERIC(18,0)=NULL
,@SiteId NUMERIC(18,0)=NULL
,@ScopeId NUMERIC(18,0)=NULL
,@TestType NVARCHAR(50) =NULL
,@IsSelected BIT =NULL

AS
BEGIN
	 DECLARE @RETURN_VALUE INT = 0 
	 IF @Filter = 'Insert'
	 BEGIN
	     INSERT INTO [dbo].[AV_DriveRoutes]([CreatedDate],[RoutePath],[CreatedBy],[SiteId],ScopeId,TestType)
	     VALUES(GETDATE(),@RoutePath,@CreatedBy,@SiteId,@ScopeId,@TestType)
	     SET @RETURN_VALUE = SCOPE_IDENTITY()
	 END
	 
	 ELSE IF @Filter = 'Update'
	 BEGIN
	     UPDATE AV_DriveRoutes 
	     SET TestType=@TestType
	     WHERE RouteId=@RouteId
	      
	     SET @RETURN_VALUE = @RouteId
	 END
	 
	 
	 IF @Filter = 'UpdateIsSelected'
	 BEGIN
	 IF @IsSelected = 1
	 begin
	 select @SiteId=SiteId,@ScopeId=ScopeId,@TestType=TestType from AV_DriveRoutes where RouteId=@RouteId
	     UPDATE AV_DriveRoutes 
	     SET IsSelected=@IsSelected 
	     WHERE RouteId=@RouteId
	     SET @RETURN_VALUE = @RouteId

		UPDATE AV_DriveRoutes 
	     SET IsSelected=0 
	     WHERE SiteId=@SiteId and ScopeId=@ScopeId and  RouteId != @RouteId  
	 END
	 else 
	 begin
	   UPDATE AV_DriveRoutes 
	     SET IsSelected=@IsSelected 
	     WHERE RouteId=@RouteId
	     SET @RETURN_VALUE = @RouteId
	 end
	 end
	 RETURN @RETURN_VALUE;
END