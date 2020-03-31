


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- [dbo].[AV_ManageUEPbx] 'SetToken',0,'','0000000000','','sdfsdf'
CREATE PROCEDURE [dbo].[AV_ManageUEPbx]
 @Filter nvarchar(50)
,@UEId numeric(18, 0)=null
,@UEName nvarchar(50)=null
,@IMEI nvarchar(50)=null
,@IsIdle bit=null
,@DeviceToken nvarchar(250)=null

AS
BEGIN
	if @Filter='SetToken'
	begin
		update AV_UEPbx
		set DeviceToken=@DeviceToken
		where IMEI=@IMEI
	end

	else if @Filter='SetIsIdle'
	begin
		update AV_UEPbx
		set IsIdle=@IsIdle
		where IMEI=@IMEI
	end
END