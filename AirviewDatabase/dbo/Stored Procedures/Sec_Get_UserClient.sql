-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE Sec_Get_UserClient --'byUserId',11
		@FILTER varchar(max),
		@Value varchar(max) = NULL
	
		 
AS
BEGIN

            IF @FILTER = 'byUserId'
			BEGIN
			Select uc.UserId,uc.ClientId,cl.ClientName from UserClients uc
			left join AD_Clients cl on cl.ClientId = uc.ClientId
			 where UserId=@Value
			END
			--			[dbo].[Sec_Get_UserClient]    'GetCountries_UserId',11
			ELSE IF @FILTER = 'GetCountries_UserId'
			BEGIN
			Select uc.UserId,uc.ClientId,cl.ClientName,ca.CountryId,cntry.DefinationName 'CountryName'
			from UserClients uc
			left join AD_Clients cl on cl.ClientId = uc.ClientId
			INNER JOIN AD_ClientAddress ca ON ca.ClientId=cl.ClientId
			INNER JOIN AD_Definations AS cntry ON cntry.DefinationId=ca.CountryId
			 where UserId=@Value AND ca.IsHeadOffice=1 AND cntry.IsActive=1 and ca.IsActive = 1
			END
    
    

			
END