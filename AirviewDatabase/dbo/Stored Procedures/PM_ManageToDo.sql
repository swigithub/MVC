CREATE PROCEDURE PM_ManageToDo
@Filter NVARCHAR(50),
@TodoId NUMERIC,
@Description NVARCHAR(500), 
@Type NVARCHAR(50)=null, 
@Status NVARCHAR(50), 
@CreatedOn DATE = null, 
@CreatedById NUMERIC = null,
@ToDoDateTime datetime,
@ToDoTitle nvarchar(50)=null,
@ProjectId nvarchar(255)=null,
@SiteId numeric(18,0)=null,
@TaskId numeric(18,0)=null,
@AssignedToIds nvarchar(50)=null
AS
BEGIN
	IF @Filter='Insert_Todo'
	BEGIN
	 INSERT INTO PM_ToDo ([Description], [Type], [Status], CreatedOn, CreatedById, [ToDoDateTime], ToDoTitle, ProjectId, SiteId, TaskId,AssignedToIds)
	 VALUES(@Description,@Type,@Status,@CreatedOn,@CreatedById, @ToDoDateTime, @ToDoTitle, @ProjectId, @SiteId, @TaskId,@AssignedToIds)
	END

	IF @Filter='Update_Todo'
	BEGIN
	 Update PM_ToDo Set [Status]=@Status where TodoId= @TodoId  
	END

	IF @Filter='Edit_Todo'
	BEGIN
	 Update PM_ToDo Set Description = @Description,AssignedToIds=@AssignedToIds , ToDoTitle = @ToDoTitle, [Status] = @Status, Type = @Type, [ToDoDateTime] = @ToDoDateTime,SiteId=@SiteId,TaskId=@TaskId
	 where TodoId= @TodoId 
	END
END