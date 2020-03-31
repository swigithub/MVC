create PROCEDURE PM_GetTaskStages
@Filter nvarchar(255) = null,
@ProjectId numeric(18, 0) = null,
@TaskId numeric(18, 0) = null,
@StageId int = -1
AS
BEGIN
	SET NOCOUNT ON;
	IF @Filter = 'GetByProjectId'
	BEGIN
		SELECT * FROM dbo.PM_TaskStages ts 
		WHERE ts.ProjectId = @ProjectId AND TaskId IS NULL AND IsDeleted = 0
		ORDER BY ts.SortOrder ASC
	END
	ELSE IF @Filter = 'IsStageUsed'
	BEGIN
		SELECT COUNT(*) FROM PM_SiteTasks s
		WHERE s.TaskStageId = @StageId
	END
	ELSE IF @Filter = 'GetStagesForTaskOrProject'
	BEGIN
		;
	
		IF (SELECT COUNT(*) FROM dbo.PM_TaskStages ts WHERE ts.ProjectId = @ProjectId AND ts.TaskId = @TaskId AND IsDeleted = 0) > 0
		BEGIN
			SELECT * FROM dbo.PM_TaskStages ts 
			WHERE ts.ProjectId = @ProjectId AND ts.TaskId = @TaskId AND IsDeleted = 0 ORDER BY ts.SortOrder ASC
		END
		ELSE
		BEGIN
			SELECT * FROM dbo.PM_TaskStages ts 
			WHERE (ts.ProjectId = @ProjectId AND TaskId IS NULL AND IsDeleted = 0) ORDER BY ts.SortOrder ASC
		END
	END
END