CREATE PROCEDURE PM_ManageSiteTasks
	@Filter nvarchar(50),
	@SiteTaskId NUMERIC=0,
	@ProjectSiteId NUMERIC=0,	
	@TaskId NUMERIC=0,
	@PTaskId NUMERIC=0,
	@PredecessorId NUMERIC=0,
	@TaskTypeId NUMERIC=0,
	@TaskTitle NVARCHAR(50)=NULL,
	@StatusId NUMERIC=0,
	@PriorityId NUMERIC=0,
	@ForecastDate Date=null, 
	@PlannedDate Date=null, 
	@TargetDate Date=null, 
	@ActualStartDate Date=null, 
	@ActualEndDate Date =NULL, 
	@CompletionPercent FLOAT=0.0, 
	@BudgetCost FLOAT=0.0, 
	@ActualCost FLOAT=0.0,
	@MapCode nvarchar(50)=NULL, 
	@MapColumn nvarchar(500)=NULL, 
	@IsActive BIT = 1,
	@Description NVARCHAR(500)='',
	@CreatedBy NUMERIC(18,0)=0,
	@AssignTo NVARCHAR(250)='',
	@ForecastStartDate date=null,
	@ForecastEndDate date=null,
	@TaskStageId nvarchar(255)=null,
	@List TaskList readonly
