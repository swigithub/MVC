
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_rptTesterSites]
@Filter varchar(200),
@Where nvarchar(MAX),
@Column nvarchar(MAX) = '*',
@Group nvarchar(MAX)=''
--,@Value nvarchar
AS
DECLARE @sql as nvarchar(max)=''
IF @Filter='Tester_Sites'
BEGIN
	SET @sql=(' 
	select  x.Tester,x.Date,x.TotalSites
	from
	(
	select 
		(ur.FirstName+ur.LastName)  as Tester,
		CAST(s.SubmittedON as date) Date ,
		COUNT(s.SiteId) AS TotalSites 
		from dbo.AV_Sites as s inner join
		dbo.AD_Definations as d on s.CityId=d.DefinationId
		inner join Sec_Users as ur on s.TesterId=ur.UserId
		where s.IsActive=1 and '+@Where+'  group by ur.FirstName,ur.LastName,CAST(s.SubmittedOn as date)
	) x
	order by x.Tester,x.Date	')
	
	
	EXEC(@sql)	
END 
-- exec [dbo].[AV_rptTesterSites] NET_LAYER_REPORT,'1=0' , 'Count(''Region'')','',''
ELSE if @Filter='NET_LAYER_REPORT'
BEGIN		
	SET @sql='SELECT '+ CASE WHEN @Column!='' AND @Column!='null' THEN @Column ELSE '*' END+ ' FROM (SELECT DISTINCT sit.WoRefId, clt.ClientName, rgn.DefinationName Region, cty.DefinationName Market, sit.SiteCode,
	net.DefinationName NetworkMode,bnd.DefinationName Band,crr.DefinationName Carrier,scp.DefinationName Scope,
	sec.ReceivedOn, sec.UploadedOn SubmittedOn, sec.AssignedOn,sec.DriveCompletedOn, sec.CompletedOn,
	stt.DefinationName Status,sit.[Description],sit.SubmittedOn ReportSubmittedOn,
	su.FirstName + '' '' + su.LastName Drive_Tester
	FROM AV_Sites sit
	INNER JOIN AV_NetLayerStatus sec ON sit.SiteId=sec.SiteId
	INNER JOIN AD_Clients AS clt ON clt.ClientId=sit.ClientId
	INNER JOIN AD_Definations AS cty ON cty.DefinationId=sit.CityId
	INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=cty.PDefinationId
	INNER JOIN AD_Definations AS stt ON stt.DefinationId=sec.[Status]
	INNER JOIN AD_Definations AS net ON net.DefinationId=sec.NetworkModeId
	INNER JOIN AD_Definations AS bnd ON bnd.DefinationId=sec.BandId
	INNER JOIN AD_Definations AS crr ON crr.DefinationId=sec.CarrierId
	INNER JOIN AD_Definations AS scp ON scp.DefinationId=sec.ScopeId
	LEFT OUTER JOIN Sec_Users AS su ON su.UserId=sec.TesterId
	WHERE sit.isActive=1) x WHERE ' + @Where + ' ' +
	CASE WHEN @Group!='' AND @Group!='null' THEN ' Group By ' + @Group ELSE '' END + ''


	EXEC(@sql) 
END
ELSE if @Filter='TEST_LOG_REPORT'
BEGIN		
	SET @sql='SELECT '+ CASE WHEN @Column!='' AND @Column!='null' THEN @Column ELSE '*' END+ ' FROM (SELECT tlog.Region, tlog.City, tlog.[TimeStamp], tlog.[Site], 
       tlog.Sector, tlog.CellId, tlog.LacId, tlog.PciId, tlog.MccId, tlog.MncId,
       tlog.Scope, tlog.NetworkMode, tlog.Band, tlog.Carrier, tlog.GsmRssi,
       tlog.GsmRxQual, tlog.WcdmaRssi, tlog.WcdmaRscp, tlog.WcdmaEcio,
       tlog.LteRssi, tlog.LteRsrp, tlog.LteRsrq, tlog.LteRsnr,
       tlog.DistanceFromSite, tlog.AngleToSite, tlog.TestResult
		FROM AV_SiteTestLog AS tlog
		WHERE tlog.isActive=1) x WHERE ' + @Where + ' ' +
	CASE WHEN @Group!='' AND @Group!='null' THEN ' Group By ' + @Group ELSE '' END + ''


	EXEC(@sql) 
