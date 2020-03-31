-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
-- [dbo].[Sec_Get_Permissions] 'byUserId',26
-- [dbo].[Sec_Get_Permissions] 'ByStatus',1
CREATE PROCEDURE Sec_GetPermissions --'byUserId',26
		@FILTER varchar(50),
		@Value varchar(50) = NULL
		,@Value2 varchar(50) = NULL
		,@Value3 varchar(50) = NULL
	
		 
AS
BEGIN
			IF @FILTER = 'ByStatus'
			BEGIN
			Select * from Sec_Permissions where IsUsed=@Value
			END 
			---  [dbo].[Sec_GetPermissions] 'Paging',0,10,''
				else IF @FILTER = 'Paging'
				BEGIN
					Select * 
					from Sec_Permissions p
					where p.Title like '%'+@Value3+'%' or p.Code like '%'+@Value3+'%' or p.URL like '%'+@Value3+'%'
					Order By p.Id OFFSET cast(@Value as int)   ROWS FETCH NEXT cast(@Value2 as int) ROWS ONLY	

					Select count(1) 'TotalRecord' 
					from Sec_Permissions p
					where p.Title like '%'+@Value3+'%' or p.Code like '%'+@Value3+'%' or p.URL like '%'+@Value3+'%'
				    
					
				END
			else IF @FILTER = 'All'
			BEGIN
			Select * from Sec_Permissions
			END 

			ELSE IF @FILTER = 'NoUse'
			BEGIN
			Select * from Sec_Permissions  where IsUsed=0
			END
--	[dbo].[Sec_Get_Permissions] 'ById',6
			ELSE IF @FILTER = 'ById'
			BEGIN
			Select * from Sec_Permissions  where Id=@Value-- and IsUsed=1 
			END


			ELSE IF @FILTER = 'LastId'
			Select max(Id) as Id from Sec_Permissions 

--	[dbo].[Sec_GetPermissions] 'byUserId',11,'AIRIVEW_PORTAL'
			ELSE IF @FILTER = 'byUserId'
			begin
				Select * 
				from Sec_UserPermissions up 
				inner join Sec_Permissions p on p.Id=up.PermissionId 
				where up.UserId=@Value and IsUsed=1
				order by p.Title 
			end

--	[dbo].[Sec_GetPermissions] 'byUserId',10053,'AIRVIEW_ANDROID'
			ELSE IF @FILTER = 'byUserId_ModuleId'
			begin
				declare @ModuleId  numeric(18,0)=(SELECT TOP 1 def.DefinationId from AD_Definations def where def.KeyCode=@Value2)
				Select * 
				from Sec_UserPermissions up 
				inner join Sec_Permissions p on p.Id=up.PermissionId 
				where up.UserId=@Value and IsUsed=1 and p.ModuleId=@ModuleId
				order by ISNULL(p.SortOrder,0)
			end

			ELSE IF @FILTER = 'byRoleId'
			begin
				
				iF @Value2 !=null 
				BEGIN
				Select * 
				from Sec_RolePermissions up 
				inner join Sec_Permissions p on p.Id=up.PermissionId
				inner join Sec_UserPermissions sup on sup.PermissionId = up.PermissionId 
				where up.RoleId=@Value and IsUsed=1 and  sup.UserId = @Value2
				order by p.Title
				END
				ELSE
				BEGIN
				Select * 
				from Sec_RolePermissions up 
				inner join Sec_Permissions p on p.Id=up.PermissionId 
				where up.RoleId=@Value and IsUsed=1  
				order by p.Title
				END
			end			
END