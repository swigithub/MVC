-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE [dbo].[Sec_User_Insert_Update] --27,1,'M.Muzzammil','Abdul Rasheed','Mobi','','lahore','0300000','muzzammil.siam@gmail.com','2016-10-28 12:54:10.390'

	@UserId numeric(18, 0),
	@RoleId numeric(18, 0),
	@FirstName nvarchar(50),
	@LastName nvarchar(150)=null,
	@UserName nvarchar(50),
	@Password nvarchar(50),
	@Address nvarchar(250)=null,
	@Contact nvarchar(50)=null,
	@Email nvarchar(50),	
	@Update_at datetime
	,@homeLatitude float
	,@homeLongitude float,
	 @Title nvarchar(250)=null,
	 @Gender nvarchar(250)=null
	 ,@CompanyId numeric (18,0)=0,
	 @Designation nvarchar(250)=null
	 ,@HiringDate nvarchar(250)=null
	 ,@ReportToId numeric(18,0)=0
	 ,@Color nvarchar(50)=null,
	 @IsManager bit=0
	

AS
BEGIN
    
	DECLARE @RETURN_VALUE int = 0 


	IF @UserId > 0 -- For Update
			BEGIN
				update Sec_Users set FirstName=@FirstName , LastName=@LastName,Address=@Address,Contact=@Contact,
					   Email=@Email,ModifyDate=@Update_at,homeLatitude=@homeLatitude,homeLongitude=@homeLongitude,Gender=@Gender,Color=@Color,ReportToId=@ReportToId,Designation=@Designation,CompanyId=@CompanyId,Title=@Title,HiringDate=@HiringDate where UserId=@UserId
				set	@RETURN_VALUE=@UserId
			END

    ELSE IF EXISTS(select UserName from Sec_Users where UserName=@UserName)
	begin
		 set	@RETURN_VALUE=-1
		 return @RETURN_VALUE -- Return -1 if Username Already Exist
	 end
	 ELSE IF EXISTS(select Email from Sec_Users where Email=@Email)
		begin
			 set	@RETURN_VALUE=-2
			 return @RETURN_VALUE -- Return -2 if Email Already Exist
		 end

	else begin -- For Insert
		insert into Sec_Users(FirstName,LastName,UserName,Password,Address,Contact,Email,IsActive,homeLatitude,homeLongitude,Gender,Color,ReportToId,Designation,CompanyId,Title,HiringDate,IsManager) 
				values (@FirstName,@LastName,@UserName,@Password,@Address,@Contact,@Email,'true',@homeLatitude,@homeLongitude,@Gender,@Color,@ReportToId,@Designation,@CompanyId,@Title,GETDATE(),@IsManager)
		SELECT @RETURN_VALUE = SCOPE_IDENTITY()	


		-- Add User Permissions
		insert into Sec_UserPermissions(UserId,PermissionId) select @RETURN_VALUE as RoleId,PermissionId from Sec_RolePermissions where RoleId=@RoleId
	end

	     delete from Sec_UserRoles where UserId=@RETURN_VALUE
		 insert into Sec_UserRoles (UserId,RoleId) values(@RETURN_VALUE,@RoleId)

	
	RETURN @RETURN_VALUE;
END