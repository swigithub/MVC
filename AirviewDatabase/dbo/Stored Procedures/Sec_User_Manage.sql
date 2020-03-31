-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE Sec_User_Manage --'Delete', 10042
	@Filter varchar(max),
	@UserId int,
	@value varchar(max)=null
	

AS
BEGIN
    

	IF @Filter='Delete'
	BEGIN
		delete from Av_WoDevices where UserId=@UserId
		delete from UserClients where UserId=@UserId
		delete from Sec_UserDateRights where UserId=@UserId
		delete from Sec_UserDevices where UserId=@UserId
		delete from Sec_UserPermissions where UserId=@UserId
		delete from Sec_UserRoles where UserId=@UserId
		delete from Sec_Users where UserId=@UserId
	end

	ELSE IF @Filter='UpdateStatus'
	update Sec_Users set IsActive=@value where UserId=@UserId


	ELSE IF @Filter='UpdatePicture'
	update Sec_Users set Picture=@value where UserId=@UserId

	ELSE IF @Filter='UpdatePassword'
	update Sec_Users set Password=@value where UserId=@UserId


END