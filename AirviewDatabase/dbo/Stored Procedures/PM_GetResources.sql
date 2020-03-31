CREATE PROCEDURE [dbo].[PM_GetResources]
	@Filter AS NVARCHAR(50),
	@ProjectId AS NUMERIC(18,0),
	@TaskId	AS NUMERIC(18,0)
	AS
BEGIN
	IF @Filter='GET_PROJECT_RESOURCES'
	BEGIN		
		
		if(@TaskId is not null and @TaskId!=0)
		BEGIN
			Select Sec_Users.UserId, ISNULL(Sec_Users.FirstName,'')+' '+ISNULL(Sec_Users.LastName,'') 'Username' from PM_TaskResources
			Inner join Sec_Users
			On Sec_Users.UserId = PM_TaskResources.ResourceId

			Inner join Sec_UserProjects
			on Sec_Users.UserId = Sec_UserProjects.UserId
			where Sec_UserProjects.ProjectId = @ProjectId and PM_TaskResources.TaskId = @TaskId
		END
		ELSE
		BEGIN
			SELECT su.UserId, ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') 'Username'
			FROM Sec_Users AS su
			Inner join Sec_UserProjects
			on su.UserId = Sec_UserProjects.UserId
			where Sec_UserProjects.ProjectId =@ProjectId
		END
		

		--SELECT su.UserId, ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') 'Username'
		--FROM Sec_Users AS su
		--Inner join Sec_UserProjects
		--on su.UserId = Sec_UserProjects.UserId
		--where Sec_UserProjects.ProjectId =@ProjectId

		--SELECT su.UserId, ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') 'Username'
		--FROM Sec_Users AS su
		--WHERE su.IsActive=1
	END
	-- [PM_GetResources] GET_PROJECT_TASK,10007,0
	ELSE IF @Filter='GET_PROJECT_TASK'
	BEGIN		
		select task.TaskId,task.Title 'Task' 
		from PM_Tasks as task
		where task.ProjectId = @ProjectId and
		task.TaskTypeId = @TaskId
	END
	ELSE IF @Filter='GET_PROJECT_ISSUES_USERS'
	BEGIN		
     SELECT su.UserId, ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') +' | '+ISNULL(Designation,'') +' | '+ISNULL(r.Name,'') as 'Username'
		FROM Sec_Users AS su
			Inner Join Sec_UserRoles ur on ur.UserId = su.UserId
			inner join Sec_Roles r on r.RoleId=ur.RoleId
		Inner join Sec_UserProjects
		on su.UserId = Sec_UserProjects.UserId
		where Sec_UserProjects.ProjectId = @ProjectId

	END

END