CREATE PROCEDURE PM_ManageIssues
	@Filter NVARCHAR(50),
	@ProjectId NUMERIC=NULL,	
	@IssueId NUMERIC=NULL,	
	@TaskId NUMERIC=NULL,
	@ProjectSiteId NUMERIC=NULL,
	@SeverityId NUMERIC=NULL,
	@TaskTypeId NUMERIC=NULL,
	@StatusId NUMERIC=NULL,
	@IssueCategoryId NUMERIC=NULL,
	@UserId NUMERIC=NULL,
	@IssuePriorityId NUMERIC=NULL,
	@IssueStatusId NUMERIC=NULL,
	@Description NVARCHAR(250)=NULL,
	@TicketTypeId NUMERIC=NULL,
	@RequestedById NUMERIC=NULL,
	@RequestDate DATE=NULL,
	@AssignedToId NVARCHAR(250)=NULL,
	@ReasonId NUMERIC=NULL,
	@IssueById NUMERIC=NULL,
	@ForecastDate DATE=NULL,
	@RequestedByDate DATE=NULL,
	@CreatedOn DATE=NULL,
	@TargetDate DATE=NULL,
	@ActualStartDate DATE =null,
	@ActualEndDate DATE = null,
	@Tags NVARCHAR(50)=NULL,
	@RequestedBy NVARCHAR(50)=NULL,
	@IsUnavoidable BIT=0,
	@ExtendedeNB NVARCHAR(250)=NULL,
	@AlarmId NUMERIC=NULL,
	@EquipmentId NVARCHAR(250)=NULL,
	@AOTSCR NVARCHAR(250)=NULL,
	@FilePath NVARCHAR(500)=NULL,
	@ENB NVARCHAR(250)=NULL,
	@ItemTypeId NUMERIC=NULL,
	@ActivityTypeId NUMERIC=NULL,
	@MSWindowId numeric=NULL


	---add all other params
AS

IF @Filter='INSERT_ISSUE'
BEGIN
	SET @IssueStatusId=(SELECT ad.DefinationId FROM AD_Definations AS ad INNER JOIN AD_DefinationTypes AS adt ON ad.DefinationTypeId=adt.DefinationTypeId AND adt.DefinationType='Issue Status' AND ad.DefinationName='Open' AND ad.IsActive=1)
--	SELECT @ProjectId,@ProjectSiteId, @TaskTypeId, @TaskId, @IssuePriorityId,@IssueStatusId, @Description, @RequestedById, @RequestDate,@AssignedToId,@ForecastDate,@TargetDate,@ActualStartDate,@ActualEndDate
	INSERT INTO PM_Issues(ProjectId,ProjectSiteId,TaskTypeId,TaskId,IssuePriorityId,IssueStatusId,IssueCategoryId,ReasonId,IssueById,[Description],RequestedById,RequestDate,AssignedToId,
	ForecastDate,TargetDate,ActualStartDate,ActualEndDate,IsUnavoidable,ENB,ExtendedeNB,EquipmentId,AOTSCR,ActivityTypeId,ItemTypeId,ItemFilePath,SeverityId,AlarmId,MSWindowId,RequestedBy,StatusId, TicketTypeId)
	VALUES(@ProjectId,@ProjectSiteId, @TaskTypeId, @TaskId, @IssuePriorityId,@IssueStatusId,@IssueCategoryId,@ReasonId,@IssueById,@Description, @RequestedById, @RequestDate,@AssignedToId,
	@ForecastDate,@TargetDate,@ActualStartDate,@ActualEndDate,@IsUnavoidable,@ENB,@ExtendedeNB,@EquipmentId,@AOTSCR,@ActivityTypeId,@ItemTypeId,@FilePath,@SeverityId,@AlarmId,@MSWindowId,@RequestedBy,@IssueStatusId, @TicketTypeId)	
	
	SELECT @IssueId=@@IDENTITY
	
	INSERT INTO PM_IssuesLog(IssueId,StatusId,UserId,Description,CreatedOn,PriorityId,SeverityId,AssignToId)
	VALUES(@IssueId,@IssueStatusId,@UserId,@Description,GETDATE(),@IssuePriorityId,@SeverityId,@AssignedToId)
END
ELSE IF @Filter='UPDATE_ISSUE'
BEGIN
	UPDATE PM_Issues SET  ProjectId=@ProjectId, ProjectSiteId=@ProjectSiteId, TaskTypeId=@TaskTypeId, TaskId=@TaskId, IssuePriorityId=@IssuePriorityId, IssueStatusId=@IssueStatusId,IssueCategoryId=@IssueCategoryId,ReasonId=@ReasonId,IssueById=@IssueById ,[Description]=@Description,
	RequestedById=@RequestedById, RequestedBy = @RequestedBy, RequestDate=@RequestDate, AssignedToId=@AssignedToId, ForecastDate=@TargetDate, TargetDate=@TargetDate, ActualStartDate=@ActualStartDate, ActualEndDate=@ActualEndDate, Tags=@Tags, IsUnavoidable=@IsUnavoidable,
	ENB=@ENB,ExtendedeNB=@ExtendedeNB,EquipmentId=@EquipmentId,AOTSCR=@AOTSCR,ActivityTypeId=@ActivityTypeId,ItemTypeId=@ItemTypeId,ItemFilePath=@FilePath,AlarmId=@AlarmId,MSWindowId=@MSWindowId,
	TicketTypeId = @TicketTypeId
	WHERE IssueId = @IssueId
	
	--INSERT INTO PM_IssuesLog(IssueId,StatusId,UserId,Description,CreatedOn,PriorityId,SeverityId)
	--VALUES(@IssueId,@StatusId,@UserId,@Description,GETDATE(),@IssuePriorityId,@SeverityId)
END
ELSE IF @Filter='Insert_IssueLog_Status'
BEGIN	
	INSERT INTO PM_IssuesLog(IssueId,StatusId,UserId,Description,CreatedOn)
	VALUES(@IssueId,@StatusId,@UserId,@Description,@CreatedOn)
	
	UPDATE PM_Issues
	SET IssueStatusId = @StatusId, StatusId =  @StatusId --CASE WHEN @StatusId=163355 THEN 163387 WHEN @StatusId=163357 THEN 163438 END 
	WHERE IssueId=@IssueId
END