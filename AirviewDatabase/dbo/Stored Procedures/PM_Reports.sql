CREATE PROCEDURE PM_Reports
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
--"[PM_Reports_Exports]-PROJECT_SUMMARY_REPORT <> created for [[PM_Reports]]-PROJECT_SUMMARY_REPORT <>. Need feedback to remove filter <>."
IF @Filter='PROJECT_SUMMARY_REPORT'
BEGIN
	SELECT  sit.*,
			tss.PlannedDate 'TSSPlanDate', tss.EstimatedStartDate 'TSSForecastDate', tss.TargetDate 'TSSTargetDate', tss.ActualEndDate 'TSSActualDate',
			ssm.PlannedDate 'SSMPlanDate', ssm.EstimatedStartDate 'SSMForecastDate', ssm.TargetDate 'SSMTargetDate', ssm.ActualEndDate 'SSMActualDate',
			epl.PlannedDate 'EPLPlanDate', epl.EstimatedStartDate 'EPLForecastDate', epl.TargetDate 'EPLTargetDate', epl.ActualEndDate 'EPLActualDate',
			pinst.PlannedDate 'PreInstallPlanDate', pinst.EstimatedStartDate 'PreInstallForecastDate', pinst.TargetDate 'PreInstallTargetDate', pinst.ActualEndDate 'PreInstallActualDate',
			mig.PlannedDate 'MigrationPlanDate', mig.EstimatedStartDate 'MigrationForecastDate', mig.TargetDate 'MigrationTargetDate', mig.ActualEndDate 'MigrationActualDate'
	FROM
	(
		select  st.ProjectSiteId,st.ProjectId, st.SiteCode, st.SiteName,
			st.SiteDate, st.SiteTypeId, typ.DefinationName 'SiteType', st.Latitude,
			st.Longitude,st.PMRefId, st.ClusterCode,
			st.CityId, cty.DefinationName 'Market', st.StatusId, sts.DefinationName 'SiteStatus',
			st.MSWindowId, wnd.DefinationName 'MaintenanceWindow', st.PriorityId, pty.DefinationName 'Priority',
			st.CreatedOn, st.CreatedBy, st.IsActive,
			st.[Description], st.ClientId, cln.ClientName, st.[Address], st.FACode,
			st.USID, st.CommonId 'eNB', st.vMME, st.ControlledIntro, st.SuperBowl,
			st.isDASInBuild, st.FirstNetRAN, st.IPlanJob, st.PaceNo,
			st.IPlanIssueDate, st.SubMarket
		from PM_ProjectSites as st
		INNER JOIN AD_Clients as cln on cln.ClientId = st.ClientId		
		INNER JOIN AD_Definations cty on cty.DefinationId= st.CityId
		INNER JOIN AD_Definations rgn on rgn.DefinationId= cty.PDefinationId
		LEFT JOIN AD_Definations sts on sts.DefinationId = st.StatusId	
		LEFT JOIN AD_Definations typ on typ.DefinationId = st.SiteTypeId				
		LEFT JOIN AD_Definations wnd on wnd.DefinationId = st.MSWindowId
		LEFT JOIN AD_Definations pty on pty.DefinationId = st.PriorityId
		WHERE st.ProjectId=@ProjectId AND st.IsActive=1
		--AND st.CreatedOn BETWEEN @FromDate AND @ToDate
		--AND st.CreatedOn BETWEEN '2017-06-01' AND '2018-01-31'
		--AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
	) sit
	LEFT OUTER JOIN
	(
		--TSS
		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
		FROM PM_SiteTasks AS pst
		WHERE pst.IsActive=1 AND pst.TaskId=50076 AND pst.ActualEndDate IS NOT NULL
	) tss ON sit.ProjectSiteId=tss.ProjectSiteId
	LEFT OUTER JOIN
	(
		--SSM
		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
		FROM PM_SiteTasks AS pst
		WHERE pst.IsActive=1 AND pst.TaskId=50077 AND pst.ActualEndDate IS NOT NULL
	) ssm ON sit.ProjectSiteId=ssm.ProjectSiteId
	LEFT OUTER JOIN
	(
		--EPL
		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
		FROM PM_SiteTasks AS pst
		WHERE pst.IsActive=1 AND pst.TaskId=5008 AND pst.ActualEndDate IS NOT NULL
	) epl ON sit.ProjectSiteId=epl.ProjectSiteId
	LEFT OUTER JOIN
	(
		--PreInstall
		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
		FROM PM_SiteTasks AS pst
		WHERE pst.IsActive=1 AND pst.TaskId=50079 AND pst.ActualEndDate IS NOT NULL
	) pinst ON sit.ProjectSiteId=pinst.ProjectSiteId
	LEFT OUTER JOIN
	(
		--Migrations
		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
		FROM PM_SiteTasks AS pst
		WHERE pst.IsActive=1 AND pst.TaskId=50080 AND pst.ActualEndDate IS NOT NULL
	) mig ON sit.ProjectSiteId=mig.ProjectSiteId
