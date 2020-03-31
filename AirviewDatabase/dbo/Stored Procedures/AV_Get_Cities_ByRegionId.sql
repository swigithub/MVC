-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_Get_Cities_ByRegionId]
	@Id int,
	@status bit
AS
BEGIN
	select * from AD_Definations where PDefinationId=@Id and IsActive=@status
END