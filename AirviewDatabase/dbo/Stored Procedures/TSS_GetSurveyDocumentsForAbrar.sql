-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_GetSurveyDocumentsForAbrar]
 @Filter NVARCHAR(50)
,@Value NVARCHAR(50)=NULL

,@pageIndex INT = 0 
,@pageSize INT = 0 



AS
BEGIN 
	
	Declare @totalCount  int;
	
--  [dbo].[TSS_GetSurveyDocuments] 'Get_All',true, 1, 5	
	IF @Filter='Get_All'
	BEGIN
		
		SET @totalCount  = (Select Count(SurveyId) from TSS_SurveyDocuments where IsActive=@Value);
		
		SELECT sd.*,ac.ClientName, ad.DefinationName 'CityName' ,cat.DefinationName 'Category',subcat.DefinationName 'SubCategory', @totalCount 'totalCount'
		FROM TSS_SurveyDocuments sd		  
		INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		INNER JOIN AD_Definations AS cat ON cat.DefinationId=sd.CategoryId
		INNER JOIN AD_Definations AS subcat ON subcat.DefinationId=sd.SubCategoryId
		WHERE sd.isActive=@Value AND ac.IsActive=1 AND ad.IsActive=1

		ORDER BY sd.SurveyId OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY; 

		 --SELECT count(SurveyId) as totalCount FROM TSS_SurveyDocuments;  
	END
	