END
ELSE IF @Filter='CUSTOM_STATUS_REPORT'
BEGIN
		SELECT  sit.*,tsk.PlannedDate 'PlanDate', tsk.EstimatedStartDate 'ForecastDate', tsk.TargetDate 'TargetDate', tsk.ActualEndDate 'ActualDate'
	FROM
	(
		select  st.ProjectSiteId,st.ProjectId, st.SiteCode, st.SiteName,
			st.SiteDate, st.SiteTypeId, typ.DefinationName 'SiteType', st.Latitude,
			st.Longitude,st.PMRefId, st.ClusterCode,
			st.CityId, cty.DefinationName 'Market', st.StatusId, sts.DefinationName 'SiteStatus',
			st.MSWindowId, wnd.DefinationName 'MaintenanceWindow', st.PriorityId, pty.DefinationName 'Priority',
			st.CreatedOn, st.CreatedBy, st.IsActive,
			st.[Description], st.ClientId, cln.ClientName, st.[Address], st.FACode,
			st.USID, st.CommonId 'eNB', st.vMME, st.ControlledIntro, st.SuperBowl,
			st.isDASInBuild, st.FirstNetRAN, st.IPlanJob, st.PaceNo,
			st.IPlanIssueDate, st.SubMarket
		from PM_ProjectSites as st
		INNER JOIN AD_Clients as cln on cln.ClientId = st.ClientId		
		INNER JOIN AD_Definations cty on cty.DefinationId= st.CityId
		INNER JOIN AD_Definations rgn on rgn.DefinationId= cty.PDefinationId
		LEFT JOIN AD_Definations sts on sts.DefinationId = st.StatusId	
		LEFT JOIN AD_Definations typ on typ.DefinationId = st.SiteTypeId				
		LEFT JOIN AD_Definations wnd on wnd.DefinationId = st.MSWindowId
		LEFT JOIN AD_Definations pty on pty.DefinationId = st.PriorityId
		WHERE st.ProjectId=@ProjectId AND st.IsActive=1
		AND st.CreatedOn BETWEEN @FromDate AND @ToDate
		--AND st.CreatedOn BETWEEN '2017-11-11' AND '2018-02-25'
		--AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
	) sit
	LEFT OUTER JOIN
	(
		--TSS
		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
		FROM PM_SiteTasks AS pst
		WHERE pst.IsActive=1
		AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
		AND pst.ActualEndDate IS NOT NULL
	) tsk ON sit.ProjectSiteId=tsk.ProjectSiteId
END
ELSE IF @Filter='ISSUE_BY_REPORT'
BEGIN
	DECLARE @totalProjectIssues AS INT=(SELECT COUNT(isu.IssueId)FROM PM_Issues isu INNER JOIN PM_ProjectSites sit ON sit.ProjectSiteId=isu.ProjectSiteId AND sit.ProjectId=isu.ProjectId WHERE isu.ProjectId=@ProjectId)
		
	SELECT x.*
	FROM
	(
	SELECT sit.ProjectId, ISNULL(ac.ClientId,0) 'IssueById', 
	CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'IssueBy',
	(COUNT(IssueId)*100.00)/@totalProjectIssues 'TotalSite',COUNT(IssueId) 'IssueCount',
	(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE 'Un-Avoidable' END) 'ColorCode'
	from PM_Issues issue 
	inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId	
	LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
	where issue.ProjectId = @ProjectId
	AND issue.RequestDate BETWEEN @FromDate AND @ToDate 
	--AND issue.RequestDate BETWEEN '2017-01-01' AND '2018-02-25'
	AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
	Group By sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
	)x
	WHERE x.IssueBy IS NOT NULL
