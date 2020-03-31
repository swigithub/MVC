-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_Vehicles_Assignment]
	-- Add the parameters for the stored procedure here
	@Filter nvarchar(50),
	 @Name nvarchar(50) = null,
	 @ManuId int = null,
	 @TypeId int = null,
	 @ParentId int = null,
	 @ModelId int = null,
	 @SubModelId int = null,
	 @Year varchar(10) = null,
	 @ChassisNumber varchar(50) = null,
	 @RegistrationNumber varchar(50) = null,
	 @IsActive bit = null,
	 @IsDeleted bit = null,
	 @VehicleId int = null,
	 @UserId int = null,
	 @DateAssign date = null,
	 @DateReturn date = null,
	 @VehicleAssignmentId int = null,
	 @IMEIId int = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @Filter='GET_DT_LIST'
	BEGIN 
		Select UserId, UserName from Sec_Users
	End
	ELSE
	if @Filter='ASSIGN_VEHICLE_TO_SINGLE_DT'
	BEGIN 
		Insert Into  FM_VehicleAssignment (UserId, VehicleId, DateAssign) values (@UserId, @VehicleId, @DateAssign);

		Update FM_Vehicle
		SET
		IsAssign = 1,
		AssignTo = @UserId,
		IMEIId= @IMEIId
		Where VehicleId = @VehicleId;
	End
	ELSE
	
	if @Filter='TRANSFER_VEHICLE_TO_SINGLE_DT'
	BEGIN 
		Update FM_VehicleAssignment
		SET
		DateReturn = @DateReturn,
		IsTransfer = 1
		Where VehicleAssignmentId = @VehicleAssignmentId;

		Insert Into  FM_VehicleAssignment (UserId, VehicleId, DateAssign) values (@UserId, @VehicleId, @DateAssign);

		Update FM_Vehicle
		SET
		AssignTo = @UserId,
		IMEIId = @IMEIId 
		Where VehicleId = @VehicleId;

		
	End

	ELSE
	if @Filter='RETURN_VEHICLE_FROM_SINGLE_DT'
	BEGIN 
		Update FM_VehicleAssignment
		SET
		DateReturn = @DateReturn
		Where VehicleAssignmentId = @VehicleAssignmentId;

		Update FM_Vehicle
		SET
		IsAssign = 0,
		AssignTo = null,
		IMEIId =0
		Where VehicleId = @VehicleId;
	End
	ELSE
	if @Filter='GET_DT_VEHICLE_LIST'
	BEGIN 
		Update FM_VehicleAssignment
		SET
		DateReturn = @DateReturn
		Where VehicleAssignmentId = @VehicleAssignmentId;
	End
END