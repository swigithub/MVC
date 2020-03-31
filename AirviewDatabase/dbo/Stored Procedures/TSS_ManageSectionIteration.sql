-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_ManageSectionIteration]
 @Filter NVARCHAR(50)
--,@SectionId NUMERIC(18,0)=NULL
--,@PSectionId NUMERIC(18,0)=NULL
--,@SurveyId NUMERIC(18,0)=NULL
--,@PIterationID NUMERIC(18,0)=NULL
--,@IterationID NUMERIC(18,0)=NULL
--,@StatusID NUMERIC(18,0)=NULL

,@List List READONLY
AS
BEGIN
	
	DECLARE @RETURN_VALUE int = 0 	
	
	IF @Filter='Insert'
	BEGIN
		INSERT INTO [dbo].[TSS_SectionIterations] ([SiteSurveyId] ,[SectionId] ,[PSectionID],[IterationID] ,[PIterationID],[StatusID])
		                                      SELECT l.Value1,   l.Value2,    l.Value3,    l.Value4,      l.Value5,      l.Value6 
											  FROM @List AS l
	
	SELECT @RETURN_VALUE = SCOPE_IDENTITY()	
	END
	
	
END