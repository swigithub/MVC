-- =============================================
-- Author:		M.Mubashar Rafqiue
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AD_ManageClients]
	@Filter varchar(max),
	@ClientId decimal=NULL,
	@ClientName nvarchar(50)=NULL,
	@IsActive bit=NULL,	
	@Logo nvarchar(max)=NULL,
	@ClientTypeId decimal=NULL,
	@PClientId decimal=NULL,
	@ClientPrefix nvarchar(50)=NULL
AS
BEGIN
	 DECLARE @RETURN_VALUE int = 0 

	IF @Filter='Insert'
	BEGIN
		insert into AD_Clients(ClientName,IsActive,Logo,ClientTypeId,PClientId,ClientPrefix) values (@ClientName,@IsActive,@Logo,@ClientTypeId,@PClientId,@ClientPrefix)
		set @RETURN_VALUE = SCOPE_IDENTITY()	
	END
	ELSE IF @Filter='Update'
	BEGIN
		update AD_Clients set ClientName=@ClientName , IsActive=@IsActive, Logo=@Logo,ClientTypeId=@ClientTypeId,PClientId = @PClientId, ClientPrefix=@ClientPrefix where ClientId=@ClientId
		set	@RETURN_VALUE=@ClientId
	END

	ELSE IF @Filter='Delete'
	BEGIN
		
		delete from AD_Clients where ClientId=@ClientId
	END
		IF @Filter='IsActive'
		BEGIN
			update AD_Clients set IsActive=  CASE WHEN  IsActive = 0 THEN 1 ELSE 0 END   where ClientId=@ClientId
		END
	RETURN @RETURN_VALUE;
END