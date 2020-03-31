CREATE PROCEDURE PM_DataImport_Old
	@Filter NVARCHAR(50),
	@ProjectId NUMERIC(18,0),
	@Data [dbo].[PM_ImportList] READONLY
	
AS
BEGIN
	IF @Filter='Import_Project_Plan'
	BEGIN
		--FACode	Milestone	Stage	Plan	Forecast	Target	Actual	Status	OldPlan	OldForecast	OldTarget	OldActual	Status
		SELECT x.Value1 'FACode', x.Value2 'Milestone', x.Value3 'Stage', CAST(x.Value4 AS DATE) 'Plan', CAST(x.Value5 AS DATE) 'Forecast', CAST(x.Value6 AS DATE) 'Target', CAST(x.Value7 AS DATE) 'Actual', x.Value8 'Status',
		CAST(sts.PlannedDate AS DATE) 'OldPlan', CAST(sts.EstimatedStartDate AS DATE) 'OldForecast', CAST(sts.TargetDate AS DATE) 'OldTarget', CAST(sts.ActualEndDate AS DATE) 'OldActual', ad.DefinationName 'OldStatus'
		FROM @Data x 
		INNER JOIN PM_ProjectSites AS sit ON sit.FACode=x.Value1
		INNER JOIN PM_Tasks AS mls ON mls.Title=x.Value2		
		INNER JOIN PM_SiteTasks AS sts ON sts.TaskId=mls.TaskId AND sts.ProjectSiteId=sit.ProjectSiteId
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=sts.StatusId
		WHERE (x.Value3 IS NULL OR x.Value3 = '')
		UNION ALL
		SELECT x.Value1 'FACode', x.Value2 'Milestone', x.Value3 'Stage', x.Value4 'Plan', x.Value5 'Forecast', x.Value6 'Target', x.Value7 'Actual', x.Value8 'Status',
		sts.PlannedDate 'OldPlan', sts.EstimatedStartDate 'OldForecast', sts.TargetDate 'OldTarget', sts.ActualEndDate 'OldActual', ad.DefinationName 'OldStatus'
		FROM @Data x 
		INNER JOIN PM_ProjectSites AS sit ON sit.FACode=x.Value1
		INNER JOIN PM_Tasks AS stg ON stg.Title=x.Value3
		INNER JOIN PM_SiteTasks AS sts ON sts.TaskId=stg.TaskId AND sts.ProjectSiteId=sit.ProjectSiteId
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=sts.StatusId
		WHERE (x.Value3 IS NOT NULL AND x.Value3 != '')
	END
	ELSE IF @Filter='Save_Project_Plan'
	BEGIN
		DECLARE @FACode AS NVARCHAR(50)=''
		DECLARE @Milestone AS NVARCHAR(50)=''
		DECLARE @Stage AS NVARCHAR(50)=''
		DECLARE @PlanDate AS NVARCHAR(50)=''
		DECLARE @ForecastDate AS NVARCHAR(50)=''
		DECLARE @TargetDate AS NVARCHAR(50)=''
		DECLARE @ActualDate AS NVARCHAR(50)=''
		DECLARE @Status AS NVARCHAR(50)=''
		
		DECLARE db_cluster2 CURSOR FOR  
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8 FROM @Data AS l
		OPEN db_cluster2 
		FETCH NEXT FROM db_cluster2 INTO @FACode, @Milestone, @Stage,@PlanDate, @ForecastDate, @TargetDate,@ActualDate,@Status
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			DECLARE @ProjectSiteId AS NUMERIC(18,0)=(SELECT pps.ProjectSiteId FROM PM_ProjectSites AS pps WHERE pps.IsActive=1 AND pps.FACode=@FACode)
			DECLARE @MilestoneId AS NUMERIC(18,0)=(SELECT pps.SiteTaskId FROM PM_SiteTasks AS pps INNER JOIN PM_Tasks AS pt ON pt.TaskId=pps.TaskId WHERE pps.IsActive=1 AND pt.Title=@Milestone AND pps.ProjectSiteId=@ProjectSiteId)
			DECLARE @StageId AS NUMERIC(18,0)=(SELECT pps.SiteTaskId FROM PM_SiteTasks AS pps INNER JOIN PM_Tasks AS pt ON pt.TaskId=pps.TaskId WHERE pps.IsActive=1 AND pt.Title=@Stage AND pps.ProjectSiteId=@ProjectSiteId)
			DECLARE @StatusId AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Status AND adt.DefinationType='Project Status')
			--PRINT @StatusId
			IF @Stage IS NOT NULL OR @Stage != ''
			BEGIN
				UPDATE PM_SiteTasks
				SET	StatusId = @StatusId,
					PlannedDate = @PlanDate,
					EstimatedStartDate = @ForecastDate,					
					TargetDate = @TargetDate,
					ActualEndDate = @ActualDate
				WHERE SiteTaskId=@MilestoneId;
			END
			ELSE 
			BEGIN
				UPDATE PM_SiteTasks
				SET	StatusId = @StatusId,
					PlannedDate = @PlanDate,
					EstimatedStartDate = @ForecastDate,					
					TargetDate = @TargetDate,
					ActualEndDate = @ActualDate
				WHERE SiteTaskId=@StageId;
			END
		FETCH NEXT FROM db_cluster2 INTO @FACode, @Milestone, @Stage,@PlanDate, @ForecastDate, @TargetDate,@ActualDate,@Status
		END   
		CLOSE db_cluster2   
		DEALLOCATE db_cluster2
	END
	ELSE IF @Filter='Import_WR_Issues' 
	BEGIN
		--FACode	eNB	othereNB	Schedule	Actual	MW	Status	Alarm	Issues	WhoFix	Notes	ContentType	Attachments	Created	CreatedBy
		--INSERT INTO PM_Issues(ProjectId,ProjectSiteId,TaskId,IssuePriorityId,IssueStatusId,IssueCategoryId,ReasonId,IssueById,[Description],RequestedById,RequestDate,AssignedToId,
		--ForecastDate,TargetDate,ActualStartDate,ActualEndDate,IsUnavoidable,ENB,ExtendedeNB,EquipmentId,AOTSCR,ActivityTypeId,ItemTypeId,ItemFilePath,SeverityId,AlarmId,MSWindowId,RequestedBy,StatusId)
		--SELECT @ProjectId,
		--(SELECT pps.ProjectSiteId FROM PM_ProjectSites pps WHERE pps.IsActive=1 AND pps.FACode=x.Value1) 'FACode',
		--50080,
		--FROM @Data x
		
		DECLARE @iFACode AS NVARCHAR(50)=''
		DECLARE @ENB AS NVARCHAR(50)=''
		DECLARE @OtherENB AS NVARCHAR(50)=''
		DECLARE @Schedule AS NVARCHAR(50)=''
		DECLARE @Actual AS NVARCHAR(50)=''
		DECLARE @MW AS NVARCHAR(50)=''
		DECLARE @Alarm AS NVARCHAR(50)=''
		DECLARE @Issues AS NVARCHAR(50)=''
		DECLARE @WhoFix AS NVARCHAR(50)=''
		DECLARE @Notes AS NVARCHAR(MAX)=''
		DECLARE @ContentType AS NVARCHAR(50)=''
		DECLARE @Attachments AS NVARCHAR(50)=''
		DECLARE @Created AS NVARCHAR(50)=''
		DECLARE @CreatedBy AS NVARCHAR(50)=''
		
		DECLARE db_issues CURSOR FOR  
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14 FROM @Data AS l
		OPEN db_issues 
		FETCH NEXT FROM db_issues INTO @iFACode, @ENB, @OtherENB,@Schedule, @Actual, @MW,@Alarm,@Issues,@WhoFix,@Notes,@ContentType,@Attachments,@Created,@CreatedBy
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			DECLARE @iProjectSiteId AS NUMERIC(18,0)=(SELECT pps.ProjectSiteId FROM PM_ProjectSites AS pps WHERE pps.IsActive=1 AND pps.FACode=@iFACode)
			DECLARE @IssueStatusId  AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Status AND adt.DefinationType='Issue Status')
			DECLARE @IssueCategoryId  AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Status AND adt.DefinationType='Issue Category')
			DECLARE @IssueTypeId  AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Issues AND adt.DefinationType='Issue Type')
			DECLARE @ContentTypeId  AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Issues AND adt.DefinationType='Item Type')   
			DECLARE @AlarmId  AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Issues AND adt.DefinationType='Alarms')   
			DECLARE @MSId  AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Issues AND adt.DefinationType='Milestone Window')   
			
			INSERT INTO PM_Issues(ProjectId,ProjectSiteId,TaskId,IssuePriorityId,IssueStatusId,IssueCategoryId,ReasonId,IssueById,[Description],RequestDate,AssignedToId,
			ForecastDate,TargetDate,ActualEndDate,IsUnavoidable,ENB,ExtendedeNB,EquipmentId,AOTSCR,ActivityTypeId,ItemTypeId,ItemFilePath,SeverityId,AlarmId,MSWindowId,RequestedBy,StatusId)
			SELECT @ProjectId,@iProjectSiteId,50080,163430,@IssueStatusId,@IssueCategoryId, @IssueTypeId,
			(CASE WHEN @WhoFix IN('AT&T','AT&T Unavoid') THEN 3 WHEN @WhoFix IN('Nokia','Nokia Unavoid') THEN 2 ELSE NULL END),
			@Notes, @ActualDate,NULL,@Schedule,NULL,@ActualDate,
			(CASE WHEN @WhoFix IN('AT&T Unavoid','Nokia Unavoid') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END),
			@ENB,@OtherENB,NULL,NULL,NULL,@ContentTypeId,NULL,NULL,@AlarmId,@MSId,@CreatedBy,@StatusId
		FETCH NEXT FROM db_issues INTO @iFACode, @ENB, @OtherENB,@Schedule, @Actual, @MW,@Alarm,@Issues,@WhoFix,@Notes,@ContentType,@Attachments,@Created,@CreatedBy
		END   
		CLOSE db_issues   
		DEALLOCATE db_issues
	END
	ELSE IF @Filter='Import_WR_Site'
	BEGIN
		--FACode	eNB	othereNB	Reasons	Schedule	Actual	MW	Status	Alarm	Issues	Notes	AddlNotes	NetAct	USID	ContentType	Created	CreatedBy
		
		
		DECLARE @xFACode AS NVARCHAR(50)=''
		DECLARE @xENB AS NVARCHAR(50)=''
		DECLARE @xOtherENB AS NVARCHAR(50)=''
		DECLARE @xReasons AS NVARCHAR(50)=''
		DECLARE @xSchedule AS NVARCHAR(50)=''
		DECLARE @xActual AS NVARCHAR(50)=''
		DECLARE @xMW AS NVARCHAR(50)=''
		DECLARE @xStatus AS NVARCHAR(50)=''
		DECLARE @xAlarm AS NVARCHAR(50)=''
		DECLARE @xIssues AS NVARCHAR(50)=''
		DECLARE @xNotes AS NVARCHAR(MAX)=''		
		DECLARE @xAddlNotes AS NVARCHAR(MAX)=''
		DECLARE @xContentType AS NVARCHAR(50)=''
		DECLARE @xUSID AS NVARCHAR(50)=''		
		DECLARE @xCreated AS NVARCHAR(50)=''
		DECLARE @xCreatedBy AS NVARCHAR(50)=''
		
		DECLARE db_issues CURSOR FOR  
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14,l.Value15,l.Value16,l.Value17 FROM @Data AS l
		OPEN db_issues 
		FETCH NEXT FROM db_issues INTO @xFACode, @xENB, @xOtherENB,@xReasons, @xSchedule, @xActual,@xMW,@xIssues,@xNotes,@xAddlNotes,@xContentType,@xUSID,@xCreated,@xCreatedBy
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			DECLARE @xProjectSiteId AS NUMERIC(18,0)=(SELECT pps.ProjectSiteId FROM PM_ProjectSites AS pps WHERE pps.IsActive=1 AND pps.FACode=@xFACode)
			DECLARE @xStatusId  AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@xStatus AND adt.DefinationType='Project Site Status')
			DECLARE @xContentTypeId  AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@xContentType AND adt.DefinationType='Item Type')   
			DECLARE @xAlarmId  AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@xAlarm AND adt.DefinationType='Alarms')   
			DECLARE @xMSId  AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@xMW AND adt.DefinationType='Milestone Window')   
			
			UPDATE PM_ProjectSites
			SET StatusId = @xStatusId, MSWindowId=@xMSId, AlarmId = @xAlarmId
			WHERE ProjectSiteId=@xProjectSiteId
		
			
			INSERT INTO PM_SiteLog(ProjectSiteId,StatusId,MSWindowId,AlarmId,DESCRIPTION,UserID,CreatedOn,ActivityTypeId,GngId,ItemTypeId,ItemFilePath)
			SELECT @xProjectSiteId, @xStatusId, @xMSId, @xAlarmId, @xNotes, 11, GETDATE(), NULL, NULL, @xContentTypeId, NULL	
		FETCH NEXT FROM db_issues INTO @xFACode, @xENB, @xOtherENB,@xReasons, @xSchedule, @xActual,@xMW,@xIssues,@xNotes,@xAddlNotes,@xContentType,@xUSID,@xCreated,@xCreatedBy
		END   
		CLOSE db_issues   
		DEALLOCATE db_issues
	END
END