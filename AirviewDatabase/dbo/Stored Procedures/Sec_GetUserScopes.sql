-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Sec_GetUserScopes]
 @Filter nvarchar(50)
,@Value nvarchar(50)=null
AS
BEGIN
	if @Filter='ByUserId'
	begin
		select * 
		from Sec_UserScopes us
		where us.UserId=@Value
	end
END