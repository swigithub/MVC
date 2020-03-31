CREATE PROCEDURE PM_ManageProjectPlan
	@Filter NVARCHAR(50),
	@ProjectId AS NUMERIC(18,0),
	@Plan as [dbo].[List] READONLY	
AS
BEGIN
	DECLARE @ProjectSiteId AS NVARCHAR(50)=''
	DECLARE @xProjectId AS NVARCHAR(50)=''
	DECLARE @SiteTaskId AS NVARCHAR(50)=''
	DECLARE @PlanDate AS NVARCHAR(50)=''
	DECLARE @EstimatedStartDate AS NVARCHAR(50)=''
	DECLARE @EstimatedEndDate AS NVARCHAR(50)=''
	DECLARE @TargetDate AS NVARCHAR(50)=''
	DECLARE @ActualStartDate AS NVARCHAR(50)=''
	DECLARE @ActualEndDate AS NVARCHAR(50)=''
	DECLARE @Status AS NVARCHAR(50)=''
	DECLARE @Resources AS NVARCHAR(50)=''
	
	IF @Filter='Bulk_Project_Plan'
	BEGIN
		--"Value1", item.ProjectSiteId, "Value2", item.ProjectId, "Value3", item.PlannedDate, "Value4", item.EstimatedStartDate, "Value5", item.TargetDate, 
        --"Value6", item.ActualEndDate, "Value7", item.StatusId, "Value8", userIds,"Value9", TaskId
        DECLARE db_cluster2 CURSOR FOR  
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11
		FROM @Plan AS l
		OPEN db_cluster2 
		FETCH NEXT FROM db_cluster2 INTO @ProjectSiteId, @xProjectId, @PlanDate, @EstimatedStartDate, @TargetDate, @ActualEndDate , @Status, @Resources, @SiteTaskId,@EstimatedEndDate,@ActualStartDate
		WHILE @@FETCH_STATUS = 0   
		BEGIN		
			UPDATE PM_SiteTasks 
			SET  
				PlannedDate = @PlanDate,
				EstimatedEndDate = @EstimatedEndDate,
				EstimatedStartDate = @EstimatedStartDate,
				TargetDate = @TargetDate,
				ActualEndDate = @ActualEndDate,
				ActualStartDate = @ActualStartDate,
				StatusId = @Status,
				ResourcesId = @Resources
			FROM @Plan p
			WHERE projectSiteID = @ProjectSiteId AND SiteTaskId= @SiteTaskId				
		FETCH NEXT FROM db_cluster2 INTO @ProjectSiteId, @xProjectId, @PlanDate, @EstimatedStartDate, @TargetDate, @ActualEndDate , @Status, @Resources, @SiteTaskId,@EstimatedEndDate,@ActualStartDate
		END   
		CLOSE db_cluster2   
		DEALLOCATE db_cluster2
		
		
	END
END