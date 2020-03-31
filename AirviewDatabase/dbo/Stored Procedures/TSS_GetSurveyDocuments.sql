-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_GetSurveyDocuments]
 @Filter NVARCHAR(50)
,@Value NVARCHAR(50)=NULL

,@pageIndex INT = 0 
,@pageSize INT = 0 ,
@IMEI NVARCHAR(50)=NULL,
@UserId NUMERIC(18,0)=0



AS
BEGIN 
	
	Declare @totalCount  int;
	
--  [dbo].[TSS_GetSurveyDocuments] 'Get_All',true, 1, 5	
	IF @Filter='Get_All'
	BEGIN
		SET @totalCount  = (Select Count(SurveyId) from TSS_SurveyDocuments where IsActive=@Value);
		
		SELECT SurveyId, sd.ClientId, CityId, SurveyTitle, Description, CategoryId, SubCategoryId, UnitSystemId, sd.IsActive, CreatedOn, CreatedBy, IsPublished, 
				ISNULL(SurveyCode,'Not Defined') SurveyCode, ISNULL(ac.ClientName,'All') AS ClientName, ad.DefinationName AS 'CityName', 
                  cat.DefinationName AS 'Category', subcat.DefinationName AS 'SubCategory', @totalCount AS 'totalCount'
		FROM     TSS_SurveyDocuments AS sd	  
		LEFT JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		INNER JOIN AD_Definations AS cat ON cat.DefinationId=sd.CategoryId
		INNER JOIN AD_Definations AS subcat ON subcat.DefinationId=sd.SubCategoryId
		WHERE sd.isActive=@Value AND ad.IsActive=1 AND CreatedBy=@UserId
		--AND ac.IsActive=1 

		ORDER BY sd.SurveyId OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY; 

		 --SELECT count(SurveyId) as totalCount FROM TSS_SurveyDocuments;  
	END
	
	IF @Filter = 'GetAllCOP'
	BEGIN
		SELECT SurveyId, cli.ClientName, city.DefinationName as CityName, SurveyTitle, Description, cat.DefinationName as Category, subcat.DefinationName as SubCategory,
				 UnitSystemId, sd.IsActive, CreatedOn, CreatedBy, IsPublished
		FROM     TSS_SurveyDocuments sd
		INNER JOIN AD_Clients AS cli ON cli.ClientId=sd.ClientId
		INNER JOIN AD_Definations AS city ON city.DefinationId=sd.CityId
		INNER JOIN AD_Definations AS cat ON cat.DefinationId=sd.CategoryId
		INNER JOIN AD_Definations AS subcat ON subcat.DefinationId=sd.SubCategoryId
		WHERE sd.IsActive=1 and  sd.IsPublished=1
	END

