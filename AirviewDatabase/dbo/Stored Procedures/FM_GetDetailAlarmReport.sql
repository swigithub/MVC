-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_GetDetailAlarmReport]
	-- Add the parameters for the stored procedure here

@Filter varchar(50) = null,
@startDate DateTime  = null,
@endDate DateTime  = null,
@Device varchar(50) = null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @Filter = 'AlarmManageHistoryReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,
			Lg.IMEI AS TrackerId,
			trackTimeStamp AS AlarmTime, ActionTime = '',
			TA.AlarmCode AS AlarmState, Action = '', AlarmProcessing = '',
			AlarmThresholdVal, AlarmCurrentVal
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE TA.AlarmCode = ''
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			
	END
	IF @Filter = 'AllAlarmsReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Lg.IMEI AS TrackerId,Direction,Rotation,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, TA.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE TA.AlarmCode IN('ExhstEmson','IdlEngn','HrdAcc','HrdDecel',
			'EngnTmp','Spd','Twng','LwVltg','Tmpr','Crsh','Emrgncy','FatgDrv',
			'ShrpTrn','QkLnChg','PwrOn','HghRPM','MIL','OBDErr','PwrOf',
			'NoGPS','PrvcyStatus','IgntOn','IllglIgnton','IllglEntr','Vib',
			'DngrousDrvng','NoCrd','UnLk','GeoFnceAlrm','IgntOff')
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'EmergencyAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Lg.IMEI AS TrackerId,Rotation,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, TA.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE TA.AlarmCode = 'Emrgncy' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'OverSpeedAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, TA.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE TA.AlarmCode = 'Spd' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'PowerTamperAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = 'Tmpr' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'OilCutAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = '' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'PowerTamperAlertReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId  
			WHERE Ad.AlarmCode = 'Tmpr' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'CrashAccidentAlertReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = 'Crsh' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'BatteryLowVoltageAlertReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = 'LwVltg' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'EmergencyHelpReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'Emrgncy' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'InformationServiceHelpReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = '' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'OriginalCarAlarmAlertReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = '' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'FatigueDrivingAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'FatgDrv'  
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF 
	@Filter = 'EngineIdleAlertReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = 'IdlEngn' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'TowingAlertReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'Twng' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'DrivingInForbiddenTimeReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId  
			WHERE Ad.AlarmCode = '' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'TempHighAlertReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'EngnTmp' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'TempLowAlertReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = '' 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'FuelStealReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId  
			WHERE Ad.AlarmCode = '' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'IllegalEngineStartedReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'IllglIgnton' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'IgnitionAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'IgntOn' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'DoorOpenAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, InDoorOpenClose AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE InDoorOpenClose = 1 AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'ShockAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = '' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'CustomAlarm1Report'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = '' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'CustomAlarm2Report'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = '' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'CustomAlarm3Report'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = '' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'CustomAlarm4Report'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = '' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'StealAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = '' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
		IF @Filter = 'ArrearageAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = '' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'ExhaustEmissionAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'ExhstEmson' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'HardAccelerationAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,Lg.IMEI AS TrackerId,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = 'HrdAcc' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'HardDecelerationAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'HrdDecel' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'EngineTempratureAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = 'EngnTmp' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'SharpTurnAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = 'ShrpTrn' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'QuickLaneChangeAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'QkLnChg' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'PowerOnAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'PwrOn' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'HighRPMAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'HghRPM' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'MILAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'MIL' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'NoGPSAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'NoGPS' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'PrivacyStatusAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'PrvcyStatus' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'DangerousDrivingAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = 'DngrousDrvng' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'NoCardAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE Ad.AlarmCode = 'NoCrd' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'UnlockAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = 'UnLk' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END
	IF @Filter = 'IgnitionOffAlarmReport'
	BEGIN	
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
			Milage,Direction,Rotation,Lg.IMEI AS TrackerId,Latitude,Longitude,GPSSignalStatus,
			InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, 
			CurrentTripMileage, Temperature, Ad.AlarmCode AS AlarmState,
			ExtendState = '',DeviceState = '',
			AlarmThresholdVal, AlarmCurrentVal 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE Ad.AlarmCode = 'IgntOff' AND
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
	END

END