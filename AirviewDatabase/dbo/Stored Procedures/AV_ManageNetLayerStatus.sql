-- =============================================
-- Author:		/*----MoB!----*/
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE AV_ManageNetLayerStatus
 @Filter NVARCHAR(100),
 @UserId NUMERIC(18,0), 
 @NetworkModeId NUMERIC(18,0),
 @SiteId NUMERIC(18,0),
 @BandId NUMERIC(18,0),
 @CarrierId NUMERIC(18,0),  
 @value1 NVARCHAR(max) =NULL,
 @value2 NVARCHAR(max)=NULL,
 @value3 NVARCHAR(max)=NULL

 

AS
BEGIN
	DECLARE  @KeyValue NUMERIC(18,0)
	DECLARE @woStatus AS INT =0

	-- Declare Table Name
	DECLARE @tablename AS nvarchar(50)
	DECLARE @current_date AS datetime
	DECLARE @query_a AS nvarchar(500)
	DECLARE @query_b AS nvarchar(500)
	SET @current_date = GETDATE()
	
	SET @woStatus=ISNULL((SELECT [Status] FROM AV_NetLayerStatus WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId),0)
	
	IF @Filter='Set_PendingWithIssue'
	BEGIN
		set  @KeyValue = (SELECT DefinationId FROM AD_Definations WHERE KeyCode='PENDING_WITH_ISSUE')		
		UPDATE AV_NetLayerStatus
		SET [STATUS]=93,
		StatusReason=@value1,
		PendingIssueDesc=@value2,
		pendingWithIssueOn=GETDATE(), 
		DriveCompletedOn = GETDATE(),		
		SubmittedOn = GETDATE(),
		SubmittedById = @UserId,
		pendingWithIssueById=@UserId
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId
		
		--INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource)
	    --VALUES(@SiteId,@NetworkModeId,@BandId,@CarrierId,NULL,93,@UserId,HOST_NAME(),GETDATE(),'AV_ManageNetLayerStatus');
		SET @tablename = (select CompleteName from ExternalDatabase where TableName = 'Log_WOStatusTracker');
		SET @query_b  = 'INSERT INTO ' + RTRIM(@tablename) + ' (SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource)
		VALUES('+RTRIM(@SiteId)+','+RTRIM(@NetworkModeId)+','+RTRIM(@NetworkModeId)+','+RTRIM(@CarrierId)+',NULL,'+@value2+','+RTRIM(@UserId)+','''+RTRIM(HOST_NAME())+''','''+RTRIM(@current_date)+''',''AV_ManageNetLayerStatus'')';
		EXECUTE sp_executesql @query_b;
	END
	
	ELSE IF @Filter='Set_Observation'
	BEGIN
		
		UPDATE AV_NetLayerStatus
		SET netLayerObservations=@value1
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId
	END
	
	ELSE IF @Filter='Set_Status'
	BEGIN
		SET @KeyValue = (SELECT DefinationId FROM AD_Definations WHERE KeyCode=@value1)	
				
		
		IF @value1='COMPLETED'
		BEGIN
			--IF @woStatus=451
			--BEGIN
				UPDATE AV_NetLayerStatus
				SET STATUS=89,
				CompletedOn = GETDATE(),
				AcceptedOn = GETDATE(),
				AcceptedById = @UserId
				WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId
			--END
		END
		ELSE IF @value1='REPORT_SUBMITTED'
		BEGIN
			DECLARE @curStatus AS INT=ISNULL((SELECT x.[Status] FROM AV_NetLayerStatus AS x WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId AND x.CarrierId=@CarrierId),0)
			
			IF @curStatus=93
			BEGIN
				UPDATE AV_NetLayerStatus
				SET STATUS=451				
				WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId
			END
			ELSE IF @curStatus=92
			BEGIN
				UPDATE AV_NetLayerStatus
				SET STATUS=451,
				SubmittedOn = GETDATE(),
				SubmittedById = @UserId
				WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId
			END
			ELSE
			BEGIN
				RAISERROR('Net Layer can be submitted only in Status: Pending with Issue or Drive Completed.',16,1)
			END
		END
		
		--INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource)
	 --   VALUES(@SiteId,@NetworkModeId,@BandId,@CarrierId,NULL,@KeyValue,@UserId,HOST_NAME(),GETDATE(),'AV_ManageNetLayerStatus');	
		SET @tablename = (select CompleteName from ExternalDatabase where TableName = 'Log_WOStatusTracker');
		SET @query_b  = 'INSERT INTO ' + RTRIM(@tablename) + ' (SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource)
		VALUES('+RTRIM(@SiteId)+','+RTRIM(@NetworkModeId)+','+RTRIM(@NetworkModeId)+','+RTRIM(@CarrierId)+',NULL,'+@value2+','+RTRIM(@UserId)+','''+RTRIM(HOST_NAME())+''','''+RTRIM(@current_date)+''',''AV_ManageNetLayerStatus'')';
		EXECUTE sp_executesql @query_b;

	END
	ELSE IF @Filter='Set_IsActive'
	BEGIN
	-- @value1=LayerStatusId , @value2=IsActive
	    UPDATE AV_NetLayerStatus
		SET IsActive=@value2
		WHERE LayerStatusId=@value1
	END
	ELSE IF @Filter='Status_Movement'
	BEGIN
	-- @value1=LayerStatusId , @value2=StatusId,@value3=Remarks
		DECLARE @Status_Key_Code AS NVARCHAR(50)=''
		DECLARE @ScopeId AS NUMERIC		
		DECLARE @curStatus1 AS NUMERIC
		
		SELECT @Status_Key_Code=ad.KeyCode FROM AD_Definations AS ad WHERE ad.DefinationTypeId=17 AND ad.IsActive=1 AND ad.DefinationId=@value2;
		
		SELECT @SiteId=anls.SiteId, @NetworkModeId=anls.NetworkModeId, @BandId=anls.BandId, @CarrierId=anls.CarrierId, @ScopeId=anls.ScopeId, @curStatus1=ISNULL(anls.[Status],0)
		  FROM AV_NetLayerStatus AS anls
		WHERE anls.LayerStatusId=@value1;
		
		IF @Status_Key_Code='SCHEDULED'
		BEGIN
			UPDATE AV_NetLayerStatus
			SET ScheduledOn = CAST(GETDATE() AS DATE), AssignedOn = GETDATE(), [Status] = @value2
			WHERE LayerStatusId=@value1;
		END
		ELSE IF @Status_Key_Code='IN_PROGRESS'
		BEGIN
			UPDATE AV_NetLayerStatus
			SET [Status] = @value2
			WHERE LayerStatusId=@value1;
		END
		ELSE IF @Status_Key_Code='PENDING_WITH_ISSUE'
		BEGIN
			UPDATE AV_NetLayerStatus
			SET [Status]=@value2,			
			PendingIssueDesc=@value3,
			pendingWithIssueOn=GETDATE(), 
			DriveCompletedOn = GETDATE(),		
			SubmittedOn = GETDATE(),
			SubmittedById = @UserId,
			pendingWithIssueById=@UserId
			WHERE LayerStatusId=@value1;
		END
		ELSE IF @Status_Key_Code='DRIVE_COMPLETED'
		BEGIN
			UPDATE AV_NetLayerStatus
			SET [Status] = @value2,	
			DriveCompletedOn = GETDATE()	    	
			WHERE LayerStatusId=@value1;
		END
		ELSE IF @Status_Key_Code='REPORT_SUBMITTED'
		BEGIN
			IF @curStatus1=93
			BEGIN
				UPDATE AV_NetLayerStatus
				SET [Status] =@value2				
				WHERE LayerStatusId=@value1;
			END
			ELSE IF @curStatus1=92
			BEGIN
				UPDATE AV_NetLayerStatus
				SET [Status] =@value2,
				SubmittedOn = GETDATE(),
				SubmittedById = @UserId
				WHERE LayerStatusId=@value1;
			END
			ELSE
			BEGIN
				RAISERROR('Net Layer can be submitted only in Status: Pending with Issue or Drive Completed.',16,1)
			END
		END
		ELSE IF @Status_Key_Code='COMPLETED'
		BEGIN
			UPDATE AV_NetLayerStatus
			SET [Status] = @value2,	
			CompletedOn = GETDATE(),
			AcceptedOn = GETDATE(),
			AcceptedById =@UserId	
			WHERE LayerStatusId=@value1;
		END
		
		--INSERT INTO AirViewLogs.dbo.Log_StatusChange
		--VALUES(@value1,@curStatus1,@value2,@UserId,GETDATE(),@value3);
		
		SET @tablename = (select CompleteName from ExternalDatabase where TableName = 'Log_StatusChange');
		-- Execute a query
		SET @query_a  = 'INSERT INTO ' + RTRIM(@tablename) + ' (LayerStatusId, OldStatusId, NewStatusId, ModifiedById, DateModified, Reason) VALUES('+@value1+','+RTRIM(@curStatus1)+','+@value2+','+RTRIM(@UserId)+','''+RTRIM(@current_date)			+''','''+@value3+''')';
		EXECUTE sp_executesql @query_a;
		
		--INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource)
	    --VALUES(@SiteId,@NetworkModeId,@BandId,@CarrierId,@ScopeId,@value2,@UserId,HOST_NAME(),GETDATE(),'Status_Change');
		
		SET @tablename = (select CompleteName from ExternalDatabase where TableName = 'Log_WOStatusTracker');
		SET @query_b  = 'INSERT INTO ' + RTRIM(@tablename) + ' (SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource)
		VALUES('+RTRIM(@SiteId)+','+RTRIM(@NetworkModeId)+','+RTRIM(@NetworkModeId)+','+RTRIM(@CarrierId)+','+RTRIM(@ScopeId)+','+@value2+','+RTRIM(@UserId)+','''+RTRIM(HOST_NAME())+''','''+RTRIM(@current_date)+''',''Status_Change'')';
		EXECUTE sp_executesql @query_b;
			   
	END
END