-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE TSS_ManageSurveyDocuments
 @Filter NVARCHAR(50)
,@SurveyId NUMERIC(18,0)=NULL
,@ClientId NUMERIC(18,0)=NULL
,@CityId NUMERIC(18,0)=NULL
,@SurveyTitle NVARCHAR(50)=NULL
,@Description NVARCHAR(500)=NULL
,@CategoryId NUMERIC(18,0)=NULL
,@SubCategoryId NUMERIC(18,0)=NULL
,@UnitSystemId NUMERIC(18,0)=NULL
,@IsActive BIT=NULL
,@CreatedBy NUMERIC(18,0)=NULL
,@IsPublished BIT=0

AS
BEGIN
	DECLARE @RETURN_VALUE int = 0 
	IF @Filter='Insert'
	BEGIN
		DECLARE @SurveyCode NVARCHAR(10)= (SELECT ISNULL('CHK'+RIGHT('000000' + CONVERT(varchar(6),MAX(RIGHT(SurveyCode,6))+1),6),'CHK'+RIGHT('000000' + CONVERT(varchar(6),1),6)) FROM  TSS_SurveyDocuments) 

		INSERT INTO TSS_SurveyDocuments(ClientId,Cityid, SurveyTitle,DESCRIPTION,CategoryId,SubCategoryId,IsActive,UnitSystemId,CreatedOn,CreatedBy,IsPublished,SurveyCode)
		VALUES (@ClientId,@Cityid, @SurveyTitle,@Description,@CategoryId,@SubCategoryId,@IsActive,@UnitSystemId,GETDATE(),@CreatedBy,@IsPublished,@SurveyCode)
		
		SELECT @RETURN_VALUE = SCOPE_IDENTITY()	
	END
	
	ELSE IF @Filter='Update'
	BEGIN
		DECLARE @SurveyCodeNew NVARCHAR(10)=
		(
			SELECT SurveyCode FROM TSS_SurveyDocuments WHERE SurveyId=@SurveyId
        )
		IF ISNULL(@SurveyCodeNew,'0') = '0'
		BEGIN
			SET @SurveyCodeNew= (SELECT ISNULL('CHK'+RIGHT('000000' + CONVERT(varchar(6),MAX(RIGHT(SurveyCode,6))+1),6),'CHK'+RIGHT('000000' + CONVERT(varchar(6),1),6)) FROM  TSS_SurveyDocuments) 
		END

		UPDATE TSS_SurveyDocuments
		SET ClientId= @ClientId,Cityid= @Cityid,
		 SurveyTitle = @SurveyTitle,
		 [Description] = @Description,
		 CategoryId = @CategoryId,
		 SubCategoryId = @SubCategoryId,
		 IsActive = @IsActive,
		 UnitSystemId = @UnitSystemId,
		 IsPublished=@IsPublished,
		 SurveyCode=@SurveyCodeNew
		WHERE SurveyId=@SurveyId
		
		SELECT @RETURN_VALUE = @SurveyId
	END
	
	
	ELSE IF @Filter='Set_IsActive'
	BEGIN
		UPDATE TSS_SurveyDocuments
		SET IsActive = @IsActive
		WHERE SurveyId=@SurveyId
		
		SELECT @RETURN_VALUE = @SurveyId
	END
	
	
	RETURN @RETURN_VALUE;
END