-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
create FUNCTION MultiUserIdsToName
(@value as nvarchar(Max)='' 

)RETURNS varchar(Max)
AS
BEGIN
	declare @iteration2 int =1,@IdCount2 int;
	Declare @TempTableForLoop2 Table(Id int IDENTITY(1,1),UName varchar(max))
	Declare @groups varchar(max)='',@UserName varchar(max)
	set @value=@value+','
		insert into @TempTableForLoop2 select DISTINCT pmg.FirstName +' '+pmg.LastName as 'Item' from Sec_Users as pmg  where  Charindex(cast(pmg.UserId as varchar(max))+',',@value) >0
	        set @IdCount2=(select COUNT(*) from @TempTableForLoop2)
			while @iteration2<=@IdCount2
				begin
						select @UserName=UName from (
						select  Id,UName from @TempTableForLoop2
						) as gg where gg.Id=@iteration2
                        set @groups=@groups+','+CONVERT(nvarchar(1000),@UserName);
						set @iteration2=@iteration2+1;
                end

			RETURN @groups

END