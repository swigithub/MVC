CREATE PROCEDURE PM_PlanProject
 	@Filter NVARCHAR(50),
 	@ProjectId NUMERIC(18,0),
 	@RevisionId INT=0,
 	@FromDate DATETIME,
 	@ToDate DATETIME,
 	@Markets NVARCHAR(500)='',
 	@Tasks NVARCHAR(500)='',
 	@SiteStatus NVARCHAR(500)='',
 	@UserId NUMERIC(18,0)	
 AS
 
 --EXEC dbo.PM_PlanProject @Filter='Get_Site_Plan',@ProjectId=20021,@RevisionId=0,@FromDate='2017-06-01 00:00:00',@ToDate='2017-06-30 00:00:00',@Markets='163408,163409,163410,163411,163412,163413,163414,163415,163416,163405,163406,163407',@Tasks='50080',@SiteStatus='',@UserId=11
 
 
 BEGIN
 	SET @Tasks+=','
 	SET @Markets+=','
 	
 	IF @Filter='Get_Site_Plan'
 	BEGIN
 	SELECT  sit.ProjectSiteId, sit.ProjectId, sit.SiteCode, sit.SiteName,
 	       sit.SiteDate, sit.SiteTypeId, sit.SiteType, sit.Latitude, sit.Longitude,
 	       sit.PMRefId, sit.ClusterCode, sit.CityId, sit.Market, sit.StatusId,
 	       sit.SiteStatus, sit.MSWindowId, sit.MaintenanceWindow, sit.PriorityId,
 	       sit.Priority, sit.CreatedOn, sit.CreatedBy, sit.IsActive,
 	       sit.[Description], sit.ClientId, sit.ClientName, sit.[Address],
 	       sit.FACode, sit.USID, sit.eNB, sit.vMME, sit.ControlledIntro,
 	       sit.SuperBowl, sit.isDASInBuild, sit.FirstNetRAN, sit.IPlanJob,
 	       sit.PaceNo, sit.IPlanIssueDate, sit.SubMarket,pst.PlannedDate, pst.EstimatedStartDate,pst.EstimatedEndDate,pst.ActualStartDate, pst.ActualEndDate, pst.TargetDate, pst.StatusId,pst.SiteTaskId as 'TaskId', pst.AssignToId
 			
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
		AND st.SiteDate BETWEEN @FromDate AND @ToDate
		--AND st.CreatedOn BETWEEN '2017-06-01' AND '2018-01-31'
		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
	) sit
	Inner JOIN
	(
		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate,pst.EstimatedEndDate,pst.ActualStartDate, pst.ActualEndDate, pst.TargetDate, pst.StatusId,pst.SiteTaskId, psr.AssignToId
		FROM PM_SiteTasks AS pst
		LEFT JOIN PM_SiteResources AS psr ON psr.ProjectSiteId=pst.ProjectSiteId
		WHERE pst.IsActive=1 
		AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0 --AND psr.IsActive=1
	) pst ON sit.ProjectSiteId=pst.ProjectSiteId	
	END
END