-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PM_GetTasks
 @Filter NVARCHAR(50),
 @ProjectId NUMERIC(18,0)=0,
 @TaskId NUMERIC(18,0)=0,
 @Value1 NVARCHAR(50)=NULL,
 @Value2 NVARCHAR(50)=NULL,
 @Value3 NVARCHAR(50)=NULL
AS
BEGIN
--	 [dbo].[PM_GetTasks] 'ByTaskTypeId',133320
	IF @Filter='ByTaskTypeId'
	BEGIN
		SELECT t.TaskId, t.PTaskId, t.ProjectId, t.TaskTypeId, t.StatusId,
		       t.PredecessorId, t.PriorityId, t.Title, t.ActualStartDate,
		       t.ActualEndDate, t.EstimatedStartDate, t.EstimatedEndDate,
		       t.[Description], t.IsActive, t.IsEstimate, t.TargetDate,
		       t.ForecastedSites, t.SortOrder,
		       CASE WHEN t.IsEstimate=1 THEN t.EstimatedStartDate ELSE t.ActualStartDate END 'ProjectStartDate',
		       CASE WHEN t.IsEstimate=1 THEN t.EstimatedEndDate ELSE t.ActualEndDate END 'ProjectEndDate',
		       tsk.ColorCode 'TaskTypeColor'
		FROM PM_Tasks AS t
		INNER JOIN AD_Definations AS tsk ON tsk.DefinationId=t.TaskTypeId
		WHERE t.TaskTypeId=@Value1 AND t.ProjectId=@Value2 AND t.IsActive=1
		ORDER BY t.sortorder
	END
	
  else	IF @Filter='ByTaskTypeKeyCode'
	BEGIN
			SELECT t.* ,sts.DefinationName 'Status',sts.ColorCode 'StatusColor',(select count(_child.TaskId) from PM_Tasks _child where _child.PTaskId=t.TaskId) 'ChildTasks',
            (select count(_iss.IssueId) from PM_Issues _iss where _iss.TaskId=t.TaskId) 'IssueCount'
		   ,(select count(pst.SiteTaskId) from PM_SiteTasks pst where pst.TaskId=t.TaskId 
		 and pst.StatusId=(select  distinct top 1 DefinationId from AD_Definations ad where ad.DefinationName='Completed' and KeyCode='PROJECT_TASK_STATUS' ) ) 'CompletedSiteTasks'
		FROM PM_Tasks AS t
		INNER JOIN AD_Definations AS def ON def.DefinationId=t.TaskTypeId
		INNER JOIN AD_Definations AS sts ON sts.DefinationId=t.StatusId
		WHERE t.ProjectId=@Value2 and t.IsActive=1 --AND t.IsActive=1 --and def.KeyCode=@Value1 
		ORDER BY t.sortorder
	END
	
	--else IF @Filter='ByTaskTypeId'
	--BEGIN
	--	SELECT t.*,tsk.ColorCode 'TaskTypeColor'
	--	FROM PM_Tasks AS t
	--	INNER JOIN AD_Definations AS tsk ON tsk.DefinationId=t.TaskTypeId
	--	WHERE t.TaskTypeId=@Value1 AND t.ProjectId=@Value2 AND t.IsActive=1
	--	ORDER BY t.sortorder
	--END
	
	
	else IF @Filter='ByPTaskId'
	BEGIN
		SELECT t.*,tsk.ColorCode 'TaskTypeColor',ISNULL((select max(SortOrder) from PM_Tasks where PTaskId=@Value1 and ProjectId=@Value2 ),0)+1 'maxSortOrder'
		FROM PM_Tasks AS t
		INNER JOIN AD_Definations AS tsk ON tsk.DefinationId=t.TaskTypeId
		WHERE t.PTaskId=@Value1 AND t.ProjectId=@Value2 AND t.IsActive=1
		ORDER BY t.sortorder
	END
	
	
	else IF @Filter='ByTaskId'
	BEGIN
		SELECT t.*,tsk.ColorCode 'TaskTypeColor'
		FROM PM_Tasks AS t
		INNER JOIN AD_Definations AS tsk ON tsk.DefinationId=t.TaskTypeId
		WHERE t.TaskId=@Value1 AND t.IsActive=1
		ORDER BY t.sortorder
	END
	
	else IF @Filter='maxSortOrder'
	BEGIN
		select ISNULL(max(SortOrder),0) +1 'maxSortOrder' from PM_Tasks 
       where ProjectId=@Value1 AND TaskTypeId= @Value2 
	END
	else IF @Filter='ByProjectId'
	BEGIN
		select * from PM_Tasks tsk where tsk.ProjectId =@ProjectId and tsk.PTaskId = 0
	END
	else IF @Filter='Get_Project_Tasks'
	BEGIN
		select tsk.TaskId, tsk.PTaskId, tsk.Title
		  from PM_Tasks tsk where tsk.ProjectId =@ProjectId
	END
	else IF @Filter='Get_Readiness'
	BEGIN
		select tsk.TaskId 'EntityTaskId', tsk.PTaskId 'EntityId', tsk.Title 'EntityTaskName',
		(select COUNT(DISTINCT st.ProjectSiteId) 
		FROM PM_SiteTasks as task
		inner join PM_Tasks pt on pt.TaskId=task.TaskId
		inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
		where pt.ProjectId = @ProjectId and pt.IsActive=1 and pt.TaskId =tsk.TaskId --AND ISNULL(pt.PTaskId,0)=0			
		AND task.ActualEndDate <= dateadd(day,-1, getdate())
		AND task.EstimatedEndDate IS NOT NULL 
		and task.IsActive=1) 'PreviousSitesCount',	(select COUNT(DISTINCT st.ProjectSiteId) 
		FROM PM_SiteTasks as task
		inner join PM_Tasks pt on pt.TaskId=task.TaskId
		inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
		where pt.ProjectId = @ProjectId and pt.IsActive=1 and pt.TaskId =tsk.TaskId --AND ISNULL(pt.PTaskId,0)=0			
		AND task.ActualEndDate = GETDATE() 
		AND task.EstimatedEndDate IS NOT NULL 
		and task.IsActive=1) 'CurrentSitesCount'
		from PM_Tasks tsk where tsk.ProjectId =@ProjectId
        
	END
	

END