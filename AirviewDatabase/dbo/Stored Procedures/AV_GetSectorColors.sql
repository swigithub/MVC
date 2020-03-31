--  [dbo].[AV_GetSectorColors] 'GetSectorColors','11'
CREATE PROCEDURE [dbo].[AV_GetSectorColors]
@Filter nvarchar(100),
@UserId NUMERIC(18,0) =0

AS
BEGIN

	IF @Filter='GetSectorColors'
	 BEGIN
		SELECT s.*, def.DefinationName 'Scope' 
		FROM AV_SectorColors s 
		INNER JOIn AD_Definations def on def.DefinationId= s.ScopeId
	END
		
END