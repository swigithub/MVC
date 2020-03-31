-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE PM_ManageGazetteHolidays
 @Filter NVARCHAR(50)
,@ProjectId NUMERIC(18,0)=NULL
,@Title NVARCHAR(50)=NULL
,@Date DATETIME=NULL
,@Isoffday BIT=NULL

AS
BEGIN
	IF @Filter='Insert'
	BEGIN
		INSERT INTO PM_GazetteHolidays(ProjectId,Title,Date,IsOffday)
		VALUEs(@ProjectId,@Title, @Date, @Isoffday)
	    	END	
	
	
	else IF @Filter='Delete'
	BEGIN
		Delete from PM_GazetteHolidays
		WHERE  ProjectId = @ProjectId	
	END	
	
END