CREATE PROCEDURE [dbo].[PM_DataImport]
	@Filter NVARCHAR(50),
	@ProjectId NUMERIC(18,0)=0,
	@Data [dbo].[PM_ImportLists] READONLY	
AS
BEGIN
	DECLARE @FACode AS NVARCHAR(50)=''
	DECLARE @ENB AS NVARCHAR(50)=''	--CommonId: is eNB in master site dump
	DECLARE @OtherENB AS NVARCHAR(50)=''
	DECLARE @AOTSCR AS NVARCHAR(50)=''
	DECLARE @ExtendedENB AS NVARCHAR(50)=''
	DECLARE @SiteName AS NVARCHAR(50)=''
	DECLARE @Reasons AS NVARCHAR(50)=''	--CC Column in WR_Log_Sites/Export
	DECLARE @Region AS NVARCHAR(50)=''
	DECLARE @Market AS NVARCHAR(50)=''
	DECLARE @SubMarket AS NVARCHAR(50)=''
	DECLARE @Schedule AS NVARCHAR(50)=''
	DECLARE @Actual AS NVARCHAR(50)=''
	DECLARE @MW AS NVARCHAR(50)=''	
	DECLARE @Status AS NVARCHAR(50)=''
	DECLARE @Alarm AS NVARCHAR(50)=''
	DECLARE @Issues AS NVARCHAR(50)=''
	DECLARE @WhoFix AS NVARCHAR(50)=''	
	DECLARE @Notes AS NVARCHAR(MAX)=''		
	DECLARE @AddlNotes AS NVARCHAR(MAX)=''
	DECLARE @NetAct AS NVARCHAR(50)=''
	DECLARE @Task AS NVARCHAR(50)=''
	DECLARE @Effort AS NVARCHAR(50)=''
	DECLARE @Migrated AS NVARCHAR(50)=''
	DECLARE @TicketRequest AS NVARCHAR(50)=''
	DECLARE @TechName AS NVARCHAR(50)=''
	DECLARE @TechNumber AS NVARCHAR(50)=''
	DECLARE @TechMail AS NVARCHAR(50)=''
	DECLARE @USID AS NVARCHAR(50)=''
	DECLARE @ExpirationDate AS NVARCHAR(50)=''
	DECLARE @Notification AS NVARCHAR(50)=''	
	DECLARE @ContentType AS NVARCHAR(50)=''
	DECLARE @AppCreatedBy AS NVARCHAR(50)=''
	DECLARE @AppModifiedBy AS NVARCHAR(50)=''
	DECLARE @Attachments AS NVARCHAR(50)='' --field exists only in WR_Issue_Log
	DECLARE @WorkflowInstanceId AS NVARCHAR(50)=''
	DECLARE @FileType AS NVARCHAR(50)=''
	DECLARE @ModifiedOn AS NVARCHAR(50)=''
	DECLARE @PMO AS NVARCHAR(50)=''			
	DECLARE @Created AS NVARCHAR(50)=''	--Created Date
	DECLARE @CreatedBy AS NVARCHAR(50)=''
		
	DECLARE @StreetAddress AS NVARCHAR(150)=''
	DECLARE @City AS NVARCHAR(50)=''
	DECLARE @State AS NVARCHAR(50)=''
	DECLARE @ZIP AS NVARCHAR(50)=''
	DECLARE @County AS NVARCHAR(50)=''
	DECLARE @Latitude AS NVARCHAR(50)=''
	DECLARE @Longitude AS NVARCHAR(50)=''
	DECLARE @vMME AS NVARCHAR(50)=''
	DECLARE @ControlledIntro AS NVARCHAR(50)=''
	DECLARE @SuperBowl AS NVARCHAR(50)=''
	DECLARE @SiteType AS NVARCHAR(50)=''
	DECLARE @DASInbuilding AS NVARCHAR(50)=''
	DECLARE @FirstNetRAN AS NVARCHAR(50)=''
	DECLARE @iPlanJob AS NVARCHAR(50)=''
	DECLARE @iPlanStatus AS NVARCHAR(50)=''
	DECLARE @iPlanIssueDate AS NVARCHAR(50)=''
	DECLARE @PACENumber AS NVARCHAR(50)=''
	DECLARE @ActualEndDate AS NVARCHAR(50)=''
	DECLARE @TargetDate AS NVARCHAR(50)=''
	DECLARE @ActivityType AS NVARCHAR(50)=''

	DECLARE @TSSPlan AS NVARCHAR(50)=''
	DECLARE @TSSForecast AS NVARCHAR(50)=''
	DECLARE @TSSSubmitted AS NVARCHAR(50)=''
	DECLARE @SSMForecast AS NVARCHAR(50)=''
	DECLARE @SSMActual AS NVARCHAR(50)=''
	DECLARE @PreInstPlan AS NVARCHAR(50)=''
	DECLARE @PreInstForecast AS NVARCHAR(50)=''
	DECLARE @PreInstSubmitted AS NVARCHAR(50)=''
	DECLARE @MigrationPlan AS NVARCHAR(50)=''
	DECLARE @MigrationForecast AS NVARCHAR(50)=''
	DECLARE @MigrationSubmitted AS NVARCHAR(50)=''
	DECLARE @EPLOrdered AS NVARCHAR(50)=''
	DECLARE @EPLCalledOut AS NVARCHAR(50)=''
	DECLARE @EPLDelivered AS NVARCHAR(50)=''
	DECLARE @EPLStatus AS NVARCHAR(50)=''
	DECLARE @IssueCategory AS NVARCHAR(50)=''
	DECLARE @RequestedBy AS NVARCHAR(50)=''
	DECLARE @RequestDate AS NVARCHAR(50)=''
	DECLARE @Equipment AS NVARCHAR(50)=''
	DECLARE @MActivityType AS NVARCHAR(50)=''
	DECLARE @WItemType AS NVARCHAR(50)=''
	DECLARE @Priority AS NVARCHAR(50)=''
    DECLARE @Severity AS NVARCHAR(50)=''
	DECLARE @AssignedTo AS NVARCHAR(50)=''
	DECLARE @ItemFilePath AS NVARCHAR(1000)=''
	DECLARE @ForecastEndDate AS  NVARCHAR(50)=''
		DECLARE @ItemFile AS  NVARCHAR(50)=''
		DECLARE @GNG AS  NVARCHAR(50)=''
		DECLARE @IsAdditional AS  NVARCHAR(50)=''
	
	
	----------------References---------------
	DECLARE @ProjectSiteId AS NUMERIC(18,0)=0
	DECLARE @ReasonId AS NUMERIC(18,0)=0 --CC Col: Reason in above defs.
	DECLARE @RegionId AS NUMERIC(18,0)=0
	DECLARE @TaskId AS NUMERIC(18,0)=0
	DECLARE @IssueById AS NUMERIC(18,0)=0
	DECLARE @MActivityTypeId AS NUMERIC(18,0)=0
	DECLARE @SeverityId AS NUMERIC(18,0)=0
	DECLARE @MSWindowId AS NUMERIC(18,0)=0
	DECLARE @WItemTypeId AS NUMERIC(18,0)=0
	DECLARE @TaskTypeId AS NUMERIC(18,0)=0
	DECLARE @SiteTaskId AS NUMERIC(18,0)=0

	DECLARE @IssueStatusId AS NUMERIC(18,0)=0
	DECLARE @AssignedToId AS NUMERIC(18,0)=0
	DECLARE @PriorityId AS NUMERIC(18,0)=0

		
	DECLARE @EquipmentId AS NUMERIC(18,0)=0
	DECLARE @MarketId AS NUMERIC(18,0)=0
	DECLARE @SiteTypeId AS NUMERIC(18,0)=0
	DECLARE @MaintenanceWindowId AS NUMERIC(18,0)=0
	DECLARE @StatusId AS NUMERIC(18,0)=0
	DECLARE @AlarmId AS NUMERIC(18,0)=0
	DECLARE @GngId AS NUMERIC(18,0)=0
	DECLARE @IssueCategoryId  AS NUMERIC(18,0)=0
	DECLARE @IssueTypeId AS NUMERIC(18,0)=0	--Issues->is: issue type(access,execution etc.)
	DECLARE @IssueOwnerId AS NUMERIC(18,0)=0
	DECLARE @IsUnavoidable AS bit=CAST(0 AS BIT) --IsUnavoidable based on WhoFix->is: Issue Owner (nokia, att, unavoid etc.)
	DECLARE @ContentTypeId AS NUMERIC(18,0)=0 --ContentType
	DECLARE @CreatedById AS NUMERIC(18,0)=0 --CreatedBy
	
	DECLARE @newFACodes AS NVARCHAR(MAX)=''

	DECLARE @MigCount as int=0;
	DECLARE @nMigCount as int=0;
	
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
	ELSE IF @Filter='Import_WR_Site'
	BEGIN
		--"Value1", p.FACode, 
		--"Value2", p.eNB,
		--"Value3", p.othereNB,
		--"Value4", p.SiteName,
		--"Value5", p.CC, 
		--"Value6", p.Market, 
		--"Value7", p.Schedule,
		--"Value8", p.Actual, 
		--"Value9", p.MW, 
		--"Value10", p.Status, 
		--"Value11", p.Alarm, 
		--"Value12", p.Issues, 
		--"Value13", p.WhoFix, 
		--"Value14", p.Notes, 
		--"Value15", p.AddlNotes,
		--"Value16", p.NetAct, 
		--"Value17", p.Effort, 
		--"Value18", p.Migrated, 
		--"Value19", p.TicketRequest, 
		--"Value20", p.TechName, 
		--"Value21", p.TechNumber, 
		--"Value22", p.TechEmail, 
		--"Value23", p.USID,
		--"Value24", p.ExpirationDate, 
		--"Value25", p.Notification, 
		--"Value26", p.ContentType, 
		--"Value27", p.AppCreatedBy, 
		--"Value28", p.AppModifiedBy, 
		--"Value29", p.WorkflowInstanceID, 
		--"Value30", p.FileType, 
		--"Value31", p.ModifiedOn,
		--"Value32", p.PMO, 
		--"Value33", p.Created, 
		--"Value34", p.CreatedBy
		
		--Reasons: CC Column in WR_Log_Sites/Export
		
		DECLARE db_sites CURSOR FOR  
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14,l.Value15,l.Value16,l.Value17,
		l.Value18
		FROM @Data AS l
		OPEN db_sites 
		FETCH NEXT FROM db_sites INTO 
		@FACode,@ActivityType,@Alarm,@GNG,@Schedule,@Attachments,@MW,@ItemFile,@Notes,@eNB,@OthereNB,@Equipment,@AOTSCR,@USID,@Status,@IsAdditional,		
		@Created,@CreatedBy
		WHILE @@FETCH_STATUS = 0   
		BEGIN
				SET @ProjectSiteId=0
			SET @ProjectSiteId=(SELECT TOP 1 pps.ProjectSiteId FROM PM_ProjectSites AS pps WHERE pps.FACode=@FACode AND pps.IsActive=1 And ProjectId =@ProjectId)
		IF @ProjectSiteId > 0 
		Begin
			SET @MActivityTypeId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1AND ad.DefinationName=@ActivityType AND adt.DefinationType='Activity Type')
          SET @GngId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1AND ad.DefinationName=@GNG AND adt.DefinationType='GNG Status')
			--CC Col: Reason in above defs.
		SET @ReasonId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
													WHERE ad.IsActive=1 AND ad.DefinationName=@Reasons AND adt.DefinationType='Reasons') 
			SET @MarketId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
													 WHERE ad.IsActive=1 AND ad.DefinationName=@Reasons AND adt.DefinationType='City')
			SET @MaintenanceWindowId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
													 WHERE ad.IsActive=1 AND ad.DefinationName=@MW AND adt.DefinationType='Milestone Window')   
			SET @StatusId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                WHERE ad.IsActive=1 AND ad.DefinationName=@Status AND adt.DefinationType='Project Site Status')
			SET @AlarmId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                               WHERE ad.IsActive=1 AND ad.DefinationName=@Alarm AND adt.DefinationType='Alarms')
			
			--Issues->is: issue type(access,execution etc.)
			SET @IssueTypeId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
													 WHERE ad.IsActive=1 AND ad.DefinationName=@Reasons AND adt.DefinationType='Issue Type')
			
			--WhoFix->is: Issue Owner (nokia, att, unavoid etc.)			
			SET @IssueOwnerId=(CASE WHEN @WhoFix IN('AT&T','AT&T Unavoid') THEN 3 WHEN @WhoFix IN('Nokia','Nokia Unavoid') THEN 2 ELSE NULL END)
			
			--ContentType
			SET @ContentTypeId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@ContentType AND adt.DefinationType='Item Type') 
			
			--CreatedBy
			SET @CreatedById=(SELECT TOP 1 usr.UserId FROM Sec_Users usr WHERE usr.IsActive=1 AND usr.LastName+', '+usr.FirstName=@CreatedBy)
			
			IF @ReasonId=0
			BEGIN
			    INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
			    SELECT @Reasons,NULL,24,'REASON',0,CAST(1 AS BIT),@Reasons;
			    	
			    SELECT @ReasonId=@@IDENTITY
			END
			
			IF @MaintenanceWindowId=0
			BEGIN
			    INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
			    SELECT @MW,NULL,70059,'MILESTONE WINDOW',0,CAST(1 AS BIT),@MW;
			    	
			    SELECT @MaintenanceWindowId=@@IDENTITY
			END
			IF @AlarmId=0
			BEGIN
			    INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
			    SELECT @Alarm,NULL,70062,'ALARMS',0,CAST(1 AS BIT),@Alarm;
			    	
			    SELECT @AlarmId=@@IDENTITY
			END		
			UPDATE PM_ProjectSites
			SET StatusId = @StatusId, MSWindowId=@MaintenanceWindowId, AlarmId = @AlarmId
			WHERE ProjectSiteId=@ProjectSiteId
			INSERT INTO PM_SiteLog(ProjectSiteId,ActivityTypeId,AlarmId,GngId,ItemFilePath,MSWindowId,ItemTypeId,Description,StatusId,IsAdditional,CreatedOn)
			SELECT @ProjectSiteId,@MActivityTypeId, @AlarmId,@GngId,@Attachments,@MaintenanceWindowId,@ContentTypeId,@Notes,@StatusId, @IsAdditional, 
			(CASE WHEN @Created!='' OR @Created IS NOT NULL THEN CAST(@Created AS DATETIME) ELSE NULL END);
			End
		FETCH NEXT FROM db_sites INTO 
		@FACode,@ActivityType,@Alarm,@GNG,@Schedule,@Attachments,@MW,@ItemFile,@Notes,@eNB,@OthereNB,@Equipment,@AOTSCR,@USID,@Status,@IsAdditional,		
		@Created,@CreatedBy
		END  
		 
		CLOSE db_sites   
		DEALLOCATE db_sites
	END
	ELSE IF @Filter='Import_WR_Issues' 
	BEGIN

		DECLARE db_issues CURSOR FOR  
		SELECT	l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14,l.Value15,l.Value16,l.Value17,
				l.Value18,l.Value19,l.Value20,l.Value21,l.Value22,l.Value23,l.Value24,l.Value25,l.Value26,l.Value27
				FROM @Data AS l
		OPEN db_issues 

		FETCH NEXT FROM db_issues INTO 	@ENB,@Task,@FACode,@othereNB,@Equipment,@AOTSCR,@IssueCategory,@WItemType,@WhoFix,@IsUnavoidable,@MActivityType,@Alarm,@Severity,@MW,@ItemFile,@ItemFilePath,@Notes,@Status,@AssignedTo,@Priority,
		@Schedule,@Actual,@TargetDate,@RequestedBy,@RequestDate,
		@Created,@CreatedBy 
		
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			SET @ProjectSiteId=0
			SET @ProjectSiteId=(SELECT TOP 1 pps.ProjectSiteId FROM PM_ProjectSites AS pps WHERE pps.FACode=@FACode AND pps.IsActive=1 And ProjectId =@ProjectId)
		IF @ProjectSiteId > 0 
		BEGIN
			SET @TaskId=(Select top 1 t.TaskId  from PM_Tasks t where t.Title=@Task and ProjectId =@ProjectId )
			  
			Set @SiteTaskId = (Select top 1 t.SiteTaskId  from PM_SiteTasks t where t.TaskId=@TaskId and t.ProjectSiteId = @ProjectSiteId)
			   
			IF  @SiteTaskId > 0 
		    BEGIN
			SET @TaskTypeId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1 AND ad.DefinationName=@WItemType AND adt.DefinationType='Task Types') 
		
			SET @PriorityId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1 AND ad.DefinationName=@Priority AND adt.DefinationType='Issue Priority') 
			SET @AssignedToId=(Select top 1 u.UserId  from Sec_Users u where u.UserName=@AssignedTo)

			SET @SeverityId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1 AND ad.DefinationName=@Severity AND adt.DefinationType='Severity') 
			SET @MActivityTypeId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1AND ad.DefinationName=@MActivityType AND adt.DefinationType='Activity Type')
            SET @SeverityId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1AND ad.DefinationName=@Severity AND adt.DefinationType='Severity')
			--SET @ProjectSiteId=(SELECT pps.ProjectSiteId FROM PM_ProjectSites AS pps WHERE pps.FACode=@FACode AND pps.IsActive=1)
			
			--CC Col: Reason in above defs.
				--SET @TaskId=(SELECT TOP 1 TaskId FROM PM_Tasks T  WHERE t.IsActive=1 AND t.Title=@Task ) 
			SET @ReasonId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1 AND ad.DefinationName=@Reasons AND adt.DefinationType='Reasons') 
			SET @MarketId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1 AND ad.DefinationName=@Market AND adt.DefinationType='City')
			SET @MaintenanceWindowId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1 AND ad.DefinationName=@MW AND adt.DefinationType='Milestone Window')   
			SET @StatusId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
											WHERE ad.IsActive=1 AND ad.DefinationName=@Status AND adt.DefinationType='Issue Status')
			SET @AlarmId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
											WHERE ad.IsActive=1 AND ad.DefinationName=@Alarm AND adt.DefinationType='Alarms')
			SET @IssueCategoryId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@IssueCategory AND adt.DefinationType='Issue Category')
			
			--Issues->is: issue type(access,execution etc.)
			SET @IssueTypeId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1 AND ad.DefinationName=@WItemType AND adt.DefinationType='Issue Type')
			
			--WhoFix->is: Issue Owner (nokia, att, unavoid etc.)			
			--SET @IssueOwnerId=(CASE WHEN @WhoFix IN('AT&T','AT&T Unavoid') THEN 3 WHEN @WhoFix IN('Nokia','Nokia Unavoid') THEN 2 ELSE NULL END)

			SET @IssueOwnerId=(SELECT c.ClientId FROM AD_Clients c WHERE c.ClientName=@WhoFix and c.IsActive=1)
			
			--SET @IsUnavoidable = (CASE WHEN @WhoFix IN('AT&T Unavoid','Nokia Unavoid','Unavoid') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)

						
			--ContentType
			SET @ContentTypeId=(SELECT TOP 1  ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
												WHERE ad.IsActive=1 AND ad.DefinationName=@ItemFile AND adt.DefinationType='Item Type')
			
			IF @ReasonId=0
			BEGIN
			    INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
			    SELECT @Reasons,NULL,24,'REASON',0,CAST(1 AS BIT),@Reasons;
			    	
			    SELECT @ReasonId=@@IDENTITY
			END
			
			IF @MaintenanceWindowId=0
			BEGIN
			    INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
			    SELECT @MW,NULL,70059,'MILESTONE WINDOW',0,CAST(1 AS BIT),@MW;
			    	
			    SELECT @MaintenanceWindowId=@@IDENTITY
			END
			
			IF @AlarmId=0
			BEGIN
			    INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
			    SELECT @Alarm,NULL,70062,'ALARMS',0,CAST(1 AS BIT),@Alarm;
			    	
			    SELECT @AlarmId=@@IDENTITY
			END
			
			IF @IssueCategoryId=0
			BEGIN
			    INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
			    SELECT @Status,NULL,70056,'ISSUE CATEGORY',0,CAST(1 AS BIT),@Status;
			    	
			    SELECT @IssueCategoryId=@@IDENTITY
			END
			
			IF @IssueTypeId=0
			BEGIN

			    INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
			    SELECT @Issues,@IssueCategoryId,70057,'ISSUE TYPE',0,CAST(1 AS BIT),@Issues;
			    	
			    SELECT @IssueTypeId=@@IDENTITY
			END
			
			if @TaskTypeId >0
			Begin
		    Set @TaskTypeId =@TaskTypeId
			end
			else
			begin
			set @TaskTypeId = 0
			End

			INSERT INTO PM_Issues(ProjectId,ProjectSiteId,TaskId,TaskTypeId,IssuePriorityId,IssueStatusId,IssueCategoryId,ReasonId,IssueById,[Description],AssignedToId,
			ForecastDate,TargetDate,ActualEndDate,IsUnavoidable,ENB,ExtendedeNB,EquipmentId,AOTSCR,ActivityTypeId,ItemTypeId,ItemFilePath,SeverityId,AlarmId,MSWindowId,RequestedBy,RequestDate,StatusId,Created,CreatedBy)
			 
			SELECT @ProjectId,@ProjectSiteId,@TaskId,@TaskTypeId,@PriorityId,@StatusId,@IssueCategoryId, @IssueTypeId,@IssueOwnerId,@Notes,
			--(CASE WHEN @Schedule!='' OR @Schedule IS NOT NULL THEN CAST(@Schedule AS DATETIME) ELSE NULL END),
			@AssignedToId,
			(CASE WHEN @ForecastEndDate!='' OR @ForecastEndDate IS NOT NULL THEN CAST(@ForecastEndDate AS DATETIME) ELSE NULL END),
			(CASE WHEN 		@TargetDate!='' OR @TargetDate IS NOT NULL THEN CAST(@TargetDate AS DATETIME) ELSE NULL END),
			(CASE WHEN @Actual!='' OR @Actual IS NOT NULL THEN CAST(@Actual AS DATETIME) ELSE NULL END),
			@IsUnavoidable,@ENB,@OtherENB,@EquipmentId,@AOTSCR,NULL,@ContentTypeId,@ItemFilePath,@SeverityId,@AlarmId,@MaintenanceWindowId,@RequestedBy,@RequestDate,@StatusId,@Created,@CreatedBy;
			END
			END
		FETCH NEXT FROM db_issues INTO 	@ENB,@Task,@FACode,@othereNB,@Equipment,@AOTSCR,@IssueCategory,@Reasons,@WhoFix,@IsUnavoidable,@MActivityType,@Alarm,@Severity,@MW,@WItemType,@ItemFilePath,@Notes,@Status,@AssignedTo,@Priority,
		@Schedule,@Actual,@TargetDate,@RequestedBy,@RequestDate,
		@Created,@CreatedBy 
		END   
		CLOSE db_issues   
		DEALLOCATE db_issues
	END
	ELSE IF @Filter='Import_ProjectPlan_Data'
	BEGIN

		--"Value1", p.FACode,
		--"Value2", p.USID,
		--"Value3", p.CommonID,
		--"Value4", p.REGION,
		--"Value5", p.MARKET,
		--"Value6", p.SUBMarket,
		--"Value7", p.SiteName,
		--"Value8", p.Street_Address , "Value9", p.CITY, "Value10", p.State, "Value11", p.ZIP, "Value12", p.COUNTY,
		--"Value13", p.Latitude, "Value14", p.Longitude,
		--"Value15", p.vMME, "Value16", p.ControlledIntroduction, "Value17", p.SuperBowl, "Value18", p.Site_Type, "Value19", p.DAS_or_Inbuilding,"Value20", p.FirstNetRAN,
		--"Value21", p.iPlanJob, "Value22", p.iPlanStatus, "Value23", p.iPlanIssueDate, "Value24", p.PACENumber,	
		--"Value25", p.TSS_Plan, "Value26", p.TSS_Forecast, "Value27", p.TSS_Submitted,
		--"Value28", p.Site_Specific_Material_Available_Forecast, "Value29", p.Site_Specific_Material_Available_Actual, 
		--"Value30", p.Pre_Install_Planned, "Value31", p.Pre_Install_Fcst, "Value32", p.Pre_Install_Actual,
		--"Value33", p.Mig_Date_Planned, "Value34", p.Mig_Date_Forecast, "Value35", p.MigrationDate,
		--"Value36", p.EPLOrdered, "Value37", p.EPLCalledOut, "Value38", p.EPLDelivered, "Value39", p.EPLStatus
		
		TRUNCATE TABLE FSM4_Sites

		insert into FSM4_Sites(FA_Code,USID,[Common ID],REGION,MARKET,[SUB Market],[Site Name],Street_Address,CITY,State,ZIP,COUNTY,
		Latitude,Longitude,vMME,[Controlled Introduction],[Super Bowl],Site_Type,DAS_or_Inbuilding,[FirstNet RAN],
		[iPlan Job], [iPlan Status], [iPlan Issue Date], [PACE Number],TSS_Plan, TSS_Forecast, TSS_Submitted,
		Site_Specific_Material_Available_Forecast, Site_Specific_Material_Available_Actual,
		Pre_Install_Planned, Pre_Install_Fcst, Pre_Install_Actual, Mig_Date_Planned, Mig_Date_Forecast, [Migration Date])
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14,l.Value15,l.Value16,l.Value17,
		l.Value18,l.Value19,l.Value20,l.Value21,l.Value22,l.Value23,l.Value24,l.Value25,l.Value26,l.Value27,l.Value28,l.Value29,l.Value30,l.Value31,l.Value32,l.Value33,l.Value34,l.Value35
		FROM @Data AS l

		--VALUES(@FACode,@USID,@ENB,@Region,@Market,@SubMarket,@StreetAddress,@City,@State,@ZIP,@County,@Latitude,@Longitude,@vMME,@ControlledIntro,@SuperBowl,@DASInbuilding,@FirstNetRAN,
		--@iPlanJob,@iPlanStatus,@PACENumber,@iPlanIssueDate,@TSSPlan,@TSSForecast,@TSSSubmitted,@SSMForecast,@SSMActual,@PreInstPlan,@PreInstForecast,@PreInstSubmitted,
		--@MigrationPlan,@MigrationForecast,@MigrationSubmitted)
						
		--DECLARE db_cluster2 CURSOR FOR  
		--SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14,l.Value15,l.Value16,l.Value17,
		--l.Value18,l.Value19,l.Value20,l.Value21,l.Value22,l.Value23,l.Value24,l.Value25,l.Value26,l.Value27,l.Value28,l.Value29,l.Value30,l.Value31,l.Value32,l.Value33,l.Value34,
		--l.Value35,l.Value36,l.Value37,l.Value38,l.Value39
		--FROM @Data AS l
		--OPEN db_cluster2 
		--FETCH NEXT FROM db_cluster2 INTO @FACode,@USID,@ENB,@Region,@Market,@SubMarket,@SiteName,@StreetAddress,@City,@State,@ZIP,@County,@Latitude,@Longitude,
		--@vMME,@ControlledIntro,@SuperBowl,@SiteType,@DASInbuilding,@FirstNetRAN,@iPlanJob,@iPlanStatus,@iPlanIssueDate,@PACENumber,@TSSPlan,@TSSForecast,@TSSSubmitted,
		--@SSMForecast,@SSMActual,@PreInstPlan,@PreInstForecast,@PreInstSubmitted,@MigrationPlan,@MigrationForecast,@MigrationSubmitted,@EPLOrdered,@EPLCalledOut,@EPLDelivered,@EPLStatus
		--WHILE @@FETCH_STATUS = 0   
		--BEGIN
		
		--	SET @ProjectSiteId=0
		--	SET @ProjectSiteId=ISNULL((SELECT pps.ProjectSiteId FROM PM_ProjectSites AS pps WHERE pps.FACode=@FACode AND pps.IsActive=1),0)	
		
		--	IF @ProjectSiteId=0
		--	BEGIN
		--		SET @RegionId=ISNULL((SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
		--	                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Region AND adt.DefinationType='Region'),0)
		--	    SET @MarketId=ISNULL((SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
		--	                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Market AND adt.DefinationType='City'),0)
		--	    SET @SiteTypeId=ISNULL((SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
		--	                                    WHERE ad.IsActive=1 AND ad.DefinationName=@SiteType AND adt.DefinationType='Site Type'),0)
			                                    
		--	    IF @RegionId=0
		--	    BEGIN
		--	    	INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
		--	    	SELECT @Region,163401,6,'REGION',0,CAST(1 AS BIT),@Region;
			    	
		--	    	SELECT @RegionId=@@IDENTITY
		--	    END
			    
		--	    IF @MarketId=0
		--	    BEGIN
		--	    	INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
		--	    	SELECT @Market,@RegionId,7,'MARKET',0,CAST(1 AS BIT),@Market;
			    	
		--	    	SELECT @MarketId=@@IDENTITY
		--	    END
			    
		--	    IF @SiteTypeId=0
		--	    BEGIN
		--	    	INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
		--	    	SELECT @SiteType,@RegionId,28,'SITE_TYPE',0,CAST(1 AS BIT),@SiteType;
			    	
		--	    	SELECT @SiteTypeId=@@IDENTITY
		--	    END
			    
		--	    DECLARE @maxWoID as int=0
		--		DECLARE @woRefID as nvarchar(25)
				
		--		SET @maxWoID=(Select ISNULL(MAX(PMCode),0) + 1 from PM_ProjectSites x Where ClientId=2 AND (YEAR(GETDATE())* 100 + MONTH(GETDATE()) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))					
		--		SET @woRefID = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientId=2)+ '-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
		--		RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@maxWoID as nvarchar(15)),5))
			    
		--	    --SELECT (CASE WHEN @IPlanIssueDate!='' THEN CAST(@IPlanIssueDate AS DATETIME) ELSE NULL END)
		--	    INSERT INTO [dbo].[PM_ProjectSites]([ProjectId],[SiteCode],[ReceivedOn],[SiteTypeId],[ClusterCode],[CityId],[ColorId],[CreatedOn],[CreatedBy],[IsActive],[Latitude],[Longitude],
		--		[Description],[ClientId],SiteName,PMRefId,PMCode,RevisionId,USID,
		--		SubMarket, vMME, ControlledIntro, SuperBowl, isDASInBuild, FirstNetRAN,
		--		IPlanJob, PaceNo, IPlanIssueDate,SiteDate,FACode)
		--		VALUES(20021, @FACode, (CASE WHEN @TSSPlan!='' THEN CAST(@TSSPlan AS DATETIME) ELSE NULL END), @SiteTypeId, NULL, @MarketId, '#ffffff',(CASE WHEN @TSSPlan!='' THEN CAST(@TSSPlan AS DATETIME) ELSE NULL END), 11, CAST(1 AS BIT), @Latitude, @Longitude,
		--		NULL, 2, @SiteName,@woRefID,@maxWoID,0,@USID, @SubMarket, CAST(@VMME AS BIT), CAST(@ControlledIntro AS BIT), CAST(@SuperBowl AS BIT), @DASInbuilding,
		--		@FirstNetRAN, @iPlanJob, @PACENumber, (CASE WHEN @IPlanIssueDate!='' THEN CAST(@IPlanIssueDate AS DATETIME) ELSE NULL END),
		--		(CASE WHEN @TSSPlan!='' THEN CAST(@TSSPlan AS DATETIME) ELSE NULL END),@FACode)
				
				
				
		--		SELECT @ProjectSiteId=@@IDENTITY
				
				
		--		SET @newFACodes = @newFACodes+ @FACode+','
				
		--		--TSS
		--		INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,PlannedDate,ActualEndDate,IsActive)
		--		SELECT @ProjectSiteId,50076,0,
		--		(CASE WHEN @TSSPlan ='' AND @TSSForecast ='' AND @TSSSubmitted ='' THEN 133317
		--							 WHEN @TSSPlan !='' AND @TSSForecast ='' AND @TSSSubmitted ='' THEN 133318
		--							 WHEN @TSSForecast !='' AND @TSSSubmitted ='' THEN 133319
		--							 WHEN @TSSSubmitted !='' THEN 163395
		--							 ELSE 133317 END),133315,CAST(CONVERT(NVARCHAR(10),@TSSForecast,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@TSSPlan,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@TSSSubmitted,102) AS DATETIME),CAST(1 AS BIT)
				
		--		--SSM
		--		INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,ActualEndDate,IsActive)
		--		SELECT @ProjectSiteId,50077,0,
		--		(CASE WHEN @SSMForecast ='' AND @SSMActual ='' THEN 133317
		--							 WHEN @SSMForecast !='' AND @SSMActual ='' THEN 133319
		--							 WHEN @SSMActual !='' THEN 163395
		--							 ELSE 133317 END),133315,CAST(CONVERT(NVARCHAR(10),@SSMForecast,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@SSMActual,102) AS DATETIME),CAST(1 AS BIT)
									 
				
		--		--Pre-Install
		--		INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,PlannedDate,ActualEndDate,IsActive)
		--		SELECT @ProjectSiteId,50079,0,
		--		(CASE WHEN @PreInstPlan ='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133317
		--							 WHEN @PreInstPlan !='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133318
		--							 WHEN @PreInstForecast !='' AND @PreInstSubmitted ='' THEN 133319
		--							 WHEN @PreInstSubmitted !='' THEN 163395
		--							 ELSE 133317 END),133315,CAST(CONVERT(NVARCHAR(10),@PreInstForecast,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@PreInstPlan,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@PreInstSubmitted,102) AS DATETIME),CAST(1 AS BIT)
									 
				
		--		--Migrations
		--		INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,PlannedDate,ActualEndDate,IsActive)
		--		SELECT @ProjectSiteId,50080,0,
		--		(CASE WHEN @MigrationPlan ='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133317
		--							 WHEN @MigrationPlan !='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133318
		--							 WHEN @MigrationForecast !='' AND @MigrationSubmitted ='' THEN 133319
		--							 WHEN @MigrationSubmitted !='' THEN 163395
		--							 ELSE 133317 END),133315,CAST(CONVERT(NVARCHAR(10),@MigrationForecast,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@MigrationPlan,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@MigrationSubmitted,102) AS DATETIME),CAST(1 AS BIT)
		--	END
		--	ELSE
		--	IF @ProjectSiteId>0    
		--	BEGIN                                
		--		--TSS
		--		UPDATE PM_SiteTasks
		--		SET	StatusId = (CASE WHEN @TSSPlan='' AND @TSSForecast='' AND @TSSSubmitted='' THEN 133317
		--							 WHEN @TSSPlan!='' AND @TSSForecast='' AND @TSSSubmitted='' THEN 133318
		--							 WHEN @TSSForecast!='' AND @TSSSubmitted='' THEN 133319
		--							 WHEN @TSSSubmitted!='' THEN 163395
		--							 ELSE 133317 END),
		--			PlannedDate = (CASE WHEN (@TSSPlan='' OR @TSSPlan IS NULL) then NULL ELSE CAST(CONVERT(NVARCHAR(10),@TSSPlan,102) AS DATETIME) END),
		--			EstimatedStartDate = (CASE WHEN (@TSSForecast='' OR @TSSForecast IS NULL) then NULL ELSE CAST(CONVERT(NVARCHAR(10),@TSSForecast,102) AS DATETIME) END),	
		--			ActualEndDate = (CASE WHEN (@TSSSubmitted='' OR @TSSSubmitted IS NULL) then NULL ELSE CAST(CONVERT(NVARCHAR(10),@TSSSubmitted,102) AS DATETIME) END)
		--		WHERE SiteTaskId=50076;
			
		--		--SSM
		--		UPDATE PM_SiteTasks
		--		SET	StatusId = (CASE WHEN @SSMForecast='' AND @SSMActual='' THEN 133317
		--							 WHEN @SSMForecast !='' AND @SSMActual='' THEN 133319
		--							 WHEN @SSMActual !='' THEN 163395
		--							 ELSE 133317 END),
		--			EstimatedStartDate = (CASE WHEN (@SSMForecast='' OR @SSMForecast IS NULL) then NULL ELSE CAST(CONVERT(NVARCHAR(10),@SSMForecast,102) AS DATETIME) END),	
		--			ActualEndDate = (CASE WHEN (@SSMActual='' OR @SSMActual IS NULL) then NULL ELSE CAST(CONVERT(NVARCHAR(10),@SSMActual,102) AS DATETIME) END)
		--		WHERE SiteTaskId=50077;
			
		--		--Pre Install
		--		UPDATE PM_SiteTasks
		--		SET	StatusId = (CASE WHEN @PreInstPlan ='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133317
		--							 WHEN @PreInstPlan !='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133318
		--							 WHEN @PreInstForecast !='' AND @PreInstSubmitted ='' THEN 133319
		--							 WHEN @PreInstSubmitted !='' THEN 163395
		--							 ELSE 133317 END),
		--			PlannedDate = (CASE WHEN (@PreInstPlan='' OR @PreInstPlan IS NULL) then NULL ELSE CAST(CONVERT(NVARCHAR(10),@PreInstPlan,102) AS DATETIME) END),
		--			EstimatedStartDate = (CASE WHEN (@PreInstForecast='' OR @PreInstForecast IS NULL) then NULL ELSE CAST(CONVERT(NVARCHAR(10),@PreInstForecast,102) AS DATETIME) END),	
		--			ActualEndDate = (CASE WHEN (@PreInstSubmitted='' OR @PreInstSubmitted IS NULL) then NULL ELSE CAST(CONVERT(NVARCHAR(10),@PreInstSubmitted,102) AS DATETIME) END)
		--		WHERE SiteTaskId=50079;
			
		--		--Migrations
		--		UPDATE PM_SiteTasks
		--		SET	StatusId = (CASE WHEN @MigrationPlan ='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133317
		--							 WHEN @MigrationPlan !='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133318
		--							 WHEN @MigrationForecast !='' AND @MigrationSubmitted ='' THEN 133319
		--							 WHEN @MigrationSubmitted !='' THEN 163395
		--							 ELSE 133317 END),
		--			PlannedDate = (CASE WHEN (@MigrationPlan='' OR @MigrationPlan IS NULL) then NULL ELSE CAST(CONVERT(NVARCHAR(10),@MigrationPlan,102) AS DATETIME) END),
		--			EstimatedStartDate = (CASE WHEN (@MigrationForecast='' OR @MigrationForecast IS NULL) then NULL ELSE CAST(CONVERT(NVARCHAR(10),@MigrationForecast,102) AS DATETIME) END),	
		--			ActualEndDate = (CASE WHEN (@MigrationSubmitted='' OR @MigrationSubmitted IS NULL) then NULL ELSE CAST(CONVERT(NVARCHAR(10),@MigrationSubmitted,102) AS DATETIME) END)
		--		WHERE SiteTaskId=50080;

		--		--IF @MigrationSubmitted='2018-03-01'
		--		--BEGIN
		--		--	SET @MigCount+=1;
		--		--	--SELECT 'Mig',@MigCount;
		--		--END
		--		--ELSE
		--		--BEGIN
		--		--	SET @nMigCount+=1;
		--		--	--SELECT 'nMig',@nMigCount
		--		----END
		--		--DECLARE @tblName as nvarchar(50)='tmp'+CAST((SELECT FLOOR(RAND()*(25-10)+10)) as nvarchar(10))
		--		--EXEC('SELECT * INTO ' +  @tblName + ' FROM  @Data')

		--	END
		--FETCH NEXT FROM db_cluster2 INTO @FACode,@USID,@ENB,@Region,@Market,@SubMarket,@SiteName,@StreetAddress,@City,@State,@ZIP,@County,@Latitude,@Longitude,
		--@vMME,@ControlledIntro,@SuperBowl,@SiteType,@DASInbuilding,@FirstNetRAN,@iPlanJob,@iPlanStatus,@iPlanIssueDate,@PACENumber,@TSSPlan,@TSSForecast,@TSSSubmitted,
		--@SSMForecast,@SSMActual,@PreInstPlan,@PreInstForecast,@PreInstSubmitted,@MigrationPlan,@MigrationForecast,@MigrationSubmitted,@EPLOrdered,@EPLCalledOut,@EPLDelivered,@EPLStatus
		--END   
		--CLOSE db_cluster2   
		--DEALLOCATE db_cluster2
		
		----IF @newFACodes=''
		----BEGIN
		
		----END
		----ELSE
		----SELECT @MigCout
		
		--IF @newFACodes!=''
		--BEGIN
		--	SELECT x.Value1 'FACode', x.Value5 'MARKET', Value25 'TSSPlan', Value26 'TSSForecast', Value27 'TSSSubmitted',
		--	Value28 'SiteSpecificMaterialAvailableForecast', Value29 'SiteSpecificMaterialAvailableActual', 
		--	Value30 'PreInstallPlanned', Value31 'PreInstallFcst', Value32 'PreInstallActual',
		--	Value33 'MigDatePlanned', Value34 'MigDateForecast', Value35 'MigrationDate','New' 'Status'
		--	FROM @Data x
		--	WHERE Charindex(cast(x.Value1 as varchar(max))+',', @newFACodes) > 0
		--	--UNION ALL
		--	--SELECT pps.FACode 'FACode', ad.DefinationName 'MARKET', NULL 'TSSPlan', NULL 'TSSForecast', NULL 'TSSSubmitted',
		--	--NULL 'SiteSpecificMaterialAvailableForecast', NULL 'SiteSpecificMaterialAvailableActual', 
		--	--NULL 'PreInstallPlanned', NULL 'PreInstallFcst', NULL 'PreInstallActual',
		--	--NULL 'MigDatePlanned', NULL 'MigDateForecast', NULL 'MigrationDate','Delete' 'Status'
		--	--FROM PM_ProjectSites AS pps 
		--	--INNER JOIN AD_Definations AS ad ON ad.DefinationId=pps.CityId
		--	--WHERE pps.IsActive=1
		--	--AND pps.FACode NOT IN(SELECT x.Value1 FROM @Data x)
			
		--END
		----ELSE
		----BEGIN
		----	SELECT pps.FACode 'FACode', ad.DefinationName 'MARKET', NULL 'TSSPlan', NULL 'TSSForecast', NULL 'TSSSubmitted',
		----	NULL 'SiteSpecificMaterialAvailableForecast', NULL 'SiteSpecificMaterialAvailableActual', 
		----	NULL 'PreInstallPlanned', NULL 'PreInstallFcst', NULL 'PreInstallActual',
		----	NULL 'MigDatePlanned', NULL 'MigDateForecast', NULL 'MigrationDate','Delete' 'Status'
		----	FROM PM_ProjectSites AS pps 
		----	INNER JOIN AD_Definations AS ad ON ad.DefinationId=pps.CityId
		----	WHERE pps.IsActive=1
		----	AND pps.FACode NOT IN(SELECT x.Value1 FROM @Data x)
		----END
			
	END
	ELSE IF @Filter='Save_Project_Plan'
	BEGIN
		--DECLARE addSite CURSOR READ_ONLY
		--FOR
		--SELECT x.FA_Code, x.USID, x.[Common ID], x.REGION, x.MARKET, x.[SUB Market],
		--	   x.[Site Name], x.Street_Address, x.CITY, x.[State], x.ZIP, x.COUNTY,
		--	   x.Latitude, x.Longitude, x.vMME, x.[Controlled Introduction],
		--	   x.[Super Bowl], x.Site_Type, x.DAS_or_Inbuilding, x.[FirstNet RAN],
		--	   x.[iPlan Job], x.[iPlan Status], x.[PACE Number], x.[iPlan Issue Date],
		--	   x.TSS_Plan, x.TSS_Forecast, x.TSS_Submitted,
		--	   x.Site_Specific_Material_Available_Forecast,
		--	   x.Site_Specific_Material_Available_Actual, x.Pre_Install_Planned,
		--	   x.Pre_Install_Fcst, x.Pre_Install_Actual, x.Mig_Date_Planned,
		--	   x.Mig_Date_Forecast, x.[Migration Date]
		--FROM FSM4_Sites x
		----OPEN CURSOR
		--OPEN addSite 
		----FETCH THE RECORD INTO THE VARIABLES.
		--FETCH NEXT FROM addSite INTO @FACode,@USID,@ENB,@REGION,@MARKET,@SUBMarket,@SiteName,@StreetAddress,@CITY,@State,@ZIP,@COUNTY,@Latitude,@Longitude,@vMME,@ControlledIntro,@SuperBowl, 
		--@SiteType,@DASInbuilding,@FirstNetRAN,@iPlanJob,@iPlanStatus,@PACENumber,@iPlanIssueDate,@TSSPlan,@TSSForecast,@TSSSubmitted,@SSMForecast,
		--@SSMActual,@PreInstPlan,@PreInstForecast,@PreInstSubmitted,@MigrationPlan,@MigrationForecast,@MigrationSubmitted
		----LOOP UNTIL RECORDS ARE AVAILABLE.
		--WHILE @@FETCH_STATUS = 0
		--BEGIN
		--	--BEGIN TRANSACTION	
		--		DECLARE @maxWoID as int=0
		--		DECLARE @woRefID as nvarchar(25)
		--		SET @SiteTypeId=ISNULL((SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationName=@SiteType AND ad.DefinationTypeId=28 AND ad.IsActive=1),3132)
		--		DECLARE @SiteClassId AS NUMERIC(18,0)=3133
		--		DECLARE @CityId AS NUMERIC(18,0)=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationName=@MARKET AND ad.DefinationTypeId=7 AND ad.IsActive=1)
		--		SET @StatusId=163384
		--		DECLARE @MSWindowId AS NUMERIC(18,0)=NULL
		--		DECLARE @PriorityId AS NUMERIC(18,0)=133315
		--		DECLARE @ClientId AS NUMERIC(18,0)=(SELECT TOP 1 pp.ClientId FROM PM_Projects AS pp WHERE pp.ProjectId=@ProjectId)
		--		DECLARE @ScopeId AS NUMERIC(18,0)=153347
				
		--		SET @maxWoID=(Select ISNULL(MAX(PMCode),0) + 1 from PM_ProjectSites x WHERE x.ProjectId=@ProjectId AND ClientId=@ClientId AND (YEAR(x.CreatedOn)* 100 + MONTH(x.CreatedOn) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))					
		--		SET @woRefID = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientId=@ClientId)+ '-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
		--		RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@maxWoID as nvarchar(15)),5))
					
		--		INSERT INTO PM_ProjectSites(WoRefId,ProjectId,SiteCode,SiteName,SiteDate,SiteTypeId,SiteClassId,Latitude,Longitude,RevisionId,PMCode,PMRefId,ClusterId,ClusterCode,
		--		CityId,StatusId,MSWindowId,PriorityId,ColorId,CreatedOn,CreatedBy,IsActive,BudgetCost,ActualCost,[Description],ClientId,ScopeId,ReceivedOn,[Address],
		--		MilestoneId,StageId,FACode,USID,CommonId,MarketId,SubMarketId,CountyId,vMME,ControlledIntro,
		--		SuperBowl,isDASInBuild,FirstNetRAN,IPlanJob,PaceNo,SubMarket,County)		
		--		SELECT @woRefID,@ProjectId,CAST(@FACode as nvarchar(50)) 'SiteCode',@SiteName,@TSSPlan 'SiteDate',@SiteTypeId,@SiteClassId,@Latitude,@Longitude,0 'Revsion',@maxWoID 'PMCode',@woRefID 'PMRefId',NULL 'ClusterId','' 'ClusterCode',
		--		@CityId,@StatusId,@MSWindowId,@PriorityId,NULL 'ColorId',GETDATE() 'CreatedOn',11 'CreatedBy',CAST(1 AS BIT) 'IsActive',0 'BudgetCost',0 'ActualCost','' 'Description',@ClientId,
		--		@ScopeId,@TSSPlan 'ReceivedOn',ISNULL(@StreetAddress,'')+', '+ISNULL(@CITY,'')+', '+ISNULL(@COUNTY,'')+', '+ISNULL(@State,'')+', '+ISNULL(@ZIP,'') 'Address',
		--		NULL 'MileStoneId',NULL 'StageId',CAST(@FACode as nvarchar(50)),@USID,@ENB,@CityId 'MarketId',NULL 'SubMarketId',NULL 'CountyId',@vMME,@ControlledIntro,
		--		@SuperBowl,@DASInbuilding,@FirstNetRAN,@iPlanJob,@PACENumber,@SUBMarket,@COUNTY
		
			
		--		DECLARE @ProjectSiteId AS NUMERIC(18,0)=0
		--		SELECT @ProjectSiteId=@@IDENTITY;			
			
		--		--SELECT 'Site_Inserted',@ProjectSiteId,@FA_Code
			
		--		INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,EstimatedEndDate,PlannedDate,TargetDate,ActualStartDate,ActualEndDate,
		--		CompletionPercent,BudgetCost,ActualCost,IsActive)
		--		SELECT @ProjectSiteId,pt.TaskId,0 'RevisionId',pt.StatusId, pt.PriorityId,
		--				@TSSForecast 'EstimatedStartDate', NULL 'EstimatedEndDate', @TSSPlan 'PlannedDate',
		--				NULL 'TargetDate', NULL 'ActualStartDate', @TSSSubmitted 'ActualEndDate',0 'CompletionPercent',pt.BudgetCost,0 'ActualCost',pt.IsActive
		--		FROM PM_Tasks AS pt
		--		WHERE pt.ProjectId=@ProjectId AND pt.IsActive=1 and pt.TaskId=50076
		--		UNION ALL
		--		SELECT @ProjectSiteId,pt.TaskId,0 'RevisionId',pt.StatusId, pt.PriorityId,
		--				@Site_Specific_Material_Available_Forecast 'EstimatedStartDate', NULL 'EstimatedEndDate', NULL 'PlannedDate',
		--				NULL 'TargetDate', NULL 'ActualStartDate', @Site_Specific_Material_Available_Actual 'ActualEndDate',0 'CompletionPercent',pt.BudgetCost,0 'ActualCost',pt.IsActive
		--		FROM PM_Tasks AS pt
		--		WHERE pt.ProjectId=@ProjectId AND pt.IsActive=1 and pt.TaskId=50077
		--		UNION ALL
		--		SELECT @ProjectSiteId,pt.TaskId,0 'RevisionId',pt.StatusId, pt.PriorityId,
		--				NULL 'EstimatedStartDate', NULL 'EstimatedEndDate', NULL 'PlannedDate',
		--				NULL 'TargetDate', NULL 'ActualStartDate', NULL 'ActualEndDate',0 'CompletionPercent',pt.BudgetCost,0 'ActualCost',pt.IsActive
		--		FROM PM_Tasks AS pt
		--		WHERE pt.ProjectId=@ProjectId AND pt.IsActive=1 and pt.TaskId IN(50078,50081,50082,50083)
		--		UNION ALL
		--		SELECT @ProjectSiteId,pt.TaskId,0 'RevisionId',pt.StatusId, pt.PriorityId,
		--				@Pre_Install_Fcst 'EstimatedStartDate', NULL 'EstimatedEndDate', @Pre_Install_Planned 'PlannedDate',
		--				NULL 'TargetDate', NULL 'ActualStartDate', @Pre_Install_Actual 'ActualEndDate',0 'CompletionPercent',pt.BudgetCost,0 'ActualCost',pt.IsActive
		--		FROM PM_Tasks AS pt
		--		WHERE pt.ProjectId=@ProjectId AND pt.IsActive=1 and pt.TaskId=50079
		--		UNION ALL
		--		SELECT @ProjectSiteId,pt.TaskId,0 'RevisionId',pt.StatusId, pt.PriorityId,
		--				@Mig_Date_Forecast 'EstimatedStartDate', NULL 'EstimatedEndDate', @Mig_Date_Planned 'PlannedDate',
		--				NULL 'TargetDate', NULL 'ActualStartDate', @Migration_Date_Actual 'ActualEndDate',0 'CompletionPercent',pt.BudgetCost,0 'ActualCost',pt.IsActive
		--		FROM PM_Tasks AS pt
		--		WHERE pt.ProjectId=@ProjectId AND pt.IsActive=1 and pt.TaskId=50080
		
		
		--		----MS: TSS
		--		--UPDATE PM_SiteTasks
		--		--SET EstimatedStartDate = @TSSForecast, PlannedDate = @TSSPlan, ActualEndDate = @TSSSubmitted 
		--		--WHERE ProjectSiteId=@ProjectSiteId AND TaskId=50076
		
		--		----MS: SSM
		--		--UPDATE PM_SiteTasks
		--		--SET EstimatedStartDate = @Site_Specific_Material_Available_Forecast, ActualEndDate = @Site_Specific_Material_Available_Actual 
		--		--WHERE ProjectSiteId=@ProjectSiteId AND TaskId=50077
		
		--		----MS: PreInstall
		--		--UPDATE PM_SiteTasks
		--		--SET EstimatedStartDate = @Pre_Install_Fcst, PlannedDate = @Pre_Install_Planned, ActualEndDate = @Pre_Install_Actual 
		--		--WHERE ProjectSiteId=@ProjectSiteId AND TaskId=50079
		
		--		--MS: Migration
		--		--UPDATE PM_SiteTasks
		--		--SET EstimatedStartDate = @Mig_Date_Forecast, PlannedDate = @Mig_Date_Planned, ActualEndDate = @Migration_Date_Actual 
		--		--WHERE ProjectSiteId=@ProjectSiteId AND TaskId=50080
		
		--		--SELECT @ProjectSiteId,@FA_Code
		--	--COMMIT
		--	--FETCH THE NEXT RECORD INTO THE VARIABLES.
		--	FETCH NEXT FROM addSite INTO @FA_Code,@USID,@CommonID,@REGION,@MARKET,@SUBMarket,@SiteName,@StreetAddress,@CITY,@State,@ZIP,@COUNTY,@Latitude,@Longitude,@vMME,@ControlledIntroduction,@SuperBowl, 
		--@SiteType,@DASorInbuilding,@FirstNetRAN,@iPlanJob,@iPlanStatus,@PACENumber,@iPlanIssue,@TSSPlan,@TSSForecast,@TSSSubmitted,@Site_Specific_Material_Available_Forecast,
		--@Site_Specific_Material_Available_Actual,@Pre_Install_Planned,@Pre_Install_Fcst,@Pre_Install_Actual,@Mig_Date_Planned,@Mig_Date_Forecast,@Migration_Date_Actual
		--END 
		----CLOSE THE CURSOR.
		--CLOSE addSite
		--DEALLOCATE addSite
		
		DECLARE db_cluster2 CURSOR FOR  
		SELECT x.FA_Code, x.USID, x.[Common ID], x.REGION, x.MARKET, x.[SUB Market],
			   x.[Site Name], x.Street_Address, x.CITY, x.[State], x.ZIP, x.COUNTY,
			   x.Latitude, x.Longitude, x.vMME, x.[Controlled Introduction],
			   x.[Super Bowl], x.Site_Type, x.DAS_or_Inbuilding, x.[FirstNet RAN],
			   x.[iPlan Job], x.[iPlan Status], x.[PACE Number], x.[iPlan Issue Date],
			   x.TSS_Plan, x.TSS_Forecast, x.TSS_Submitted,
			   x.Site_Specific_Material_Available_Forecast,
			   x.Site_Specific_Material_Available_Actual, x.Pre_Install_Planned,
			   x.Pre_Install_Fcst, x.Pre_Install_Actual, x.Mig_Date_Planned,
			   x.Mig_Date_Forecast, x.[Migration Date]
		FROM FSM4_Sites x
		OPEN db_cluster2 
		FETCH NEXT FROM db_cluster2 INTO @FACode,@USID,@ENB,@Region,@Market,@SubMarket,@SiteName,@StreetAddress,@City,@State,@ZIP,@County,@Latitude,@Longitude,
		@vMME,@ControlledIntro,@SuperBowl,@SiteType,@DASInbuilding,@FirstNetRAN,@iPlanJob,@iPlanStatus,@iPlanIssueDate,@PACENumber,@TSSPlan,@TSSForecast,@TSSSubmitted,
		@SSMForecast,@SSMActual,@PreInstPlan,@PreInstForecast,@PreInstSubmitted,@MigrationPlan,@MigrationForecast,@MigrationSubmitted
		WHILE @@FETCH_STATUS = 0   
		BEGIN
		
			SET @ProjectSiteId=0
			SET @ProjectSiteId=ISNULL((SELECT pps.ProjectSiteId FROM PM_ProjectSites AS pps WHERE pps.FACode=@FACode AND pps.IsActive=1),0)	
		
			IF @ProjectSiteId=0
			BEGIN
				SET @RegionId=ISNULL((SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Region AND adt.DefinationType='Region'),0)
			    SET @MarketId=ISNULL((SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Market AND adt.DefinationType='City'),0)
			    SET @SiteTypeId=ISNULL((SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@SiteType AND adt.DefinationType='Site Type'),0)
			                                    
			    IF @RegionId=0
			    BEGIN
			    	INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
			    	SELECT @Region,163401,6,'REGION',0,CAST(1 AS BIT),@Region;
			    	
			    	SELECT @RegionId=@@IDENTITY
			    END
			    
			    IF @MarketId=0
			    BEGIN
			    	INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
			    	SELECT @Market,@RegionId,7,'MARKET',0,CAST(1 AS BIT),@Market;
			    	
			    	SELECT @MarketId=@@IDENTITY
			    END
			    
			    IF @SiteTypeId=0
			    BEGIN
			    	INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,SortOrder,IsActive,DisplayText)
			    	SELECT @SiteType,@RegionId,28,'SITE_TYPE',0,CAST(1 AS BIT),@SiteType;
			    	
			    	SELECT @SiteTypeId=@@IDENTITY
			    END
			    
			    SELECT @SiteTypeId
			    
			    DECLARE @maxWoID as int=0
				DECLARE @woRefID as nvarchar(25)
				
				SET @maxWoID=(Select ISNULL(MAX(PMCode),0) + 1 from PM_ProjectSites x Where ClientId=2 AND (YEAR(GETDATE())* 100 + MONTH(GETDATE()) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))					
				SET @woRefID = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientId=2)+ '-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
				RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@maxWoID as nvarchar(15)),5))
			    
			    --SELECT (CASE WHEN @IPlanIssueDate!='' THEN CAST(@IPlanIssueDate AS DATETIME) ELSE NULL END)
			    INSERT INTO [dbo].[PM_ProjectSites]([ProjectId],[SiteCode],[ReceivedOn],[SiteTypeId],[ClusterCode],[CityId],[ColorId],[CreatedOn],[CreatedBy],[IsActive],[Latitude],[Longitude],
				[Description],[ClientId],SiteName,PMRefId,PMCode,RevisionId,USID,
				SubMarket, vMME, ControlledIntro, SuperBowl, isDASInBuild, FirstNetRAN,
				IPlanJob, PaceNo, IPlanIssueDate,SiteDate,FACode)
				VALUES(@ProjectId, @FACode, (CASE WHEN @TSSPlan!='' THEN CAST(@TSSPlan AS DATETIME) ELSE NULL END), @SiteTypeId, NULL, @MarketId, '#ffffff',(CASE WHEN @TSSPlan!='' THEN CAST(@TSSPlan AS DATETIME) ELSE NULL END), 11, CAST(1 AS BIT), @Latitude, @Longitude,
				NULL, 2, @SiteName,@woRefID,@maxWoID,0,@USID, @SubMarket, CAST(@VMME AS BIT), CAST(@ControlledIntro AS BIT), CAST(@SuperBowl AS BIT), @DASInbuilding,
				@FirstNetRAN, @iPlanJob, @PACENumber, (CASE WHEN @IPlanIssueDate!='' THEN CAST(@IPlanIssueDate AS DATETIME) ELSE NULL END),
				(CASE WHEN @TSSPlan!='' THEN CAST(@TSSPlan AS DATETIME) ELSE NULL END),@FACode)
				
				
				
				SELECT @ProjectSiteId=@@IDENTITY
				
				
				SET @newFACodes = @newFACodes+ @FACode+','
				
				--SELECT @TSSPlan
				
				--TSS
				INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,PlannedDate,ActualEndDate,IsActive)
				SELECT @ProjectSiteId,50076,0,
				(CASE WHEN @TSSPlan ='' AND @TSSForecast ='' AND @TSSSubmitted ='' THEN 133317
									 WHEN @TSSPlan !='' AND @TSSForecast ='' AND @TSSSubmitted ='' THEN 133318
									 WHEN @TSSForecast !='' AND @TSSSubmitted ='' THEN 133319
									 WHEN @TSSSubmitted !='' THEN 163395
									 ELSE 133317 END),133315,
									 CAST(@TSSForecast AS DATETIME),CAST(@TSSPlan AS DATETIME),CAST(@TSSSubmitted AS DATETIME),CAST(1 AS BIT)
				
				--SSM
				INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,ActualEndDate,IsActive)
				SELECT @ProjectSiteId,50077,0,
				(CASE WHEN @SSMForecast ='' AND @SSMActual ='' THEN 133317
									 WHEN @SSMForecast !='' AND @SSMActual ='' THEN 133319
									 WHEN @SSMActual !='' THEN 163395
									 ELSE 133317 END),133315,
									 CAST(@SSMForecast AS DATETIME),CAST(@SSMActual AS DATETIME),CAST(1 AS BIT)
									 
				
				--Pre-Install
				INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,PlannedDate,ActualEndDate,IsActive)
				SELECT @ProjectSiteId,50079,0,
				(CASE WHEN @PreInstPlan ='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133317
									 WHEN @PreInstPlan !='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133318
									 WHEN @PreInstForecast !='' AND @PreInstSubmitted ='' THEN 133319
									 WHEN @PreInstSubmitted !='' THEN 163395
									 ELSE 133317 END),133315,
									 CAST(@PreInstForecast AS DATETIME),CAST(@PreInstPlan AS DATETIME),CAST(@PreInstSubmitted AS DATETIME),CAST(1 AS BIT)
									 
				
				--Migrations
				INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,PlannedDate,ActualEndDate,IsActive)
				SELECT @ProjectSiteId,50080,0,
				(CASE WHEN @MigrationPlan ='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133317
									 WHEN @MigrationPlan !='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133318
									 WHEN @MigrationForecast !='' AND @MigrationSubmitted ='' THEN 133319
									 WHEN @MigrationSubmitted !='' THEN 163395
									 ELSE 133317 END),133315,
									 CAST(@MigrationForecast AS DATETIME),CAST(@MigrationPlan AS DATETIME),CAST(@MigrationSubmitted AS DATETIME),CAST(1 AS BIT)
			END
			ELSE
			IF @ProjectSiteId>0    
			BEGIN                                
				--TSS
				UPDATE PM_SiteTasks
				SET	StatusId = (CASE WHEN @TSSPlan='' AND @TSSForecast='' AND @TSSSubmitted='' THEN 133317
									 WHEN @TSSPlan!='' AND @TSSForecast='' AND @TSSSubmitted='' THEN 133318
									 WHEN @TSSForecast!='' AND @TSSSubmitted='' THEN 133319
									 WHEN @TSSSubmitted!='' THEN 163395
									 ELSE 133317 END),
					PlannedDate = (CASE WHEN (@TSSPlan='1900-01-01 00:00:00.000' OR @TSSPlan IS NULL) then NULL ELSE CAST(@TSSPlan AS DATETIME) END),
					EstimatedStartDate = (CASE WHEN (@TSSForecast='1900-01-01 00:00:00.000' OR @TSSForecast IS NULL) then NULL ELSE CAST(@TSSForecast AS DATETIME) END),	
					ActualEndDate = (CASE WHEN (@TSSSubmitted='1900-01-01 00:00:00.000' OR @TSSSubmitted IS NULL) then NULL ELSE CAST(@TSSSubmitted AS DATETIME) END)
				WHERE SiteTaskId=50076;
			
				--SSM
				UPDATE PM_SiteTasks
				SET	StatusId = (CASE WHEN @SSMForecast='' AND @SSMActual='' THEN 133317
									 WHEN @SSMForecast !='' AND @SSMActual='' THEN 133319
									 WHEN @SSMActual !='' THEN 163395
									 ELSE 133317 END),
					EstimatedStartDate = (CASE WHEN (@SSMForecast='1900-01-01 00:00:00.000' OR @SSMForecast IS NULL) then NULL ELSE CAST(@SSMForecast AS DATETIME) END),
					ActualEndDate = (CASE WHEN (@SSMActual='1900-01-01 00:00:00.000' OR @SSMActual IS NULL) then NULL ELSE CAST(@SSMActual AS DATETIME) END)
				WHERE SiteTaskId=50077;
			
				--Pre Install
				UPDATE PM_SiteTasks
				SET	StatusId = (CASE WHEN @PreInstPlan ='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133317
									 WHEN @PreInstPlan !='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133318
									 WHEN @PreInstForecast !='' AND @PreInstSubmitted ='' THEN 133319
									 WHEN @PreInstSubmitted !='' THEN 163395
									 ELSE 133317 END),
					PlannedDate = (CASE WHEN (@PreInstPlan='1900-01-01 00:00:00.000' OR @PreInstPlan IS NULL) then NULL ELSE CAST(@PreInstPlan AS DATETIME) END),
					EstimatedStartDate = (CASE WHEN (@PreInstForecast='1900-01-01 00:00:00.000' OR @PreInstForecast IS NULL) then NULL ELSE CAST(@PreInstForecast AS DATETIME) END),	
					ActualEndDate = (CASE WHEN (@PreInstSubmitted='1900-01-01 00:00:00.000' OR @PreInstSubmitted IS NULL) then NULL ELSE CAST(@PreInstSubmitted AS DATETIME) END)					
				WHERE SiteTaskId=50079;
			
				--Migrations
				UPDATE PM_SiteTasks
				SET	StatusId = (CASE WHEN @MigrationPlan ='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133317
									 WHEN @MigrationPlan !='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133318
									 WHEN @MigrationForecast !='' AND @MigrationSubmitted ='' THEN 133319
									 WHEN @MigrationSubmitted !='' THEN 163395
									 ELSE 133317 END),
					PlannedDate = (CASE WHEN (@MigrationPlan='1900-01-01 00:00:00.000' OR @MigrationPlan IS NULL) then NULL ELSE CAST(@MigrationPlan AS DATETIME) END),
					EstimatedStartDate = (CASE WHEN (@MigrationForecast='1900-01-01 00:00:00.000' OR @MigrationForecast IS NULL) then NULL ELSE CAST(@MigrationForecast AS DATETIME) END),	
					ActualEndDate = (CASE WHEN (@MigrationSubmitted='1900-01-01 00:00:00.000' OR @MigrationSubmitted IS NULL) then NULL ELSE CAST(@MigrationSubmitted AS DATETIME) END)
				WHERE SiteTaskId=50080;

				--IF @MigrationSubmitted='2018-03-01'
				--BEGIN
				--	SET @MigCount+=1;
				--	--SELECT 'Mig',@MigCount;
				--END
				--ELSE
				--BEGIN
				--	SET @nMigCount+=1;
				--	--SELECT 'nMig',@nMigCount
				----END
				--DECLARE @tblName as nvarchar(50)='tmp'+CAST((SELECT FLOOR(RAND()*(25-10)+10)) as nvarchar(10))
				--EXEC('SELECT * INTO ' +  @tblName + ' FROM  @Data')

			END
		FETCH NEXT FROM db_cluster2 INTO @FACode,@USID,@ENB,@Region,@Market,@SubMarket,@SiteName,@StreetAddress,@City,@State,@ZIP,@County,@Latitude,@Longitude,
		@vMME,@ControlledIntro,@SuperBowl,@SiteType,@DASInbuilding,@FirstNetRAN,@iPlanJob,@iPlanStatus,@iPlanIssueDate,@PACENumber,@TSSPlan,@TSSForecast,@TSSSubmitted,
		@SSMForecast,@SSMActual,@PreInstPlan,@PreInstForecast,@PreInstSubmitted,@MigrationPlan,@MigrationForecast,@MigrationSubmitted
		END   
		CLOSE db_cluster2   
		DEALLOCATE db_cluster2
		
		--IF @newFACodes=''
		--BEGIN
		
		--END
		--ELSE
		--SELECT @MigCout
		
		IF @newFACodes!=''
		BEGIN
			SELECT x.Value1 'FACode', x.Value5 'MARKET', Value25 'TSSPlan', Value26 'TSSForecast', Value27 'TSSSubmitted',
			Value28 'SiteSpecificMaterialAvailableForecast', Value29 'SiteSpecificMaterialAvailableActual', 
			Value30 'PreInstallPlanned', Value31 'PreInstallFcst', Value32 'PreInstallActual',
			Value33 'MigDatePlanned', Value34 'MigDateForecast', Value35 'MigrationDate','New' 'Status'
			FROM @Data x
			WHERE Charindex(cast(x.Value1 as varchar(max))+',', @newFACodes) > 0
			--UNION ALL
			--SELECT pps.FACode 'FACode', ad.DefinationName 'MARKET', NULL 'TSSPlan', NULL 'TSSForecast', NULL 'TSSSubmitted',
			--NULL 'SiteSpecificMaterialAvailableForecast', NULL 'SiteSpecificMaterialAvailableActual', 
			--NULL 'PreInstallPlanned', NULL 'PreInstallFcst', NULL 'PreInstallActual',
			--NULL 'MigDatePlanned', NULL 'MigDateForecast', NULL 'MigrationDate','Delete' 'Status'
			--FROM PM_ProjectSites AS pps 
			--INNER JOIN AD_Definations AS ad ON ad.DefinationId=pps.CityId
			--WHERE pps.IsActive=1
			--AND pps.FACode NOT IN(SELECT x.Value1 FROM @Data x)
			
		END
		--ELSE
		--BEGIN
		--	SELECT pps.FACode 'FACode', ad.DefinationName 'MARKET', NULL 'TSSPlan', NULL 'TSSForecast', NULL 'TSSSubmitted',
		--	NULL 'SiteSpecificMaterialAvailableForecast', NULL 'SiteSpecificMaterialAvailableActual', 
		--	NULL 'PreInstallPlanned', NULL 'PreInstallFcst', NULL 'PreInstallActual',
		--	NULL 'MigDatePlanned', NULL 'MigDateForecast', NULL 'MigrationDate','Delete' 'Status'
		--	FROM PM_ProjectSites AS pps 
		--	INNER JOIN AD_Definations AS ad ON ad.DefinationId=pps.CityId
		--	WHERE pps.IsActive=1
		--	AND pps.FACode NOT IN(SELECT x.Value1 FROM @Data x)
		--END
	END
END