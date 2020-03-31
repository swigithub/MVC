create PROCEDURE Sec_ManageUserDefinationTypes 
	@UserId int,
	@List Tb_Data READONLY
AS
BEGIN

	delete from Sec_UserDefinationType where UserId=@UserId

	INSERT INTO Sec_UserDefinationType(UserId,DefinationTypeId)
	SELECT * FROM @List
	
END