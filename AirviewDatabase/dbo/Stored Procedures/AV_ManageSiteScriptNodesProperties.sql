create  PROCEDURE AV_ManageSiteScriptNodesProperties
	-- Parameters
	@Filter varchar(max),
	@FormId numeric(18, 0) = null,
	@NodeTypeId numeric(18, 0) = null,
	@Title nvarchar(50) = null,
	@DataType nvarchar(50) = null,
	@DefaultValue nvarchar(max) = null,
	@MaxLength nvarchar(50) = null,
	@Required nvarchar(50) = null,
	@SortOrder int = null,
	@IsDeleted int = 0,
	@Revision numeric(18, 0) = 0,
	@ActualValue nvarchar(max) = null,
	@List TaskList readonly
	


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
		update [AV_SiteScriptFormEntry] set IsDeleted=@IsDeleted,Title=@Title,DefaultVALUE=@DefaultValue,[Required]=@Required,[MaxLength]=@MaxLength,[SortOrder]=@SortOrder
		WHERE formid=@FormId
		end
		else if(@IsDeleted =0)
		begin
		INSERT INTO [dbo].[AV_SiteScriptFormEntry] ([NodeTypeId] ,[Title] ,[DataType] ,[DefaultValue] ,[MaxLength] ,[Required] , [SortOrder],IsDeleted) VALUES ( @NodeTypeId, @Title, @DataType, @DefaultValue, @MaxLength, @Required, @SortOrder,@IsDeleted)

		set @RETURN_VALUE = SCOPE_IDENTITY()	
		end
	END

	IF @Filter = 'Update_UI'
	BEGIN

	
	BEGIN TRANSACTION	
	
	DECLARE addSite CURSOR READ_ONLY
		FOR
		SELECT l.Value1,l.Value2
		 FROM @List l
		--OPEN CURSOR
		OPEN addSite 
		 
		--FETCH THE RECORD INTO THE VARIABLES.
		FETCH NEXT FROM addSite INTO @FormId,@ActualValue
		--LOOP UNTIL RECORDS ARE AVAILABLE.
		WHILE @@FETCH_STATUS = 0
		BEGIN
		--	BEGIN TRANSACTION				 
	    print('2')
		if (@FormId > 0)
		begin
		update  AV_SiteScriptFormEntry set ActualValue=@ActualValue
		where FormId=@FormId
		end
		
				--COMMIT
			--FETCH THE NEXT RECORD INTO THE VARIABLES.
			FETCH NEXT FROM addSite INTO @FormId,@ActualValue
		END 
		--CLOSE THE CURSOR.
		CLOSE addSite
		DEALLOCATE addSite

		COMMIT
	END

	ELSE IF @Filter = 'Update'
	BEGIN

	UPDATE [dbo].[AV_SiteScriptFormEntry]
		SET [NodeTypeId] = @NodeTypeId
		  ,[Title] = @Title
		  ,[DataType] = @DataType
		  ,[DefaultValue] = @DefaultValue
		  ,[MaxLength] = @MaxLength
		  ,[Required] = @Required
		  ,[SortOrder] = @SortOrder
		WHERE [FormId] = @FormId

		SET	@RETURN_VALUE = @FormId
	END

	ELSE IF @Filter = 'Delete'
	BEGIN

		DELETE FROM [dbo].[AV_SiteScriptFormEntry] WHERE [NodeTypeId] = @NodeTypeId

	END

END