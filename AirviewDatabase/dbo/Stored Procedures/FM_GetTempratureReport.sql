-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FM_GetTempratureReport] 
	-- Add the parameters for the stored procedure here
@Filter varchar(50) = null,
@startDate DateTime  = null,
@endDate DateTime  = null,
@Device varchar(50) = null,
@startTemp int = null,
@endTemp int = null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @Filter = 'TempratureStatChart'
	BEGIN
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,
			Temperature 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId WHERE 
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
			AND Temperature > @startTemp AND Temperature < @endTemp
			GROUP BY RegistrationNumber,trackTimeStamp,Temperature
	END
	IF @Filter = 'TempratureStatReport'
	BEGIN
			SELECT RegistrationNumber AS AssetName,trackTimeStamp AS Time,
			Temperature,Lg.IMEI AS TrackerId 
			FROM FM_VehicleTrackingLog AS Lg 
			inner join FM_Vehicle x 
			ON x.VehicleId = Lg.VehicleId
			WHERE 
			trackTimeStamp >=@startDate AND trackTimeStamp <=@endDate 
			AND RegistrationNumber = @Device
			AND Temperature > @startTemp AND Temperature < @endTemp
	END
END