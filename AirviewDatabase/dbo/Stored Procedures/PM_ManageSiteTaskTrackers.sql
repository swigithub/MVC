-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PM_ManageSiteTaskTrackers
	-- Add the parameters for the stored procedure here
	@Filter NVARCHAR(50),
	@ProjectId NUMERIC(18,0)=0,
	@Data [dbo].[PM_ImportLists] READONLY	
AS
BEGIN
	DECLARE @TrackerGroupId AS NVARCHAR(50)=''
	DECLARE @SiteTaskId AS NVARCHAR(50)=''	--CommonId: is eNB in master site dump
	DECLARE @SiteId AS NVARCHAR(50)=''
	DECLARE @Actual AS NVARCHAR(50)=''
	DECLARE @CreatedByID AS NVARCHAR(50)=''
 IF @Filter='Insert_SiteTaskTrackers'
	BEGIN
		DECLARE db_sites CURSOR FOR  
		SELECT l.Value1,l.Value2,l.Value3,l.Value4
		FROM @Data AS l
		OPEN db_sites 
		FETCH NEXT FROM db_sites INTO 
		@TrackerGroupId,@SiteTaskId ,@SiteId, @Actual ,@CreatedByID
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			INSERT INTO [dbo].[PM_SiteTaskTracker]
           ([GroupTrackerId]
           ,[TaskTrackerId]
           ,[Actual]
           ,[CreatedById],[SiteId])
			SELECT @TrackerGroupId,@SiteTaskId,@SiteId, @Actual ,@CreatedByID		
		FETCH NEXT FROM db_sites INTO 
		@TrackerGroupId,@SiteTaskId,@SiteId , @Actual ,@CreatedByID
		END   
		CLOSE db_sites   
		DEALLOCATE db_sites
	END
END