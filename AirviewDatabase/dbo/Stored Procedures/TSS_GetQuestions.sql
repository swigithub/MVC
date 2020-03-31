-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_GetQuestions]
@Filter NVARCHAR(50)
,@Value NVARCHAR(50)=NULL
,@surveyId int=NULL
AS
BEGIN
-- [dbo].[TSS_GetQuestions]	'GET_BY_SECTIONID',10047
	IF @Filter='GET_BY_SECTIONID'
	BEGIN
		SELECT tq.*,def.DefinationName 'QuestionType',Convert(bit,Case When tq.QuestionId in (Select Distinct FromQuestionId from TSS_QuestionLogics) then 1 else 0 end) as IsLogicExists
		FROM TSS_Questions AS tq
		INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
		WHERE tq.SectionId=@Value
		
	END
	ELSE IF @Filter='GET_Questions_BY_SECTIONID'
	BEGIN
	SELECT tq.*,tsq.SiteQuestionId,case when tr.ResponseId > 0 then 0 else 1 end [IsQuestion],tr.*,def.DefinationName 'QuestionType', tsq.IsInclude
		FROM TSS_Questions AS tq
		INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
		INNER JOIN TSS_SiteQuestions AS tsq ON tsq.QuestionId=tq.QuestionId and tsq.SiteSectionId = @Value and tsq.SiteSurveyId = @surveyId
		--INNER JOIN TSS_SiteSections AS tss ON TSS.SiteSectionId = TSQ.SiteSectionId
		LEFT JOIN TSS_Responses tr on tq.QuestionId =tr.QuestionId  --where tsq.IsInclude = 1
	END
	ELSE IF @Filter='GET_Questions_BY_SECTIONID_For_Dashboard'
	BEGIN
	SELECT Distinct tq.*,tsq.SiteQuestionId,case when tr.ResponseId > 0 then 0 else 1 end [IsQuestion],tsq.SiteSectionId,
		tr.ResponseId , tr.QuestionId, tr.ResponseText, 
		case when    (ISNULL(tsr.ResponseId,0)=0) then (case when tsr.ResponseText is null and tsr.ResponseValue is null then tr.ResponseValue else  tsr.ResponseValue end) 
		 else tr.ResponseValue end as ResponseValue,
		tr.SortOrder, tr.IsPassed, tr.MinValue, 
		 tr.MaxValue, tr.IsGps, tr.IsActive, tr.IsReadOnly, tr.UserValues,
		 def.DefinationName 'QuestionType'--,tsr.ResponseValue as SelectedRawResponse
		, Convert(bit,(case when (select Count(*) from TSS_SiteResponses where tr.ResponseId=TSS_SiteResponses.ResponseId and Tsq.SiteQuestionId=TSS_SiteResponses.SiteQuestionId )>0 then 1 
		else (case when isnull(tsr.ResponseId,0)=0 and def.DefinationName = 'Table'  then 1 else 0 end)
	    end )) as IsChecked,
		(
		case when def.DefinationName = 'Table' and tsr.ResponseId is not null  
		then (select Top 1  ResponseValue from TSS_SiteResponses 
			  where ResponseId = tr.ResponseId and SiteQuestionId = tsq.SiteQuestionId )
		else '' end
		)as [SelectedRawResponse]
		,tsq.IsInclude
		FROM TSS_Questions AS tq
		INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
		INNER JOIN TSS_SiteQuestions AS tsq ON tsq.QuestionId=tq.QuestionId and tsq.SiteSectionId = @Value and tsq.SiteSurveyId = @surveyId
		LEFT JOIN TSS_Responses tr on tq.QuestionId =tr.QuestionId
		LEFT JOIN TSS_SiteResponses tsr on tsr.SiteQuestionId=tsq.SiteQuestionId 
		where tq.IsActive = 1
		--and tsr.ResponseId=tr.ResponseId
		
	END
	ELSE IF @Filter='GET_Questions_BY_SECTIONID_API'
	BEGIN
	Declare @tempSiteSurveyid int = (select top 1 SiteSurveyId from TSS_SiteSections where SiteSectionId = @Value);
	SELECT Distinct tq.Question,tq.QuestionTypeId,tq.IsRepeatable,tq.IsRequired,tq.IsImageRequired
		  ,tq.IsAudioRequired,tq.IsBarCodeRequired,tq.IsDocumentRequired,tq.IsNoteRequired,tq.Description,
		  tq.DynamicRows,tq.DynamicRowsCount,
		   tq.IsActive,tq.IsVerificationRequired,tq.IsVideoRequired,tq.TotalColumn,tq.TotalRows,tq.Weightage,@Value as SectionId,
		   tsq.SiteQuestionId,case when tr.ResponseId > 0 then 0 else 1 end [IsQuestion],
		   case when def.DefinationName='Range' then 
		  Convert(varchar(50),tr.MinValue)+','+ Convert(varchar(50),tr.MaxValue)
		  else tr.UserValues end as UserValues,
		   case when tsr.MaxValue <> 0 then tsr.MaxValue else tr.MaxValue end as MaxValue,
		   case when tsr.MinValue <> 0 then tsr.MinValue else tr.MinValue end as MinValue,
		   tr.IsReadOnly,tsq.QuestionId,tr.IsGps,def.DefinationName 'QuestionType',
		    tr.ResponseText, tr.ResponseId,
			CASE WHEN def.DefinationName='Rating' THEN tsr.ResponseValue
		 WHEN def.DefinationName='Scale' THEN tsr.ResponseValue 
		 WHEN def.DefinationName='DateTime' THEN tsr.ResponseValue
		 ELSE (
		case when    (ISNULL(tsr.ResponseId,0)=0) then (case when tsr.ResponseText is null and tsr.ResponseValue is null then tr.ResponseValue else  tsr.ResponseValue end) 
		 else tr.ResponseValue end ) end as ResponseValue,
		  Convert(bit,(case when (select Count(*) from TSS_SiteResponses where tr.ResponseId=TSS_SiteResponses.ResponseId and Tsq.SiteQuestionId=TSS_SiteResponses.SiteQuestionId )>0 then 1 
		else (case when isnull(tsr.ResponseId,0)=0 and def.DefinationName = 'Table'  then 1 else 0 end)
	    end )) as IsChecked,
		(
		case when def.DefinationName = 'Table' and tsr.ResponseId is not null  
		then (select Top 1  ResponseValue from TSS_SiteResponses 
			  where ResponseId = tr.ResponseId and SiteQuestionId = tsq.SiteQuestionId )
		else '' end
		) as [SelectedRawResponse],
		tq.SortOrder,tsq.MapImage,tsr.Signature,tq.IsImageDetailRequired,IsMultiLocation,isnull(tsq.MapZoom,0) as MapZoom
		,tq.SurveyEntity,tsq.Azimuth,tq.Prefix
		FROM TSS_Questions AS tq
		INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
		INNER JOIN TSS_SiteQuestions AS tsq ON tsq.QuestionId=tq.QuestionId and tsq.SiteSectionId = @Value and tsq.SiteSurveyId = @tempSiteSurveyid
		LEFT JOIN TSS_Responses tr on tq.QuestionId =tr.QuestionId
		LEFT JOIN TSS_SiteResponses tsr on tsr.SiteQuestionId=tsq.SiteQuestionId 
		ORDER BY tsq.SiteQuestionId, tr.ResponseId asc
	END
	ELSE IF @Filter='GET_Questions_BY_SECTIONID_For_Preview'
	BEGIN
	SELECT Distinct tq.*,tsq.SiteQuestionId,case when tr.ResponseId > 0 then 0 else 1 end [IsQuestion],
		tr.ResponseId , tr.QuestionId, tr.ResponseText, 
		CASE WHEN def.DefinationName='Rating' THEN tsr.ResponseValue
		 WHEN def.DefinationName='Scale' THEN tsr.ResponseValue 
		 WHEN def.DefinationName='DateTime' THEN tsr.ResponseValue
		 ELSE (
		case when    (ISNULL(tsr.ResponseId,0)=0) then (case when tsr.ResponseText is null and tsr.ResponseValue is null then tr.ResponseValue else  tsr.ResponseValue end) 
		 else tr.ResponseValue end) end as ResponseValue,
		tr.SortOrder, tr.IsPassed,
			case when tsr.MaxValue <> 0 then tsr.MaxValue else tr.MaxValue end as MaxValue,
		   case when tsr.MinValue <> 0 then tsr.MinValue else tr.MinValue end as MinValue,
		  tr.IsGps, tr.IsActive, tr.IsReadOnly,
		  case when def.DefinationName='Range' then 
		  Convert(varchar(50),tr.MinValue)+','+ Convert(varchar(50),tr.MaxValue)
		  else tr.UserValues end as UserValues,
		 def.DefinationName 'QuestionType'--,tsr.ResponseValue as SelectedRawResponse
		, Convert(bit,(case when (select Count(*) from TSS_SiteResponses where tr.ResponseId=TSS_SiteResponses.ResponseId and Tsq.SiteQuestionId=TSS_SiteResponses.SiteQuestionId )>0 then 1 
		else (case when isnull(tsr.ResponseId,0)=0 and def.DefinationName = 'Table'  then 1 else 0 end)
	    end )) as IsChecked,
		(
		case when def.DefinationName = 'Table' and tsr.ResponseId is not null  
		then (select Top 1  ResponseValue from TSS_SiteResponses 
			  where ResponseId = tr.ResponseId and SiteQuestionId = tsq.SiteQuestionId )
		else '' end
		)as [SelectedRawResponse],
		tsr.Signature,isnull(tsq.MapZoom,0) MapZoom,tq.SurveyEntity,tsq.Azimuth,tsq.MapImage
		FROM TSS_Questions AS tq
		INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
		INNER JOIN TSS_SiteQuestions AS tsq ON tsq.QuestionId=tq.QuestionId and tsq.SiteSectionId = @Value and tsq.SiteSurveyId = @surveyId
		LEFT JOIN TSS_Responses tr on tq.QuestionId =tr.QuestionId
		LEFT JOIN TSS_SiteResponses tsr on tsr.SiteQuestionId=tsq.SiteQuestionId --and tsr.ResponseId=tr.ResponseId
		where tsq.IsInclude = 1 and tq.IsActive = 1
		

	END
	ELSE IF @Filter='GET_QUESIONS_REQUIRED_ACTIONS_BY_SECTIONID'
	BEGIN
			select tra.*,tra.ActionValue as [RequiredAction] from TSS_RequiredActions tra
			inner join TSS_SiteQuestions ts on ts.SiteSectionId=@Value and ts.SiteQuestionId = tra.SiteQuestionId
	END
	ELSE IF @Filter='GET_QUESTION_BY_SECTION'
	BEGIN
	Declare @tempSurveyid int = (select top 1 SiteSurveyId from TSS_SiteSections where SiteSectionId = @Value);
		SELECT tq.*,tsq.SiteQuestionId,def.DefinationName 'QuestionType'
		FROM TSS_Questions AS tq
		INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
		INNER JOIN TSS_SiteQuestions AS tsq ON tsq.QuestionId=tq.QuestionId and tsq.SiteSectionId = @Value and tsq.SiteSurveyId = @tempSurveyid
	--SELECT tsq.SiteQuestionId 'QuestionId', tsq.SiteSectionId 'SectionId', tq.QuestionTypeId, tq.Question,
	--	       tq.[Description], tq.Weightage, tq.SortOrder, tq.IsRequired,
	--	       tq.IsNoteRequired, tq.IsImageRequired, tq.IsBarCodeRequired,
	--	       tq.CreatedOn, tq.CreatedBy, tq.IsRepeatable, tq.UnitTypeId,
	--	       tq.UnitId, tq.UnitSystemId, tq.IsActive,def.DefinationName 'QuestionType'
	--	FROM TSS_Questions AS tq
	--	INNER JOIN TSS_SiteSections AS ts ON ts.SectionId=tq.SectionId
	--	INNER JOIN TSS_SiteQuestions AS tsq ON tsq.SiteSectionId=ts.SiteSectionId AND tsq.QuestionId=tq.QuestionId and ts.SiteSurveyId = @tempSiteSurveyid
	--	INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
	--	WHERE ts.SiteSectionId=@Value 
		--SELECT tq.*,def.DefinationName 'QuestionType'
		--FROM TSS_Questions AS tq
		--INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
		--WHERE tq.SectionId=@Value AND tq.IsRepeatable=0
		--UNION ALL

		--SELECT tq.*,def.DefinationName 'QuestionType'
		--FROM TSS_Questions AS tq
		--INNER JOIN TSS_Sections AS ts ON ts.SectionId=tq.SectionId
		--INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
		--WHERE ts.PSectionId=@Value AND tq.IsRepeatable=1
		--Backup Store Procedure
		--SELECT tsq.SiteQuestionId 'QuestionId', tsq.SiteSectionId 'SectionId', tq.QuestionTypeId, tq.Question,
		--       tq.[Description], tq.Weightage, tq.SortOrder, tq.IsRequired,
		--       tq.IsNoteRequired, tq.IsImageRequired, tq.IsBarCodeRequired,
		--       tq.CreatedOn, tq.CreatedBy, tq.IsRepeatable, tq.UnitTypeId,
		--       tq.UnitId, tq.UnitSystemId, tq.IsActive,def.DefinationName 'QuestionType'
		--FROM TSS_Questions AS tq
		--INNER JOIN TSS_SiteSections AS ts ON ts.SectionId=tq.SectionId
		--INNER JOIN TSS_SiteQuestions AS tsq ON tsq.SiteSectionId=ts.SiteSectionId AND tsq.QuestionId=tq.QuestionId
		--INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
		--WHERE ts.SiteSectionId=@Value AND tq.IsRepeatable=1
		--UNION ALL
		--SELECT tsq.SiteQuestionId 'QuestionId', tsq.SiteSectionId 'SectionId', tq.QuestionTypeId, tq.Question,
		--       tq.[Description], tq.Weightage, tq.SortOrder, tq.IsRequired,
		--       tq.IsNoteRequired, tq.IsImageRequired, tq.IsBarCodeRequired,
		--       tq.CreatedOn, tq.CreatedBy, tq.IsRepeatable, tq.UnitTypeId,
		--       tq.UnitId, tq.UnitSystemId, tq.IsActive,def.DefinationName 'QuestionType'
		--FROM TSS_Questions AS tq
		--INNER JOIN TSS_SiteSections AS ts ON ts.SectionId=tq.SectionId
		--INNER JOIN TSS_SiteQuestions AS tsq ON tsq.SiteSectionId=ts.SiteSectionId AND tsq.QuestionId=tq.QuestionId
		--INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
		--WHERE ts.SiteSectionId=@Value AND tq.IsRepeatable=0

		
	END
	
	
	-- [dbo].[TSS_GetQuestions]	'GET_BY_SECTIONID',10047
