-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_Vehicles] 
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
	 @IsAssign int = null,
	 @Picture varchar(50) = null,

	 @IMEI varchar(50) = null,
	 @IMEIID int  =null,
	 @Title nvarchar(50) = null,
	 @Description varchar(500) = null,
	 @VehicleGroupId int = null,
	 @Search nvarchar(50) = null,
	 @UserID int = null

AS
BEGIN
	if @Filter='Insert_Manufacturer'
	BEGIN 
		Insert Into FM_Manufacturer (Name) values(@Name)
	End
	Else
	if @Filter='Edit_Manufacturer'
	BEGIN 
		Update FM_Manufacturer Set Name = @Name
	End
	Else
	if @Filter='Delete_Manufacturer'
	BEGIN 
		Delete from FM_Manufacturer where ManuId = @ManuId
	End
	Else
	if @Filter='Get_Manufacturer'
	BEGIN 
		Select x.DefinationId as ManuId, x.DisplayText as ManuName
		from AD_Definations x
		where x.KeyCode = 'VEHICLE MANUFACTURER' and x.PDefinationId = @TypeId
	End
	Else
	if @Filter='Insert_VehicleType'
	BEGIN 
		Insert Into FM_VehicleType (Name) values(@Name)
	End
	Else
	if @Filter='Edit_VehicleType'
	BEGIN 
		Update FM_VehicleType Set Name = @Name
	End
	Else
	if @Filter='Delete_VehicleType'
	BEGIN 
		Delete from FM_VehicleType where TypeId = @TypeId
	End
	Else
	if @Filter='Get_VehicleType'
	BEGIN 
		Select DefinationId as TypeId, DisplayText as TypeName from AD_Definations
		where
		KeyCode = 'VEHICLE TYPE'
	End
	Else
	if @Filter='Insert_VehicleModel'
	BEGIN 
		Insert Into FM_VehicleModel (Name, ParentId) values(@Name, @ParentId)
	End
	Else
	if @Filter='Edit_VehicleModel'
	BEGIN 
		Update FM_VehicleModel Set Name = @Name, ParentId = @ParentId
	End
	Else
	if @Filter='Delete_VehicleModel'
	BEGIN 
		Delete from FM_VehicleModel where ModelId = @ModelId
	End
	Else
	if @Filter='Get_VehicleModel'
	BEGIN 
		Select x.DefinationId as ModelId, x.DisplayText as ModelName
		from AD_Definations x
		where x.PDefinationId = @ManuId
	End
	Else
	if @Filter='Get_VehicleSubModel'
	BEGIN 
		Select x.DefinationId as SubModelId, x.DisplayText as SubModelName
		from AD_Definations x
		where x.PDefinationId = @ParentId
	End
	Else
	if @Filter='Insert_Vehicle'
	BEGIN 
		Insert Into FM_Vehicle (TypeId, ManuId, ModelId, SubModelId, [Year], ChassisNumber, RegistrationNumber, IsActive, VehicleGroupId)
		values(@TypeId, @ManuId, @ModelId, @SubModelId, @Year, @ChassisNumber, @RegistrationNumber, @IsActive, @VehicleGroupId);
		SELECT VehicleId from FM_Vehicle WHERE RegistrationNumber = @RegistrationNumber
		
	End
	Else
	if @Filter='Insert_Vehicle_With_Image'
	BEGIN 
		Insert Into FM_Vehicle (TypeId, ManuId, ModelId, SubModelId, Year, ChassisNumber, RegistrationNumber, IsActive, VehicleImage, VehicleGroupId)
		values(@TypeId, @ManuId, @ModelId, @SubModelId, @Year, @ChassisNumber, @RegistrationNumber, @IsActive, @Picture, @VehicleGroupId);

		SELECT SCOPE_IDENTITY() as VehicleId;

	End
	Else
	if @Filter='Edit_Vehicle_For_Tracker'
	BEGIN 
		Update FM_Vehicle Set
		IMEIId = @IMEIId
		Where VehicleId = @VehicleId
	End
	Else
	if @Filter='Edit_Vehicle'
	BEGIN 
		Update FM_Vehicle Set
		VehicleGroupId = @VehicleGroupId,
		TypeId = @TypeId,
		ManuId = @ManuId,
		ModelId = @ModelId,
		SubModelId = @SubModelId,
		Year = @Year,
		ChassisNumber = @ChassisNumber,
		RegistrationNumber = @RegistrationNumber
		--IsActive = @IsActive
		Where VehicleId = @VehicleId
	End
	Else
	if @Filter='Edit_Vehicle_With_Image'
	BEGIN 
		Update FM_Vehicle Set
		VehicleGroupId = @VehicleGroupId,
		TypeId = @TypeId,
		ManuId = @ManuId,
		ModelId = @ModelId,
		SubModelId = @SubModelId,
		Year = @Year,
		ChassisNumber = @ChassisNumber,
		RegistrationNumber = @RegistrationNumber,
		--IsActive = @IsActive,
		VehicleImage = @Picture
		Where VehicleId = @VehicleId
	End
	Else
	if @Filter='Delete_Vehicle'
	BEGIN 
		Update FM_Vehicle
		Set IsDeleted = @IsDeleted
		where VehicleId = @VehicleId
	End
	Else
	if @Filter='Get_Vehicle'
	BEGIN 
		Select * from FM_Vehicle Where IsActive = @IsActive and IsDeleted = 0
	End
	Else
	if @Filter='Get_EditVehicle'
	BEGIN 
		Select * from FM_Vehicle Where VehicleId = @VehicleId and IsDeleted = 0
	End
	Else
	if @Filter='Get_EditVehicle_By_Id'
	BEGIN 
		Select
		y.VehicleId,
		y.VehicleImage,
		y.VehicleGroupId,
		y.TypeId,
		y.ManuId,
		y.ModelId,
		y.SubModelId,
		y.ChassisNumber,
		y.RegistrationNumber,
		x.SerialNo as IMEI,
		x.UEId as IMEIId
		from FM_Vehicle y
		Inner Join AD_UserEquipments x
		On y.IMEIId = x.UEId
		Where y.VehicleId = @VehicleId and y.IsDeleted = 0

		
	End
	Else
	if @Filter='Get_Vehicle_List'
	BEGIN
		

				Select
		FM_Vehicle.VehicleId,

		(Select x.DisplayText from AD_Definations x where x.KeyCode = 'VEHICLE TYPE' and x.DefinationId = FM_Vehicle.TypeId) as TypeName,

		(Select x.DisplayText from AD_Definations x	where x.KeyCode = 'VEHICLE MANUFACTURER' and x.PDefinationId = FM_Vehicle.TypeId and x.DefinationId = FM_Vehicle.ManuId ) as ManuName,

		(Select x.DisplayText from AD_Definations x	where x.KeyCode = 'VEHICLE MODEL' and x.DefinationId = FM_Vehicle.ModelId ) as ModelName,
		(Select x.DisplayText from AD_Definations x	where x.KeyCode = 'VEHICLE SUB MODEL' and x.DefinationId = FM_Vehicle.SubModelId ) as SubModelName,
		(Select SerialNo  From  AD_UserEquipments where FM_Vehicle.IMEIId = AD_UserEquipments.UEID ) as IMEI,
		FM_Vehicle.VehicleImage,
		FM_VehicleGroup.Title as VehicleGroupName,
		(Select Top 1 CAST(Latitude as varchar)+','+CAST(Longitude as varchar) from FM_VehicleTrackingLog
		where VehicleId = FM_Vehicle.VehicleId order by	TrackingId desc) as 'Location',
		FM_Vehicle.Year,
		FM_Vehicle.ChassisNumber,
		FM_Vehicle.RegistrationNumber
		from FM_Vehicle
		Inner Join FM_VehicleGroup
		On FM_Vehicle.VehicleGroupId = FM_VehicleGroup.VehicleGroupId
		Where FM_Vehicle.IsActive = @IsActive and FM_Vehicle.IsAssign = 0 and FM_Vehicle.IsDeleted = 0
		and FM_VehicleGroup.IsDelete = 0
	End
	Else
	if @Filter='Get_Vehicle_List_Search'
	BEGIN
		select
		FM_Vehicle.VehicleId,
		AD_UserEquipments.SerialNo as IMEI,
		FM_Vehicle.VehicleImage,
		FM_VehicleGroup.Title as VehicleGroupName,

		VType.DisplayText as TypeName,
		VManu.DisplayText as ManuName,
		VModel.DisplayText as ModelName,
		VSubModel.DisplayText as SubModelName,

		(Select Top 1 CAST(Latitude as varchar)+','+CAST(Longitude as varchar) from FM_VehicleTrackingLog where VehicleId = FM_Vehicle.VehicleId order by TrackingId desc) as 'Location',
		
		FM_Vehicle.Year,
		FM_Vehicle.ChassisNumber,
		FM_Vehicle.RegistrationNumber
		from FM_Vehicle
		

		Inner Join FM_VehicleGroup
		On FM_Vehicle.VehicleGroupId = FM_VehicleGroup.VehicleGroupId

		-- AD_Definations
		Inner Join AD_Definations VType
		on FM_Vehicle.TypeId = VType.DefinationId

		Inner Join AD_Definations VManu
		on FM_Vehicle.ManuId = VManu.DefinationId and VManu.PDefinationId = FM_Vehicle.TypeId

		Inner Join AD_Definations VModel
		on FM_Vehicle.ModelId = VModel.DefinationId

		Inner Join AD_Definations VSubModel
		on FM_Vehicle.SubModelId = VSubModel.DefinationId

		left Join AD_UserEquipments
		On FM_Vehicle.IMEIId = AD_UserEquipments.UEID

		Where FM_Vehicle.IsActive = 1 and FM_Vehicle.IsAssign = 0 and FM_Vehicle.IsDeleted = 0
		and VType.KeyCode = 'VEHICLE TYPE'
		and VManu.KeyCode = 'VEHICLE MANUFACTURER'
		and VModel.KeyCode = 'VEHICLE MODEL'
		and VSubModel.KeyCode = 'VEHICLE SUB MODEL' 

		and (FM_VehicleGroup.Title like '%'+@Search+'%'

		or  VType.DisplayText like '%'+@Search+'%'
		or  VManu.DisplayText like '%'+@Search+'%'
		or  VModel.DisplayText like '%'+@Search+'%'
		or  VSubModel.DisplayText like '%'+@Search+'%'
		or AD_UserEquipments.SerialNo like '%'+@Search+'%'
		--or FM_Vehicle.Year like '%'+@Search+'%'
		--or FM_Vehicle.ChassisNumber like '%'+@Search+'%'
		or FM_Vehicle.RegistrationNumber like '%'+@Search+'%'
		)
	End
	Else
	if @Filter='Get_Vehicle_List_Search_Assigned'
	BEGIN