--	[dbo].[TSS_GetSurveyDocuments] 'GetAllIsActive',1
	ELSE IF @Filter='GetAll_byIsActive'
	BEGIN
		
		SELECT * 
		FROM TSS_SurveyDocuments sur
		WHERE sur.IsActive= @Value and IsPublished=1 AND CreatedBy=@UserId
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
		       sd.IsActive, sd.CreatedOn, sd.CreatedBy, tss2.SiteSectionId 'SectionId', ts.SectionTitle, sit.SiteId 'SiteId', tss2.TotalQuestions 'TotalQuestions',
			    (select ISNULL(Count(*),0) from TSS_SiteQuestions where SiteSurveyId=tss.SiteSurveyId and SiteSectionId=tss2.SiteSectionId and IsAnswered=1 ) TotalAnswered,
			    ts.[Description], ts.SortOrder,ac.ClientName, ad.DefinationName 'CityName',
		tss2.PSiteSectionId as [PSectionId],tss2.IsRepeatable,
		CASE WHEN 
		tss2.RepeatCount='' THEN '0' WHEN tss2.RepeatCount is NULL THEN '0' else tss2.RepeatCount END AS RepeatCount,
		tss2.ChildTitle, ts.SectionId as [TemplateSectionId] , tss2.IsRepeatable
		,def.DefinationId as Status,def.ColorCode as StatusColor,ts.IsSignatureRequired,tss2.IsDeletable,sit.Longitude,sit.Latitude
		FROM TSS_SurveyDocuments sd
		INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
		INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
		INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=sit.SiteId AND anls.SiteSurveyId=tss.SiteSurveyId
		INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
		INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
		INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		LEFT JOIN AD_Definations def on def.DefinationId=tss2.StatusId
		INNER JOIN AV_WoDevices AS awd ON awd.SiteId=sit.SiteId AND awd.NetLayerStatusId=anls.LayerStatusId
		INNER JOIN Sec_UserDevices AS sud ON sud.DeviceId=awd.UserDeviceId AND sud.UserId=awd.UserId
		WHERE sd.isActive=1 AND awd.UserId=@pageIndex AND sud.IMEI=@IMEI
		--AND ac.IsActive=1 AND ad.IsActive=1 AND ts.IsActive=1
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
	SELECT tss.SiteSurveyId 'SurveyId', sd.ClientId, sd.CityId, sd.SurveyTitle,sit.SiteCode,tss2.PSectionId,
		       sd.[Description], sd.CategoryId, sd.SubCategoryId, sd.UnitSystemId,
		       sd.IsActive, sd.CreatedOn, sd.CreatedBy, tss2.SiteSectionId 'SectionId', ts.SectionTitle, tss2.TotalQuestions 'TotalQuestions',
			   (select ISNULL(Count(*),0) from TSS_SiteQuestions where SiteSurveyId=tss.SiteSurveyId and SiteSectionId=tss2.SiteSectionId and IsAnswered=1 ) TotalAnswered,
			    ts.[Description], ts.SortOrder,
		tss2.IsRepeatable,tss2.RepeatCount,tss2.ChildTitle, ts.SectionId as [TemplateSectionId], tss2.IsRepeatable, tss2.IsInclude,tss2.Signature,

		Case when	def.DefinationName='Drive Completed' then 'Completed' 
			when def.DefinationName='Pending With Issues' then 'Issues' 
			when def.DefinationName='Pending Schedule' then 'Pending' 
			when def.DefinationName='Schedule' then 'Pending' 
			else def.DefinationName end as DefinationName


		
		,def.ColorCode,tss2.IsDeletable,ts.IsSignatureRequired,sit.Longitude,sit.Latitude
		FROM TSS_SurveyDocuments sd
		INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
		INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
		INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
		INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
		LEFT JOIN AD_Definations def on def.DefinationId=tss2.StatusId
		--INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		--INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		WHERE sd.isActive=1 --AND ac.IsActive=1 AND ad.IsActive=1 AND ts.IsActive=1
		AND tss.SiteSurveyId=@value and tss2.IsInclude = 1
	END
	else IF @Filter='SURVEY_SECTIONS_BY_SITESURVEYID_FOR_COMPLETION'
	BEGIN

DECLARE @SectionCompletionTable TABLE(
    SurveyId numeric(18, 0),
    ClientId numeric(18, 0),
	 SectionId numeric(18, 0),
	 PSectionId numeric(18, 0),
	 SectionTitle varchar(max),
	TotalQuestions numeric(18, 0),
	TotalAnswered numeric(18, 0),
	 DefinationName varchar(max),
	IsRepeatable bit
);

