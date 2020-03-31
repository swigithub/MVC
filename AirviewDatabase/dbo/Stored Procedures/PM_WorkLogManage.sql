CREATE PROCEDURE PM_WorkLogManage 
	@Filter AS NVARCHAR(50),
	@WLogId AS NUMERIC(18,0)=NULL,
	@ProjectId AS NUMERIC(18,0)=NULL,
	@ProjectSiteId AS NUMERIC(18,0)=NULL,
	@TaskId AS NUMERIC(18,0)=NULL,
	@LogType AS NVARCHAR(15)=NULL,
	@UserId AS NUMERIC(18,0)=NULL,
	@IssueId AS NUMERIC(18,0)=NULL,
	@LogDate date=NULL,
	@LogHours  AS FLOAT=NULL,
	@Description NVARCHAR(300)=NULL,
    @IsApproved  bit=0,
	@RatePerUnit  float=NULL,
	@IsAttended bit=0,
	@Comment NVARCHAR(300)='',
	@Data List READONLY
	AS
BEGIN
IF @Filter='INSERT_WorkLog'
BEGIN
	--SELECT @ProjectId, @ProjectSiteId, @TaskId,@LogType, @UserId, @LogDate, @LogHours
	select top(1) @RatePerUnit = RatePerHour from PM_TaskResources WHERE TaskId = @TaskId AND ProjectId = @ProjectId
	INSERT INTO PM_WorkLog(ProjectId,ProjectSiteId,TaskId,LogType,UserId,LogDate,LogHours, RatePerUnit,[Description])
	VALUES (@ProjectId, @ProjectSiteId, @TaskId,@LogType, @UserId, @LogDate, @LogHours,@RatePerUnit, @Description)
END

Else IF @Filter='ApproveWorkLog'
BEGIN
	Update PM_WorkLog Set

		IsApproved=@IsApproved,
		ApprovedById= @UserId,
		ApprovalDate= @LogDate
	Where WLogId= @WLogId
END
Else IF @Filter='EditWorkLog'
BEGIN
	Update PM_WorkLog Set
		LogHours=@LogHours,
		LogDate=@LogDate,
		[Description]=@Description
	Where WLogId= @WLogId
END

if (@Filter='approveworklogs')
begin

DECLARE approvelogs CURSOR READ_ONLY
FOR
SELECT x.Value1,x.Value2,x.Value3,x.Value4,x.Value5,x.Value6
FROM @Data x
--OPEN CURSOR
OPEN approvelogs 
--FETCH THE RECORD INTO THE VARIABLES.
FETCH NEXT FROM approvelogs INTO @WLogId,@IsAttended,@LogDate,@UserId,@IsApproved,@Comment
--LOOP UNTIL RECORDS ARE AVAILABLE.
WHILE @@FETCH_STATUS = 0
BEGIN
	--BEGIN TRANSACTION	
	--print(@WLogId)
	--print(@IsAttended)
	--print(@LogDate)
	--print(@UserId)
	--print(@IsApproved)
	--print(@Comment)
	--print('-------')
		Update PM_WorkLog Set

		IsApproved=@IsApproved,
		ApprovedById= @UserId,
		ApprovalDate= @LogDate,
		IsAttended =  @IsAttended,
		Comment = @Comment
	    Where WLogId= @WLogId
		
	FETCH NEXT FROM approvelogs INTO @WLogId,@IsAttended,@LogDate,@UserId,@IsApproved,@Comment
END 
--CLOSE THE CURSOR.
CLOSE approvelogs
DEALLOCATE approvelogs


end
END