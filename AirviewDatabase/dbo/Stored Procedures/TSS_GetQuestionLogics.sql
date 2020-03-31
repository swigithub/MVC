-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_GetQuestionLogics]
@Filter NVARCHAR(50),
@Value NVARCHAR(50)

AS
BEGIN
 
--	[dbo].[TSS_GetQuestionLogics] 'GET_BY_SECTIONID',60036
	IF @Filter='GET_BY_SECTIONID'
	BEGIN
		SELECT  l.LogicId, l.SurveyId, l.SectionId,l.FromQuestionId,l.ToQuestionId ,fq.Question 'FromQuestion',
		tq.Question 'ToQuestion', cond.DefinationName 'Condition',act.DefinationName 'Action',res.ResponseText 'Response' 	
		FROM TSS_QuestionLogics l
		INNER JOIN TSS_Questions AS fq ON fq.QuestionId=l.FromQuestionId
		INNER JOIN TSS_Responses AS res ON res.ResponseId=l.ResponseId
		INNER JOIN AD_Definations AS cond ON cond.DefinationId=l.ConditionId
		INNER JOIN AD_Definations AS act ON act.DefinationId=l.ActionId
		INNER JOIN TSS_Questions AS tq ON tq.QuestionId IN(SELECT ISNULL(x.Item,0) FROM dbo.SplitString(l.ToQuestionId,',') x)
		WHERE l.SectionId=@Value

		--DECLARE @ToQuestionId as nvarchar(50)=''

		--SELECT @ToQuestionId = COALESCE(@ToQuestionId + ',', '') + ToQuestionId FROM TSS_QuestionLogics where  SectionId=@Value

		--SELECT  l.LogicId, l.SurveyId, l.SectionId,l.FromQuestionId,l.ToQuestionId ,fq.Question 'FromQuestion',
		--tq.Question 'ToQuestion', cond.DefinationName 'Condition',act.DefinationName 'Action',res.ResponseText 'Response' 	
		--FROM TSS_QuestionLogics l
		--INNER JOIN TSS_Questions AS fq ON fq.QuestionId=l.FromQuestionId
		--INNER JOIN TSS_Responses AS res ON res.ResponseId=l.ResponseId
		--INNER JOIN AD_Definations AS cond ON cond.DefinationId=l.ConditionId
		--INNER JOIN AD_Definations AS act ON act.DefinationId=l.ActionId
		--INNER JOIN TSS_Questions AS tq ON tq.QuestionId=(SELECT x.Item FROM dbo.SplitString(l.ToQuestionId,',') x)
		--WHERE l.SectionId=@Value
		
		--SELECT ql.*,fq.Question 'FromQuestion',tq.Question 'ToQuestion' ,cond.DefinationName 'Condition',act.DefinationName 'Action',res.ResponseText 'Response'
		--FROM TSS_QuestionLogics AS ql
		--INNER JOIN TSS_Questions AS fq ON fq.QuestionId=ql.FromQuestionId
		--INNER JOIN TSS_Questions AS tq ON tq.QuestionId=ql.ToQuestionId
		--INNER JOIN TSS_Responses AS res ON res.ResponseId=ql.ResponseId
		--INNER JOIN AD_Definations AS cond ON cond.DefinationId=ql.ConditionId
		--INNER JOIN AD_Definations AS act ON act.DefinationId=ql.ActionId
		--WHERE ql.SectionId=@Value
	END
	
	
	--	[dbo].[TSS_GetQuestionLogics] 'GET_BY_SECTIONID',4
	ELSE IF @Filter='GET_BY_QUESTIONID'
	BEGIN
		--SELECT ql.*,fq.Question 'FromQuestion',tq.Question 'ToQuestion' ,cond.DefinationName 'Condition',act.DefinationName 'Action',res.ResponseText 'Response'
		--FROM TSS_QuestionLogics AS ql
		--INNER JOIN TSS_Questions AS fq ON fq.QuestionId=ql.FromQuestionId
		--INNER JOIN TSS_Questions AS tq ON tq.QuestionId=ql.ToQuestionId
		--INNER JOIN TSS_Responses AS res ON res.ResponseId=ql.ResponseId
		--INNER JOIN AD_Definations AS cond ON cond.DefinationId=ql.ConditionId
		--INNER JOIN AD_Definations AS act ON act.DefinationId=ql.ActionId
		--WHERE ql.FromQuestionId=@Value







		SELECT  l.LogicId, l.SurveyId, l.SectionId,l.FromQuestionId, l.ToQuestionId ,fq.Question 'FromQuestion',
		tq.Question 'ToQuestion', cond.DefinationName 'Condition',act.DefinationName 'Action',res.ResponseText 'Response' 	, ActionId, ConditionId, l.ResponseId
		FROM TSS_QuestionLogics l
		INNER JOIN TSS_Questions AS fq ON fq.QuestionId=l.FromQuestionId
		INNER JOIN TSS_Responses AS res ON res.ResponseId=l.ResponseId
		INNER JOIN AD_Definations AS cond ON cond.DefinationId=l.ConditionId
		INNER JOIN AD_Definations AS act ON act.DefinationId=l.ActionId
		INNER JOIN TSS_Questions AS tq ON tq.QuestionId IN(SELECT x.Item FROM dbo.SplitString(l.ToQuestionId,',') x)
		WHERE l.FromQuestionId=@Value
	END

	ELSE IF @Filter='GET_BY_QUESTIONID_QUESTIONLOGIC'
	BEGIN
		--SELECT ql.*,fq.Question 'FromQuestion',tq.Question 'ToQuestion' ,cond.DefinationName 'Condition',act.DefinationName 'Action',res.ResponseText 'Response'
		--FROM TSS_QuestionLogics AS ql
		--INNER JOIN TSS_Questions AS fq ON fq.QuestionId=ql.FromQuestionId
		--INNER JOIN TSS_Questions AS tq ON tq.QuestionId=ql.ToQuestionId
		--INNER JOIN TSS_Responses AS res ON res.ResponseId=ql.ResponseId
		--INNER JOIN AD_Definations AS cond ON cond.DefinationId=ql.ConditionId
		--INNER JOIN AD_Definations AS act ON act.DefinationId=ql.ActionId
		--WHERE ql.FromQuestionId=@Value







		SELECT  l.LogicId, l.SurveyId, l.SectionId,l.FromQuestionId,
		--l.ToQuestionId 
		tq.QuestionId as ToQuestionId,fq.Question 'FromQuestion',
		tq.Question 'ToQuestion', cond.DefinationName 'Condition',act.DefinationName 'Action',res.ResponseText 'Response' 	, ActionId, ConditionId, l.ResponseId
		FROM TSS_QuestionLogics l
		INNER JOIN TSS_Questions AS fq ON fq.QuestionId=l.FromQuestionId
		INNER JOIN TSS_Responses AS res ON res.ResponseId=l.ResponseId
		INNER JOIN AD_Definations AS cond ON cond.DefinationId=l.ConditionId
		INNER JOIN AD_Definations AS act ON act.DefinationId=l.ActionId
		INNER JOIN TSS_Questions AS tq ON tq.QuestionId IN(SELECT x.Item FROM dbo.SplitString(l.ToQuestionId,',') x)
		WHERE l.FromQuestionId=@Value
	END

	--add by junaid
	ELSE IF @Filter='GET_ToQuestionIds_BY_SECTIONID'
	BEGIN
		select * 
		from TSS_QuestionLogics where  LogicID = @Value
	END

END