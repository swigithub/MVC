-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PM_ManageProjectResources
@Filter NVARCHAR(50)
,@List List READONLY
,@MapCode nvarchar(50)=NULL
,@ProjectId nvarchar(50)=0
,@TaskId nvarchar(50)=0
,@UserId nvarchar(50)=0
,@AssignToId NUMERIC(18,0)=0
AS
BEGIN
	--IF @Filter='Insert'
	--BEGIN
	--	DECLARE @TaskId AS NUMERIC(18,0)=(SELECT TOP 1 l.Value2 FROM @List l)
		
	--	DELETE FROM PM_ProjectResources WHERE TaskId=@TaskId
	--	INSERT INTO PM_ProjectResources
	--	(
			
	--		ProjectId,
	--		TaskId,
	--		AssignedById,
	--		AssignToId,
	--		AssignedDate,
	--		ForecastDate,
	--		PlanDate,
	--		IsActive
	--	)
	--	SELECT l.Value1,l.Value2,l.Value3,l.Value4,GETDATE(),GETDATE(),GETDATE(),1 FROM @List l







	--END

	IF @Filter='Insert2'	
	BEGIN
	BEGIN TRANSACTION				 
	 print '1'
		DECLARE @PId AS NUMERIC(18,0)=(SELECT TOP 1 l.Value1 FROM @List l)
		DELETE FROM PM_ProjectResources WHERE ProjectId = @PId
		--DECLARE THE CURSOR FOR A QUERY.
		DECLARE addSite CURSOR READ_ONLY
		FOR
		SELECT l.Value1,l.Value2,l.Value3,l.Value4 FROM @List l
		--OPEN CURSOR
		OPEN addSite 
		 print '2'


		--FETCH THE RECORD INTO THE VARIABLES.
		FETCH NEXT FROM addSite INTO @ProjectId,@TaskId,@UserId,@AssignToId
		--LOOP UNTIL RECORDS ARE AVAILABLE.
		WHILE @@FETCH_STATUS = 0
		BEGIN
			
			INSERT INTO PM_ProjectResources
		(
			
			ProjectId,
			TaskId,
			AssignedById,
			AssignToId,
			AssignedDate,
			ForecastDate,
			PlanDate,
			IsActive
		)
		SELECT @ProjectId,@TaskId,@UserId,@AssignToId,GETDATE(),GETDATE(),GETDATE(),1 
			
				
			--FETCH THE NEXT RECORD INTO THE VARIABLES.
			FETCH NEXT FROM addSite INTO @ProjectId,@TaskId,@UserId,@AssignToId
		END 
		--CLOSE THE CURSOR.
		CLOSE addSite
		DEALLOCATE addSite
		COMMIT
	END
END