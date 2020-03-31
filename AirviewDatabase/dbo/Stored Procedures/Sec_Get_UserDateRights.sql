-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE [dbo].[Sec_Get_UserDateRights] --'byUserId',11
		@FILTER varchar(max),
		@Value varchar(max) = NULL
	
		 
AS
BEGIN


			IF @FILTER = 'byUserId'
			Select * from Sec_UserDateRights where UserId=@Value

			

			
END