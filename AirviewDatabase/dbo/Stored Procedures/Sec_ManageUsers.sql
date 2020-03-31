-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE Sec_ManageUsers --'Delete', 10042
	@Filter varchar(max),
	@value1 varchar(max)=NULL,
	@value2 varchar(max)=NULL,
	@value3 nvarchar(50)=NULL,
	@value4 varchar(max)=NULL
	
	

AS
BEGIN
    

	IF @Filter='Delete'
	BEGIN
		delete from Av_WoDevices where UserId=@value1
		delete from UserClients where UserId=@value1
		delete from Sec_UserDateRights where UserId=@value1
		delete from Sec_UserDevices where UserId=@value1
		delete from Sec_UserPermissions where UserId=@value1
		delete from Sec_UserRoles where UserId=@value1
		delete from Sec_Users where UserId=@value1
	end

	ELSE IF @Filter='UpdateStatus'
	update Sec_Users set IsActive=@value2 where UserId=@value1


	ELSE IF @Filter='UpdatePicture'
	update Sec_Users set Picture=@value2 where UserId=@value1

	ELSE IF @Filter='UpdatePassword'
	update Sec_Users set Password=@value2 where UserId=@value1
	
	
	
	
--	[dbo].[Sec_ManageUsers] 'ChangePassword',10034,'356095064009947','kJmj2aYaCnA=','sdfsd'
	ELSE IF @Filter='ChangePassword'
	BEGIN
		IF	EXISTS(select * from Sec_UserDevices where IMEI=@value2 AND UserId=@value1)
			BEGIN
				IF  @value3=(SELECT [PASSWORD] FROM Sec_Users AS su WHERE su.UserId=@value1)
					BEGIN
					 update Sec_Users set Password=@value4 where UserId=@value1
					END
				ELSE 
					BEGIN
				     	RAISERROR('wrong old password',16,1)
				    END
				
			END
		ELSE 
			BEGIN
		     	RAISERROR('Device not found',16,1)
		     END
	END


END