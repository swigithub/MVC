CREATE PROCEDURE [dbo].[AD_GetReportConfiguration]
	@Filter NVARCHAR(50)=NULL,
	@value NVARCHAR(50)=NULL,
	@value2 VARCHAR(50)=NULL,
	@value3 varchar(50)=NULL
AS
BEGIN
	PRINT @value
	PRINT @value3
	PRINT @value2
	IF	@Filter='Definations'
	BEGIN
		-- sec as section
		SELECT sec.DefinationId 'SectionId', sec.DefinationName 'SectionName',ad.DefinationId 'KeyId',ad.DefinationName 'KeyName' , ad.KeyCode ,ad.InputType,ad.DisplayType,ad.DisplayText
		FROM AD_Definations AS Sec
		left JOIN AD_Definations AS ad ON ad.PDefinationId=sec.DefinationId AND ad.IsActive=1
		WHERE Sec.PDefinationId=@value AND sec.IsActive=1
	END
-- [dbo].[AD_GetReportConfiguration] 'IsPanelItems_byCityId_ClientId',119,1
	IF	@Filter='byCityId_ClientId'
	BEGIN
	
		-- sec as section
		SELECT DISTINCT rc.KeyId,rc.KeyValue,rc.isActive, rc.KeyCode,def.InputType,def.DisplayText,rc.IsPanelItem,rc.fontColor
		FROM AD_ReportConfiguration AS rc
		INNER JOIN AD_Definations AS def ON def.DefinationId=rc.KeyId
		WHERE rc.CityId=@value AND rc.ClientId=@value2 AND rc.ReportId=444 AND def.IsActive=1
		AND rc.ScopeId=@value3
		
		
		
		
	END
	
	
	
END