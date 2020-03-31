-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_GetFuelReport]
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

	IF @Filter = 'FuelStatReportPer100kmPerLitre'
	BEGIN
		Select Convert(date,trackTimeStamp) AS Time,Lg.IMEI AS TrackerId, 
		RegistrationNumber AS AssetName,
		MAX(CurrentTripMileage) AS Mileage, 
		FuelStatReportPer100kmLitre = (MAX(FuelLiter)/MAX(CurrentTripMileage)) * 100 
		FROM FM_VehicleTrackingLog AS Lg 
		inner join FM_Vehicle x 
		ON x.VehicleId = Lg.VehicleId
		WHERE RegistrationNumber = @Device AND trackTimeStamp >= @startDate AND trackTimeStamp <= @endDate
		GROUP BY Convert(date,trackTimeStamp),RegistrationNumber,Lg.IMEI
	
	END

	IF @Filter = 'FuelReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,
			FuelPercent,Lg.IMEI AS TrackerId, 
			FuelLiter,Address,Longitude,Latitude
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate AND RegistrationNumber = @Device
	
	END
	IF @Filter = 'FuelConsumptionVsMileageReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,MIN(Odometer) AS RunningMileage,Lg.IMEI AS TrackerId,
			MAX(FuelPercent) AS UseFuelPercentage, MAX(TotalFuelConsumed)-MIN(TotalFuelConsumed) AS UseFuelLitre,
			MIN(trackTimeStamp) AS StartTime, 
			MIN(Address) AS StartAddress, MIN(FuelPercent) AS StartFuelPercentage,
			MIN(TotalFuelConsumed) AS StartFuelLitre, MAX(Odometer)-MIN(Odometer) AS StartMileage, 
			MAX(trackTimeStamp) AS EndTime,MAX(Address) AS EndAddress,
			MAX(FuelPercent) AS EndFuelPercentage, MAX(TotalFuelConsumed) AS EndFuelLitre,
			MAX(Odometer) AS EndMileage, MIN(Longitude) AS StartLongitude,
			MIN(Latitude) AS StartLatitude, MAX(Longitude) AS EndLongitude,
			MAX(Latitude) AS EndLatitude
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
			GROUP BY RegistrationNumber,Lg.IMEI
	
	
	END
END