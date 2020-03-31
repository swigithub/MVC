-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_InsertWoTracker]

@SiteId numeric(18, 0),
@SectorId numeric(18, 0)=NULL,
@NetworkModeId numeric(18, 0)=NULL,
@BandId numeric(18, 0)=NULL,
@CarrierId numeric(18, 0)=NULL,
@WoRefId nvarchar(50),
@Latitude float,
@Longitude float,
@TesterId numeric(18, 0),
@TestType nvarchar(50),
@AppVersion nvarchar(50),
@AndroidVersion nvarchar(50),
@IMEI nvarchar(50)=NULL

AS
BEGIN
	DECLARE @woStatusId AS INT=-1
	DECLARE @DriveCompletedOn AS DATETIME=NULL
	
	
	 IF (@SiteId IS NOT NULL) AND (@NetworkModeId IS NOT NULL) AND (@BandId IS NOT NULL) AND (@CarrierId IS NOT NULL)
     --AND (@TestType!='Drive Completed' OR )
    BEGIN
		SELECT @woStatusId=ISNULL(anls.[Status],0), @DriveCompletedOn=anls.DriveCompletedOn
	    FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.NetworkModeId=@NetworkModeId AND anls.BandId=@BandId AND anls.CarrierId=@CarrierId
    END   
    
    INSERT INTO [dbo].[AV_WoTracker]([SiteId],[SectorId],[NetworkModeId],[BandId],[CarrierId],[WoRefId],[Latitude],[Longitude],[TesterId],[TestType],AppVersion,AndroidVersion)
	VALUES (@SiteId,@SectorId,@NetworkModeId,@BandId,@CarrierId,@WoRefId,@Latitude,@Longitude,@TesterId,@TestType,@AppVersion,@AndroidVersion)
    
 --   INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource,[Description])
	--VALUES(@SiteId,@NetworkModeId,@BandId,@CarrierId,NULL,@woStatusId,@TesterId,HOST_NAME(),GETDATE(),'Before: AV_InsertWoTracker',@TestType);
   
     IF ((@SiteId IS NOT NULL) AND (@NetworkModeId IS NOT NULL) AND (@BandId IS NOT NULL) AND (@CarrierId IS NOT NULL) AND (@DriveCompletedOn IS NULL)) --AND @woStatusId NOT IN(92,93,89,451)
     BEGIN     	 
     	UPDATE AV_NetLayerStatus
     	SET [Status] = 450
     	WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId
     	
     	UPDATE AV_Sites
     	SET [Status] = 450
     	WHERE SiteId=@SiteId
     	
     	IF @woStatusId=91
     	BEGIN 	
     		INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource,[Description])
			VALUES(@SiteId,@NetworkModeId,@BandId,@CarrierId,NULL,450,@TesterId,HOST_NAME(),GETDATE(),'WO_STATUS_INPROGRESS',@TestType);
     	END     	
     		
     	INSERT INTO AirViewLogs.dbo.Log_WOStatusTracker(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,StatusId,UserId,TerminalName,TransactionDate,TransactionSource,[Description])
		VALUES(@SiteId,@NetworkModeId,@BandId,@CarrierId,NULL,450,@TesterId,HOST_NAME(),GETDATE(),'AV_InsertWoTracker',@TestType);
     END
END