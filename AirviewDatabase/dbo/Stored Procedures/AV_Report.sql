
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--  [dbo].[AV_Report]  'NetworkModeId = 7 AND CityId = 178 AND Region = 'South' '
CREATE PROCEDURE [dbo].[AV_Report] 
	@Filter  nvarchar(50)
AS
BEGIN
	
	--SET @Filter =' AND ' + @Filter
   DECLARE @sql as nvarchar(max) = 'select top 100 * from AV_SiteTestLog where ' + @Filter
   EXEC (@sql)
END