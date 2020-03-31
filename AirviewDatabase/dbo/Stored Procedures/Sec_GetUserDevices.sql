-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- Sec_GetUserDevices 'byUserId','12'
CREATE PROCEDURE [dbo].[Sec_GetUserDevices]
	@Filter NVARCHAR(50),
	@Value NVARCHAR(50)=null
AS
BEGIN
-- [dbo].[Sec_GetUserDevices] 'All'
	IF @Filter = 'All'
	BEGIN
	    SELECT sud.*,su.FirstName + ' ' + su.LastName 'UserFullName'
	    FROM   Sec_UserDevices  sud
		INNER JOIN Sec_Users su ON  sud.UserId = su.UserId
	    WHERE su.IsActive=1 
		order by su.FirstName 
	END
-- [dbo].[Sec_GetUserDevices] 'byUserId','10037'
	ELSE IF @Filter = 'byUserId'
	BEGIN
	 --   SELECT ue.*,su.FirstName + ' ' + su.LastName 'UserFullName'
	 --   FROM   Sec_UserDevices sud
		--INNER JOIN Sec_Users su ON  sud.UserId = su.UserId
		--inner join AD_UserEquipments ue on ue.UEId=sud.UEId
	 --   WHERE  sud.UserId = @Value

	     SELECT sud.UEId 'UEId', sud.UserId, sud.IMEI, sud.MAC, sud.Manufacturer,
	            sud.Model, sud.isActive, sud.UEId,su.FirstName + ' ' + su.LastName 'UserFullName'
	    FROM   Sec_UserDevices sud
		INNER JOIN Sec_Users su ON  sud.UserId = su.UserId
		--inner join AD_UserEquipments ue on ue.UEId=sud.UEId
	    WHERE  sud.UserId = @Value AND sud.IsActive=1
	END

	IF @Filter = 'byIMEI'
	BEGIN
	    SELECT *
	    FROM Sec_UserDevices
	    WHERE  IMEI = @Value AND isActive=1
	END


END