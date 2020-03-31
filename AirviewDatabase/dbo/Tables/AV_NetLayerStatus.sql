CREATE TABLE [dbo].[AV_NetLayerStatus] (
    [LayerStatusId]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteId]               NUMERIC (18)   NOT NULL,
    [NetworkModeId]        NUMERIC (18)   NOT NULL,
    [ScopeId]              NUMERIC (18)   NOT NULL,
    [BandId]               NUMERIC (18)   NOT NULL,
    [CarrierId]            NUMERIC (18)   NOT NULL,
    [TesterId]             NUMERIC (18)   NULL,
    [ReceivedOn]           DATETIME       CONSTRAINT [DF_AV_NetLayerStatus_ReceivedOn] DEFAULT (getdate()) NULL,
    [UploadedOn]           DATETIME       CONSTRAINT [DF_AV_NetLayerStatus_SubmittedOn] DEFAULT (getdate()) NULL,
    [AssignedOn]           DATETIME       NULL,
    [CompletedOn]          DATETIME       NULL,
    [ScheduledOn]          DATETIME       NULL,
    [DriveCompletedOn]     DATETIME       NULL,
    [DownloadedOn]         DATETIME       NULL,
    [SubmittedOn]          DATETIME       NULL,
    [AcceptedOn]           DATETIME       NULL,
    [UploadedById]         NUMERIC (18)   NULL,
    [ScheduledById]        NUMERIC (18)   CONSTRAINT [DF_AV_NetLayerStatus_TesterAssignedById] DEFAULT ((0)) NULL,
    [SubmittedById]        NUMERIC (18)   NULL,
    [AcceptedById]         NUMERIC (18)   NULL,
    [Status]               NUMERIC (18)   NULL,
    [StatusReason]         NUMERIC (18)   NULL,
    [PendingIssueDesc]     NVARCHAR (500) NULL,
    [IsActive]             BIT            CONSTRAINT [DF_AV_NetLayerStatus_IsActive] DEFAULT ((1)) NOT NULL,
    [IsPublished]          BIT            CONSTRAINT [DF_AV_NetLayerStatus_IsPublished] DEFAULT ((0)) NULL,
    [PublishedOn]          DATETIME       NULL,
    [isRedrive]            BIT            CONSTRAINT [DF_AV_NetLayerStatus_isRedrive] DEFAULT ((0)) NULL,
    [redriveTypeId]        INT            NULL,
    [redriveReasonId]      INT            NULL,
    [PWoRefID]             NVARCHAR (50)  NULL,
    [redriveComments]      NVARCHAR (500) NULL,
    [netLayerObservations] NVARCHAR (500) NULL,
    [pendingWithIssueOn]   DATETIME       NULL,
    [pendingWithIssueById] NUMERIC (18)   NULL,
    [MRBTS]                NVARCHAR (50)  NULL,
    [SiteClusterID]        NUMERIC (18)   NULL,
    [SiteSurveyId]         INT            NULL,
    CONSTRAINT [PK_AV_NetLayerStatus] PRIMARY KEY CLUSTERED ([LayerStatusId] ASC),
    CONSTRAINT [FK_AV_NetLayerStatus_AV_Sites] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[AV_Sites] ([SiteId])
);


GO

CREATE TRIGGER [dbo].[AV_UpdateWOStatus]
       ON [dbo].[AV_NetLayerStatus]
