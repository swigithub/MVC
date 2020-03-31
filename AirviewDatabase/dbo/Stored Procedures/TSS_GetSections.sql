-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_GetSections]
@Filter NVARCHAR(50)
,@Value NVARCHAR(50)
AS
BEGIN
	--	[dbo].[TSS_GetSections] 'By_SurveyId',10055
	IF @Filter='By_SurveyId'
	BEGIN
		SELECT sec.*,Convert(bit,Case When sec.SectionId in (Select Distinct SectionId from TSS_QuestionLogics) then 1 else 0 end) as IsLogicExists
		FROM TSS_Sections AS sec
		WHERE sec.SurveyId=@Value 
		Order By sec.SectionId asc
		
	END
-- [dbo].[TSS_GetSections]	'By_SiteSurveyId',40010
	ELSE IF @Filter='By_SiteSurveyId'
	BEGIN
	
	--SELECT *,sit.SiteCode,sit.WoRefId
	-- FROM TSS_SiteSurvey AS S 
	--		RIGHT JOIN AV_Sites AS sit ON sit.SiteId= S.SiteId
	--		LEFT JOIN TSS_SiteSections AS Sec ON Sec.SiteSurveyId = S.SiteSurveyId 
	--		LEFT JOIN TSS_SiteQuestions AS Q ON Q.SiteSectionId = Sec.SiteSectionId
	--		LEFT JOIN TSS_Responses R on R.QuestionId = Q.QuestionId
	--		LEFT JOIN TSS_RequiredActions Ac on Ac.ActionTypeId = Q.QuestionId
	--	WHERE S.SiteSurveyId=@Value 	

		 SELECT sit.SiteId,sit.SiteCode,sit.WoRefId,sit.Latitude, sit.Longitude, st.DefinationName 'Status', client.DefinationName AS 'ClientName', cit.DefinationName AS 'CityName', scope.DefinationName 'Scope', 
		 Sur_Doc.SurveyTitle 'SurveyTitle', Sur_Doc.[Description] AS 'Sur Descp',sitSur.SurveyId,sitSur.SiteSurveyId, sitSec.SiteSectionId,Sec.SectionId,
		 Sec.SectionTitle, Sec.[Description] AS 'Sec Descp', Que.QuestionId,Que.QuestionTypeId, Que.Question, Que.[Description] AS 'Que Descp', Que.Weightage, Que.IsRequired, Que.UnitTypeId,Que.UnitId,
		 Resp.ResponseId, Resp.ResponseText, Resp.ResponseValue, Ql.FromQuestionId, 
		 client.DefinationId,  sitSec.TotalQuestions, sitSec.RepeatCount, cat.DefinationName 'Category', subCat.DefinationName 'SubCategory', Que.QuestionId, Que.IsNoteRequired,
		 Que.IsImageRequired, Que.IsBarCodeRequired, Que.QuestionTypeId, ReqAct.ActionId,ReqAct.SiteQuestionId, ReqAct.ActionTypeId, ReqAct.ActionValue   ,convert(  bit,sitSec.IsRepeatable ) as [IsRepeatable],sitSec.ChildTitle
		 --secIter.PSectionId, secIter.IterationId, secIter.PIterationId
	 FROM AV_Sites	sit		 
			INNER JOIN TSS_SiteSurvey			AS sitSur	ON sitSur.SiteId = sit.SiteId
			INNER JOIN TSS_SurveyDocuments		AS Sur_Doc 	ON Sur_Doc.SurveyId= sitSur.SurveyId
			
			INNER JOIN AD_Definations			AS client	ON client.DefinationId = sit.ClientId	
			INNER JOIN AD_Definations			AS cit		ON cit.DefinationId = sit.CityId
			INNER JOIN AD_Definations			AS st		ON st.DefinationId=sit.Status 
			INNER JOIN AD_Definations			AS scope	ON scope.DefinationId=sit.ScopeId 		
			INNER JOIN AD_Definations			AS cat		ON cat.DefinationId=Sur_Doc.CategoryId 	
			INNER JOIN AD_Definations			AS subCat	ON subCat.DefinationId=Sur_Doc.SubCategoryId 					
	
			INNER JOIN TSS_SiteSections			AS sitSec	ON sitSec.SiteSurveyId = sitSur.SiteSurveyId
			INNER JOIN TSS_Sections				AS Sec		ON Sec.SectionId = sitSec.SectionId		

			INNER JOIN TSS_SiteQuestions		AS sitQue	ON sitQue.SiteSectionId = sitSec.SiteSectionId
			INNER JOIN TSS_Questions			AS Que		ON Que.QuestionId = sitQue.QuestionId 	
			LEFT OUTER JOIN TSS_QuestionLogics	AS Ql       ON Ql.FromQuestionId=Que.QuestionId	
			LEFT OUTER JOIN TSS_Responses		AS Resp		ON Resp.QuestionId = Que.QuestionId 
			LEFT OUTER JOIN TSS_SiteResponses	AS sitResp	ON sitResp.SiteQuestionId = sitQue.SiteQuestionId AND sitResp.ResponseId=resp.ResponseId

			LEFT OUTER JOIN TSS_RequiredActions AS ReqAct	ON ReqAct.SiteQuestionId = Que.QuestionId
			  								  
