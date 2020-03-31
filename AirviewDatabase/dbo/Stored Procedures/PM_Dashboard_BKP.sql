


CREATE PROCEDURE [dbo].[PM_Dashboard_BKP]
	@Filter AS NVARCHAR(50),
	@FilterOption AS NVARCHAR(50)='',	
	@ProjectId AS NUMERIC(18,0),
	@MilestoneId AS NUMERIC(18,0)=0,
	@Value1 AS NVARCHAR(500)='0',
	@MapStatus AS NVARCHAR(50)='0',
	@Offset AS INT=0,
	@Page AS INT=5,
	@SearchFilter AS NVARCHAR(50)='',
	@Tasks AS NVARCHAR(500)='',
	@Markets AS NVARCHAR(500)='',
	@FromDate AS Date=NULL,
	@ToDate AS Date=NULL,
	@MapType AS NVARCHAR(50)='0',
	@IsActive bit = 0
AS
BEGIN
	
	SET @Tasks+=','
	SET @Markets+=','
	SET @MapStatus+=','
	SET @MapType+=','
	
-- [dbo].[PM_Dashboard] 'Get_MarketView_WO','',10007,0,0,5
	IF @Filter='Get_Project_WO'
	BEGIN	
		DECLARE @cols AS NVARCHAR(MAX), @query  AS NVARCHAR(MAX); 

		--IF @FilterOption!='' AND @FilterOption is not null
		--begin
			SET @IsActive=1
		--end
		
		IF @IsActive=1
		BEGIN
		SELECT st.ProjectId,st.ProjectSiteId 'SiteId',st.FACode,st.CommonId 'eNB',rgn.DefinationName 'Region',city.DefinationName 'Market',st.SubMarket,st.USID,
		       st.vMME, st.ControlledIntro, st.SuperBowl, st.isDASInBuild 'DAS/Inbuilding',
		       st.FirstNetRAN,st.IPlanJob, st.PaceNo, st.IPlanIssueDate,
		       sts.ColorCode,sts.DefinationName 'Status',sts.KeyCode, st.CreatedOn, su.FirstName+' '+su.LastName 'CreatedBy', st.IsActive,
			   (select count(tsk.ProjectSiteId) 'Count'  from PM_SiteTasks tsk where tsk.ProjectSiteId=st.ProjectSiteId  ) 
		from PM_ProjectSites as st 
		inner join AD_Clients as cln on cln.ClientId = st.ClientId		
		inner join AD_Definations city on city.DefinationId= st.CityId
		inner join AD_Definations rgn on rgn.DefinationId= city.PDefinationId
		INNER JOIN Sec_Users AS su ON su.UserId=st.CreatedBy
		--inner join AD_Definations reg on reg.DefinationId =(select pDefinationId from AD_Definations where DefinationId= st.CityId)
		left join AD_Definations sts on sts.DefinationId = st.StatusId
		WHERE  st.IsActive=@IsActive
		AND st.SiteDate between @FromDate and @ToDate
		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0 AND st.ProjectId=@ProjectId
		AND (city.DefinationName LIKE '%'+ @FilterOption+'%' OR
			st.WoRefId LIKE '%' + @FilterOption + '%' OR
			rgn.DefinationName LIKE '%' +@FilterOption + '%' OR
			st.SiteCode LIKE '%' +@FilterOption+ '%' OR
		st.FACode LIKE '%' +@FilterOption+ '%' 
		OR st.CommonId LIKE '%' +@FilterOption+ '%'
		OR st.USID LIKE '%' +@FilterOption+ '%')		
		Order By st.SiteDate DESC OFFSET @offset ROWS FETCH NEXT @Page ROWS ONLY
		
		
		--SELECT st.ProjectId,st.ProjectSiteId 'SiteId',st.FACode,st.CommonId 'Common Id',rgn.DefinationName 'Region',city.DefinationName 'Market',st.USID,
		--       st.vMME, st.ControlledIntro, st.SuperBowl, st.isDASInBuild,
		--       st.FirstNetRAN,sts.ColorCode,sts.DefinationName 'Status',sts.KeyCode
		--from PM_ProjectSites as st 
		--inner join AD_Clients as cln on cln.ClientId = st.ClientId		
		--inner join AD_Definations city on city.DefinationId= st.CityId
		--inner join AD_Definations rgn on rgn.DefinationId= city.PDefinationId
		----inner join AD_Definations reg on reg.DefinationId =(select pDefinationId from AD_Definations where DefinationId= st.CityId)
		--left join AD_Definations sts on sts.DefinationId = st.StatusId
		--WHERE st.ProjectSiteId IN(SELECT DISTINCT x.ProjectSiteId 
		--                          FROM PM_SiteTasks x
		--                          INNER JOIN PM_ProjectSites AS pps ON x.ProjectSiteId=pps.ProjectSiteId
		--                          WHERE x.IsActive=1 AND CAST(x.EstimatedStartDate AS DATE)=CAST(GETDATE() AS DATE) 
		--                          AND Charindex(cast(pps.CityId as varchar(max))+',', @Markets) > 0
		--                          AND pps.ProjectId=@ProjectId)
		--AND (city.DefinationName LIKE '%'+ @FilterOption+'%' OR
		--	WoRefId LIKE '%' + @FilterOption + '%' OR
		--	rgn.DefinationName LIKE '%' +@FilterOption + '%' OR
		--	st.SiteCode LIKE '%' +@FilterOption+ '%' OR
		--st.FACode LIKE '%' +@FilterOption+ '%' 
		--OR
		--st.CommonId LIKE '%' +@FilterOption+ '%')
		--Order By st.SiteDate DESC OFFSET @offset ROWS FETCH NEXT @Page ROWS ONLY
		
		--SET @cols = STUFF((SELECT DISTINCT ',' + QUOTENAME(c.Title) 
		--	FROM PM_Tasks c where c.ProjectId = @ProjectId
		--	and c.TaskTypeId IN(SELECT x.DefinationId FROM AD_Definations x WHERE x.KeyCode IN('PROJECT_STAGE','PROJECT_MILESTONE'))
		--	--ORDER BY c.taskid
		--	FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,1,'')
			
		--	set @query = 'SELECT [ProjectId],[SiteId],SubmittedOn,[WoRefNo],[SiteCode] ''Site Id'',[Market],[Region],[Scope],'+@cols+',ColorCode,Status,KeyCode
		--		FROM
		--		(
		--			select  st.ProjectId,st.ProjectSiteId ''SiteId'',Cast(PmRefId As nvarchar(100)) As WoRefNo,st.SiteCode,city.DefinationName Market,reg.DefinationName as Region,'''' Scope,
		--			tsk.Title Stage,CONVERT (varchar(10), tsk.ActualEndDate, 103) ActualDate,sts.DefinationName as Status,sts.KeyCode,sts.ColorCode,CONVERT (varchar(10), st.SiteDate, 103) ''SubmittedOn''
		--			from PM_ProjectSites as st 
		--			inner join AD_Clients as cln on cln.ClientId = st.ClientId
		--			inner join PM_Tasks as tsk on tsk.ProjectId = st.ProjectId  
		--			inner join PM_SiteTasks as stk on stk.ProjectSiteId=st.ProjectSiteId AND stk.TaskId=tsk.TaskId
		--			inner join AD_Definations city on city.DefinationId= st.CityId
		--			inner join AD_Definations rgn on rgn.DefinationId= city.PDefinationId
		--			inner join AD_Definations def on def.DefinationId = tsk.TaskTypeId
		--			inner join AD_Definations reg on reg.DefinationId =(select pDefinationId from AD_Definations where DefinationId= st.CityId)
		--			inner join AD_Definations sts on sts.DefinationId = st.StatusId
		--			where st.projectId='+CAST(@ProjectId as nvarchar(15))+' and tsk.TaskTypeId IN(SELECT x.DefinationId FROM AD_Definations x WHERE x.KeyCode IN(''PROJECT_STAGE'',''PROJECT_MILESTONE''))					
					
		--			AND (city.DefinationName LIKE ''%'+ @FilterOption+'%'' OR
		--			WoRefId LIKE ''%'+ @FilterOption + '%'' OR
		--			rgn.DefinationName LIKE ''%' +@FilterOption + '%'' OR
		--			st.SiteCode LIKE ''%' +@FilterOption+ '%'' )		
		--		) x
		--		PIVOT
		--		(
		--			MAX(ActualDate) FOR [Stage] IN ('+@cols+')
		--		) AS Ps  						
		--		Order by [SubmittedOn] DESC OFFSET '+CAST(@Offset as nvarchar(15))+' ROWS FETCH NEXT '+CAST(@Page as nvarchar(15))+' ROWS ONLY'
								
		--execute(@query)
	
	--AND CONVERT (varchar(10), st.SiteDate, 103) BETWEEN ' + CONVERT (varchar(10), @FromDate, 103) + ' AND ' + CONVERT (varchar(10), @ToDate, 103) + ' 
	--AND Charindex(cast(st.CityId as varchar(max))+'','','+ @Markets + ') > 0
	
	SELECT COUNT(st.ProjectId) 'Count'
	from PM_ProjectSites as st 
	inner join AD_Clients as cln on cln.ClientId = st.ClientId		
	inner join AD_Definations city on city.DefinationId= st.CityId
	inner join AD_Definations rgn on rgn.DefinationId= city.PDefinationId
	INNER JOIN Sec_Users AS su ON su.UserId=st.CreatedBy
	--inner join AD_Definations reg on reg.DefinationId =(select pDefinationId from AD_Definations where DefinationId= st.CityId)
	left join AD_Definations sts on sts.DefinationId = st.StatusId
	WHERE  st.IsActive=@IsActive
	AND st.SiteDate between @FromDate and @ToDate
	AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0 AND st.ProjectId=@ProjectId
	AND (city.DefinationName LIKE '%'+ @FilterOption+'%' OR
		st.WoRefId LIKE '%' + @FilterOption + '%' OR
		rgn.DefinationName LIKE '%' +@FilterOption + '%' OR
		st.SiteCode LIKE '%' +@FilterOption+ '%' OR
	st.FACode LIKE '%' +@FilterOption+ '%' 
	OR st.CommonId LIKE '%' +@FilterOption+ '%'
	OR st.USID LIKE '%' +@FilterOption+ '%') 
	END
	ELSE IF @IsActive=0
	BEGIN
	SELECT st.ProjectId,st.ProjectSiteId 'SiteId',st.FACode,st.CommonId 'eNB',rgn.DefinationName 'Region',city.DefinationName 'Market',st.SubMarket,st.USID,
		       st.vMME, st.ControlledIntro, st.SuperBowl, st.isDASInBuild 'DAS/Inbuilding',
		       st.FirstNetRAN,st.IPlanJob, st.PaceNo, st.IPlanIssueDate,
		       sts.ColorCode,sts.DefinationName 'Status',sts.KeyCode, st.CreatedOn, su.FirstName+' '+su.LastName 'CreatedBy', st.IsActive
		from PM_ProjectSites as st 
		inner join AD_Clients as cln on cln.ClientId = st.ClientId		
		inner join AD_Definations city on city.DefinationId= st.CityId
		inner join AD_Definations rgn on rgn.DefinationId= city.PDefinationId
		INNER JOIN Sec_Users AS su ON su.UserId=st.CreatedBy
		--inner join AD_Definations reg on reg.DefinationId =(select pDefinationId from AD_Definations where DefinationId= st.CityId)
		left join AD_Definations sts on sts.DefinationId = st.StatusId
		WHERE  st.IsActive=@IsActive
		--AND st.SiteDate between @FromDate and @ToDate
		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0 AND st.ProjectId=@ProjectId
		AND (city.DefinationName LIKE '%'+ @FilterOption+'%' OR
			st.WoRefId LIKE '%' + @FilterOption + '%' OR
			rgn.DefinationName LIKE '%' +@FilterOption + '%' OR
			st.SiteCode LIKE '%' +@FilterOption+ '%' OR
		st.FACode LIKE '%' +@FilterOption+ '%' 
		OR st.CommonId LIKE '%' +@FilterOption+ '%'
		OR st.USID LIKE '%' +@FilterOption+ '%')		
		Order By st.SiteDate DESC OFFSET @offset ROWS FETCH NEXT @Page ROWS ONLY

	SELECT COUNT(st.ProjectId) 'Count'
	from PM_ProjectSites as st 
	inner join AD_Clients as cln on cln.ClientId = st.ClientId		
	inner join AD_Definations city on city.DefinationId= st.CityId
	inner join AD_Definations rgn on rgn.DefinationId= city.PDefinationId
	INNER JOIN Sec_Users AS su ON su.UserId=st.CreatedBy
	--inner join AD_Definations reg on reg.DefinationId =(select pDefinationId from AD_Definations where DefinationId= st.CityId)
	left join AD_Definations sts on sts.DefinationId = st.StatusId
	WHERE  st.IsActive=@IsActive
	--AND st.SiteDate between @FromDate and @ToDate
	AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0 AND st.ProjectId=@ProjectId
	AND (city.DefinationName LIKE '%'+ @FilterOption+'%' OR
		st.WoRefId LIKE '%' + @FilterOption + '%' OR
		rgn.DefinationName LIKE '%' +@FilterOption + '%' OR
		st.SiteCode LIKE '%' +@FilterOption+ '%' OR
	st.FACode LIKE '%' +@FilterOption+ '%' 
	OR st.CommonId LIKE '%' +@FilterOption+ '%'
	OR st.USID LIKE '%' +@FilterOption+ '%') 
	END
	
		--SELECT COUNT(st.ProjectId) 'Count'
		--from PM_ProjectSites as st 
		--inner join AD_Clients as cln on cln.ClientId = st.ClientId		
		--inner join AD_Definations city on city.DefinationId= st.CityId
		--inner join AD_Definations rgn on rgn.DefinationId= city.PDefinationId
		----inner join AD_Definations reg on reg.DefinationId =(select pDefinationId from AD_Definations where DefinationId= st.CityId)
		--left join AD_Definations sts on sts.DefinationId = st.StatusId
		--WHERE st.ProjectSiteId IN(SELECT DISTINCT x.ProjectSiteId 
		--                          FROM PM_SiteTasks x
		--                          INNER JOIN PM_ProjectSites AS pps ON x.ProjectSiteId=pps.ProjectSiteId
		--                          WHERE x.IsActive=1 AND CAST(x.EstimatedStartDate AS DATE)=CAST(GETDATE() AS DATE) 
		--                          AND Charindex(cast(pps.CityId as varchar(max))+',', @Markets) > 0
		--                          AND pps.ProjectId=@ProjectId)
		--                          AND (city.DefinationName LIKE '%'+ @FilterOption+'%' OR
		--	WoRefId LIKE '%' + @FilterOption + '%' OR
		--	rgn.DefinationName LIKE '%' +@FilterOption + '%' OR
		--	st.SiteCode LIKE '%' +@FilterOption+ '%' OR
		--st.FACode LIKE '%' +@FilterOption+ '%'
		--OR
		--st.CommonId LIKE '%' +@FilterOption+ '%' )
	END
	ELSE IF @Filter='TASK_PIE_CHART'
	BEGIN
		--SELECT ad.DefinationName 'Status',
		--ad.ColorCode,COUNT(DISTINCT pps.ProjectSiteId) 'TotalSites'
		--FROM PM_ProjectSites AS pps
		--INNER JOIN PM_SiteTasks AS pst ON pps.ProjectSiteId=pst.ProjectSiteId
		--INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
		--INNER JOIN AD_Definations AS ad ON ad.DefinationId=pst.StatusId
		--WHERE pps.ProjectId=@ProjectId AND pps.IsActive=1 AND pst.ActualEndDate IS  NOT NULL
		--AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks +',') > 0
		--GROUP BY ad.DefinationName, ad.ColorCode

		SELECT Status,ColorCode,TotalSites
		FROM piechartdata pst
		WHERE Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks +',') > 0
	END	
	ELSE IF @Filter='Get_Progam_Status'
	BEGIN
		DECLARE @MlsForecastMTD AS INT= 0
		DECLARE @MlsActualMTD AS INT=0
		DECLARE @PrjForecastTD AS INT=0
		DECLARE @PrjActualTD AS INT=0
		
		SELECT st.ProjectSiteId
		INTO #tmpProjectSites
		FROM PM_ProjectSites AS st
		WHERE st.ProjectId=@ProjectId AND st.IsActive=1
		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
		AND st.CreatedOn<=GETDATE()
		
		SELECT @MlsForecastMTD=ISNULL(COUNT(DISTINCT fct.ProjectSiteId),0)
		FROM PM_SiteTasks AS fct
		WHERE fct.ProjectSiteId IN(SELECT tps.ProjectSiteId FROM #tmpProjectSites AS tps)
		AND Charindex(cast(fct.TaskId as varchar(max))+',', @Tasks) > 0
		AND fct.IsActive=1 AND (fct.EstimatedStartDate IS NOT NULL AND fct.EstimatedStartDate<@ToDate)	
		
		SELECT @MlsActualMTD=ISNULL(COUNT(DISTINCT fct.ProjectSiteId),0)
		FROM PM_SiteTasks AS fct
		WHERE fct.ProjectSiteId IN(SELECT tps.ProjectSiteId FROM #tmpProjectSites AS tps)
		AND Charindex(cast(fct.TaskId as varchar(max))+',', @Tasks) > 0
		AND fct.IsActive=1 AND (fct.ActualEndDate IS NOT NULL AND fct.ActualEndDate<@ToDate)
		
		SELECT @PrjForecastTD=ISNULL(COUNT(DISTINCT fct.ProjectSiteId),0)
		FROM PM_SiteTasks AS fct
		WHERE fct.ProjectSiteId IN(SELECT tps.ProjectSiteId FROM #tmpProjectSites AS tps)
		AND fct.IsActive=1 AND (fct.EstimatedStartDate IS NOT NULL AND fct.EstimatedStartDate<@ToDate)	
		
		SELECT @PrjActualTD=ISNULL(COUNT(DISTINCT fct.ProjectSiteId),0)
		FROM PM_SiteTasks AS fct
		WHERE fct.ProjectSiteId IN(SELECT tps.ProjectSiteId FROM #tmpProjectSites AS tps)
		AND fct.IsActive=1 AND (fct.ActualEndDate IS NOT NULL AND fct.ActualEndDate<@ToDate)
		
		DROP TABLE #tmpProjectSites
		
		SELECT @MlsForecastMTD 'MlsForecastMTD',@MlsActualMTD 'MlsActualMTD',@PrjForecastTD 'PrjForecastTD',@PrjActualTD 'PrjActualTD'
	END
	ELSE IF @Filter='Get_WO_Milestones'
	BEGIN
		select task.SiteTaskId 'TaskId',task.TaskId 'Task', tsk.ProjectId,tsk.Title 'Milestone',CONVERT (varchar(10), task.EstimatedStartDate, 103) 'Forecast Date',CONVERT (varchar(10), task.PlannedDate , 103) 'Plan Date',CONVERT (varchar(10), task.ActualEndDate , 103) 'Actual Date',CONVERT (varchar(10), task.TargetDate , 103) 'Target Date',pri.DefinationName 'Priority',pri.ColorCode 'PriorityColor' ,st.DefinationName 'Status',st.ColorCode 'StatusColor',st.DefinationId 'StatusId',
		ISNULL((SELECT psr.AssignToId FROM PM_SiteResources AS psr WHERE psr.ProjectSiteId=task.ProjectSiteId AND psr.SiteTaskId=task.SiteTaskId AND psr.IsActive=1),'') 'AssignTo'
		--,60 as ActualSites,108 as TotalSites
		from PM_SiteTasks as task
		inner join PM_Tasks as tsk on tsk.TaskId = task.TaskId
		INNER JOIN AD_Definations AS st ON st.DefinationId=task.StatusId
		INNER JOIN	AD_Definations As pri ON  pri.DefinationId = task.PriorityId
		WHERE tsk.ProjectId=@ProjectId AND task.ProjectSiteId = @value1 and tsk.IsActive=1
		and tsk.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)		
	END
	ELSE IF @Filter='Get_WO_Stages'
	BEGIN
		select task.SiteTaskId 'TaskId', tsk.ProjectId, tsk.PTaskId,tsk.Title 'Stage',CONVERT (varchar(10), task.EstimatedStartDate, 103) 'ForecastDate',CONVERT (varchar(10), task.PlannedDate , 103) 'PlanDate',CONVERT (varchar(10), task.ActualEndDate , 103) 'ActualDate',CONVERT (varchar(10), task.TargetDate , 103) 'TargetDate',pri.DefinationName 'Priority',pri.ColorCode 'PriorityColor',st.DefinationName 'Status',st.ColorCode 'StatusColor',st.DefinationId 'StatusId',
		ISNULL((SELECT psr.AssignToId FROM PM_SiteResources AS psr WHERE psr.ProjectSiteId=task.ProjectSiteId AND psr.SiteTaskId=task.SiteTaskId AND psr.IsActive=1),'') 'AssignTo'
		--,task.StatusId,60 as ActualSites,108 as TotalSites
		from PM_SiteTasks as task
		inner join PM_Tasks as tsk on tsk.TaskId = task.TaskId
		INNER JOIN AD_Definations AS st ON st.DefinationId=task.StatusId
		INNER JOIN	AD_Definations As pri ON  pri.DefinationId = task.PriorityId
		where tsk.ProjectId = @ProjectId AND tsk.PTaskId IN(SELECT st.TaskId FROM PM_SiteTasks st where st.SiteTaskId=@MilestoneId)
		AND task.ProjectSiteId=@Value1 and tsk.IsActive=1
		and tsk.TaskTypeId =(SELECT TOP(1)  x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_STAGE' and x.IsActive=1)
		ORDER BY tsk.sortorder
	END
	ELSE IF @Filter='Get_WO_Milestones_UE'
	BEGIN
		select task.SiteTaskId 'TaskId',task.TaskId 'Task', tsk.ProjectId,tsk.Title 'Milestone',task.EstimatedStartDate 'Forecast Date',task.PlannedDate 'Plan Date',task.ActualEndDate 'Actual Date',task.TargetDate 'Target Date',pri.DefinationName 'Priority',pri.ColorCode 'PriorityColor' ,st.DefinationName 'Status',st.ColorCode 'StatusColor',st.DefinationId 'StatusId',
		ISNULL((SELECT psr.AssignToId FROM PM_SiteResources AS psr WHERE psr.ProjectSiteId=task.ProjectSiteId AND psr.SiteTaskId=task.SiteTaskId AND psr.IsActive=1),'') 'AssignTo'
		--,60 as ActualSites,108 as TotalSites
		from PM_SiteTasks as task
		inner join PM_Tasks as tsk on tsk.TaskId = task.TaskId
		INNER JOIN AD_Definations AS st ON st.DefinationId=task.StatusId
		INNER JOIN	AD_Definations As pri ON  pri.DefinationId = task.PriorityId
		WHERE tsk.ProjectId=@ProjectId AND task.ProjectSiteId = @value1 and tsk.IsActive=1
		and tsk.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)		
	END
	ELSE IF @Filter='Get_WO_Stages_UE'
	BEGIN
		select task.SiteTaskId 'TaskId', tsk.ProjectId, tsk.PTaskId,tsk.Title 'Stage',task.EstimatedStartDate 'Forecast Date',task.PlannedDate 'Plan Date',task.ActualEndDate 'Actual Date',task.TargetDate 'Target Date',pri.DefinationName 'Priority',pri.ColorCode 'PriorityColor',st.DefinationName 'Status',st.ColorCode 'StatusColor',st.DefinationId 'StatusId',
		ISNULL((SELECT psr.AssignToId FROM PM_SiteResources AS psr WHERE psr.ProjectSiteId=task.ProjectSiteId AND psr.SiteTaskId=task.SiteTaskId AND psr.IsActive=1),'') 'AssignTo'
		--,task.StatusId,60 as ActualSites,108 as TotalSites
		from PM_SiteTasks as task
		inner join PM_Tasks as tsk on tsk.TaskId = task.TaskId
		INNER JOIN AD_Definations AS st ON st.DefinationId=task.StatusId
		INNER JOIN	AD_Definations As pri ON  pri.DefinationId = task.PriorityId
		where tsk.ProjectId = @ProjectId AND tsk.PTaskId IN(SELECT st.TaskId FROM PM_SiteTasks st where st.SiteTaskId=@MilestoneId)
		AND task.ProjectSiteId=@Value1 and tsk.IsActive=1
		and tsk.TaskTypeId =(SELECT TOP(1)  x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_STAGE' and x.IsActive=1)
		ORDER BY tsk.sortorder
	END
	ELSE IF @Filter='Get_Project_Tasks'
	BEGIN
		SELECT x.TaskId,ISNULL(pt.totalSites,0) 'totalSites'
		INTO #temp
		FROM PM_Tasks x left outer join
		(
			select pt.taskId,ISNULL(COUNT(DISTINCT st.ProjectSiteId),0) 'totalSites'		
				FROM PM_SiteTasks as task
				inner join PM_Tasks pt on pt.TaskId=task.TaskId
				inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
				where pt.ProjectId = @ProjectId and pt.IsActive=1 --AND ISNULL(pt.PTaskId,0)=0	
				AND st.IsActive=1 and pt.IsActive=1	
				--AND task.EstimatedStartDate <=GETDATE() 
				AND task.ActualEndDate IS NOT NULL
				--AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
				GROUP BY pt.TaskId
		) pt on pt.taskid=x.TaskId
		WHERE x.ProjectId=@ProjectId and x.IsActive=1
			
		IF @ProjectId=0
		BEGIN
			select task.TaskId, pt.ProjectId,pt.Title 'Task' ,COUNT(DISTINCT st.ProjectSiteId) as ActualSites, xx.totalSites as TotalSites ,pt.Color
			FROM PM_SiteTasks as task
			left join PM_Tasks pt on pt.TaskId=task.TaskId
			inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
			LEFT OUTER JOIN #temp xx ON xx.taskid=pt.TaskId
			where pt.ProjectId = @ProjectId and pt.IsActive=1 AND ISNULL(pt.PTaskId,0)=0
			AND pt.PTaskId = @MilestoneId			
			and pt.TaskTypeId = (SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@FilterOption)
			AND task.EstimatedStartDate <=GETDATE() 
			AND task.ActualEndDate IS NOT NULL AND st.IsActive=1	
			--AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0	
			GROUP BY task.TaskId,pt.Title,pt.ProjectId,pt.SortOrder,pt.Color,xx.totalSites
			ORDER BY pt.PTaskId,task.TaskId, pt.SortOrder
			
			--select task.TaskId, task.ProjectId,task.Title 'Task' ,60 as ActualSites,108 as TotalSites ,'red' as Color
			--from PM_Tasks as task
			--where task.PTaskId = @MilestoneId AND task.IsActive=1
			--and task.TaskTypeId = (SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@FilterOption)
			--ORDER BY task.sortorder
		END
		ELSE IF @ProjectId>0
		BEGIN
			IF (@FilterOption!='' AND @FilterOption IS NOT NULL)
			BEGIN
				SELECT task.TaskId, pt.PTaskId, pt.ProjectId,pt.Title 'Task' ,COUNT(DISTINCT st.ProjectSiteId) as ActualSites,xx.totalSites as TotalSites ,pt.Color as Color, ad.DefinationName 'TaskType'
				--task.ActualEndDate,task.ActualEndDate,sts.DefinationName 'Status'
				FROM PM_SiteTasks as task
				left join PM_Tasks pt on pt.TaskId=task.TaskId
				inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
				INNER JOIN AD_Definations AS ad ON ad.DefinationId=pt.TaskTypeId
				INNER JOIN AD_Definations AS sts ON sts.DefinationId=task.StatusId
				LEFT OUTER JOIN #temp xx ON xx.taskid=pt.TaskId
				where pt.ProjectId = @ProjectId and pt.IsActive=1 --AND ISNULL(pt.PTaskId,0)=0
				and pt.TaskTypeId = (SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@FilterOption)
				AND task.EstimatedStartDate <=GETDATE()
				AND task.ActualEndDate IS NOT NULL AND st.IsActive=1	
				--AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0	
				GROUP BY task.TaskId, pt.PTaskId, pt.ProjectId,pt.Title, pt.Color, ad.DefinationName, pt.SortOrder,xx.totalSites
				ORDER BY pt.PTaskId,task.TaskId, pt.SortOrder
			
				--select task.TaskId, task.PTaskId, task.ProjectId,task.Title 'Task' ,60+LEN(task.Title) as ActualSites,108 as TotalSites ,task.Color as Color, ad.DefinationName 'TaskType',
				--task.ActualEndDate,task.ActualEndDate,sts.DefinationName 'Status'
				--from PM_Tasks as task
				--INNER JOIN AD_Definations AS ad ON ad.DefinationId=task.TaskTypeId
				--INNER JOIN AD_Definations AS sts ON sts.DefinationId=task.StatusId
				--where task.ProjectId = @ProjectId AND task.IsActive=1
				----AND 
				--AND task.TaskTypeId = (SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@FilterOption)
				--ORDER BY task.sortorder
			END
			ELSE 
			BEGIN
				SELECT *
FROM
(
--SELECT pt.TaskId, pt.PTaskId, pt.ProjectId,pt.Title 'Task' ,0 as ActualSites,1 as TotalSites ,pt.Color as Color, ad.DefinationName 'TaskType',pt.SortOrder
----task.ActualEndDate,task.ActualEndDate,sts.DefinationName 'Status'
--FROM PM_Tasks pt 
--INNER JOIN AD_Definations AS ad ON ad.DefinationId=pt.TaskTypeId
--LEFT OUTER JOIN #temp xx ON xx.taskid=pt.TaskId
--where pt.ProjectId = @ProjectId and pt.IsActive=1
--AND pt.IsActive=1	 and xx.totalSites=0				
--GROUP BY pt.TaskId, pt.PTaskId, pt.ProjectId,pt.Title, pt.Color, ad.DefinationName, pt.SortOrder,xx.totalSites
--union all
SELECT pt.TaskId, pt.PTaskId, pt.ProjectId,pt.Title 'Task' ,COUNT(DISTINCT st.ProjectSiteId) as ActualSites,xx.totalSites as TotalSites ,pt.Color as Color, ad.DefinationName 'TaskType',pt.SortOrder
--task.ActualEndDate,task.ActualEndDate,sts.DefinationName 'Status'
FROM PM_SiteTasks as task
left join PM_Tasks pt on pt.TaskId=task.TaskId
inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
INNER JOIN AD_Definations AS ad ON ad.DefinationId=pt.TaskTypeId
INNER JOIN AD_Definations AS sts ON sts.DefinationId=task.StatusId
LEFT OUTER JOIN #temp xx ON xx.taskid=pt.TaskId
where pt.ProjectId = @ProjectId and pt.IsActive=1 --AND ISNULL(pt.PTaskId,0)=0
AND task.EstimatedStartDate <=GETDATE()
AND task.ActualEndDate IS NOT NULL
AND st.IsActive=1	 and xx.totalSites>0				
GROUP BY pt.TaskId, pt.PTaskId, pt.ProjectId,pt.Title, pt.Color, ad.DefinationName, pt.SortOrder,xx.totalSites
) x
ORDER BY x.PTaskId,x.TaskId, x.SortOrder
				
				--select task.TaskId, task.PTaskId, task.ProjectId,task.Title 'Task' ,60+LEN(task.Title) as ActualSites,108 as TotalSites ,task.Color as Color, ad.DefinationName 'TaskType',
				--task.ActualEndDate,task.ActualEndDate,sts.DefinationName 'Status'
				--from PM_Tasks as task
				--INNER JOIN AD_Definations AS ad ON ad.DefinationId=task.TaskTypeId
				--INNER JOIN AD_Definations AS sts ON sts.DefinationId=task.StatusId
				--where task.ProjectId = @ProjectId AND task.IsActive=1
				--ORDER BY task.sortorder
			END
		END
		
		DROP TABLE #temp
	END	
	ELSE IF @Filter='Get_Readiness'
	BEGIN
	DECLARE @totalSiteCount as int=0
	select @totalSiteCount=COUNT(DISTINCT st.ProjectSiteId)
	FROM PM_SiteTasks as task
	inner join PM_Tasks pt on pt.TaskId=task.TaskId
	inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
	where pt.ProjectId = @ProjectId and pt.IsActive=1 --AND ISNULL(pt.PTaskId,0)=0			
	--AND task.EstimatedStartDate <=GETDATE() 
	AND task.ActualEndDate IS NOT NULL
	--AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0


	select pt.taskId,COUNT(DISTINCT st.ProjectSiteId) 'totalSites'
	INTO #xtemp
	FROM PM_SiteTasks as task
	inner join PM_Tasks pt on pt.TaskId=task.TaskId
	inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
	where pt.ProjectId = @ProjectId and pt.IsActive=1 --AND ISNULL(pt.PTaskId,0)=0			
	--AND task.EstimatedStartDate <=GETDATE() 
	AND task.ActualEndDate IS NOT NULL and task.IsActive=1
	--AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
	GROUP BY pt.TaskId				
			
	SELECT task.TaskId, pt.PTaskId, pt.ProjectId,pt.Title 'Task' ,COUNT(DISTINCT st.ProjectSiteId) as ActualSites,xx.totalSites as TotalSites ,pt.Color as Color, ad.DefinationName 'TaskType',
	COUNT(DISTINCT st.ProjectSiteId) 'Compeleted',@totalSiteCount-COUNT(DISTINCT st.ProjectSiteId) 'Remaining','0' 'Readiness', ROW_NUMBER() over(order by task.TaskId) 'RowId'
	INTO #tmpTasks
	FROM PM_SiteTasks as task
	inner join PM_Tasks pt on pt.TaskId=task.TaskId
	inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
	INNER JOIN AD_Definations AS ad ON ad.DefinationId=pt.TaskTypeId
	INNER JOIN AD_Definations AS sts ON sts.DefinationId=task.StatusId
	LEFT OUTER JOIN #xtemp xx ON xx.taskid=pt.TaskId
	where pt.ProjectId = @ProjectId and pt.IsActive=1 --AND ISNULL(pt.PTaskId,0)=0
	AND task.EstimatedStartDate <=GETDATE()
	AND task.ActualEndDate IS NOT NULL and task.IsActive=1
	GROUP BY task.TaskId, pt.PTaskId, pt.ProjectId,pt.Title, pt.Color, ad.DefinationName, pt.SortOrder,xx.totalSites
	ORDER BY pt.PTaskId,task.TaskId, pt.SortOrder

	SELECT x.TaskId, x.PTaskId, x.ProjectId,x.Task, x.ActualSites,x.TotalSites,x.Color,x.TaskType,x.Compeleted,x.Remaining,
	CASE 
		 --WHEN ((SELECT y.Compeleted FROM #tmpTasks y WHERE y.RowId=x.RowId-1)-x.Compeleted)<=0 THEN 0
		 WHEN x.RowId>1 THEN (SELECT y.Compeleted FROM #tmpTasks y WHERE y.RowId=x.RowId-1)-x.Compeleted
		 ELSE x.Remaining END  'Readiness'
	from #tmpTasks x
		
	DROP TABLE #xtemp
	DROP TABLE #tmpTasks

	END	
	ELSE IF @Filter='GET_PROJECT_TASKS_UE'
	BEGIN
		select pt.taskId,COUNT(DISTINCT st.ProjectSiteId) 'totalSites'
		INTO #tempx
		FROM PM_SiteTasks as task
		inner join PM_Tasks pt on pt.TaskId=task.TaskId
		inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
		where pt.ProjectId = @ProjectId and pt.IsActive=1 --AND ISNULL(pt.PTaskId,0)=0			
		--AND task.EstimatedStartDate <=GETDATE() 
		AND task.ActualEndDate IS NOT NULL
		--AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
		GROUP BY pt.TaskId
		
		
		SELECT pt.TaskId, pt.PTaskId, pt.ProjectId,pt.Title 'Task' ,COUNT(DISTINCT st.ProjectSiteId) as ActualSites,xx.totalSites as TotalSites ,pt.Color as Color, ad.DefinationName 'TaskType',
		pt.EstimatedStartDate 'ActualStartDate',pt.ActualEndDate,sts.DefinationName 'Status'
		FROM PM_SiteTasks as task
		inner join PM_Tasks pt on pt.TaskId=task.TaskId
		inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=pt.TaskTypeId
		INNER JOIN AD_Definations AS sts ON sts.DefinationId=task.StatusId
		LEFT OUTER JOIN #tempx xx ON xx.taskid=pt.TaskId
		where pt.ProjectId = @ProjectId and pt.IsActive=1 --AND ISNULL(pt.PTaskId,0)=0
		AND task.EstimatedStartDate <=GETDATE()
		AND task.ActualEndDate IS NOT NULL
		GROUP BY pt.TaskId, pt.PTaskId, pt.ProjectId,pt.Title, pt.Color, ad.DefinationName, pt.SortOrder,xx.totalSites,pt.EstimatedStartDate,pt.ActualEndDate,sts.DefinationName
		ORDER BY pt.PTaskId,pt.TaskId, pt.SortOrder
		
		
		DROP TABLE #tempxx
	END
	ELSE IF @Filter='Get_Project_Timeline_Variance'
	BEGIN		
		--CAST(DATENAME(week, issue.RequestDate) AS NVARCHAR(5))
		IF @SearchFilter='' OR @SearchFilter='Daily'
		BEGIN
			select CAST(pst.EstimatedStartDate as date) 'WoDate', COUNT(DISTINCT sit.ProjectSiteId)  'WoCount', 'Forecast' 'WoType'
			INTO #forecast
			FROM PM_ProjectSites sit
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=sit.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where sit.ProjectId=@ProjectId  AND pst.IsActive=1 AND sit.IsActive=1 AND pst.EstimatedStartDate IS NOT NULL
			AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by CAST(pst.EstimatedStartDate as date)
			
			select CAST(pst.ActualEndDate as date) 'WoDate', COUNT(DISTINCT sit.ProjectSiteId)  'WoCount', 'Actual' 'WoType'
			INTO #actual
			FROM PM_ProjectSites sit
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=sit.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where sit.ProjectId=@ProjectId  AND pst.IsActive=1 AND sit.IsActive=1 AND pst.ActualEndDate IS NOT NULL 
			AND pst.ActualEndDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by CAST(pst.ActualEndDate as date)
			
			select CAST(tgt.TargetDate as date) 'WoDate', tgt.SiteCount  'WoCount', 'Target' 'WoType'
			INTO #target
			FROM PM_ProjectSites sit
			INNER JOIN PM_Targets AS tgt ON tgt.ProjectId=sit.ProjectId			 
			where sit.ProjectId=@ProjectId  AND sit.IsActive=1 AND tgt.TargetType='day'
			AND tgt.TargetDate BETWEEN @FromDate AND @ToDate
			AND Charindex(cast(tgt.MilestoneId as varchar(max))+',', @Tasks) > 0
			AND tgt.RevisionId=(
								SELECT MAX(pt.RevisionId)
								FROM PM_Targets AS pt
								WHERE pt.ProjectId=@ProjectId
								AND pt.TargetDate=tgt.TargetDate
								AND Charindex(cast(pt.MilestoneId as varchar(max))+',', @Tasks) > 0)
			
			select CAST(tgt.TargetDate as date) 'WoDate', tgt.SiteCount  'WoCount', 'RunRate' 'WoType'
			INTO #runrate
			FROM PM_ProjectSites sit
			INNER JOIN PM_Targets AS tgt ON tgt.ProjectId=sit.ProjectId			 
			where sit.ProjectId=@ProjectId  AND sit.IsActive=1 AND tgt.TargetType='day'
			AND tgt.TargetDate BETWEEN @FromDate AND @ToDate
			AND Charindex(cast(tgt.MilestoneId as varchar(max))+',', @Tasks) > 0
			AND tgt.RevisionId=(
								SELECT MAX(pt.RevisionId)
								FROM PM_Targets AS pt
								WHERE pt.ProjectId=@ProjectId
								AND pt.TargetDate=tgt.TargetDate
								AND Charindex(cast(pt.MilestoneId as varchar(max))+',', @Tasks) > 0)					
				
			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpforecast
			FROM #forecast			
			GROUP BY woDate,WOCount
			
			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpactual
			FROM #actual			
			GROUP BY woDate,WOCount
			
			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmptarget
			FROM #target			
			GROUP BY woDate,WOCount
			
			DECLARE @maxActualDate AS DATETIME = (SELECT MAX(x.WoDate) FROM #tmpActual x)
			DECLARE @EstSitePending AS INT = ISNULL((SELECT SUM(x.WoCount) FROM #tmpforecast x WHERE x.WoDate>@maxActualDate),0)
			DECLARE @EstDaysLeft AS INT = ISNULL((SELECT CASE WHEN @maxActualDate<=@ToDate THEN DATEDIFF(DAY,@maxActualDate,@ToDate) ELSE 0 END),0)
			
			DROP TABLE #forecast
			DROP TABLE #actual
			DROP TABLE #target			

			SELECT  TOP (DATEDIFF(DAY, @FromDate, @ToDate) + 1)
			Date = DATEADD(DAY, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @FromDate)
			INTO #tmpdates
			FROM    sys.all_objects a
			CROSS JOIN sys.all_objects b;			
						
			SELECT x.*
			FROM
			(
				SELECT x.Date 'WoDate',WoCount 'WoCount', 'Forecast' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #tmpdates x left outer join #tmpforecast y ON x.Date=y.WoDate
				UNION ALL
				SELECT x.Date 'WoDate',RunningTotal 'WoCount', 'CumForecast' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Forecast') 'ColorCode'
				FROM  #tmpdates x left outer join #tmpforecast y ON x.Date=y.WoDate
				UNION ALL
				SELECT x.Date 'WoDate',WoCount 'WoCount', 'Actual' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM  #tmpdates x left outer join #tmpactual y ON x.Date=y.WoDate
				UNION ALL
				SELECT x.Date 'WoDate',RunningTotal 'WoCount', 'CumActual' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Actual') 'ColorCode'
				FROM  #tmpdates x  left outer join #tmpactual y ON x.Date=y.WoDate
				UNION ALL
				SELECT x.Date 'WoDate',WoCount 'WoCount', 'Target' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Target') 'ColorCode'
				FROM  #tmpdates x left outer join  #tmptarget y ON x.Date=y.WoDate
				UNION ALL
				SELECT x.Date 'WoDate',RunningTotal 'WoCount', 'CumTarget' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Target') 'ColorCode'
				FROM  #tmpdates x  left outer join  #tmptarget y ON x.Date=y.WoDate
				UNION ALL
				SELECT x.Date 'WoDate', CASE WHEN x.Date>@maxActualDate THEN @EstSitePending/@EstDaysLeft ELSE NULL END 'WoCount', 'RunRate' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Run Rate') 'ColorCode'
				FROM  #tmpdates x
				--WHERE x.Date>@maxActualDate
			) x
			ORDER BY x.WoDate
			
			DROP TABLE #tmpforecast
			DROP TABLE #tmpactual
			DROP TABLE #tmptarget
			--DROP TABLE #tmprunrate
		END	
		ELSE IF @SearchFilter='Weekly'
		BEGIN
			select 'W-'+CAST(DATENAME(week, DATEADD(Day,4,  CAST(pst.EstimatedStartDate as date))) AS NVARCHAR(5)) 'WoDate', COUNT(DISTINCT sit.ProjectSiteId)  'WoCount', 'Forecast' 'WoType'
			INTO #wforecast
			FROM PM_ProjectSites sit
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=sit.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where sit.ProjectId=@ProjectId  AND pst.IsActive=1 AND sit.IsActive=1 AND pst.EstimatedStartDate IS NOT NULL
			AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by DATENAME(week, DATEADD(Day,4,  CAST(pst.EstimatedStartDate as date)))
			
			select 'W-'+CAST(DATENAME(week, DATEADD(Day,4,  CAST(pst.ActualEndDate as date))) AS NVARCHAR(5)) 'WoDate', COUNT(DISTINCT sit.ProjectSiteId) 'WoCount', 'Actual' 'WoType'
			INTO #wactual
			FROM PM_ProjectSites sit
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=sit.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where sit.ProjectId=@ProjectId  AND pst.IsActive=1 AND sit.IsActive=1 AND pst.ActualEndDate IS NOT NULL 
			AND pst.ActualEndDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by DATENAME(week, DATEADD(Day,4,  CAST(pst.ActualEndDate as date)))

			select 'W-'+tgt.TargetValue 'WoDate', SUM(tgt.SiteCount) 'WoCount', 'Target' 'WoType'
			--CAST(tgt.TargetDate as date) 'WoDate', tgt.SiteCount  'WoCount', 'Target' 'WoType'
			INTO #wtarget
			FROM PM_ProjectSites sit
			INNER JOIN PM_Targets AS tgt ON tgt.ProjectId=sit.ProjectId			 
			where sit.ProjectId=@ProjectId  AND sit.IsActive=1 AND tgt.TargetType='week'
			AND (@FromDate BETWEEN tgt.TargetFromDate AND tgt.TargetEndDate) AND (@ToDate BETWEEN tgt.TargetFromDate AND tgt.TargetEndDate)
			AND Charindex(cast(tgt.MilestoneId as varchar(max))+',', @Tasks) > 0
			AND tgt.RevisionId=(
								SELECT MAX(pt.RevisionId)
								FROM PM_Targets AS pt
								WHERE pt.ProjectId=@ProjectId
								AND tgt.TargetType='week'
								AND pt.TargetValue=tgt.TargetValue AND pt.TargetYear=tgt.TargetYear
								AND Charindex(cast(pt.MilestoneId as varchar(max))+',', @Tasks) > 0)
			GROUP BY 'W-'+tgt.TargetValue
			
			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpwforecast
			FROM #wforecast			
			GROUP BY woDate,WOCount
			
			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpwactual
			FROM #wactual			
			GROUP BY woDate,WOCount

			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpwtarget
			FROM #wtarget			
			GROUP BY woDate,WOCount		
			
			DROP TABLE #wforecast
			DROP TABLE #wactual
			DROP TABLE #wtarget									
			
			SELECT x.*
			FROM
			(
				SELECT WoDate,WoCount, 'Forecast' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #tmpwforecast
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumForecast' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Forecast') 'ColorCode'
				FROM #tmpwforecast
				UNION ALL
				SELECT WoDate,WoCount, 'Actual' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #tmpwactual
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumActual' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Actual') 'ColorCode'
				FROM #tmpwactual
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'Target' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Target') 'ColorCode'
				FROM #tmpwtarget
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumTarget' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Target') 'ColorCode'
				FROM #tmpwtarget				
			) x
			ORDER BY x.WoDate
			
			DROP TABLE #tmpwforecast
			DROP TABLE #tmpwactual
			DROP TABLE #tmpwtarget
		END
		ELSE IF @SearchFilter='Monthly'
		BEGIN
			select CAST(pst.EstimatedStartDate AS CHAR(3))+'-'+RIGHT(YEAR(pst.EstimatedStartDate),2) 'WoDate', COUNT(DISTINCT sit.ProjectSiteId)  'WoCount', 'Forecast' 'WoType'
			INTO #mforecast
			FROM PM_ProjectSites sit
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=sit.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where sit.ProjectId=@ProjectId  AND pst.IsActive=1 AND sit.IsActive=1 AND pst.EstimatedStartDate IS NOT NULL
			AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by CAST(pst.EstimatedStartDate AS CHAR(3))+'-'+RIGHT(YEAR(pst.EstimatedStartDate),2)
			
			select CAST(pst.ActualEndDate AS CHAR(3))+'-'+RIGHT(YEAR(pst.ActualEndDate),2) 'WoDate', COUNT(DISTINCT sit.ProjectSiteId)  'WoCount', 'Actual' 'WoType'
			INTO #mactual
			FROM PM_ProjectSites sit
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=sit.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where sit.ProjectId=@ProjectId  AND pst.IsActive=1 AND sit.IsActive=1 AND pst.ActualEndDate IS NOT NULL 
			AND pst.ActualEndDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by CAST(pst.ActualEndDate AS CHAR(3))+'-'+RIGHT(YEAR(pst.ActualEndDate),2)

			select CASE WHEN tgt.TargetValue='1' THEN 'Jan' WHEN tgt.TargetValue='2' THEN 'Feb' WHEN tgt.TargetValue='3' THEN 'Mar' WHEN tgt.TargetValue='4' THEN 'Apr'
					WHEN tgt.TargetValue='5' THEN 'May' WHEN tgt.TargetValue='6' THEN 'Jun' WHEN tgt.TargetValue='7' THEN 'Jul' WHEN tgt.TargetValue='8' THEN 'Aug'
					WHEN tgt.TargetValue='9' THEN 'Sep' WHEN tgt.TargetValue='10' THEN 'Oct' WHEN tgt.TargetValue='11' THEN 'Nov' WHEN tgt.TargetValue='12' THEN 'Dec'
			ELSE NULL
			END+'-'+CAST(tgt.TargetYear as nvarchar(4)) 'WoDate', SUM(tgt.SiteCount) 'WoCount', 'Target' 'WoType'
			INTO #mtarget
			FROM PM_ProjectSites sit
			INNER JOIN PM_Targets AS tgt ON tgt.ProjectId=sit.ProjectId			 
			where sit.ProjectId=@ProjectId  AND sit.IsActive=1 AND tgt.TargetType='month'
			AND (@FromDate BETWEEN tgt.TargetFromDate AND tgt.TargetEndDate) AND (@ToDate BETWEEN tgt.TargetFromDate AND tgt.TargetEndDate)
			AND Charindex(cast(tgt.MilestoneId as varchar(max))+',', @Tasks) > 0
			AND tgt.RevisionId=(
								SELECT MAX(pt.RevisionId)
								FROM PM_Targets AS pt
								WHERE pt.ProjectId=@ProjectId
								AND pt.TargetValue=tgt.TargetValue AND pt.TargetYear=tgt.TargetYear
								AND tgt.TargetType='month'
								AND Charindex(cast(pt.MilestoneId as varchar(max))+',', @Tasks) > 0)
			GROUP BY (CASE WHEN tgt.TargetValue='1' THEN 'Jan' WHEN tgt.TargetValue='2' THEN 'Feb' WHEN tgt.TargetValue='3' THEN 'Mar' WHEN tgt.TargetValue='4' THEN 'Apr'
					WHEN tgt.TargetValue='5' THEN 'May' WHEN tgt.TargetValue='6' THEN 'Jun' WHEN tgt.TargetValue='7' THEN 'Jul' WHEN tgt.TargetValue='8' THEN 'Aug'
					WHEN tgt.TargetValue='9' THEN 'Sep' WHEN tgt.TargetValue='10' THEN 'Oct' WHEN tgt.TargetValue='11' THEN 'Nov' WHEN tgt.TargetValue='12' THEN 'Dec'
			ELSE NULL
			END+'-'+CAST(tgt.TargetYear as nvarchar(4)))
			
			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpmforecast
			FROM #mforecast			
			GROUP BY woDate,WOCount
			
			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpmactual
			FROM #mactual			
			GROUP BY woDate,WOCount

			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpmtarget
			FROM #mtarget			
			GROUP BY woDate,WOCount	

			DROP TABLE #mforecast
			DROP TABLE #mactual
			DROP TABLE #mtarget
			
			SELECT x.*
			FROM
			(
				SELECT WoDate,WoCount, 'Forecast' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #tmpmforecast
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumForecast' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Forecast') 'ColorCode'
				FROM #tmpmforecast
				UNION ALL
				SELECT WoDate,WoCount, 'Actual' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #tmpmactual
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumActual' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Actual') 'ColorCode'
				FROM #tmpmactual
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'Target' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Target') 'ColorCode'
				FROM #tmpmtarget
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumTarget' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Target') 'ColorCode'
				FROM #tmpmtarget
			) x
			ORDER BY x.WoDate desc
			
			DROP TABLE #tmpmforecast
			DROP TABLE #tmpmactual
			DROP TABLE #tmpmtarget
		END
		ELSE IF @SearchFilter='Quaterly'
		BEGIN
			select 'Q-'+CAST(DATENAME(quarter, DATEADD(Day,4,  CAST(pst.EstimatedStartDate as date))) AS NVARCHAR(5)) 'WoDate', COUNT(DISTINCT sit.ProjectSiteId)  'WoCount', 'Forecast' 'WoType'
			INTO #qforecast
			FROM PM_ProjectSites sit
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=sit.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where sit.ProjectId=@ProjectId  AND pst.IsActive=1 AND sit.IsActive=1 AND pst.EstimatedStartDate IS NOT NULL
			AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by DATENAME(quarter, DATEADD(Day,4,  CAST(pst.EstimatedStartDate as date)))
			
			select 'Q-'+CAST(DATENAME(quarter, DATEADD(Day,4,  CAST(pst.ActualEndDate as date))) AS NVARCHAR(5)) 'WoDate', COUNT(DISTINCT sit.ProjectSiteId)  'WoCount', 'Actual' 'WoType'
			INTO #qactual
			FROM PM_ProjectSites sit
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=sit.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where sit.ProjectId=@ProjectId  AND pst.IsActive=1 AND sit.IsActive=1 AND pst.ActualEndDate IS NOT NULL 
			AND pst.ActualEndDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by DATENAME(quarter, DATEADD(Day,4,  CAST(pst.ActualEndDate as date)))

			select 'Q-'+tgt.TargetValue 'WoDate', SUM(tgt.SiteCount) 'WoCount', 'Target' 'WoType'
			--CAST(tgt.TargetDate as date) 'WoDate', tgt.SiteCount  'WoCount', 'Target' 'WoType'
			INTO #qtarget
			FROM PM_ProjectSites sit
			INNER JOIN PM_Targets AS tgt ON tgt.ProjectId=sit.ProjectId			 
			where sit.ProjectId=@ProjectId  AND sit.IsActive=1 AND tgt.TargetType='quarter'
			AND (@FromDate BETWEEN tgt.TargetFromDate AND tgt.TargetEndDate) AND (@ToDate BETWEEN tgt.TargetFromDate AND tgt.TargetEndDate)
			AND Charindex(cast(tgt.MilestoneId as varchar(max))+',', @Tasks) > 0
			AND tgt.RevisionId=(
								SELECT MAX(pt.RevisionId)
								FROM PM_Targets AS pt
								WHERE pt.ProjectId=@ProjectId
								AND tgt.TargetType='quarter'
								AND pt.TargetValue=tgt.TargetValue AND pt.TargetYear=tgt.TargetYear
								AND Charindex(cast(pt.MilestoneId as varchar(max))+',', @Tasks) > 0)
			GROUP BY 'Q-'+tgt.TargetValue
			
			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpqforecast
			FROM #qforecast			
			GROUP BY woDate,WOCount
			
			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpqactual
			FROM #qactual			
			GROUP BY woDate,WOCount

			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpqtarget
			FROM #qtarget			
			GROUP BY woDate,WOCount	

			DROP TABLE #qforecast
			DROP TABLE #qactual
			DROP TABLE #qtarget
			
			SELECT x.*
			FROM
			(
				SELECT WoDate,WoCount, 'Forecast' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #tmpqforecast
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumForecast' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Forecast') 'ColorCode'
				FROM #tmpqforecast
				UNION ALL
				SELECT WoDate,WoCount, 'Actual' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #tmpqactual
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumActual' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Actual') 'ColorCode'
				FROM #tmpqactual
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'Target' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Target') 'ColorCode'
				FROM #tmpqtarget
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumTarget' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Target') 'ColorCode'
				FROM #tmpqtarget
			) x
			ORDER BY x.WoDate asc
			
			DROP TABLE #tmpqforecast
			DROP TABLE #tmpqactual
			DROP TABLE #tmpqtarget
		END	
		ELSE IF @SearchFilter='Yearly'
		BEGIN
			select DATENAME(year, DATEADD(Day,4,  CAST(pst.EstimatedStartDate as date))) 'WoDate', COUNT(DISTINCT sit.ProjectSiteId)  'WoCount', 'Forecast' 'WoType'
			INTO #yforecast
			FROM PM_ProjectSites sit
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=sit.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where sit.ProjectId=@ProjectId  AND pst.IsActive=1 AND sit.IsActive=1 AND pst.EstimatedStartDate IS NOT NULL
			AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by DATENAME(year, DATEADD(Day,4,  CAST(pst.EstimatedStartDate as date)))
			
			select DATENAME(year, DATEADD(Day,4,  CAST(pst.ActualEndDate as date))) 'WoDate', COUNT(DISTINCT sit.ProjectSiteId)  'WoCount', 'Actual' 'WoType'
			INTO #yactual
			FROM PM_ProjectSites sit
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=sit.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where sit.ProjectId=@ProjectId  AND pst.IsActive=1 AND sit.IsActive=1 AND pst.ActualEndDate IS NOT NULL 
			AND pst.ActualEndDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by DATENAME(year, DATEADD(Day,4,  CAST(pst.ActualEndDate as date)))

			select tgt.TargetValue 'WoDate', SUM(tgt.SiteCount) 'WoCount', 'Target' 'WoType'
			--CAST(tgt.TargetDate as date) 'WoDate', tgt.SiteCount  'WoCount', 'Target' 'WoType'
			INTO #ytarget
			FROM PM_ProjectSites sit
			INNER JOIN PM_Targets AS tgt ON tgt.ProjectId=sit.ProjectId			 
			where sit.ProjectId=@ProjectId  AND sit.IsActive=1 AND tgt.TargetType='year'
			AND (@FromDate BETWEEN tgt.TargetFromDate AND tgt.TargetEndDate) AND (@ToDate BETWEEN tgt.TargetFromDate AND tgt.TargetEndDate)
			AND Charindex(cast(tgt.MilestoneId as varchar(max))+',', @Tasks) > 0
			AND tgt.RevisionId=(
								SELECT MAX(pt.RevisionId)
								FROM PM_Targets AS pt
								WHERE pt.ProjectId=@ProjectId
								AND tgt.TargetType='year'
								AND pt.TargetValue=tgt.TargetValue AND pt.TargetYear=tgt.TargetYear
								AND Charindex(cast(pt.MilestoneId as varchar(max))+',', @Tasks) > 0)
			GROUP BY tgt.TargetValue
			
			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpyforecast
			FROM #yforecast			
			GROUP BY woDate,WOCount
			
			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpyactual
			FROM #yactual			
			GROUP BY woDate,WOCount

			SELECT WoDate,WoCount,SUM(WOCount) OVER(ORDER BY woDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #tmpytarget
			FROM #ytarget		
			GROUP BY woDate,WOCount

			DROP TABLE #yforecast
			DROP TABLE #yactual
			DROP TABLE #ytarget
			
			SELECT x.*
			FROM
			(
				SELECT WoDate,WoCount, 'Forecast' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #tmpyforecast
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumForecast' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Forecast') 'ColorCode'
				FROM #tmpyforecast
				UNION ALL
				SELECT WoDate,WoCount, 'Actual' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #tmpyactual
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumActual' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Actual') 'ColorCode'
				FROM #tmpyactual
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'Target' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Target') 'ColorCode'
				FROM #tmpytarget
				UNION ALL
				SELECT WoDate,RunningTotal 'WoCount', 'CumTarget' 'WoType',(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Cum. Target') 'ColorCode'
				FROM #tmpytarget
			) x
			ORDER BY x.WoDate asc
			
			DROP TABLE #tmpyforecast
			DROP TABLE #tmpyactual
			DROP TABLE #tmpytarget			
		END	
	END
	ELSE IF @Filter='Get_MarketView_WO'
	BEGIN		
		select COUNT(pst.ProjectSiteId ) 'TotalSites' ,cty.DefinationId,cty.DefinationName 'DefinationName',  'Forecast' 'Type',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
		INTO #mktForecast
		from PM_ProjectSites st
		INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
		INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
		inner join AD_Definations cty on cty.DefinationId=st.CityId
		inner join AD_Definations rgn on rgn.DefinationId=cty.PDefinationId
		inner join AD_Definations stt on stt.DefinationId=rgn.PDefinationId
		WHERE st.ProjectId=@ProjectId AND cty.PDefinationId=@Value1 AND pst.EstimatedStartDate IS NOT NULL
		AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate
		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
		AND Charindex(cast(pt.TaskId as varchar(max))+',', @Tasks) > 0
		group by cty.DefinationId,cty.DefinationName

		select COUNT(pst.ProjectSiteId ) 'TotalSites' ,cty.DefinationId,cty.DefinationName 'DefinationName',  'Actual' 'Type',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
		INTO #mktActual
		from PM_ProjectSites st
		INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
		INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
		inner join AD_Definations cty on cty.DefinationId=st.CityId
		inner join AD_Definations rgn on rgn.DefinationId=cty.PDefinationId
		inner join AD_Definations stt on stt.DefinationId=rgn.PDefinationId
		WHERE st.ProjectId=@ProjectId AND cty.PDefinationId=@Value1 AND pst.ActualEndDate IS NOT NULL
		AND  pst.ActualEndDate BETWEEN @FromDate AND @ToDate
		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
		AND Charindex(cast(pt.TaskId as varchar(max))+',', @Tasks) > 0
		group by cty.DefinationId,cty.DefinationName
		
		SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
		SUM(x.TotalSites) OVER(ORDER BY x.DefinationId ROWS UNBOUNDED PRECEDING) AS RunningTotal
		INTO #mFctSites
		FROM #mktForecast x
		group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

		SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
		SUM(x.TotalSites) OVER(ORDER BY x.DefinationId ROWS UNBOUNDED PRECEDING) AS RunningTotal
		INTO #mActSites
		FROM #mktActual x
		group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

		drop table #mktForecast
		drop table #mktActual

		SELECT x.*
			FROM
			(
				SELECT x.TotalSites, x.DefinationId,x.DefinationName, 'Forecast' 'Type', x.ColorCode
				FROM #mFctSites x
				UNION ALL
				SELECT x.RunningTotal, x.DefinationId,x.DefinationName, 'CumForecast' 'Type', x.ColorCode
				FROM #mFctSites x
				UNION ALL
				SELECT x.TotalSites, x.DefinationId,x.DefinationName, 'Actual' 'Type', x.ColorCode
				FROM #mActSites x
				UNION ALL
				SELECT x.RunningTotal, x.DefinationId,x.DefinationName, 'CumActual' 'Type', x.ColorCode
				FROM #mActSites x
			) x
			ORDER BY x.DefinationName

		drop table #mFctSites
		drop table #mActSites
	END
	ELSE IF @Filter='Get_DateWise_WO'
	BEGIN
		IF @SearchFilter='' OR @SearchFilter='Daily'
		BEGIN
			select COUNT(st.ProjectSiteId ) 'TotalSites' ,0 'DefinationId',pst.EstimatedStartDate 'DefinationName', 'Forecast' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'			
			INTO #dtForecast
			FROM PM_ProjectSites st
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where st.ProjectId=@ProjectId  AND pst.IsActive=1 AND pst.EstimatedStartDate IS NOT NULL
			AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate AND st.CityId=@Value1
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by pst.EstimatedStartDate

			select COUNT(st.ProjectSiteId ) 'TotalSites' ,0 'DefinationId',pst.ActualEndDate 'DefinationName', 'Actual' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
			INTO #dtActual
			FROM PM_ProjectSites st
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where st.ProjectId=@ProjectId  AND pst.IsActive=1 AND pst.ActualEndDate IS NOT NULL
			AND pst.ActualEndDate BETWEEN @FromDate AND @ToDate AND st.CityId=@Value1
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by pst.ActualEndDate

			SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
			SUM(x.TotalSites) OVER(ORDER BY x.DefinationName ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #dFctSites
			FROM #dtForecast x
			group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

			SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
			SUM(x.TotalSites) OVER(ORDER BY x.DefinationName ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #dActSites
			FROM #dtActual x
			group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

			drop table #dtForecast
			drop table #dtActual

			SELECT  TOP (DATEDIFF(DAY, @FromDate, @ToDate) + 1)
			Date = DATEADD(DAY, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @FromDate)
			INTO #dtdates
			FROM    sys.all_objects a
			CROSS JOIN sys.all_objects b;	

			SELECT x.*
			FROM
			(
				SELECT y.TotalSites, y.DefinationId, x.Date 'DefinationName', 'Forecast' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #dtdates x left outer join #dFctSites y ON x.Date=y.DefinationName				
				UNION ALL
				SELECT y.RunningTotal, y.DefinationId,x.Date 'DefinationName', 'CumForecast' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #dtdates x left outer join #dFctSites y ON x.Date=y.DefinationName				
				UNION ALL
				SELECT y.TotalSites, y.DefinationId,x.Date 'DefinationName', 'Actual' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #dtdates x left outer join #dActSites y ON x.Date=y.DefinationName				
				UNION ALL
				SELECT y.RunningTotal, y.DefinationId,x.Date 'DefinationName', 'CumActual' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #dtdates x left outer join #dActSites y ON x.Date=y.DefinationName	
			) x			
			ORDER BY CAST(x.DefinationName as datetime) asc

		drop table #dFctSites
		drop table #dActSites
		END
		ELSE IF @SearchFilter='Weekly'
		BEGIN
			select COUNT(st.ProjectSiteId ) 'TotalSites' ,0 'DefinationId','W-'+CAST(DATENAME(week,pst.EstimatedStartDate) AS NVARCHAR(5)) 'DefinationName', 'Forecast' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
			INTO #wkForecast
			FROM PM_ProjectSites st
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where st.ProjectId=@ProjectId  AND pst.IsActive=1 AND pst.EstimatedStartDate IS NOT NULL
			AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate AND st.CityId=@Value1
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by 'W-'+CAST(DATENAME(week,pst.EstimatedStartDate) AS NVARCHAR(5))

			select COUNT(st.ProjectSiteId ) 'TotalSites' ,0 'DefinationId','W-'+CAST(DATENAME(week,pst.ActualEndDate) AS NVARCHAR(5)) 'DefinationName', 'Actual' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
			INTO #wkActual
			FROM PM_ProjectSites st
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where st.ProjectId=@ProjectId  AND pst.IsActive=1 AND pst.ActualEndDate IS NOT NULL
			AND pst.ActualEndDate BETWEEN @FromDate AND @ToDate AND st.CityId=@Value1
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by 'W-'+CAST(DATENAME(week,pst.ActualEndDate) AS NVARCHAR(5))

			SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
			SUM(x.TotalSites) OVER(ORDER BY x.DefinationName ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #wFctSites
			FROM #wkForecast x
			group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

			SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
			SUM(x.TotalSites) OVER(ORDER BY x.DefinationName ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #wActSites
			FROM #wkActual x
			group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

			drop table #wkForecast
			drop table #wkActual	


			SELECT x.*
			FROM
			(
				SELECT y.TotalSites, y.DefinationId, y.DefinationName 'DefinationName', 'Forecast' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #wFctSites y			
				UNION ALL
				SELECT y.RunningTotal, y.DefinationId, y.DefinationName 'DefinationName', 'CumForecast' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #wFctSites y				
				UNION ALL
				SELECT y.TotalSites, y.DefinationId, y.DefinationName 'DefinationName', 'Actual' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #wActSites y			
				UNION ALL
				SELECT y.RunningTotal, y.DefinationId, y.DefinationName 'DefinationName', 'CumActual' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #wActSites y	
			) x
			ORDER BY x.DefinationName

			drop table #wFctSites
			drop table #wActSites
		END
		ELSE IF @SearchFilter='Monthly'
		BEGIN
			select COUNT(st.ProjectSiteId ) 'TotalSites' ,0 'DefinationId',CAST(pst.EstimatedStartDate AS CHAR(3))+'-'+RIGHT(YEAR(pst.EstimatedStartDate),2) 'DefinationName', 'Forecast' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
			INTO #mnForecast
			FROM PM_ProjectSites st
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where st.ProjectId=@ProjectId  AND pst.IsActive=1 AND pst.EstimatedStartDate IS NOT NULL
			AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate AND st.CityId=@Value1
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by CAST(pst.EstimatedStartDate AS CHAR(3))+'-'+RIGHT(YEAR(pst.EstimatedStartDate),2)
			
			select COUNT(pst.ProjectSiteId ) 'TotalSites' ,0 'DefinationId',CAST(pst.ActualEndDate AS CHAR(3))+'-'+RIGHT(YEAR(pst.ActualEndDate),2) 'DefinationName', 'Actual' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
			INTO #mnActual
			FROM PM_ProjectSites st
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where st.ProjectId=@ProjectId  AND pst.IsActive=1 AND pst.ActualEndDate IS NOT NULL
			AND pst.ActualEndDate BETWEEN @FromDate AND @ToDate AND st.CityId=@Value1
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by CAST(pst.ActualEndDate AS CHAR(3))+'-'+RIGHT(YEAR(pst.ActualEndDate),2)

			SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
			SUM(x.TotalSites) OVER(ORDER BY x.DefinationName ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #mnFctSites
			FROM #mnForecast x
			group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

			SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
			SUM(x.TotalSites) OVER(ORDER BY x.DefinationName ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #mnActSites
			FROM #mnActual x
			group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

			drop table #mnForecast
			drop table #mnActual	

			SELECT x.*
			FROM
			(
				SELECT y.TotalSites, y.DefinationId, y.DefinationName 'DefinationName', 'Forecast' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #mnFctSites y			
				UNION ALL
				SELECT y.RunningTotal, y.DefinationId, y.DefinationName 'DefinationName', 'CumForecast' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #mnFctSites y				
				UNION ALL
				SELECT y.TotalSites, y.DefinationId, y.DefinationName 'DefinationName', 'Actual' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #mnActSites y			
				UNION ALL
				SELECT y.RunningTotal, y.DefinationId, y.DefinationName 'DefinationName', 'CumActual' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #mnActSites y	
			) x
			ORDER BY x.DefinationName desc

			drop table #mnFctSites
			drop table #mnActSites
			
		END
		ELSE IF @SearchFilter='Quaterly'
		BEGIN
			select COUNT(st.ProjectSiteId ) 'TotalSites' ,0 'DefinationId','Q-'+CAST(DATENAME(quarter,pst.EstimatedStartDate) AS NVARCHAR(5)) 'DefinationName', 'Forecast' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
			INTO #qtForecast
			FROM PM_ProjectSites st
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where st.ProjectId=@ProjectId  AND pst.IsActive=1 AND pst.EstimatedStartDate IS NOT NULL
			AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate AND st.CityId=@Value1
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by 'Q-'+CAST(DATENAME(quarter,pst.EstimatedStartDate) AS NVARCHAR(5))

			select COUNT(st.ProjectSiteId ) 'TotalSites' ,0 'DefinationId','Q-'+CAST(DATENAME(quarter,pst.ActualEndDate) AS NVARCHAR(5)) 'DefinationName', 'Actual' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
			INTO #qtActual
			FROM PM_ProjectSites st
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where st.ProjectId=@ProjectId  AND pst.IsActive=1 AND pst.ActualEndDate IS NOT NULL
			AND pst.ActualEndDate BETWEEN @FromDate AND @ToDate AND st.CityId=@Value1
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by 'Q-'+CAST(DATENAME(quarter,pst.ActualEndDate) AS NVARCHAR(5))

			SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
			SUM(x.TotalSites) OVER(ORDER BY x.DefinationName ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #qFctSites
			FROM #qtForecast x
			group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

			SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
			SUM(x.TotalSites) OVER(ORDER BY x.DefinationName ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #qActSites
			FROM #qtActual x
			group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

			drop table #qtForecast
			drop table #qtActual	

			SELECT x.*
			FROM
			(
				SELECT y.TotalSites, y.DefinationId, y.DefinationName 'DefinationName', 'Forecast' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #qFctSites y			
				UNION ALL
				SELECT y.RunningTotal, y.DefinationId, y.DefinationName 'DefinationName', 'CumForecast' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #qFctSites y				
				UNION ALL
				SELECT y.TotalSites, y.DefinationId, y.DefinationName 'DefinationName', 'Actual' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #qActSites y			
				UNION ALL
				SELECT y.RunningTotal, y.DefinationId, y.DefinationName 'DefinationName', 'CumActual' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #qActSites y	
			) x
			ORDER BY x.DefinationName

			drop table #wFctSites
			drop table #wActSites
		END	
		ELSE IF @SearchFilter='Yearly'
		BEGIN
			select COUNT(st.ProjectSiteId ) 'TotalSites' ,0 'DefinationId',CAST(DATENAME(year,pst.EstimatedStartDate) AS NVARCHAR(5)) 'DefinationName', 'Forecast' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
			INTO #yrForecast
			FROM PM_ProjectSites st
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where st.ProjectId=@ProjectId  AND pst.IsActive=1 AND pst.EstimatedStartDate IS NOT NULL
			AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate AND st.CityId=@Value1
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by CAST(DATENAME(year,pst.EstimatedStartDate) AS NVARCHAR(5))

			select COUNT(st.ProjectSiteId ) 'TotalSites' ,0 'DefinationId',CAST(DATENAME(year,pst.ActualEndDate) AS NVARCHAR(5)) 'DefinationName', 'Actual' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
			INTO #yrActual
			FROM PM_ProjectSites st
			INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
			INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
			where st.ProjectId=@ProjectId  AND pst.IsActive=1 AND pst.ActualEndDate IS NOT NULL
			AND pst.ActualEndDate BETWEEN @FromDate AND @ToDate AND st.CityId=@Value1
			AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			AND pt.TaskTypeId =(SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.KeyCode='PROJECT_MILESTONE' AND x.IsActive=1)
			group by CAST(DATENAME(year,pst.ActualEndDate) AS NVARCHAR(5))

			SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
			SUM(x.TotalSites) OVER(ORDER BY x.DefinationName ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #yFctSites
			FROM #yrForecast x
			group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

			SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
			SUM(x.TotalSites) OVER(ORDER BY x.DefinationName ROWS UNBOUNDED PRECEDING) AS RunningTotal
			INTO #yActSites
			FROM #yrActual x
			group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

			drop table #yrForecast
			drop table #yrActual	

			SELECT x.*
			FROM
			(
				SELECT y.TotalSites, y.DefinationId, y.DefinationName 'DefinationName', 'Forecast' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #yFctSites y			
				UNION ALL
				SELECT y.RunningTotal, y.DefinationId, y.DefinationName 'DefinationName', 'CumForecast' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
				FROM #yFctSites y				
				UNION ALL
				SELECT y.TotalSites, y.DefinationId, y.DefinationName 'DefinationName', 'Actual' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #yActSites y			
				UNION ALL
				SELECT y.RunningTotal, y.DefinationId, y.DefinationName 'DefinationName', 'CumActual' 'Type', 
				(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
				FROM #yActSites y		
			) x
			ORDER BY x.DefinationName

			drop table #yFctSites
			drop table #yActSites
		END
	END
	ELSE IF @Filter='Get_RegionView_WO'
	BEGIN
		select COUNT(pst.ProjectSiteId ) 'TotalSites' ,rgn.DefinationId,rgn.DefinationName 'DefinationName',  'Forecast' 'Type',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Forecast') 'ColorCode'
		INTO #rgnForecast
		from PM_ProjectSites st
		INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
		INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
		inner join AD_Definations cty on cty.DefinationId=st.CityId
		inner join AD_Definations rgn on rgn.DefinationId=cty.PDefinationId
		inner join AD_Definations stt on stt.DefinationId=rgn.PDefinationId
		WHERE st.ProjectId=@ProjectId AND pst.EstimatedStartDate IS NOT NULL
		AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate
		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
		AND Charindex(cast(pt.TaskId as varchar(max))+',', @Tasks) > 0
		group by rgn.DefinationId,rgn.DefinationName,stt.DefinationName
		
		select COUNT(pst.ProjectSiteId ) 'TotalSites' ,rgn.DefinationId,rgn.DefinationName 'DefinationName',  'Actual' 'Type',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Program View' AND x.DataSeries='Actual') 'ColorCode'
		INTO #rgnActual
		from PM_ProjectSites st
		INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
		INNER JOIN PM_Tasks AS pt ON pt.TaskId=pst.TaskId
		inner join AD_Definations cty on cty.DefinationId=st.CityId
		inner join AD_Definations rgn on rgn.DefinationId=cty.PDefinationId
		inner join AD_Definations stt on stt.DefinationId=rgn.PDefinationId
		WHERE st.ProjectId=@ProjectId  AND pst.ActualEndDate IS NOT NULL
		AND pst.ActualEndDate BETWEEN @FromDate AND @ToDate
		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
		AND Charindex(cast(pt.TaskId as varchar(max))+',', @Tasks) > 0
		group by rgn.DefinationId,rgn.DefinationName,stt.DefinationName
		
		SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
		SUM(x.TotalSites) OVER(ORDER BY x.DefinationId ROWS UNBOUNDED PRECEDING) AS RunningTotal
		INTO #rFctSites
		FROM #rgnForecast x
		group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

		SELECT x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode,
		SUM(x.TotalSites) OVER(ORDER BY x.DefinationId ROWS UNBOUNDED PRECEDING) AS RunningTotal
		INTO #rActSites
		FROM #rgnActual x
		group by x.TotalSites, x.DefinationId,x.DefinationName, x.Type, x.ColorCode

		drop table #rgnForecast
		drop table #rgnActual

		SELECT x.*
			FROM
			(
				SELECT x.TotalSites, x.DefinationId,x.DefinationName, 'Forecast' 'Type', x.ColorCode
				FROM #rFctSites x
				UNION ALL
				SELECT x.RunningTotal, x.DefinationId,x.DefinationName, 'CumForecast' 'Type', x.ColorCode
				FROM #rFctSites x
				UNION ALL
				SELECT x.TotalSites, x.DefinationId,x.DefinationName, 'Actual' 'Type', x.ColorCode
				FROM #rActSites x
				UNION ALL
				SELECT x.RunningTotal, x.DefinationId,x.DefinationName, 'CumActual' 'Type', x.ColorCode
				FROM #rActSites x
			) x
			ORDER BY x.DefinationName

		drop table #rFctSites
		drop table #rActSites
	END
	ELSE IF @Filter='Get_ProjectIssue_WO'
	BEGIN
		--@Value1: UserId
		select issue.ProjectSiteId,issue.ReasonId,issue.IssueById,st.FACode,issue.IsUnavoidable,pri.DefinationName 'Priority',sts.DefinationName 'Status',tsk.Title 'Task' ,issue.Description,
		ISNULL((
			select CASE WHEN def.DefinationName='Close' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END
			from AD_Definations def
			WHERE def.DefinationId=(SELECT TOP 1 pil.StatusId FROM PM_IssuesLog AS pil WHERE pil.IssueId=issue.IssueId ORDER BY pil.CreatedOn DESC)
		),CAST(0 AS BIT)) 'LogStatus',
		
		--(select def.DefinationName from AD_Definations def where def.DefinationId= issuelog.StatusId) 'LogStatus',
		--ISNULL(users.FirstName,'' )+' '+ISNULL( users.LastName,'') as AssingedTo,
		SUBSTRING((SELECT DISTINCT ', '+ISNULL(usr.FirstName,'' )+' '+ISNULL( usr.LastName,'') AS [text()]
					FROM Sec_Users usr	
					WHERE  Charindex(cast(usr.UserId as varchar(max))+',', issue.AssignedToId+',')>0	
		FOR XML PATH('')), 2, 1000) 'AssingedTo',		
		GETDATE() 'TargetDate',pri.ColorCode as PriorityColor ,
		issue.IssueId,issue.TaskTypeId,issue.IssuePriorityId,issue.IssueStatusId,issue.TaskId,
		issue.AssignedToId,		
		issue.ForecastDate,'' 'UpdatedBy','' 'LastUpdated'
		from PM_Issues issue
		inner join PM_ProjectSites st on st.ProjectSiteId=issue.ProjectSiteId
		left join AD_Definations pri on pri.DefinationId=issue.IssuePriorityId 
		left join AD_Definations sts on sts.DefinationId= issue.IssueStatusId
		left join PM_SiteTasks stk on stk.SiteTaskId= issue.TaskId AND stk.ProjectSiteId=issue.ProjectSiteId
		left join PM_Tasks tsk on tsk.TaskId= stk.TaskId
		--inner join Sec_Users users on issue.AssignedToId = users.UserId 
		where 
		--issue.IssueId=41095
		issue.ProjectId = @ProjectId 
		AND issue.RequestDate BETWEEN @FromDate AND @ToDate
		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
		AND Charindex(cast(issue.StatusId as varchar(max))+',', @Tasks) > 0
		AND (pri.DefinationName LIKE '%'+ ISNULL(@FilterOption,'')+'%' 
		OR sts.DefinationName LIKE '%'+ ISNULL(@FilterOption,'') + '%' OR tsk.Title  LIKE '%' +ISNULL(@FilterOption,'') + '%' 
		--OR issue.Description LIKE '%' +@FilterOption+ '%' OR users.FirstName  LIKE '%' +@FilterOption + '%' 
		--OR users.LastName LIKE '%' +@FilterOption+ '%' 
		OR issue.TargetDate LIKE '%' +ISNULL(@FilterOption,'')+ '%'
		OR st.FACode LIKE '%' +ISNULL(@FilterOption,'')+ '%')		
		Order by [ProjectSiteId] DESC OFFSET @Offset ROWS FETCH NEXT @Page  ROWS ONLY


		select COUNT(*) 'Count'
		from PM_Issues issue
		inner join PM_ProjectSites st on st.ProjectSiteId=issue.ProjectSiteId
		left join AD_Definations pri on pri.DefinationId=issue.IssuePriorityId 
		left join AD_Definations sts on sts.DefinationId= issue.IssueStatusId
		left join PM_SiteTasks stk on stk.SiteTaskId= issue.TaskId AND stk.ProjectSiteId=issue.ProjectSiteId
		left join PM_Tasks tsk on tsk.TaskId= stk.TaskId
		where issue.ProjectId = @ProjectId 
		AND issue.RequestDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
		AND Charindex(cast(issue.StatusId as varchar(max))+',', @Tasks) > 0
		AND (pri.DefinationName LIKE '%'+ @FilterOption+'%' 
		OR sts.DefinationName LIKE '%'+ @FilterOption + '%' OR tsk.Title  LIKE '%' +@FilterOption + '%' 
		--OR issue.Description LIKE '%' +@FilterOption+ '%' OR users.FirstName  LIKE '%' +@FilterOption + '%' 
		--OR users.LastName LIKE '%' +@FilterOption+ '%' 
		OR issue.TargetDate LIKE '%' +@FilterOption+ '%'
		OR st.FACode LIKE '%' +@FilterOption+ '%')		
	END
	ELSE IF @Filter='Get_WO_Issues'
	BEGIN
		--@Value1: UserId
		SELECT issue.ProjectSiteId,issue.ReasonId,issue.IssueById,st.FACode,issue.IsUnavoidable,pri.DefinationName 'Priority',sts.DefinationName 'Status',tsk.Title 'Task' ,issue.Description,
		ISNULL((
			select CASE WHEN def.DefinationName='Close' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END
			from AD_Definations def
			WHERE def.DefinationId=(SELECT TOP 1 pil.StatusId FROM PM_IssuesLog AS pil WHERE pil.IssueId=issue.IssueId ORDER BY pil.CreatedOn DESC)
		),CAST(0 AS BIT)) 'LogStatus',
		
		--(select def.DefinationName from AD_Definations def where def.DefinationId= issuelog.StatusId) 'LogStatus',
		--ISNULL(users.FirstName,'' )+' '+ISNULL( users.LastName,'') as AssingedTo,
		SUBSTRING((SELECT DISTINCT ', '+ISNULL(usr.FirstName,'' )+' '+ISNULL( usr.LastName,'') AS [text()]
					FROM Sec_Users usr	
					WHERE  Charindex(cast(usr.UserId as varchar(max))+',', issue.AssignedToId+',')>0	
		FOR XML PATH('')), 2, 1000) 'AssingedTo',		
		issue.TargetDate,pri.ColorCode as PriorityColor ,
		issue.IssueId,issue.TaskTypeId,issue.IssuePriorityId,issue.IssueStatusId,issue.TaskId,
		issue.AssignedToId,		
		issue.ForecastDate,issue.RequestDate 'CreatedOn',ISNULL(issue.ItemFilePath,'') 'ItemFilePath'
		from PM_Issues issue
		inner join PM_ProjectSites st on st.ProjectSiteId=issue.ProjectSiteId
		inner join AD_Definations pri on pri.DefinationId=issue.IssuePriorityId 
		inner join AD_Definations sts on sts.DefinationId= issue.IssueStatusId
		left join PM_SiteTasks stk on stk.SiteTaskId= issue.TaskId AND stk.ProjectSiteId=issue.ProjectSiteId
		left join PM_Tasks tsk on tsk.TaskId= stk.TaskId
		--inner join Sec_Users users on issue.AssignedToId = users.UserId 
		where issue.ProjectId = @ProjectId 
		AND issue.ProjectSiteId=@Value1
		Order by [ProjectSiteId] DESC
	END
	ELSE IF @Filter='Get_Calendar_WO'
	BEGIN
		select COUNT(task.ActualEndDate) 'TotalTask',task.Title, CAST (ActualEndDate as date) 'ActualDate'
		from PM_Tasks task where ProjectId = @ProjectId 
		and  TaskTypeId = (SELECT top(1) x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Stage')
		group by task.ActualEndDate ,task.Title
	END
	ELSE IF @Filter='Get_Defination_Issue'
	BEGIN
		select def.DefinationId,def.DefinationName,def.KeyCode,def.ColorCode from AD_Definations def where  
		def.KeyCode Like '%Issue_%' or def.KeyCode = 'PROJECT_MILESTONE' or def.KeyCode = 'PROJECT_STAGE'
		or def.KeyCode = 'REASON' or def.KeyCode = 'ISSUE CATEGORY' or def.KeyCode = 'Project Site Status' 
		or def.KeyCode = 'Milestone Window' or def.KeyCode = 'ALARMS' or def.KeyCode ='GNG STATUS'
		or def.KeyCode = 'Severity' or def.KeyCode = 'ACTIVITY TYPE' or def.KeyCode ='ITEM TYPE' or def.KeyCode ='PROJECT DELIVERY'
		
	END
	ELSE IF @Filter='Get_Site_LatLong'
	BEGIN
		--select  st.Latitude,st.Longitude,'TSS' 'Stage'
		--from PM_ProjectSites as st 
		--inner join AD_Clients as cln on cln.ClientId = st.ClientId		
		--inner join AD_Definations city on city.DefinationId= st.CityId
		--inner join AD_Definations rgn on rgn.DefinationId= city.PDefinationId
		----inner join AD_Definations reg on reg.DefinationId =(select pDefinationId from AD_Definations where DefinationId= st.CityId)
		--left join AD_Definations sts on sts.DefinationId = st.StatusId
		--WHERE st.ProjectSiteId IN(SELECT DISTINCT x.ProjectSiteId 
		--                          FROM PM_SiteTasks x
		--                          INNER JOIN PM_ProjectSites AS pps ON x.ProjectSiteId=pps.ProjectSiteId
		--                          WHERE x.IsActive=1 AND CAST(x.EstimatedStartDate AS DATE)=CAST(GETDATE() AS DATE) 
		--                          AND Charindex(cast(pps.CityId as varchar(max))+',', @Markets) > 0
		--                          AND pps.ProjectId=@ProjectId)
		
		--SELECT @Tasks,@FromDate,@ToDate,@Markets
		IF (@Tasks is not null AND @Tasks!=',' AND @Tasks!='')
		BEGIN
		SELECT x.*
		FROM
		(
		SELECT x.ProjectSiteId, x.Latitude, x.Longitude,
		--CASE WHEN x.tssDate IS NULL AND x.ssmDate IS NULL AND x.eplDate IS NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'PENDING'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NULL AND x.eplDate IS NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NOT NULL AND x.eplDate IS NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS_SSM'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NULL AND x.eplDate IS NOT NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS_EPL'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NOT NULL AND x.eplDate IS NOT NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS_SSM_EPL'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NOT NULL AND x.eplDate IS NOT NULL AND x.pinstDate IS NOT NULL AND x.migDate IS NULL THEN 'PREINSTALL'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NOT NULL AND x.eplDate IS NOT NULL AND x.pinstDate IS NOT NULL AND x.migDate IS NOT NULL THEN 'MIGRATION'
		CASE WHEN x.tssDate IS NULL AND x.ssmDate IS NULL AND x.eplDate IS NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'PENDING'			 
			 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NOT NULL AND x.eplDate IS NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS_SSM'
			 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NULL AND x.eplDate IS NOT NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS_EPL'			 
			 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NOT NULL AND x.eplDate IS NOT NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS_SSM_EPL'
			 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NULL AND x.eplDate IS NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS'
			 WHEN x.pinstDate IS NOT NULL AND x.migDate IS NULL THEN 'PREINSTALL'
			 WHEN x.migDate IS NOT NULL THEN 'MIGRATE'
		ELSE 'PENDING'
		END 'Stage'
		FROM
		(
			SELECT sit.ProjectSiteId, sit.Latitude, sit.Longitude, tss.ActualEndDate 'tssDate', ssm.ActualEndDate 'ssmDate', epl.ActualEndDate 'eplDate', pinst.ActualEndDate 'pinstDate', mig.ActualEndDate 'migDate'
			FROM
			(
				select  st.ProjectSiteId,st.Latitude,st.Longitude
				from PM_ProjectSites as st
				inner join AD_Clients as cln on cln.ClientId = st.ClientId		
				inner join AD_Definations city on city.DefinationId= st.CityId
				inner join AD_Definations rgn on rgn.DefinationId= city.PDefinationId
				--inner join AD_Definations reg on reg.DefinationId =(select pDefinationId from AD_Definations where DefinationId= st.CityId)
				LEFT JOIN AD_Definations sts on sts.DefinationId = st.StatusId				
				WHERE st.ProjectId=@ProjectId AND st.IsActive=1				
				--AND st.CreatedOn BETWEEN '2017-06-01' AND '2018-01-31'
				AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
				--AND Charindex(cast(st.StatusId as varchar(max))+',', @MapStatus) > 0
			) sit
			LEFT OUTER JOIN
			(
				SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
				FROM PM_SiteTasks AS pst
				INNER JOIN PM_ProjectSites sit on sit.ProjectSiteId=pst.ProjectSiteId
				WHERE pst.IsActive=1 AND pst.TaskId=50076 AND pst.ActualEndDate IS NOT NULL
				AND  pst.ActualEndDate BETWEEN @FromDate AND @ToDate
				AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0 and sit.IsActive=1 and ProjectId=@ProjectId
				AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
			) tss ON sit.ProjectSiteId=tss.ProjectSiteId
			LEFT OUTER JOIN
			(
				SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
				FROM PM_SiteTasks AS pst
				INNER JOIN PM_ProjectSites sit on sit.ProjectSiteId=pst.ProjectSiteId
				WHERE pst.IsActive=1 AND pst.TaskId=50077 AND pst.ActualEndDate IS NOT NULL
				AND  pst.ActualEndDate BETWEEN @FromDate AND @ToDate
				AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
				AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0 and sit.IsActive=1 and ProjectId=@ProjectId
			) ssm ON sit.ProjectSiteId=ssm.ProjectSiteId
			LEFT OUTER JOIN
			(
				SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
				FROM PM_SiteTasks AS pst
				INNER JOIN PM_ProjectSites sit on sit.ProjectSiteId=pst.ProjectSiteId
				WHERE pst.IsActive=1 AND pst.TaskId=50078 AND pst.ActualEndDate IS NOT NULL
				AND  pst.ActualEndDate BETWEEN @FromDate AND @ToDate
				AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
				AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0 and sit.IsActive=1 and ProjectId=@ProjectId
			) epl ON sit.ProjectSiteId=epl.ProjectSiteId
			LEFT OUTER JOIN
			(
				SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
				FROM PM_SiteTasks AS pst
				INNER JOIN PM_ProjectSites sit on sit.ProjectSiteId=pst.ProjectSiteId
				WHERE pst.IsActive=1 AND pst.TaskId=50079 AND pst.ActualEndDate IS NOT NULL
				AND  pst.ActualEndDate BETWEEN @FromDate AND @ToDate
				AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
				AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0 and sit.IsActive=1 and ProjectId=@ProjectId
			) pinst ON sit.ProjectSiteId=pinst.ProjectSiteId
			LEFT OUTER JOIN
			(
				SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
				FROM PM_SiteTasks AS pst
				INNER JOIN PM_ProjectSites sit on sit.ProjectSiteId=pst.ProjectSiteId
				WHERE pst.IsActive=1 AND pst.TaskId=50080 AND pst.ActualEndDate IS NOT NULL
				AND  pst.ActualEndDate BETWEEN @FromDate AND @ToDate
				AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
				AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0 and sit.IsActive=1 and ProjectId=@ProjectId
			) mig ON sit.ProjectSiteId=mig.ProjectSiteId
		) x
		) x
		WHERE x.Stage!='PENDING'
		END
		--ELSE IF (@MapStatus is not null AND @MapStatus!=',' AND @MapStatus!='')
		--BEGIN
		--SELECT x.ProjectSiteId, x.Latitude, x.Longitude,
		--CASE WHEN x.tssDate IS NULL AND x.ssmDate IS NULL AND x.eplDate IS NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'PENDING'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NULL AND x.eplDate IS NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NOT NULL AND x.eplDate IS NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS_SSM'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NULL AND x.eplDate IS NOT NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS_EPL'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NOT NULL AND x.eplDate IS NOT NULL AND x.pinstDate IS NULL AND x.migDate IS NULL THEN 'TSS_SSM_EPL'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NOT NULL AND x.eplDate IS NOT NULL AND x.pinstDate IS NOT NULL AND x.migDate IS NULL THEN 'PREINSTALL'
		--	 WHEN x.tssDate IS NOT NULL AND x.ssmDate IS NOT NULL AND x.eplDate IS NOT NULL AND x.pinstDate IS NOT NULL AND x.migDate IS NOT NULL THEN 'MIGRATION'
		--ELSE 'PENDING'
		--END 'Stage'
		--FROM
		--(
		--	SELECT sit.ProjectSiteId, sit.Latitude, sit.Longitude, tss.ActualEndDate 'tssDate', ssm.ActualEndDate 'ssmDate', epl.ActualEndDate 'eplDate', pinst.ActualEndDate 'pinstDate', mig.ActualEndDate 'migDate'
		--	FROM
		--	(
		--		select  st.ProjectSiteId,st.Latitude,st.Longitude
		--		from PM_ProjectSites as st
		--		inner join AD_Clients as cln on cln.ClientId = st.ClientId		
		--		inner join AD_Definations city on city.DefinationId= st.CityId
		--		inner join AD_Definations rgn on rgn.DefinationId= city.PDefinationId
		--		--inner join AD_Definations reg on reg.DefinationId =(select pDefinationId from AD_Definations where DefinationId= st.CityId)
		--		INNER JOIN AD_Definations sts on sts.DefinationId = st.StatusId				
		--		WHERE st.ProjectId=@ProjectId AND st.IsActive=1
		--		AND st.SiteDate BETWEEN @FromDate AND @ToDate
		--		--AND st.CreatedOn BETWEEN '2017-06-01' AND '2018-01-31'
		--		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
		--		AND Charindex(cast(st.StatusId as varchar(max))+',', @MapStatus) > 0
		--	) sit
		--	LEFT OUTER JOIN
		--	(
		--		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
		--		FROM PM_SiteTasks AS pst
		--		WHERE pst.IsActive=1 AND pst.TaskId=50076 AND pst.ActualEndDate IS NOT NULL
		--	) tss ON sit.ProjectSiteId=tss.ProjectSiteId
		--	LEFT OUTER JOIN
		--	(
		--		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
		--		FROM PM_SiteTasks AS pst
		--		WHERE pst.IsActive=1 AND pst.TaskId=50077 AND pst.ActualEndDate IS NOT NULL
		--	) ssm ON sit.ProjectSiteId=ssm.ProjectSiteId
		--	LEFT OUTER JOIN
		--	(
		--		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
		--		FROM PM_SiteTasks AS pst
		--		WHERE pst.IsActive=1 AND pst.TaskId=5008 AND pst.ActualEndDate IS NOT NULL
		--	) epl ON sit.ProjectSiteId=epl.ProjectSiteId
		--	LEFT OUTER JOIN
		--	(
		--		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
		--		FROM PM_SiteTasks AS pst
		--		WHERE pst.IsActive=1 AND pst.TaskId=50079 AND pst.ActualEndDate IS NOT NULL
		--	) pinst ON sit.ProjectSiteId=pinst.ProjectSiteId
		--	LEFT OUTER JOIN
		--	(
		--		SELECT pst.ProjectSiteId,pst.PlannedDate, pst.EstimatedStartDate, pst.ActualEndDate, pst.TargetDate
		--		FROM PM_SiteTasks AS pst
		--		WHERE pst.IsActive=1 AND pst.TaskId=50080 AND pst.ActualEndDate IS NOT NULL
		--	) mig ON sit.ProjectSiteId=mig.ProjectSiteId
		--) x
		--END
		
		
		--MAP LEGEND
		SELECT '#333333' 'ColorCode','PENDING' 'Stage'
		union all
		SELECT '#bf000e' 'ColorCode','TSS' 'Stage'
		union all
		SELECT '#ad2fad' 'ColorCode','TSS_SSM' 'Stage'
		union all
		SELECT '#9e005d' 'ColorCode','TSS_EPL' 'Stage'
		union all
		SELECT '#45004f' 'ColorCode','TSS_SSM_EPL' 'Stage'
		union all
		SELECT '#39b54a' 'ColorCode','PREINSTALL' 'Stage'
		union all
		SELECT '#008c0d' 'ColorCode','ONGOING' 'Stage'
		union all
		SELECT '#00592c' 'ColorCode','MIGRATED' 'Stage'
		union all
		SELECT '#012b01' 'ColorCode','COMPLETED' 'Stage'
	END
	ELSE IF @Filter='Get_Region_Issue'
	BEGIN		
		select reg.DefinationId,reg.DefinationName+' '+stt.DefinationName 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
		CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue', 
		--ISNULL(ac.ClientName,'Un-Avoidable') 'Issue',
		COUNT(IssueId) 'TotalSite',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
		from PM_Issues issue 
		inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
		inner join AD_Definations cty on cty.DefinationId=sit.CityId
		inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
		inner join AD_Definations stt on stt.DefinationId =reg.PDefinationId
		left join PM_SiteTasks stk on stk.SiteTaskId= issue.TaskId AND stk.ProjectSiteId=issue.ProjectSiteId
		left join PM_Tasks tsk on tsk.TaskId= stk.TaskId
		LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
		where issue.ProjectId = @ProjectId and reg.IsActive=1 --and sit.IsActive=1 and reg.isActive=1 and stt.IsActive=1
		AND issue.RequestDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
		--AND Charindex(cast(stk.TaskId as varchar(max))+',', @Tasks) > 0
		Group By reg.DefinationId,reg.DefinationName+' '+stt.DefinationName, sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
	END
	ELSE IF @Filter='Get_Market_Issue'
	BEGIN
		--@Value1: RegionId	
		
		select cty.DefinationId,cty.DefinationName 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
		CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue',
		COUNT(IssueId) 'TotalSite',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
		from PM_Issues issue 
		inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
		inner join AD_Definations cty on cty.DefinationId=sit.CityId
		inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
		inner join AD_Definations stt on stt.DefinationId =reg.PDefinationId
		left join PM_SiteTasks stk on stk.SiteTaskId= issue.TaskId AND stk.ProjectSiteId=issue.ProjectSiteId
		left join PM_Tasks tsk on tsk.TaskId= stk.TaskId
		LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
		where issue.ProjectId = @ProjectId AND cty.PDefinationId=@Value1 and reg.IsActive=1
		AND issue.RequestDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
		--AND Charindex(cast(stk.TaskId as varchar(max))+',', @Tasks) > 0
		Group By cty.DefinationId,cty.DefinationName, sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
	END
	ELSE IF @Filter='Get_Datewise_Issue'
	BEGIN
		--@Value1: MarketId
		IF @SearchFilter='' OR @SearchFilter='Daily'
		BEGIN
			select 0 'DefinationId', issue.RequestDate 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
			CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue',
			COUNT(IssueId) 'TotalSite',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
			from PM_Issues issue 
			inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
			inner join AD_Definations cty on cty.DefinationId=sit.CityId
			inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
			inner join AD_Definations stt on stt.DefinationId =reg.PDefinationId
			left join PM_SiteTasks stk on stk.SiteTaskId= issue.TaskId AND stk.ProjectSiteId=issue.ProjectSiteId
			left join PM_Tasks tsk on tsk.TaskId= stk.TaskId
			LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
			where issue.ProjectId = @ProjectId AND sit.CityId=@Value1 and reg.IsActive=1
			AND issue.RequestDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
		--AND Charindex(cast(stk.TaskId as varchar(max))+',', @Tasks) > 0
			Group By issue.RequestDate, sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
		END
		ELSE IF @SearchFilter='Weekly'
		BEGIN
			select 0 'DefinationId', 'W-'+CAST(DATENAME(week, issue.RequestDate) AS NVARCHAR(5)) 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
			CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue',
			COUNT(IssueId) 'TotalSite',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
			from PM_Issues issue 
			inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
			inner join AD_Definations cty on cty.DefinationId=sit.CityId
			inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
			inner join AD_Definations stt on stt.DefinationId =reg.PDefinationId
			left join PM_SiteTasks stk on stk.SiteTaskId= issue.TaskId AND stk.ProjectSiteId=issue.ProjectSiteId
			left join PM_Tasks tsk on tsk.TaskId= stk.TaskId
			LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
			where issue.ProjectId = @ProjectId AND sit.CityId=@Value1 and reg.IsActive=1
			AND issue.RequestDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
		--AND Charindex(cast(stk.TaskId as varchar(max))+',', @Tasks) > 0
			Group By DATENAME(week, issue.RequestDate), sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
		END
		ELSE IF @SearchFilter='Monthly'
		BEGIN
			select 0 'DefinationId', CAST(issue.RequestDate AS CHAR(3))+'-'+RIGHT(YEAR(issue.RequestDate),2) 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
			CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue',
			COUNT(IssueId) 'TotalSite',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
			from PM_Issues issue 
			inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
			inner join AD_Definations cty on cty.DefinationId=sit.CityId
			inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
			inner join AD_Definations stt on stt.DefinationId =reg.PDefinationId
			left join PM_SiteTasks stk on stk.SiteTaskId= issue.TaskId AND stk.ProjectSiteId=issue.ProjectSiteId
			left join PM_Tasks tsk on tsk.TaskId= stk.TaskId
			LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
			where issue.ProjectId = @ProjectId AND sit.CityId=@Value1 and reg.IsActive=1
			AND issue.RequestDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
		--AND Charindex(cast(stk.TaskId as varchar(max))+',', @Tasks) > 0
			Group By CAST(issue.RequestDate AS CHAR(3))+'-'+RIGHT(YEAR(issue.RequestDate),2), sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
		END
		ELSE IF @SearchFilter='Quaterly'
		BEGIN
			select 0 'DefinationId', 'Q-'+CAST(DATENAME(quarter, issue.RequestDate) AS NVARCHAR(5)) 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
			CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue',
			COUNT(IssueId) 'TotalSite',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
			from PM_Issues issue 
			inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
			inner join AD_Definations cty on cty.DefinationId=sit.CityId
			inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
			inner join AD_Definations stt on stt.DefinationId =reg.PDefinationId
			left join PM_SiteTasks stk on stk.SiteTaskId= issue.TaskId AND stk.ProjectSiteId=issue.ProjectSiteId
			left join PM_Tasks tsk on tsk.TaskId= stk.TaskId
			LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
			where issue.ProjectId = @ProjectId AND sit.CityId=@Value1 and reg.IsActive=1
			AND issue.RequestDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
		--AND Charindex(cast(stk.TaskId as varchar(max))+',', @Tasks) > 0
			Group By DATENAME(quarter, issue.RequestDate), sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
		END
		ELSE IF @SearchFilter='Yearly'
		BEGIN
			select 0 'DefinationId', CAST(DATENAME(year, issue.RequestDate) AS NVARCHAR(5)) 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
			CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue',
			COUNT(IssueId) 'TotalSite',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
			from PM_Issues issue 
			inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
			inner join AD_Definations cty on cty.DefinationId=sit.CityId
			inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
			inner join AD_Definations stt on stt.DefinationId =reg.PDefinationId
			left join PM_SiteTasks stk on stk.SiteTaskId= issue.TaskId AND stk.ProjectSiteId=issue.ProjectSiteId
			left join PM_Tasks tsk on tsk.TaskId= stk.TaskId
			LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
			where issue.ProjectId = @ProjectId AND sit.CityId=@Value1 and reg.IsActive=1
			AND issue.RequestDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
			--AND Charindex(cast(stk.TaskId as varchar(max))+',', @Tasks) > 0
			Group By DATENAME(year, issue.RequestDate), sit.ProjectId, ac.ClientId, ac.ClientName, issue.IsUnavoidable
		END
	END
	ELSE IF @Filter='Get_Project_Issue_By'
	BEGIN
		--@Value1: MarketId
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
		AND issue.RequestDate BETWEEN @FromDate AND @ToDate AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
		Group By sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
		)x
		WHERE x.IssueBy IS NOT NULL
	END
	else IF @Filter='Dashboard_Tasks_Summary'
	BEGIN
		--SELECT task.TaskId, pt.PTaskId, pt.ProjectId,pt.Title 'Task' ,COUNT(DISTINCT st.ProjectSiteId) as ActualSites,0 as TotalSites ,pt.Color as Color, ad.DefinationName 'TaskType'
		----task.ActualEndDate,task.ActualEndDate,sts.DefinationName 'Status'
		--FROM PM_SiteTasks as task
		--inner join PM_Tasks pt on pt.TaskId=task.TaskId
		--inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
		--INNER JOIN AD_Definations AS ad ON ad.DefinationId=pt.TaskTypeId
		--INNER JOIN AD_Definations AS sts ON sts.DefinationId=task.StatusId
		--where pt.ProjectId = @ProjectId and pt.IsActive=1		
		--AND task.EstimatedStartDate <=GETDATE()
		--AND task.ActualEndDate IS NOT NULL
		--AND Charindex(cast(pt.TaskId as varchar(max))+',', @value1+',') > 0
		--GROUP BY task.TaskId, pt.PTaskId, pt.ProjectId,pt.Title, pt.Color, ad.DefinationName, pt.SortOrder
		--ORDER BY pt.PTaskId,task.TaskId, pt.SortOrder

		SELECT * from dashboarddata
		WHERE Charindex(cast(TaskId as varchar(max))+',', @value1+',') > 0
		
	END
	ELSE IF @Filter='GET_PROJECT_READINESS'
	BEGIN
		select pt.TaskId 'DefinitionId',pt.Title 'DefinationName', pt.ProjectId, COUNT(DISTINCT st.ProjectSiteId) 'WoCount', 'Yesterday' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Readiness View' AND x.DataSeries='Yesterday') 'ColorCode'
		FROM PM_SiteTasks as task
		inner join PM_Tasks pt on pt.TaskId=task.TaskId
		inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
		where pt.ProjectId = @ProjectId and pt.IsActive=1 AND ISNULL(pt.PTaskId,0)=0
		AND (( task.ActualEndDate<GETDATE() AND task.ActualEndDate IS NOT NULL)
		AND task.EstimatedStartDate <=GETDATE())
		GROUP BY pt.TaskId,pt.Title,pt.ProjectId
		UNION ALL
		select pt.TaskId 'DefinitionId',pt.Title 'DefinationName', pt.ProjectId, COUNT(DISTINCT st.ProjectSiteId) 'WoCount', 'Today' 'Type',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Readiness View' AND x.DataSeries='Today') 'ColorCode'
		FROM PM_SiteTasks as task
		inner join PM_Tasks pt on pt.TaskId=task.TaskId
		inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
		where pt.ProjectId = @ProjectId and pt.IsActive=1 AND ISNULL(pt.PTaskId,0)=0
		AND (( task.ActualEndDate=GETDATE() AND task.ActualEndDate IS NOT NULL)
		AND task.EstimatedStartDate <=GETDATE())
		GROUP BY pt.TaskId,pt.Title,pt.ProjectId
		UNION ALL
		SELECT r.DefinitionId,r.DefinationName, r.ProjectId, ISNULL(r.WoCount,0)- (CASE WHEN ISNULL(t.WoCount,0)=0 THEN ISNULL(y.WoCount,0) ELSE ISNULL(t.WoCount,0) END)   'WoCount', 'Remaining' 'Type',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Readiness View' AND x.DataSeries='Remaining') 'ColorCode'
		FROM
		(
			select pt.TaskId 'DefinitionId',pt.Title 'DefinationName', pt.ProjectId, COUNT(DISTINCT st.ProjectSiteId) 'WoCount', 'Total' 'Type'
			FROM PM_SiteTasks as task
			inner join PM_Tasks pt on pt.TaskId=task.TaskId
			inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
			where pt.ProjectId = @ProjectId and pt.IsActive=1 AND ISNULL(pt.PTaskId,0)=0			
			AND task.EstimatedStartDate <=GETDATE()
			GROUP BY pt.TaskId,pt.Title,pt.ProjectId
		) r LEFT JOIN
		(
			select pt.TaskId 'DefinitionId',pt.Title 'DefinationName', pt.ProjectId, COUNT(DISTINCT st.ProjectSiteId) 'WoCount', 'Today' 'Type'
			FROM PM_SiteTasks as task
			inner join PM_Tasks pt on pt.TaskId=task.TaskId
			inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
			where pt.ProjectId = @ProjectId and pt.IsActive=1 AND ISNULL(pt.PTaskId,0)=0
			AND (( task.ActualEndDate=GETDATE() AND task.ActualEndDate IS NOT NULL)
			AND task.EstimatedStartDate <=GETDATE())
			GROUP BY pt.TaskId,pt.Title,pt.ProjectId
		) t  ON r.DefinitionId=t.DefinitionId		
		LEFT JOIN (
			select pt.TaskId 'DefinitionId',pt.Title 'DefinationName', pt.ProjectId, COUNT(DISTINCT st.ProjectSiteId) 'WoCount', 'Yesterday' 'Type'
			FROM PM_SiteTasks as task
			inner join PM_Tasks pt on pt.TaskId=task.TaskId
			inner join PM_ProjectSites st on st.ProjectSiteId=task.ProjectSiteId
			where pt.ProjectId = @ProjectId and pt.IsActive=1 AND ISNULL(pt.PTaskId,0)=0
			AND (( task.ActualEndDate<GETDATE() AND task.ActualEndDate IS NOT NULL)
			AND task.EstimatedStartDate <=GETDATE())
			GROUP BY pt.TaskId,pt.Title,pt.ProjectId
		) y ON r.DefinitionId=y.DefinitionId		
	END
	ELSE IF @Filter='PROGRAM_SUMMARY'
	BEGIN
		DECLARE @Comments AS NVARCHAR(800)=''
		DECLARE @todayPlannedSites AS INT=ISNULL((SELECT COUNT(DISTINCT x.ProjectSiteId) FROM PM_SiteTasks x INNER JOIN PM_ProjectSites AS pps ON x.ProjectSiteId=pps.ProjectSiteId WHERE x.IsActive=1 AND CAST(x.EstimatedStartDate AS DATE) BETWEEN @FromDate AND @ToDate AND x.EstimatedStartDate IS NOT null AND pps.ProjectId=@ProjectId),0)
		DECLARE @todayCompletedSites AS NVARCHAR(5)=ISNULL((SELECT CASE WHEN COUNT(DISTINCT x.ProjectSiteId)=0 THEN '0' ELSE CAST(COUNT(DISTINCT x.ProjectSiteId) AS NVARCHAR(50)) END FROM PM_SiteTasks x INNER JOIN PM_ProjectSites AS pps ON x.ProjectSiteId=pps.ProjectSiteId  WHERE x.IsActive=1 AND CAST(x.ActualEndDate AS DATE) BETWEEN @FromDate AND @ToDate AND x.TaskId=50080 AND pps.ProjectId=@ProjectId AND pps.StatusId=163388),0)
		DECLARE @todayMigratedSites AS NVARCHAR(5)=ISNULL((SELECT CASE WHEN COUNT(DISTINCT x.ProjectSiteId)=0 THEN '0' ELSE CAST(COUNT(DISTINCT x.ProjectSiteId) AS NVARCHAR(50)) END FROM PM_SiteTasks x INNER JOIN PM_ProjectSites AS pps ON x.ProjectSiteId=pps.ProjectSiteId  WHERE x.IsActive=1 AND CAST(x.ActualEndDate AS DATE) BETWEEN @FromDate AND @ToDate AND x.TaskId=50080 AND pps.ProjectId=@ProjectId AND pps.StatusId=163436),0)
		DECLARE @cancelledSites AS NVARCHAR(5)='' 
		
		SELECT @cancelledSites=CAST(COUNT(issue.IssueId) AS NVARCHAR(5))
		from PM_Issues issue 
		inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId	INNER JOIN AD_Definations AS ad ON ad.DefinationId=issue.ReasonId
		LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
		where issue.ProjectId = @ProjectId	
		AND issue.RequestDate BETWEEN @FromDate AND @ToDate 
		--AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
		AND sit.StatusId IN(163436,163388) AND issue.IssueCategoryId=163375
		
		--SELECT @todayPlannedSites 'Planned Sites'@todayCompletedSites 'Completed Sites',@todayMigratedSites 'Migrated Sites',@cancelledSites 'Cancelled Sites',
		--'#2aa847' 'PlanColor','#073e5e' 'CompleteColor','#046012' 'MigrateColor','#ba4618' 'CancelColor'
		
		SELECT @todayPlannedSites 'SiteStatus','#2aa847' 'ColorCode','Planned Sites' 'StatusType'
		UNION ALL
		SELECT @todayCompletedSites'SiteStatus','#073e5e' 'ColorCode','Completed Sites' 'StatusType'
		UNION ALL
		SELECT @todayMigratedSites 'SiteStatus','#046012' 'ColorCode','Migrated Sites' 'StatusType'
		UNION ALL
		SELECT @cancelledSites 'SiteStatus','#ba4618' 'ColorCode','Cancelled Sites' 'StatusType'
		
		
		SELECT ad.DefinationName 'IssueType',CAST(COUNT(issue.IssueId) AS NVARCHAR(5)) 'TotalIssues'
		from PM_Issues issue 
		inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId	
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=issue.ReasonId
		LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
		where issue.ProjectId = @ProjectId
		AND issue.RequestDate BETWEEN @FromDate AND @ToDate 
		--AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
		AND sit.StatusId IN(163436,163388) AND issue.IssueCategoryId=163375
		Group By ad.DefinationName
				
		SELECT 
		CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'IssueOwner',
		COUNT(IssueId) 'TotalIssues',
		(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
		from PM_Issues issue 
		inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId	
		LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
		where issue.ProjectId = @ProjectId		
		AND issue.RequestDate BETWEEN @FromDate AND @ToDate 
		--AND Charindex(cast(sit.CityId as varchar(max))+',', @Markets) > 0
		AND sit.StatusId IN(163436,163388) AND issue.IssueCategoryId=163375
		Group By sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
	END
	ELSE IF @Filter='SITE_DISPOSITON'
	BEGIN
		SELECT pps.StatusId,ad.DefinationName 'Status',COUNT(pps.ProjectSiteId) 'SiteCount',ad.ColorCode
		FROM PM_ProjectSites AS pps
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=pps.StatusId
		WHERE pps.ProjectId=@ProjectId 
		AND pps.ActualEndDate BETWEEN @FromDate AND @ToDate
		--AND pps.StatusId IN(163388,163386,163387,163395,163384)
		AND Charindex(cast(pps.CityId as varchar(max))+',', @Markets) > 0
		GROUP BY pps.StatusId,ad.DefinationName ,ad.ColorCode
	END

		ELSE IF @Filter='GET_COUNT_SITE_STATUS'
	BEGIN
		select COUNT(def.DefinationName) from PM_Issues pmissue
		inner join AD_Definations def on def.DefinationId= pmissue.IssueStatusId
		 where pmissue.ProjectSiteId = @Value1 and def.DefinationName='Completed'
	END

	ELSE IF @Filter='NATIONAL_PROGRAM_ISSUES'
	BEGIN
		--@Value1: MarketId
		IF @SearchFilter='' OR @SearchFilter='Daily'
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
			AND issue.RequestDate BETWEEN @FromDate AND @ToDate
			and reg.IsActive=1
			Group By issue.RequestDate, sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
			) x
			WHERE x.Issue IS NOT NULL
		END
		ELSE IF @SearchFilter='Weekly'
		BEGIN
			SELECT x.*
			FROM
			(
			select 0 'DefinationId', 'W-'+CAST(DATENAME(week, issue.RequestDate) AS NVARCHAR(5)) 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
			CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue',
			COUNT(IssueId) 'TotalSite',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
			from PM_Issues issue 
			inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
			inner join AD_Definations cty on cty.DefinationId=sit.CityId
			inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
			LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
			where issue.ProjectId = @ProjectId
			AND issue.RequestDate BETWEEN @FromDate AND @ToDate
			and reg.IsActive=1
			Group By DATENAME(week, issue.RequestDate), sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
			) x
			WHERE x.Issue IS NOT NULL
		END
		ELSE IF @SearchFilter='Monthly'
		BEGIN
			SELECT x.*
			FROM
			(
			select 0 'DefinationId', CAST(issue.RequestDate AS CHAR(3))+'-'+RIGHT(YEAR(issue.RequestDate),2) 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
			CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue',
			COUNT(IssueId) 'TotalSite',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
			from PM_Issues issue 
			inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
			inner join AD_Definations cty on cty.DefinationId=sit.CityId
			inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
			LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
			where issue.ProjectId = @ProjectId
			AND issue.RequestDate BETWEEN @FromDate AND @ToDate
			and reg.IsActive=1
			Group By CAST(issue.RequestDate AS CHAR(3))+'-'+RIGHT(YEAR(issue.RequestDate),2), sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
			) x
			WHERE x.Issue IS NOT NULL
		END
		ELSE IF @SearchFilter='Quaterly'
		BEGIN
			SELECT x.*
			FROM
			(
			select 0 'DefinationId', 'Q-'+CAST(DATENAME(quarter, issue.RequestDate) AS NVARCHAR(5)) 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
			CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue',
			COUNT(IssueId) 'TotalSite',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
			from PM_Issues issue 
			inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
			inner join AD_Definations cty on cty.DefinationId=sit.CityId
			inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
			LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
			where issue.ProjectId = @ProjectId
			AND issue.RequestDate BETWEEN @FromDate AND @ToDate
			and reg.IsActive=1
			Group By DATENAME(quarter, issue.RequestDate), sit.ProjectId, ac.ClientId, ac.ClientName,issue.IsUnavoidable
			) x
			WHERE x.Issue IS NOT NULL
		END
		ELSE IF @SearchFilter='Yearly'
		BEGIN
			SELECT x.*
			FROM
			(
			select 0 'DefinationId', CAST(DATENAME(year, issue.RequestDate) AS NVARCHAR(5)) 'DefinationName', sit.ProjectId, ISNULL(ac.ClientId,0) 'ReasonId',
			CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END 'Issue',
			COUNT(IssueId) 'TotalSite',
			(SELECT x.ColorCode FROM PM_ChartSettings x WHERE x.PanelName='Issues View' AND x.DataSeries=CASE WHEN ISNULL(issue.IsUnavoidable,0)=CAST(0 AS BIT) THEN ac.ClientName WHEN ISNULL(issue.IsUnavoidable,0)=CAST(1 AS BIT) THEN ISNULL(ac.ClientName,'') + ' Un-Avoidable' ELSE '' END) 'ColorCode'
			from PM_Issues issue 
			inner join PM_ProjectSites sit on sit.ProjectSiteId=issue.ProjectSiteId
			inner join AD_Definations cty on cty.DefinationId=sit.CityId
			inner join AD_Definations reg on reg.DefinationId =cty.PDefinationId
			LEFT JOIN AD_Clients AS ac ON ac.ClientId=issue.IssueById
			where issue.ProjectId = @ProjectId
			AND issue.RequestDate BETWEEN @FromDate AND @ToDate
			and reg.IsActive=1
			Group By DATENAME(year, issue.RequestDate), sit.ProjectId, ac.ClientId, ac.ClientName, issue.IsUnavoidable
			) x
			WHERE x.Issue IS NOT NULL
		END
	END
END