END
ELSE IF @Filter='PROJECT_SUMMARY_REPORT'
BEGIN
	SET @sql='SELECT '+ CASE WHEN @Column!='' AND @Column!='null' THEN @Column ELSE '*' END+ ' FROM	
	(
		SELECT sit.SiteCode, sit.SiteName, sit.SiteDate,sit.SiteType, sit.Latitude, sit.Longitude, sit.PMRefId ''PMRefNo'',sit.ClusterCode, sit.Market, sit.SiteStatus,sit.MaintenanceWindow, sit.Priority,sit.CreatedOn, sit.CreatedBy, sit.IsActive, sit.[Description],sit.ClientName, sit.[Address], sit.FACode, sit.USID, sit.eNB, sit.vMME,sit.ControlledIntro, sit.SuperBowl, sit.isDASInBuild, sit.FirstNetRAN,sit.IPlanJob, sit.PaceNo, sit.IPlanIssueDate, sit.SubMarket,tss.PlannedDate ''TSSPlanDate'', tss.EstimatedStartDate ''TSSForecastDate'', tss.TargetDate ''TSSTargetDate'', tss.ActualEndDate ''TSSActualDate'',ssm.PlannedDate ''SSMPlanDate'', ssm.EstimatedStartDate ''SSMForecastDate'', ssm.TargetDate ''SSMTargetDate'', ssm.ActualEndDate ''SSMActualDate'',epl.PlannedDate ''EPLPlanDate'', epl.EstimatedStartDate ''EPLForecastDate'', epl.TargetDate ''EPLTargetDate'', epl.ActualEndDate ''EPLActualDate'',pinst.PlannedDate ''PreInstallPlanDate'', pinst.EstimatedStartDate ''PreInstallForecastDate'', pinst.TargetDate ''PreInstallTargetDate'', pinst.ActualEndDate ''PreInstallActualDate'',mig.PlannedDate ''MigrationPlanDate'', mig.EstimatedStartDate ''MigrationForecastDate'', mig.TargetDate ''MigrationTargetDate'', mig.ActualEndDate ''MigrationActualDate''
		FROM
		(
			select  st.ProjectSiteId,st.ProjectId, st.SiteCode, st.SiteName,st.SiteDate, st.SiteTypeId, typ.DefinationName ''SiteType'', st.Latitude,st.Longitude,st.PMRefId, st.ClusterCode,st.CityId, cty.DefinationName ''Market'', st.StatusId, sts.DefinationName ''SiteStatus'',st.MSWindowId, wnd.DefinationName ''MaintenanceWindow'', st.PriorityId, pty.DefinationName ''Priority'',st.CreatedOn, st.CreatedBy, st.IsActive,st.[Description], st.ClientId, cln.ClientName, st.[Address], st.FACode,st.USID, st.CommonId ''eNB'', st.vMME, st.ControlledIntro, st.SuperBowl,st.isDASInBuild, st.FirstNetRAN, st.IPlanJob, st.PaceNo,st.IPlanIssueDate, st.SubMarket
			from PM_ProjectSites as st INNER JOIN AD_Clients as cln on cln.ClientId = st.ClientId INNER JOIN AD_Definations cty on cty.DefinationId= st.CityId INNER JOIN AD_Definations rgn on rgn.DefinationId= cty.PDefinationId LEFT JOIN AD_Definations sts on sts.DefinationId = st.StatusId LEFT JOIN AD_Definations typ on typ.DefinationId = st.SiteTypeId LEFT JOIN AD_Definations wnd on wnd.DefinationId = st.MSWindowId LEFT JOIN AD_Definations pty on pty.DefinationId = st.PriorityId
			WHERE st.ProjectId=20021 AND st.IsActive=1
		) sit LEFT OUTER JOIN
		(
			--TSS
			SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate FROM PM_SiteTasks AS pst WHERE pst.IsActive=1 AND pst.TaskId=50076 AND pst.ActualEndDate IS NOT NULL
		) tss ON sit.ProjectSiteId=tss.ProjectSiteId
		LEFT OUTER JOIN
		(
			--SSM
			SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate FROM PM_SiteTasks AS pst WHERE pst.IsActive=1 AND pst.TaskId=50077 AND pst.ActualEndDate IS NOT NULL
		) ssm ON sit.ProjectSiteId=ssm.ProjectSiteId
		LEFT OUTER JOIN
		(
			--EPL
			SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate FROM PM_SiteTasks AS pst WHERE pst.IsActive=1 AND pst.TaskId=5008 AND pst.ActualEndDate IS NOT NULL
		) epl ON sit.ProjectSiteId=epl.ProjectSiteId
		LEFT OUTER JOIN
		(
			--PreInstall
			SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate FROM PM_SiteTasks AS pst WHERE pst.IsActive=1 AND pst.TaskId=50079 AND pst.ActualEndDate IS NOT NULL
		) pinst ON sit.ProjectSiteId=pinst.ProjectSiteId
		LEFT OUTER JOIN
		(
			--Migrations
			SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate FROM PM_SiteTasks AS pst WHERE pst.IsActive=1 AND pst.TaskId=50080 AND pst.ActualEndDate IS NOT NULL
		) mig ON sit.ProjectSiteId=mig.ProjectSiteId
	) x ' + CASE WHEN @Where!='' AND @Where!='null' AND @Where!='()' AND @Where!='(())' THEN 'WHERE ' + @Where ELSE '' END + ' ' +
	CASE WHEN @Group!='' AND @Group!='null' THEN ' Group By ' + @Group ELSE '' END + ''
	
	--SELECT @sql
	
	EXEC (@sql)
END
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
	where issue.ProjectId = 20021 
	Order by RequestDate asc
END