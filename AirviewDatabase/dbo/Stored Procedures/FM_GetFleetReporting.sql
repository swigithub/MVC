-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_GetFleetReporting] 

@Filter varchar(50) = null,
@startDate DateTime  = null,
@endDate DateTime  = null,
@Device varchar(50) = null,
@Search varchar(50) = null,
@Radius int = 0,
@Latitude varchar(MAX) = null,
@Longitude varchar(MAX) = null,
@Geo1 Geography =  null,
@startWork time = null,
@endWork time = null,
@ParkingTime int = null,
@DrivingTime int = null

AS
Declare @Parktime int = 0;
Declare @Drivetime int = 0;
Declare @StartingWork time = null;
Declare @EndingWork time = null;
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		IF @Filter = 'Get_Device'
	BEGIN
			SELECT RegistrationNumber AS Device
			FROM FM_Vehicle
	
	END

	IF @Filter = 'DetailedTripReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,Lg.IMEI As TrackerId,trackTimeStamp AS Time,Speed,Odometer AS 
			Mileage,Direction,Rotation,Latitude,Longitude,GPSSignalStatus,InEngineOnOff AS AssetStatus, 
			Address, FuelPercent, FuelLiter,GSMSignal, CurrentTripMileage, Temperature, AlarmState = '',
			ExtendState = '',DeviceState = '' 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate AND RegistrationNumber = @Device
	
	END
	
	IF @Filter = 'LastPositionReport'
	BEGIN
		 
			WITH cte AS
			(
			  SELECT RegistrationNumber AS AssetName,Lg.IMEI As TrackerId,trackTimeStamp AS Time,Speed,Odometer AS 
			  Mileage,Direction,Rotation,Latitude,Longitude,GPSSignalStatus,InEngineOnOff AS AssetStatus, 
			  Address, FuelPercent, FuelLiter,GSMSignal, CurrentTripMileage, Temperature, AlarmState = '',
			  ExtendState = '',DeviceState = '' ,
			  row_number() over(partition by datediff(d, 0, trackTimeStamp) order by trackTimeStamp desc) as rn 
			  FROM FM_VehicleTrackingLog AS Lg 
			  inner join FM_Vehicle x 
			  ON x.VehicleId = Lg.VehicleId
			)
			SELECT *
			FROM cte  
			WHERE rn = 1 AND [Time] >=@startDate AND [Time] <=@endDate
	END
	IF @Filter = 'PasserByLocationReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Address,
			Lg.IMEI As TrackerId,
			AlarmState = '', DeviceState = '' , InEngineOnOff AS AssetStatus,
			ExtendState = '', Speed,FuelPercent, FuelLiter,
			Odometer AS Mileage,Direction,Rotation,
			Latitude,Longitude,GPSSignalStatus,CurrentTripMileage,
			Temperature, GSMSignal,   
			Cast(dbo.fnCalcDistanceKM(Latitude,@Latitude,Longitude,@Longitude) as numeric(36,3)) as Distance
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId WHERE 
			Cast(dbo.fnCalcDistanceKM(Latitude,@Latitude,Longitude,@Longitude) as numeric(36,3)) < @RADIUS 
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate AND RegistrationNumber = @Device
	END
	IF @Filter = 'ParkingReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,MIN(trackTimeStamp) AS StartTime,
			Lg.IMEI As TrackerId,
			MAX(trackTimeStamp) AS EndTime, ParkingTime= NULl,Address,Longitude,Latitude
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device AND @Parktime = @ParkingTime
			GROUP BY RegistrationNumber,Address, Longitude,Latitude,Lg.IMEI
	
	END
	IF @Filter = 'LimitParkReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,MIN(trackTimeStamp) AS StartTime,
			Lg.IMEI As TrackerId,
			MAX(trackTimeStamp) AS EndTime, ParkingTime= NULl,Address,
			FenceName = '',FenceType = '',Longitude,Latitude
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device AND @Parktime = @ParkingTime
			GROUP BY RegistrationNumber,Address, Longitude,Latitude,Lg.IMEI
	
	END
	IF @Filter = 'DrivingReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,MIN(trackTimeStamp) AS StartTime,
			Lg.IMEI As TrackerId,
			MAX(trackTimeStamp) AS EndTime, DrivingTime= '', ParkingTime= NULl,
			MIN(Address) AS StartAddress,MAX(Address) AS EndAddress, LocateNumber = '',
			sixtytoninetyNumber = '', ninetytoonethirtyNumber = '',
			Overonethirty = '', StartMileage = '', maxSpeed = '',
			avgSpeed = '',EndMileage = '', RunningMileage= '',
			MIN(Longitude) AS StartLongitude, MIN(Latitude) AS StartLatitude,
			MAX(Longitude) AS EndLongitude,MAX(Latitude) AS EndLatitude
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device AND @Parktime = @ParkingTime
			AND @Drivetime = @DrivingTime
			GROUP BY RegistrationNumber,Lg.IMEI
	
	END
	IF @Filter = 'WorkingHoursReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,DrivingTime= '', ParkingTime= NULl,
			Lg.IMEI As TrackerId,
			TotalMileage = '', WorkDrivingTime = '', MileageTraveled = '',
			NonworkDrivingTime = '', NonworkDrivingMileage = '',
			MIN(trackTimeStamp) AS StartTime,
			MAX(trackTimeStamp) AS EndTime,
			StartWork = '', EndWork = ''
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device AND @StartingWork= @startWork
			AND @EndingWork = @endWork
			AND @Drivetime = @DrivingTime
			GROUP BY RegistrationNumber,Lg.IMEI
	
	END
	IF @Filter = 'DailyStatReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName, CONVERT(date, trackTimeStamp) AS Date,Lg.IMEI As TrackerId, DrivingTime= '', 
			ParkingTime= NULl, LocateNumber = '', InvalidLocation = '',
			AlarmNumber = '', MAX(CurrentTripMileage) AS Mileage, Avg(Speed) AS AverageSpeed,
			EngineOnNumber = '', EngineOnTime = STUFF(CONVERT(VARCHAR(20),MAX(trackTimeStamp)-MIN(trackTimeStamp),114),1,2,DATEDIFF(hh,0,MAX(trackTimeStamp)-MIN(trackTimeStamp))),
			DoorOpenNumber = '',
			DoorOpenTime = STUFF(CONVERT(VARCHAR(20),MAX(trackTimeStamp)-MIN(trackTimeStamp),114),1,2,DATEDIFF(hh,0,MAX(trackTimeStamp)-MIN(trackTimeStamp))), 
			ShockNumber = '', ShockTime = '',
			TotalMileage = '', WorkDrivingTime = '', MileageTraveled = ''
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId   
			WHERE trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device AND InEngineOnOff = 1 AND InDoorOpenClose = 1
			GROUP BY RegistrationNumber,Convert(date,trackTimeStamp),Lg.IMEI
	
	END
   
END