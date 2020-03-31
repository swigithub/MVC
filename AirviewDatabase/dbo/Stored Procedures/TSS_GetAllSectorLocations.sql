CREATE PROCEDURE TSS_GetAllSectorLocations
@SiteSurveyId NUMERIC(18,0)
AS 
BEGIN
SELECT SS.SiteSectionId,SQ.SiteQuestionId,SQ.Azimuth,SR.ResponseValue as LatLng
	FROM TSS_SiteSections SS 
	inner join TSS_SiteQuestions SQ ON SQ.SiteSectionId=SS.SiteSectionId
	inner join TSS_SiteResponses SR ON SR.SiteQuestionId=SQ.SiteQuestionId
	inner join TSS_Questions TQ on TQ.QuestionId=SQ.QuestionId
	WHERE SS.SiteSurveyId=@SiteSurveyId and SQ.Azimuth is not null and SR.ResponseValue <> '' and TQ.SurveyEntity='Sector Location'
END