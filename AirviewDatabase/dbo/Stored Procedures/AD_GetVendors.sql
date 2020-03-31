-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AD_GetVendors]
	@Filter NVARCHAR(50),
	@value NVARCHAR(50)=NULL
AS
BEGIN
	
   if @Filter='AllVendors'
	BEGIN
		select cl.*,def.DefinationName from AD_Clients cl
		inner join AD_Definations def on def.DefinationId=cl.ClientTypeId
		where DefinationName ='Vendor'
	END

	 if @Filter='Company'
	  BEGIN
		select cl.*,def.DefinationName from AD_Clients cl
		inner join AD_Definations def on def.DefinationId=cl.ClientTypeId
		where DefinationName ='Vendor' and PClientId is NULL
	END	
END