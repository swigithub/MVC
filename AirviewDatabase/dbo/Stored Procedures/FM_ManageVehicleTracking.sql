-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_ManageVehicleTracking]
	-- Add the parameters for the stored procedure here
	@Filter varchar(50) = null,
	@IMEI varchar(50) = null,
	@Latitude decimal(12,9) = null,
	@Longitude decimal(12,9) = null,
	@Speed decimal(18,0) = null,
	@Odometer decimal(18,0) = null,
	@Direction decimal(18,0) = null,
	@Rotation varchar(50) = null,
	@Altitude  decimal(18,0) = null,
	@TrackerStream text = null,
	@GPSSignalStatus varchar(50) = null,
	@UTCTimeAndDate dateTime = null,
	@VehicleId int = null,
	@FromDate datetime = null,
	@ToDate datetime = null,

	-- Tracker Other Parameters

	@OutLockthedoor bit = null,
	@OutSirenSound bit = null,
	@OutUnlockthedoor bit = null,
	@OutRelyToStopCar bit = null,

	@InSOS bit = null,
	@InAntiTemper bit = null,
	@InDoorOpenClose bit = null,
	@InUnlockDoor bit = null,
	@InEngineOnOff bit = null,
	@IsOfflineData bit = null,
	
	--Track Position Tab
	@DeviceState	nvarchar(50)= null,	
	@AssetStatus	nvarchar(50)= null,	
	@ExtendState	nvarchar(50)= null,
	@Address		nvarchar(50)= null,	
	@GSMSignal		nvarchar(50)= null,	
	@Temperature	float	= null,
	@CurrentTripMileage	float= null,
	@FuelLiter		float	= null,
	@FuelPercent	float	= null,
	
	--Tracking Alarm Status
	@AlarmCode nvarchar(15)=null,
	@AlarmThreshodVal float = null,
	@AlarmCurrentVal float = null,
	@Status bit = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	If @Filter = 'InsertTrackerLog'
	Begin		
		select @VehicleId = x.VehicleId from AD_UserEquipments y
		Inner Join FM_Vehicle x
		on x.IMEIId  = y.UEid
		where SerialNo = @IMEI

		Insert Into FM_VehicleTrackingLog
		(VehicleId,IMEI, Latitude, Longitude, Speed, Odometer, Direction, Rotation, Altitude, TrackerStream, GPSSignalStatus, UTCTimeAndDate,trackTimeStamp, OutLockthedoor, OutSirenSound, OutUnlockthedoor, OutRelyToStopCar, InSOS, InAntiTemper, InDoorOpenClose, InUnlockDoor, InEngineOnOff, IsOfflineData, DeviceState, AssetStatus, ExtendState, [Address], GSMSignal, Temperature, CurrentTripMileage, FuelLiter, FuelPercent)
		Values
		(@VehicleId,@IMEI, @Latitude,@Longitude, @Speed, @Odometer, @Direction, @Rotation, @Altitude, @TrackerStream, @GPSSignalStatus, @UTCTimeAndDate,GETDATE(), @OutLockthedoor, @OutSirenSound, @OutUnlockthedoor, @OutRelyToStopCar, @InSOS, @InAntiTemper, @InDoorOpenClose, @InUnlockDoor, @InEngineOnOff, @IsOfflineData,@DeviceState, @AssetStatus, @ExtendState, @Address, @GSMSignal, @Temperature, @CurrentTripMileage, @FuelLiter, @FuelPercent)
	End
	ELSE
	If @Filter = 'Get_Drive_Dates'
	Begin
		select  Count(*) as Counter, CONVERT(VARCHAR,DATEADD(DAY,0,trackTimeStamp),106) as DrivePlayDate
		from FM_VehicleTrackingLog where VehicleId = @VehicleId
		GROUP BY CONVERT(VARCHAR,DATEADD(DAY,0,trackTimeStamp),106)

	End
	ELSE
	If @Filter = 'Get_Drive_Date_KML'
	BEGIN
		SELECT
		'<?xml version="1.0" encoding="UTF-8"?><kml xmlns="http://earth.google.com/kml/2.1"><Document><name>'+ 'NY001' +'</name><description>'+'NY001'+'</description>'+
		REPLACE(REPLACE(REPLACE(
			(SELECT '<Style id="Id'+CAST(ROW_NUMBER() OVER (ORDER BY x.trackingid) as nvarchar)+'">'+
			'<LineStyle>'+
			'<color>ff' + '4286f4' + '</color>'+
			'<width>10</width>'+
			'</LineStyle>'+
			'</Style>'+
			'<Placemark>'+
			'<name>cw1</name>'+
			'<styleUrl>#Id'+CAST(ROW_NUMBER() OVER (ORDER BY x.trackingid) as nvarchar)+'</styleUrl>'+
			'<LineString>'+
			'<altitudeMode>relative</altitudeMode>'+
			'<coordinates>'+ CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0 '+CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15))+','+CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15))+',0'+'</coordinates>'+
			'</LineString>'+
			'</Placemark>'  + ' '
			 FROM FM_VehicleTrackingLog x
		where x.VehicleId = @VehicleId and x.trackTimeStamp between @FromDate and @ToDate		
		FOR XML PATH('')),'&lt;/','</'),'&gt;','>'),'&lt;','<')+
		'</Document></kml>' 'KML'
	End
	Else
	If @Filter = 'Get_Drive_Date_KML_details'
	BEGIN
		SELECT
		CAST(CAST(x.Latitude AS DECIMAL(10,7)) AS NVARCHAR(15)) as Latitude,
		CAST(CAST(x.Longitude AS DECIMAL(10,7)) AS NVARCHAR(15)) as Longitude,
		x.Speed,
		x.Direction,
		x.Rotation,
		x.Altitude,
		x.GPSSignalStatus,
		x.trackTimeStamp as UTCTimeAndDate,
		x.Odometer,
		x.OutLockthedoor,
		x.OutSirenSound,
		x.OutUnlockthedoor,
		x.OutRelyToStopCar,
		x.InSOS,
		x.InAntiTemper,
		x.InDoorOpenClose,
		x.InUnlockDoor,
		x.InEngineOnOff,
		x.DeviceState,
		x.AssetStatus,
		x.ExtendState,
		x.[Address], 
		x.GSMSignal,
		x.Temperature,
		x.CurrentTripMileage,
		x.FuelLiter,
		x.FuelPercent
		FROM FM_VehicleTrackingLog x
		where x.VehicleId = @VehicleId and x.trackTimeStamp between @FromDate and @ToDate	
		order by x.trackTimeStamp
	End
	Else
	If @Filter ='Get_All_IMEI'
	Begin
		Select distinct UE.SerialNo as IMEI 
		FROM FM_Vehicle Vehicle
		inner join [AD_UserEquipments] as UE
		on UE.UEId=  Vehicle.IMEIId

		--Select SerialNo as IMEI from [AD_UserEquipments] 
		--where UETypeID 
		--in (select  DefinationId
		--from AD_Definations def		
		--where KeyCode='UE_TRACKER' AND def.IsActive=1 )
	End 
	Else
	If @Filter ='Validate_IMEI'
	Begin
		Select UE.SerialNo as IMEI 
		FROM FM_Vehicle Vehicle
		inner join [AD_UserEquipments] as UE
		on UE.UEId=  Vehicle.IMEIId
		and UE.SerialNo =@IMEI

		--Select SerialNo as IMEI from [AD_UserEquipments] where UETypeID 
		--in (select  DefinationId
		--from AD_Definations def		
		--where KeyCode='UE_TRACKER' AND def.IsActive=1 ) and SerialNo =@IMEI
	End 
	Else
	IF @Filter = 'Insert_VState'
	Begin
		select @VehicleId = x.VehicleId from AD_UserEquipments y
		Inner Join FM_Vehicle x
		on x.IMEIId  = y.UEid
		where SerialNo = @IMEI

		Declare @MaxVal int
		Set @MaxVal=(Select Max(TrackingID) from FM_VehicleTrackingLog 
					where VehicleId = @VehicleId and IMEI =@IMEI and Latitude = @Latitude 
					and Longitude = @Longitude)

		Insert Into FM_TrackingVState 
		(TrackingID,AlarmCode,[Status])
		Values
		(@MaxVal,@AlarmCode,@Status)
	End
	ELSE
	IF @Filter = 'Insert_TrackingAlarms'
	Begin 
	
		SELECT @VehicleId = x.VehicleId FROM AD_UserEquipments y
		INNER JOIN FM_Vehicle x
		ON x.IMEIId  = y.UEid
		WHERE SerialNo = @IMEI

		DECLARE @TID INT
		SET @TID=(SELECT MAX(TrackingID) FROM FM_VehicleTrackingLog 
				  WHERE VehicleId = @VehicleId AND IMEI =@IMEI AND Latitude = @Latitude 
				  AND Longitude = @Longitude)

		INSERT INTO FM_TrackingAlarmDetails
		(TrackingID,AlarmCode,AlarmThresholdVal,AlarmCurrentVal,[Status])
		VALUES
		(@TID,@AlarmCode,@AlarmThreshodVal,@AlarmCurrentVal,@Status)

	End
	ELSE
	IF @Filter = 'GetAlarmsForNotification'
	Begin
	DECLARE @TrID INt
	SET @TrID=(SELECT UEID from AD_UserEquipments where SerialNo = @IMEI)
	SELECT AlarmCode,FM_Vehicle.RegistrationNumber,TrackerID,FM_Vehicle.AssignTo from FM_TrackerAlarmConfiguration config
	inner join FM_Vehicle on FM_Vehicle.IMEIId= config.TrackerId
	where TrackerID = @TrID and IsEnabled =1 and SendAlert= 1 
	End
END