CREATE PROCEDURE PM_GetIssues
	@Filter NVARCHAR(50),
	@ProjectId NUMERIC=NULL,
	@IssueId NUMERIC=NULL
AS
BEGIN
--PM_GetIssues Get_Defination_Issue ,1,1
	IF @Filter='Get_Defination_Issue'
	BEGIN
	select def.DefinationId,def.DefinationName,def.KeyCode,def.ColorCode from AD_Definations def where
	def.KeyCode Like '%Issue_%' or def.KeyCode = 'PROJECT_MILESTONE' or def.KeyCode = 'PROJECT_STAGE'
	or def.KeyCode = 'REASON'
	END

	ELSE IF @Filter='GET_IssueLog'
	BEGIN

	select issuelog.* ,def.DefinationName 'Status', users.FirstName +' '+users.LastName 'UserName',users.Picture,isu.ItemFilePath,isu.ActivityTypeId,isu.IssueCategoryId,isu.ENB,isu.ExtendedeNB,
	isu.EquipmentId,isu.AOTSCR,isu.ReasonId,isu.SeverityId,isu.IssueById,isu.RequestDate 'RequestedDate',isu.AlarmId,isu.MSWindowId,isu.SeverityId,isu.RequestedBy,tsk.Title 'Task',defff.DefinationName 'Priority'
	,(select dbo.MultiUserIdsToName(isu.AssignedToId) )'AssingedTo'
	from PM_IssuesLog as issuelog
	INNER JOIN PM_Issues AS isu ON isu.IssueId=issuelog.IssueId
	left join AD_Definations def on  issuelog.StatusId = def.DefinationId
	left join PM_Tasks as tsk on isu.TaskId=tsk.TaskId
	left join AD_Definations as defff on issuelog.PriorityId=defff.DefinationId
	left join Sec_Users users on issuelog.UserId = users.UserId 

	where issuelog.IssueId = @IssueId
	END
	ELSE IF @Filter='GET_Issue'
	BEGIN
		SELECT isu.IssueId, isu.ProjectId, isu.ProjectSiteId, isu.TaskTypeId,
		       isu.TaskId, isu.ReasonId, isu.IssueById, isu.IssuePriorityId,
		       isu.IssueStatusId, isu.[Description], isu.RequestedById,
		       isu.RequestDate 'RequestedDate', isu.AssignedToId, isu.ForecastDate, isu.TargetDate,
		       isu.ActualStartDate, isu.ActualEndDate, isu.Tags, isu.IsUnavoidable,
		       isu.IssueCategoryId, isu.SeverityId, isu.eNB, isu.ExtendedeNB,
		       isu.EquipmentId, isu.AOTSCR, isu.ActivityTypeId, isu.ItemTypeId,
		       isu.ItemFilePath, isu.AlarmId, isu.MSWindowId, isu.RequestedBy, isu.StatusId,
		       ad.DefinationName 'Status'
		FROM PM_Issues AS isu
		left JOIN AD_Definations AS ad ON ad.DefinationId=isu.StatusId
		WHERE isu.IssueId=@IssueId
		ORDER BY isu.RequestDate DESC
		
		
	--select def.DefinationName 'Status',isu.ItemFilePath,isu.ActivityTypeId,isu.IssueCategoryId,isu.ENB,isu.ExtendedeNB,
	--isu.EquipmentId,isu.AOTSCR,isu.ReasonId,isu.SeverityId,isu.IssueById,isu.RequestDate 'RequestedDate',isu.AlarmId,isu.MSWindowId,isu.SeverityId,isu.RequestedBy
	--from PM_Issues AS isu
	--inner join AD_Definations def on  isu.StatusId = def.DefinationId
	--where isu.IssueId = @IssueId
	END

END