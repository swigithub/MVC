-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PM_ManageTasks
 @Filter NVARCHAR(50)
,@TaskId numeric(18, 0)=null
,@PTaskId numeric(18, 0)=0
,@ProjectId numeric(18, 0)=NULL
,@TaskTypeId numeric(18, 0)=NULL
,@StatusId numeric(18, 0)=133317
,@PriorityId numeric(18, 0)=133315
,@PredecessorId numeric(18, 0)=0
,@Title nvarchar(50)=NULL
,@PlannedDate  DATETIME=NULL
,@TargetDate  DATETIME=NULL
,@ActualStartDate  DATETIME=NULL
,@ActualEndDate  DATETIME=NULL
,@EstimatedStartDate  DATETIME=NULL
,@EstimatedEndDate  DATETIME=NULL
,@Description nvarchar(250)=NULL
,@IsEstimate BIT=NULL
,@ForecastedSites INT=NULL
,@CompletionPercent FLOAT=NULL
,@BudgetCost FLOAT=NULL
,@ActualCost FLOAT=NULL
,@MapCode nvarchar(50)=NULL
,@MapColumn nvarchar(50)=NULL
,@Color nvarchar(50)=NULL
,@IsActive BIT=1
,@ScopeId NUMERIC(18,0)=NULL
,@IsStartMilestone BIT=NULL
,@IsEndMilestone BIT=NULL
,@SortOrder numeric(18, 0)=NULL 
,@SortDirection nvarchar(50)=NULL     --- UP, DOWN,
,@Level numeric(18,0)=0
,@Duration numeric(18,0)=0
,@SavingType nvarchar(50)='' 
,@List TaskList readonly

