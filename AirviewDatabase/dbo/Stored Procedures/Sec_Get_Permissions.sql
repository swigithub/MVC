-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
-- [dbo].[Sec_Get_Permissions] 'byUserId',26
-- [dbo].[Sec_Get_Permissions] 'ByStatus',1
CREATE PROCEDURE [dbo].[Sec_Get_Permissions] --'byUserId',26
		@FILTER varchar(max),
		@Value varchar(max) = NULL
	
		 
AS
BEGIN
			IF @FILTER = 'ByStatus'
			BEGIN
			Select * from Sec_Permissions where IsUsed=@Value
			END 
			
			else IF @FILTER = 'All'
			BEGIN
			Select * from Sec_Permissions
			END 

			ELSE IF @FILTER = 'NoUse'
			BEGIN
			Select * from Sec_Permissions  where IsUsed=0
			END

			ELSE IF @FILTER = 'ById'
			BEGIN
			Select * from Sec_Permissions  where Id=@Value and IsUsed=1 
			END


			ELSE IF @FILTER = 'LastId'
			Select max(Id) as Id from Sec_Permissions 


			ELSE IF @FILTER = 'byUserId'
			Select * from Sec_UserPermissions up inner join Sec_Permissions p on p.Id=up.PermissionId where up.UserId=@Value and IsUsed=1 order by p.Title 

			ELSE IF @FILTER = 'byRoleId'
			Select * from Sec_RolePermissions up inner join Sec_Permissions p on p.Id=up.PermissionId where up.RoleId=@Value and IsUsed=1  order by p.Title

			
END