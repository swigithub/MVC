-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =========================================
create PROCEDURE PM_GetGazetteHolidays
 @Filter NVARCHAR(50)
,@value1 NVARCHAR(50)=NULL
,@value2 NVARCHAR(50)=NULL
,@value3 NVARCHAR(500)=NULL
,@value4 NVARCHAR(50)=NULL

AS
BEGIN
	IF @Filter='ByProjectId'
	BEGIN
		select * from PM_GazetteHolidays where ProjectId =CONVERT(NUMERIC(18,0), @value1)
	    	END	
	
	
	
END