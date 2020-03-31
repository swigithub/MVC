CREATE PROCEDURE [dbo].[TMP_ManageNodes] 
	@Filter varchar(max), 
	@NodeId NUMERIC(18,0) = null,
	@TemplateId NUMERIC(18,0) = null,
	@NodeTitle nvarchar(50) = null,
	@Height int = null,
	@Width int = null,
	@X_axis int = null,
	@Y_axis int = null,
	@PageTyppeId NUMERIC(18,0) = null,
	@NodeUrl nvarchar(50) = null,
	@NodeSQL nvarchar(50) = null,
	@IsActive bit = null
AS
BEGIN
    DECLARE @RETURN_VALUE int = 0 

	IF @Filter='Insert'
	BEGIN
		INSERT INTO TMP_Nodes ( TemplateId, NodeTitle ,Height ,Width ,x_axis ,y_axis ,PageTyppeId ,NodeUrl ,NodeSQL ,IsActive) VALUES ( @TemplateId, @NodeTitle ,@Height ,@Width ,@X_axis ,@Y_axis ,@PageTyppeId ,@NodeUrl ,@NodeSQL ,@IsActive)

		SET @RETURN_VALUE = SCOPE_IDENTITY()
	
	END
	
	ELSE IF @Filter='Update'
	BEGIN
		UPDATE [dbo].[TMP_Nodes]
		   SET [TemplateId] = @TemplateId
			  ,[Height] = @Height
			  ,[Width] = @Width
			  ,[x_axis] = @X_axis
			  ,[y_axis] = @Y_axis
			  ,[IsActive] = @IsActive
		 WHERE NodeId = @NodeId
	END


	ELSE IF @Filter='UpdateRelaventData'
	BEGIN
		UPDATE [dbo].[TMP_Nodes]
		   SET [NodeTitle] = @NodeTitle
		      ,[NodeUrl] = @NodeUrl
			  ,[NodeSQL] = @NodeSQL
		 WHERE NodeId = @NodeId
	END

	ELSE IF @Filter='SoftDelete'
	BEGIN
		UPDATE [dbo].[TMP_Nodes] SET [IsActive] = 'false' WHERE NodeId = @NodeId
	END

	--ELSE IF @Filter='Delete'
	--BEGIN
	--	delete from AD_Projects where ProjectID=@ProjectID

	--END
	

	--ELSE IF @Filter='UpdateStatus'
	--BEGIN
	--   update TMP_Nodes set IsActive=@IsActive where NodeId=@ProjectID
	--END


	RETURN @RETURN_VALUE;
END