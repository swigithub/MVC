CREATE PROCEDURE [dbo].[TMP_GetNodeSettings] 
	@Filter NVARCHAR(50),
	@Value NVARCHAR(50)=NULL
AS
BEGIN
	SET NOCOUNT ON;


	IF @Filter='GET_BY_NODEID'
	BEGIN
		SELECT * FROM [TMP_NodeSettings] WHERE NodeId = @Value Order by SortOrder
	END
	

END