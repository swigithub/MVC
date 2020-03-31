-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PM_ManageTrackers
	-- Add the parameters for the stored procedure here
	@Filter NVARCHAR(50),
	@ProjectId NUMERIC(18,0)=0,
	@Data [dbo].[PM_ImportLists] READONLY	
AS
BEGIN
	DECLARE @Title AS NVARCHAR(50)=''
	DECLARE @Unit AS NVARCHAR(50)=''	--CommonId: is eNB in master site dump
	DECLARE @Budget AS NVARCHAR(50)=''
	DECLARE @Actual AS NVARCHAR(50)=''
	DECLARE @IsDeleted AS NVARCHAR(50)=''
	DECLARE @TrackerGroup AS NVARCHAR(50)=''
	DECLARE @TrackerId AS NVARCHAR(50)=''
	DECLARE @SiteTaskTrackerId AS NVARCHAR(50)=''
	DECLARE @TrackerGroupId AS Numeric (20,0)
	DECLARE @Count AS Numeric (20,0)
	DECLARE @TaskId AS NVARCHAR(50)=''
    DECLARE @SiteId AS NVARCHAR(50)=''
 IF @Filter='Insert_Trackers'
	BEGIN

		DECLARE db_sites CURSOR FOR  
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8
		FROM @Data AS l
		OPEN db_sites 
		FETCH NEXT FROM db_sites INTO 
		@TrackerGroup,@Title, @Unit ,@Budget,@Actual,@TrackerId,@IsDeleted,@TaskId
		WHILE @@FETCH_STATUS = 0   
		BEGIN
		     set @TrackerGroupId=0
	        Set @TrackerGroupId= (select Top 1 TrackerGroupId from PM_TrackerGroup where Title =@TrackerGroup and TaskId =CONVERT(numeric(20,0),@TaskId))
	   --set @Count =(select Top 1 TrackerGroupId from PM_TrackerGroup where Title =@TrackerGroup)
	           if @IsDeleted='True'
	           Begin
	               delete tt  from PM_TaskTracker as tt Where tt.TrackerId=CONVERT(numeric(20,0),@TrackerId) 
	            end
	   	else
		begin
		            if @TrackerId <> '0'
	    begin
			update PM_TaskTracker 
			Set Title= @Title ,
			 Budget = @Budget ,
			 Actual = @Actual,
			Unit = @Unit,
			 TrackerGroupId = @TrackerGroupId
			where TrackerId=convert(numeric(20,0),@TrackerId)
		end
		else
		begin
			INSERT INTO PM_TaskTracker ([Title],[Budget],[Actual],[Unit],[TrackerGroupId])
			SELECT @Title,@Budget,@Actual,@Unit, @TrackerGroupId
			End
			END
		FETCH NEXT FROM db_sites INTO 
@TrackerGroup,@Title, @Unit ,@Budget,@Actual,@TrackerId,@IsDeleted,@TaskId
		  
		End 
		CLOSE db_sites   
		DEALLOCATE db_sites


	End
IF @Filter='Insert_SiteTaskTrackers'
BEGIN

		DECLARE db_sites CURSOR FOR  
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7
		FROM @Data AS l
		OPEN db_sites 
		FETCH NEXT FROM db_sites INTO 
		@TrackerGroup,@TrackerId , @TaskId ,@Actual,@SiteTaskTrackerId,@IsDeleted,@SiteId
		WHILE @@FETCH_STATUS = 0   
		BEGIN
		set @TrackerGroupId=0
	   Set @TrackerGroupId= (select Top 1 TrackerGroupId from PM_TrackerGroup where Title =@TrackerGroup And TaskId = CONVERT(numeric(20,0),@TaskId))
	    if @IsDeleted='True'
	           Begin
	              	delete tt  from PM_SiteTaskTracker as tt Where tt.SiteTaskTrackerId =CONVERT(numeric(20,0),@SiteTaskTrackerId)
	            end
		else
		Begin
		     if @SiteTaskTrackerId <> '0'
	           begin
			   update PM_SiteTaskTracker 
			Set Actual= @Actual 
			where SiteTaskTrackerId=convert(numeric(20,0),@SiteTaskTrackerId)
			   End
			   else
			   begin
			   	INSERT INTO PM_SiteTaskTracker([GroupTrackerId],[TaskTrackerId],[Actual],[SiteId])
			    SELECT @TrackerGroupId,CONVERT(numeric(20,0),@TrackerId),@Actual,CONVERT(numeric(20,0),@SiteId)		
			   End
		End
	   
		
		FETCH NEXT FROM db_sites INTO 
      	@TrackerGroup,@TrackerId , @TaskId ,@Actual,@SiteTaskTrackerId,@IsDeleted,@SiteId
		END   
		CLOSE db_sites   
		DEALLOCATE db_sites

END	
END