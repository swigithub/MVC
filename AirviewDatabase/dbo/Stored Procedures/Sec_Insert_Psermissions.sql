

CREATE PROCEDURE [dbo].[Sec_Insert_Psermissions] 
	@SECTORS Tbl_Permissions READONLY
AS
BEGIN


	  SET NOCOUNT ON;
      --UPDATE EXISTING RECORDS
      UPDATE Sec_Permissions
      SET Title = c2.Title
      ,URL = c2.URL,
      Code=c2.Code,
     
      IsUsed=c2.IsUsed
      FROM Sec_Permissions c1
      INNER JOIN @SECTORS c2
      ON c1.Id = c2.Id 
      
      
    --delete from Sec_RolePermissions
    --delete from Sec_UserPermissions
    --delete from Sec_Permissions

	INSERT INTO Sec_Permissions(Id,ParentId,Title,URL,Code,Icon,IsMenuItem,IsUsed)
    SELECT * FROM @SECTORS
	
END