CREATE Procedure [dbo].[AV_GetNetLayersBySiteCode]

@SiteCode  nvarchar(100),
@UserId NUMERIC(18,0)

AS
BEGIN
--if @Filter='LayersBySiteCode'

	SELECT s.SiteId, s.SiteCode, nls.LayerStatusId, Convert(nvarchar(50),nls.BandId )+'-'+ Convert(nvarchar(50),nls.CarrierId) +'-'+ Convert(nvarchar(50),def.DefinationName) 'NetLayer'
	FROM   AV_Sites s
		INNER JOIN AV_NetLayerStatus nls ON s.SiteId = nls.SiteId 
		INNER JOIN AD_Definations def ON def.DefinationId = s.ScopeId
		inner join Sec_UserScopes us on us.ScopeId=def.DefinationId
		WHERE s.SiteId=@SiteCode and us.UserId=@UserId
		--WHERE s.SiteCode='TNNSSV01' and us.UserId=11
END