INSERT INTO @SectionCompletionTable (SurveyId, ClientId, SectionId, PSectionId, SectionTitle, TotalQuestions, TotalAnswered, DefinationName, IsRepeatable)
select SurveyId, ClientId, SectionId, PSectionId, SectionTitle, TotalQuestions, TotalAnswered, DefinationName, IsRepeatable from (SELECT tss.SiteSurveyId 'SurveyId', sd.ClientId, ts.SectionId, ts.PSectionId, ts.SectionTitle,
		      CASE WHEN tss2.IsRepeatable =1 then 0 else tss2.TotalQuestions end as 'TotalQuestions',
			   (select ISNULL(Count(*),0) from TSS_SiteQuestions where SiteSurveyId=tss.SiteSurveyId and SiteSectionId=tss2.SiteSectionId and IsAnswered=1 ) TotalAnswered,
			    
		Case when	def.DefinationName='Drive Completed' then 'Completed' 
			when def.DefinationName='Pending With Issues' then 'Issues' 
			when def.DefinationName='Pending Schedule' then 'Pending' 
			when def.DefinationName='Schedule' then 'Pending' 
			else def.DefinationName end as DefinationName,
		tss2.IsRepeatable,
		CASE WHEN tss2.IsRepeatable = 1 and ts.PSectionId != 0 then 1 else 0 end as IsPsectionId
		FROM TSS_SurveyDocuments sd
		INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
		INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
		INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
		INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
		LEFT JOIN AD_Definations def on def.DefinationId=tss2.StatusId
		--INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		--INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		WHERE sd.isActive=1 --AND ac.IsActive=1 AND ad.IsActive=1 AND ts.IsActive=1
		AND tss.SiteSurveyId=@value and tss2.IsInclude = 1) p where IsPsectionId != 1

	
;WITH SectionCompletion as
(
  SELECT   item.SectionId AS SectionIdParent
        ,item.SectionTitle AS SectionTitleParent,item.DefinationName as DefinationNameParent,  item.SurveyId, item.ClientId, item.SectionId, item.PSectionId, item.SectionTitle, item.TotalQuestions, item.TotalAnswered, item.DefinationName, item.IsRepeatable 
    FROM @SectionCompletionTable item where item.PSectionId = 0

  UNION ALL

  select  M.SectionIdParent
        ,M.SectionTitleParent, M.DefinationNameParent, item2.SurveyId, item2.ClientId, item2.SectionId, item2.PSectionId, item2.SectionTitle, item2.TotalQuestions, item2.TotalAnswered, item2.DefinationName, item2.IsRepeatable
from @SectionCompletionTable item2

  INNER JOIN SectionCompletion M
  ON M.SectionId = item2.PsectionId
 )
SELECT
    b.SectionIdParent as SectionId,
    b.SectionTitleParent as SectionTitle,
	 b.DefinationNameParent as DefinationName,
    SUM(b.TotalQuestions) AS TotalQuestions,
	 SUM(b.TotalAnswered) AS TotalAnswered
FROM
    SectionCompletion b
    
GROUP BY
    b.SectionIdParent
    ,b.SectionTitleParent
	,b.DefinationNameParent
