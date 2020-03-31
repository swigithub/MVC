CREATE PROCEDURE [dbo].[FM_ManageFleetSettings] 
	-- Add the parameters for the stored procedure here
	 @Filter nvarchar(50),
	 @AlarmCode nvarchar(50) = null,
	 @TrackerId int = null,
	 @IsEnabled bit = null,
	 @ThresholdValues numeric(6, 2) = null,
	 @ModifiedBy int = null,
	 @SendAlert bit = null,
	 @ModifiedOn DateTime =null,
	 @TrackerSerialNo  nvarchar(50) = null
AS
BEGIN
	if @Filter='Insert_UpdateTrackerAlarmConfig'
	Begin
			DECLARE @count int =0	
			SET @count = (select count (*) from FM_TrackerAlarmConfiguration where TrackerId = @TrackerId and AlarmCode =@AlarmCode)	
			select @count
		If @count > 0
		Begin
			Update FM_TrackerAlarmConfiguration Set ThresholdValues = @ThresholdValues, IsEnabled = @IsEnabled,SendAlert=@SendAlert,ModifiedBy = @ModifiedBy,ModifiedOn =  GETDATE() ,IsApplied =0
			where TrackerId= @TrackerId and AlarmCode =@AlarmCode
		End		
	
		Else if @count=0
		Begin
			Insert Into FM_TrackerAlarmConfiguration (AlarmCode, TrackerId,IsEnabled,ThresholdValues,ModifiedBy,ModifiedOn,SendAlert,IsApplied) 
												values(@AlarmCode, @TrackerId,@IsEnabled,@ThresholdValues,@ModifiedBy,GETDATE() ,@SendAlert,0)
		End
	End
	Else
	if @Filter='Insert_UpdateMultipleTrackerAlarmConfig'
	Begin
			DECLARE @counts int =0	
			SET @counts = (select count (*) from FM_TrackerAlarmConfiguration where TrackerId = @TrackerId and AlarmCode =@AlarmCode)	
			select @counts
		If @counts > 0
		Begin
			Update FM_TrackerAlarmConfiguration Set  IsEnabled = @IsEnabled,SendAlert=@SendAlert,ModifiedBy = @ModifiedBy,ModifiedOn =  GETDATE() ,IsApplied =0
			where TrackerId= @TrackerId and AlarmCode =@AlarmCode
		End		
	
		Else if @counts=0
		Begin
			Insert Into FM_TrackerAlarmConfiguration (AlarmCode, TrackerId,IsEnabled,ThresholdValues,ModifiedBy,ModifiedOn,SendAlert,IsApplied) 
												values(@AlarmCode, @TrackerId,@IsEnabled,@ThresholdValues,@ModifiedBy,GETDATE() ,@SendAlert,0)
		End
	End
	Else 

	IF @Filter='UpdateTrackerAlarmConfig'
	Begin		
		Update FM_TrackerAlarmConfiguration Set ThresholdValues = @ThresholdValues, IsEnabled = @IsEnabled,SendAlert=@SendAlert,ModifiedBy = @ModifiedBy,ModifiedOn =  GETDATE() ,IsApplied =0
		where TrackerId= @TrackerId and AlarmCode =@AlarmCode 					
	End
	Else
	
	if @Filter = 'List_IMEI_All'
	Begin 
		Select UERefNo, Manufacturer as ManufacturerModel, SerialNo, UEId, UENumber,RegistrationNumber  from [AD_UserEquipments] as AD_UE
		inner join FM_Vehicle
		on AD_UE.UEId = FM_Vehicle.IMEIId  	
	End	

	if @Filter = 'Get_AlarmSettings_ByTID'
	Begin 
		Select AlarmCode,TrackerId,IsEnabled,ThresholdValues,IsApplied,ModifiedBy,ModifiedOn,SendAlert   from FM_TrackerAlarmConfiguration 
		where TrackerId= @TrackerId
	End

	if @Filter = 'UpdateConfig_ByTID'
	Begin 
		Update FM_TrackerAlarmConfiguration Set IsApplied = 1 
		where
			 TrackerId
		in 
			(Select UEID from [AD_UserEquipments]
			 where SerialNo = @TrackerSerialNo and UEID in  (select IMEIId from FM_Vehicle where IMEIId is not null and IMEIId = UEId))
		And  AlarmCode  = @AlarmCode
	End

	if @Filter = 'GetAllAlarmsToApply_ByTID'
	Begin 
		Select AlarmCode,IsEnabled,ThresholdValues from  FM_TrackerAlarmConfiguration  
		where
			 TrackerId
		in 
			(Select UEID from [AD_UserEquipments]
			 where SerialNo = @TrackerSerialNo and UEID in  (select IMEIId from FM_Vehicle where IMEIId is not null and IMEIId = UEId))					
	End
End