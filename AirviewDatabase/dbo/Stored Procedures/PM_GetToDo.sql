CREATE procedure PM_GetToDo
	@Filter as nvarchar(50),
	@ProjectId as numeric(18,0)=0,
	@UserId as numeric(18,0),
	@WhereClause  nvarchar(Max) = ''
AS
	IF @Filter='Get_Todo'
	BEGIN
		DECLARE @strSQL AS NVARCHAR(MAX)=''
		SELECT @strSQL =  CONCAT('
SELECT td.TodoId,td.AssignedToIds, td.Type, td.Status, td.Description, td.CreatedOn, td.ToDoDateTime, td.ToDoTitle, ps.SiteName, ts.Title as [TaskName], ps.ProjectSiteId as [SiteId], ts.TaskId FROM PM_ToDo td left join PM_ProjectSites ps on td.SiteId = ps.ProjectSiteId left join PM_Tasks ts on ts.TaskId = td.TaskId where  td.ProjectId=',@ProjectId, @WhereClause)
		EXEC(@strSQL);
	END
	else if @Filter='Get_Todo_Filters'
	begin
		select d.DefinationId as 'StatusId',d.DisplayText as 'StatusName' 
		From Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='Project Task Status' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)

	select st.TaskId as 'EntityTaskId',t.Title as 'EntityTaskName',st.ProjectSiteId as 'EntityId' from PM_SiteTasks st
	inner join PM_Tasks t on t.TaskId=st.TaskId
	where t.ProjectId =@ProjectId
	
	select ps.ProjectSiteId as 'EntityId',ps.SiteName as 'EntityName',ps.SiteCode as 'EntityCode' from PM_ProjectSites ps
	where ps.ProjectId =@ProjectId 
	   
	

	select u.FirstName +' '+u.LastName+' | '+u.Designation +' | '+r.Name  as 'Title',u.UserId,up.ProjectId,u.FirstName +' '+u.LastName+  'Plural' from Sec_Users u
	inner join Sec_UserProjects up on up.UserId=u.UserId
	Inner Join Sec_UserRoles ur on ur.UserId = u.UserId
	inner join Sec_Roles r on r.RoleId=ur.RoleId
	where up.ProjectId=@ProjectId
	select d.DefinationId as 'TypeId',d.DisplayText as 'TypeName' 
		From Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='Project Event Type' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
	end