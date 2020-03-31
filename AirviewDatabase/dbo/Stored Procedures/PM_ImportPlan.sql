CREATE PROCEDURE PM_ImportPlan
	@Filter NVARCHAR(50),
	@ProjectId NUMERIC(18,0)=0,
	@Data [dbo].[PM_ImportLists] readonly
AS
DECLARE @FA_Code nvarchar(255)
DECLARE @USID nvarchar(255)
DECLARE @CommonID nvarchar(255)
DECLARE @REGION nvarchar(255)
DECLARE @MARKET nvarchar(255)
DECLARE @SUBMarket nvarchar(255)
DECLARE @SiteName nvarchar(255)
DECLARE @StreetAddress nvarchar(255)
DECLARE @CITY nvarchar(255)
DECLARE @State nvarchar(255)
DECLARE @ZIP nvarchar(255)
DECLARE @COUNTY nvarchar(255)
DECLARE @Latitude float
DECLARE @Longitude float
DECLARE @vMME bit 
DECLARE @ControlledIntroduction bit 
DECLARE @SuperBowl bit 
DECLARE @SiteType nvarchar(255)
DECLARE @DASorInbuilding nvarchar(255)
DECLARE @FirstNetRAN nvarchar(255)
DECLARE @iPlanJob nvarchar(255)
DECLARE @iPlanStatus nvarchar(255)
DECLARE @PACENumber nvarchar(255)
DECLARE @iPlanIssue datetime
DECLARE @TSSPlan datetime
DECLARE @TSSForecast datetime
DECLARE @TSSSubmitted datetime
DECLARE @Site_Specific_Material_Available_Forecast datetime
DECLARE @Site_Specific_Material_Available_Actual datetime
DECLARE @Pre_Install_Planned datetime
DECLARE @Pre_Install_Fcst datetime
DECLARE @Pre_Install_Actual datetime
DECLARE @Mig_Date_Planned datetime
DECLARE @Mig_Date_Forecast datetime
DECLARE @Migration_Date_Actual DATETIME
DECLARE @FACode AS NVARCHAR(150)=''
DECLARE @Task AS NVARCHAR(50)=''
DECLARE @PlanDate AS NVARCHAR(50)=''	--CC Column in WR_Log_Sites/Export
DECLARE @ForecastStartDate AS NVARCHAR(50)=''
DECLARE @ForecastEndDate AS NVARCHAR(50)=''
DECLARE @TargetDate AS NVARCHAR(50)=''
DECLARE @ActualStartDate AS NVARCHAR(50)=''
DECLARE @ActualEndDate AS NVARCHAR(50)=''
DECLARE @Status AS NVARCHAR(50)=''	--CommonId: is eNB in master site dump
DECLARE @Resources AS NVARCHAR(50)=''
declare @Cluster nvarchar(50) =''
declare @SiteClass nvarchar(100) = ''
declare @Scope nvarchar(100) = ''
declare @Description nvarchar(250) = ''
declare @Priority nvarchar(50) = ''
declare @Completion AS integer=0
declare @CreatedOn nvarchar(100) = ''
declare @CreatedBy  AS integer=0
declare @ModifiedOn nvarchar(50) = ''
declare @ModifiedBy AS integer=0
declare @LastFACode as nvarchar(200) = ''
declare @TaskCounter AS integer=0
declare @RevisionId AS integer=0
declare @User AS integer=0
--SET @ProjectId=20021

--TRUNCATE TABLE PM_SiteTasks
--TRUNCATE TABLE PM_ProjectSites

--DECLARE THE CURSOR FOR A QUERY.9.19
if (@Filter='Custom')
begin
--SET @ProjectId=20021


DECLARE @newFACodes AS NVARCHAR(MAX)=''
--TRUNCATE TABLE PM_SiteTasks
--TRUNCATE TABLE PM_ProjectSites

