-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AD_GetReportFilters]
	 @Filter nvarchar(50)
	,@Value nvarchar(50)=null
AS
BEGIN
-- [dbo].[AD_GetReportFilters] 'FILTER_BY_REPORTID',444
	if @Filter='FILTER_BY_REPORTID'
	begin 
		select def.DefinationId,def.DefinationName,def.DisplayType,def.InputType,def.MapColumn 
		from AD_ReportFilters rf
		inner join AD_Definations def on def.DefinationId=rf.FilterId
		where rf.ReportId=@Value and rf.IsActive=1
	end
END