else	IF @Filter='GET_BY_QUESTIONID'
	BEGIN
		SELECT tq.*,def.DefinationName 'QuestionType'
		FROM TSS_Questions AS tq
		INNER JOIN AD_Definations AS def ON def.DefinationId=tq.QuestionTypeId
		WHERE tq.QuestionId=@Value
	END
END
--select * from TSS_SiteSurvey where SiteSurveyId =70023
--select * from TSS_SiteSections where SiteSurveyId = 70023
--select * from TSS_Sections where SectionId in (select SectionId from TSS_SiteSections where SiteSurveyId = 70023)
--SELECT tss.SiteSurveyId 'SurveyId', sd.ClientId, sd.CityId, sd.SurveyTitle,
--		       sd.[Description], sd.CategoryId, sd.SubCategoryId, sd.UnitSystemId,
--		       sd.IsActive, sd.CreatedOn, sd.CreatedBy, tss2.SiteSectionId 'SectionId', ts.SectionTitle, tss2.TotalQuestions 'TotalQuestions', ts.[Description], ts.SortOrder,
--		ts.PSectionId,ts.IsRepeatable,tss2.RepeatCount
--		FROM TSS_SurveyDocuments sd
--		INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
--		--INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
--		INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
--		INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
--		--INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
--		--INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
--		WHERE sd.isActive=1 --AND ac.IsActive=1 AND ad.IsActive=1 AND ts.IsActive=1
--		AND tss.SiteSurveyId=70023