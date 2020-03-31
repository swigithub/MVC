-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_ManageSiteAttendees]
	-- Add the parameters for the stored procedure here
@Filter NVARCHAR(50)
,@List TSS_SiteAttendeeList READONLY,
@SiteId numeric(18,0),
@SiteSurveyId numeric(18,0)
AS
BEGIN
	
	DECLARE @Name AS VARCHAR(100)=''
	DECLARE @Designation AS VARCHAR(50)=''
	DECLARE @Company AS VARCHAR(100)=''
	DECLARE @Signature AS VARCHAR(MAX)=''
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @Filter='SurveyAttendee'
	BEGIN
	DELETE from TSS_SiteAttendees where SiteId = @SiteId and SiteSurveyId = @SiteSurveyId
		DECLARE db_cluster2 CURSOR FOR  
		SELECT res.Value1, res.Value2, res.Value3, res.Value4,res.Value5,res.Value6 FROM @List res
		OPEN db_cluster2 
		FETCH NEXT FROM db_cluster2 INTO @SiteId, @SiteSurveyId, @Name, @Designation, @Company, @Signature
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			
			
			INSERT INTO TSS_SiteAttendees(SiteId,SiteSurveyId,Name,Designation,Company,[Signature])
			SELECT @SiteId, @SiteSurveyId, @Name, @Designation, @Company, @Signature
			
		FETCH NEXT FROM db_cluster2 INTO @SiteId, @SiteSurveyId, @Name, @Designation, @Company, @Signature
		END   
		CLOSE db_cluster2   
		DEALLOCATE db_cluster2
	END
END