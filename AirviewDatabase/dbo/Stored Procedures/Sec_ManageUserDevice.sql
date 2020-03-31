-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Sec_ManageUserDevice
	@Filter NVARCHAR(50),
	@DeviceId NUMERIC(18, 0) = NULL,
	@UserId NUMERIC(18, 0) = NULL,
	@IMEI NVARCHAR(50) = NULL,
	@MAC NVARCHAR(50) = NULL,
	@Manufacturer NVARCHAR(50) = NULL,
	@Model NVARCHAR(50) = NULL,
	@IsActive bit = NULL,
	@Password NVARCHAR(50) = NULL,
	@TranferToId NVARCHAR(50) = NULL
   ,@UEId NUMERIC(18, 0) = NULL
	
AS
BEGIN
	IF @Filter = 'Insert'
		BEGIN
		IF Exists(select * from Sec_UserDevices where UserId = @UserId and UEId = @UEId and DeviceId = @DeviceId)
		Begin
		UPDATE Sec_UserDevices SET [IMEI]=@IMEI,[MAC]=@MAC,Manufacturer = @Manufacturer,Model = @Model,isActive =1 WHERE DeviceId=@DeviceId and UserId = @UserId
		End
		ELSE
		BEGIN
			INSERT INTO [dbo].[Sec_UserDevices]([UserId],[IMEI],[MAC],[Manufacturer],[Model],IsActive,UEId)
			VALUES(@UserId,@IMEI,@MAC,@Manufacturer,@Model,1,@UEId)
			END
		END
	ELSE IF @Filter = 'Update'
	BEGIN
		UPDATE Sec_UserDevices SET [IMEI]=@IMEI,[MAC]=@MAC,Manufacturer = @Manufacturer,Model = @Model WHERE DeviceId=@DeviceId		
	END
	ELSE IF @Filter = 'UpdateStatus'
	BEGIN
		UPDATE Sec_UserDevices SET IsActive=@IsActive WHERE DeviceId=@DeviceId		
	END
	ELSE IF @Filter = 'Delete'
	BEGIN
		UPDATE Sec_UserDevices
		SET isActive = CAST(0 AS BIT)
		WHERE  DeviceId = @DeviceId
	END
--  [dbo].[Sec_ManageUserDevice] 'TransferDevice',0,'12','356095064009947','','','','kJmj2aYaCnA=',10034
    ELSE IF @Filter = 'TransferDevice'
	BEGIN
		IF	EXISTS(select DISTINCT IMEI from Sec_UserDevices WHERE UserId=@UserId AND IMEI=@IMEI)
			BEGIN
			
				IF	@Password=(select [Password] from Sec_Users where UserId=@UserId)
					BEGIN					
						--De-activate user device for TransferFrom User
						UPDATE Sec_UserDevices
						SET isActive=0
						WHERE UserId=@UserId AND IMEI=@IMEI AND isActive=1
				    
						--Check whether TransferTo User already have this user device
						IF EXISTS(SELECT IMEI FROM Sec_UserDevices AS sud WHERE sud.UserId=@TranferToId AND sud.IMEI=@IMEI AND sud.isActive=0)
						BEGIN
				    		DECLARE @userDeviceId AS INT=(SELECT sud.DeviceId FROM Sec_UserDevices AS sud WHERE sud.UserId=@TranferToId AND sud.IMEI=@IMEI AND sud.isActive=0)
				    	
				    		UPDATE Sec_UserDevices
				    		SET isActive=1
				    		WHERE DeviceId=@userDeviceId;
						END
						ELSE
						BEGIN
				    		INSERT INTO Sec_UserDevices(UserId,IMEI,MAC,Manufacturer,Model,isActive)
				    		SELECT @TranferToId,x.IMEI,x.MAC,x.Manufacturer,x.Model,CAST(1 AS BIT)
				    		FROM
				    		(
				    			SELECT TOP 1 sud.IMEI, sud.MAC,sud.Manufacturer,sud.Model
				    			FROM Sec_UserDevices AS sud
				    			WHERE sud.IMEI=@IMEI
				    		) x
						END
					END
				ELSE 
					BEGIN
						RAISERROR('Wrong Password',16,1)
						END
			END
		ELSE
			BEGIN
				RAISERROR('Device not found',16,1)
			END
	END
END