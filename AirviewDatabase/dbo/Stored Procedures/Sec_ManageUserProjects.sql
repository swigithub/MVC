

CREATE PROCEDURE [dbo].[Sec_ManageUserProjects] 

    @Filter NVARCHAR(50),
	@Id numeric (18,0)=null,
	@UserId numeric(18,0)=null,
	@List Tb_Data READONLY
AS
BEGIN

	
	if @Filter='Insert'
	begin
	
	delete from Sec_UserProjects where UserId=@Id
	INSERT INTO Sec_UserProjects (UserId,ProjectId)
	SELECT * FROM @List
	end
END