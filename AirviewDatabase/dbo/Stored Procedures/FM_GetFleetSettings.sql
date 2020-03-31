CREATE PROCEDURE [dbo].[FM_GetFleetSettings]
	-- Add the parameters for the stored procedure here
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
	if @Filter = 'Get_AlarmSettings_ByTID'
	Begin 
		Select AlarmCode,TrackerId,IsEnabled,ThresholdValues,IsApplied,ModifiedBy,ModifiedOn,SendAlert   from FM_TrackerAlarmConfiguration 
		where TrackerId= @TrackerId
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
END