AS
BEGIN
	IF @Filter='Update_SiteTask'
	BEGIN
		 UPDATE PM_SiteTasks SET ProjectSiteId=@ProjectSiteId, 
		 PriorityId=@PriorityId, EstimatedStartDate=@ForecastDate, PlannedDate=@PlannedDate,
		 TargetDate=@TargetDate, ActualStartDate=@ActualStartDate, ActualEndDate=@ActualEndDate, CompletionPercent=@CompletionPercent, BudgetCost=@BudgetCost,
		 ActualCost=@ActualCost, IsActive = @IsActive
		 WHERE TaskId = @TaskId AND ProjectSiteId = @ProjectSiteId

	END
	ELSE IF @Filter='Update_SiteTask_PLAN'
	BEGIN
		 UPDATE PM_SiteTasks
		 SET EstimatedStartDate=@ForecastStartDate,EstimatedEndDate=@ForecastEndDate, PlannedDate=@PlannedDate,
		 TargetDate=@TargetDate, ActualEndDate=@ActualEndDate,ActualStartDate=@ActualStartDate, StatusId = @StatusId
		 WHERE SiteTaskId = @TaskId AND ProjectSiteId = @ProjectSiteId
	
		 INSERT INTO PM_SiteTaskStatus(ProjectSiteId,TaskId,SiteTaskId,StatusId,StatusDate,CreatedBy,CreatedOn,[Description])
		 SELECT @ProjectSiteId,(SELECT x.TaskId FROM PM_SiteTasks x WHERE x.SiteTaskId=@SiteTaskId),@SiteTaskId,@StatusId,GETDATE(),@CreatedBy,GETDATE(),@MapCode
		 
		 IF (@AssignTo IS NOT NULL AND @AssignTo!='')
		 BEGIN
		 	UPDATE PM_SiteResources
			SET isActive=0
			WHERE SiteTaskId = @TaskId AND ProjectSiteId = @ProjectSiteId
		 
			INSERT INTO PM_SiteResources(ProjectSiteId,SiteTaskId,AssignedById,AssignToId,AssignedDate,ForecastDate,PlanDate,IsActive)
			VALUES(@ProjectSiteId,@TaskId,@CreatedBy,@AssignTo,GETDATE(),NULL,NULL,CAST(1 AS BIT))
		 END
	END
	ELSE IF @Filter='Update_Task_Plan'
	BEGIN
		 UPDATE PM_SiteTasks
		 SET EstimatedStartDate=@ForecastDate, PlannedDate=@PlannedDate,
		 TargetDate=@TargetDate, ActualEndDate=@ActualEndDate, StatusId = @StatusId, ResourcesId = @AssignTo
		 WHERE SiteTaskId = @SiteTaskId AND ProjectSiteId = @ProjectSiteId
	
		 INSERT INTO PM_SiteTaskStatus(ProjectSiteId,TaskId,SiteTaskId,StatusId,StatusDate,CreatedBy,CreatedOn,[Description])
		 SELECT @ProjectSiteId,(SELECT x.TaskId FROM PM_SiteTasks x WHERE x.SiteTaskId=@SiteTaskId),@SiteTaskId,@StatusId,GETDATE(),@CreatedBy,GETDATE(),@MapCode
		 
		 IF (@AssignTo IS NOT NULL AND @AssignTo!='')
		 BEGIN
		 	UPDATE PM_SiteResources
			SET isActive=0
			WHERE SiteTaskId = @TaskId AND ProjectSiteId = @ProjectSiteId
		 
			INSERT INTO PM_SiteResources(ProjectSiteId,SiteTaskId,AssignedById,AssignToId,AssignedDate,ForecastDate,PlanDate,IsActive)
			VALUES(@ProjectSiteId,@TaskId,@CreatedBy,@AssignTo,GETDATE(),NULL,NULL,CAST(1 AS BIT))
		 END
	END
	else if @Filter='Update_SiteTask_PLAN_BULK'
	BEGIN
	 print '1'
	DECLARE addSite CURSOR READ_ONLY
		FOR
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12
		 FROM @List l
		--OPEN CURSOR
		OPEN addSite 
		 print '2'


		--FETCH THE RECORD INTO THE VARIABLES.
		FETCH NEXT FROM addSite INTO @ProjectSiteId,@SiteTaskId,@ActualStartDate,@ActualEndDate,@ForecastStartDate,@ForecastEndDate,@PlannedDate,@TargetDate,@StatusId,@AssignTo,@CreatedBy,@TaskStageId
		--LOOP UNTIL RECORDS ARE AVAILABLE.
		WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION				 
			
			 UPDATE PM_SiteTasks
		 SET EstimatedStartDate=@ForecastStartDate,EstimatedEndDate=@ForecastEndDate, PlannedDate=@PlannedDate,
		 TargetDate=@TargetDate, ActualEndDate=@ActualEndDate,ActualStartDate=@ActualStartDate, StatusId = @StatusId, TaskStageId = @TaskStageId
		 WHERE SiteTaskId = @SiteTaskId AND ProjectSiteId = @ProjectSiteId
	
		 INSERT INTO PM_SiteTaskStatus(ProjectSiteId,TaskId,SiteTaskId,StatusId,StatusDate,CreatedBy,CreatedOn)
		 SELECT @ProjectSiteId,(SELECT x.TaskId FROM PM_SiteTasks x WHERE x.SiteTaskId=@SiteTaskId),@SiteTaskId,(SELECT x.StatusId FROM PM_SiteTasks x WHERE x.SiteTaskId=@SiteTaskId),GETDATE(),@CreatedBy,GETDATE()
		 
		 IF (@AssignTo IS NOT NULL AND @AssignTo!='')
		 BEGIN
		 	UPDATE PM_SiteResources
			SET isActive=0
			WHERE SiteTaskId = @SiteTaskId AND ProjectSiteId = @ProjectSiteId
		 
			INSERT INTO PM_SiteResources(ProjectSiteId,SiteTaskId,AssignedById,AssignToId,AssignedDate,ForecastDate,PlanDate,IsActive)
			VALUES(@ProjectSiteId,@SiteTaskId,@CreatedBy,@AssignTo,GETDATE(),NULL,NULL,CAST(1 AS BIT))
		 END
				COMMIT
			--FETCH THE NEXT RECORD INTO THE VARIABLES.
			FETCH NEXT FROM addSite INTO @ProjectSiteId,@SiteTaskId,@ActualStartDate,@ActualEndDate,@ForecastStartDate,@ForecastEndDate,@PlannedDate,@TargetDate,@StatusId,@AssignTo,@CreatedBy,@TaskStageId
		END 
		--CLOSE THE CURSOR.
		CLOSE addSite
		DEALLOCATE addSite
		
	END
END