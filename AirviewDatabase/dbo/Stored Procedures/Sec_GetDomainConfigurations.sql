-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
/*
		Sec_GetDomainConfigurations 'DeviceConfig'
 */
 
CREATE PROCEDURE [dbo].[Sec_GetDomainConfigurations]
	@Filter NVARCHAR(50)
AS
BEGIN
	IF @Filter='DeviceConfig'
	BEGIN
		SELECT * FROM Sec_DomainConfigurations AS sdc
	END
END