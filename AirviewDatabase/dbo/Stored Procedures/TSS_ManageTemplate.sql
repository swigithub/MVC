-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_ManageTemplate]
@Filter NVARCHAR(50)
,@List List READONLY
AS
BEGIN
	IF @Filter='ADD_QUESTION_FROM_TEMPLATE'
	BEGIN
		-- Value1=SurveyId,Value2=SectionId,Value3=QuestionId,Value4=IdType (e.g Section or Survey)

		DECLARE @NewSurveyId AS NUMERIC(18,0)=(SELECT TOP 1 Value1 FROM @List)
		DECLARE @NewSectionIdParameter AS NUMERIC(18,0)=(SELECT TOP 1 Value1 FROM @List)
		DECLARE @IdType AS varchar(50)=(SELECT TOP 1 Value4 FROM @List)

		------------- IMPORTANT DECLRATIONS ----------------------
					
		DECLARE @SectionId_Cursor AS NUMERIC(18,0)
		DECLARE @QuestionId_Cursor AS NUMERIC(18,0)
		DECLARE @NewSectionId_Generated AS NUMERIC(18,0)
		DECLARE @NewQuestionId_Generated AS NUMERIC(18,0)

		----------------------------------------------------------

		IF (@IdType='Survey')
			BEGIN
					
				---------NEW SECTION CREATION ------------


				DECLARE SECTION_CURSOR CURSOR 
				FOR
				SELECT DISTINCT Value2 FROM @List

				OPEN SECTION_CURSOR

				FETCH NEXT FROM SECTION_CURSOR INTO @SectionId_Cursor 

				WHILE @@FETCH_STATUS=0
					BEGIN
							INSERT INTO TSS_Sections
										(PSectionId, SurveyId, SectionTitle, Description, SortOrder, IsActive, CreatedOn, CreatedBy, IsRepeatable, IsApplicable, IsSignatureRequired)
							SELECT		  0, @NewSurveyId, SectionTitle, Description, SortOrder, IsActive, GetDate(), CreatedBy, IsRepeatable, IsApplicable, IsSignatureRequired
							FROM TSS_Sections 
							WHERE SectionId=@SectionId_Cursor

							SET @NewSectionId_Generated=SCOPE_IDENTITY()

							--------- NEW QUESTION CREATION --------------

								DECLARE QUESTION_CURSOR CURSOR
								FOR
								SELECT DISTINCT Value3 FROM @List WHERE Value2= @SectionId_Cursor

								OPEN QUESTION_CURSOR 

								FETCH NEXT FROM QUESTION_CURSOR INTO @QuestionId_Cursor

								WHILE @@FETCH_STATUS=0
								 	BEGIN
										    INSERT INTO TSS_Questions
													  (SectionId, QuestionTypeId, Question, Description, Weightage, SortOrder, IsRequired, IsNoteRequired, IsImageRequired, 
													  IsBarCodeRequired, CreatedOn, CreatedBy, IsRepeatable, UnitTypeId, UnitId, UnitSystemId, IsActive, 
													   IsVerificationRequired, IsVideoRequired, IsAudioRequired, IsDocumentRequired, TotalColumn, TotalRows, DynamicRows, DynamicRowsCount,
													   IsImageDetailRequired,IsMultiLocation,SurveyEntity,Prefix)

											SELECT    @NewSectionId_Generated, QuestionTypeId, Question, Description, Weightage, SortOrder, IsRequired, IsNoteRequired, IsImageRequired,
														IsBarCodeRequired, GETDATE(), CreatedBy, IsRepeatable, UnitTypeId, UnitId, UnitSystemId, IsActive, 
													   IsVerificationRequired, IsVideoRequired, IsAudioRequired, IsDocumentRequired, TotalColumn, TotalRows, DynamicRows, DynamicRowsCount,
													   IsImageDetailRequired,IsMultiLocation,SurveyEntity,Prefix
											FROM TSS_Questions 
											WHERE QuestionId =@QuestionId_Cursor

											SET @NewQuestionId_Generated=SCOPE_IDENTITY()

											 -------- NEW RESPONSES CREATION -------------


											 INSERT INTO TSS_Responses
													  (QuestionId, ResponseText, ResponseValue, SortOrder, IsPassed, MinValue, MaxValue, IsGps, IsActive, IsReadOnly, UserValues)
											 SELECT    @NewQuestionId_Generated, ResponseText, ResponseValue, SortOrder, IsPassed, MinValue, MaxValue, IsGps, IsActive, IsReadOnly, UserValues
											 FROM TSS_Responses
											 WHERE QuestionId=@QuestionId_Cursor

											 -------- END NEW RESPONSE CREATION -----------

											 FETCH NEXT FROM QUESTION_CURSOR INTO @QuestionId_Cursor
									END

								CLOSE QUESTION_CURSOR
								DEALLOCATE QUESTION_CURSOR
								

							-------- END NEW QUESTION CREATION ----------

				FETCH NEXT FROM SECTION_CURSOR INTO @SectionId_Cursor 

					END

				CLOSE SECTION_CURSOR
				DEALLOCATE SECTION_CURSOR


				-------- END NEW SECTION CREATION ------------
				
			END
		 ELSE 
			BEGIN


				--------- NEW QUESTION CREATION --------------

								DECLARE QUESTION_CURSOR CURSOR
								FOR
								SELECT DISTINCT Value3 FROM @List

								OPEN QUESTION_CURSOR 

								FETCH NEXT FROM QUESTION_CURSOR INTO @QuestionId_Cursor

								WHILE @@FETCH_STATUS=0
								 	BEGIN
										    INSERT INTO TSS_Questions
													  (SectionId, QuestionTypeId, Question, Description, Weightage, SortOrder, IsRequired, IsNoteRequired, IsImageRequired, 
													  IsBarCodeRequired, CreatedOn, CreatedBy, IsRepeatable, UnitTypeId, UnitId, UnitSystemId, IsActive, 
													   IsVerificationRequired, IsVideoRequired, IsAudioRequired, IsDocumentRequired, TotalColumn, TotalRows, DynamicRows, DynamicRowsCount,
													   IsImageDetailRequired,IsMultiLocation,SurveyEntity,Prefix)

											SELECT    @NewSectionIdParameter, QuestionTypeId, Question, Description, Weightage, SortOrder, IsRequired, IsNoteRequired, IsImageRequired,
														IsBarCodeRequired, GETDATE(), CreatedBy, IsRepeatable, UnitTypeId, UnitId, UnitSystemId, IsActive, 
													   IsVerificationRequired, IsVideoRequired, IsAudioRequired, IsDocumentRequired, TotalColumn, TotalRows, DynamicRows, DynamicRowsCount,
													   IsImageDetailRequired,IsMultiLocation,SurveyEntity,Prefix
											FROM TSS_Questions 
											WHERE QuestionId =@QuestionId_Cursor

											SET @NewQuestionId_Generated=SCOPE_IDENTITY()

											 -------- NEW RESPONSES CREATION -------------


											 INSERT INTO TSS_Responses
													  (QuestionId, ResponseText, ResponseValue, SortOrder, IsPassed, MinValue, MaxValue, IsGps, IsActive, IsReadOnly, UserValues)
											 SELECT    @NewQuestionId_Generated, ResponseText, ResponseValue, SortOrder, IsPassed, MinValue, MaxValue, IsGps, IsActive, IsReadOnly, UserValues
											 FROM TSS_Responses
											 WHERE QuestionId=@QuestionId_Cursor

											 -------- END NEW RESPONSE CREATION -----------

											 FETCH NEXT FROM QUESTION_CURSOR INTO @QuestionId_Cursor
									END

								CLOSE QUESTION_CURSOR
								DEALLOCATE QUESTION_CURSOR
								

				-------- END NEW QUESTION CREATION ----------

			END

	END
END