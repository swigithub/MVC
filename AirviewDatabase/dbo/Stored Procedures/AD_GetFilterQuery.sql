-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AD_GetFilterQuery]
	 @Filter nvarchar(50)
	,@Value nvarchar(50)=null
AS
BEGIN
--  [dbo].[AD_GetFilterQuery] 'GET_QUERY_RESULT_ByFilterId',23229
	if @Filter='GET_QUERY_RESULT_ByFilterId'
	begin
		
		DECLARE @sql as nvarchar(max)=(SELECT x.sqlQuery FROM AD_FilterQuery x WHERE x.FilterId=@Value)
		EXEC(@sql)
	end
	
END