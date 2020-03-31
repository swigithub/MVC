-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Inv_GetUEIssues 
 @Filter nvarchar(50) =null
,@Value nvarchar(50) =null	
AS
BEGIN
	
SET NOCOUNT ON;
if @Filter='byUEId'
	BEGIN
	SELECT *from INV_UEIssues where UEId= @Value
    END
END