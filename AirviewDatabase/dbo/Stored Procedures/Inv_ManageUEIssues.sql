-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Inv_ManageUEIssues]
 @Filter NVARCHAR(50)
,@UEId NUMERIC(18,0)=NULL
,@UserId NUMERIC(18,0)=NULL
,@Date Datetime=NULL
,@Description NVARCHAR(50)=NULL
AS
BEGIN
	IF @Filter='Insert'
	BEGIN
	 
	 INSERT INTO [dbo].[INV_UEIssues]
			   ([UEId],[Description],[IssueUserId] ,[ReportDate] )
		 VALUES
			   (@UEId,@Description,@UserId,@Date)	
	END
END