--DECLARE THE CURSOR FOR A QUERY.9.19
DECLARE addSite CURSOR READ_ONLY
FOR
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
--OPEN CURSOR
OPEN addSite 
--FETCH THE RECORD INTO THE VARIABLES.
FETCH NEXT FROM addSite INTO @FA_Code,@USID,@CommonID,@REGION,@MARKET,@SUBMarket,@SiteName,@StreetAddress,@CITY,@State,@ZIP,@COUNTY,@Latitude,@Longitude,@vMME,@ControlledIntroduction,@SuperBowl, 
@SiteType,@DASorInbuilding,@FirstNetRAN,@iPlanJob,@iPlanStatus,@PACENumber,@iPlanIssue,@TSSPlan,@TSSForecast,@TSSSubmitted,@Site_Specific_Material_Available_Forecast,
@Site_Specific_Material_Available_Actual,@Pre_Install_Planned,@Pre_Install_Fcst,@Pre_Install_Actual,@Mig_Date_Planned,@Mig_Date_Forecast,@Migration_Date_Actual
--LOOP UNTIL RECORDS ARE AVAILABLE.
WHILE @@FETCH_STATUS = 0
BEGIN
	--BEGIN TRANSACTION	
		DECLARE @maxWoID as int=0
		DECLARE @woRefID as nvarchar(25)
		DECLARE @SiteTypeId AS NUMERIC(18,0)=ISNULL((SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationName=@SiteType AND ad.DefinationTypeId=28 AND ad.IsActive=1),3132)
		DECLARE @SiteClassId AS NUMERIC(18,0)=3133
		DECLARE @CityId AS NUMERIC(18,0)=(SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationName=@MARKET AND ad.DefinationTypeId=7 AND ad.IsActive=1)
		DECLARE @StatusId AS NUMERIC(18,0)=163384
		DECLARE @MSWindowId AS NUMERIC(18,0)=NULL
		DECLARE @PriorityId AS NUMERIC(18,0)=133315
		DECLARE @ClientId AS NUMERIC(18,0)=(SELECT TOP 1 pp.ClientId FROM PM_Projects AS pp WHERE pp.ProjectId=@ProjectId)
		DECLARE @ScopeId AS NUMERIC(18,0)=153347
				
		IF NOT EXISTS(SELECT ProjectSiteId FROM PM_ProjectSites WHERE FACode=@FA_Code)
		BEGIN
			SET @maxWoID=(Select ISNULL(MAX(PMCode),0) + 1 from PM_ProjectSites x WHERE x.ProjectId=@ProjectId AND ClientId=@ClientId AND (YEAR(x.CreatedOn)* 100 + MONTH(x.CreatedOn) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))					
			SET @woRefID = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientId=@ClientId)+ '-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
			RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@maxWoID as nvarchar(15)),5))
					
			INSERT INTO PM_ProjectSites(WoRefId,ProjectId,SiteCode,SiteName,SiteDate,SiteTypeId,SiteClassId,Latitude,Longitude,RevisionId,PMCode,PMRefId,ClusterId,ClusterCode,
			CityId,StatusId,MSWindowId,PriorityId,ColorId,CreatedOn,CreatedBy,IsActive,BudgetCost,ActualCost,[Description],ClientId,ScopeId,ReceivedOn,[Address],
			MilestoneId,StageId,FACode,USID,CommonId,MarketId,SubMarketId,CountyId,vMME,ControlledIntro,
			SuperBowl,isDASInBuild,FirstNetRAN,IPlanJob,PaceNo,SubMarket,County)		
			SELECT @woRefID,@ProjectId,STR(@FA_Code,10,0) 'SiteCode',@SiteName,@TSSPlan 'SiteDate',@SiteTypeId,@SiteClassId,@Latitude,@Longitude,0 'Revsion',@maxWoID 'PMCode',@woRefID 'PMRefId',NULL 'ClusterId','' 'ClusterCode',
			@CityId,@StatusId,@MSWindowId,@PriorityId,NULL 'ColorId',GETDATE() 'CreatedOn',11 'CreatedBy',CAST(1 AS BIT) 'IsActive',0 'BudgetCost',0 'ActualCost','' 'Description',@ClientId,
			@ScopeId,@TSSPlan 'ReceivedOn',ISNULL(@StreetAddress,'')+', '+ISNULL(@CITY,'')+', '+ISNULL(@COUNTY,'')+', '+ISNULL(@State,'')+', '+ISNULL(@ZIP,'') 'Address',
			NULL 'MileStoneId',NULL 'StageId',STR(@FA_Code,10,0),@USID,@CommonID,@CityId 'MarketId',NULL 'SubMarketId',NULL 'CountyId',@vMME,@ControlledIntroduction,
			@SuperBowl,@DASorInbuilding,@FirstNetRAN,@iPlanJob,@PACENumber,@SUBMarket,@COUNTY
		
			
			DECLARE @ProjectSiteId AS NUMERIC(18,0)=0
			SELECT @ProjectSiteId=@@IDENTITY;			
			
			--SELECT 'Site_Inserted',@ProjectSiteId,@FA_Code
			
			INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,EstimatedEndDate,PlannedDate,TargetDate,ActualStartDate,ActualEndDate,
			CompletionPercent,BudgetCost,ActualCost,IsActive)
			SELECT @ProjectSiteId,pt.TaskId,0 'RevisionId',pt.StatusId, pt.PriorityId,
					@TSSForecast 'EstimatedStartDate', NULL 'EstimatedEndDate', @TSSPlan 'PlannedDate',
					NULL 'TargetDate', NULL 'ActualStartDate', @TSSSubmitted 'ActualEndDate',0 'CompletionPercent',pt.BudgetCost,0 'ActualCost',pt.IsActive
			FROM PM_Tasks AS pt
			WHERE pt.ProjectId=@ProjectId AND pt.IsActive=1 and pt.TaskId=50076
			UNION ALL
			SELECT @ProjectSiteId,pt.TaskId,0 'RevisionId',pt.StatusId, pt.PriorityId,
					@Site_Specific_Material_Available_Forecast 'EstimatedStartDate', NULL 'EstimatedEndDate', NULL 'PlannedDate',
					NULL 'TargetDate', NULL 'ActualStartDate', @Site_Specific_Material_Available_Actual 'ActualEndDate',0 'CompletionPercent',pt.BudgetCost,0 'ActualCost',pt.IsActive
			FROM PM_Tasks AS pt
			WHERE pt.ProjectId=@ProjectId AND pt.IsActive=1 and pt.TaskId=50077
			UNION ALL
			SELECT @ProjectSiteId,pt.TaskId,0 'RevisionId',pt.StatusId, pt.PriorityId,
					NULL 'EstimatedStartDate', NULL 'EstimatedEndDate', NULL 'PlannedDate',
					NULL 'TargetDate', NULL 'ActualStartDate', NULL 'ActualEndDate',0 'CompletionPercent',pt.BudgetCost,0 'ActualCost',pt.IsActive
			FROM PM_Tasks AS pt
			WHERE pt.ProjectId=@ProjectId AND pt.IsActive=1 and pt.TaskId IN(50078,50081,50082,50083)
			UNION ALL
			SELECT @ProjectSiteId,pt.TaskId,0 'RevisionId',pt.StatusId, pt.PriorityId,
					@Pre_Install_Fcst 'EstimatedStartDate', NULL 'EstimatedEndDate', @Pre_Install_Planned 'PlannedDate',
					NULL 'TargetDate', NULL 'ActualStartDate', @Pre_Install_Actual 'ActualEndDate',0 'CompletionPercent',pt.BudgetCost,0 'ActualCost',pt.IsActive
			FROM PM_Tasks AS pt
			WHERE pt.ProjectId=@ProjectId AND pt.IsActive=1 and pt.TaskId=50079
			UNION ALL
			SELECT @ProjectSiteId,pt.TaskId,0 'RevisionId',pt.StatusId, pt.PriorityId,
					@Mig_Date_Forecast 'EstimatedStartDate', NULL 'EstimatedEndDate', @Mig_Date_Planned 'PlannedDate',
					NULL 'TargetDate', NULL 'ActualStartDate', @Migration_Date_Actual 'ActualEndDate',0 'CompletionPercent',pt.BudgetCost,0 'ActualCost',pt.IsActive
			FROM PM_Tasks AS pt
			WHERE pt.ProjectId=@ProjectId AND pt.IsActive=1 and pt.TaskId=50080

			--SET @newFACodes = @newFACodes+ STR(@FA_Code,10,0)+','

			--SELECT 0;
		END
		ELSE
		BEGIN
		--SELECT 1;
			SELECT @ProjectSiteId=ProjectSiteId FROM PM_ProjectSites WHERE FACode=@FA_Code

			--SELECT @FA_Code, @ProjectSiteId,@TSSPlan, @TSSForecast, @TSSSubmitted

				--TSS
				UPDATE PM_SiteTasks
				SET	
				--StatusId = (CASE WHEN @TSSPlan='' AND @TSSForecast='' AND @TSSSubmitted='' THEN 133317
				--					 WHEN @TSSPlan!='' AND @TSSForecast='' AND @TSSSubmitted='' THEN 133318
				--					 WHEN @TSSForecast!='' AND @TSSSubmitted='' THEN 133319
				--					 WHEN @TSSSubmitted!='' THEN 163395
				--					 ELSE 133317 END),
					PlannedDate = @TSSPlan,
					EstimatedStartDate = @TSSForecast,	
					ActualEndDate = @TSSSubmitted
				WHERE ProjectSiteId=@ProjectSiteId and TaskId=50076;
			
				--SSM
				UPDATE PM_SiteTasks
				SET	
				--StatusId = (CASE WHEN @Site_Specific_Material_Available_Forecast='' AND @Site_Specific_Material_Available_Actual='' THEN 133317
				--					 WHEN @Site_Specific_Material_Available_Forecast !='' AND @Site_Specific_Material_Available_Actual='' THEN 133319
				--					 WHEN @Site_Specific_Material_Available_Actual !='' THEN 163395
				--					 ELSE 133317 END),
					EstimatedStartDate = @Site_Specific_Material_Available_Forecast,	
					ActualEndDate = @Site_Specific_Material_Available_Actual
				WHERE ProjectSiteId=@ProjectSiteId and TaskId=50077;
			
				--Pre Install
				UPDATE PM_SiteTasks
				SET	
				--StatusId = (CASE WHEN @Pre_Install_Planned ='' AND @Pre_Install_Fcst ='' AND @Pre_Install_Actual ='' THEN 133317
				--					 WHEN @Pre_Install_Planned !='' AND @Pre_Install_Fcst ='' AND @Pre_Install_Actual ='' THEN 133318
				--					 WHEN @Pre_Install_Fcst !='' AND @Pre_Install_Actual ='' THEN 133319
				--					 WHEN @Pre_Install_Actual !='' THEN 163395
				--					 ELSE 133317 END),
					PlannedDate = @Pre_Install_Planned,
					EstimatedStartDate = @Pre_Install_Fcst,	
					ActualEndDate = @Pre_Install_Actual
				WHERE ProjectSiteId=@ProjectSiteId and  TaskId=50079;
			
				--Migrations
				UPDATE PM_SiteTasks
				SET	
				--StatusId = (CASE WHEN @Mig_Date_Planned ='' AND @Mig_Date_Forecast ='' AND @Migration_Date_Actual ='' THEN 133317
				--					 WHEN @Mig_Date_Planned !='' AND @Mig_Date_Forecast ='' AND @Migration_Date_Actual ='' THEN 133318
				--					 WHEN @Mig_Date_Forecast !='' AND @Migration_Date_Actual ='' THEN 133319
				--					 WHEN @Migration_Date_Actual !='' THEN 163395
				--					 ELSE 133317 END),
					PlannedDate = @Mig_Date_Planned,
					EstimatedStartDate = @Mig_Date_Forecast,	
					ActualEndDate = @Migration_Date_Actual
				WHERE ProjectSiteId=@ProjectSiteId and TaskId=50080;
		END
		
		----MS: TSS
		--UPDATE PM_SiteTasks
		--SET EstimatedStartDate = @TSSForecast, PlannedDate = @TSSPlan, ActualEndDate = @TSSSubmitted 
		--WHERE ProjectSiteId=@ProjectSiteId AND TaskId=50076
		
		----MS: SSM
		--UPDATE PM_SiteTasks
		--SET EstimatedStartDate = @Site_Specific_Material_Available_Forecast, ActualEndDate = @Site_Specific_Material_Available_Actual 
		--WHERE ProjectSiteId=@ProjectSiteId AND TaskId=50077
		
		----MS: PreInstall
		--UPDATE PM_SiteTasks
		--SET EstimatedStartDate = @Pre_Install_Fcst, PlannedDate = @Pre_Install_Planned, ActualEndDate = @Pre_Install_Actual 
		--WHERE ProjectSiteId=@ProjectSiteId AND TaskId=50079
		
		--MS: Migration
		--UPDATE PM_SiteTasks
		--SET EstimatedStartDate = @Mig_Date_Forecast, PlannedDate = @Mig_Date_Planned, ActualEndDate = @Migration_Date_Actual 
		--WHERE ProjectSiteId=@ProjectSiteId AND TaskId=50080
		
		--SELECT @ProjectSiteId,@FA_Code
	--COMMIT
	--FETCH THE NEXT RECORD INTO THE VARIABLES.
	FETCH NEXT FROM addSite INTO @FA_Code,@USID,@CommonID,@REGION,@MARKET,@SUBMarket,@SiteName,@StreetAddress,@CITY,@State,@ZIP,@COUNTY,@Latitude,@Longitude,@vMME,@ControlledIntroduction,@SuperBowl, 
