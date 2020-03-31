-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AD_ManageUEMovement]
 @Filter nvarchar(50)
,@UEMovementId numeric(18, 0)=null
,@UEId numeric(18, 0)=null
,@UserId numeric(18, 0)=null
,@UEStatusId numeric(18, 0)=NULL
,@FromUserId numeric(18, 0)=NULL
AS
BEGIN

	declare @UserDeviceId numeric(18,0)
	declare @TempUserId numeric(18,0)
	declare @IssueId numeric(18,0)= (select top 1 DefinationId from AD_Definations where DefinationName='Issue'  AND KeyCode='UE_STATUS')
	declare @ReturnId numeric(18,0)=(select top 1 DefinationId from AD_Definations where DefinationName='Return' AND KeyCode='UE_STATUS')

	
	SELECT TOP 1 @UserDeviceId=DeviceId from Sec_UserDevices WHERE UEID = @UEId AND isActive=1
	
	DECLARE @SerialNo AS NVARCHAR(50)=''
	DECLARE @MAC AS NVARCHAR(50)=''
	DECLARE @Manufacturer AS NVARCHAR(50)=''
	DECLARE @Model AS NVARCHAR(50)=''
			
	SELECT @SerialNo=aue.SerialNo,@Manufacturer=aue.Manufacturer, @Model=aue.Model, @MAC=aue.MAC
	FROM AD_UserEquipments AS aue WHERE aue.UEId=@UEId
	
	IF	@Filter='Insert'
	BEGIN
		INSERT INTO AD_UEMovement (UEId,UserId,UEStatusId,[Date])
		 values(@UEId,@UserId,@UEStatusId,GETDATE())
	END
	ELSE IF @Filter='Issue'
	BEGIN
		INSERT INTO AD_UEMovement (UEId,UserId,UEStatusId,[Date])
		values(@UEId,@UserId,@UEStatusId,GETDATE())
		
		EXEC AD_ManageUserEquipments
			@Filter = 'Set_UEStatusId',
			@UEId = @UEId,
			@UETypeId = null,
			@Manufacturer = null,
			@Model = null,
			@SerialNo = null,
			@MAC = null,
			@UENumber = null,
			@IsActive = null,
			@UEStatusId = @IssueId,
			@Token = NULL		
			
			
			EXEC Sec_ManageUserDevice
			@Filter = 'Insert',
			@DeviceId = @UserDeviceId,
			@UserId = @UserId,
			@IMEI = @SerialNo,
			@MAC = @MAC,
			@Manufacturer = @Manufacturer,
			@Model = @Model,
			@IsActive = 1,
			@Password = null,
			@TranferToId = @UEStatusId,
			@UEId = @UEId
	end
	ELSE IF @Filter='Return'
	BEGIN
		EXEC AD_ManageUserEquipments
			@Filter = 'Set_UEStatusId',
			@UEId = @UEId,
			@UETypeId = null,
			@Manufacturer = null,
			@Model = null,
			@SerialNo = null,
			@MAC = null,
			@UENumber = null,
			@IsActive = null,
			@UEStatusId = @ReturnId,
			@Token = null
		
		INSERT INTO AD_UEMovement (UEId,UserId,UEStatusId,[Date])
		values(@UEID,@UserId,@UEStatusId,GETDATE())
		
		EXEC Sec_ManageUserDevice
			@Filter = 'Delete',
			@DeviceId = @UserDeviceId,
			@UserId = 0,
			@IMEI = null,
			@MAC = null,
			@Manufacturer = null,
			@Model = null,
			@IsActive = null,
			@Password = null,
			@TranferToId = null,
			@UEId = @UEId
	END
	ELSE IF @Filter='Transfer'
	BEGIN		
		--SELECT TOP 1 @UserDeviceId=ISNULL(DeviceId,0),@TempUserId=UserId from Sec_UserDevices where UEId = @UEId AND isActive=1

		IF @UserDeviceId > 0
		BEGIN
			INSERT INTO AD_UEMovement (UEId,UserId,UEStatusId,[Date])
		    values(@UEId,@FromUserId,@ReturnId,GETDATE())
		    
		    INSERT INTO AD_UEMovement (UEId,UserId,UEStatusId,[Date])
		    values(@UEId,@UserId,@IssueId,GETDATE())
		    
			EXEC Sec_ManageUserDevice
			@Filter = 'Delete',
			@DeviceId = @UserDeviceId,
			@UserId = 0,
			@IMEI = null,
			@MAC = null,
			@Manufacturer = null,
			@Model = null,
			@IsActive = null,
			@Password = null,
			@TranferToId = null,
			@UEId = @UEId
			
			EXEC Sec_ManageUserDevice
			@Filter = 'Insert',
			@DeviceId = @UserDeviceId,
			@UserId = @UserId,
			@IMEI = @SerialNo,
			@MAC = @MAC,
			@Manufacturer = @Manufacturer,
			@Model = @Model,
			@IsActive = 1,
			@Password = null,
			@TranferToId = @UEStatusId,
			@UEId = @UEId
		END
		ELSE 
		BEGIN
			INSERT INTO AD_UEMovement (UEId,UserId,UEStatusId,[Date])
		    values(@UEId,@UserId,@UEStatusId,GETDATE())
			
			EXEC Sec_ManageUserDevice
			@Filter = 'Insert',
			@DeviceId = @UserDeviceId,
			@UserId = @UserId,
			@IMEI = @SerialNo,
			@MAC = @MAC,
			@Manufacturer = @Manufacturer,
			@Model = @Model,
			@IsActive = 1,
			@Password = null,
			@TranferToId = @UEStatusId,
			@UEId = @UEId
		END
	END
END