

create PROCEDURE [dbo].[Sec_Insert_RolePsermissions] 
	@RoleId int,
	@List Tb_Data READONLY
AS
BEGIN

  delete from Sec_RolePermissions where RoleId=@RoleId

	 INSERT INTO Sec_RolePermissions(RoleId,PermissionId)
	SELECT * FROM @List
	
END