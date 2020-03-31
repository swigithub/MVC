-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_GetGeoFenceReport]
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
	IF @Filter = 'MovingInAlarmReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,
			trackTimeStamp AS Time, FenceName = '',
			FenceAlarm = '',Lg.IMEI AS TrackerId
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
	IF @Filter = 'MovingOutAlarmReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,
			trackTimeStamp AS Time, FenceName = '',
			FenceAlarm = '',Lg.IMEI AS TrackerId
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
	IF @Filter = 'InOutFenceAlarmReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,
			trackTimeStamp AS Time, FenceName = '',
			FenceAlarm = '',Lg.IMEI AS TrackerId
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
	IF @Filter = 'InOutStationDetailedReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,
			trackTimeStamp AS Time, FenceName = '',
			FenceAlarm = '',Lg.IMEI AS TrackerId
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
	IF @Filter = 'InOutStationStatsReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,
			trackTimeStamp AS Time, FenceName = '',
			FenceAlarm = '',Lg.IMEI AS TrackerId
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
END