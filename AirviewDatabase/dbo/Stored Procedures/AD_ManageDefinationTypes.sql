-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE AD_ManageDefinationTypes
	-- Add the parameters for the stored procedure here 
	     @filter NVARCHAR(50)
        ,@DefinationType  varchar(50)
		,@DefinationTypeId  int
		,@PDefinationTypeId int
		,@Status            bit
AS
BEGIN
	IF @FILTER = 'New'
				BEGIN 
					INSERT INTO [dbo].[AD_DefinationTypes]
           ([DefinationType]
           ,[SortOrder]
           ,[IsActive]
           ,[PDefinationTypeId])
     VALUES
           (@DefinationType
            ,0
           ,@Status
           ,@PDefinationTypeId)
				END
					else IF @FILTER = 'Edite'
				BEGIN 
				UPDATE [dbo].[AD_DefinationTypes]
   SET [DefinationType] = @DefinationType
      ,[SortOrder] = 0
      ,[IsActive] =@Status
      ,[PDefinationTypeId] = @PDefinationTypeId
 WHERE DefinationTypeId = @DefinationTypeId
				END 
End