-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--sp_UpdateCheckListSectionsQuestions '60093,60094,60095', ''
CREATE PROCEDURE [dbo].[sp_UpdateCheckListSectionsQuestions]
	-- Add the parameters for the stored procedure here
	@Sections nvarchar(max),
	@Questions nvarchar(max),
	@SiteSurveyID varchar(50),
	@SiteSectionIds varchar(max)
	

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
SET @Sections = '''' + REPLACE(@Sections,',',''',''') + ''''
SET @Questions = '''' + REPLACE(@Questions,',',''',''') + ''''
SET @SiteSectionIds = '''' + REPLACE(@SiteSectionIds,',',''',''') + ''''
    -- Insert statements for procedure here
	UPDATE TSS_SiteSections set IsInclude = 0 where SiteSurveyId = @SiteSurveyID
	update TSS_SiteQuestions set IsInclude = 0 where ' ,'+@SiteSectionIds+', ' LIKE '%,'''+Convert(varchar(max),SiteSectionId)+''',%'  and SiteSurveyId = @SiteSurveyID
	update TSS_SiteSections set IsInclude = 1 where ' ,'+@Sections+', ' LIKE '%,'''+Convert(varchar(max),SiteSectionId)+''',%' 
	update TSS_SiteQuestions set IsInclude = 1 where ' ,'+@Questions+', ' LIKE '%,'''+Convert(varchar(max),SiteQuestionId)+''',%' 
END