

CREATE PROCEDURE [dbo].[Sec_Insert_UserPsermissions] 
	@UserId int,
	@List Tb_Data READONLY
AS
BEGIN

    delete from Sec_UserPermissions where UserId=@UserId
	 INSERT INTO Sec_UserPermissions(UserId,PermissionId)
	 SELECT * FROM @List
	
END