--	[dbo].[TSS_GetSurveyDocuments] 'GetAllIsActive',1
	ELSE IF @Filter='GetAll_byIsActive'
	BEGIN
		
		SELECT * 
		FROM TSS_SurveyDocuments sur
		WHERE sur.IsActive= @Value
	END

	ELSE IF @Filter='GetIsActive'
	BEGIN
		SET @totalCount  = (Select Count(SurveyId) from TSS_SurveyDocuments where isActive = @Value);
		SELECT sd.*,ac.ClientName, ad.DefinationName 'CityName',cat.DefinationName 'Category',subcat.DefinationName 'SubCategory', @totalCount 'totalCount'
		FROM TSS_SurveyDocuments sd		  
		INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		INNER JOIN AD_Definations AS cat ON cat.DefinationId=sd.CategoryId
		INNER JOIN AD_Definations AS subcat ON subcat.DefinationId=sd.SubCategoryId
		WHERE sd.isActive=@Value AND ac.IsActive=1 AND ad.IsActive=1
		ORDER BY sd.SurveyId OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY; 
	END
	
	ELSE IF @Filter='SURVEY_BY_WO'
	BEGIN
		SELECT DISTINCT tss.SiteSurveyId 'SurveyId', sd.ClientId, sd.CityId, sd.SurveyTitle,
		       sd.[Description], sd.CategoryId, sd.SubCategoryId, sd.UnitSystemId,
		       sd.IsActive, sd.CreatedOn, sd.CreatedBy, tss2.SiteSectionId 'SectionId', ts.SectionTitle, sit.SiteId 'SiteId', tss2.TotalQuestions 'TotalQuestions', ts.[Description], ts.SortOrder,ac.ClientName, ad.DefinationName 'CityName',
		tss2.PSiteSectionId as [PSectionId],tss2.IsRepeatable,tss2.RepeatCount,tss2.ChildTitle, ts.SectionId as [TemplateSectionId] , tss2.IsRepeatable
		FROM TSS_SurveyDocuments sd
		INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
		INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
		INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
		INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
		INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		WHERE sd.isActive=1 --AND ac.IsActive=1 AND ad.IsActive=1 AND ts.IsActive=1
		AND sit.SiteId=@Value
	END
	IF @Filter='SURVEY_BY_SITESURVEYID'
	BEGIN
		SELECT DISTINCT tss.SiteSurveyId 'SurveyId', sd.ClientId, sd.CityId, sd.SurveyTitle,tss2.PSectionId,
		       sd.[Description], sd.CategoryId, sd.SubCategoryId, sd.UnitSystemId,
		       sd.IsActive, sd.CreatedOn, sd.CreatedBy, tss2.SiteSectionId 'SectionId', ts.SectionTitle, sit.SiteId 'SiteId', tss2.TotalQuestions 'TotalQuestions', ts.[Description], ts.SortOrder,ac.ClientName, ad.DefinationName 'CityName',
		tss2.PSectionId,tss2.IsRepeatable,tss2.RepeatCount ,tss2.ChildTitle , ts.SectionId as [TemplateSectionId], tss2.IsRepeatable
		FROM TSS_SurveyDocuments sd
		INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
		INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
		INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
		INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
		INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		WHERE sd.isActive=1 --AND ac.IsActive=1 AND ad.IsActive=1 AND ts.IsActive=1
		AND tss.SiteSurveyId=@Value
	END
	else IF @Filter='SURVEY_SECTIONS_BY_SITESURVEYID'
	BEGIN
		SELECT tss.SiteSurveyId 'SurveyId', sd.ClientId, sd.CityId, sd.SurveyTitle,tss2.PSectionId,
		       sd.[Description], sd.CategoryId, sd.SubCategoryId, sd.UnitSystemId,
		       sd.IsActive, sd.CreatedOn, sd.CreatedBy, tss2.SiteSectionId 'SectionId', ts.SectionTitle, tss2.TotalQuestions 'TotalQuestions', ts.[Description], ts.SortOrder,
		tss2.IsRepeatable,tss2.RepeatCount,tss2.ChildTitle, ts.SectionId as [TemplateSectionId], tss2.IsRepeatable, tss2.IsInclude
		FROM TSS_SurveyDocuments sd
		INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
		--INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
		INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
		INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
		--INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		--INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		WHERE sd.isActive=1 --AND ac.IsActive=1 AND ad.IsActive=1 AND ts.IsActive=1
		AND tss.SiteSurveyId=@value and tss2.IsInclude = 1
	END
	else IF @Filter='SURVEY_SECTIONS_BY_SITESURVEYID_For_Dashboard'
	BEGIN
			SELECT tss.SiteSurveyId 'SurveyId', sd.ClientId, sd.CityId, sd.SurveyTitle,tss2.PSectionId,
		       sd.[Description], sd.CategoryId, sd.SubCategoryId, sd.UnitSystemId,
		       sd.IsActive, sd.CreatedOn, sd.CreatedBy, tss2.SiteSectionId 'SectionId', ts.SectionTitle, tss2.TotalQuestions 'TotalQuestions', ts.[Description], ts.SortOrder,
		tss2.IsRepeatable,tss2.RepeatCount,tss2.ChildTitle, ts.SectionId as [TemplateSectionId], tss2.IsRepeatable, tss2.IsInclude
		FROM TSS_SurveyDocuments sd
		INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
		--INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
		INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
		INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
		--INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		--INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		WHERE sd.isActive=1 --AND ac.IsActive=1 AND ad.IsActive=1 AND ts.IsActive=1
		AND tss.SiteSurveyId=@value
	END
	else IF @Filter='By_SurveyId'
	BEGIN
		SELECT * 
		FROM TSS_SurveyDocuments sur
		WHERE sur.SurveyId= @Value
	END
	
	else IF @Filter='By_SubCategoryId'
	BEGIN
		SELECT * 
		FROM TSS_SurveyDocuments sur
		WHERE sur.SubCategoryId= @Value AND sur.IsActive=1
	END
	ELSE IF @Filter='CLIENT_MARKET_SURVEY'
	BEGIN
		SELECT * 
		FROM TSS_SurveyDocuments sur
		WHERE sur.ClientId= @Value
	END


	--ELSE IF @Filter='By_SectionId'
	--BEGIN
	--	SELECT *FROM TSS_SiteSurvey S
	--INNER JOIN [TSS_SiteSections]  Sec on  Sec.SiteSurveyId=S.SiteSurveyId 
	--INNER JOIN [TSS_SiteQuestions]  Q  on  Sec.SiteSectionId= Q.SiteSectionId
	--where S.SiteSurveyId=40010
	--END

		


END