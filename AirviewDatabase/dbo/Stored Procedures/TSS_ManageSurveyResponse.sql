CREATE PROCEDURE [dbo].[TSS_ManageSurveyResponse]
@Filter NVARCHAR(50)
,@List TSS_ResponsesList READONLY	
AS
BEGIN
	DECLARE @SiteId AS NUMERIC(18,0)=0
	DECLARE @SurveyId AS NUMERIC(18,0)=0
	DECLARE @SectionId AS NUMERIC(18,0)=0
	DECLARE @QuestionId AS NUMERIC(18,0)=0
	DECLARE @ResponseId AS NUMERIC(18,0)=0
	
	DECLARE @IterationId AS NUMERIC(18,0)=0
	DECLARE @ResponseText AS NVARCHAR(max)=''
	DECLARE @pIterationId AS NUMERIC(18,0)=0
	DECLARE @MinValue AS FLOAT=0
	DECLARE @MaxValue AS FLOAT=0
	DECLARE @IsGPS AS BIT=0
	DECLARE @Signature AS nvarchar(MAX)=null
	DECLARE @MapImage AS nvarchar(MAX)=null
	DECLARE @MapZoom as int=17;
	DECLARE @AzimuthForMap as numeric(18,2)=0;
	
	DECLARE @ActionTypeId AS NUMERIC=0
	DECLARE @ActionValue AS NVARCHAR(MAX)=''
	DECLARE @Remarks AS NVARCHAR(50)=''
	DECLARE @ResponseValue nvarchar(max) = ''
	DECLARE @Latitude varchar(50)
	DECLARE @Longitude varchar(50)
	DECLARE @Azimuth varchar(50)
	DECLARE @ObjectView varchar(50)
	DECLARE @Altitude varchar(50)
	DECLARE @GpsAccuracy varchar(50)

	--{"SiteId":538478,"SurveyId":0,"SectionId":40073,"QuestionId":"42277","ResponseId":"0","IterationId":1,"ResponseText":"vh12qhh","pIterationId":"40073","min":0,"max":0,"isGPS":1,"parentId":"40073"}

	
	IF @Filter='SurveyResponse'
	BEGIN
		--"Value1", res.SiteId, "Value2", res.SurveyId, "Value3", res.SectionId, "Value4", res.QuestionId, "Value5", res.ResponseId
		
		DECLARE @RepeatCount AS INT=(SELECT COUNT(DISTINCT Value6) FROM @List)
		DECLARE @SiteQuestionId NUMERIC(18,0)=(SELECT TOP(1) Value4 From @List)


		DECLARE @GetQuestionType NVARCHAR(50)=		
		(SELECT DefinationName FROM AD_Definations WHere DefinationId in (SELECT QuestionTypeId FROM TSS_SiteQuestions tsq
		INNER JOIN  TSS_Questions tq ON tq.QuestionId=tsq.QuestionId
		WHERE SiteQuestionId=@SiteQuestionId))

		IF @GetQuestionType='Multi Select'
		BEGIN
		DELETE FROM TSS_SiteResponses
		WHERE SiteQuestionId=@SiteQuestionId
		END
		--SELECT * from @List
		
		DECLARE db_cluster2 CURSOR FOR  
		SELECT res.Value1, res.Value2, res.Value3, res.Value4,res.Value5,res.Value6, res.Value7, res.Value8, res.Value9, res.Value10, res.Value11, res.Value12,res.Value13,res.Value14,res.Value15,res.Value16 FROM @List res
		OPEN db_cluster2 
		FETCH NEXT FROM db_cluster2 INTO @SiteId, @SurveyId, @SectionId, @QuestionId, @ResponseId, @IterationId, @ResponseText, @pIterationId, @MinValue, @MaxValue, @IsGPS,@ResponseValue,@MapZoom,@MapImage,@Signature,@AzimuthForMap
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			--- Saving Map Images
			IF @MapImage <> '' 
			BEGIN
				UPDATE TSS_SiteQuestions SET MapImage=@MapImage,MapZoom=@MapZoom,Azimuth=@AzimuthForMap
				 where SiteQuestionId=@QuestionId and SiteSectionId=@SectionId and SiteSurveyId=@SurveyId
			END
			-- End Saving Map Images
			UPDATE TSS_SiteSections
			SET RepeatCount = CASE WHEN @RepeatCount>1 THEN @RepeatCount ELSE 0 END
			WHERE SiteSurveyId=@SurveyId AND SiteSectionId=@SectionId
			
			--SELECT @SurveyId,@SectionId
			   
			IF ISNULL(@ResponseId,0)=0
			BEGIN
				DELETE FROM TSS_SiteResponses
				WHERE SiteQuestionId=@QuestionId;	
			END
			ELSE
			BEGIN
				DELETE FROM TSS_SiteResponses
				WHERE SiteQuestionId=@QuestionId AND ResponseId=@ResponseId;
			END
			
			UPDATE TSS_SiteQuestions set IsAnswered = 1 where SiteQuestionId = @QuestionId and SiteSurveyId = @SurveyId
			-- NOTE : If Questions have  Answered Count Problem From Mobile .Please Verify SurveyId. 
			INSERT INTO TSS_SiteResponses(SiteQuestionId,ResponseId,ResponseText,ResponseValue,IterationId,PIterationId,MinValue,MaxValue,IsGps,Signature)
			SELECT @QuestionId,(CASE WHEN @ResponseId>0 THEN @ResponseId ELSE NULL END),
			(CASE WHEN @ResponseId=0 THEN null ELSE @ResponseText END),
			CASE WHEN @MapImage <> '' THEN @ResponseValue ELSE
			(CASE WHEN @ResponseId=0 THEN @ResponseText ELSE @ResponseValue END) END
			,@IterationId,@pIterationId,@MinValue,@MaxValue,ISNULL(@IsGPS,0),@Signature
			
		FETCH NEXT FROM db_cluster2 INTO @SiteId, @SurveyId, @SectionId, @QuestionId, @ResponseId, @IterationId, @ResponseText, @pIterationId, @MinValue, @MaxValue, @IsGPS,@ResponseValue,@MapZoom,@MapImage,@Signature,@AzimuthForMap
		END   
		CLOSE db_cluster2   
		DEALLOCATE db_cluster2
	END
	
	else IF @Filter='SurveyAction'
	BEGIN
		-- "Value1", res.SiteId, "Value2", res.SurveyId, "Value3", res.SectionId, "Value4", res.QuestionId, "Value5", res.ActionType, "Value6", res.ActionValue
		--SurveyActions:[{"SiteId":538478,"SurveyId":0,"SectionId":40069,"QuestionId":"0","ActionType":"99","ActionValue":"\/UTSS01\/MY_SITE_AUDIT_SURVEY\/BTS\/UTSS01_MY_SITE_AUDIT_SURVEY_BTS_signature_2018-02-23 03-10-09858.jpg","Remarks":"","IterationId":1,"pIterationId":"0"},
		--{"SiteId":538478,"SurveyId":0,"SectionId":40069,"QuestionId":"42121","ActionType":"1","ActionValue":"jello","Remarks":"","IterationId":1,"pIterationId":"0"}]
		
		--value1: siteid, 2: surveyid, 3: sectionid, 4: questionid, 5: actiontypeid, 6: remarks, 7: iterationid, 8:piterationid, 15:actionvalue, 
		-- First Delete All 
		DELETE FROM TSS_RequiredActions WHERE SiteQuestionId IN (SELECT Value4 from @List)
		-- Then Insert All
		INSERT INTO TSS_RequiredActions
				(SiteId,SiteSurveyId,SiteQuestionId,IterationId,PIterationId,ActionTypeId,ActionValue,Remarks,Latitude,Longitude,Azimuth,ObjectView,Altitude,GPSAccuracy)
		SELECT	Value1,Value2 ,Value4,Value7,Value8,Value5,Value15,Value6,Value9,Value10,Value11,Value12,Value13,Value14
		From @List						
		
		SELECT * FROM @List

		--DECLARE db_cluster3 CURSOR FOR  
		--SELECT res.Value1, res.Value2, res.Value3, res.Value4,res.Value5,res.Value6, res.Value7, res.Value8,res.Value9,res.Value10,res.Value11,res.Value12,res.Value13,res.Value14, res.Value15 FROM @List res
		--OPEN db_cluster3 
		--FETCH NEXT FROM db_cluster3 INTO @SiteId, @SurveyId, @SectionId, @QuestionId, @ActionTypeId, @Remarks, @IterationId, @pIterationId,@Latitude,@Longitude,@Azimuth,@ObjectView,@Altitude,@GpsAccuracy, @ActionValue
		--WHILE @@FETCH_STATUS = 0   
		--BEGIN
		--	IF @QuestionId>0
		--	BEGIN
		--		SELECT '1',@SiteId, @SurveyId, @SectionId, @QuestionId, @ActionTypeId, @Remarks, @IterationId, @pIterationId, @ActionValue
		--		DELETE FROM TSS_RequiredActions
		--		WHERE SiteQuestionId=@QuestionId AND ActionTypeId=@ActionTypeId AND IterationId=@IterationId AND PIterationId=@pIterationId
		--		--AND ActionValue=@ActionValue;
		--	END
		--	ELSE
		--	BEGIN
		--		SELECT '2',@SiteId, @SurveyId, @SectionId, @QuestionId, @ActionTypeId, @Remarks, @IterationId, @pIterationId, @ActionValue
		--		DELETE FROM TSS_RequiredActions
		--		WHERE ActionTypeId=@ActionTypeId AND IterationId=@IterationId AND PIterationId=@pIterationId;
		--	END
			
		--	INSERT INTO TSS_RequiredActions(SiteId,SiteSurveyId,SiteQuestionId,IterationId,PIterationId,ActionTypeId,ActionValue,Remarks,Latitude,Longitude,Azimuth,ObjectView,Altitude,GPSAccuracy)			
		--	SELECT @SiteId,@SurveyId,@QuestionId,@IterationId,@pIterationId,@ActionTypeId,
		--	@ActionValue,
		--	--CASE WHEN LEFT(@ActionValue,7)='Content' THEN '/'+@ActionValue ELSE @ActionValue END,
		--	@Remarks,@Latitude,@Longitude,@Azimuth,@ObjectView,@Altitude,@GpsAccuracy
		--FETCH NEXT FROM db_cluster3 INTO @SiteId, @SurveyId, @SectionId, @QuestionId, @ActionTypeId, @Remarks, @IterationId, @pIterationId,@Latitude,@Longitude,@Azimuth,@ObjectView,@Altitude,@GpsAccuracy, @ActionValue
		--END   
		--CLOSE db_cluster3   
		--DEALLOCATE db_cluster3
	END
	
END