WHERE sitSur.SiteSurveyId=@Value
ORDER BY sitSur.SiteId, SitSur.SurveyId, sitSec.SiteSectionId, --sitQue.SiteQuestionId, 
Resp.ResponseId

--LEFT OUTER JOIN TSS_SectionIterations   AS secIter  ON secIter.SectionId= Sec.SectionId		
--30002--40002--40003--40004--40005--40006--40007--40008--40009--40010
			
	END
	ELSE IF @Filter='By_SiteSurveyId_Only_Sections'
	BEGIN
	
	--SELECT *,sit.SiteCode,sit.WoRefId
	-- FROM TSS_SiteSurvey AS S 
	--		RIGHT JOIN AV_Sites AS sit ON sit.SiteId= S.SiteId
	--		LEFT JOIN TSS_SiteSections AS Sec ON Sec.SiteSurveyId = S.SiteSurveyId 
	--		LEFT JOIN TSS_SiteQuestions AS Q ON Q.SiteSectionId = Sec.SiteSectionId
	--		LEFT JOIN TSS_Responses R on R.QuestionId = Q.QuestionId
	--		LEFT JOIN TSS_RequiredActions Ac on Ac.ActionTypeId = Q.QuestionId
	--	WHERE S.SiteSurveyId=@Value 	

		 SELECT Project.ProjectName,sit.Description as WODescription,SType.DefinationName as SiteType, sit.SiteAddress,sit.SiteId,sit.SiteCode,sit.WoRefId,sit.Latitude, sit.Longitude, st.DefinationName 'Status', cli.ClientName AS 'ClientName', cit.DefinationName AS 'CityName', scope.DefinationName 'Scope', 
		 Sur_Doc.SurveyTitle 'SurveyTitle', Sur_Doc.[Description] AS 'Sur Descp',sitSur.SurveyId,sitSur.SiteSurveyId, sitSec.SiteSectionId,sitSec.SectionId,
		 Sec.SectionTitle, Sec.[Description] AS 'Sec Descp',  convert(  bit,sitSec.IsRepeatable ) as [IsRepeatable],Sec.SectionId as [TemplateSectionId],
		  '' DefinationId,  sitSec.TotalQuestions, sitSec.RepeatCount, cat.DefinationName 'Category', subCat.DefinationName 'SubCategory',sitSec.ChildTitle,sitSec.PSectionId , sitSec.Signature
	 FROM AV_Sites	sit		 
			INNER JOIN TSS_SiteSurvey			AS sitSur	ON sitSur.SiteId = sit.SiteId
			INNER JOIN TSS_SurveyDocuments		AS Sur_Doc 	ON Sur_Doc.SurveyId= sitSur.SurveyId
			
			INNER JOIN AD_Clients				AS cli		ON cli.ClientId=sit.ClientId
			--INNER JOIN AD_Definations			AS client	ON client.DefinationId = sit.ClientId	

			INNER JOIN AD_Definations			AS cit		ON cit.DefinationId = sit.CityId
			INNER JOIN AD_Definations			AS st		ON st.DefinationId=sit.Status 
			INNER JOIN AD_Definations			AS scope	ON scope.DefinationId=sit.ScopeId 		
			INNER JOIN AD_Definations			AS cat		ON cat.DefinationId=Sur_Doc.CategoryId 	
			INNER JOIN AD_Definations			AS subCat	ON subCat.DefinationId=Sur_Doc.SubCategoryId 					
	
			INNER JOIN TSS_SiteSections			AS sitSec	ON sitSec.SiteSurveyId = sitSur.SiteSurveyId
			INNER JOIN TSS_Sections				AS Sec		ON Sec.SectionId = sitSec.SectionId	
			LEFT JOIN  PM_Projects				AS Project  ON sit.ProjectId = Project.ProjectId
			LEFT JOIN  AD_Definations			AS SType    ON sit.SiteTypeId= SType.DefinationId	

			
			  								  
