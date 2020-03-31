
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PM_ManageTrackerGroups]
	-- Add the parameters for the stored procedure here
	@Filter nvarchar(50),
@ProjectId numeric(20,0),
@TaskId numeric(20,0),
@Title nvarchar(50),
@ParentId numeric(20,0),
@Groupcode nvarchar(50)
AS
BEGIN
	IF @Filter ='Insert_Groups'
	Begin
	Declare @Count as numeric(20,0)
	Set @Count=(select COUNT(*) from [dbo].[PM_TrackerGroup] as tg where Title = @Title and TaskId=@TaskId AND ParentId= @ParentId)
	if  @Count = 0
	begin
	INSERT INTO [dbo].[PM_TrackerGroup]
           ([ProjectId]
           ,[TaskId]
           ,[ParentId]
           ,[Title],[GroupCode])
     VALUES
           (@ProjectId
           ,@TaskId
           ,@ParentId
		   ,@Title,@Groupcode)
		   end
	End
END