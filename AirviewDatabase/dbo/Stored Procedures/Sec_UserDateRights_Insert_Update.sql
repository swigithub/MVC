-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
CREATE PROCEDURE [dbo].[Sec_UserDateRights_Insert_Update] 
   
	@UserId int,
	@DaysForward int,
	@DaysBack int,	
	@AssignDate datetime,
	@IsActive bit

AS
BEGIN
    
	DECLARE @RETURN_VALUE int = 0 	


	        if EXISTS(select  UserId from Sec_UserDateRights where UserId=@UserId)  -- For Update
				BEGIN
				update Sec_UserDateRights set DaysForward=@DaysForward , DaysBack=@DaysBack, IsActive=@IsActive,AssignDate=@AssignDate where UserId=@UserId
				set	@RETURN_VALUE=@UserId
				END  

		   else begin -- For Insert
	   
			   insert into Sec_UserDateRights (UserId,DaysForward,DaysBack,AssignDate,IsActive) values (@UserId,@DaysForward,@DaysBack,@AssignDate,@IsActive)
			   set @RETURN_VALUE = @UserId
		   end

	
	RETURN @RETURN_VALUE;
END