WHERE sitSur.SiteSurveyId=@Value OR sitsur.SiteId=@Value
	--ORDER BY sitsec.PSiteSectionId,sitsec.SiteSectionId
ORDER BY sitSur.SiteId, SitSur.SurveyId, sitSec.SiteSectionId --sitQue.SiteQuestionId

--LEFT OUTER JOIN TSS_SectionIterations   AS secIter  ON secIter.SectionId= Sec.SectionId		
--30002--40002--40003--40004--40005--40006--40007--40008--40009--40010
			
	END

	else IF @Filter='By_SectionId'
	BEGIN
		SELECT * 
		FROM TSS_Sections AS sec
		WHERE sec.SectionId=@Value AND sec.IsActive=1
		Order By sec.SectionId asc
		
	END
	


	--else IF @Filter='By_SiteSectionId'
	--BEGIN
	--	SELECT * 
	--	FROM TSS_SiteQuestions AS sec
	--	WHERE sec.SiteSectionId=@Value 
		
	--END

--	[dbo].[TSS_GetSections]
	else IF @Filter='By_PSectionId'
	BEGIN
		SELECT * 
		FROM TSS_Sections AS sec
		WHERE sec.PSectionId=@Value AND sec.IsActive=1
		
	END
	
	ELSE IF @Filter='Sections_by_SiteSurvey'
	BEGIN
		SELECT tss.SiteSectionId, tss.SiteSurveyId, tss.SectionId, ts.SectionTitle,
		       tss.QuestionsAnswered, tss.TotalQuestions, tss.StatusId, sts. DefinationName 'Status', sts.ColorCode 'StatusColor'
		FROM TSS_SiteSections AS tss
		INNER JOIN TSS_Sections AS ts ON ts.SectionId=tss.SectionId
		INNER JOIN AD_Definations AS sts ON sts.DefinationId=tss.StatusId
		WHERE  tss.SiteSurveyId=@Value AND tss.IsActive=1
	END


	ELSE IF @Filter='Sections_by_SiteSurvey_For_Dashboard'
	BEGIN
		SELECT tss.SiteSectionId as TemplateSectionId, tss.SiteSurveyId as SurveyId, ts.SectionId, ts.SectionTitle,tss.PSectionId, ts.PSectionId as SectionPSectionId, ts.SortOrder, tss.IsRepeatable,
		       tss.QuestionsAnswered as TotalAnswered, tss.TotalQuestions, tss.StatusId, 
			   
			   Case when	sts.DefinationName='Drive Completed' then 'Completed' 
			when sts.DefinationName='Pending With Issues' then 'Issues' 
			when sts.DefinationName='Pending Schedule' then 'Pending' 
			when sts.DefinationName='Schedule' then 'Pending' 
			else sts.DefinationName end as 

 'Status'
			   
			   , sts.ColorCode 'StatusColor', tss.ChildTitle
		FROM TSS_SiteSections AS tss
		INNER JOIN TSS_Sections AS ts ON ts.SectionId=tss.SectionId
		INNER JOIN AD_Definations AS sts ON sts.DefinationId=tss.StatusId
		WHERE  tss.SiteSurveyId=@Value AND tss.IsActive=1
	END

	



END