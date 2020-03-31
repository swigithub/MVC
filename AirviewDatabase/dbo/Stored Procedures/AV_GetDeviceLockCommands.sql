-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_GetDeviceLockCommands]
	@MenuType nvarchar(50)
	,@NetworkModeId numeric(18, 0)
	,@BandId numeric(18, 0)
	,@DeviceModel nvarchar(50)
AS
BEGIN
	
	select * 
	from AV_DeviceLockCommands dc
	where dc.MenuType=@MenuType and dc.NetworkModeId=@NetworkModeId and dc.BandId=@BandId and dc.DeviceModel=@DeviceModel

END