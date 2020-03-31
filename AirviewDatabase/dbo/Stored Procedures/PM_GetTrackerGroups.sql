-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PM_GetTrackerGroups
	-- Add the parameters for the stored procedure here
	@Filter nvarchar(50),
	@Value nvarchar(50)='',
	@Value2 nvarchar(50)=''

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
if @Filter='Get_Tasks_Trackers_Groups'
begin
    -- Insert statements for procedure here
	SELECT * from PM_TrackerGroup where TaskId = CONVERT(numeric(30,0), @Value) 
END
else if @Filter='Get_Tasks_Trackers_Groups'
begin
    -- Insert statements for procedure here
	SELECT * from PM_TrackerGroup where TaskId = CONVERT(numeric(30,0), @Value) 
END
Else If @Filter='Get_Tasks_Trackers'
Begin
SELECT tt.TrackerId,tt.Title,tt.Unit,tt.Budget,tt.Actual,pg.Title as 'TrackerGroup',pg.GroupCode   from PM_TaskTracker as tt inner join PM_TrackerGroup as pg on tt.TrackerGroupId= pg.TrackerGroupId
where pg.TaskId =CONVERT(numeric(30,0), @Value) 
End
Else If @Filter='GetTrackerTitles'
Begin
set @Value =(select Top 1 * from SplitString(@Value,'|'))
SELECT tt.TrackerId,tt.Title,tt.Unit,tt.Budget,tt.Actual,tt.TrackerGroupId   from PM_TaskTracker tt 
Inner join PM_TrackerGroup tg on tt.TrackerGroupId=tg.TrackerGroupId
where tg.Title=@Value and tg.TaskId= CONVERT(numeric(30,0), @Value2)
End
Else If @Filter='GetSiteTrackerTitles'
Begin

SELECT tt.TrackerId,tt.Title,tt.Unit,tt.Budget,st.Actual,pg.Title as 'TrackerGroup',pg.GroupCode,st.SiteTaskTrackerId as 'SiteTaskTrackerId'   from PM_TaskTracker as tt 
inner join PM_SiteTaskTracker as st on st.TaskTrackerId= tt.TrackerId 
inner join PM_TrackerGroup as pg on pg.TrackerGroupId=tt.TrackerGroupId
where pg.TaskId =CONVERT(numeric(30,0), @Value) and st.SiteId=CONVERT(numeric(30,0), @Value2) 
--where pg.TaskId =110240
End
End