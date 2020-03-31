CREATE PROCEDURE [dbo].[AV_Insert_TestConfigurations] 
@ClientId int,
@CityId INT,
@NetworkModeId int,
@BandId int
,@List tbl_TestConfiguration READONLY
AS
BEGIN
    delete from AV_TestConfigurations where ClientId=@ClientId and CityId=@CityId and NetworkModeId = @NetworkModeId and BandId = @BandId
	INSERT INTO AV_TestConfigurations(ClientId,[CityId],[TestTypeId],[KpiId],[KpiValue],[TestCategoryId],NetworkModeId,BandId)
   SELECT * FROM @List
END