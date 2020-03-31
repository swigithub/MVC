CREATE PROCEDURE [dbo].[TSS_UpdateSurveyStatusForDashboard]
@SiteSurveyId numeric(18,0)
As
BEGIN

DECLARE @StatusId_Cursor numeric(18,0)
DECLARE @StatusId_ToCheck numeric(18,0)=(select Top(1) StatusId from TSS_SiteSections where SiteSurveyId=@SiteSurveyId)
DECLARE @Count int=(select count(*) StatusId from TSS_SiteSections where SiteSurveyId=@SiteSurveyId)

DECLARE @Counter int=0

DECLARE CHANGE_STATUS CURSOR FOR
select StatusId from TSS_SiteSections where SiteSurveyId=@SiteSurveyId

OPEN CHANGE_STATUS 

FETCH NEXT FROM CHANGE_STATUS INTO 
@StatusId_Cursor

WHILE @@FETCH_STATUS=0
BEGIN
	
	IF (@StatusId_Cursor=@StatusId_ToCheck)
	BEGIN
		SET @Counter=@Counter+1
	END

	FETCH NEXT FROM CHANGE_STATUS INTO 
	@StatusId_Cursor
END

CLOSE CHANGE_STATUS
DEALLOCATE CHANGE_STATUS

	IF (@Counter=@Count)
		BEGIN
			UPDATE AV_NetLayerStatus set Status=@StatusId_ToCheck WHERE SiteSurveyId=@SiteSurveyId
			
		END
	ELSE
		BEGIN
			UPDATE AV_NetLayerStatus set Status=450 WHERE SiteSurveyId=@SiteSurveyId
			
		END

END