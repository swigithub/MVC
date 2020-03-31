CREATE PROCEDURE TMP_ManageNodeSettings 
	@Filter varchar(max), 
	@NodeSettingsId NUMERIC(18,0) = null,
	@NodeId NUMERIC(18,0) = null,
	@DefinationId NUMERIC(18,0) = null,
	@KeyName nvarchar(50) = null,
	@MappingId nvarchar(50) = null,
	@Value nvarchar(MAX) = null,
	@Settings nvarchar(Max) = Null,
	@QueryWhereClause nvarchar(Max) = Null,
	@SortOrder int = 0
AS
BEGIN
	
	DECLARE @RETURN_VALUE int = 0 

	IF @Filter='INSERT'
	BEGIN
		INSERT INTO TMP_NodeSettings ( [NodeId] ,[DefinationId] ,[KeyName], [MappedId] ,[Value], [Settings], [SortOrder], [QueryWhereClause] ) VALUES ( @NodeId, @DefinationId, @KeyName,@MappingId, @Value, @Settings, @SortOrder, @QueryWhereClause)

		SET @RETURN_VALUE = SCOPE_IDENTITY()
	
	END

	IF @Filter='UPDATE'
	BEGIN
		
		UPDATE [dbo].[TMP_NodeSettings]
		   SET [NodeId] = @NodeId
			  ,[DefinationId] = @DefinationId
			  ,[KeyName] = @KeyName
			  ,[MappedId] = @MappingId
			  ,[Value] = @Value
			  ,[Settings] = @Settings
			  ,[SortOrder] = @SortOrder
			  ,QueryWhereClause = @QueryWhereClause
		WHERE NodeSettingsId = @NodeSettingsId

		SET @RETURN_VALUE = @NodeSettingsId
	
	END

	IF @Filter='ReportQueryBuilderUPDATE'
	BEGIN
		
		UPDATE [dbo].[TMP_NodeSettings]
		   SET [Value] = @Value,
			  QueryWhereClause = @QueryWhereClause
		WHERE NodeSettingsId = @NodeSettingsId

		SET @RETURN_VALUE = @NodeSettingsId
	
	END


   
END