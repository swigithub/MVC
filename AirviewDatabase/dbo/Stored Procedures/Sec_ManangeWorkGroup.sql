-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE Sec_ManangeWorkGroup 
	-- Add the parameters for the stored procedure here
	@Filter varchar(50) = '',
	@WorkgroupId int = 0,
	@WorkgroupName varchar(50) = '',
	@UserId int = 0 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @Filter = 'InsertWorkGroup'
	BEGIN
	Insert into Sec_WorkGroup(Name, IsDeleted,CreatedBy) Values (@WorkgroupName,0,@UserId)
	END
	Else
	if @Filter = 'UpdateWorkGroup'
	BEGIN
	Update Sec_WorkGroup Set Name = @WorkgroupName ,
	 ModifiedBy = @UserId,
	 ModifiedOn = GETDATE()
	where WorkGroupId = @WorkgroupId
	END
	Else
	if @Filter = 'GetGroup'
	BEGIN
	Select WorkGroupId as WorkgroupId, Name as WorkgroupName from Sec_WorkGroup where IsDeleted = 0
	END
	Else
	if @Filter = 'DeleteGroup'
	BEGIN
	--Update PM_WorkGroup SET IsDeleted = 1 where PMWGId = @WorkgroupId
		Update PM_TaskResources
		SET
		IsDeleted = 1
		Where GroupId = @WorkgroupId
		Update Sec_WorkGroup SET IsDeleted = 1 where WorkGroupId = @WorkgroupId
	END
	if @Filter = 'CheckDelete'
	Begin
	SELECT ISNULL(
( select  tr.GroupId  from PM_TaskResources tr where GroupId =@WorkgroupId and IsDeleted !=1
), 0) as 'WorkgroupId','' as 'WorkgroupName'
	  	--Declare @data int = 0   CASE WHEN Isnull(tr.GroupId,0) =1  THEN tr.GroupId ELSE 0 END
	--SET @data = (select distinct PM_TaskResources.ResourceId from PM_TaskResources
	--Inner join PM_SiteTasks
	--On PM_TaskResources.TaskId = PM_SiteTasks.TaskId
	--Inner join PM_SiteResources
	--On PM_SiteResources.SiteTaskId = PM_SiteTasks.SiteTaskId
	--where PM_TaskResources.GroupId = @WorkgroupId and Charindex(cast( PM_TaskResources.ResourceId  as varchar(max))+',', PM_SiteResources.AssignToId+',')>0)

	--IF (@data is not null)
	--	Begin 
	--	Select @data as WorkgroupId
	--	END
	--ELSE
	--	Begin 
	--	Select null as WorkgroupId
	--	--Update PM_TaskResources
	--	--SET
	--	--IsDeleted = 1
	--	--Where GroupId = @WorkgroupId
	--	--Update PM_WorkGroup SET IsDeleted = 1 where PMWGId = @WorkgroupId
	--	END
	END
END