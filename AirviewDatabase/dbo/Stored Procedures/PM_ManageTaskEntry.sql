-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PM_ManageTaskEntry
	@Filter NVARCHAR(50)
	 ,
	@EntryId NUMERIC(18, 0) = NULL
	 ,
	@ProjectId NUMERIC(18, 0) = NULL
	 ,
	@TaskId NUMERIC(18, 0) = NULL
	 ,
	@FormId NUMERIC(18, 0) = NULL
	 ,
	@FormValue NVARCHAR(500) = NULL
	 ,
	@CreatedById NUMERIC(18, 0) = NULL
	 ,
	@CreatedOn DATETIME = NULL
	,
	@ProjectSiteId NUMERIC(18, 0) = NULL,
	@Revision NUMERIC(18, 0) = 0,
	@List TaskList readonly,
	 @Comments NVARCHAR(200) = NULL
AS
BEGIN
	IF @Filter='Insert'
	BEGIN

	set @Revision = (select max(revision) from PM_TaskEntry where ProjectId=(select top 1 l.Value2 from @List l) and ProjectSiteId=(select top 1 l.Value3 from @List l)
	and TaskId = (select top 1 l.Value4 from @List l))
set @Revision =	(SELECT  ISNULL(@Revision,0))
print(@Revision)
	BEGIN TRANSACTION	
	
	DECLARE addSite CURSOR READ_ONLY
		FOR
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value15
		 FROM @List l
		--OPEN CURSOR
		OPEN addSite 
		 
		--FETCH THE RECORD INTO THE VARIABLES.
		FETCH NEXT FROM addSite INTO @EntryId,@ProjectId,@ProjectSiteId,@TaskId,@FormId,@FormValue,@CreatedById,@CreatedOn,@Comments
		--LOOP UNTIL RECORDS ARE AVAILABLE.
		WHILE @@FETCH_STATUS = 0
		BEGIN
		--	BEGIN TRANSACTION				 
	print('2')
		INSERT INTO pm_TaskEntry(ProjectId,ProjectSiteId,TaskId,FormId,FormValue,CreatedById,CreatedOn,Revision,Comments)
		VALUES(@ProjectId,@ProjectSiteId, @TaskId,@FormId,@FormValue,@CreatedById,GETDATE(),@Revision+1,@Comments)
			
				--COMMIT
			--FETCH THE NEXT RECORD INTO THE VARIABLES.
			FETCH NEXT FROM addSite INTO @EntryId,@ProjectId,@ProjectSiteId,@TaskId,@FormId,@FormValue,@CreatedById,@CreatedOn,@Comments
		END 
		--CLOSE THE CURSOR.
		CLOSE addSite
		DEALLOCATE addSite

		COMMIT
	END

	else if @Filter='GetTaskEntries'
	begin 
	select *,tnp.Title from PM_TaskEntry pte
	inner join TMP_NodesProperties  tnp on tnp.FormId = pte.FormId
	 where pte.TaskId=@TaskId and pte.ProjectSiteId=@ProjectSiteId and tnp.IsDeleted =0
	end
END