-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PM_TaskResource] 
	-- Add the parameters for the stored procedure here
	@Filter varchar(50) = '',
	@ResourceId int = 0,
	@GroupId int = 0,
	@RACIId int = 0,
	@ProjectId int = 0,
	@TaskId int = 0,
	@PMTRId int = 0,
	@RatePerHour int = 0
	,@IsDeleted bit = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @Filter = 'InsertResources'
	BEGIN
	Declare @Pcheck int
	set @Pcheck = (select Count(*) from PM_WorkGroup where WorkGroupId = @GroupId and ProjectId=@ProjectId)
	if @Pcheck > 0
	begin
	Declare @count int
	Insert into PM_TaskResources
	(ResourceId, GroupId, RACIId, ProjectId, TaskId, RatePerHour)
	Values
	(@ResourceId,@GroupId,@RACIId,@ProjectId,@TaskId, @RatePerHour)

	 set @count= (select count(*) from Sec_UserProjects where UserId=@ResourceId and ProjectId = @ProjectId)
	 if @count = 0
	 begin
	 Insert into Sec_UserProjects
	 (UserId,ProjectId)
	 values
	 (@ResourceId,@ProjectId)
	 End
	 end
	END
	ELSE
	if @Filter = 'GetResources'
	BEGIN
	declare @check int
	set @check=(select COUNT(*) from PM_WorkGroup where ProjectId = @ProjectId)
	if @check >0 
	begin 
	Select * from PM_TaskResources tr
	inner join PM_WorkGroup  wg on wg.WorkGroupId =tr.GroupId and wg.ProjectId = tr.ProjectId
	Where tr.ProjectId = @ProjectId
	 and
	  TaskId = @TaskId and IsDeleted = 0

	end
	END
	ELSE
	if @Filter = 'UpdateResources'
	BEGIN
	Update PM_TaskResources
	SET
	ResourceId = @ResourceId,
	GroupId = @GroupId,
	RACIId = @RACIId,
	RatePerHour = @RatePerHour,
	IsDeleted = @IsDeleted
	Where PMTRId = @PMTRId
	END

	ELSE
	if @Filter = 'DeleteResources'
	BEGIN
	Update PM_TaskResources
	SET
	IsDeleted = 1
	Where PMTRId = @PMTRId
	END

	ELSE
	if @Filter = 'PopulateUserResources'
	BEGIN	 
	Select
	Sec_Users.UserId as ResourceId,
	Sec_Users.FirstName +' '+ Sec_Users.LastName+' | '+Designation +' | '+r.Name  as ResourceName
	from Sec_Users
	Inner Join Sec_UserRoles ur on ur.UserId = Sec_Users.UserId
			inner join Sec_Roles r on r.RoleId=ur.RoleId
	Inner join [PM_ProjectResources]
	on Sec_Users.UserId = PM_ProjectResources.AssignToId
	where PM_ProjectResources.ProjectId = @ProjectId  and PM_ProjectResources.TaskId = @TaskId 
	END

	ELSE
	if @Filter = 'PopulateUserResourcesProject'
	BEGIN
	
		Select u.UserId as ResourceId, ISNULL(u.FirstName,'')+' '+ISNULL(u.LastName,'') +' | '+ISNULL(u.Designation,'') +' | '+ISNULL(r.Name,'') as  ResourceName  from Sec_Users u
		Inner Join Sec_UserRoles ur on ur.UserId = u.UserId
			inner join Sec_Roles r on r.RoleId=ur.RoleId
		Where u.IsActive = 1 
	--Select
	--Sec_Users.UserId as ResourceId,
	--Sec_Users.FirstName +' '+ Sec_Users.LastName as ResourceName
	--from Sec_Users
	--Inner join Sec_UserProjects
	--on Sec_Users.UserId = Sec_UserProjects.UserId
	--where Sec_UserProjects.ProjectId = @ProjectId 
	END

	ELSE
	if @Filter = 'PopulateGroupResources'
	BEGIN
	Select
	swg.WorkGroupId as 'GroupId',
	swg.Name as 'GroupName'
	from Sec_WorkGroup swg
	inner join PM_WorkGroup as pwg on swg.WorkGroupId = pwg.WorkGroupId 
	where pwg.ProjectId=@ProjectId and IsDeleted = 0

	select swg.WorkGroupId as 'GroupId',
	swg.Name as 'GroupName' from PM_WorkGroup pwg
	inner join Sec_WorkGroup swg on swg.WorkGroupId =  pwg.WorkGroupId 
	where pwg.ProjectId=@ProjectId 
	END
END