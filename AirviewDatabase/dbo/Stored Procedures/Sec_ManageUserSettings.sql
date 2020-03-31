-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Sec_ManageUserSettings]
@Filter NVARCHAR(50)
,@UserId NUMERIC(18,0)=NULL
,@TypeId NUMERIC(18,0)=NULL
,@TypeValue NUMERIC(18,0)=NULL
,@Value1 NVARCHAR(50)=NULL
AS
BEGIN
	IF @Filter='Set_IsRequested'
	BEGIN
		DECLARE @UEId AS NUMERIC=0
		(SELECT TOP 1 @UEId=UEId FROM AD_UserEquipments WHERE SerialNo=@Value1)
		
		IF	@UEId>0
		BEGIN
			PRINT @UEId
			UPDATE Sec_UserSettings
			SET IsRequested = 1,IsRequestApproved = 0,IsDownloaded = 0,EmailPIN = NULL  
				,MobilePIN = NULL,PinGenerateDate = NULL,UEId = @UEId
			WHERE UserId=@UserId AND TypeValue=@TypeValue	
		END
		ELSE 
			BEGIN
		     		RAISERROR('Device not found.',16,1)
		     END
			
	END
	
   ELSE 	IF @Filter='Set_IsRequestApproved'
	BEGIN
		DECLARE @EmailPIN AS NVARCHAR(50)=CAST(RAND() * 10000 AS INT)
		DECLARE @MobilePIN AS NVARCHAR(50)=CAST(RAND() * 10000 AS INT)
		
		UPDATE Sec_UserSettings
		SET IsRequestApproved = 1,IsDownloaded = 0, EmailPIN = @EmailPIN, MobilePIN = @MobilePIN, PinGenerateDate = GETDATE()
		WHERE UserId=@UserId --AND TypeValue=@TypeValue
		
	END
	
	 ELSE 	IF @Filter='Set_IsDownloaded'
	BEGIN
		UPDATE Sec_UserSettings
		SET IsDownloaded =0
		WHERE UserId=@UserId AND TypeValue=@TypeValue
		
	END
	
	
	 ELSE 	IF @Filter='Insert'
	 BEGIN
	 	DECLARE @PackageId AS NUMERIC(18,0)=0
	 	
	 	SELECT @PackageId=ad.PDefinationId FROM AD_Definations AS ad WHERE ad.DefinationId=@TypeValue AND ad.IsActive=1
		INSERT INTO [dbo].[Sec_UserSettings]
           ([UserId],[TypeId],[TypeValue],[EmailPIN],[MobilePIN],[PinGenerateDate],[IsRequested],[RequestDate],[IsRequestApproved],[ApprovalDate],[IsDownloaded],[DownloadDate],[UEId])
		VALUES
           (@UserId,@PackageId,@TypeValue,123, 123,GETDATE(),1,GETDATE(),1,GETDATE(),0,NULL,@Value1)
		
	END


END