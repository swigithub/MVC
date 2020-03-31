-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_GetSurveySiteAttendees]
	-- Add the parameters for the stored procedure here
 @Filter NVARCHAR(50),
 @SiteID numeric(18,0),
 @SiteSurveyID numeric(18,0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @Filter='Get_Site_Attendees_For_Dashboard'
	BEGIN
	select * from TSS_SiteAttendees where SiteId = @SiteID and SiteSurveyId = @SiteSurveyID
	end
END