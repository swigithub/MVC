-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AD_GetQueryResult]
 @Filter nvarchar(50)
,@Value nvarchar(50)=null
,@Value2 nvarchar(50)=null
AS
BEGIN
	if	@Filter='WidgetQueryResult'
	begin
		DECLARE @sql as nvarchar(max)=(SELECT x.SqlQuery FROM AV_Widgets x WHERE x.WidgetId=@Value)
		PRINT @sql
		EXEC(@sql)
	end
END