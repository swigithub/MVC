-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE PM_ManageWorkGroups
	@filter NVARCHAR(50),
		@Value numeric(18,0)
		,@Value2 varchar(50) = NULL
AS
BEGIN
	IF	@Filter='Insert'
	BEGIN
	Delete  from PM_WorkGroup where ProjectId =@Value
		Declare @IdCount2 int,@iteration2 int=1,@WorkGroupId numeric(18,0)
		Declare @TempTableForLoop2 Table(Id int IDENTITY(1,1),WGId numeric(18,0))
		insert into @TempTableForLoop2 select Item from SplitString(@Value2,',')
		select @IdCount2=COUNT(*) from @TempTableForLoop2
			while @iteration2<=@IdCount2
				begin
						select @WorkGroupId=WGId from (
						select  Id,WGId from @TempTableForLoop2
						) as ff where ff.Id=@iteration2
						INSERT INTO PM_WorkGroup(ProjectId,WorkGroupId)
						VALUES(@Value,CONVERT(numeric(18,0), @WorkGroupId));
						set @iteration2=@iteration2+1;
                end
SELECT @WorkGroupId=@@IDENTITY;
	END	
	IF	@Filter='All'
	BEGIN
		SELECT * FROM AD_DefinationTypes
	END	

END