Select
		FM_VehicleAssignment.VehicleAssignmentId,
		AD_UserEquipments.SerialNo as IMEI,
		FM_Vehicle.VehicleImage,
		FM_Vehicle.VehicleId,
		FM_VehicleGroup.Title as VehicleGroupName,
		VType.DisplayText as TypeName,
		VManu.DisplayText as ManuName,
		VModel.DisplayText as ModelName,
		VSubModel.DisplayText as SubModelName,
		(Select Top 1 CAST(Latitude as varchar)+','+CAST(Longitude as varchar) from FM_VehicleTrackingLog where VehicleId = FM_Vehicle.VehicleId order by TrackingId desc) as 'Location',
		Sec_Users.UserId,
		Sec_Users.UserName,
		
		FM_Vehicle.Year,
		FM_Vehicle.ChassisNumber,
		FM_Vehicle.RegistrationNumber
		from FM_VehicleAssignment
		Inner Join FM_Vehicle
		On FM_VehicleAssignment.VehicleId = FM_Vehicle.VehicleId

	
		Inner Join Sec_Users
		On FM_VehicleAssignment.UserId = Sec_Users.UserId

		Inner Join FM_VehicleGroup
		On FM_Vehicle.VehicleGroupId = FM_VehicleGroup.VehicleGroupId

		-- AD_Definations
		Inner Join AD_Definations VType
		on FM_Vehicle.TypeId = VType.DefinationId

		Inner Join AD_Definations VManu
		on FM_Vehicle.ManuId = VManu.DefinationId and VManu.PDefinationId = FM_Vehicle.TypeId

		Inner Join AD_Definations VModel
		on FM_Vehicle.ModelId = VModel.DefinationId

		Inner Join AD_Definations VSubModel
		on FM_Vehicle.SubModelId = VSubModel.DefinationId

		left Join AD_UserEquipments
		On FM_Vehicle.IMEIId = AD_UserEquipments.UEID

		Where DateReturn IS NULL

		and VType.KeyCode = 'VEHICLE TYPE'
		and VManu.KeyCode = 'VEHICLE MANUFACTURER'
		and VModel.KeyCode = 'VEHICLE MODEL'
		and VSubModel.KeyCode = 'VEHICLE SUB MODEL' 
		
		and (FM_VehicleGroup.Title like '%'+@Search+'%'
		or  VType.DisplayText like '%'+@Search+'%'
		or  VManu.DisplayText like '%'+@Search+'%'
		or  VModel.DisplayText like '%'+@Search+'%'
		or  VSubModel.DisplayText like '%'+@Search+'%'
		or AD_UserEquipments.SerialNo like '%'+@Search+'%'
		
		--or FM_Vehicle.Year like '%'+@Search+'%'
		--or FM_Vehicle.ChassisNumber like '%'+@Search+'%'
		or FM_Vehicle.RegistrationNumber like '%'+@Search+'%'
		or Sec_Users.UserName like '%'+@Search+'%')
	End
	Else
	if @Filter='Get_Assigned_Vehicle_List'
	BEGIN
		
		Select
		FM_VehicleAssignment.VehicleAssignmentId,
		(Select SerialNo  From  AD_UserEquipments where FM_Vehicle.IMEIId = AD_UserEquipments.UEID ) as IMEI,
		FM_Vehicle.VehicleImage,
		FM_Vehicle.VehicleId,
		FM_VehicleGroup.Title as VehicleGroupName,
		
		(Select x.DisplayText from AD_Definations x where x.KeyCode = 'VEHICLE TYPE' and x.DefinationId = FM_Vehicle.TypeId) as TypeName,
		(Select x.DisplayText from AD_Definations x	where x.KeyCode = 'VEHICLE MANUFACTURER' and x.PDefinationId = FM_Vehicle.TypeId and x.DefinationId = FM_Vehicle.ManuId ) as ManuName,
		(Select x.DisplayText from AD_Definations x	where x.KeyCode = 'VEHICLE MODEL' and x.DefinationId = FM_Vehicle.ModelId ) as ModelName,
		(Select Top 1 CAST(Latitude as varchar)+','+CAST(Longitude as varchar) from FM_VehicleTrackingLog where VehicleId = FM_Vehicle.VehicleId order by TrackingId desc) as 'Location',
		(Select Top 1 Direction from FM_VehicleTrackingLog where IMEI = (Select SerialNo  From  AD_UserEquipments where FM_Vehicle.IMEIId = AD_UserEquipments.UEID ) order by TrackingId desc) as 'Direction',
		Sec_Users.UserId,
		Sec_Users.FirstName +' '+ Sec_Users.LastName as UserName,
		Sec_Users.Picture as UserImage,

		(Select x.DisplayText from AD_Definations x	where  x.KeyCode = 'VEHICLE SUB MODEL' and x.DefinationId = FM_Vehicle.SubModelId ) as SubModelName,
		FM_Vehicle.Year,
		FM_Vehicle.ChassisNumber,
		FM_Vehicle.RegistrationNumber
		from FM_VehicleAssignment
		
		Inner Join FM_Vehicle
		On FM_VehicleAssignment.VehicleId = FM_Vehicle.VehicleId
		
		
		Inner Join Sec_Users
		On FM_VehicleAssignment.UserId = Sec_Users.UserId
		
		Inner Join FM_VehicleGroup
		On FM_Vehicle.VehicleGroupId = FM_VehicleGroup.VehicleGroupId
		Where DateReturn IS NULL
	End
	Else
	if @Filter='Set_Vehicle_Status'
	BEGIN 
		Update FM_Vehicle
		Set IsActive = @IsActive
		Where VehicleId = @VehicleId 
	End
	Else
	if @Filter='Validate_ChassisNumber'
	BEGIN 
		Select * from FM_Vehicle where ChassisNumber = @ChassisNumber
	End
	Else
	if @Filter='Validate_RegistrationNumber'
	BEGIN 
	Select * from FM_Vehicle where RegistrationNumber = @RegistrationNumber
	End
	Else
	if @Filter='Validate_IMEI'
	BEGIN 
	Select * from FM_Vehicle where IMEI = @RegistrationNumber
	End
	Else
	if @Filter='Validate_ChassisNumber_OnUpd'
	BEGIN 
		Select * from FM_Vehicle where ChassisNumber = @ChassisNumber and VehicleId <> @VehicleId 
	End
	Else
	if @Filter='Validate_RegistrationNumber_OnUpd'
  	BEGIN 
    	Select * from FM_Vehicle where RegistrationNumber = @RegistrationNumber and VehicleId <> @VehicleId
  	End
	/* VehicleGroup Procedure Code*/
	Else
	if @Filter='Insert_VehicleGroup'
	BEGIN 
		Insert into FM_VehicleGroup
		(Title, Description, IsActive, IsDelete)
		Values
		(@Title, @Description, @IsActive, @IsDeleted)
	End
	Else
	if @Filter='Get_VehicleGroup'
	BEGIN 
	
		Select x.VehicleGroupId, x.Title, x.Description, cast((case when (Select count (distinct FM_VehicleGroup.Title)
		from FM_VehicleAssignment
		Inner Join FM_Vehicle
		On FM_VehicleAssignment.VehicleId = FM_Vehicle.VehicleId
		Inner Join FM_VehicleGroup
		On FM_Vehicle.VehicleGroupId = FM_VehicleGroup.VehicleGroupId
		Where DateReturn IS NULL and FM_Vehicle.VehicleGroupId = x.VehicleGroupId)=1 then 1 else 0 end)as bit) as IsAssign,x.IsActive, x.IsDelete from FM_VehicleGroup x			Where IsDelete = 0

	End
	Else
	if @Filter='Get_VehicleGroup_By_Id'
	BEGIN 
	
		Select x.VehicleGroupId, x.Title, x.Description, cast((case when (Select count (distinct FM_VehicleGroup.Title)
		from FM_VehicleAssignment
		Inner Join FM_Vehicle
		On FM_VehicleAssignment.VehicleId = FM_Vehicle.VehicleId
		Inner Join FM_VehicleGroup
		On FM_Vehicle.VehicleGroupId = FM_VehicleGroup.VehicleGroupId
		Where DateReturn IS NULL and FM_Vehicle.VehicleGroupId = x.VehicleGroupId)=1 then 1 else 0 end)as bit) as IsAssign,x.IsActive, x.IsDelete from FM_VehicleGroup x			Where IsDelete = 0 and VehicleGroupId = @VehicleGroupId

	End
	Else
	if @Filter='Update_VehicleGroup'
	BEGIN 
		Update FM_VehicleGroup 
		Set Title = @Title,
		Description = @Description,
		IsActive = @IsActive
		Where VehicleGroupId = @VehicleGroupId
	End
	Else
	if @Filter='Delete_VehicleGroup'
	BEGIN 
		Update FM_VehicleGroup 
		Set IsDelete = @IsDeleted
		Where VehicleGroupId = @VehicleGroupId
	End
	Else
	if @Filter='List_VehicleGroup'
	BEGIN 
		Select VehicleGroupId, Title as VehicleGroupName from FM_VehicleGroup Where IsDelete = 0 and IsActive = 1
	End
	if @Filter='List_IMEI'
	Begin


		Select UERefNo, Manufacturer +' '+ Model as ManufacturerModel, SerialNo, UEId, UENumber from [AD_UserEquipments] where UETypeID 
		in (select  DefinationId
		from AD_Definations def		
		where KeyCode='UE_TRACKER' AND def.IsActive=1 ) and not exists (select IMEIId from FM_Vehicle where IMEIId is not null and IMEIId = UEId)
	End
	ELSE
	if @Filter='List_IMEI_All'
	Begin
		Select UERefNo, Manufacturer +' '+ Model as ManufacturerModel, SerialNo, UEId, UENumber from [AD_UserEquipments] where UETypeID 
		in (select  DefinationId
		from AD_Definations def		
		where KeyCode='UE_TRACKER' AND def.IsActive=1 ) and not exists (select IMEIId from FM_Vehicle where IMEIId is not null and IMEIId = UEId)
	End
	ELSE
	if @Filter='List_IMEI_By_Id'
	Begin
		Select UERefNo, Manufacturer +' '+ Model as ManufacturerModel, SerialNo, UEId, UENumber from [AD_UserEquipments] where UETypeID 
		in (select  DefinationId
		from AD_Definations def		
		where KeyCode='UE_TRACKER' AND def.IsActive=1 ) and exists (select IMEIId from FM_Vehicle where VehicleId = @VehicleId and IMEIId = UEId)

		Select x.TrackerDate,y.UERefNo, y.Manufacturer +' '+ y.Model as ManufacturerModel, y.SerialNo, y.UEId, y.UENumber  from [AD_UserEquipments] y
		Inner Join FM_VehicleTrackerHistory x
		on x.UEId = y.UEId
		where y.UETypeID 
		in (select  DefinationId
		from AD_Definations def		
		where KeyCode='UE_TRACKER' AND def.IsActive=1 ) and x.VehicleId = @VehicleId
		order by TrackerDate 
	End
	Else	
	if @Filter='List_IMEI_By_IdWithoutHistory'
	Begin
		Select UERefNo, Manufacturer +' '+ Model as ManufacturerModel, SerialNo, UEId, UENumber from [AD_UserEquipments] where UETypeID 
		in (select  DefinationId
		from AD_Definations def		
		where KeyCode='UE_TRACKER' AND def.IsActive=1 ) and exists (select IMEIId from FM_Vehicle where VehicleId = @VehicleId and IMEIId = UEId)
	End
	Else
	if @Filter = 'GetTrackersByUserID'
	Begin
		SELECT 
		UE.UERefNo, UE.Manufacturer +' '+ UE.Model as ManufacturerModel, UE.SerialNo, UE.UEId, UE.UENumber 
		FROM 
		Sec_UserDevices SecUsersDev
		inner join 
		AD_UserEquipments UE  
		on
		SecUsersDev.UEId  =   UE.UEId

		where
		SecUsersDev.UserId = @UserID and UETypeId in 
		(select  DefinationId
		from AD_Definations def		
		where KeyCode='UE_TRACKER' AND def.IsActive=1 )
		and not exists (select IMEIId from FM_Vehicle where IMEIId is not null and IMEIId = UE.UEId)

	End
	if @Filter = 'GetTrackersByUserID_Assigned'
	Begin
	
	SELECT 
		UE.UERefNo, UE.Manufacturer +' '+ UE.Model as ManufacturerModel, UE.SerialNo, UE.UEId, UE.UENumber
		FROM 
		Sec_UserDevices SecUsersDev
		inner join 
		AD_UserEquipments UE  
		on
		SecUsersDev.UEId  =   UE.UEId

		where
		SecUsersDev.UserId = @UserID and UETypeId in 
		(select  DefinationId
		from AD_Definations def		
		where KeyCode='UE_TRACKER' AND def.IsActive=1 )
		and not exists (select IMEIId from FM_Vehicle where IMEIId is not null and IMEIId = UE.UEId)

	End
END