END
ELSE IF @Filter='NATIONAL_ISSUES'
BEGIN
	SELECT x.*
	FROM
	(
	select 0 'DefinationId', issue.RequestDate 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
	CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue',
	COUNT(IssueId) 'TotalSite',
	(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
	from PM_Issues issue 
	inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
	inner join AD_Definations cty on cty.DefinationId=sit.CityId
	inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
	LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
	where issue.ProjectId = @ProjectId
	--AND issue.RequestDate BETWEEN '2017-01-01' AND '2018-02-25'
	AND issue.RequestDate BETWEEN @FromDate AND @ToDate
	AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
	and reg.IsActive=1
	Group By issue.RequestDate, sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
	) x
	WHERE x.Issue IS NOT NULL
END
--"[PM_Reports_Exports]-Import_WR_Issues <> created for [[PM_Reports]]-Import_WR_Issues <>. Need feedback to remove filter <>."
ELSE IF @Filter='Import_WR_Issues'
BEGIN
	SELECT TOP 100 st.FACode,issue.Description 'Note',cl.ClientName 'IssueOwner',
	CASE WHEN issue.IsUnavoidable=CAST(1 as bit) then 'Un-Avoidable' ELSE '' END 'IsUnavoidable',pri.DefinationName 'Priority',cat.DefinationName 'Category',sts.DefinationName 'Status',
	rsn.DefinationName 'Reason',
	ISNULL((
		select CASE WHEN def.DefinationName='Close' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END
		from AD_Definations def
		WHERE def.DefinationId=(SELECT TOP 1 pil.StatusId FROM PM_IssuesLog AS pil WHERE pil.IssueId=issue.IssueId ORDER BY pil.CreatedOn DESC)
	),CAST(0 AS BIT)) 'LogStatus',		
	SUBSTRING((SELECT DISTINCT ', '+ISNULL(usr.FirstName,'' )+' '+ISNULL( usr.LastName,'') AS [text()]
				FROM Sec_Users usr	
				WHERE  Charindex(cast(usr.UserId as varchar(max))+',', issue.AssignedToId+',')>0	
	FOR XML PATH('')), 2, 1000) 'AssingedTo',		
	issue.TargetDate,issue.RequestDate 'CreatedOn',ISNULL(issue.ItemFilePath,'') 'ItemFilePath'
	from PM_Issues issue
	inner join AD_Clients cl on cl.ClientId=issue.IssueById
	inner join PM_ProjectSites st on st.ProjectSiteId=issue.ProjectSiteId
	left join AD_Definations pri on pri.DefinationId=issue.IssuePriorityId 
	left join AD_Definations sts on sts.DefinationId= issue.IssueStatusId
	left join AD_Definations cat on cat.DefinationId= issue.IssueCategoryId
	left join AD_Definations rsn on rsn.DefinationId= issue.ReasonId
	where issue.ProjectId = @ProjectId 
	Order by RequestDate asc
END
--"[PM_Reports_Exports]-Import_WR_Site <> created for [[PM_Reports]]-Import_WR_Site <>. Need feedback to remove filter <>."
ELSE IF @Filter='Import_WR_Site'
BEGIN
	SELECT pps.FACode, pps.USID, pps.CommonId, psl.[Description] 'Note',sts.DefinationName 'Status',msw.DefinationName 'MaintenanceWindow',alm.DefinationName 'Alarm',
	gng.DefinationName 'Go_NoGo',itm.DefinationName 'ItemType',psl.ItemFilePath,psl.CreatedOn
	FROM PM_ProjectSites AS pps
	INNER JOIN PM_SiteLog AS psl ON psl.ProjectSiteId=pps.ProjectSiteId
	left join AD_Definations sts on sts.DefinationId=psl.StatusId
	left join AD_Definations msw on msw.DefinationId=psl.MSWindowId
	left join AD_Definations alm on alm.DefinationId=psl.AlarmId
	left join AD_Definations gng on gng.DefinationId=psl.GngId
	left join AD_Definations itm on itm.DefinationId=psl.ItemTypeId 
	WHERE pps.IsActive=1 AND pps.ProjectId=@ProjectId
END