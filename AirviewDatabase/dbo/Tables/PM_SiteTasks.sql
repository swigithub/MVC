CREATE TABLE [dbo].[PM_SiteTasks] (
    [SiteTaskId]         NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ProjectSiteId]      NUMERIC (18)   NULL,
    [TaskId]             NUMERIC (18)   NULL,
    [PTaskId]            NUMERIC (18)   NULL,
    [RevisionId]         INT            CONSTRAINT [DF_PM_SiteTasks_RevisionId] DEFAULT ((0)) NULL,
    [StatusId]           NUMERIC (18)   NULL,
    [PriorityId]         NUMERIC (18)   NULL,
    [EstimatedStartDate] DATETIME       NULL,
    [EstimatedEndDate]   DATETIME       NULL,
    [PlannedDate]        DATETIME       NULL,
    [TargetDate]         DATETIME       NULL,
    [ActualStartDate]    DATETIME       NULL,
    [ActualEndDate]      DATETIME       NULL,
    [CompletionPercent]  FLOAT (53)     NULL,
    [BudgetCost]         FLOAT (53)     NULL,
    [ActualCost]         FLOAT (53)     NULL,
    [IsActive]           BIT            NULL,
    [ResourcesId]        NVARCHAR (100) NULL,
    [TaskStageId]        INT            NULL,
    CONSTRAINT [PK_PM_SiteTasks] PRIMARY KEY CLUSTERED ([SiteTaskId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_PM_SiteTasks_1]
    ON [dbo].[PM_SiteTasks]([ProjectSiteId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PM_SiteTasks]
    ON [dbo].[PM_SiteTasks]([TaskId] ASC);


GO
CREATE trigger PM_SiteTasks_Trigger  on PM_SiteTasks
after insert,update 
as
begin
	declare @MinEstimatedStartDate NVARCHAR(100)=''
	declare @MaxEstimatedEndDate NVARCHAR(100)=''
	declare @MinActualStartDate NVARCHAR(100)=''
	declare @MaxActualEndDate NVARCHAR(100)=''
	declare @MinPlanStartDate NVARCHAR(100)=''
	declare @MaxTargetDate NVARCHAR(100)=''
	declare @CompletionPercent int=0
	declare @TaskId int=0
	declare @PTaskId int=0
	declare @ProjectId int=0
	declare @ProjectSiteId int=0
	
	SELECT @ProjectSiteId = INSERTED.ProjectSiteId from INSERTED
	SELECT @TaskId = INSERTED.TaskId from INSERTED
	SELECT @PTaskId = INSERTED.PTaskId from INSERTED
	SELECT @ProjectId = (select top 1 projectid from PM_ProjectSites where ProjectSiteId=@ProjectSiteId)

	declare @Count int = 0
	select @Count = (select count(*) from PM_SiteTasks st where st.TaskId=@TaskId and st.IsActive=1)
	
	if @Count >0
	begin
		select @CompletionPercent =((cast((select count(*) from PM_SiteTasks st where st.TaskId=@TaskId and st.IsActive=1 and
		(select top 1 DefinationName  from AD_Definations df where df.DefinationId = st.StatusId) = 'Completed') as float) /
		cast((select count(*) from PM_SiteTasks st where st.TaskId=@TaskId and st.IsActive=1) as float)) *100)
	end
	
	update PM_Tasks  
	set PM_Tasks.ActualStartDate=(select min(ps.ActualStartDate) from PM_SiteTasks ps where ps.TaskId=@TaskId  and ps.IsActive=1),
	PM_Tasks.ActualEndDate=(select max(pt.ActualEndDate) from PM_SiteTasks pt where pt.TaskId=@TaskId  and pt.IsActive=1),
	PM_Tasks.IsEstimate=0,
	PM_Tasks.CompletionPercent = @CompletionPercent
	where PM_Tasks.TaskId=@TaskId and PM_Tasks.ProjectId=@ProjectId

	if(@PTaskId > 0 )
	begin
		set @MinEstimatedStartDate =(select distinct min(EstimatedStartDate) from PM_SiteTasks where PTaskId=@PTaskId AND ProjectSiteId=@ProjectSiteId)
		set @MaxEstimatedEndDate= COALESCE((select distinct max(EstimatedEndDate) from PM_SiteTasks where PTaskId=@PTaskId AND ProjectSiteId=@ProjectSiteId),(select distinct max(ActualEndDate) from PM_SiteTasks where PTaskId=@PTaskId AND ProjectSiteId=@ProjectSiteId))
		set @MinActualStartDate = (select distinct min(ActualStartDate) from PM_SiteTasks where PTaskId=@PTaskId AND ProjectSiteId=@ProjectSiteId)
		set @MaxActualEndDate = (select distinct max(ActualEndDate) from PM_SiteTasks where PTaskId=@PTaskId AND ProjectSiteId=@ProjectSiteId)
		set @MinPlanStartDate =(select distinct min(PlannedDate) from PM_SiteTasks where PTaskId=@PTaskId AND ProjectSiteId=@ProjectSiteId)
		set @MaxTargetDate =(select distinct max(TargetDate) from PM_SiteTasks where PTaskId=@PTaskId AND ProjectSiteId=@ProjectSiteId)
		print(@ProjectSiteId)
		update PM_SiteTasks set EstimatedStartDate=@MinEstimatedStartDate,EstimatedEndDate=@MaxEstimatedEndDate,ActualStartDate=@MinActualStartDate
		,ActualEndDate=@MaxActualEndDate,PlannedDate=@MinPlanStartDate,TargetDate=@MaxTargetDate
		where TaskId=@PTaskId and ProjectSiteId=@ProjectSiteId	
	end
	else
	begin
		set @MinEstimatedStartDate =(select distinct min(EstimatedStartDate) from PM_SiteTasks where PTaskId=0 AND ProjectSiteId=@ProjectSiteId )
		set @MaxEstimatedEndDate= COALESCE((select distinct max(EstimatedEndDate) from PM_SiteTasks where PTaskId=0 AND ProjectSiteId=@ProjectSiteId ),(select distinct max(ActualEndDate) from PM_SiteTasks where PTaskId=0 AND ProjectSiteId=@ProjectSiteId ))
		set @MinActualStartDate = (select distinct min(ActualStartDate) from PM_SiteTasks where PTaskId=0 AND ProjectSiteId=@ProjectSiteId )
		set @MaxActualEndDate = (select distinct max(ActualEndDate) from PM_SiteTasks where PTaskId=0 AND ProjectSiteId=@ProjectSiteId )
		set @MinPlanStartDate =(select distinct min(PlannedDate) from PM_SiteTasks where PTaskId=0 AND ProjectSiteId=@ProjectSiteId  )
		set @MaxTargetDate =(select distinct max(TargetDate) from PM_SiteTasks where PTaskId=0 AND ProjectSiteId=@ProjectSiteId )
		update PM_ProjectSites set EstimatedStartDate=@MinEstimatedStartDate,EstimatedEndDate=@MaxEstimatedEndDate,ActualStartDate=@MinActualStartDate
		,ActualEndDate=@MaxActualEndDate,PlannedDate=@MinPlanStartDate,TargetDate=@MaxTargetDate
		where  ProjectSiteId=@ProjectSiteId
	end
end