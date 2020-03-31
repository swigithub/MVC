-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_VehiclesSchedule] 
	-- Add the parameters for the stored procedure here
	@Filter nvarchar(50),
	@UserId nvarchar(50) = null,
	@VehicleId nvarchar(50) = null,
	@SiteId nvarchar(50) = null,
	@NetworkId nvarchar(50) = null,
	@BandId nvarchar(50) = null,
	@CarrierId nvarchar(50) = null,
	@ScheduleDate date = null,
	@UserRoleId nvarchar(50) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @Filter='FleetManagementPermission'
	BEGIN 
		Select * from [dbo].[Sec_RolePermissions] Where RoleId = @UserRoleId AND PermissionId = 168
	End
	ELSE
	if @Filter='VehicleAssignTesterCLS'
	BEGIN 
		Insert Into FM_WoVehicles
		(UserId, VehicleId, SiteId, NetworkId, BandId, CarrierId)
		values
		(@UserId, @VehicleId, @SiteId, @NetworkId, @BandId, @CarrierId)
	End
END