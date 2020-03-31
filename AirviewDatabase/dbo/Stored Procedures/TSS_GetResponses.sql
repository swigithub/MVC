-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_GetResponses]
@Filter NVARCHAR(50)
,@Value NVARCHAR(500)
AS
BEGIN
	IF @Filter='GET_BY_QUESTIONID'
		BEGIN
			SELECT * --, logic.ConditionId, logic.ResponseId, logic.ActionId
			FROM TSS_Responses AS res

			--inner join TSS_QuestionLogics logic on res.QuestionId = logic.ToQuestionId
			WHERE res.QuestionId=@Value
		END
	ELSE IF @Filter='GET_RESPONSE_BY_SITEQUESTION'
	BEGIN
		--{"ResponseId":60089,"QuestionId":70059,"SiteQuestionId":42031,"ResponseText":"Beta Mount & View","ResponseValue":"","SortOrder":0,"IsPassed":false,"MinValue":0,"MaxValue":0,
		--"IsGps":false,"IsActive":true,"ConditionId":0,"ActionId":0}
		--SELECT res.ResponseId, res.QuestionId, res.ResponseText, res.ResponseValue,
		--       res.SortOrder, res.IsPassed, res.MinValue, res.MaxValue, res.IsGps,
		--       res.IsActive, tsq.SiteQuestionId, tsq.SiteSurveyId,
		--       tsq.SiteSectionId,
		       
		--       tsq.QuestionId, tsq.IsRequired,
		--       tsq.IsNoteRequired, tsq.IsImageRequired, tsq.IsBarCodeRequired,
		--       tsq.IsAnswered
		--FROM TSS_Responses AS res
		--INNER JOIN TSS_SiteQuestions AS tsq ON tsq.QuestionId=res.QuestionId
		--LEFT JOIN TSS_SiteResponses AS tsr ON tsr.SiteQuestionId=tsq.SiteQuestionId AND tsr.ResponseId=res.ResponseId		
		--WHERE tsq.SiteQuestionId=@Value
		----UNION ALL
		----SELECT res.ResponseId, tsq.QuestionId, res.ResponseText, res.ResponseValue,
		----	   0 'SorOrder', CAST(1 AS BIT) 'IsPassed', res.MinValue, res.MaxValue, res.IsGps,
		----	   CAST(1 AS BIT) 'IsActive', res.SiteQuestionId, tsq.SiteSurveyId,
		----	   tsq.SiteSectionId,	       
		       
		----       tsq.QuestionId, tsq.IsRequired, tsq.IsNoteRequired,
		----       tsq.IsImageRequired, tsq.IsBarCodeRequired, tsq.IsAnswered
		----FROM TSS_SiteResponses AS res
		----INNER JOIN TSS_SiteQuestions AS tsq ON tsq.SiteQuestionId=res.SiteQuestionId
		----WHERE tsq.SiteQuestionId=@Value
		
		SELECT res.ResponseId, res.QuestionId, res.ResponseText, res.ResponseValue,
		       res.SortOrder, res.IsPassed, res.MinValue, res.MaxValue, res.IsGps,
		       res.IsActive, tsq.SiteQuestionId, tsq.SiteSurveyId,
		       tsq.SiteSectionId,
		       
		       tsq.QuestionId, tsq.IsRequired,
		       tsq.IsNoteRequired, tsq.IsImageRequired, tsq.IsBarCodeRequired,
		       CASE WHEN (SELECT COUNT(tsr.SiteQuestionId) FROM TSS_SiteResponses AS tsr WHERE tsr.SiteQuestionId=tsq.SiteQuestionId AND tsr.ResponseId=res.ResponseId)>0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END 'IsAnswered'
		FROM TSS_Responses AS res
		INNER JOIN TSS_SiteQuestions AS tsq ON tsq.QuestionId=res.QuestionId
		--LEFT JOIN TSS_SiteResponses AS tsr ON tsr.SiteQuestionId=tsq.SiteQuestionId AND tsr.ResponseId=res.ResponseId		
		WHERE tsq.SiteQuestionId=@Value
	END
END