@SiteType,@DASorInbuilding,@FirstNetRAN,@iPlanJob,@iPlanStatus,@PACENumber,@iPlanIssue,@TSSPlan,@TSSForecast,@TSSSubmitted,@Site_Specific_Material_Available_Forecast,
@Site_Specific_Material_Available_Actual,@Pre_Install_Planned,@Pre_Install_Fcst,@Pre_Install_Actual,@Mig_Date_Planned,@Mig_Date_Forecast,@Migration_Date_Actual
END 
--CLOSE THE CURSOR.
CLOSE addSite
DEALLOCATE addSite


end


if (@Filter ='Save_Project_Plan')
	begin


select * into #saveProjectPlanFlagUpdate from @Data
		DECLARE addSite CURSOR --READ_ONLY
		FOR
		select x.Value1,x.Value2,x.Value3,x.Value4,x.Value5,x.Value6,x.Value7,x.Value8,x.Value9,x.Value10,x.Value11,x.Value12,x.Value13,x.Value14
		,x.Value15,x.Value16,x.Value17,x.Value18,x.Value19,x.Value20,x.Value21,x.Value22,x.Value23,x.Value24,x.Value25,x.Value26,x.Value27
		from @Data x
		--OPEN CURSOR
		OPEN addSite 

		fetch next from addSite into @FACode,@Task,@PlanDate,@ForecastStartDate,@ForecastEndDate,@TargetDate,@ActualStartDate,@ActualEndDate,@Status,@Resources,@Cluster,@Market,
			 @Latitude,@Longitude,@SiteName,@SiteType,@SiteClass,@Scope,@Description,@Status,@Priority,@Completion,@CreatedOn,@CreatedBy,@ModifiedOn,@ModifiedBy,@TaskCounter
		WHILE @@FETCH_STATUS = 0
		BEGIN
		    set @User = @CreatedBy
		    SET @PlanDate=(CASE WHEN @PlanDate='' THEN NULL ELSE @PlanDate END)
			SET @TargetDate=(CASE WHEN @TargetDate='' THEN NULL ELSE @TargetDate END)
			SET @ForecastStartDate=(CASE WHEN @ForecastStartDate='' THEN NULL ELSE @ForecastStartDate END)
			SET @ForecastEndDate=(CASE WHEN @ForecastEndDate='' THEN NULL ELSE @ForecastEndDate END)
			SET @ActualStartDate=(CASE WHEN @ActualStartDate='' THEN NULL ELSE @ActualStartDate END)
			SET @ActualEndDate=(CASE WHEN @ActualEndDate='' THEN NULL ELSE @ActualEndDate END)

			--BEGIN TRANSACTION	   
			set	@StatusId = (select top 1  DefinationId from AD_Definations where keycode='PROJECT_TASK_STATUS' and DisplayText=@Status and IsActive=1)
			set	@SiteTypeId = (select top 1  DefinationId from AD_Definations where keycode='SITE_TYPE' and DefinationName=@SiteType and IsActive=1)
			set	@SiteClassId = (select top 1  DefinationId from AD_Definations where keycode='SITE_CLASS' and DefinationName=@SiteClass and IsActive=1)
			set	@ScopeId = (select top 1  DefinationId from AD_Definations where keycode='SCOPE' and DefinationName=@Scope and IsActive=1)
			set	@PriorityId = (select top 1  DefinationId from AD_Definations where DefinationTypeId=(select top 1 adt.DefinationTypeId from AD_DefinationTypes adt where adt.DefinationType= 'Priority') and DefinationName=@Priority and IsActive=1) 
			set	@MARKET = (select top 1  DefinationId from AD_Definations where keycode='MARKET' and DefinationName=@MARKET and IsActive=1) 
			--select @PriorityId
			--If Status is not provided
			IF ISNULL(@StatusId,0)=0
			BEGIN
				set	@StatusId = (select top 1  DefinationId from AD_Definations where keycode='PROJECT_TASK_STATUS' and DefinationName='Active' and IsActive=1)
			END
						
			--If Priority is not provided
			IF ISNULL(@PriorityId,0)=0
			BEGIN
				set	@PriorityId = (select top 1  DefinationId from AD_Definations where keycode='High' and DefinationName='High' and IsActive=1) 
			END	

			if (@FACode <> '')
			begin
				IF EXISTS(SELECT top 1 ProjectSiteId FROM PM_ProjectSites WHERE FACode=@FACode and ProjectId = @ProjectId and IsActive=1)
				BEGIN	
					set	@LastFACode=@FACode
					set @ProjectSiteId = (SELECT top 1 ProjectSiteId FROM PM_ProjectSites WHERE FACode=@LastFACode and ProjectId = @ProjectId and IsActive=1)
					
					IF @ProjectSiteId>0
					BEGIN
					 
						update PM_ProjectSites set ClusterCode=@Cluster,CityId=@MARKET,Latitude=@Latitude,Longitude=@Longitude,SiteName=@SiteName,SiteTypeId=@SiteTypeId,SiteClassId=@SiteClassId,ScopeId=@ScopeId,[Description]=@Description,ModifyOn=@ModifiedOn,
						ModifyBy=@ModifiedBy,CreatedOn=@CreatedOn,CreatedBy=@CreatedBy--,siteDate=@CreatedOn
						WHERE FACode=@FACode and ProjectId = @ProjectId
						
						UPDATE #saveProjectPlanFlagUpdate
                        SET    Value28 = 'update',
						Value30 = @ProjectSiteId
                        WHERE  Value27 = @TaskCounter
										
						declare @TaskId as integer = (select top 1 TaskId from PM_Tasks where Title=@Task and ProjectId=@ProjectId ) 
						set	@LastFACode=@FACode

						if(@TaskId >0 )
						begin		
							declare @SiteTaskId as integer = (select top 1 SiteTaskId from PM_SiteTasks where TaskId=@TaskId and ProjectSiteId = @ProjectSiteId) 
		
							if(@SiteTaskId >0 )
							begin
								print 'update site task'
								--SELECT @ActualStartDate
								update PM_SiteTasks set PlannedDate=@PlanDate,EstimatedStartDate=@ForecastStartDate,EstimatedEndDate=@ForecastEndDate,TargetDate=@TargetDate
								,ActualStartDate=@ActualStartDate,ActualEndDate=@ActualEndDate,ResourcesId=@Resources,StatusId=@StatusId,PriorityId=@PriorityId,CompletionPercent=@Completion
								where SiteTaskId = @SiteTaskId

								UPDATE #saveProjectPlanFlagUpdate
                                SET    Value29 = 'update',
								Value31=@SiteTaskId
                                WHERE  Value27 = @TaskCounter
							end	
							else			
						begin
						UPDATE #saveProjectPlanFlagUpdate
                        SET    Value29 = 'site task not exist',
						Value31 =0
                        WHERE  Value27 = @TaskCounter 
						end
						end
						else
						BEGIN
							print 'Task not exists'
							UPDATE #saveProjectPlanFlagUpdate
                                SET    Value29 = 'task not exist',
								Value31 =0
                                WHERE  Value27 = @TaskCounter
							--SELECT 0;
						END
					END
				END
				ELSE
				BEGIN
					--if site does not exists	
					DECLARE @ProjectSiteIdN AS NUMERIC(18,0)=0

					--Create Project Site
					insert into PM_ProjectSites (ProjectId,FACode,SiteCode,ClusterCode,ClusterId,CityId,Latitude,Longitude,SiteName,SiteTypeId,SiteClassId,ScopeId,[Description],ModifyOn,ModifyBy,CreatedOn,CreatedBy,SiteDate,statusid,ReceivedOn)	
					values (@ProjectId,@FACode,@FACode,@Cluster,0,@MARKET,@Latitude,@Longitude,@SiteName,@SiteTypeId,@SiteClassId,@ScopeId,@Description,@ModifiedOn,@ModifiedBy,@CreatedOn,@CreatedBy,@CreatedOn,(select top 1  DefinationId from AD_Definations where KeyCode='PROJECT SITE STATUS' and DefinationName='Active' and IsActive=1),@CreatedOn) 
					
					 	
					 SELECT @ProjectSiteIdN=SCOPE_IDENTITY()
					 set	@LastFACode =@FACode

					 
					
					 UPDATE #saveProjectPlanFlagUpdate
                     SET    Value28 = 'new',
					 Value30 = @ProjectSiteIdN
                     WHERE  Value27 = @TaskCounter		
				
					--Attach WBS with Newly created Project Site
					INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,PTaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,EstimatedEndDate,PlannedDate,TargetDate,ActualStartDate,ActualEndDate,
					CompletionPercent,BudgetCost,ActualCost,IsActive)
					SELECT @ProjectSiteIdN,pt.taskid,pt.PTaskId,0,pt.StatusId,pt.PriorityId,null,null,null,null,null,null,null,pt.BudgetCost,0 ,pt.IsActive
					FROM PM_Tasks AS pt
					WHERE   pt.ProjectId=@ProjectId --pt.IsActive=1 and
				
				        UPDATE #saveProjectPlanFlagUpdate
                        SET    Value29 = 'new'
                        WHERE  Value27 = @TaskCounter

					--	print 'Exist'
					declare @TaskIdN as integer = (select top 1 TaskId from PM_Tasks where Title=@Task and ProjectId=@ProjectId ) 
					if(@TaskIdN >0 )
					begin		
						declare @SiteTaskIdN as integer = (select top 1 SiteTaskId from PM_SiteTasks where TaskId=@TaskIdN and ProjectSiteId=@ProjectSiteIdN)
		
						if(@SiteTaskIdN >0 )
						begin			
							update PM_SiteTasks set PlannedDate=@PlanDate,EstimatedStartDate=@ForecastStartDate,EstimatedEndDate=@ForecastEndDate,TargetDate=@TargetDate
							,ActualStartDate=@ActualStartDate,ActualEndDate=@ActualEndDate,ResourcesId=@Resources,StatusId=@StatusId,PriorityId=@PriorityId,CompletionPercent=@Completion
							where SiteTaskId = @SiteTaskIdN

							UPDATE #saveProjectPlanFlagUpdate
                            SET    Value31 = @SiteTaskIdN
                            WHERE  Value27 = @TaskCounter

						end
	     else			
						begin
						UPDATE #saveProjectPlanFlagUpdate
                        SET    Value29 = 'site task not exist',
						Value31 = '0'
						WHERE  Value27 = @TaskCounter 
						end
					end
					else			
						begin
						UPDATE #saveProjectPlanFlagUpdate
                        SET    Value29 = 'task not exist',
						Value31 = '0'
                        WHERE  Value27 = @TaskCounter 
						end
					END
				end
			else 
			begin
				--if last site's task row comes in iteration
				DECLARE @ProjectSiteIdLast AS NUMERIC(18,0)=0				
				
				SELECT @ProjectSiteIdLast=(select top 1 ProjectSiteId from PM_ProjectSites where ProjectId=@ProjectId  and FACode=@LastFACode and IsActive=1)	
				declare @TaskId2 as integer = (select top 1 TaskId from PM_Tasks where Title=@Task and ProjectId=@ProjectId) 
				
				
				--Check if site exists and is active
				if (@ProjectSiteIdLast>0)
				begin
					if(@TaskId2>0 )
					begin		
						declare @SiteTaskId2 as integer = (select top 1 SiteTaskId from PM_SiteTasks where TaskId=@TaskId2 and ProjectSiteId = @ProjectSiteIdLast) 
					
						if(@SiteTaskId2 >0 )
						begin								
							--If Status is not provided
							IF ISNULL(@StatusId,0)=0
							BEGIN
								set	@StatusId = (select top 1  DefinationId from AD_Definations where keycode='PROJECT_TASK_STATUS' and DefinationName='Active' and IsActive=1)
							END
						
							--If Priority is not provided
							IF ISNULL(@PriorityId,0)=0
							BEGIN
								set	@PriorityId = (select top 1  DefinationId from AD_Definations where keycode='High' and DefinationName='High' and IsActive=1) 
							END	
							
							--SELECT @LastFaCode,@ProjectSiteIdLast,@TaskId2,@SiteTaskId2,@StatusId,@PriorityId,@Status,@Priority,@Task	

							 update PM_SiteTasks set PlannedDate=@PlanDate,EstimatedStartDate=@ForecastStartDate,EstimatedEndDate=@ForecastEndDate,TargetDate=@TargetDate
							,ActualStartDate=@ActualStartDate,ActualEndDate=@ActualEndDate,ResourcesId=@Resources,StatusId=@StatusId,PriorityId=@PriorityId,CompletionPercent=@Completion
							 where SiteTaskId = @SiteTaskId2

							UPDATE #saveProjectPlanFlagUpdate
                            SET    Value29 = 'update',
							Value30 = @ProjectSiteIdLast,
							Value31 = @SiteTaskId2
                            WHERE  Value27 = @TaskCounter
						end	
						else
						begin
						    UPDATE #saveProjectPlanFlagUpdate
                            SET    Value29 = 'site task not exist',
							Value30=@ProjectSiteIdLast,
							Value31 = '0'
                            WHERE  Value27 = @TaskCounter
						end
					end
					else			
						begin
						UPDATE #saveProjectPlanFlagUpdate
                            SET    Value29 = 'task not exist',
							Value30 = @ProjectSiteIdLast,
							Value31 = '0'
                            WHERE  Value27 = @TaskCounter
						end
					end
					else
					begin
					UPDATE #saveProjectPlanFlagUpdate
                            SET    Value29 = 'site not exist',
							Value30 = '0',
							Value31 = '0'
                            WHERE  Value27 = @TaskCounter
					end
				end
		FETCH NEXT FROM addSite INTO @FACode,@Task,@PlanDate,@ForecastStartDate,@ForecastEndDate,@TargetDate,@ActualStartDate,@ActualEndDate,@Status,@Resources,@Cluster,
			 @Market,@Latitude,@Longitude,@SiteName,@SiteType,@SiteClass,@Scope,@Description,@Status,@Priority,@Completion,@CreatedOn,@CreatedBy,@ModifiedOn,@ModifiedBy,@TaskCounter
		END 
		--CLOSE THE CURSOR.
		CLOSE addSite
		DEALLOCATE addSite


		  INSERT INTO [dbo].[PM_ProjectPlanHistory]
           ([RevisionType]
          ,[CreatedOn]
           ,[CreatedBy]
          ,[ProjectId])
        values (0,GETDATE(),@User,@ProjectId)
		
		  select @RevisionId =   SCOPE_IDENTITY()

		  INSERT INTO [dbo].[PM_ProjectPlanHistory_Details]
           ([SiteTaskId]
           ,[ProjectSiteId]
           ,[RevisionId]
           ,[Status]
           ,[Priority]
           ,[EstimatedStartDate]
           ,[EstimatedEndDate]
           ,[PlanDate]
           ,[TargetDate]
           ,[ActualStartDate]
           ,[ActualEndDate]
           ,[Completion]
           ,[FACode]
           ,[SiteName]
           ,[SiteType]
           ,[SiteRevisionType]
           ,[TaskRevisionType])
     select
           _spp.Value31
           ,_spp.Value30
           ,@RevisionId
           ,_spp.Value20
           ,_spp.Value21
           ,_spp.Value4
           ,_spp.Value5
           ,_spp.Value3
           ,_spp.Value6
           ,_spp.Value7
           ,_spp.Value8
           ,_spp.Value22
           ,_spp.Value1
           ,_spp.Value15
           ,_spp.Value16
           ,_spp.Value28
           ,_spp.Value29
		   from #saveProjectPlanFlagUpdate _spp where _spp.Value28 = 'new' or _spp.Value28 = 'update' or _spp.Value31 >0

		   update  #saveProjectPlanFlagUpdate set Value32 = @RevisionId

		   select *
		   from #saveProjectPlanFlagUpdate
		--update PM_Tasks  set ActualStartDate=(select min(ps.ActualStartDate) from PM_SiteTasks ps where ps.TaskId=TaskId),
		--		ActualEndDate=(select max(pt.ActualEndDate) from PM_SiteTasks pt where pt.TaskId=TaskId), IsEstimate=0
		--		where (select min(ps.ActualStartDate) from PM_SiteTasks ps where ps.TaskId=TaskId) <> null and
		--		(select max(ps.ActualEndDate) from PM_SiteTasks ps where ps.TaskId=TaskId) <> null 

	end