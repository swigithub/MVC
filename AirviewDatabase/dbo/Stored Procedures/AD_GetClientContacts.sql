-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AD_GetClientContacts]
	@Filter NVARCHAR(50),
	@value NVARCHAR(50)=NULL
AS
BEGIN
	IF @Filter='AllActive'
	BEGIN
		SELECT ac.*
		FROM AD_ClientContacts AS ac		
		WHERE ac.IsActive=1
	END

	else IF @FILTER = 'UserAll'
				BEGIN
					Select u.*,r.RoleId,r.Name 'RoleName'
					from Sec_Users u
					inner join Sec_UserRoles ur on ur.UserId=u.UserId
					inner join Sec_Roles r on r.RoleId=ur.RoleId
					--WHERE u.IsActive=1
				END

--	[dbo].[AD_GetClientAddress] 'ById',20108

		else IF @FILTER = 'ById'
				BEGIN
					Select cli.* 
		              from AD_ClientContacts cli
		
		        WHERE cli.ClientId=@value and IsActive=1
				END
END