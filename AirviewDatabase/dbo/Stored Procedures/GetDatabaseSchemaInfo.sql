create PROCEDURE GetDatabaseSchemaInfo
	@Filter nvarchar(255) = null,
	@TableName nvarchar(255) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if	@Filter='Tables'
	BEGIN
		SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
	END 

	ELSE IF	@Filter='Table_Columns'
	BEGIN
		SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName
	END 

	ELSE IF	@Filter='Views'
	BEGIN
		SELECT Name FROM sys.views
	END 

	ELSE IF	@Filter='Views_Columns'
	BEGIN
		SELECT COLUMN_NAME, DATA_TYPE, typ.DefinationType FROM INFORMATION_SCHEMA.COLUMNS cols
		left join AD_DefinationTypes typ on typ.DefinationType = cols.COLUMN_NAME
		WHERE cols.TABLE_NAME = @TableName
	END 
END