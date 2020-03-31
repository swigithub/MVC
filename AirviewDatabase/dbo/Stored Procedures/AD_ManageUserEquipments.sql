-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE AD_ManageUserEquipments
 @Filter nvarchar(50) =null
,@UEId numeric(18, 0) =null
,@UETypeId numeric(18, 0) =null
,@Manufacturer nvarchar(50) =null
,@Model nvarchar(50) =null
,@SerialNo nvarchar(50) =null
,@MAC nvarchar(50) =null
,@UENumber nvarchar(50) =null
,@IsActive nvarchar(50) =null
,@UEStatusId nvarchar(50) =null
,@Token nvarchar(250) =null
,@UERefNo nvarchar(50) =null
,@UEOwnerId numeric(18, 0) =null
AS
BEGIN
	if @Filter='Insert'
	begin				
		INSERT INTO [dbo].[AD_UserEquipments]([UETypeId],[Manufacturer],[Model],[SerialNo],[MAC],[UENumber],[UEStatusId],[IsActive],Token,UERefNo,UEOwnerId)
		VALUES(@UETypeId,@Manufacturer,@Model,@SerialNo,@MAC,@UENumber,@UEStatusId,@IsActive,@Token, @UERefNo, @UEOwnerId)		
	END
	ELSE if @Filter='Update'
	BEGIN
		UPDATE AD_UserEquipments SET UETypeId=@UETypeId,Manufacturer=@Manufacturer,Model=@Model,SerialNo=@SerialNo,MAC=@MAC,UENumber=LTRIM(RTRIM(@UENumber)),
		UEStatusId=NULL,IsActive=1,UERefNo=@UERefNo,UEOwnerId=@UEOwnerId
		WHERE UEId=@UEId
	END

-- [dbo].[AD_ManageUserEquipments] 'Set_DeviceNumber',0,0,'','','030000','',''

	 ELSE IF @Filter = 'Set_DeviceNumber'
		BEGIN
			UPDATE AD_UserEquipments 
			SET UENumber=LTRIM(RTRIM(@UENumber))
			WHERE SerialNo=@SerialNo and isActive=1
		END

		ELSE IF @Filter = 'Set_IsActive'
		BEGIN
			UPDATE AD_UserEquipments 
			SET IsActive=@IsActive
			WHERE SerialNo=@SerialNo
		END

		ELSE IF @Filter = 'Set_UEStatusId'
		BEGIN
			declare @UEStatusCode NVARCHAR(50)= (select top 1 DefinationName from AD_Definations WHERE DefinationId=@UEStatusId AND KeyCode='UE_STATUS')
			
			IF @UEStatusCode='Issue' --|| @UEStatusCode='Transfer'
			BEGIN
				UPDATE AD_UserEquipments 
				SET UEStatusId=@UEStatusId
				WHERE UEId=@UEId
			END
			ELSE IF @UEStatusCode='Return'
			BEGIN
				UPDATE AD_UserEquipments 
				SET UEStatusId=NULL
				WHERE UEId=@UEId
			END
		END

		ELSE IF @Filter = 'Set_Token'
		BEGIN
			UPDATE AD_UserEquipments 
			SET Token=@Token
			WHERE SerialNo=@SerialNo
		END
END