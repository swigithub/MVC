-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_GetMileageReport]
	-- Add the parameters for the stored procedure here
@Filter varchar(50) = null,
@startDate DateTime  = null,
@endDate DateTime  = null,
@Device varchar(50) = null,
@FuelConsumption decimal(18,0) = null,
@FuelPrice decimal(18,0) = null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		IF @Filter  = 'TotalMileageStatChart'
		BEGIN
   			SELECT RegistrationNumber AS AssetName,
			Convert(date,trackTimeStamp) AS Time, MAX(Odometer) AS Mileage
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId WHERE trackTimeStamp >=@startDate 
			AND trackTimeStamp <=@endDate AND RegistrationNumber = @Device 
			GROUP BY RegistrationNumber,Convert(date,trackTimeStamp)
		END
		IF @Filter  = 'MileageVsFuelCost'
		BEGIN
   			SELECT RegistrationNumber AS AssetName,Lg.IMEI as TrackerId,
			MAX(CurrentTripMileage) AS CurrentTripMileage,
			Max(CurrentTripMileage)/(100/@FuelConsumption) AS FuelConsumption,
			(Max(CurrentTripMileage)/(100/@FuelConsumption)) * @FuelPrice AS FuelPrice,
			MIN(trackTimeStamp) AS StartTime,
			MAX(trackTimeStamp) AS EndTime
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate AND RegistrationNumber = @Device 
			GROUP BY RegistrationNumber,Lg.IMEI
		END
END