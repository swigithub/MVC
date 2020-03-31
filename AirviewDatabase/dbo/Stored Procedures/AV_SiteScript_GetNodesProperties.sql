create PROCEDURE AV_SiteScript_GetNodesProperties 
	@Filter NVARCHAR(50),
	@Value NVARCHAR(50)=0
AS
BEGIN
	SET NOCOUNT ON;


	IF @Filter='ByNodeTypeId'
	BEGIN
	print(@Value)
		SELECT * FROM AV_SiteScriptFormEntry WHERE NodeTypeId = @Value and isdeleted=0
	END

	ELSE IF @Filter='GET_ALL'
	BEGIN
		SELECT * FROM AV_SiteScriptFormEntry WHERE  isdeleted=0
	END
    
	ELSE IF @Filter='GET_FORM_DATA'
	BEGIN

		SELECT 
			np.NodeTypeId,
			np.Title, 
			(SELECT DefinationName FROM AD_Definations WHERE DefinationId = np.DataType) DefinationName 
		FROM [TMP_Nodes] as n INNER JOIN [AV_SiteScriptFormEntry] as np ON np.NodeTypeId = n.PageTyppeId 
		WHERE NodeId = @Value and  np.isdeleted=0
	END

END