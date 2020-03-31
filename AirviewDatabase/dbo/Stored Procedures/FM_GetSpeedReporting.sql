-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_GetSpeedReporting] 
	
@Filter varchar(50) = null,
@startDate DateTime  = null,
@endDate DateTime  = null,
@Device varchar(50) = null,
@Speed int  = null,
@DriveOverHour int = null,
@NoParkRest int = null,
@IdleSpeedMoreThan int = null
	
AS


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @Filter  = 'SpeedStatChart'
	BEGIN
		
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,Speed,Lg.IMEI As TrackerId 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE trackTimeStamp >=@startDate 
			AND trackTimeStamp <= @endDate AND RegistrationNumber = @Device
			Group by RegistrationNumber,trackTimeStamp,Speed,Lg.IMEI
	
	END
	IF @Filter  = 'SpeedStatReport'
	BEGIN
		
			SELECT RegistrationNumber AS AssetName,Lg.IMEI As TrackerId,trackTimeStamp AS Time,Speed 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId 
			WHERE trackTimeStamp >=@startDate AND trackTimeStamp <= @endDate AND Speed > @Speed AND RegistrationNumber = @Device
	
	END
	IF @Filter  = 'FatigueDrivingReport'
	BEGIN
		
			SELECT RegistrationNumber AS AssetName,Lg.IMEI As TrackerId,
			FatigueStartTime = CONVERT(varchar(7), 
			DATEADD(minute, 
			DATEDIFF(minute, format(DATEADD(MINUTE,AlarmCurrentVal,CAST(CAST(0 AS FLOAT) AS DATETIME)),'HH:mm:ss'), 
			trackTimeStamp), 0), 114), Convert(time(0),trackTimeStamp) AS FatigueEndTime,
			Address, Longitude, Latitude
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE TA.AlarmCode = 'FatgDrv' AND AD.Status = 1
			AND trackTimeStamp >=@startDate AND trackTimeStamp <= @endDate 
			AND RegistrationNumber = @Device
	
	END
	IF @Filter  = 'EngineIdleReport'
	BEGIN
		
			SELECT RegistrationNumber AS AssetName,Lg.IMEI As TrackerId,
			IdleStartTime = CONVERT(varchar(7), 
			DATEADD(minute, 
			DATEDIFF(minute, format(DATEADD(MINUTE,AlarmCurrentVal,CAST(CAST(0 AS FLOAT) AS DATETIME)),'HH:mm:ss'), 
			trackTimeStamp), 0), 114), Convert(time(0),trackTimeStamp) AS IdleEndTime,
			Address, Longitude, Latitude
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_TrackingAlarmDetails Ad
			ON ad.TrackingID = Lg.TrackingId
			inner join  FM_TrackingAlarm TA
			on TA.AlarmCode = AD.AlarmCode
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE TA.AlarmCode = 'IdlEngn' AND AD.Status = 1
			AND trackTimeStamp >=@startDate AND trackTimeStamp <= @endDate 
			AND RegistrationNumber = @Device
	
	END
   
END