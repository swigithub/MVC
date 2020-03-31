-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE  PROCEDURE PM_GetWorkLogs
 @Filter NVARCHAR(50),
 @SelectOption NVARCHAR(50)=NULL, -- 0, 1,2 (Here 2 for All)
 @StartDate NVARCHAR(150)=NULL,
 @EndDate NVARCHAR(150)=NULL,
 @TaskId numeric(18,0)=0,
 @SiteTaskId nvarchar(max)='0',
 @ProjectSiteId nvarchar(max)='0',
 @ProjectId numeric(18,0)=0,
 @LogType nvarchar(15) = '',
 @WorkGroups NVARCHAR(max)='',
 @Users NVARCHAR(max)='',
 @UserId numeric(18,0)=0,
 @WorkgroupId numeric(18,0)=0
AS
BEGIN
--	 [dbo].[PM_GetWorkLogs] 'WorkLogs','', ''
	IF @Filter='WorkLogs'
	BEGIN
		IF @SelectOption ='0'
		BEGIN
			Select l.*,usr.Picture, usr.[FirstName]+' '+usr.[LastName] as 'Name' from PM_WorkLog  l
			Inner join Sec_Users usr on usr.UserId = l.UserId 		
			Where (l.LogDate BETWEEN @StartDate AND @EndDate) 
			AND (l.IsApproved=0 OR l.IsApproved is NULL)	
			ORDER BY l.WLogId DESC
			--ORDER BY CONVERT(DateTime, l.LogDate,101)  ASC
		END
		ELSE IF @SelectOption ='1'
		BEGIN
			Select l.*,usr.Picture, usr.[FirstName]+' '+usr.[LastName] as 'Name' from PM_WorkLog  l
			Inner join Sec_Users usr on usr.UserId = l.UserId 		
			Where (l.LogDate BETWEEN @StartDate AND @EndDate) 
			AND (l.IsApproved=1 AND l.IsApproved is not NULL)	
			ORDER BY l.WLogId DESC
			--ORDER BY CONVERT(DateTime, l.LogDate,101)  ASC
		END
		ELSE
		BEGIN
			Select l.*,usr.Picture, usr.[FirstName]+' '+usr.[LastName] as 'Name' from PM_WorkLog  l
			Inner join Sec_Users usr on usr.UserId = l.UserId 		
			Where (l.LogDate BETWEEN @StartDate AND @EndDate)
			ORDER BY l.WLogId DESC
			--ORDER BY CONVERT(DateTime, l.LogDate,101)  ASC
		END
	END

	ELSE IF @Filter='WorkLogsCost'
	BEGIN
		Select
		l.[WLogId]
	   ,usr.Picture
	   ,usr.[FirstName]+' '+usr.[LastName] as 'Name'
       ,l.[LogType]
       ,l.[LogDate]
       ,l.[LogHours]
	   ,l.[RatePerUnit]
       ,l.[IsApproved]
       ,l.[ApprovalDate]
		
		FROM PM_WorkLog l
		Inner join Sec_Users usr on usr.UserId = l.UserId 		
		Where (l.LogDate BETWEEN @StartDate AND @EndDate) AND (l.IsApproved=1 and l.ApprovalDate is not NULL)
		ORDER BY --CONVERT(DateTime, l.LogDate,101),
		usr.[FirstName]+' '+usr.[LastName], l.LogType  ASC
	END
	ELSE IF @Filter='Work_Log_Charts'
	BEGIN
		SELECT x.Name,x.LogType,SUM(x.LogHours) 'LogHours', SUM(x.LogCost) 'Cost'
		FROM
		(
			Select usr.[FirstName]+' '+usr.[LastName] as 'Name',l.[LogType],l.[LogHours] LogHours,
			l.[LogHours]*ISNULL(l.[RatePerUnit],0) 'LogCost'
			FROM PM_WorkLog l
			Inner join Sec_Users usr on usr.UserId = l.UserId 	
			Where (l.LogDate BETWEEN @StartDate AND @EndDate) AND (l.IsApproved=1 and l.ApprovalDate is not NULL)
		) x
		group by x.Name,x.LogType Order by x.Name ASC
	END
	ELSE IF @Filter='Get_TaskWorklog'
	BEGIN
		SELECT w.WLogId, w.LogDate, w.LogHours, w.ApprovalDate, w.IsApproved, w.Description from PM_WorkLog w
		WHERE w.TaskId = @TaskId AND w.ProjectSiteId = @ProjectSiteId AND w.LogType = @LogType AND w.ProjectId = @ProjectId
		order by w.WLogId desc
	END
	ELSE IF @Filter='Get_LookupData'
	BEGIN
		--select * from PM_Projects where ProjectId = @ProjectId
		declare @IsManager bit = COALESCE((select IsManager from Sec_Users where UserId=@UserId),0)
		if(@IsManager=1)
		begin
		SELECT * 
	         FROM   PM_Projects pp
	         INNER JOIN Sec_UserProjects AS us ON us.ProjectId=PP.ProjectId
			 left JOIN PM_ProjectEntity AS pe ON pe.Id=IsNull(PP.EntityId, 1)
             WHERE  us.UserId= @UserId and pp.ProjectId = @ProjectId

		select distinct sw.WorkGroupId 'WorkgroupId',sw.Name 'WorkgroupName' from PM_WorkGroup pw
		inner join Sec_WorkGroup sw on sw.WorkGroupId = pw.WorkGroupId
		--inner join Sec_Users su on su.UserId = @UserId
		where ProjectId = @ProjectId

		select distinct tr.GroupId,su.*,su.UserName 'ResourceName',su.UserId 'ResourceId',pwg.ProjectId from Sec_Users su 
		inner join PM_TaskResources tr on tr.ResourceId = su.UserId
		inner join PM_WorkGroup pwg on pwg.WorkGroupId = tr.GroupId
		where pwg.ProjectId = @ProjectId
		end
		else
		begin
		SELECT * 
	         FROM   PM_Projects pp
	         INNER JOIN Sec_UserProjects AS us ON us.ProjectId=PP.ProjectId
			 left JOIN PM_ProjectEntity AS pe ON pe.Id=IsNull(PP.EntityId, 1)
             WHERE  us.UserId= @UserId and pp.ProjectId = @ProjectId

		    select distinct pmw.WorkGroupId 'WorkgroupId',pmw.Name 'WorkgroupName' from PM_WorkGroup pw
		    inner join PM_TaskResources pmtr on pmtr.ResourceId = @UserId --AND pmtr.TaskId=tsk.TaskId
	        inner join Sec_WorkGroup pmw on pmw.WorkGroupId = pmtr.GroupId and pmw.WorkGroupId = pw.WorkGroupId
		    where pw.ProjectId = @ProjectId

		select distinct tr.GroupId,su.*,su.UserName 'ResourceName',su.UserId 'ResourceId',pwg.ProjectId from Sec_Users su 
		inner join PM_TaskResources tr on tr.ResourceId = su.UserId
		inner join PM_WorkGroup pwg on pwg.WorkGroupId = tr.GroupId
		where pwg.ProjectId = 70030 and su.UserId=@UserId
		end
	END
 ELSE IF @Filter='getworklogs'
	BEGIN
			Select   l.*,usr.Picture, usr.[FirstName]+' '+usr.[LastName] as 'Name',pmp.ProjectName 'ProjectName'
		,pms.SiteName,pms.FACode,tsk.Title 'Task',pmtr.RatePerHour 'RatePerHour',pmw.Name 'WorkGroup'
		 from PM_WorkLog  l
		    inner join PM_Projects pmp on pmp.ProjectId = l.ProjectId
		    inner join PM_ProjectSites pms on pms.ProjectSiteId = l.ProjectSiteId AND pms.ProjectId=pmp.ProjectId 
			inner join PM_SiteTasks pmt on pmt.ProjectSiteId=pms.ProjectSiteId AND l.TaskId=pmt.SiteTaskId
			inner join PM_Tasks tsk on tsk.TaskId=pmt.TaskId
			inner join Sec_Users usr on usr.UserId = l.UserId 
			inner join PM_TaskResources pmtr on pmtr.ResourceId = usr.UserId AND pmtr.TaskId=tsk.TaskId
			inner join Sec_WorkGroup pmw on pmw.WorkGroupId = pmtr.GroupId
			Where l.ProjectId = @ProjectId and Charindex(cast(l.UserId as varchar(max))+'', @Users) > 0
			and Charindex(cast(pmw.WorkGroupId as varchar(max))+'', @WorkGroups) > 0
			and l.LogType =@LogType
			and l.LogDate BETWEEN CAST(@StartDate AS datetime)  AND CAST(@EndDate AS datetime)  
			and l.LogHours > 0 and pmtr.RatePerHour > 0 


	END
	ELSE IF @Filter='getsites'
	BEGIN
			select * from PM_ProjectSites where ProjectId = @ProjectId
	END
	ELSE IF @Filter='gettasks'
	BEGIN
	declare @IsUserManager bit = COALESCE((select IsManager from Sec_Users where UserId=@UserId),0)
		if(@IsUserManager=1)
			begin
			select distinct pmt.TaskId, pmt.* from  PM_SiteTasks  pmst
			inner join PM_Tasks  pmt on pmt.TaskId = pmst.TaskId
            where pmst.ProjectSiteId = @ProjectSiteId
			end
			else
			begin
			select distinct pmt.TaskId,pmt.* from  PM_SiteTasks  pmst
			inner join PM_Tasks pmt on pmt.TaskId = pmst.TaskId
			inner join PM_TaskResources ptr on ptr.ResourceId = @UserId  and ptr.TaskId = pmt.TaskId  
            where pmst.ProjectSiteId = @ProjectSiteId
			end
	END
	ELSE IF @Filter='getworkgroups'
	BEGIN
	declare @IsWorkGroupManager bit = COALESCE((select IsManager from Sec_Users where UserId=@UserId),0)
		if(@IsWorkGroupManager=1)
			begin
			select distinct swg.WorkGroupId ,swg.* from Sec_WorkGroup swg 
			inner join PM_WorkGroup pmw on pmw.WorkGroupId = swg.WorkGroupId
			--inner join PM_Tasks pmst on pmst.TaskId = 110217--@SiteTaskId
	        inner join PM_TaskResources  pmtr on pmtr.GroupId =  pmw.WorkGroupId and pmtr.TaskId = @SiteTaskId
			where pmtr.TaskId = @SiteTaskId 
			end
			else
			begin
			select distinct swg.WorkGroupId ,swg.* from Sec_WorkGroup swg 
			inner join PM_WorkGroup pmw on pmw.WorkGroupId = swg.WorkGroupId
			 inner join PM_TaskResources  pmtr on pmtr.GroupId =  pmw.WorkGroupId and pmtr.TaskId = @SiteTaskId
			where pmtr.TaskId = @SiteTaskId and pmtr.ResourceId = @UserId
			end
	END
		ELSE IF @Filter='getresources'
	BEGIN
	declare @IsResourceManager bit = COALESCE((select IsManager from Sec_Users where UserId=@UserId),0)
		if(@IsResourceManager=1)
			begin
			
	        select distinct ptr.ResourceId, ptr.*,su.UserName 'ResourceName' from PM_TaskResources ptr
			inner join PM_WorkGroup pwg on pwg.WorkGroupId = ptr.GroupId
			inner join Sec_Users su on su.UserId = ptr.ResourceId 
			inner join PM_Tasks pmt on pmt.TaskId = ptr.TaskId and pmt.ProjectId = ptr.ProjectId
			where pwg.WorkGroupId = @WorkgroupId  and pmt.TaskId = @SiteTaskId
			end
			else
			begin
			select distinct ptr.ResourceId, ptr.*,su.UserName 'ResourceName' from PM_TaskResources ptr
			inner join PM_WorkGroup pwg on pwg.WorkGroupId = ptr.GroupId
			inner join Sec_Users su on su.UserId = ptr.ResourceId 
			inner join PM_Tasks pmt on pmt.TaskId = ptr.TaskId and pmt.ProjectId = ptr.ProjectId
			where pwg.WorkGroupId = @WorkgroupId  and ptr.ResourceId = @UserId and pmt.TaskId = @SiteTaskId
			end
	END
	ELSE IF @Filter='advancesearch'
	BEGIN
	declare @IsSearchManager bit = COALESCE((select IsManager from Sec_Users where UserId=@UserId),0)
		if(@Users='0' and @IsSearchManager = 1)
		begin 
		set @Users = (SELECT
		  STUFF((SELECT distinct ',' + cast(ptr.ResourceId as nvarchar(max))
				 FROM pm_taskresources  ptr where ptr.ProjectId = @ProjectId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		else 
		begin
		set @Users = (SELECT
		  STUFF((SELECT distinct ',' + cast(ptr.ResourceId as nvarchar(max))
				 FROM pm_taskresources  ptr where ptr.ProjectId = @ProjectId
				 and ptr.ResourceId= @UserId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		if(@WorkGroups='0' and @IsSearchManager = 1)
		begin 
		set @WorkGroups = (SELECT
		  STUFF((SELECT distinct ',' + cast(pwg.WorkGroupId as nvarchar(max))
				 FROM PM_WorkGroup  pwg where pwg.ProjectId = @ProjectId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		else
		begin
		set @WorkGroups = (SELECT
		  STUFF((SELECT distinct ',' + cast(pwg.WorkGroupId as nvarchar(max))
				  FROM PM_WorkGroup  pwg
				 inner join  PM_TaskResources ptr on ptr.GroupId = pwg.WorkGroupId 
				  where pwg.ProjectId = @ProjectId and ptr.ResourceId = @UserId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		if(@SiteTaskId='0' and @IsSearchManager = 1)
		begin 
		set @SiteTaskId = (SELECT
		  STUFF((SELECT distinct ',' + cast(pt.TaskId as nvarchar(max))
				 FROM PM_Tasks  pt where pt.ProjectId = @ProjectId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		else
		begin
		set @SiteTaskId = (SELECT
		  STUFF((SELECT distinct ',' + cast(pt.TaskId as nvarchar(max))
				  FROM PM_Tasks  pt 
				  inner join  PM_TaskResources ptr on ptr.TaskId = pt.TaskId 
				  where pt.ProjectId = @ProjectId and ptr.ResourceId = @UserId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		if(@ProjectSiteId='0' )
		begin 
		set @ProjectSiteId = (SELECT
		  STUFF((SELECT distinct ',' + cast(ps.ProjectSiteId as nvarchar(max))
				 FROM pm_projectsites  ps where ps.ProjectId = @ProjectId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end


					Select   l.*,usr.Picture, usr.[FirstName]+' '+usr.[LastName] as 'Name',pmp.ProjectName 'ProjectName'
				,pms.SiteName,pms.FACode,tsk.Title 'Task',pmtr.RatePerHour 'RatePerHour',pmw.Name 'WorkGroup'
				 from PM_WorkLog  l
					inner join PM_Projects pmp on pmp.ProjectId = l.ProjectId
					inner join PM_ProjectSites pms on pms.ProjectSiteId = l.ProjectSiteId AND pms.ProjectId=pmp.ProjectId 
					inner join PM_SiteTasks pmt on pmt.ProjectSiteId=pms.ProjectSiteId AND l.TaskId=pmt.SiteTaskId
					inner join PM_Tasks tsk on tsk.TaskId=pmt.TaskId
					inner join Sec_Users usr on usr.UserId = l.UserId 
					inner join PM_TaskResources pmtr on pmtr.ResourceId = usr.UserId AND pmtr.TaskId=tsk.TaskId
					inner join Sec_WorkGroup pmw on pmw.WorkGroupId = pmtr.GroupId
					Where l.ProjectId = @ProjectId and Charindex(cast(l.UserId as varchar(max))+'', @Users) > 0
					and Charindex(cast(pmw.WorkGroupId as varchar(max))+'', @WorkGroups) > 0
					and l.LogType =@LogType
					and l.LogDate BETWEEN CAST(@StartDate AS datetime)  AND CAST(@EndDate AS datetime)  
					and Charindex(cast(tsk.TaskId as varchar(max))+'', @SiteTaskId) > 0
					and  Charindex(cast(pms.ProjectSiteId as varchar(max))+'',@ProjectSiteId) > 0
					and l.LogHours > 0 and pmtr.RatePerHour > 0 
	END
ELSE IF @Filter='advancesearch_summary_table'
	BEGIN
	declare @IsSearchSummaryManager bit = COALESCE((select IsManager from Sec_Users where UserId=@UserId),0)
		if(@Users='0' and @IsSearchSummaryManager = 1)
		begin 
		set @Users = (SELECT
		  STUFF((SELECT distinct ',' + cast(ptr.ResourceId as nvarchar(max))
				 FROM pm_taskresources  ptr where ptr.ProjectId = @ProjectId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		else 
		begin
		set @Users = (SELECT
		  STUFF((SELECT distinct ',' + cast(ptr.ResourceId as nvarchar(max))
				 FROM pm_taskresources  ptr where ptr.ProjectId = @ProjectId
				 and ptr.ResourceId= @UserId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		if(@WorkGroups='0' and @IsSearchSummaryManager = 1)
		begin 
		set @WorkGroups = (SELECT
		  STUFF((SELECT distinct ',' + cast(pwg.WorkGroupId as nvarchar(max))
				 FROM PM_WorkGroup  pwg where pwg.ProjectId = @ProjectId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		else
		begin
		set @WorkGroups = (SELECT
		  STUFF((SELECT distinct ',' + cast(pwg.WorkGroupId as nvarchar(max))
				  FROM PM_WorkGroup  pwg
				 inner join  PM_TaskResources ptr on ptr.GroupId = pwg.WorkGroupId 
				  where pwg.ProjectId = @ProjectId and ptr.ResourceId = @UserId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		if(@SiteTaskId='0' and @IsSearchSummaryManager = 1)
		begin 
		set @SiteTaskId = (SELECT
		  STUFF((SELECT distinct ',' + cast(pt.TaskId as nvarchar(max))
				 FROM PM_Tasks  pt where pt.ProjectId = @ProjectId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		else
		begin
		set @SiteTaskId = (SELECT
		  STUFF((SELECT distinct ',' + cast(pt.TaskId as nvarchar(max))
				  FROM PM_Tasks  pt 
				  inner join  PM_TaskResources ptr on ptr.TaskId = pt.TaskId 
				  where pt.ProjectId = @ProjectId and ptr.ResourceId = @UserId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end
		if(@ProjectSiteId='0' )
		begin 
		set @ProjectSiteId = (SELECT
		  STUFF((SELECT distinct ',' + cast(ps.ProjectSiteId as nvarchar(max))
				 FROM pm_projectsites  ps where ps.ProjectId = @ProjectId
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)')
				,1,1,''))
		end


		--select sum((GroupingBaseOnUserIdAndTaskID.rateperhours*GroupingBaseOnUserIdAndTaskID.loghours)) as 'Cost',
		--GroupingBaseOnUserIdAndTaskID.UserId,GroupingBaseOnUserIdAndTaskID.UserName 'Name',
		--sum(GroupingBaseOnUserIdAndTaskID.loghours) 'LogHours'
		--from (Select usr.UserName,usr.UserId,sum(pmtr.RatePerHour) as rateperhours,
		--sum(l.LogHours) as loghours,(sum(pmtr.RatePerHour) * sum(l.LogHours) ) 'Cost',pmtr.TaskId 
		--from PM_WorkLog  l
		--inner join PM_Projects pmp on pmp.ProjectId = l.ProjectId
		--inner join PM_ProjectSites pms on pms.ProjectSiteId = l.ProjectSiteId AND pms.ProjectId=pmp.ProjectId 
		--left join PM_SiteTasks pmt on pmt.ProjectSiteId=pms.ProjectSiteId AND l.TaskId=pmt.SiteTaskId
		--left join PM_Tasks tsk on tsk.TaskId=pmt.TaskId
		--left join Sec_Users usr on usr.UserId = l.UserId 
		--left join PM_TaskResources pmtr on pmtr.ResourceId = usr.UserId AND pmtr.TaskId=tsk.TaskId
		--left join Sec_WorkGroup pmw on pmw.WorkGroupId = pmtr.GroupId
		--Where l.ProjectId = @ProjectId and Charindex(cast(l.UserId as varchar(max))+'', @Users) > 0
		--and Charindex(cast(pmw.WorkGroupId as varchar(max))+'', @WorkGroups) > 0
		--and l.LogDate BETWEEN CAST(@StartDate AS datetime)  AND CAST(@EndDate AS datetime)
		--and Charindex(cast(tsk.TaskId as varchar(max))+'', @SiteTaskId) > 0
		--and  Charindex(cast(pms.ProjectSiteId as varchar(max))+'',@ProjectSiteId) > 0
		--group by usr.UserId,pmtr.TaskId,usr.UserName) as GroupingBaseOnUserIdAndTaskID   Group by UserId,UserName

		
		  
		 Select   l.*,usr.Picture, usr.[FirstName]+' '+usr.[LastName] as 'Name',pmp.ProjectName 'ProjectName'
		,pms.SiteName,pms.FACode,tsk.Title 'Task',pmtr.RatePerHour 'RatePerHour',pmw.Name 'WorkGroup'
		 from PM_WorkLog  l
		    inner join PM_Projects pmp on pmp.ProjectId = l.ProjectId
		    inner join PM_ProjectSites pms on pms.ProjectSiteId = l.ProjectSiteId AND pms.ProjectId=pmp.ProjectId 
			inner join PM_SiteTasks pmt on pmt.ProjectSiteId=pms.ProjectSiteId AND l.TaskId=pmt.SiteTaskId
			inner join PM_Tasks tsk on tsk.TaskId=pmt.TaskId
			inner join Sec_Users usr on usr.UserId = l.UserId 
			inner join PM_TaskResources pmtr on pmtr.ResourceId = usr.UserId AND pmtr.TaskId=tsk.TaskId
			inner join Sec_WorkGroup pmw on pmw.WorkGroupId = pmtr.GroupId
			Where l.ProjectId = @ProjectId and Charindex(cast(l.UserId as varchar(max))+'', @Users) > 0
			and Charindex(cast(pmw.WorkGroupId as varchar(max))+'', @WorkGroups) > 0
			and Charindex(cast(l.LogType as varchar(max))+'', 'Issue,Task') > 0
			and Charindex(cast(l.LogType as varchar(max))+'', 'Issue,Task') > 0
			and Charindex(cast(tsk.TaskId as varchar(max))+'', @SiteTaskId) > 0
		    and  Charindex(cast(pms.ProjectSiteId as varchar(max))+'',@ProjectSiteId) > 0
			and l.LogDate BETWEEN CAST(@StartDate AS datetime)  AND CAST(@EndDate AS datetime)  
			and l.LogHours > 0 and pmtr.RatePerHour > 0 
			
	END
	ELSE IF @Filter='getworklogs_summary_table'
	BEGIN
		--select sum((GroupingBaseOnUserIdAndTaskID.rateperhours*GroupingBaseOnUserIdAndTaskID.loghours)) as 'Cost',
		--GroupingBaseOnUserIdAndTaskID.UserId,GroupingBaseOnUserIdAndTaskID.UserName 'Name',sum(GroupingBaseOnUserIdAndTaskID.loghours) 'LogHours'
		--from (Select usr.UserName,usr.UserId,sum(pmtr.RatePerHour) as rateperhours,sum(l.LogHours) as loghours,(sum(pmtr.RatePerHour) * sum(l.LogHours) ) 'Cost',
		--pmtr.TaskId 
		--from PM_WorkLog  l
		--inner join PM_Projects pmp on pmp.ProjectId = l.ProjectId
		--inner join PM_ProjectSites pms on pms.ProjectSiteId = l.ProjectSiteId AND pms.ProjectId=pmp.ProjectId 
		--left join PM_SiteTasks pmt on pmt.ProjectSiteId=pms.ProjectSiteId AND l.TaskId=pmt.SiteTaskId
		--left join PM_Tasks tsk on tsk.TaskId=pmt.TaskId
		--left join Sec_Users usr on usr.UserId = l.UserId 
		--left join PM_TaskResources pmtr on pmtr.ResourceId = usr.UserId AND pmtr.TaskId=tsk.TaskId
		--left join Sec_WorkGroup pmw on pmw.WorkGroupId = pmtr.GroupId
		--Where l.ProjectId = @ProjectId and Charindex(cast(l.UserId as varchar(max))+'', @Users) > 0
		--and Charindex(cast(pmw.WorkGroupId as varchar(max))+'', @WorkGroups) > 0
		--and l.LogDate BETWEEN CAST(@StartDate AS datetime)  AND CAST(@EndDate AS datetime)
		--group by usr.UserId,pmtr.TaskId,usr.UserName) as GroupingBaseOnUserIdAndTaskID   Group by UserId,UserName

	      
		 Select   l.*,usr.Picture, usr.[FirstName]+' '+usr.[LastName] as 'Name',pmp.ProjectName 'ProjectName'
		,pms.SiteName,pms.FACode,tsk.Title 'Task',pmtr.RatePerHour 'RatePerHour',pmw.Name 'WorkGroup'
		 from PM_WorkLog  l
		    inner join PM_Projects pmp on pmp.ProjectId = l.ProjectId
		    inner join PM_ProjectSites pms on pms.ProjectSiteId = l.ProjectSiteId AND pms.ProjectId=pmp.ProjectId 
			inner join PM_SiteTasks pmt on pmt.ProjectSiteId=pms.ProjectSiteId AND l.TaskId=pmt.SiteTaskId
			inner join PM_Tasks tsk on tsk.TaskId=pmt.TaskId
			inner join Sec_Users usr on usr.UserId = l.UserId 
			inner join PM_TaskResources pmtr on pmtr.ResourceId = usr.UserId AND pmtr.TaskId=tsk.TaskId
			inner join Sec_WorkGroup pmw on pmw.WorkGroupId = pmtr.GroupId
			Where l.ProjectId = @ProjectId and Charindex(cast(l.UserId as varchar(max))+'', @Users) > 0
			and Charindex(cast(pmw.WorkGroupId as varchar(max))+'', @WorkGroups) > 0
			and Charindex(cast(l.LogType as varchar(max))+'', 'Issue,Task') > 0
			and l.LogDate BETWEEN CAST(@StartDate AS datetime)  AND CAST(@EndDate AS datetime)  
			and l.LogHours > 0 and pmtr.RatePerHour > 0 
			
	END
END