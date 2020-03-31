-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_Optimization]
@Filter nvarchar(50),
@whereClause nvarchar(250),
@SiteId NUMERIC(18,0)=NULL,
@NetworkLayeId NUMERIC(18,0)=NULL,
@SitePci List READONLY,
@RfLegend List READONLY
AS
BEGIN

	select * from @RFLegend
END