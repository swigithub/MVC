-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AD_GetClientAddress]
	@Filter NVARCHAR(50),
	@value NVARCHAR(50)=NULL
AS
BEGIN
	
--  [dbo].[AD_GetClientAddress] 'ById',1
	
	 if	@Filter='ById'
	BEGIN
		Select cli.* 
		from AD_ClientAddress cli
		
		WHERE cli.ClientId=@value AND IsActive=1
	END

END