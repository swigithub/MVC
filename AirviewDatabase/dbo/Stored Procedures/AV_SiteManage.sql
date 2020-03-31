-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_SiteManage] --'UpdateStatus',15,92
	@Filter nvarchar(50),
	@Value1 nvarchar(50),
	@Value2 nvarchar(50) = NULL,
	@Value3 nvarchar(50) = NULL,
	@Value4 nvarchar(50) = NULL,	
	@Value5 nvarchar(50) = NULL,
	@UserId NUMERIC(18,0)=0,
	@DeviceScheduleId NUMERIC(18,0)=0
AS
BEGIN
	--exec AV_SiteManage @Filter=N'CheckStatus',@Value2=N'15',@Value3=N'3166',@Value4=N'233566',@Value1=N'699252'

	--@value1: Siteid
	--@VAlue2: 
	IF @Filter='CheckStatus'
	BEGIN
		--SELECT 1;
		--DECLARE @nlStatusID1 AS INT=0
		
		
		--SELECT @nlStatusID1=anls.[Status],@UserId=anls.TesterId
		--FROM AV_NetLayerStatus AS anls
		--WHERE SiteId=@Value1 AND NetworkModeId=@Value2 AND BandId=@Value3 AND CarrierId=@Value4;
	    
	 --   --update AV_Sites set Status=@woStatusID, DriveCompletedOn=GETDATE() where SiteId=@Value1
	 --   --DECLARE @woStatus AS INT =(SELECT [Status] FROM AV_NetLayerStatus WHERE SiteId=@Value1 AND NetworkModeId=@Value3 AND BandId=@Value4 AND CarrierId=@Value5)
	    
	 --   IF @nlStatusID1=450
	 --   BEGIN
		--	UPDATE AV_NetLayerStatus
		--	SET [Status] = 92,	
		--	DriveCompletedOn = GETDATE()	    	
		--	WHERE SiteId=@Value1 AND NetworkModeId=@Value2 AND BandId=@Value3 AND CarrierId=@Value4;
			
		--	INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource)
		--	VALUES(@Value1,@Value2,@Value3,@Value4,NULL,92,@UserId,HOST_NAME(),GETDATE(),'AV_SiteManage');
	 --   END	  
		SELECT  anls.Status
		FROM AV_NetLayerStatus AS anls
		WHERE SiteId=@Value1 AND NetworkModeId=@Value2 AND BandId=@Value3 AND CarrierId=@Value4 AND Status = 450;
	END 
	ELSE if	@Filter='UpdateDownload' 
	begin
	-- @Value1 for TesterId  or UserId
	-- @Value2 for IMEI
	   IF EXISTS(select * from Sec_UserDevices where IMEI=@Value2 and UserId=@Value1) BEGIN
	   	--IF (@Value1 NOT IN(10053,10124))
	   	--BEGIN
	   		DECLARE @userDeviceId AS INT=(SELECT DISTINCT sud.DeviceId 
	   									FROM Sec_UserDevices AS sud
	   									INNER JOIN Sec_Users AS su ON su.UserId=sud.UserId
	   		                            WHERE sud.IMEI=@Value2 AND sud.isActive=1 AND su.IsActive=1)
	      update AV_WoDevices set IsDownlaoded=1, DownloadDate=GETDATE()
	      where UserId=@Value1 AND UserDeviceId=@userDeviceId and ISNULL(IsDownlaoded,0)=0
		  SET @DeviceScheduleId=(SELECT y.DeviceScheduleId FROM AV_ClusterSchedule y where UserId=@Value1 AND UserDeviceId=@userDeviceId)
		   update AV_ClusterSchedule  set IsDownloaded=1, DownloadDate=GETDATE()
		   where DeviceScheduleId=@DeviceScheduleId
	      --AND UserId IN(SELECT su.UserId FROM Sec_Users AS su WHERE su.UserId=@Value1)
	     --END
	   end
	end


	else if	@Filter='UpdateStatus' 
	BEGIN
		SELECT @Value1 +' '+@Value2+' '+@value3
	-- @Value1 for SiteId
	-- @Value2 for StatusId
	-- @Value3 for NetModeId
	-- @Value4 for BandID
	-- @Value5 for CarrierId
	DECLARE @woStatusID int=0

	SET @woStatusID=(SELECT x.DefinationId FROM AD_Definations x WHERE x.KeyCode=@Value2)

	if @Value2='COMPLETED'
	BEGIN
		
	    --update AV_Sites set Status=@woStatusID, CompletedOn=GETDATE() where SiteId=@Value1   
	    
	    
	   IF  (@Value2 IS NOT NULL) AND (@Value3 IS NULL) AND (@Value4 IS NULL) AND (@Value5 IS NULL)
	   BEGIN
	   	
	  
	   	--WO Level Approval	   	
	   	UPDATE AV_NetLayerStatus
	    SET [Status] = 89,	
	    CompletedOn = GETDATE(),
	    AcceptedOn = GETDATE(),
	    AcceptedById =@UserId	    
	    WHERE SiteId=@Value1 AND [Status]=451
	    
	     update AV_Sites
	     set ApprovedComments=@Value3
	     where SiteId=@Value1
	     
	     INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate)
		  VALUES(@Value1,0,0,0,NULL,89,@UserId,HOST_NAME(),GETDATE());
	   END
	   ELSE IF  (@Value2 IS NOT NULL) AND (@Value3 IS NOT NULL) AND (@Value4 IS NOT NULL) AND (@Value5 IS NOT NULL)
	   BEGIN
	   	 
	    UPDATE AV_NetLayerStatus
	    SET [Status] = 89,	
	    CompletedOn = GETDATE(),
	    AcceptedOn = GETDATE(),
	    AcceptedById =@UserId	
	    WHERE SiteId=@Value1 AND NetworkModeId=@Value3 AND BandId=@Value4 AND CarrierId=@Value5
	    
	    INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate)
		  VALUES(@Value1,@Value3,@Value4,@Value5,NULL,89,@UserId,HOST_NAME(),GETDATE());
	  END  	
	  
	  
	END
	ELSE if @Value2 IN('DRIVE_COMPLETED')
	BEGIN
		SELECT 0;
		DECLARE @nlStatusID AS INT=0
		
		SELECT @nlStatusID=anls.[Status]
		FROM AV_NetLayerStatus AS anls
		WHERE SiteId=@Value1 AND NetworkModeId=@Value3 AND BandId=@Value4 AND CarrierId=@Value5;
	    
	    --update AV_Sites set Status=@woStatusID, DriveCompletedOn=GETDATE() where SiteId=@Value1
	    --DECLARE @woStatus AS INT =(SELECT [Status] FROM AV_NetLayerStatus WHERE SiteId=@Value1 AND NetworkModeId=@Value3 AND BandId=@Value4 AND CarrierId=@Value5)
	    
	    IF @nlStatusID=450
	    BEGIN
			UPDATE AV_NetLayerStatus
			SET [Status] = 92,	
			DriveCompletedOn = GETDATE()	    	
			WHERE SiteId=@Value1 AND NetworkModeId=@Value3 AND BandId=@Value4 AND CarrierId=@Value5
			
			INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource)
			VALUES(@Value1,@Value3,@Value4,@Value5,NULL,92,@UserId,HOST_NAME(),GETDATE(),'AV_SiteManage');
	    END	    
	END
	
	end
	
 --	[dbo].[AV_SiteManage] 'ChangeStatus',10096,0
	 ELSE IF	@Filter='ChangeStatus'
	 BEGIN
	 	--PRINT 'ChangeStatus'
	 	UPDATE AV_Sites
	 	SET IsActive = @Value2,
	 	InactiveById=@UserId
	 	WHERE SiteId=@Value1
	 END
	 
	 --	[dbo].[AV_SiteManage] 'WoHold',10096,0
	 ELSE IF	@Filter='WoHold'
	 BEGIN
	 	UPDATE AV_Sites
	 	SET IsActive = 0,
	 	STATUS=(SELECT x.DefinationId FROM AD_Definations x WHERE x.KeyCode='HOLD'),
	 	HoldById=@UserId
	 	WHERE SiteId=@Value1
	 	
	 	INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource)
	    VALUES(@Value1,@Value3,@Value4,@Value5,NULL,(SELECT x.DefinationId FROM AD_Definations x WHERE x.KeyCode='HOLD'),@UserId,HOST_NAME(),GETDATE(),'AV_SiteManage');
	 END
END