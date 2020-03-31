create PROCEDURE PM_Reports_Exports
	@ProjectId AS NUMERIC(18,0),
	@Where nvarchar(MAX)='',
	@Column nvarchar(500) = '*',
	@Group nvarchar(500)='',
	@Filter AS NVARCHAR(50)='CUSTOM_STATUS_REPORT',
	@FromDate AS DATE=NULL,
	@ToDate AS DATE=NULL,
	@Markets AS NVARCHAR(250)='',
	@Tasks AS NVARCHAR(250)=''
AS
--DECLARE @Projectid AS INT=20021
SET @Tasks+=','
SET @Markets+=','
	
IF @Filter='PROJECT_SUMMARY_REPORT'
BEGIN
	Select PM_ProjectSites.FACode, PM_ProjectSites.ClusterCode as Cluster, cty.DefinationName as Market, PM_ProjectSites.Latitude, PM_ProjectSites.Longitude,
	PM_ProjectSites.SiteName, typ.DefinationName as SiteType, PM_ProjectSites.SiteClassId, scope.DefinationName as Scope, PM_ProjectSites.Description,
	PM_Tasks.Title as Task, PM_Tasks.PlannedDate, PM_Tasks.EstimatedStartDate, PM_Tasks.EstimatedEndDate, PM_Tasks.TargetDate, PM_Tasks.ActualStartDate, PM_Tasks.ActualEndDate,
	stat.DefinationName as Status, priority.DefinationName as Priority
	from PM_ProjectSites

	Inner Join PM_SiteTasks
	on PM_SiteTasks.ProjectSiteId = PM_ProjectSites.ProjectSiteId
	Inner Join PM_Tasks
	On PM_Tasks.TaskId = PM_SiteTasks.TaskId 

	Inner JOIN AD_Definations typ on typ.DefinationId = PM_ProjectSites.SiteTypeId
	Inner JOIN AD_Definations scope on scope.DefinationId = PM_ProjectSites.ScopeId
	Inner JOIN AD_Definations cty on cty.DefinationId= PM_ProjectSites.CityId
	Inner JOIN AD_Definations stat on stat.DefinationId= PM_SiteTasks.StatusId
	Inner JOIN AD_Definations priority on priority.DefinationId= PM_SiteTasks.PriorityId
	
	where PM_ProjectSites.ProjectId = @ProjectId and PM_SiteTasks.IsActive=1
END
ELSE IF @Filter='Import_WR_Issues'
BEGIN
	select Top 100 
	--issue.IssueId,pps.ProjectSiteId,pst.SiteTaskId, pst.TaskId, pps.SiteCode,
	pps.FACode, pt.Title as Task, issue.ENB, issue.ExtendedeNB, equip.DefinationName as 'Equipment', issue.AOTSCR,
	issuecat.DefinationName as 'Category', tasktype.DefinationName as 'Type',
	AD_Clients.ClientName as 'WhoFix', issue.IsUnavoidable, activitytype.DefinationName as 'ActivityType', alarm.DefinationName as 'Alarms',
	severity.DefinationName as 'Severity', MW.DefinationName as 'MW',attachementtype.DefinationName as 'AttachmentType', issue.ItemFilePath as 'Attachment',
	issue.Description, stat.DefinationName as 'Status',
	--issue.AssignedToId as 'AssingedTo',
	SUBSTRING((SELECT DISTINCT ', '+ISNULL(usr.FirstName,'' )+' '+ISNULL( usr.LastName,'') AS [text()]
				FROM Sec_Users usr	
				WHERE  Charindex(cast(usr.UserId as varchar(max))+',', issue.AssignedToId+',')>0	
	FOR XML PATH('')), 2, 1000) 'AssingedTo',	
	priority.DefinationName as 'Priority', '' as 'ScheduleDate', issue.ActualEndDate as 'ActualDate', issue.TargetDate, issue.RequestedBy, issue.RequestDate,
	issue.Created as 'CreateDate',
	issue.CreatedBy
	from PM_Issues issue
	
	Inner Join PM_ProjectSites pps
	On pps.ProjectSiteId = issue.ProjectSiteId
	Inner Join PM_SiteTasks pst
	On pst.ProjectSiteId = pps.ProjectSiteId
	Inner Join PM_Tasks pt
	On pt.TaskId = pst.TaskId

	left Join AD_Clients
	On AD_Clients.ClientId = issue.IssueById
	left join AD_Definations equip on equip.DefinationId= issue.EquipmentId
	left join AD_Definations tasktype on tasktype.DefinationId= issue.ReasonId
	left join AD_Definations alarm on alarm.DefinationId= issue.AlarmId
	left join AD_Definations severity on severity.DefinationId= issue.SeverityId
	left join AD_Definations stat on stat.DefinationId= issue.IssueStatusId
	left join AD_Definations issuecat on issuecat.DefinationId= issue.IssueCategoryId
	left join AD_Definations priority on priority.DefinationId= issue.IssuePriorityId
	left join AD_Definations attachementtype on attachementtype.DefinationId= issue.ItemTypeId
	left join AD_Definations activitytype on activitytype.DefinationId= issue.ActivityTypeId
	left join AD_Definations MW on MW.DefinationId= issue.MSWindowId
	
	Where issue.ProjectId = @ProjectId
END
ELSE IF @Filter='Import_WR_Site'
BEGIN
	SELECT pps.FACode, act.DefinationName 'ActivityType', alm.DefinationName 'Alarm', gng.DefinationName 'GNG', '' as 'Scheduled', msw.DefinationName 'MW', itm.DefinationName 'AttachementType', psl.ItemFilePath 'Attachement',psl.[Description] 'Note', '' as 'eNB', '' as 'ExtendedENB', '' as 'EquipmentId', '' as 'AOTSCR',pps.USID,
	sts.DefinationName 'Status', psl.IsAdditional, psl.CreatedOn 'Created', psl.UserId 'CreatedBy'
	FROM PM_ProjectSites AS pps
	INNER JOIN PM_SiteLog AS psl ON psl.ProjectSiteId=pps.ProjectSiteId
	left join AD_Definations act on act.DefinationId=psl.ActivityTypeId
	left join AD_Definations sts on sts.DefinationId=psl.StatusId
	left join AD_Definations msw on msw.DefinationId=psl.MSWindowId
	left join AD_Definations alm on alm.DefinationId=psl.AlarmId
	left join AD_Definations gng on gng.DefinationId=psl.GngId
	left join AD_Definations itm on itm.DefinationId=psl.ItemTypeId 
	WHERE pps.IsActive=1 AND pps.ProjectId= @ProjectId
END