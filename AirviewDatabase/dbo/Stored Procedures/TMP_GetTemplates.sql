CREATE PROCEDURE TMP_GetTemplates
	@Filter NVARCHAR(50),
	@Value NVARCHAR(50)=NULL,
	@ProjectId numeric(18, 0)=0
AS
BEGIN
	SET NOCOUNT ON;

	IF @Filter='GET_ALL'
	BEGIN
		SELECT *, def.DefinationId AS 'ModuleId', def.DefinationName as 'ModuleType'  FROM TMP_Templates t 
		JOIN dbo.AD_Definations def
		on def.DefinationId = t.ModuleId
	END 
	ELSE IF @Filter='GetById'
	BEGIN
		SELECT *, def.DefinationId AS 'ModuleId', def.DefinationName as 'ModuleType'  FROM TMP_Templates t
		JOIN dbo.AD_Definations def
		on def.DefinationId = t.ModuleId
		WHERE TemplateId = @Value 
	END

	ELSE IF @Filter='GetTemplateTypeByNodeId'
	BEGIN
		SELECT t.TemplateType from dbo.TMP_Nodes n 
		join tmp_templates t on t.TemplateId = n.templateId
		WHERE n.NodeId = @Value
	END
	ELSE IF @Filter='IsTemplateExist'
	BEGIN
		SELECT temp.IsActive, temp.IsDefault, * from   AD_Definations def
		INNER JOIN TMP_Templates temp 
		ON temp.ModuleId = def.DefinationId AND def.KeyCode = @Value
		WHERE temp.IsDefault = 1 AND temp.IsActive = 1
	END
	ELSE IF @Filter='GetProjectIdBySiteId'
	BEGIN
		select ProjectId from AV_Sites WHERE SiteId = @Value
	END
	ELSE IF @Filter='GetScopeIdByScopeName'
	BEGIN
		select DefinationId as 'ScopeId' from AD_Definations where DefinationName = @Value
	END

END