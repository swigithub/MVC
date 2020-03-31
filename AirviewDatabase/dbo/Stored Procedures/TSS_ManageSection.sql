-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_ManageSection]
@Filter NVARCHAR(50)
,@SectionId NUMERIC(18,0)=NULL
,@PSectionId NUMERIC(18,0)=NULL
,@SurveyId NUMERIC(18,0)=NULL
,@PIterationID NUMERIC(18,0)=NULL
,@IterationID NUMERIC(18,0)=NULL
,@SectionTitle NVARCHAR(50)=NULL
,@Description NVARCHAR(500)=NULL
,@SortOrder INT=NULL
,@IsActive BIT=NULL
--,@CreatedOn DATETIME=NULL
,@CreatedBy NUMERIC(18,0)=NULL
,@IsRepeatable BIT=NULL
,@IsApplicable BIT=NULL
,@RepeatCount INT=NULL
,@list List readonly ,
@IsSignatureRequired BIT=0
AS
BEGIN
	
	DECLARE @RETURN_VALUE int = 0 	
	
	IF @Filter='Insert'
	
	BEGIN
			--Getting last Sort Order index

		SELECT @SortOrder=ISNULL(MAX(SortOrder)+1,0) FROM TSS_Sections WHERE SurveyId=@SurveyId  

		--SELECT @PSectionId,@surveyId,@SectionTitle, @Description,@SortOrder,@IsActive,GETDATE(),@CreatedBy
		INSERT INTO TSS_Sections (PSectionId,SurveyId, SectionTitle,[Description],SortOrder, IsActive, CreatedOn, CreatedBy,IsRepeatable,IsApplicable,IsSignatureRequired)		
		VALUES(@PSectionId,@surveyId,@SectionTitle, @Description,@SortOrder,1,GETDATE(),@CreatedBy,@IsRepeatable,@IsApplicable,@IsSignatureRequired)
				
		SELECT @RETURN_VALUE = SCOPE_IDENTITY()	
		
		
	END	
	ELSE IF @Filter='Update'
	BEGIN
		UPDATE TSS_Sections
		SET SectionTitle = @SectionTitle,
		IsApplicable=@IsApplicable,
		
		[Description] = @Description,SortOrder = @SortOrder,IsActive = @IsActive,PSectionId = @PSectionId, IsRepeatable=@IsRepeatable,IsSignatureRequired=@IsSignatureRequired
		WHERE SectionId=@SectionId		
		
		SELECT @RETURN_VALUE =@SectionId
	END

	
	else IF @Filter='Delete'
	BEGIN
		DELETE FROM TSS_Sections
		WHERE SectionId=@SectionId
	END

	ELSE IF @Filter='Save_Sections_SortOrder'
	BEGIN
		DECLARE @NewSortOrder int,@SectionIdForSort NUMERIC(18,0);
		DECLARE updateSortOrder CURSOR FOR
		SELECT l.Value1,l.Value2 FROM @list as l
		OPEN updateSortOrder
		
		FETCH NEXT FROM updateSortOrder INTO @SectionIdForSort,@NewSortOrder
		WHILE @@FETCH_STATUS=0
			BEGIN

				UPDATE TSS_Sections SET SortOrder=@NewSortOrder WHERE SectionId=@SectionIdForSort

			FETCH NEXT FROM updateSortOrder INTO @SectionIdForSort,@NewSortOrder
			END
		CLOSE updateSortOrder
		DEALLOCATE updateSortOrder

	END

	ELSE IF @Filter='Delete_Section_By_Id'
	BEGIN
		-- Temporary Tables to Manage Child Sections

		DECLARE @TempSectionHolder as TABLE 
		(
			SectionId numeric(18,0)
		)

		DECLARE @TempSectionHolder_Cursor as TABLE 
		(
			SectionId numeric(18,0)
		)
		--------------------------------------------

		IF  NOT EXISTS (SELECT 1 FROM TSS_SiteSections WHERE SectionId=@SectionId)
		BEGIN
			-- INSERTING IMMEDIATE CHILDS --

			INSERT INTO @TempSectionHolder
			SELECT SectionId FROM TSS_Sections WHERE PSectionId=@SectionId

			------------------------------

			-- DELETING SECTION RESPONSE --

			DELETE FROM TSS_Responses WHERE QuestionId IN 
			(
			SELECT QuestionId FROM TSS_Questions WHERE SectionId=@SectionId
			)
			-- DELETING SECTION QUESTIONS --

			DELETE FROM TSS_Questions WHERE SectionId=@SectionId

			-- DELETING SECTION

			DELETE FROM TSS_Sections WHERE SectionId=@SectionId

			----------- START DELETING CHILD RECORDS ---------------

			RepeatChildSectionSteps:

			DELETE FROM @TempSectionHolder_Cursor

			IF EXISTS (SELECT 1 FROM @TempSectionHolder)
			BEGIN
		
				DECLARE @SectionId_Cursor NUMERIC(18,0)

				-- LOOPING THROUGH ALL CHILD SECTIONS ---

				DECLARE DELETE_CURSOR CURSOR FOR
				SELECT SectionId FROM @TempSectionHolder

				OPEN DELETE_CURSOR

				FETCH NEXT FROM DELETE_CURSOR 
				INTO @SectionId_Cursor

				WHILE @@FETCH_STATUS=0
				BEGIN

					-- Code To Delete Section and Manage their Child Sections

					INSERT INTO @TempSectionHolder_Cursor 
					SELECT SectionId FROM TSS_Sections WHERE PSectionId = @SectionId_Cursor

					-- DELETING RESPONSES ----

					DELETE FROM TSS_Responses WHERE QuestionId IN 
					(
					SELECT QuestionId FROM TSS_Questions WHERE SectionId=@SectionId_Cursor
					)
			
					--- DELETING QUESTIONS --

					DELETE FROM TSS_Questions WHERE SectionId=@SectionId_Cursor

					-- DELETING SECTION

					DELETE FROM TSS_Sections WHERE SectionId=@SectionId_Cursor

					----------------------

					FETCH NEXT FROM DELETE_CURSOR 
					INTO @SectionId_Cursor
				END

				CLOSE DELETE_CURSOR
				DEALLOCATE DELETE_CURSOR

				-----------------------------------------
				-- CHECK IF HAVE MORE CHILD HIERARCHY --- 

				IF EXISTS (SELECT 1 FROM @TempSectionHolder_Cursor)
				BEGIN
					-- UPDATING RECORDS TO PERFORM RECURSIVE OPERATIONS --
			 
					DELETE FROM @TempSectionHolder

					INSERT INTO @TempSectionHolder
					SELECT SectionId FROM @TempSectionHolder_Cursor

					GOTO RepeatChildSectionSteps

				END
		

			END

			---------- END DELETING CHILD RECORDS --------------

			SELECT 'SECTION_DELETED'
		END

		ELSE
		BEGIN
			SELECT 'SECTION_IS_IN_USE'
		END
 
	END
END