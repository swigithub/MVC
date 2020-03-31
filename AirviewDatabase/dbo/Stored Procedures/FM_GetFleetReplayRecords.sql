-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_GetFleetReplayRecords] 

@Filter varchar(100) = null,
@Device varchar(100) = null,
@resultDate DateTime = null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @Filter = 'ParkingReport'
	BEGIN
	-- Insert statements for procedure here
		WITH CTE  
			AS
		(
			SELECT RegistrationNumber,lg.IMEI,
			StartTime = CONVERT(varchar(7),DATEADD(minute, 
			DATEDIFF(minute, format(DATEADD(MINUTE,AlarmCurrentVal,CAST(CAST(0 AS FLOAT) AS DATETIME)),'HH:mm:ss'), 
			trackTimeStamp), 0), 114), Convert(time(0),trackTimeStamp) AS EndTime,
			Latitude,Longitude
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE TA.AlarmCode = 'IdlEngn' AND Status = 1
			AND RegistrationNumber = @Device AND Convert(date,trackTimeStamp) =  @resultDate 
		)
			SELECT *,CONVERT(TIME(0),DATEADD(MS,DATEDIFF(SS, StartTime, EndTime )*1000,0),114) AS ParkingTime
			FROM CTE
	
	END

	IF @Filter = 'IdleEngineReport'
	BEGIN
	-- Insert statements for procedure here
		WITH CTE  
			AS
		(
			SELECT RegistrationNumber,lg.IMEI,
			StartTime = CONVERT(varchar(7),DATEADD(minute, 
			DATEDIFF(minute, format(DATEADD(MINUTE,AlarmCurrentVal,CAST(CAST(0 AS FLOAT) AS DATETIME)),'HH:mm:ss'), 
			trackTimeStamp), 0), 114), Convert(time(0),trackTimeStamp) AS EndTime,
			Latitude,Longitude
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE TA.AlarmCode = 'IdlEngn' AND Status = 1
			AND RegistrationNumber = @Device AND Convert(date,trackTimeStamp) =  @resultDate 
		)
			SELECT *,CONVERT(TIME(0),DATEADD(MS,DATEDIFF(SS, StartTime, EndTime )*1000,0),114) AS IdleTime
			FROM CTE
	
	END

	IF @Filter = 'DrivingReport'
	BEGIN
	-- Insert statements for procedure here
	SELECT RegistrationNumber,lg.IMEI, ParkingTime = '',
	MIN(trackTimeStamp) AS StartTime, MAX(trackTimeStamp) AS EndTime,
	Latitude,Longitude
	FROM FM_VehicleTrackingLog AS Lg 
	inner join FM_Vehicle x 
	ON x.VehicleId = Lg.VehicleId
	WHERE RegistrationNumber = @Device AND Convert(date,trackTimeStamp) =  @resultDate
	GROUP BY RegistrationNumber,Lg.IMEI,Latitude,Longitude
	END

	IF @Filter = 'MileageReport'
	BEGIN
	-- Insert statements for procedure here
	
	SELECT RegistrationNumber,lg.IMEI, MAX(Odometer)-MIN(Odometer) AS RunningMileage,
	MIN(trackTimeStamp) AS StartTime, MAX(trackTimeStamp) AS EndTime,
	StartMileage = MIN(Odometer),MAX(Odometer) AS EndMileage
	FROM FM_VehicleTrackingLog AS Lg 
	inner join FM_Vehicle x 
	ON x.VehicleId = Lg.VehicleId
	WHERE RegistrationNumber = @Device AND Convert(date,trackTimeStamp) = @resultDate
	GROUP BY RegistrationNumber,Lg.IMEI,Convert(date,trackTimeStamp)
	END

	IF @Filter = 'FatigueDriving'
	BEGIN
	-- Insert statements for procedure here
	SELECT RegistrationNumber,lg.IMEI, Status,
	StartTime = CONVERT(varchar(7), 
	DATEADD(minute, 
	DATEDIFF(minute, format(DATEADD(MINUTE,AlarmCurrentVal,CAST(CAST(0 AS FLOAT) AS DATETIME)),'HH:mm:ss'), 
	trackTimeStamp), 0), 114), Convert(time(0),trackTimeStamp) AS EndTime
	FROM FM_VehicleTrackingLog AS Lg 
	inner join FM_TrackingAlarmDetails Ad
	ON ad.TrackingID = Lg.TrackingId
	inner join  FM_TrackingAlarm TA
	on TA.AlarmCode = AD.AlarmCode
	inner join FM_Vehicle x 
	ON x.VehicleId = Lg.VehicleId 
	WHERE TA.AlarmCode = 'FatgDrv' AND Status = 1 AND 
	RegistrationNumber = @Device AND Convert(date,trackTimeStamp) =  @resultDate
	GROUP BY RegistrationNumber,Lg.IMEI,Status,AlarmCurrentVal,trackTimeStamp
	END

	IF @Filter = 'Alarms'
	BEGIN
	SELECT  RegistrationNumber,lg.IMEI,trackTimeStamp AS Time,Speed,Odometer,
	Direction,Rotation,Latitude,Longitude,GPSSignalStatus,
	InEngineOnOff, 
	Address, FuelPercent, FuelLiter,GSMSignal, 
	CurrentTripMileage,AD.[Status] AS AlarmState,
	AlarmThresholdVal, AlarmCurrentVal,TA.[Description] AS AlarmDescription
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
	AND Convert(date,trackTimeStamp) = @resultDate
	AND RegistrationNumber = @Device
	END

	IF @Filter = 'DetailTrack'
	BEGIN
	SELECT RegistrationNumber,lg.IMEI,trackTimeStamp AS Time,Speed,Odometer,
	Direction,Rotation,Latitude,Longitude,GPSSignalStatus,InEngineOnOff, 
	Address, FuelPercent, FuelLiter,GSMSignal, CurrentTripMileage, Temperature
	FROM FM_VehicleTrackingLog AS Lg 
	inner join FM_Vehicle x 
	ON x.VehicleId = Lg.VehicleId
	WHERE Convert(date,trackTimeStamp) = @resultDate AND RegistrationNumber = @Device
	
	END
END