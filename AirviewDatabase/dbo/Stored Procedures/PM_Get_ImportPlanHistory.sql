create PROCEDURE PM_Get_ImportPlanHistory
	@Filter NVARCHAR(50),
	@ProjectId NUMERIC(18,0)=0,
	@UserId NUMERIC(18,0)=0,
	@RevisionId NUMERIC(18,0)=0
AS

if (@Filter ='Get_By_ProjectId')
	begin
	select * from PM_ProjectPlanHistory where projectid = @ProjectId
	end
	if (@Filter ='Get_By_RevisionId')
	begin
	   select *,pt.Title 'Task',impd.EstimatedStartDate 'ForecastStartDate',impd.EStimatedEndDate 'ForecastEndDate' from PM_ProjectPlanHistory_Details impd
	   inner join PM_ProjectPlanHistory imph on imph.RevisionId = impd.RevisionId
	   left join PM_SiteTasks pst on pst.SiteTaskId = impd.SiteTaskId
	   left join PM_Tasks pt on pt.TaskId = pst.TaskId
       where imph.projectid = @ProjectId and impd.revisionId =  @RevisionId
	end
	if (@Filter ='Get_Last_Revision')
	begin
	select *,pt.Title 'Task',impd.EstimatedStartDate 'ForecastStartDate',impd.EStimatedEndDate 'ForecastEndDate' from PM_ProjectPlanHistory_Details impd
	   inner join PM_ProjectPlanHistory imph on imph.RevisionId = impd.RevisionId
	   left join PM_SiteTasks pst on pst.SiteTaskId = impd.SiteTaskId
	   left join PM_Tasks pt on pt.TaskId = pst.TaskId
       where imph.projectid = @ProjectId and impd.revisionId =(select top 1 max(t.RevisionId) from  PM_ProjectPlanHistory_Details t
	    inner join PM_ProjectPlanHistory imphi on imphi.RevisionId = t.RevisionId where imphi.ProjectId = @ProjectId
	   )
	end
	if (@Filter ='Get_Revision_AllProperties')
	begin
	--select distinct(impd.SiteTaskId) from PM_Tasks pt, PM_ProjectPlanHistory_Details impd
	--   inner join PM_SiteTasks pst on (pst.SiteTaskId = impd.SiteTaskId)
	--   where pt.ProjectId =70030  --order by impd.RevisionId desc

		WITH cte AS
		(   SELECT impd.*,pt.Title 'Task',impd.EstimatedStartDate 'ForecastStartDate',impd.EStimatedEndDate 'ForecastEndDate', ROW_NUMBER() OVER (PARTITION BY impd.sitetaskid  ORDER BY impd.projectplanhistoryid DESC) AS rn
			FROM PM_ProjectPlanHistory_Details impd
			inner join PM_SiteTasks pst on pst.SiteTaskId = impd.SiteTaskId
			inner join PM_Tasks pt on pt.TaskId = pst.TaskId where pt.ProjectId =@ProjectId and impd.RevisionId < @RevisionId
		)
		SELECT *
		FROM cte
		WHERE rn = 1
		order by projectplanhistoryid desc
      end