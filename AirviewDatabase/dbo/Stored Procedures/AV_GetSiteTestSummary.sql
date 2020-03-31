
CREATE PROCEDURE [dbo].[AV_GetSiteTestSummary] --10064
	@SiteId numeric(18,0)
AS
BEGIN
	
Select smry.*,site.*,cli.Logo 'ClientLogo',ven.Logo 'VendorLogo'
from AV_SiteTestSummary smry
Inner Join AV_Sites site On Site.SiteId = smry.SiteId 
left join AD_Clients cli on cli.ClientId=site.ClientId 
left join AD_Clients ven on ven.ClientId=cli.PClientId
Where smry.SiteId = @SiteId




	select cat.DefinationId 'TestCateoryId',cat.DefinationName 'TestCategory',typ.DefinationId 'TestTypeId',typ.DefinationName 'TestType'
		,kpi.DefinationId 'KpiId',kpi.DefinationName 'Kpi', config.KpiValue
			from AD_Definations cat
		left join AD_Definations typ on typ.PDefinationId=cat.DefinationId
		left join AD_Definations kpi on kpi.PDefinationId=typ.DefinationId
		Left Outer Join AV_SiteConfigurations config On Config.KpiId = kpi.DefinationId
		where cat.DefinationTypeId=16 and config.SiteId=@SiteId

	Select  * from AV_SiteTestLog
	Where SiteId =@SiteId
	and (Latitude<>0.0 and Longitude<>0.0)
	AND TestType='CW'

	Select  * from AV_SiteTestLog
	Where SiteId = @SiteId
	and (Latitude<>0.0 and Longitude<>0.0)
	AND TestType='CCW'
END