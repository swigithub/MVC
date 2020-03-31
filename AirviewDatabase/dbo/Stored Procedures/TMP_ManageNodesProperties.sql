CREATE PROCEDURE TMP_ManageNodesProperties
	-- Parameters
	@Filter varchar(max),
	@FormId numeric(18, 0) = null,
	@NodeTypeId numeric(18, 0) = null,
	@Title nvarchar(50) = null,
	@ControlType nvarchar(50) = null,
	@DataType nvarchar(50) = null,
	@DefaultValue nvarchar(max) = null,
	@MaxLength nvarchar(50) = null,
	@Required nvarchar(50) = null,
	@IsAttachment nvarchar(50) = null,
	@SortOrder int = null,
	@IsDeleted int = 0,
   @Comments nvarchar(50) = null
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @RETURN_VALUE int = 0 

    -- Insert statements for procedure here
	
	IF @Filter = 'Insert'
		BEGIN
		
		if(@FormId >0)
		begin
		update [TMP_NodesProperties] set IsDeleted=@IsDeleted,Title=@Title,DefaultVALUE=@DefaultValue,[Required]=@Required,[MaxLength]=@MaxLength,[IsAttachment]=@IsAttachment,[SortOrder]=@SortOrder,[Comments]=@Comments
		WHERE formid=@FormId
		end
		else if(@IsDeleted =0)
		begin
		INSERT INTO [dbo].[TMP_NodesProperties] ([NodeTypeId] ,[Title] ,[ControlType] ,[DataType] ,[DefaultValue] ,[MaxLength] ,[Required] ,[IsAttachment], [SortOrder],IsDeleted,Comments) VALUES ( @NodeTypeId, @Title, @ControlType, @DataType, @DefaultValue, @MaxLength, @Required,@IsAttachment, @SortOrder,@IsDeleted,@Comments)

		set @RETURN_VALUE = SCOPE_IDENTITY()	
		end
	END


	ELSE IF @Filter = 'Update'
	BEGIN

	UPDATE [dbo].[TMP_NodesProperties]
		SET [NodeTypeId] = @NodeTypeId
		  ,[Title] = @Title
		  ,[ControlType] = @ControlType
		  ,[DataType] = @DataType
		  ,[DefaultValue] = @DefaultValue
		  ,[MaxLength] = @MaxLength
		  ,[Required] = @Required
		  ,[IsAttachment] = @IsAttachment
		  ,[SortOrder] = @SortOrder
		WHERE [FormId] = @FormId

		SET	@RETURN_VALUE = @FormId
	END

	ELSE IF @Filter = 'Delete'
	BEGIN

		DELETE FROM [dbo].[TMP_NodesProperties] WHERE [NodeTypeId] = @NodeTypeId

	END

END