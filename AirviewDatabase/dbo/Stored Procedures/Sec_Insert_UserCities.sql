

CREATE PROCEDURE [dbo].[Sec_Insert_UserCities] 
     @Filter nvarchar(50)
	,@UserId int
	,@List Tb_Data READONLY
AS
BEGIN

	if @Filter='UserCities'
	begin

		 delete from Sec_UserCities where UserId=@UserId
		 INSERT INTO Sec_UserCities(UserId,CityId)
		 SELECT * FROM @List
	end

    else if @Filter='CityUsers'
	begin

		 delete from Sec_UserCities where CityId=@UserId
		 INSERT INTO Sec_UserCities(CityId,UserId)
		 SELECT * FROM @List
	end
     
	
END