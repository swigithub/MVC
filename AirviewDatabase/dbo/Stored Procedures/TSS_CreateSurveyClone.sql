--TSS_CreateSurveyClone 'Software Field Survey 2', 70081
CREATE PROCEDURE [dbo].[TSS_CreateSurveyClone]
 @SurveyTitle varchar(100)='',
 @SurveyId numeric(18,0)=NULL
AS 
BEGIN

BEGIN TRANSACTION
BEGIN TRY


-- Creating Survey

INSERT INTO TSS_SurveyDocuments
	 (ClientId,CityId,SurveyTitle,Description,CategoryId,SubCategoryId,UnitSystemId,IsActive,CreatedOn,CreatedBy)
SELECT ClientId, CityId,@SurveyTitle, Description, CategoryId, SubCategoryId, UnitSystemId, IsActive, GetDate(), CreatedBy
FROM     TSS_SurveyDocuments 
WHERE SurveyId=@SurveyId

Declare @NewSurveyId numeric(18,0)=SCOPE_IDENTITY();


-- Creating Questions  and Sections

Declare @SectionId_Cursor numeric(18,0)

declare @TEMPSections table ([SectionId] [numeric](18, 0),
	[NewSectionId] [numeric](18, 0))
---
DECLARE SECTION_CURSOR CURSOR FOR     
SELECT SectionId FROM  TSS_Sections  where SurveyId=@SurveyId
OPEN SECTION_CURSOR    
  
FETCH NEXT FROM SECTION_CURSOR     
INTO @SectionId_Cursor   

WHILE @@FETCH_STATUS = 0    
BEGIN    
    -- Insert Sections Here 
	
	INSERT INTO TSS_Sections 
			(PSectionId,SurveyId,SectionTitle,Description,SortOrder,IsActive,CreatedOn,CreatedBy,IsRepeatable,IsApplicable,IsSignatureRequired)
	SELECT PSectionId,@NewSurveyId, SectionTitle, Description, SortOrder, IsActive, GetDate(), CreatedBy, IsRepeatable, IsApplicable, IsSignatureRequired
FROM     TSS_Sections Where SectionId=@SectionId_Cursor

DECLARE @NewSectionId numeric(18,0)=SCOPE_IDENTITY()
INSERT INTO @TEMPSections(SectionId, NewSectionId) values (@SectionId_Cursor, @NewSectionId)

-- CURSOR FOR QUESTIONS

			Declare @QuestionId_Cursor numeric(18,0)

			DECLARE QUESTION_CURSOR CURSOR FOR     
			SELECT QuestionId FROM  TSS_Questions  where SectionId=@SectionId_Cursor

			OPEN QUESTION_CURSOR    
  
			FETCH NEXT FROM QUESTION_CURSOR     
			INTO @QuestionId_Cursor   

			WHILE @@FETCH_STATUS = 0    
			BEGIN   

			-- Insert Question Here  

				INSERT INTO TSS_Questions 
					 (SectionId,QuestionTypeId,Question,Description,Weightage,SortOrder,IsRequired,IsNoteRequired,IsImageRequired,IsBarCodeRequired,CreatedOn,CreatedBy,
						IsRepeatable,UnitTypeId,UnitId,UnitSystemId,IsActive,IsVerificationRequired,IsVideoRequired,IsAudioRequired,IsDocumentRequired,
						TotalColumn,TotalRows,DynamicRows,DynamicRowsCount)

				SELECT @NewSectionId, QuestionTypeId, Question, Description, Weightage, SortOrder, IsRequired, IsNoteRequired, IsImageRequired, IsBarCodeRequired, GetDate(), CreatedBy, 
						IsRepeatable, UnitTypeId, UnitId, UnitSystemId, IsActive, IsVerificationRequired, IsVideoRequired, IsAudioRequired, IsDocumentRequired,
						 TotalColumn, TotalRows, DynamicRows, DynamicRowsCount
				FROM     TSS_Questions Where QuestionId=@QuestionId_Cursor

				DECLARE @NewQuestionId numeric(18,0)=SCOPE_IDENTITY()

				------------- CREATE FOR RESPONSES
				 
										INSERT INTO TSS_Responses
												(QuestionId, ResponseText, ResponseValue, SortOrder, IsPassed, MinValue, MaxValue, IsGps, IsActive, IsReadOnly, UserValues)
										SELECT @NewQuestionId, ResponseText, ResponseValue, SortOrder, IsPassed, MinValue, MaxValue, IsGps, IsActive, IsReadOnly, UserValues
										FROM  TSS_Responses where QuestionId=@QuestionId_Cursor


				------------- END CREATION FOR RESPONSES

			FETCH NEXT FROM QUESTION_CURSOR     
		    INTO @QuestionId_Cursor   
    
   
			END     
			CLOSE QUESTION_CURSOR;    
			DEALLOCATE QUESTION_CURSOR; 


-- END CURSOR FOR QUESTIONS

FETCH NEXT FROM SECTION_CURSOR     
INTO @SectionId_Cursor   
    
   
END     
CLOSE SECTION_CURSOR;    
DEALLOCATE SECTION_CURSOR; 



Declare @TempSectionId numeric(18,0)
Declare @TempNewSectionId numeric(18,0)

DECLARE PSECTION_CURSOR CURSOR FOR     
SELECT SectionId, NewSectionId from @TEMPSections
OPEN PSECTION_CURSOR    
  
FETCH NEXT FROM PSECTION_CURSOR     
INTO @TempSectionId,    @TempNewSectionId

WHILE @@FETCH_STATUS = 0    
BEGIN    

UPDATE TSS_Sections set PSectionId = @TempNewSectionId where PSectionId = @TempSectionId and SurveyId = @NewSurveyId

FETCH NEXT FROM PSECTION_CURSOR     
INTO @TempSectionId,    @TempNewSectionId
    
   
END     
CLOSE PSECTION_CURSOR;    
DEALLOCATE PSECTION_CURSOR; 

select * from TSS_Sections where SurveyId = @NewSurveyId


COMMIT TRANSACTION
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION
END CATCH

END