AFTER UPDATE
AS
BEGIN
		DECLARE @SiteId INT=0
		DECLARE @NetworkModeId INT=0
		DECLARE @BandId INT=0
		DECLARE @CarrierId INT=0	
		DECLARE @SchduledOn datetime

		--PRINT 1;
		--select i.*, d.*
		--from inserted i
		--join deleted d on (i.id = d.id)
		

		SELECT @SiteId = INSERTED.SiteId, @NetworkModeId=INSERTED.NetworkModeId, @BandId=INSERTED.BandId, @CarrierId=INSERTED.CarrierId, @SchduledOn=INSERTED.ScheduledOn
		FROM INSERTED;
	   
	   --In case of multiple statuses at layer level
		IF (SELECT COUNT(DISTINCT x.Status) FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.IsActive=1)>1
		BEGIN
			DECLARE @inProgStatus AS INT=(SELECT x.definationId FROM AD_Definations x WHERE KeyCode='IN_PROGRESS' AND x.IsActive=1)
			DECLARE @pendingIssueStatus AS BIT=CAST(0 AS BIT)
			
			DECLARE @pendingScheduledCount AS INT=(SELECT COUNT(x.SiteId) FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.IsActive=1 AND x.[Status]=90)
			DECLARE @scheduledCount AS INT=(SELECT COUNT(x.SiteId) FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.IsActive=1 AND x.ScheduledOn IS NOT NULL)
			DECLARE @driveCompletedCount AS INT=(SELECT COUNT(x.SiteId) FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.IsActive=1 AND x.DriveCompletedOn IS NOT NULL)
			DECLARE @reportSubmittedCount AS INT=(SELECT COUNT(x.SiteId) FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.IsActive=1 AND x.SubmittedOn IS NOT NULL)
			DECLARE @completedCount AS INT=(SELECT COUNT(x.SiteId) FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.IsActive=1 AND x.CompletedOn IS NOT NULL)
			DECLARE @totalLayers AS INT=(SELECT COUNT(x.SiteId) FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.IsActive=1)
			
			IF (SELECT COUNT(x.SiteId) FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.[Status]=93 AND x.IsActive=1)>0
			BEGIN
				SET @pendingIssueStatus=CAST(1 AS BIT)
				
				UPDATE AV_Sites
				SET [Status] = 93,
				ReportSubmittedOn = (CASE WHEN ReportSubmittedOn IS NULL THEN GETDATE() ELSE ReportSubmittedOn END),
				DriveCompletedOn = (CASE WHEN DriveCompletedOn IS NULL THEN GETDATE() ELSE DriveCompletedOn END)
				WHERE SiteId=@SiteId;
			END	
			ELSE
			BEGIN
				IF (SELECT COUNT(x.SiteId) FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.[Status]=450 AND x.IsActive=1)>0
				BEGIN
					--If any one Layer has In-Progress Status
					UPDATE AV_Sites
					SET [Status] = 450 
					WHERE SiteId=@SiteId;
				END
				ELSE IF (SELECT COUNT(x.SiteId) FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.[Status]=450 AND x.IsActive=1)=0
				BEGIN
					--If No Layer has In-Progress Status
					IF ((@pendingScheduledCount>0 AND @pendingScheduledCount<@totalLayers) AND (@scheduledCount>0 AND @scheduledCount<@totalLayers) AND @driveCompletedCount<@totalLayers AND @reportSubmittedCount<@totalLayers AND @completedCount<@totalLayers)
					BEGIN
						UPDATE AV_Sites
						SET [Status] = 91,
						ScheduledOn =(SELECT MAX(anls.ScheduledOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1),
						AssignedOn =(SELECT MAX(anls.AssignedOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1) 					
						WHERE SiteId=@SiteId;
					END
					ELSE IF (@scheduledCount=@totalLayers AND @driveCompletedCount<@totalLayers AND @reportSubmittedCount<@totalLayers AND @completedCount<@totalLayers)
					BEGIN
						UPDATE AV_Sites
						SET [Status] = 450
						--ScheduledOn =(SELECT MAX(anls.ScheduledOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1),
						--AssignedOn =(SELECT MAX(anls.AssignedOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1) 					
						WHERE SiteId=@SiteId;
					END
					
					ELSE IF (@scheduledCount=@totalLayers AND @driveCompletedCount=@totalLayers AND @reportSubmittedCount<@totalLayers AND @completedCount<@totalLayers)
					BEGIN
						UPDATE AV_Sites
						SET [Status] = 92,
						DriveCompletedOn =(SELECT MAX(anls.DriveCompletedOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1) 					
						WHERE SiteId=@SiteId;
					END
					
					ELSE IF (@scheduledCount=@totalLayers AND @driveCompletedCount=@totalLayers AND @reportSubmittedCount=@totalLayers AND @completedCount<@totalLayers)
					BEGIN
						UPDATE AV_Sites
						SET [Status] = 451,
						ReportSubmittedOn =(SELECT MAX(anls.SubmittedOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1) 					
						WHERE SiteId=@SiteId;
					END
					
					ELSE IF (@scheduledCount=@totalLayers AND @driveCompletedCount=@totalLayers AND @reportSubmittedCount=@totalLayers AND @completedCount=@totalLayers)
					BEGIN
						UPDATE AV_Sites
						SET [Status] = 89,
						CompletedOn =(SELECT MAX(anls.CompletedOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1) 					
						WHERE SiteId=@SiteId;
					END
				END
			END
		END
		ELSE IF (SELECT COUNT(DISTINCT x.Status) FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.IsActive=1)=1
		BEGIN
			DECLARE @WoStatus INT=(SELECT DISTINCT x.Status FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteId AND x.IsActive=1)
			DECLARE @Status AS NVARCHAR(50)=(SELECT x.KeyCode FROM AD_Definations x WHERE x.DefinationId=@WoStatus)

			IF @Status='PENDING_SCHEDULED'
			BEGIN
				UPDATE AV_Sites
				SET [Status] = @WoStatus,
				SubmittedOn = (SELECT MAX(anls.UploadedOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1) 		
				WHERE SiteId=@SiteId;
			END
			ELSE IF @Status='SCHEDULED'
			BEGIN
				UPDATE AV_Sites
				SET [Status] = @WoStatus,
				ScheduledOn = @SchduledOn,
				AssignedOn = (SELECT MAX(anls.AssignedOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1) 		,
				PublishedOn = (SELECT MAX(anls.PublishedOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1) 		,
				IsPublished = 1
				WHERE SiteId=@SiteId;
			END
			ELSE IF @Status='IN_PROGRESS'
			BEGIN
				UPDATE AV_Sites
				SET [Status] = @WoStatus
				WHERE SiteId=@SiteId;
			END			
			ELSE IF @Status='DRIVE_COMPLETED'
			BEGIN
				UPDATE AV_Sites
				SET [Status] = @WoStatus,
				DriveCompletedOn = (SELECT MAX(anls.DriveCompletedOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1)
				WHERE SiteId=@SiteId;
			END
			ELSE IF @Status='REPORT_SUBMITTED'
			BEGIN
				UPDATE AV_Sites
				SET [Status] = @WoStatus,
				ReportSubmittedOn = (SELECT MAX(anls.SubmittedOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1) 		
				WHERE SiteId=@SiteId;
			END
			ELSE IF @Status='PENDING_WITH_ISSUE'
			BEGIN
				UPDATE AV_Sites
				SET [Status] = @WoStatus,
				ReportSubmittedOn = (CASE WHEN ReportSubmittedOn IS NULL THEN GETDATE() ELSE ReportSubmittedOn END),
				DriveCompletedOn = (CASE WHEN DriveCompletedOn IS NULL THEN GETDATE() ELSE DriveCompletedOn END)
				WHERE SiteId=@SiteId;
			END								
			ELSE IF @Status='COMPLETED'
			BEGIN
				UPDATE AV_Sites
				SET [Status] = @WoStatus,
				CompletedOn = (SELECT MAX(anls.CompletedOn) FROM AV_NetLayerStatus AS anls WHERE anls.SiteId=@SiteId AND anls.IsActive=1)				 		
				WHERE SiteId=@SiteId;
			END			
		END
END