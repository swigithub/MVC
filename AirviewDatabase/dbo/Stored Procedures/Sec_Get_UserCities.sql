-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE [dbo].[Sec_Get_UserCities] --'byUserId',11
		@FILTER varchar(max),
		@Value varchar(max) = NULL
	
		 
AS
BEGIN

           


			if @FILTER='byUserId' begin
			Select uc.CityId,city.DefinationName 'CityName' from Sec_UserCities uc
			inner join AD_Definations city on city.DefinationId=uc.CityId
			where UserId=@Value
			end

			
END