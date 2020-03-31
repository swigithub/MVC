-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE Sec_Permission_Manage --'UpdateIsMenuItem',0,1
	@Filter varchar(max),
	@Id INT=null,
	@Value varchar(100)=NULL

AS
BEGIN
    

	IF @Filter='DeleteAll'
	begin
	delete from Sec_UserPermissions 
	delete from Sec_RolePermissions
	delete from Sec_Permissions 
	end

    ELSE IF @Filter='UpdateIsMenuItem'
	begin
	update Sec_Permissions set IsMenuItem=@Value where Id=@Id
	END
	ELSE IF @Filter='UpdateIsUsedItem'
	begin
	update Sec_Permissions set IsUsed=@Value where Id=@Id
	end

	IF @Filter='DeleteById'
	begin
	delete from Sec_UserPermissions where PermissionId=@Id
	delete from Sec_RolePermissions where PermissionId=@Id
	delete from Sec_Permissions  where Id=@Id
	end

	


END