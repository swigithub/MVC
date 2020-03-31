-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_GetWidgets]
 @Filter nvarchar(50)
,@Value nvarchar(50)=null
AS
BEGIN

-- [dbo].[AV_GetWidgets] 'GetAll',1
	if @Filter='GetAll'
	begin
		select * 
		from AV_Widgets
	end

-- [dbo].[AV_GetWidgets] 'ByWidgetId',1
	if @Filter='ByWidgetId'
	begin
		select * 
		from AV_Widgets
		where WidgetId=@Value
	end
END