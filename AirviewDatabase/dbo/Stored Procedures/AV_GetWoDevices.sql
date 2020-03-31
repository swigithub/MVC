CREATE PROCEDURE [dbo].[AV_GetWoDevices]
 @Filter nvarchar(50)
,@SiteId numeric(18,0)=null
,@NetworkId numeric(18,0)=null
,@BandId numeric(18,0)=null
,@CarrierId numeric(18,0)=null
,@ScopeId numeric(18,0)=null
,@UserId numeric(18,0)=null

AS
BEGIN
	if @Filter='GetTestTypes'
	begin
		select TestTypes 
		from AV_WoDevices
		where SiteId = @SiteId AND NetworkId=@NetworkId AND CarrierId=@CarrierId and BandId = @BandId AND ScopeId=@ScopeId	

	END
	
-- [dbo].[AV_GetWoDevices] 'BySiteId',528421,0,0,0,0
	else if @Filter='BySiteId'
	begin
		select *,cs.DeviceScheduleId,cs.SequenceId,cs.LayerStatusId
		from AV_WoDevices wd
		left outer join AV_ClusterSchedule cs on wd.DeviceScheduleId=cs.DeviceScheduleId
		where wd.SiteId = @SiteId

	END
	
	-- [dbo].[AV_GetWoDevices] 'BySiteId',388358,0,0,0,0
	else if @Filter='BySiteId_UserId'
	begin
		select * 
		from AV_WoDevices
		where SiteId = @SiteId AND UserId=@UserId

	end

END