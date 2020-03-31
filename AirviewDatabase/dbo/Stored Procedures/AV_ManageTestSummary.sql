
CREATE PROCEDURE [dbo].[AV_ManageTestSummary] -- 45
	
	@Filter nvarchar(50) =null,
	@SiteId nvarchar(50) =null,
	@SectorId nvarchar(50) =null,
	@NetworkModeId nvarchar(50) =null,
	@BandId nvarchar(50) =null,
	@CarrierId nvarchar(50) =null,
	@ScopeId nvarchar(50) =null,
	@TestType nvarchar(50) =null,
	@Ping nvarchar(50) =null,
	@Value4 nvarchar(50) =null,--@DL
	@Value3 nvarchar(50) =null,--@UL
	@ImagePath nvarchar(500) =null,
	@Value1 nvarchar(50) =null,--@IsHandover
	@Value2 nvarchar(50) =NULL,--@HoType
	@UserId NUMERIC(18,0)=0,
	@CWComments NVARCHAR(MAX)='',
	@CCWComments NVARCHAR(MAX)='',
	@PDSCHComments NVARCHAR(MAX)='',
	@PUSCHComments NVARCHAR(MAX)=''
	
AS
BEGIN
	
	if @Filter='UpdateImages' 
	begin
		if (@TestType LIKE 'Ookla%' OR @TestType='OOKLA_TEST') 
		BEGIN
			-- @Value3=OoklaUplinkResult,@Value4=OoklaDownlinkResult
			update AV_SiteTestSummary
			set OoklaTestFilePath=@ImagePath,
			OoklaPingResult=@Ping , OoklaDownlinkResult=@Value4 , OoklaUplinkResult=@Value3
			where SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId --and ScopeId=@ScopeId
					
			
			--INSERT INTO AV_SiteOoklaLogs(SiteId,ScopeId,NetworkModeId,BandId,CarrierId,SectorId,Latitude,Longitude,DownlinkSpeed,UplinkSpeed,Latency,OoklaFilePath)
			--SELECT @SiteId,@ScopeId,@NetworkModeId,@BandId,@CarrierId,@SectorId,NULL,NULL,@Value4,@Value3,@Ping,@ImagePath
			
			
			
			--IF (SELECT COUNT(x.LogID) FROM AV_SiteTestLog x WHERE SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId AND x.TestType IN('Ookla'))=0
			--BEGIN
				INSERT INTO AV_SiteOoklaLogs(SiteId,ScopeId,NetworkModeId,BandId,CarrierId,SectorId,Latitude,Longitude,DownlinkSpeed,UplinkSpeed,Latency,OoklaFilePath,NetworkMode,RSRP,RSRQ,RSNR,Timestamp)
				SELECT @SiteId,@ScopeId,@NetworkModeId,@BandId,@CarrierId,@SectorId,NULL,NULL,@value4,@value3,@Ping,@ImagePath,NULL,NULL,NULL,NULL,GETDATE();
			--END
			--ELSE
			--BEGIN
			--	DECLARE @logNetMode AS NVARCHAR(15)=''
			--	DECLARE @serverTimestamp AS DATETIME
				
			--	SELECT TOP 1 @serverTimestamp = x.serverTimeStamp FROM AV_SiteTestLog x WHERE SiteId=@SiteId
			--	and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId
			--	AND x.TestType IN('Ookla')
			--	ORDER BY x.serverTimeStamp DESC
			
			--	IF (SELECT COUNT(DISTINCT x.NetworkMode) FROM AV_SiteTestLog x WHERE SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId AND x.TestType IN('Ookla') AND x.serverTimeStamp=@serverTimestamp)=1
			--	BEGIN
			--		IF (SELECT DISTINCT x.NetworkMode FROM AV_SiteTestLog x WHERE SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId AND x.TestType IN('Ookla') AND x.serverTimeStamp=@serverTimestamp)='NR'
			--		BEGIN
			--			SET @logNetMode='NR'
			--		END
			--		ELSE IF (SELECT DISTINCT x.NetworkMode FROM AV_SiteTestLog x WHERE SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId AND x.TestType IN('Ookla') AND x.serverTimeStamp=@serverTimestamp)='LTE'
			--		BEGIN
			--			SET @logNetMode='LTE'
			--		END
			--		ELSE IF (SELECT DISTINCT x.NetworkMode FROM AV_SiteTestLog x WHERE SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId AND x.TestType IN('Ookla') AND x.serverTimeStamp=@serverTimestamp)='WCDMA'
			--		BEGIN
			--			SET @logNetMode='WCDMA'
			--		END
			--		ELSE IF (SELECT DISTINCT x.NetworkMode FROM AV_SiteTestLog x WHERE SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId AND x.TestType IN('Ookla') AND x.serverTimeStamp=@serverTimestamp)='GSM'
			--		BEGIN
			--			SET @logNetMode='GSM'
			--		END
			--	END
			--	ELSE
			--	BEGIN				
			--			SET @logNetMode='NR'				
			--	END
			
			--	INSERT INTO AV_SiteOoklaLogs(SiteId,ScopeId,NetworkModeId,BandId,CarrierId,SectorId,Latitude,Longitude,DownlinkSpeed,UplinkSpeed,Latency,OoklaFilePath,NetworkMode,RSRP,RSRQ,RSNR)
			--	SELECT @SiteId,@ScopeId,@NetworkModeId,@BandId,@CarrierId,@SectorId,
			--	(SELECT TOP 1 x.Latitude FROM AV_SiteTestLog x WHERE SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId AND x.TestType IN('Ookla')),
			--	(SELECT TOP 1 x.Longitude FROM AV_SiteTestLog x WHERE SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId AND x.TestType IN('Ookla')),
			--	@value4,@value3,@Ping,@ImagePath,
			--	NULL,
			--	(SELECT AVG(x.LteRsrp) FROM AV_SiteTestLog x WHERE SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId AND x.TestType IN('Ookla') AND x.LteRsrp<>0),
			--	(SELECT AVG(x.LteRsrq) FROM AV_SiteTestLog x WHERE SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId AND x.TestType IN('Ookla') AND x.LteRsrq<>0),
			--	(SELECT AVG(x.LteRsnr) FROM AV_SiteTestLog x WHERE SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId AND x.SectorId=@SectorId AND x.TestType IN('Ookla') AND x.LteRsnr<>0);
			--END
			
			
		END		
	    ELSE IF	@TestType='ISSUE_REPORT'
	    BEGIN
	    	UPDATE AV_SiteIssueTracker
	    	SET ImagePath = @ImagePath
	    	WHERE SiteId=@SiteId  and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId and ScopeId=@ScopeId
	    END
	    ELSE IF	@TestType='AirView_Stationary'
	    BEGIN
	    	UPDATE AV_SiteTestSummary
	    	SET StationaryTestFilePath = @ImagePath
	    	where SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId and ScopeId=@ScopeId
	    END 
	    ELSE IF	@TestType='AirView_CW'
	    BEGIN
	    	UPDATE AV_SiteTestSummary
	    	SET CwTestFilePath = @ImagePath
	    	where SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId and ScopeId=@ScopeId
	    END 
	    ELSE IF	@TestType='AirView_CCW'
	    BEGIN
	    	UPDATE AV_SiteTestSummary
	    	SET CcwTestFilePath = @ImagePath
	    	where SiteId=@SiteId and SectorId=@SectorId and NetworkModeId=@NetworkModeId and BandId=@BandId and CarrierId = @CarrierId and ScopeId=@ScopeId
	    END 
	END
	
	else if @Filter='UpdateHandoverStatus'
	BEGIN
		-- @Value1=CwHandoverStatus
		update AV_SiteTestSummary
		set CwHandoverStatus=@Value1			
		where SiteId=0;
		
	END
	
	ELSE IF @Filter='E911Status'
	BEGIN
		DECLARE @TesterId AS NUMERIC=0
		
		SET @TesterId=(SELECT TOP 1 anls.TesterId FROM AV_NetLayerStatus AS anls WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND ScopeId=@ScopeId AND anls.IsActive=1)		
		
		INSERT INTO [dbo].[AV_WoTracker]([SiteId],[SectorId],[NetworkModeId],[BandId],[CarrierId],[WoRefId],[Latitude],[Longitude],[TesterId],[TestType],AppVersion,AndroidVersion)
		VALUES (@SiteId,@SectorId,@NetworkModeId,@BandId,@CarrierId,'',0,0,@TesterId,'E911 Already Performed','','')
		
		UPDATE AV_SiteTestSummary
		SET IsE911Performed=CAST(@Value1 AS BIT), TesterName=@Value2, Comments=@Value3		
		where SiteId=@SiteId AND SectorId=@SectorId
	END
	ELSE IF @Filter='E911ConfirmStatus'
	BEGIN
		-- @Value1=E911Status , @Value3=E911Comment
		DECLARE @TesterId1 AS NUMERIC=0		
		SET @TesterId1=(SELECT TOP 1 anls.TesterId FROM AV_NetLayerStatus AS anls WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND ScopeId=@ScopeId AND anls.IsActive=1)		
				
		INSERT INTO [dbo].[AV_SiteIssueTracker]([SiteId],[TesterId],[NetworkModeId],[BandId],[CarrierId],[ScopeId],[Description],[Status],ImagePath, IssueType)
	    VALUES (@SiteId,@TesterId1,@NetworkModeId,@BandId,@CarrierId,@ScopeId,@Value3,'',NULL,449)	
		
		UPDATE AV_SiteTestSummary
		SET E911Status = @Value1,SwitchComment=@Value3,TechName=@Value2
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND SectorId=@SectorId AND ScopeId=@ScopeId
			
	END

	
END