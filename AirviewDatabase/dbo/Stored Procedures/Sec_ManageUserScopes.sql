

CREATE PROCEDURE [dbo].[Sec_ManageUserScopes] 
	@UserId int,
	@List Tb_Data READONLY
AS
BEGIN

	delete from Sec_UserScopes where UserId=@UserId

	INSERT INTO Sec_UserScopes(UserId,ScopeId)
	SELECT * FROM @List
	
END