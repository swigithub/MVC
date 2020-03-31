-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_GetRequiredActions]
@Filter NVARCHAR(50)
,@Value NVARCHAR(500)
AS
BEGIN
	--IF @Filter='GET_BY_QUESTIONID'
	--	BEGIN
	--		SELECT * --, logic.ConditionId, logic.ResponseId, logic.ActionId
	--		FROM TSS_Responses AS res

	--		--inner join TSS_QuestionLogics logic on res.QuestionId = logic.ToQuestionId
	--		WHERE res.QuestionId=@Value
	--	END
	--ELSE 
	
	IF @Filter='GET_REQACTION_BY_SITEQUESTION'
	BEGIN
		SELECT reqA.ActionId,	reqA.SiteId,	reqA.SiteSurveyId,	reqA.SiteQuestionId,	reqA.IterationId,	reqA.PIterationId,	reqA.ActionTypeId,
			reqA.ActionValue 'RequiredAction',	reqA.Remarks
		FROM TSS_RequiredActions AS reqA		
		INNER JOIN TSS_SiteQuestions AS tsq ON tsq.SiteQuestionId=reqA.SiteQuestionId
		WHERE tsq.SiteQuestionId=@Value
	END
END