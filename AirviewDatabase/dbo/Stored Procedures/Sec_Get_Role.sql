-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE [dbo].[Sec_Get_Role]
		@FILTER varchar(max),
		@Value varchar(max)  = null
AS
BEGIN
    
			IF @FILTER = 'All'
			BEGIN
				Select * from Sec_Roles 
			END

			ELSE IF @FILTER = 'byName'
			BEGIN
				Select * from Sec_Roles where Name=@Value
			END

			ELSE IF @FILTER = 'ById'
			BEGIN
				Select * from Sec_Roles where RoleId=@Value
			END
END