ORDER BY
    b.SectionIdParent
	

	END
	else IF @Filter='SURVEY_SECTIONS_BY_SITESURVEYID_For_Dashboard'
	BEGIN
	SELECT tss.SiteSurveyId 'SurveyId', sd.ClientId, sd.CityId, sd.SurveyTitle,tss2.PSectionId,
		       sd.[Description], sd.CategoryId, sd.SubCategoryId, sd.UnitSystemId,
		       sd.IsActive, sd.CreatedOn, sd.CreatedBy, tss2.SiteSectionId 'SectionId', ts.SectionTitle, tss2.TotalQuestions 'TotalQuestions',
			   (select ISNULL(Count(*),0) from TSS_SiteQuestions where SiteSurveyId=tss.SiteSurveyId and SiteSectionId=tss2.SiteSectionId and IsAnswered=1 ) TotalAnswered,
			    ts.[Description], ts.SortOrder,
		tss2.IsRepeatable,tss2.RepeatCount,tss2.ChildTitle, ts.SectionId as [TemplateSectionId], tss2.IsRepeatable, tss2.IsInclude,tss2.Signature,
		def.DefinationName,def.ColorCode,tss2.IsDeletable
		FROM TSS_SurveyDocuments sd
		INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
		--INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
		INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
		INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
		LEFT JOIN AD_Definations def on def.DefinationId=tss2.StatusId
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

	ELSE IF @Filter='Survey_By_SiteId'
	BEGIN
		SELECT SiteSurveyId
		 FROM TSS_SiteSurvey 
		 WHERE SiteId=@Value
	END

	ELSE IF @Filter='MULTISURVEY_SECTIONS_BY_SITEID'
	BEGIN
		SELECT tss.SiteSurveyId 'SurveyId', sd.ClientId, sd.CityId, sd.SurveyTitle,sit.SiteCode,tss2.PSectionId,
		       sd.[Description], sd.CategoryId, sd.SubCategoryId, sd.UnitSystemId,
		       sd.IsActive, sd.CreatedOn, sd.CreatedBy, tss2.SiteSectionId 'SectionId', ts.SectionTitle, tss2.TotalQuestions 'TotalQuestions',
			   (select ISNULL(Count(*),0) from TSS_SiteQuestions where SiteSurveyId=tss.SiteSurveyId and SiteSectionId=tss2.SiteSectionId and IsAnswered=1 ) TotalAnswered,
			    ts.[Description], ts.SortOrder,
		tss2.IsRepeatable,tss2.RepeatCount,tss2.ChildTitle, ts.SectionId as [TemplateSectionId], tss2.IsRepeatable, tss2.IsInclude,tss2.Signature,

		Case when	def.DefinationName='Drive Completed' then 'Completed' 
			when def.DefinationName='Pending With Issues' then 'Issues' 
			when def.DefinationName='Pending Schedule' then 'Pending' 
			when def.DefinationName='Schedule' then 'Pending' 
			else def.DefinationName end as DefinationName
		,def.ColorCode,tss2.IsDeletable,ts.IsSignatureRequired,sit.Longitude,sit.Latitude
		FROM TSS_SurveyDocuments sd
		INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
		INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
		INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
		INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
		LEFT JOIN AD_Definations def on def.DefinationId=tss2.StatusId
		--INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		--INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		WHERE sd.isActive=1 --AND ac.IsActive=1 AND ad.IsActive=1 AND ts.IsActive=1
		AND tss.SiteId=@value and tss2.IsInclude = 1
	END
	ELSE IF @Filter='GET_SECTIONS_BY_SurveyId'
	BEGIN
		SELECT DISTINCT tss.SiteSurveyId 'SurveyId', sd.ClientId, sd.CityId, sd.SurveyTitle,
		       sd.[Description], sd.CategoryId, sd.SubCategoryId, sd.UnitSystemId,
		       sd.IsActive, sd.CreatedOn, sd.CreatedBy, tss2.SiteSectionId 'SectionId', ts.SectionTitle, sit.SiteId 'SiteId', tss2.TotalQuestions 'TotalQuestions',
			    (select ISNULL(Count(*),0) from TSS_SiteQuestions where SiteSurveyId=tss.SiteSurveyId and SiteSectionId=tss2.SiteSectionId and IsAnswered=1 ) TotalAnswered,
			    ts.[Description], ts.SortOrder,ac.ClientName, ad.DefinationName 'CityName',
		tss2.PSiteSectionId as [PSectionId],tss2.IsRepeatable,
		CASE WHEN 
		tss2.RepeatCount='' THEN '0' WHEN tss2.RepeatCount is NULL THEN '0' else tss2.RepeatCount END AS RepeatCount,
		tss2.ChildTitle, ts.SectionId as [TemplateSectionId] , tss2.IsRepeatable
		,def.DefinationId as Status,def.ColorCode as StatusColor,ts.IsSignatureRequired,tss2.IsDeletable,sit.Longitude,sit.Latitude
		FROM TSS_SurveyDocuments sd
		INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
		INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
		INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
		INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
		INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
		LEFT JOIN AD_Definations def on def.DefinationId=tss2.StatusId
		WHERE sd.isActive=1 AND tss.SiteSurveyId=@Value
	END
	--ELSE IF @Filter='By_SectionId'
	--BEGIN
	--	SELECT *FROM TSS_SiteSurvey S
	--INNER JOIN [TSS_SiteSections]  Sec on  Sec.SiteSurveyId=S.SiteSurveyId 
	--INNER JOIN [TSS_SiteQuestions]  Q  on  Sec.SiteSectionId= Q.SiteSectionId
	--where S.SiteSurveyId=40010
	--END

		


END