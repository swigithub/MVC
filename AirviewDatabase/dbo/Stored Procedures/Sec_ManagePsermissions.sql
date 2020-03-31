CREATE PROCEDURE Sec_ManagePsermissions
@Filter nvarchar(50)
,@Id int,
@ParentId int=NULL,
@Title varchar(max)=NULL,
@URL varchar(max)=NULL,
@IsMenuItem bit=NULL,
@Code varchar(max)=NULL,
@Icon varchar(max) =NULL,
@IsUsed varchar(max) =null,
@IsModule bit = null,
@ModuleId numeric(18,0)=NULL
	
AS
BEGIN
 
 if	@Filter='Insert'
  begin
	 insert into Sec_Permissions (Id,ParentId,Title,URL,IsMenuItem,Code,Icon,IsUsed,ModuleId,IsModule)
	 values(@Id,@ParentId,@Title,@URL,@IsMenuItem,@Code,@Icon,@IsUsed,@ModuleId, @IsModule)

 end
 else if	@Filter='Update'
 
 begin
	update Sec_Permissions 
	set Id=@Id,ParentId=@ParentId, Title=@Title, URL=@URL, IsMenuItem =@IsMenuItem, Code= @Code, Icon=@Icon,IsUsed = @IsUsed,ModuleId=@ModuleId, IsModule=@IsModule
	where Id=@Id
 end

  ELSE IF @Filter='set_IsMenuItem'
	begin
		update Sec_Permissions set IsMenuItem=@IsMenuItem where Id=@Id
	END
 ELSE IF @Filter='set_IsUsed'
	begin
		update Sec_Permissions set IsUsed=@IsUsed where Id=@Id
	end
	else IF @Filter='DeleteById'
	begin
		delete from Sec_UserPermissions where PermissionId=@Id
		delete from Sec_RolePermissions where PermissionId=@Id
		delete from Sec_Permissions  where Id=@Id
	end
	
END