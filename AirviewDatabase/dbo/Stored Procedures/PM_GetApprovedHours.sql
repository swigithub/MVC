CREATE  PROCEDURE [dbo].[PM_GetApprovedHours]
 @Filter NVARCHAR(50),
 @StartDate NVARCHAR(50)=NULL,
 @EndDate NVARCHAR(50)=NULL
AS
BEGIN
--	 [dbo].[PM_GetWorkLogs] 'WorkLogs','', ''
	IF @Filter='WorkLogsCost'
	BEGIN
		Select
		[WLogId]
       ,[LogType]
       ,[LogDate]
       ,[LogHours]
       ,[IsApproved]
       ,[ApprovalDate]
		
		FROM PM_WorkLog  		
		Where IsApproved=1 

	END
	
END