-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE [dbo].[Sec_ManageRole]
	@Filter varchar(max),
	@RoleId INT=NULL,
	@Name nvarchar(50)=NULL,
	@Description nvarchar(500)=NULL,	
	@ModifyDate datetime=NULL,
	@IsActive BIT=NULL,
	@DefaultUrl NVARCHAR(50)=NULL

	

AS
BEGIN
    DECLARE @RETURN_VALUE int = 0 

	IF @Filter='Insert'
	BEGIN
		insert into Sec_Roles(Name,DESCRIPTION,DefaultUrl,IsActive) values (@Name,@Description,@DefaultUrl,@IsActive)
		set @RETURN_VALUE = SCOPE_IDENTITY()
		-- Add role Permissions
		insert into Sec_RolePermissions(RoleId,PermissionId) select @RETURN_VALUE as RoleId,Id from Sec_Permissions
	END
	
	ELSE IF @Filter='Update'
	BEGIN
		update Sec_Roles set Name=@Name , Description=@Description, IsActive=@IsActive,ModifyDate=@ModifyDate,DefaultUrl = @DefaultUrl where RoleId=@RoleId
		set	@RETURN_VALUE=@RoleId
	END

	ELSE IF @Filter='Delete'
	BEGIN
		delete from Sec_RolePermissions where RoleId=@RoleId
		delete from Sec_Roles where RoleId=@RoleId
	END
	

	else IF @Filter='UpdateStatus'
	BEGIN
	   update Sec_Roles set IsActive=@IsActive where RoleId=@RoleId
	END


	RETURN @RETURN_VALUE;
END