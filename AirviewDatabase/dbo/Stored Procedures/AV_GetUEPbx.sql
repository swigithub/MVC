-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[AV_GetUEPbx]
 @Filter nvarchar(50)
,@Value nvarchar(50)
AS
BEGIN
	if @Filter='byIsIdle'
	begin
		select * 
		from AV_UEPbx 
		where IsIdle=@Value

	end
END