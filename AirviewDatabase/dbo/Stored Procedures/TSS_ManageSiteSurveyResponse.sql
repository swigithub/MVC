CREATE PROCEDURE TSS_ManageSiteSurveyResponse
@Filter NVARCHAR(50),
@status int = null,
@SecId int = 0,
@PSiteQuestionId int = 0,
@Question_Id int=0,
@DynamicRows int=null,
@Responses TSS_ResponsesList READONLY	
,@List TSS_ResponsesList READONLY,
@base64String nvarchar(max) = null,
@MapImage nvarchar(max) = null,
@MapZoom int=17,
@AzimuthForMap numeric(18,2)=0,
@IsFinished bit=0

AS
BEGIN
	DECLARE @SiteId AS NUMERIC(18,0)=0
	DECLARE @SurveyId AS NUMERIC(18,0)=0
	DECLARE @SectionId AS NUMERIC(18,0)=0
	DECLARE @QuestionId AS NUMERIC(18,0)=0
	DECLARE @SiteSectionId AS NUMERIC(18,0)=0
	DECLARE @SiteQuestionId AS NUMERIC(18,0)=0
	DECLARE @ResponseId AS NUMERIC(18,0)=0
	
	DECLARE @IterationId AS NUMERIC(18,0)=0
	DECLARE @ResponseText AS NVARCHAR(250)=''
	DECLARE @pIterationId AS NUMERIC(18,0)=0
	DECLARE @MinValue AS FLOAT=0
	DECLARE @MaxValue AS FLOAT=0
	DECLARE @IsGPS AS BIT=0
	DECLARE @Signature AS Nvarchar(MAX)
	
	DECLARE @ActionTypeId AS NUMERIC=0
	DECLARE @ActionValue AS NVARCHAR(MAX)=''
	DECLARE @Remarks AS NVARCHAR(50)=''
	DECLARE @ResponseValue AS NVARCHAR(MAX)=''
	DECLARE @Azimuth AS VARCHAR(50)
	DECLARE @Latitude AS VARCHAR(50)
	DECLARE @Longitude AS VARCHAR(50)

	DECLARE @Altitude AS VARCHAR(50)
	DECLARE @ObjectView AS VARCHAR(50)
	DECLARE @GPSAccuracy AS VARCHAR(50)

	---- Declaretion For Status Changes


	DECLARE @SiteSectionIdForStatus NUMERIC(18,0)
	DECLARE @GetPSiteSectionofCurrentSection NUMERIC(18,0)
	DECLARE @Count int=0;
	DECLARE @CompletedCount int =0;
	DECLARE @SiteSectionId_Cursor numeric(18,0)	
	DECLARE @GetStatusId numeric(18,0)
	DECLARE @CheckIfMandatoryLeftCount int=0;
	DECLARE @SiteQuestionId_Cursor numeric(18,0)
	Declare @CheckIsRequired BIT=0;
	DECLARE @CheckIsAnswered BIT=0;

	DECLARE @GetSiteSurveyId numeric(18,0)
	DECLARE @GetSiteSectionId NUMERIC(18,0)
	--- End Declaration For Status Changes
	
	DECLARE @IN_PROGRESS_ID INT =(SELECT  DefinationId FROM AD_Definations WHERE KeyCode='IN_PROGRESS')
	DECLARE @PENDING_SCHEDULED_ID INT =(SELECT  DefinationId FROM AD_Definations WHERE KeyCode='PENDING_SCHEDULED')
	DECLARE @DRIVE_COMPLETED_ID INT =(SELECT  DefinationId FROM AD_Definations WHERE KeyCode='DRIVE_COMPLETED')
	DECLARE @PENDING_WITH_ISSUE_ID INT =(SELECT  DefinationId FROM AD_Definations WHERE KeyCode='PENDING_WITH_ISSUE')
	
	IF @Filter='Save_Single_SurveyResponse'
	BEGIN
		SET @GetSiteSectionId=(SELECT SiteSectionId FROM TSS_SiteQuestions WHERE SiteQuestionId=@PSiteQuestionId)
		SET @GetSiteSurveyId=(select SiteSurveyId from TSS_SiteSections where SiteSectionId=@GetSiteSectionId)
		DECLARE @RepeatCount AS INT=(SELECT COUNT(*) FROM @Responses);
		DELETE FROM TSS_SiteResponses
		WHERE SiteQuestionId=@PSiteQuestionId ;
		UPDATE TSS_SiteQuestions set IsAnswered = 0
		WHERE SiteQuestionId=@PSiteQuestionId ;
		
		DECLARE db_cluster2 CURSOR FOR  
		SELECT ress.Value1, ress.Value2, ress.Value3, ress.Value4,ress.Value5,ress.Value6, ress.Value7,ress.Value15 FROM @Responses ress
		OPEN db_cluster2 
		FETCH NEXT FROM db_cluster2 INTO @SiteQuestionId, @ResponseText, @ResponseValue, @ResponseId, @MinValue, @MaxValue, @IsGPS,@Signature
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			--INSERT INTO TSS_SiteResponses(SiteQuestionId,ResponseId,ResponseText,ResponseValue,IterationId,PIterationId,MinValue,MaxValue,IsGps)
			--SELECT @SiteQuestionId,(CASE WHEN @ResponseId>0 THEN @ResponseId ELSE NULL END),
			--(CASE WHEN @ResponseId=0 THEN @ResponseText ELSE NULL END),@ResponseValue,0,0,@MinValue,@MaxValue,ISNULL(@IsGPS,0)
			--FETCH NEXT FROM db_cluster2 INTO @SiteQuestionId, @ResponseText, @ResponseValue, @ResponseId, @MinValue, @MaxValue, @IsGPS

			INSERT INTO TSS_SiteResponses(SiteQuestionId,ResponseId,ResponseText,ResponseValue,IterationId,PIterationId,MinValue,MaxValue,IsGps,Signature)
			SELECT @SiteQuestionId,(CASE WHEN @ResponseId>0 THEN @ResponseId ELSE NULL END),
			@ResponseText,@ResponseValue,0,0,@MinValue,@MaxValue,ISNULL(@IsGPS,0),@Signature
			FETCH NEXT FROM db_cluster2 INTO @SiteQuestionId, @ResponseText, @ResponseValue, @ResponseId, @MinValue, @MaxValue, @IsGPS,@Signature

		END   
		CLOSE db_cluster2   
		DEALLOCATE db_cluster2
		IF @RepeatCount > 0
		Begin
		UPDATE TSS_SiteQuestions set IsAnswered = 1,MapImage=@MapImage,MapZoom=@MapZoom,Azimuth=@AzimuthForMap
			WHERE SiteQuestionId=@PSiteQuestionId ;
		End
		--Required Actions
		DECLARE @ResponseCount AS INT=(SELECT COUNT(*) FROM @List);
		SET @SurveyId = ISNULL((select TOP 1 SiteSurveyId from TSS_SiteQuestions where SiteQuestionId = @PSiteQuestionId),0);
		SET @SiteId = ISNULL((select TOP 1 SiteId from TSS_SiteSurvey where SiteSurveyId = @SurveyId),0);
		IF @ResponseCount > 0
		BEGIN
		DELETE FROM TSS_RequiredActions
			WHERE SiteQuestionId=@PSiteQuestionId
		End
		
		DECLARE db_cluster3 CURSOR FOR  
		SELECT res.Value1, res.Value2, res.Value3, res.Value4,res.Value5,res.Value6,res.Value7,res.Value8,res.Value9,res.Value10,res.Value11 FROM @List res
		OPEN db_cluster3 
		FETCH NEXT FROM db_cluster3 INTO @SectionId, @QuestionId, @ActionTypeId, @Remarks, @ActionValue,@Azimuth,@Latitude,@Longitude,@Altitude,@GPSAccuracy,@ObjectView
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			INSERT INTO TSS_RequiredActions(SiteId,SiteSurveyId,SiteQuestionId,IterationId,PIterationId,ActionTypeId,ActionValue,Remarks,Azimuth,Latitude,Longitude,Altitude,GPSAccuracy,ObjectView)			
			SELECT @SiteId,@SurveyId,@QuestionId,@IterationId,@pIterationId,@ActionTypeId,@ActionValue,@Remarks,@Azimuth,@Latitude,@Longitude,@Altitude,@GPSAccuracy,@ObjectView
			
		FETCH NEXT FROM db_cluster3 INTO @SectionId, @QuestionId, @ActionTypeId, @Remarks, @ActionValue,@Azimuth,@Latitude,@Longitude,@Altitude,@GPSAccuracy,@ObjectView
		END   
		CLOSE db_cluster3   
		DEALLOCATE db_cluster3

		-- Steps For Dynamic Rows
		if @Question_Id <> 0
		Begin
			update TSS_Questions set DynamicRowsCount=@DynamicRows where QuestionId=@Question_Id
	    End
		-- End of Dynamic Rows
		DECLARE @tempSiteSectionId int = (select top 1 SiteSectionId from TSS_SiteQuestions where SiteQuestionId =@PSiteQuestionId);
		DECLARE  @tempStatusid int  = (select top 1 DefinationId from  AD_Definations where KeyCode = 'IN_PROGRESS');
		
		-- If Already Mark Completed Do Nothing
		
		DECLARE @CheckStatusAlready INT =( SELECT StatusId FROM TSS_SiteSections WHERE SiteSectionId=@tempSiteSectionId)
		DECLARE  @GetCompletedId int  = (select top 1 DefinationId from  AD_Definations where KeyCode = 'DRIVE_COMPLETED');
		IF( @CheckStatusAlready <> @GetCompletedId )
		BEGIN

		update TSS_SiteSections set StatusId = @tempStatusid where SiteSectionId = @tempSiteSectionId; 

		END
		------------------


		------ Code To Change Status Of Sections
		-- Updating Heirarchy Status Child-----
			DECLARE @CountStatusChild int=0;
			DECLARE @TempTableStatus as Table(SiteSectionId numeric(18,0))
			DECLARE @TempHolderStatus as Table(SiteSectionId numeric(18,0))

			
		IF @CheckStatusAlready=@IN_PROGRESS_ID
		BEGIN
			SET @CountStatusChild=0
			DECLARE @TempTableParentStatus as Table(SiteSectionId numeric(18,0))
			DECLARE @TempHolderParentStatus as Table(SiteSectionId numeric(18,0))

			Insert Into @TempTableParentStatus
			select Distinct PSiteSectionId from TSS_SiteSections 
			Where SiteSectionId=@tempSiteSectionId

			LoopHereParentStatus:
			Select @CountStatusChild=Count(*) from @TempTableParentStatus
			IF @CountStatusChild>0
			BEGIN
				Update TSS_SiteSections set StatusId=@CheckStatusAlready Where SiteSectionId in (Select SiteSectionId from @TempTableParentStatus)

				Insert Into @TempHolderParentStatus Select SiteSectionId from @TempTableParentStatus

				Delete From @TempTableParentStatus

				Insert Into @TempTableParentStatus
				select Distinct PSiteSectionId from TSS_SiteSections 
				Where SiteSectionId in (Select SiteSectionId from @TempHolderParentStatus)

				Delete From @TempHolderParentStatus

				GOTO LoopHereParentStatus
			END

			 -- END Updateing Parent Level Heirarchy ---
		END
		------ End Code For Sections Change Status

		-- Update Survey Status
		print 'updateing'
		print @GetSiteSurveyId
			EXEC TSS_UpdateSurveyStatusForDashboard @GetSiteSurveyId
		-- END 
		Declare @statusName nvarchar(max) = ( select top 1 DefinationName from  AD_Definations where KeyCode = 'IN_PROGRESS');
		return (SELECT COUNT(*) FROM @List);

		
	END
	else if @Filter='Save_Section_Signature'
	Begin
		SET @GetSiteSectionId=(SELECT SiteSectionId FROM TSS_SiteQuestions WHERE SiteQuestionId=@PSiteQuestionId)
		SET @GetSiteSurveyId=(select SiteSurveyId from TSS_SiteSections where SiteSectionId=@GetSiteSectionId)
	
	if @base64String is not null
		Begin
		Declare @pendingStatus int = (select top 1 DefinationId from  AD_Definations where KeyCode = 'IN_PROGRESS');
		update TSS_SiteSections set Signature = @base64String , StatusId = @pendingStatus where SiteSectionId = @PSiteQuestionId
		-- Update Survey Status
			EXEC TSS_UpdateSurveyStatusForDashboard @GetSiteSurveyId
		-- END 

		End
	End
	
	else if @Filter='Mark_Section_Status'
	Begin
		SET @GetSiteSurveyId=(select SiteSurveyId from TSS_SiteSections where SiteSectionId=@SecId)
		DECLARE @UpdatedStauts varchar(50)='STATUS_CHANGED'

		-- Updating Child Heirarchy Status -----
		Declare @CountStatus int=0;
		DECLARE @TempTable as Table(SiteSectionId numeric(18,0))
		DECLARE @TempHolder as Table(SiteSectionId numeric(18,0))

		Update TSS_SiteSections set StatusId=@status Where SiteSectionId =@SecId
		Insert Into @TempTable
		select SiteSectionId from TSS_SiteSections 
		Where SiteSectionId=@SecId
		LoopHere:
		Select @CountStatus=Count(*) from @TempTable
		IF @CountStatus>0
		BEGIN
			Update TSS_SiteSections set StatusId=@status Where PSiteSectionId in (Select SiteSectionId from @TempTable)

			Insert Into @TempHolder Select SiteSectionId from @TempTable

			Delete From @TempTable

			Insert Into @TempTable
			select SiteSectionId from TSS_SiteSections 
			Where PSiteSectionId in (Select SiteSectionId from @TempHolder)

			Delete From @TempHolder

			GOTO LoopHere
		END
		-- END Updating Child Heirarchy Status --- 	
		-- Updateing Parent Level Heirarchy ---
		IF @status=@IN_PROGRESS_ID  or @status=@PENDING_WITH_ISSUE_ID
		BEGIN
			SET @CountStatus=0
			DECLARE @TempTableParent as Table(SiteSectionId numeric(18,0))
			DECLARE @TempHolderParent as Table(SiteSectionId numeric(18,0))

			Insert Into @TempTableParent
			select Distinct PSiteSectionId from TSS_SiteSections 
			Where SiteSectionId=@SecId

			LoopHereParent:
			Select @CountStatus=Count(*) from @TempTableParent
			IF @CountStatus>0
			BEGIN
				Update TSS_SiteSections set StatusId=@status Where SiteSectionId in (Select SiteSectionId from @TempTableParent)

				Insert Into @TempHolderParent Select SiteSectionId from @TempTableParent

				Delete From @TempTableParent

				Insert Into @TempTableParent
				select Distinct PSiteSectionId from TSS_SiteSections 
				Where SiteSectionId in (Select SiteSectionId from @TempHolderParent)

				Delete From @TempHolderParent

				GOTO LoopHereParent
			END

		END
			 -- END Updateing Parent Level Heirarchy ---
		-- Update Survey Status For Dashboard
		 
		EXEC TSS_UpdateSurveyStatusForDashboard @GetSiteSurveyId
	-- END 			

			SELECT @UpdatedStauts
	END
		
END