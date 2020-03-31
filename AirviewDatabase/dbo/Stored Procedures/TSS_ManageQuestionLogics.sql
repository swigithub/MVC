-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_ManageQuestionLogics]
@Filter NVARCHAR(50)
,@Value NVARCHAR(50)=NULL
,@List List READONLY
AS
BEGIN
		DECLARE @SurveyID as nvarchar(50)=''
		DECLARE @SectionID as nvarchar(50)=''
		DECLARE @FQuesID as nvarchar(50)=''
		DECLARE @TQuesID as nvarchar(50)=''
		DECLARE @CondID as nvarchar(50)=''
		DECLARE @ResponseID as nvarchar(50)=''
		DECLARE @ActionID as nvarchar(50)=''

	IF @Filter='Insert'
	BEGIN
		SELECT @SurveyID=x.Value1, @SectionID=x.Value2, @FQuesID=x.Value3,
		 @TQuesID=x.Value4,
		 @CondID=x.Value5,@ResponseID=x.Value6,@ActionID=x.Value7 
		FROM @List x;
		
		DELETE FROM TSS_QuestionLogics
		WHERE   SurveyId=@SurveyID AND 
				SectionID=@SectionID AND 
				FromQuestionId=@FQuesID AND 
				ConditionId=@CondID AND 
				ResponseId=@ResponseID AND 
				ActionId=@ActionID --AND
		        --Charindex(cast(ToQuestionId as varchar(max))+',', @TQuesID) > 0;
			

		INSERT INTO TSS_QuestionLogics
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8 FROM @List AS l
	END
	
	ELSE IF @Filter='Delete'
	BEGIN
		DELETE FROM TSS_QuestionLogics
		WHERE LogicId=@Value
	END

END