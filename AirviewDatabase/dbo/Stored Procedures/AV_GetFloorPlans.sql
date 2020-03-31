-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_GetFloorPlans]
 @Filter NVARCHAR(50)
,@SiteId numeric(18,0)= 0


AS
BEGIN
	
	IF @Filter='GetById'
	BEGIN
		select * from AV_FloorPlan where SiteId=@SiteId 
	END
END