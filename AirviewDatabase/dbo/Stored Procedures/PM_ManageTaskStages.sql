create PROCEDURE PM_ManageTaskStages
@TaskStages dbo.TaskStages READONLY,
@Filter nvarchar(255) = null,
@StageId int = 0,
@Title nvarchar(255) = null,
@SortOrder int = null,
@Description nvarchar(MAX) = null,
@ProjectId numeric(18, 0) = 0,
@ProjectIdTemp numeric(18, 0) = 0,
@ResultValue int = 0,
@TaskId numeric(18, 0) = 0,
@TaskTemp numeric(18, 0) = 0,
@IsDeleted bit = 0
AS
BEGIN
	SET NOCOUNT ON;
	IF @Filter = 'UpdateOrAdd'
	BEGIN
		DECLARE TaskStages_Cursor CURSOR FOR
		Select StageId, Title, [Description], SortOrder, ProjectId, IsDeleted, TaskId from @TaskStages
		OPEN TaskStages_Cursor;
		FETCH NEXT FROM TaskStages_Cursor INTO @StageId, @Title, @Description, @SortOrder, @ProjectIdTemp, @IsDeleted, @TaskTemp
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @StageId != 0
			BEGIN
				UPDATE dbo.PM_TaskStages SET Title = @Title, [Description] = @Description, SortOrder = @SortOrder, IsDeleted = @IsDeleted, TaskId = NULL WHERE StageId = @StageId;
				SET @ResultValue = @ResultValue + 1
			END
			ELSE 
			BEGIN
				INSERT INTO dbo.PM_TaskStages (Title, [Description], SortOrder, ProjectId, IsDeleted, TaskId) VALUES (@Title, @Description, @SortOrder, @ProjectId, @IsDeleted, NULL)
				SET @ResultValue = @ResultValue + 1
			END
			FETCH NEXT FROM TaskStages_Cursor INTO @StageId, @Title, @Description, @SortOrder, @ProjectIdTemp, @IsDeleted, @TaskTemp
		END
		CLOSE TaskStages_Cursor;
		DEALLOCATE TaskStages_Cursor;
		RETURN  @ResultValue
	END
	ELSE IF @Filter = 'UpdateOrAddForTaskWorkFlow'
	BEGIN
		DECLARE TaskStages_Cursor CURSOR FOR
		Select StageId, Title, [Description], SortOrder, ProjectId, IsDeleted, TaskId from @TaskStages
		OPEN TaskStages_Cursor;
		FETCH NEXT FROM TaskStages_Cursor INTO @StageId, @Title, @Description, @SortOrder, @ProjectIdTemp, @IsDeleted, @TaskTemp
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @StageId != 0 AND @TaskTemp != 0
			BEGIN
				BEGIN
					UPDATE dbo.PM_TaskStages SET Title = @Title, [Description] = @Description, SortOrder = @SortOrder, IsDeleted = @IsDeleted, TaskId = @TaskId WHERE StageId = @StageId;
					SET @ResultValue = @ResultValue + 1
				END
			END
			ELSE 
			BEGIN
				INSERT INTO dbo.PM_TaskStages (Title, [Description], SortOrder, ProjectId, IsDeleted, TaskId) VALUES (@Title, @Description, @SortOrder, @ProjectId, @IsDeleted, @TaskId)
				SET @ResultValue = @ResultValue + 1
			END
			FETCH NEXT FROM TaskStages_Cursor INTO @StageId, @Title, @Description, @SortOrder, @ProjectIdTemp, @IsDeleted, @TaskTemp
		END
		CLOSE TaskStages_Cursor;
		DEALLOCATE TaskStages_Cursor;
		RETURN  @ResultValue
	END
END