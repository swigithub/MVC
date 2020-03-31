CREATE PROCEDURE PM_DataImport1
	@Filter NVARCHAR(50),
	@ProjectId NUMERIC(18,0),
	@Data [dbo].[PM_ImportLists] READONLY	
AS
BEGIN
	DECLARE @FACode AS NVARCHAR(50)=''
	DECLARE @ENB AS NVARCHAR(50)=''	--CommonId: is eNB in master site dump
	DECLARE @OtherENB AS NVARCHAR(50)=''
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
	
	----------------References---------------
	DECLARE @ProjectSiteId AS NUMERIC(18,0)=0
	DECLARE @ReasonId AS NUMERIC(18,0)=0 --CC Col: Reason in above defs.
	DECLARE @RegionId AS NUMERIC(18,0)=0
	DECLARE @MarketId AS NUMERIC(18,0)=0
	DECLARE @SiteTypeId AS NUMERIC(18,0)=0
	DECLARE @MaintenanceWindowId AS NUMERIC(18,0)=0
	DECLARE @StatusId AS NUMERIC(18,0)=0
	DECLARE @AlarmId AS NUMERIC(18,0)=0
	DECLARE @IssueCategoryId  AS NUMERIC(18,0)=0
	DECLARE @IssueTypeId AS NUMERIC(18,0)=0	--Issues->is: issue type(access,execution etc.)
	DECLARE @IssueOwnerId AS NUMERIC(18,0)=0
	DECLARE @IsUnavoidable AS bit=CAST(0 AS BIT) --IsUnavoidable based on WhoFix->is: Issue Owner (nokia, att, unavoid etc.)
	DECLARE @ContentTypeId AS NUMERIC(18,0)=0 --ContentType
	DECLARE @CreatedById AS NUMERIC(18,0)=0 --CreatedBy
	
	DECLARE @newFACodes AS NVARCHAR(MAX)=''
	
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
		l.Value18,l.Value19,l.Value20,l.Value21,l.Value22,l.Value23,l.Value24,l.Value25,l.Value26,l.Value27,l.Value28,l.Value29,l.Value30,l.Value31,l.Value32,l.Value33,l.Value34
		FROM @Data AS l
		OPEN db_sites 
		FETCH NEXT FROM db_sites INTO 
		@FACode,@ENB,@OtherENB,@SiteName,@Reasons,@Market,@Schedule,@Actual,@MW,@Status,@Alarm,@Issues,@WhoFix,@Notes,@AddlNotes,@NetAct,@Effort,@Migrated,@TicketRequest,
		@TechName,@TechNumber,@TechMail,@USID,@ExpirationDate,@Notification,@ContentType,@AppCreatedBy,@AppModifiedBy,@WorkflowInstanceId,@FileType,@ModifiedOn,@PMO,			
		@Created,@CreatedBy
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			SET @ProjectSiteId=0
			SET @ProjectSiteId=(SELECT pps.ProjectSiteId FROM PM_ProjectSites AS pps WHERE pps.FACode=@FACode AND pps.IsActive=1)
			
			--CC Col: Reason in above defs.
			SET @ReasonId=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
													 WHERE ad.IsActive=1 AND ad.DefinationName=@Reasons AND adt.DefinationType='Reasons') 
			SET @MarketId=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
													 WHERE ad.IsActive=1 AND ad.DefinationName=@Reasons AND adt.DefinationType='City')
			SET @MaintenanceWindowId=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
													 WHERE ad.IsActive=1 AND ad.DefinationName=@MW AND adt.DefinationType='Milestone Window')   
			SET @StatusId=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                WHERE ad.IsActive=1 AND ad.DefinationName=@Status AND adt.DefinationType='Project Site Status')
			SET @AlarmId=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                               WHERE ad.IsActive=1 AND ad.DefinationName=@Alarm AND adt.DefinationType='Alarms')
			
			--Issues->is: issue type(access,execution etc.)
			SET @IssueTypeId=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
													 WHERE ad.IsActive=1 AND ad.DefinationName=@Reasons AND adt.DefinationType='Issue Type')
			
			--WhoFix->is: Issue Owner (nokia, att, unavoid etc.)			
			SET @IssueOwnerId=(CASE WHEN @WhoFix IN('AT&T','AT&T Unavoid') THEN 3 WHEN @WhoFix IN('Nokia','Nokia Unavoid') THEN 2 ELSE NULL END)
			
			--ContentType
			SET @ContentTypeId=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@ContentType AND adt.DefinationType='Item Type') 
			
			--CreatedBy
			SET @CreatedById=(SELECT usr.UserId FROM Sec_Users usr WHERE usr.IsActive=1 AND usr.LastName+', '+usr.FirstName=@CreatedBy)
			
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
			
			INSERT INTO PM_SiteLog(ProjectSiteId,StatusId,MSWindowId,AlarmId,DESCRIPTION,UserID,CreatedOn,ActivityTypeId,GngId,ItemTypeId,ItemFilePath)
			SELECT @ProjectSiteId, @StatusId, @MaintenanceWindowId, @AlarmId, @Notes, @CreatedById,
			(CASE WHEN @Created!='' OR @Created IS NOT NULL THEN CAST(@Created AS DATETIME) ELSE NULL END),
			NULL, NULL, @ContentTypeId, NULL;	
			
		FETCH NEXT FROM db_sites INTO 
		@FACode,@ENB,@OtherENB,@SiteName,@Reasons,@Market,@Schedule,@Actual,@MW,@Status,@Alarm,@Issues,@WhoFix,@Notes,@AddlNotes,@NetAct,@Effort,@Migrated,@TicketRequest,
		@TechName,@TechNumber,@TechMail,@USID,@ExpirationDate,@Notification,@ContentType,@AppCreatedBy,@AppModifiedBy,@WorkflowInstanceId,@FileType,@ModifiedOn,@PMO,			
		@Created,@CreatedBy
		END   
		CLOSE db_sites   
		DEALLOCATE db_sites
	END
	ELSE IF @Filter='Import_WR_Issues' 
	BEGIN
		--"Value1", p.eNB,
		--"Value2", p.FACode,
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
        --"Value15", p.ContentType,
        --"Value16", p.AppCreatedBy,
        --"Value17", p.AppModifiedBy,
        --"Value18", p.Attachments,
        --"Value19", p.WorkflowInstanceID,
        --"Value20", p.FileType,
        --"Value21", p.PMO,
        --"Value22", p.Modified
		
		DECLARE db_issues CURSOR FOR  
		SELECT	l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14,l.Value15,l.Value16,l.Value17,
				l.Value18,l.Value19,l.Value20,l.Value21,l.Value22
				FROM @Data AS l
		OPEN db_issues 
		FETCH NEXT FROM db_issues INTO @ENB,@FACode,@OtherENB,@SiteName,@Reasons,@Market,@Schedule,@Actual,@MW,@Status,@Alarm,@Issues,@WhoFix,@Notes,@ContentType,@AppCreatedBy,
		@AppModifiedBy,@Attachments,@WorkflowInstanceId,@FileType,@PMO,@ModifiedOn
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			SET @ProjectSiteId=0
			SET @ProjectSiteId=(SELECT pps.ProjectSiteId FROM PM_ProjectSites AS pps WHERE pps.FACode=@FACode AND pps.IsActive=1)
			
			--CC Col: Reason in above defs.
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
			                                    WHERE ad.IsActive=1 AND ad.DefinationName=@Status AND adt.DefinationType='Issue Category')
			
			--Issues->is: issue type(access,execution etc.)
			SET @IssueTypeId=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
														WHERE ad.IsActive=1 AND ad.DefinationName=@Issues AND adt.DefinationType='Issue Type')
			
			--WhoFix->is: Issue Owner (nokia, att, unavoid etc.)			
			SET @IssueOwnerId=(CASE WHEN @WhoFix IN('AT&T','AT&T Unavoid') THEN 3 WHEN @WhoFix IN('Nokia','Nokia Unavoid') THEN 2 ELSE NULL END)
			
			SET @IsUnavoidable = (CASE WHEN @WhoFix IN('AT&T Unavoid','Nokia Unavoid','Unavoid') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
			
			--ContentType
			SET @ContentTypeId=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON adt.DefinationTypeId=ad.DefinationTypeId
												WHERE ad.IsActive=1 AND ad.DefinationName=@ContentType AND adt.DefinationType='Item Type')
			
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
			
			INSERT INTO PM_Issues(ProjectId,ProjectSiteId,TaskId,IssuePriorityId,IssueStatusId,IssueCategoryId,ReasonId,IssueById,[Description],RequestDate,AssignedToId,
			ForecastDate,TargetDate,ActualEndDate,IsUnavoidable,ENB,ExtendedeNB,EquipmentId,AOTSCR,ActivityTypeId,ItemTypeId,ItemFilePath,SeverityId,AlarmId,MSWindowId,RequestedBy,StatusId)
			SELECT @ProjectId,@ProjectSiteId,50080,163430,@StatusId,@IssueCategoryId, @IssueTypeId,@IssueOwnerId,@Notes,
			(CASE WHEN @Schedule!='' OR @Schedule IS NOT NULL THEN CAST(@Schedule AS DATETIME) ELSE NULL END),NULL,
			(CASE WHEN @Schedule!='' OR @Schedule IS NOT NULL THEN CAST(@Schedule AS DATETIME) ELSE NULL END),NULL,
			(CASE WHEN @Actual!='' OR @Actual IS NOT NULL THEN CAST(@Actual AS DATETIME) ELSE NULL END),
			@IsUnavoidable,@ENB,@OtherENB,NULL,NULL,NULL,@ContentTypeId,NULL,NULL,@AlarmId,@MaintenanceWindowId,@CreatedBy,@StatusId;
			
		FETCH NEXT FROM db_issues INTO @ENB,@FACode,@OtherENB,@SiteName,@Reasons,@Market,@Schedule,@Actual,@MW,@Status,@Alarm,@Issues,@WhoFix,@Notes,@ContentType,@AppCreatedBy,
		@AppModifiedBy,@Attachments,@WorkflowInstanceId,@FileType,@PMO,@ModifiedOn
		END   
		CLOSE db_issues   
		DEALLOCATE db_issues
	END
	ELSE IF @Filter='Save_Project_Plan'
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
						
		DECLARE db_cluster2 CURSOR FOR  
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14,l.Value15,l.Value16,l.Value17,
		l.Value18,l.Value19,l.Value20,l.Value21,l.Value22,l.Value23,l.Value24,l.Value25,l.Value26,l.Value27,l.Value28,l.Value29,l.Value30,l.Value31,l.Value32,l.Value33,l.Value34,
		l.Value35,l.Value36,l.Value37,l.Value38,l.Value39
		FROM @Data AS l
		OPEN db_cluster2 
		FETCH NEXT FROM db_cluster2 INTO @FACode,@USID,@ENB,@Region,@Market,@SubMarket,@SiteName,@StreetAddress,@City,@State,@ZIP,@County,@Latitude,@Longitude,
		@vMME,@ControlledIntro,@SuperBowl,@SiteType,@DASInbuilding,@FirstNetRAN,@iPlanJob,@iPlanStatus,@iPlanIssueDate,@PACENumber,@TSSPlan,@TSSForecast,@TSSSubmitted,
		@SSMForecast,@SSMActual,@PreInstPlan,@PreInstForecast,@PreInstSubmitted,@MigrationPlan,@MigrationForecast,@MigrationSubmitted,@EPLOrdered,@EPLCalledOut,@EPLDelivered,@EPLStatus
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
				VALUES(20021, @FACode, (CASE WHEN @TSSPlan!='' THEN CAST(@TSSPlan AS DATETIME) ELSE NULL END), @SiteTypeId, NULL, @MarketId, '#ffffff',(CASE WHEN @TSSPlan!='' THEN CAST(@TSSPlan AS DATETIME) ELSE NULL END), 11, CAST(1 AS BIT), @Latitude, @Longitude,
				NULL, 2, @SiteName,@woRefID,@maxWoID,0,@USID, @SubMarket, CAST(@VMME AS BIT), CAST(@ControlledIntro AS BIT), CAST(@SuperBowl AS BIT), @DASInbuilding,
				@FirstNetRAN, @iPlanJob, @PACENumber, (CASE WHEN @IPlanIssueDate!='' THEN CAST(@IPlanIssueDate AS DATETIME) ELSE NULL END),
				(CASE WHEN @TSSPlan!='' THEN CAST(@TSSPlan AS DATETIME) ELSE NULL END),@FACode)
				
				
				
				SELECT @ProjectSiteId=@@IDENTITY
				
				
				SET @newFACodes = @newFACodes+ @FACode+','
				
				--TSS
				INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,PlannedDate,ActualEndDate,IsActive)
				SELECT @ProjectSiteId,50076,0,
				(CASE WHEN @TSSPlan ='' AND @TSSForecast ='' AND @TSSSubmitted ='' THEN 133317
									 WHEN @TSSPlan !='' AND @TSSForecast ='' AND @TSSSubmitted ='' THEN 133318
									 WHEN @TSSForecast !='' AND @TSSSubmitted ='' THEN 133319
									 WHEN @TSSSubmitted !='' THEN 163395
									 ELSE 133317 END),133315,CAST(CONVERT(NVARCHAR(10),@TSSForecast,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@TSSPlan,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@TSSSubmitted,102) AS DATETIME),CAST(1 AS BIT)
				
				--SSM
				INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,ActualEndDate,IsActive)
				SELECT @ProjectSiteId,50077,0,
				(CASE WHEN @SSMForecast ='' AND @SSMActual ='' THEN 133317
									 WHEN @SSMForecast !='' AND @SSMActual ='' THEN 133319
									 WHEN @SSMActual !='' THEN 163395
									 ELSE 133317 END),133315,CAST(CONVERT(NVARCHAR(10),@SSMForecast,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@SSMActual,102) AS DATETIME),CAST(1 AS BIT)
									 
				
				--Pre-Install
				INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,PlannedDate,ActualEndDate,IsActive)
				SELECT @ProjectSiteId,50079,0,
				(CASE WHEN @PreInstPlan ='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133317
									 WHEN @PreInstPlan !='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133318
									 WHEN @PreInstForecast !='' AND @PreInstSubmitted ='' THEN 133319
									 WHEN @PreInstSubmitted !='' THEN 163395
									 ELSE 133317 END),133315,CAST(CONVERT(NVARCHAR(10),@PreInstForecast,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@PreInstPlan,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@PreInstSubmitted,102) AS DATETIME),CAST(1 AS BIT)
									 
				
				--Migrations
				INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,PlannedDate,ActualEndDate,IsActive)
				SELECT @ProjectSiteId,50080,0,
				(CASE WHEN @MigrationPlan ='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133317
									 WHEN @MigrationPlan !='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133318
									 WHEN @MigrationForecast !='' AND @MigrationSubmitted ='' THEN 133319
									 WHEN @MigrationSubmitted !='' THEN 163395
									 ELSE 133317 END),133315,CAST(CONVERT(NVARCHAR(10),@MigrationForecast,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@MigrationPlan,102) AS DATETIME),CAST(CONVERT(NVARCHAR(10),@MigrationSubmitted,102) AS DATETIME),CAST(1 AS BIT)
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
					PlannedDate = CAST(CONVERT(NVARCHAR(10),@TSSPlan,102) AS DATETIME),
					EstimatedStartDate = CAST(CONVERT(NVARCHAR(10),@TSSForecast,102) AS DATETIME),	
					ActualEndDate = CAST(CONVERT(NVARCHAR(10),@TSSSubmitted,102) AS DATETIME)
				WHERE SiteTaskId=50076;
			
				--SSM
				UPDATE PM_SiteTasks
				SET	StatusId = (CASE WHEN @SSMForecast='' AND @SSMActual='' THEN 133317
									 WHEN @SSMForecast !='' AND @SSMActual='' THEN 133319
									 WHEN @SSMActual !='' THEN 163395
									 ELSE 133317 END),
					EstimatedStartDate = @SSMForecast,	
					ActualEndDate = @SSMActual
				WHERE SiteTaskId=50077;
			
				--Pre Install
				UPDATE PM_SiteTasks
				SET	StatusId = (CASE WHEN @PreInstPlan ='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133317
									 WHEN @PreInstPlan !='' AND @PreInstForecast ='' AND @PreInstSubmitted ='' THEN 133318
									 WHEN @PreInstForecast !='' AND @PreInstSubmitted ='' THEN 133319
									 WHEN @PreInstSubmitted !='' THEN 163395
									 ELSE 133317 END),
					PlannedDate = @PreInstPlan,
					EstimatedStartDate = @PreInstForecast,	
					ActualEndDate = @PreInstSubmitted
				WHERE SiteTaskId=50079;
			
				--Migrations
				UPDATE PM_SiteTasks
				SET	StatusId = (CASE WHEN @MigrationPlan ='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133317
									 WHEN @MigrationPlan !='' AND @MigrationForecast ='' AND @MigrationSubmitted ='' THEN 133318
									 WHEN @MigrationForecast !='' AND @MigrationSubmitted ='' THEN 133319
									 WHEN @MigrationSubmitted !='' THEN 163395
									 ELSE 133317 END),
					PlannedDate = @MigrationPlan,
					EstimatedStartDate = @MigrationForecast,	
					ActualEndDate = @MigrationSubmitted
				WHERE SiteTaskId=50080;
			END
		FETCH NEXT FROM db_cluster2 INTO @FACode,@USID,@ENB,@Region,@Market,@SubMarket,@SiteName,@StreetAddress,@City,@State,@ZIP,@County,@Latitude,@Longitude,
		@vMME,@ControlledIntro,@SuperBowl,@SiteType,@DASInbuilding,@FirstNetRAN,@iPlanJob,@iPlanStatus,@iPlanIssueDate,@PACENumber,@TSSPlan,@TSSForecast,@TSSSubmitted,
		@SSMForecast,@SSMActual,@PreInstPlan,@PreInstForecast,@PreInstSubmitted,@MigrationPlan,@MigrationForecast,@MigrationSubmitted,@EPLOrdered,@EPLCalledOut,@EPLDelivered,@EPLStatus
		END   
		CLOSE db_cluster2   
		DEALLOCATE db_cluster2
		
		--IF @newFACodes=''
		--BEGIN
		
		--END
		--ELSE
		
		
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