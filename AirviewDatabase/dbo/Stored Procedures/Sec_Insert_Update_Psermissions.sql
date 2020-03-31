

CREATE PROCEDURE [dbo].[Sec_Insert_Update_Psermissions] 
@Id int,
@ParentId int,
@Title varchar(max),
@URL varchar(max),
@IsMenuItem bit,
@Code varchar(max) ,
@Icon varchar(max) =NULL,
@IsUsed varchar(max) =null
	
AS
BEGIN
 

 if	 EXISTS(select Id from Sec_Permissions where Id=@Id)
      update Sec_Permissions set Id=@Id,ParentId=@ParentId, Title=@Title, URL=@URL, IsMenuItem =@IsMenuItem, Code= @Code, Icon=@Icon,IsUsed = @IsUsed where Id=@Id

else	   
insert into Sec_Permissions (Id,ParentId,Title,URL,IsMenuItem,Code,Icon,IsUsed) values(@Id,@ParentId,@Title,@URL,@IsMenuItem,@Code,@Icon,@IsUsed)
	
END