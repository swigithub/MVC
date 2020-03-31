-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>

-- =============================================
CREATE PROCEDURE [dbo].[TSS_ManageQuestions] 
@Filter NVARCHAR(50)
,@QuestionId numeric(18, 0)=NULL
,@SectionId numeric(18, 0)=NULL
,@QuestionTypeId numeric(18, 0)=NULL
,@Question nvarchar(250)=NULL
,@Description nvarchar(500)=NULL
,@Weightage float=NULL
,@SortOrder INT=NULL
,@IsRequired bit=NULL
,@IsNoteRequired bit=NULL
,@IsImageRequired bit=NULL
,@IsBarCodeRequired bit=NULL
,@IsRepeatable bit=NULL
--,@CreatedOn DATETIME
,@CreatedBy  numeric(18, 0)=NULL
,@UnitSystemId numeric(18, 0)=NULL
,@UnitTypeId numeric(18, 0)=NULL
,@UnitId numeric(18, 0)=NULL
,@IsActive bit=NULL
,@isVerificationRequired bit = NULL
,@isVideoRequired bit = NULL
,@isAudioRequired bit = NULL
,@isDocumentRequired bit = NULL
,@TotalColumn int=0
,@TotalRows int=0
,@DynamicRows bit=0,
@IsImageDetailRequired bit=0,
@IsMultiLocation bit=0,
@SurveyEntity varchar(50)=''
,@Prefix varchar(50)=''
,@List TSS_QuestionResponses READONLY

AS
BEGIN
	IF @Filter='Insert'
	BEGIN
		-- Getting Max Sort Order Index 

		SELECT @SortOrder=ISNULL(MAX(SortOrder)+1,0) FROM TSS_Questions WHERE SectionId=@SectionId

		INSERT INTO TSS_Questions 
		VALUES (@SectionId,@QuestionTypeId,@Question,@Description ,@Weightage ,@SortOrder ,@IsRequired ,@IsNoteRequired ,@IsImageRequired ,@IsBarCodeRequired ,GETDATE(),@CreatedBy,@IsRepeatable,@UnitTypeId, @UnitId,@UnitSystemId,1,@isVerificationRequired,@isVideoRequired,@isAudioRequired,@isDocumentRequired,@TotalColumn,@TotalRows,@DynamicRows,0,@IsImageDetailRequired,@IsMultiLocation,@SurveyEntity,@Prefix)
		DECLARE @LastId AS NUMERIC(18,0)= SCOPE_IDENTITY()
		
		IF @QuestionTypeId IN(63293,63294,103297,103298,103299,223547)
		BEGIN
			INSERT INTO TSS_Responses 
			SELECT @LastId,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,Value10,Value11
			FROM @List AS l
		END
	END
	IF @Filter='Update'
	BEGIN
		UPDATE TSS_Questions
		SET
			-- QuestionId -- this column value is auto-generated
			SectionId = @SectionId,
			QuestionTypeId = @QuestionTypeId,
			Question = @Question,
			[Description] = @Description,
			Weightage = @Weightage,
			SortOrder = @SortOrder,
			IsRequired = @IsRequired,
			IsNoteRequired = @IsNoteRequired,
			IsImageRequired = @IsImageRequired,
			IsBarCodeRequired = @IsBarCodeRequired,
			UnitTypeId= @UnitTypeId,
			UnitId= @UnitId,
			UnitSystemId= @UnitSystemId,
			IsActive= @IsActive,
			IsVerificationRequired = @isVerificationRequired,
			IsVideoRequired = @isVideoRequired,
			IsAudioRequired = @isAudioRequired,
			IsDocumentRequired = @isDocumentRequired,
			TotalColumn=@TotalColumn,
			TotalRows=@TotalRows,
			DynamicRows=@DynamicRows,
			IsImageDetailRequired=@IsImageDetailRequired,
			IsMultiLocation=@IsMultiLocation,
			SurveyEntity=@SurveyEntity,
			Prefix=@Prefix
		WHERE QuestionId=@QuestionId
		
		Delete From TSS_Responses where QuestionId=@QuestionId

		IF @QuestionTypeId IN(63293,63294,103297,103298,103299,223547)
		BEGIN
			INSERT INTO TSS_Responses 
					   (QuestionId,ResponseText,ResponseValue,SortOrder,IsPassed ,MinValue,MaxValue,IsGps,IsActive,IsReadOnly,UserValues)
				SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,Value10,Value11 
			FROM @List AS l
		END
	END
	
	else IF @Filter='Delete'
	BEGIN
		DELETE FROM TSS_Questions
		WHERE QuestionId=@QuestionId
	END


	ELSE IF @Filter='UpdateStatus'
	BEGIN
		UPDATE TSS_Questions SET IsActive= @IsActive,IsRequired=@IsRequired
		WHERE QuestionId=@QuestionId
	END
	ELSE IF @Filter='Save_Questions_SortOrder'
	BEGIN
		
		DECLARE @QuestionIdSort NUMERIC(18,0),@SortOrder_ForSort INT;

		DECLARE SORT_CURSOR CURSOR FOR 
		SELECT Value1,Value2 FROM @List

		OPEN SORT_CURSOR

		FETCH NEXT FROM SORT_CURSOR INTO @QuestionIdSort,@SortOrder_ForSort

		WHILE @@FETCH_STATUS=0
		BEGIN
			
			UPDATE TSS_Questions SET SortOrder=@SortOrder_ForSort WHERE QuestionId=@QuestionIdSort

			FETCH NEXT FROM SORT_CURSOR INTO @QuestionIdSort,@SortOrder_ForSort
		END

		CLOSE SORT_CURSOR
		DEALLOCATE SORT_CURSOR

	END
END