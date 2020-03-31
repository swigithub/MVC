-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_GetAssetStatus]

@Filter varchar(50) = null,
@startDate DateTime  = null,
@endDate DateTime  = null,
@Device varchar(50) = null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @Filter = 'EngineON/OFFReport'
	BEGIN
	
		-- Insert statements for procedure here
		WITH operating AS
		(
			SELECT
			RegistrationNumber AS AssetName,CurrentTripMileage AS Mileage, trackTimeStamp,Lg.IMEI,InEngineOnOff As EngineStatus,
			ROW_NUMBER() OVER (PARTITION BY RegistrationNumber ORDER BY [trackTimeStamp])  RowNum,
			ROW_NUMBER() OVER (PARTITION BY RegistrationNumber ORDER BY [trackTimeStamp]) -
			ROW_NUMBER() OVER (PARTITION BY RegistrationNumber, [InEngineOnOff] ORDER BY [trackTimeStamp]) AS [Group]
				
    
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE RegistrationNumber = @Device
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate
		)
			SELECT 
			state1.AssetName,state1.EngineStatus,state1.IMEI AS TrackerId,
			ContinousTime = STUFF(CONVERT(VARCHAR(20),MAX(state1.trackTimeStamp)-MIN(state1.trackTimeStamp),114),1,2,DATEDIFF(hh,0,MAX(state1.trackTimeStamp)-MIN(state1.trackTimeStamp))),
			MIN(state1.[trackTimeStamp]) [StartTime],
			MAX(state2.[trackTimeStamp]) [EndTime],
			MAX(state1.Mileage) AS Mileage
			FROM 
			operating state1
			LEFT JOIN
			operating state2 
			ON 
			state1.RowNum = state2.RowNum - 1
			WHERE
			state2.[trackTimeStamp] IS NOT NULL 
			GROUP BY  
			state1.AssetName, state1.EngineStatus,state1.IMEI, state1.[Group]
			ORDER BY 
			MIN(state1.[trackTimeStamp])
	END

	IF @Filter = 'DoorOpen/CloseReport'
	BEGIN
		-- Insert statements for procedure here
		WITH operating AS
		(
			SELECT
			RegistrationNumber AS AssetName,CurrentTripMileage AS Mileage,Lg.IMEI, trackTimeStamp,InDoorOpenClose As DoorStatus,
			ROW_NUMBER() OVER (PARTITION BY RegistrationNumber ORDER BY [trackTimeStamp])  RowNum,
			ROW_NUMBER() OVER (PARTITION BY RegistrationNumber ORDER BY [trackTimeStamp]) -
			ROW_NUMBER() OVER (PARTITION BY RegistrationNumber, [InDoorOpenClose] ORDER BY [trackTimeStamp]) AS [Group]
				
    
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE RegistrationNumber = @Device
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate
		)
			SELECT 
			state1.AssetName,state1.DoorStatus,state1.IMEI AS TrackerId,
			ContinousTime = STUFF(CONVERT(VARCHAR(20),MAX(state1.trackTimeStamp)-MIN(state1.trackTimeStamp),114),1,2,DATEDIFF(hh,0,MAX(state1.trackTimeStamp)-MIN(state1.trackTimeStamp))),
			MIN(state1.[trackTimeStamp]) [StartTime],
			MAX(state2.[trackTimeStamp]) [EndTime],
			MAX(state1.Mileage) AS Mileage
			FROM 
			operating state1
			LEFT JOIN
			operating state2 
			ON 
			state1.RowNum = state2.RowNum - 1
			WHERE
			state2.[trackTimeStamp] IS NOT NULL 
			GROUP BY  
			state1.AssetName, state1.DoorStatus,state1.IMEI, state1.[Group]
			ORDER BY 
			MIN(state1.[trackTimeStamp])
	END

	IF @Filter = 'ShockReport'
	BEGIN
		-- Insert statements for procedure here
		WITH operating AS
		(
			SELECT
			RegistrationNumber AS AssetName,CurrentTripMileage AS Mileage,Lg.IMEI, trackTimeStamp,[Status] As VibrationSensorState,
			ROW_NUMBER() OVER (PARTITION BY RegistrationNumber ORDER BY [trackTimeStamp])  RowNum,
			ROW_NUMBER() OVER (PARTITION BY RegistrationNumber ORDER BY [trackTimeStamp]) -
			ROW_NUMBER() OVER (PARTITION BY RegistrationNumber, [InDoorOpenClose] ORDER BY [trackTimeStamp]) AS [Group]
				
    
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE RegistrationNumber = @Device AND TA.AlarmCode = 'Vib'
			AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate
		)
			SELECT 
			state1.AssetName,state1.VibrationSensorState,state1.IMEI AS TrackerId,
			ContinousTime = STUFF(CONVERT(VARCHAR(20),MAX(state1.trackTimeStamp)-MIN(state1.trackTimeStamp),114),1,2,DATEDIFF(hh,0,MAX(state1.trackTimeStamp)-MIN(state1.trackTimeStamp))),
			MIN(state1.[trackTimeStamp]) [StartTime],
			MAX(state2.[trackTimeStamp]) [EndTime],
			MAX(state1.Mileage) AS Mileage
			FROM 
			operating state1
			LEFT JOIN
			operating state2 
			ON 
			state1.RowNum = state2.RowNum - 1
			WHERE
			state2.[trackTimeStamp] IS NOT NULL 
			GROUP BY  
			state1.AssetName, state1.VibrationSensorState,state1.IMEI, state1.[Group]
			ORDER BY 
			MIN(state1.[trackTimeStamp])
	
	END
	IF @Filter = 'BatteryVoltageHighAlert'
	BEGIN
		-- Insert statements for procedure here
		SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Odometer AS 
		Milage,Lg.IMEI As TrackerId,Direction,Rotation,Latitude,Longitude,GPSSignalStatus,
		InEngineOnOff AS AssetStatus, 
		Address, FuelPercent, FuelLiter,GSMSignal, 
		CurrentTripMileage, Temperature, TA.AlarmCode AS AlarmState,
		ExtendState = '',DeviceState = '' 
		FROM FM_VehicleTrackingLog AS Lg 
		inner join FM_TrackingAlarmDetails Ad
		ON ad.TrackingID = Lg.TrackingId
		inner join  FM_TrackingAlarm TA
		on TA.AlarmCode = AD.AlarmCode
		inner join FM_Vehicle x 
		ON x.VehicleId = Lg.VehicleId  
		WHERE TA.AlarmCode = '' 
		AND trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
		AND RegistrationNumber = @Device
	END

END