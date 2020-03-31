CREATE PROCEDURE TMP_ManageTemplates
	@Filter varchar(max), 
	@TemplateId int = null,
	@TemplateTitle nvarchar(50) = null,
	@ProjectId int = null,
	@ScopeId int = null,
	@backgroundColor nvarchar(50) = null,
	@pageType nvarchar(50) = null,
	@parameter nvarchar(max) = null,
	@IsActive bit = null,
	@TemplateType nvarchar(50) = null,
	@IsDefault bit = null,
	@ModuleId numeric(18, 0) = null
AS
BEGIN
	SET NOCOUNT ON;

     DECLARE @RETURN_VALUE int = 0
	 DECLARE @OldTemplateId int = 0
	IF @Filter='Insert'
	BEGIN
		
		INSERT INTO [dbo].[TMP_Templates] ([TemplateTitle] ,[ProjectId] ,[ScopeId],[BackgroundColor], [PageType] ,[Parameters],[IsActive], [TemplateType], [IsDefault], [ModuleId]) VALUES (@TemplateTitle ,@ProjectId ,@ScopeId, @backgroundColor, @pageType,@parameter ,@IsActive, @TemplateType, @IsDefault, @ModuleId)

		SET @RETURN_VALUE = SCOPE_IDENTITY()
	
	END
	
	ELSE IF @Filter='Update'
	BEGIN
		UPDATE [dbo].[TMP_Templates]
		   SET [TemplateTitle] = @TemplateTitle
			  ,[ProjectId] = @ProjectId
			  ,[ScopeId] = @ScopeId
			  ,[BackgroundColor] = @backgroundColor
			  ,[PageType] = @pageType
			  ,[Parameters] = @parameter
			  ,[TemplateType] = @TemplateType
			  ,[ModuleId] = @ModuleId
			   WHERE [TemplateId] = @TemplateId
	END

	ELSE IF @Filter='Disable'
	BEGIN
		UPDATE [dbo].[TMP_Templates]
		   SET [IsDefault] = 0
			  ,[IsActive] = 0
			   WHERE [TemplateId] = @TemplateId
	END

	ELSE IF @Filter='Active'
	BEGIN
		UPDATE [dbo].[TMP_Templates]
		   SET [IsDefault] = 0
			  ,[IsActive] = 1
			   WHERE [TemplateId] = @TemplateId
	END

	ELSE IF @Filter='MakeTemplateDefault'
	BEGIN
		UPDATE [dbo].[TMP_Templates] SET [IsDefault] = 0 WHERE ModuleId = @ModuleId AND  ProjectId = @ProjectId
		UPDATE [dbo].[TMP_Templates] SET [IsDefault] = 1, [IsActive] = 1 WHERE TemplateId = @TemplateId
	END

	ELSE IF @Filter='MakeTemplateUndefault'
	BEGIN
		UPDATE [dbo].[TMP_Templates] SET [IsDefault] = 0 WHERE ModuleId = @ModuleId AND  ProjectId = @ProjectId AND TemplateId = @TemplateId
	END


	
END