AS
BEGIN
	declare @TEMP table (ID int)
	DECLARE @RETURN_VALUE int = 0 
	IF @Filter='Insert'
	BEGIN
		INSERT INTO PM_Tasks(PTaskId,ProjectId,TaskTypeId,StatusId,PriorityId,PredecessorId,Title,PlannedDate,TargetDate,ActualStartDate,ActualEndDate,EstimatedStartDate,EstimatedEndDate,DESCRIPTION
		,IsEstimate,ForecastedSites,CompletionPercent,BudgetCost,ActualCost,MapCode,MapColumn,Color,IsActive,ScopeId,IsStartMilestone,IsEndMilestone,SortOrder)
		VALUES(@PTaskId,@ProjectId,@TaskTypeId,@StatusId,@PriorityId,@PredecessorId,@Title,@PlannedDate,@TargetDate,@ActualStartDate,@ActualEndDate,@EstimatedStartDate,@EstimatedEndDate,@Description,@IsEstimate
		,@ForecastedSites,@CompletionPercent,@BudgetCost,@ActualCost,@MapCode,@MapColumn,@Color,@IsActive,@ScopeId,@IsStartMilestone,@IsEndMilestone,@SortOrder)
			   
		set @RETURN_VALUE = SCOPE_IDENTITY()
	END
	
    else	IF @Filter='Update'
	BEGIN
		UPDATE PM_Tasks
		SET
			-- TaskId -- this column value is auto-generated
			PTaskId = @PTaskId,ProjectId = @projectId,TaskTypeId = @TaskTypeId,StatusId = @statusId,PredecessorId = @PredecessorId,PriorityId = @priorityId,Title = @Title,ActualStartDate = @ActualStartDate,
			ActualEndDate = @ActualEndDate,EstimatedStartDate = @EstimatedStartDate,EstimatedEndDate = @EstimatedEndDate,[Description] = @description,IsActive = @isactive,IsEstimate = @isestimate,TargetDate = @targetDate,
			ForecastedSites = @forecastedSites,CompletionPercent = @completionPercent,ScopeId=@ScopeId,IsEndMilestone=@IsEndMilestone,IsStartMilestone=@IsStartMilestone,Color = @Color
			WHERE TaskId=@TaskId
			
			set @RETURN_VALUE =@TaskId
		
	END

	ELSE IF @Filter='UpdateSortOrder'
	BEGIN		
		DECLARE @tmpProjectId AS NUMERIC(18,0)
		DECLARE @tmpPTaskId AS NUMERIC(18,0)
		DECLARE @tmpSortOrder AS INT
		
		DECLARE @updSortOrder AS NUMERIC(18,0)
		DECLARE @updTaskId AS INT
		
		SELECT @tmpProjectId=pt.ProjectId, @tmpPTaskId=pt.PTaskId,@tmpSortOrder=pt.SortOrder
		FROM PM_Tasks AS pt
		WHERE pt.TaskId=@TaskId 

		SELECT @tmpProjectId,@tmpPTaskId,@tmpSortOrder,@updSortOrder,@updTaskId
		
		IF ISNULL(@tmpPTaskId,0)>0
		BEGIN
			SELECT @updTaskId=pt.TaskId, @updSortOrder=pt.SortOrder
			FROM PM_Tasks AS pt
			WHERE pt.ProjectId=@tmpProjectId AND pt.PTaskId=@tmpPTaskId AND pt.SortOrder=(CASE WHEN @SortDirection='UP' THEN @tmpSortOrder-1 ELSE @tmpSortOrder+1 END)
		END
		ELSE
		BEGIN
			SELECT @updTaskId=pt.TaskId, @updSortOrder=pt.SortOrder
			FROM PM_Tasks AS pt
			WHERE pt.ProjectId=@tmpProjectId AND (pt.PTaskId IS NULL OR pt.PTaskId=0) AND pt.SortOrder=(CASE WHEN @SortDirection='UP' THEN @tmpSortOrder-1 ELSE @tmpSortOrder+1 END)
		END

		SELECT @updTaskId,@updSortOrder
			
		UPDATE PM_Tasks
		SET SortOrder=(CASE WHEN @SortDirection='UP' THEN @tmpSortOrder-1 ELSE @tmpSortOrder+1 END) 
		WHERE TaskId=@TaskId
		
		UPDATE PM_Tasks
		SET SortOrder=(CASE WHEN @SortDirection='UP' THEN @updSortOrder+1 ELSE @updSortOrder-1 END) 
		WHERE TaskId=@updTaskId



			
		SET @RETURN_VALUE =@TaskId		
	END
		IF @Filter='Insert2'	
	BEGIN
	BEGIN TRANSACTION	
	 print '1'
	DECLARE addSite CURSOR READ_ONLY
		FOR
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14,l.Value15,
		       l.Value16,l.Value17,l.Value18,l.Value19,l.Value20,l.Value21,l.Value22,l.Value23,l.Value24,l.Value25,l.Value26,l.Value27,l.Value28,l.Value29,l.Value30,l.Value31
		 FROM @List l
		--OPEN CURSOR
		OPEN addSite 
		 print '2'


		--FETCH THE RECORD INTO THE VARIABLES.
		FETCH NEXT FROM addSite INTO @TaskId,@PTaskId,@ProjectId,@TaskTypeId,@StatusId,@PriorityId,@PredecessorId,@Title,@PlannedDate,@TargetDate,@ActualStartDate,@ActualEndDate,@EstimatedStartDate,@EstimatedEndDate,@DESCRIPTION
		,@IsEstimate,@ForecastedSites,@CompletionPercent,@BudgetCost,@ActualCost,@MapCode,@MapColumn,@Color,@IsActive,@ScopeId,@IsStartMilestone,@IsEndMilestone,@SortOrder,@Level,@Duration,@SavingType
		--LOOP UNTIL RECORDS ARE AVAILABLE.
		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF ISNULL(@StatusId,0)=0
			BEGIN
				set	@StatusId = (select top 1  DefinationId from AD_Definations where keycode='PROJECT_TASK_STATUS' and DefinationName='Active' and IsActive=1)
			END
						
			--If Priority is not provided
			IF ISNULL(@PriorityId,0)=0
			BEGIN
				set	@PriorityId = (select top 1  DefinationId from AD_Definations where keycode='High' and DefinationName='High' and IsActive=1) 
			END	
		--	BEGIN TRANSACTION				 
				IF @TaskId = 0
	BEGIN
			  print 'new'
	--	if(@SavingType = 'Mobile')
	--	begin
		declare @MaxSortId int = (select CASE WHEN max(SortOrder) IS NOT NULL THEN  max(SortOrder)   ELSE 1 END   from PM_Tasks where ProjectId=@ProjectId)
		if(@SortOrder = @MaxSortId)
			begin
		    set	@SortOrder = @SortOrder+1
			end
			else if(@SortOrder > @MaxSortId)
			begin
		    set	@SortOrder = @MaxSortId+1
			end
			else if(@SortOrder = (select CASE WHEN max(SortOrder) IS NOT NULL THEN  max(SortOrder)   ELSE 1 END   from PM_Tasks where ProjectId=@ProjectId and SortOrder=@SortOrder))
			begin
			set @SortOrder =@MaxSortId+1
			end
		--end

		INSERT INTO PM_Tasks(PTaskId,ProjectId,TaskTypeId,StatusId,PriorityId,PredecessorId,Title,PlannedDate,TargetDate,ActualStartDate,ActualEndDate,EstimatedStartDate,EstimatedEndDate,DESCRIPTION
		,IsEstimate,ForecastedSites,CompletionPercent,BudgetCost,ActualCost,MapCode,MapColumn,Color,IsActive,ScopeId,IsStartMilestone,IsEndMilestone,SortOrder,[Level],Duration)
		VALUES(@PTaskId,@ProjectId,@TaskTypeId,@StatusId,@PriorityId,@PredecessorId,@Title,convert(datetime,@PlannedDate ,105) ,convert(datetime,@TargetDate, 105) ,convert(datetime,@ActualStartDate ,105),convert(datetime,@ActualEndDate ,105),convert(datetime,@EstimatedStartDate ,105) ,convert(datetime,@EstimatedEndDate ,105) ,@Description,@IsEstimate
		,@ForecastedSites,@CompletionPercent,@BudgetCost,@ActualCost,@MapCode,@MapColumn,@Color,@IsActive,@ScopeId,@IsStartMilestone,@IsEndMilestone,@SortOrder,@Level,@Duration)
		set @RETURN_VALUE = SCOPE_IDENTITY()
		insert into @TEMP (id) values (@RETURN_VALUE)

		INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,
		CompletionPercent,BudgetCost,ActualCost,IsActive,PTaskId)
		SELECT pst.ProjectSiteId,pt.TaskId,0,pt.StatusId, pt.PriorityId,0,pt.BudgetCost,0,pt.IsActive,pt.PTaskId
		FROM PM_Tasks AS pt cross join PM_ProjectSites pst
		WHERE pt.TaskId=@RETURN_VALUE and pst.IsActive=1 and pst.ProjectId=@projectId



	END
	
    else	IF @TaskId > 0
	BEGIN
	 print 'UPDATE'
		UPDATE PM_Tasks
		SET
			PTaskId = @PTaskId,ProjectId = @projectId,TaskTypeId = @TaskTypeId,StatusId = @statusId,PredecessorId = @PredecessorId,PriorityId = @priorityId,Title = @Title,ActualStartDate =  convert(datetime,@ActualStartDate ,105) ,
			ActualEndDate = convert(datetime,@ActualEndDate ,105) ,EstimatedStartDate = convert(datetime,@EstimatedStartDate ,105) ,EstimatedEndDate = convert(datetime,@EstimatedEndDate ,105) ,[Description] = @description,IsActive = @IsActive,IsEstimate = @isestimate,TargetDate =  convert(datetime,@targetDate ,105) ,
			ForecastedSites = @forecastedSites,CompletionPercent = @completionPercent,ScopeId=@ScopeId,IsEndMilestone=@IsEndMilestone,IsStartMilestone=@IsStartMilestone,Color = @Color,MapColumn=@MapColumn,MapCode=@MapCode,BudgetCost=@BudgetCost,[Level]=@Level,SortOrder=@SortOrder,Duration=@Duration
			WHERE TaskId=@TaskId
				insert into @TEMP (id) values (@TaskId)

			    declare @Sitetask int= 0
				if EXISTS(SELECT top 1 ProjectSiteId FROM PM_SiteTasks where TaskId = @TaskId)
				begin
				UPDATE PM_SiteTasks SET PTaskId = @PTaskId WHERE TaskId=@TaskId
				end
				else
				begin
				INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,EstimatedEndDate,PlannedDate,TargetDate,ActualStartDate,ActualEndDate,
		CompletionPercent,BudgetCost,ActualCost,IsActive,PTaskId)
		SELECT pst.ProjectSiteId,pt.TaskId,0,pt.StatusId, pt.PriorityId,
			    pt.EstimatedStartDate, pt.EstimatedEndDate, pt.PlannedDate,
			    pt.TargetDate, NULL, NULL,0,pt.BudgetCost,0,pt.IsActive,PT.PTaskId
		FROM PM_Tasks AS pt cross join PM_ProjectSites pst
		WHERE pt.TaskId=@TaskId and pst.IsActive=1 and pst.ProjectId=@projectId
				end
			
	END

		
			
				--COMMIT
			--FETCH THE NEXT RECORD INTO THE VARIABLES.
			FETCH NEXT FROM addSite INTO @TaskId,@PTaskId,@ProjectId,@TaskTypeId,@StatusId,@PriorityId,@PredecessorId,@Title,@PlannedDate,@TargetDate,@ActualStartDate,@ActualEndDate,@EstimatedStartDate,@EstimatedEndDate,@DESCRIPTION
		,@IsEstimate,@ForecastedSites,@CompletionPercent,@BudgetCost,@ActualCost,@MapCode,@MapColumn,@Color,@IsActive,@ScopeId,@IsStartMilestone,@IsEndMilestone,@SortOrder,@Level,@Duration,@SavingType
		END 
		--CLOSE THE CURSOR.
		CLOSE addSite
		DEALLOCATE addSite

		--update PM_Tasks
		--set ActualStartDate=(select min(ps.ActualStartDate) from PM_SiteTasks ps where ps.TaskId=PM_Tasks.TaskId AND ps.IsActive=1 AND ps.ActualStartDate IS NOT NULL),
		--ActualEndDate=(select max(pt.ActualEndDate) from PM_SiteTasks pt where pt.TaskId=PM_Tasks.TaskId  AND pt.IsActive=1 AND pt.ActualEndDate IS NOT NULL),
		--IsEstimate=0
		--WHERE ProjectId=@ProjectId


		select * from   @TEMP
		--set @RETURN_VALUE =0
		COMMIT
	END

	IF @Filter='UpdateParent'	
	BEGIN
	 print '1'
	
		DECLARE addSite CURSOR READ_ONLY
		FOR
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14,l.Value15,
		       l.Value16,l.Value17,l.Value18,l.Value19,l.Value20,l.Value21,l.Value22,l.Value23,l.Value24,l.Value25,l.Value26,l.Value27,l.Value28
		 FROM @List l
		--OPEN CURSOR
		OPEN addSite 
		 print '2'


		--FETCH THE RECORD INTO THE VARIABLES.
		FETCH NEXT FROM addSite INTO @TaskId,@PTaskId,@ProjectId,@TaskTypeId,@StatusId,@PriorityId,@PredecessorId,@Title,@PlannedDate,@TargetDate,@ActualStartDate,@ActualEndDate,@EstimatedStartDate,@EstimatedEndDate,@DESCRIPTION
		,@IsEstimate,@ForecastedSites,@CompletionPercent,@BudgetCost,@ActualCost,@MapCode,@MapColumn,@Color,@IsActive,@ScopeId,@IsStartMilestone,@IsEndMilestone,@SortOrder
		--LOOP UNTIL RECORDS ARE AVAILABLE.
		WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION				 
	
    	
	 print 'UPDATE'
		UPDATE PM_Tasks
		SET
			PTaskId = @PTaskId,PredecessorId = @PredecessorId WHERE TaskId=@TaskId
				insert into @TEMP (id) values (@TaskId)

				  UPDATE PM_SiteTasks SET PTaskId = @PTaskId WHERE TaskId=@TaskId
			
				COMMIT
			--FETCH THE NEXT RECORD INTO THE VARIABLES.
			FETCH NEXT FROM addSite INTO @TaskId,@PTaskId,@ProjectId,@TaskTypeId,@StatusId,@PriorityId,@PredecessorId,@Title,@PlannedDate,@TargetDate,@ActualStartDate,@ActualEndDate,@EstimatedStartDate,@EstimatedEndDate,@DESCRIPTION
		,@IsEstimate,@ForecastedSites,@CompletionPercent,@BudgetCost,@ActualCost,@MapCode,@MapColumn,@Color,@IsActive,@ScopeId,@IsStartMilestone,@IsEndMilestone,@SortOrder
		END 
		--CLOSE THE CURSOR.
		CLOSE addSite
		DEALLOCATE addSite
		select * from   @TEMP
		--set @RETURN_VALUE =0
	END
	--select * from   @TEMP
	 else	IF @Filter='Active_Deactive'
	BEGIN
		UPDATE PM_Tasks
		SET
			IsActive = @isactive,StatusId = @statusId WHERE TaskId=@TaskId
			set @RETURN_VALUE =@TaskId
	    END
END