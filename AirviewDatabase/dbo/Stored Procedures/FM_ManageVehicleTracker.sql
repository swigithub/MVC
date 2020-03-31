-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_ManageVehicleTracker] 
	-- Add the parameters for the stored procedure here
	@Filter nvarchar(50),
	@VehicleId int = null,
	@UEId int = null
AS
BEGIN
	
	if @Filter='Insert_FM_VehicleTrackerHistory'
	BEGIN
		Insert Into FM_VehicleTrackerHistory (VehicleId, UEId, TrackerDate) values (@VehicleId, @UEId, GETDATE());
	End
END