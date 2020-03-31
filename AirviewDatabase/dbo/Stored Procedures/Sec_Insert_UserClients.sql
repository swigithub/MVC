

create PROCEDURE [dbo].[Sec_Insert_UserClients] 
	@UserId int,
	@List Tb_Data READONLY
AS
BEGIN

    delete from UserClients where UserId=@UserId
	 INSERT INTO UserClients(UserId,ClientId)
	 SELECT * FROM @List
	
END