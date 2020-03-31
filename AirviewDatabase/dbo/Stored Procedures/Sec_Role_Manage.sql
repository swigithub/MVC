-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE [dbo].[Sec_Role_Manage]
	@Filter varchar(max),
	@Id int,
	@Status bit
	

AS
BEGIN
    

	IF @Filter='Delete'
	delete from Sec_Roles where RoleId=@Id

	else IF @Filter='UpdateStatus'
	update Sec_Roles set IsActive=@Status where RoleId=@Id


END