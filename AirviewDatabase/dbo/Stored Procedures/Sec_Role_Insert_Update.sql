-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE [dbo].[Sec_Role_Insert_Update] 
	@RoleId int,
	@Name nvarchar(50),
	@Description nvarchar(500),	
	@ModifyDate datetime,
	@IsActive bit

AS
BEGIN
    
	DECLARE @RETURN_VALUE int = 0 
	


	


	IF @RoleId > 0 -- For Update
			BEGIN
			update Sec_Roles set Name=@Name , Description=@Description, IsActive=@IsActive,ModifyDate=@ModifyDate where RoleId=@RoleId
		    set	@RETURN_VALUE=@RoleId
			END
    ELSE IF EXISTS(select Name from Sec_Roles where Name=@Name) return @RETURN_VALUE

	else begin -- For Insert
		insert into Sec_Roles(Name,Description,IsActive) values (@Name,@Description,@IsActive)
		set @RETURN_VALUE = SCOPE_IDENTITY()

		-- Add role Permissions
		insert into Sec_RolePermissions(RoleId,PermissionId) select @RETURN_VALUE as RoleId,Id from Sec_Permissions

	end

	
	RETURN @RETURN_VALUE;
END