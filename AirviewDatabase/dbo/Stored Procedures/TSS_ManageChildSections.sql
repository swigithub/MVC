-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_ManageChildSections]
 @Filter varchar(200)='GENERATE_CHILD_SITE_SECTIONS'
,@PSectionId NUMERIC(18,0)=NULL
,@SectionId  NUMERIC(18,0)=NULL
,@IsRepeatable BIT=NULL
,@RepeatCount INT=NULL
,@IsDeletable bit=NULL

AS
BEGIN
	
	IF @Filter='GENERATE_CHILD_SITE_SECTIONS'
	BEGIN

	Declare @sectionIds table(SectionId int,ChildTitle nvarchar(max),SectionTitle nvarchar(max),PSectionId int,TotalQuestions int);
	DECLARE @iterationCount int = 0;
	DECLARE @TempSectionId int = 0;
	
	DECLARE @ChildSectionsCount int = (select ISNULL(MAX(ChildTitle),0) from TSS_SiteSections where PSectionId = @PSectionId);
	--DECLARE @ChildSectionsCount int = (select count(*) from TSS_SiteSections where PSectionId = @PSectionId);
	DECLARE @TempSectionTitle nvarchar(max) = '';
	
	DECLARE @SiteId AS NUMERIC(18,0)=0
	DECLARE @SiteSurveyId AS NUMERIC(18,0)=0
	DECLARE @tPSectionId AS NUMERIC(18,0)=0
	DECLARE @tSectionId AS NUMERIC(18,0)=0

	DECLARE @QSiteSectionId int = 0 
	DECLARE @QTargetSiteSectionId int = 0 
	DECLARE @TotalQuestions int = 0;

	DECLARE @GetAlreadyRepeatedCount INT=0;
	-- UPDATE ChildCount 

	UPDATE TSS_SiteSections SET RepeatCount=@RepeatCount WHERE SiteSectionId=@PSectionId and IsActive=1

	--
	SELECT @GetAlreadyRepeatedCount=Count(*) FROM TSS_SiteSections where PSectionId=@PSectionId and IsActive=1
	print @GetAlreadyRepeatedCount
	IF @GetAlreadyRepeatedCount<=@RepeatCount
		BEGIN
			SET @RepeatCount=@RepeatCount-@GetAlreadyRepeatedCount;
			IF @RepeatCount<0
				BEGIN
					SET @RepeatCount=0;
				END
		END
	ELSE
		BEGIN
		-- Do Here What Ever You Want If AlreadyRepeatedCount is greater than current count 

		SET @GetAlreadyRepeatedCount=@GetAlreadyRepeatedCount-@RepeatCount;
		Delete from TSS_SiteSections where SiteSectionId in 
		(
			select top(@GetAlreadyRepeatedCount) SiteSectionId 
			from TSS_SiteSections 
			where PSectionId=@PSectionId and IsActive=1 
			order by SiteSectionId desc
		)
		SET @RepeatCount=0;

		END
	
	SET @TempSectionTitle = (select top 1 ts.SectionTitle from TSS_SiteSections tss inner join  TSS_Sections ts on tss.SectionId = ts.SectionId where tss.SiteSectionId =@PSectionId);
	
	SELECT @SiteSurveyId=tss.SiteSurveyId, @tPSectionId=tss.PSectionId, @tSectionId=tss.SectionId
	FROM TSS_SiteSections AS tss WHERE SiteSectionId =@PSectionId
	
	SELECT @SiteId=tss.SiteId
	FROM TSS_SiteSurvey AS tss
	WHERE tss.SiteSurveyId=@SiteSurveyId
	
	WHILE @iterationCount < @RepeatCount
	Begin
		SET @ChildSectionsCount = @ChildSectionsCount+1;
		
		--Insert Section
		--TODO: 
		INSERT INTO TSS_SiteSections(SiteSurveyId,SectionId,QuestionsAnswered,TotalQuestions,StatusId,IsActive,PSectionId,IsRepeatable,ChildTitle,IsApplicable,PSiteSectionId,IsInclude,IsDeletable) 
	
		SELECT DISTINCT ts.SiteSurveyId, ts.SectionId,0,
		(SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts2.SectionId AND tq.IsActive=1) 'TotalQuestionsx',
		90 'StatusId',ts.IsActive,@PSectionId, 0  [IsRepeatable],@ChildSectionsCount+'' [SubTitle],ts.IsApplicable,@PSectionId,1,Cast(1 as bit)
		FROM TSS_SiteSections AS ts INNER JOIN TSS_Sections AS ts2 ON ts.SectionId=ts2.sectionId
		WHERE  ts.SiteSurveyId=@SiteSurveyId AND ts2.IsActive=1
		AND ts.SiteSectionId=@PSectionId OR ts.PSectionId=@tPSectionId AND psitesectionid=@PSectionId
						
		SET @TempSectionId = SCOPE_IDENTITY();
		
		INSERT INTO TSS_SiteSections(SiteSurveyId,SectionId,QuestionsAnswered,TotalQuestions,StatusId,IsActive,PSectionId,IsRepeatable,ChildTitle,IsApplicable,PSiteSectionId,IsInclude)
		SELECT DISTINCT @SiteSurveyId 'SiteSurveyId', ts.SectionId,0,
		--ts.TotalQuestions 'TotalQuestionsx',
		(SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId AND tq.IsActive=1) 'TotalQuestionsx',
		90 'StatusId',ts.IsActive,@TempSectionId, ts.IsRepeatable,'' [SubTitle],ts.IsApplicable,@TempSectionId,1
		FROM TSS_Sections AS ts
		WHERE ts.PSectionId=@tSectionId AND ts.IsActive=1


		set @TotalQuestions = (SELECT DISTINCT top 1  TotalQuestions 'TotalQuestionsx'
		FROM TSS_SiteSections AS ts INNER JOIN TSS_Sections AS ts2 ON ts.SectionId=ts2.sectionId
		WHERE ts.PSectionId=@tSectionId)
		
		
		insert into @sectionIds(SectionId,ChildTitle,SectionTitle,PSectionId,TotalQuestions)
		SELECT @TempSectionId,@ChildSectionsCount,@TempSectionTitle,@PSectionId,@TotalQuestions
		UNION ALL
		SELECT ts.SiteSectionId,0,ts2.SectionTitle,@TempSectionId,ts.TotalQuestions
		FROM TSS_SiteSections AS ts INNER JOIN TSS_Sections AS ts2 ON ts.SectionId=ts2.sectionId
		WHERE ts.PSiteSectionId=@TempSectionId
		
		SELECT x.*
		INTO #tempx
		FROM
		(
		SELECT @TempSectionId 'SiteSectionId',@ChildSectionsCount 'ChildSectionsCount',@TempSectionTitle 'SectionTitle',@PSectionId 'PSectionId'
		UNION ALL
		SELECT ts.SiteSectionId,0 'ChildSectionsCount',ts2.SectionTitle,@TempSectionId 'PSectionId'
		FROM TSS_SiteSections AS ts INNER JOIN TSS_Sections AS ts2 ON ts.SectionId=ts2.sectionId
		WHERE ts.PSiteSectionId=@TempSectionId
		) x

		
		DECLARE db_cluster_Question CURSOR FOR  
		SELECT ress.SiteSectionId , ress.PSectionId FROM #tempx ress

		OPEN db_cluster_Question 
			FETCH NEXT FROM db_cluster_Question INTO @QSiteSectionId, @QTargetSiteSectionId
			WHILE @@FETCH_STATUS = 0   
			BEGIN
				--Section Questions
				INSERT INTO TSS_SiteQuestions(SiteSurveyId,SiteSectionId,QuestionId,IsRequired,IsNoteRequired,IsImageRequired,IsBarCodeRequired,IsAnswered,IsInclude)
				SELECT tss.SiteSurveyId,tss.SiteSectionId,tq.QuestionId,tq.IsRequired, tq.IsNoteRequired,tq.IsImageRequired, tq.IsBarCodeRequired,CAST(0 AS BIT),1
				FROM TSS_SiteSections AS tss
				INNER JOIN TSS_Questions AS tq  ON tss.SectionId=tq.SectionId
				WHERE tss.IsActive=1 and tq.IsActive=1 and tss.SiteSectionId = @QSiteSectionId AND tss.SiteSurveyId=@SiteSurveyId;

				FETCH NEXT FROM db_cluster_Question INTO @QSiteSectionId, @QTargetSiteSectionId
			END   
		CLOSE db_cluster_Question   
		DEALLOCATE db_cluster_Question

		DROP TABLE #tempx
		set @iterationCount = @iterationCount+1;
	END
	--update TSS_SiteSections set IsRepeatable = 0 where SiteSectionId = @PSectionId;
	--select * from @sectionIds
	
	SELECT DISTINCT tss.SiteSurveyId 'SurveyId', sd.ClientId, sd.CityId, sd.SurveyTitle,
		    sd.[Description], sd.CategoryId, sd.SubCategoryId, sd.UnitSystemId,
		    sd.IsActive, sd.CreatedOn, sd.CreatedBy, tss2.SiteSectionId 'SectionId', ts.SectionTitle, sit.SiteId 'SiteId', tss2.TotalQuestions 'TotalQuestions', ts.[Description], ts.SortOrder,ac.ClientName, ad.DefinationName 'CityName',
	tss2.PSiteSectionId as [PSectionId],tss2.IsRepeatable,
	tss2.PSiteSectionId as [PSectionId],tss2.IsRepeatable,
		CASE WHEN 
		tss2.RepeatCount='' THEN '0' WHEN tss2.RepeatCount is NULL THEN '0' else tss2.RepeatCount END AS RepeatCount
	,tss2.ChildTitle, ts.SectionId as [TemplateSectionId] , tss2.IsRepeatable, tss2.IsDeletable
	FROM TSS_SurveyDocuments sd
	INNER JOIN TSS_SiteSurvey AS tss ON tss.SurveyId=sd.SurveyId
	INNER JOIN AV_Sites AS sit ON sit.SiteId=tss.SiteId
	INNER JOIN TSS_Sections AS ts ON ts.SurveyId=sd.SurveyId	  
	INNER JOIN TSS_SiteSections AS tss2 ON tss2.SiteSurveyId=tss.SiteSurveyId AND tss2.SectionId=ts.SectionId
	INNER JOIN AD_Clients AS ac ON ac.ClientId=sd.ClientId
	INNER JOIN AD_Definations AS ad ON ad.DefinationId=sd.CityId
	WHERE sd.isActive=1 --AND ac.IsActive=1 AND ad.IsActive=1 AND ts.IsActive=1
	AND sit.SiteId=@SiteId AND tss2.PSectionId IN(@PSectionId)--select x.SectionId from @sectionIds x)
	--AND sit.SiteId=@SiteId AND tss2.SiteSectionId IN(select x.SectionId from @sectionIds x)
	END
	ELSE IF @Filter='DELETE_SECTIONS_WITH_CHILD'
	BEGIN

	-- It only Deletes the Current and Immediate Child 
	-- Getting SiteSurveyId For Deleting Questions

	   DECLARE @SiteSurveyIdForDelete NUMERIC(18,0);
	   SELECT @SiteSurveyIdForDelete=SiteSurveyId FROM TSS_SiteSections WHERE PSectionId=@PSectionId AND SiteSectionId=@SectionId
	   DELETE FROM TSS_SiteQuestions WHERE SiteSurveyId=@SiteSurveyIdForDelete AND SiteSectionId=@SectionId

	-- Deleting imediate childs

		DELETE FROM TSS_SiteSections WHERE PSectionId=@SectionId

	-- Deleting parent

		DELETE FROM TSS_SiteSections WHERE PSectionId=@PSectionId AND SiteSectionId=@SectionId

	
	END
END