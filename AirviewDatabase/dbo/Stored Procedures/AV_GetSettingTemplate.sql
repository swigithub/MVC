

CREATE PROCEDURE [dbo].[AV_GetSettingTemplate]
	@Filter NVARCHAR(50)=NULL,
	@value1 NVARCHAR(50)=NULL,
	@value2 NVARCHAR(50)=NULL,
	@value3 NVARCHAR(50)=NULL,
	@value4 NVARCHAR(50)=NULL,
	@value5 NVARCHAR(50)=NULL
AS
BEGIN
--  [dbo].[AV_GetSettingTemplate] 'Template'
	
	IF @Filter='Template'
	BEGIN
		 select cat.DefinationId 'TestCategoryId',cat.DefinationName 'TestCategory',typ.DefinationId 'TestTypeId',typ.DefinationName 'TestType'
		,kpi.DefinationId 'KpiId',kpi.DefinationName 'Kpi'
		 from AD_Definations cat
		left join AD_Definations typ on typ.PDefinationId=cat.DefinationId
		left join AD_Definations kpi on kpi.PDefinationId=typ.DefinationId
		where cat.DefinationTypeId=16 and typ.IsActive=1
	END
	
--  [dbo].[AV_GetSettingTemplate] 'byClinetAndCityId',1,5


   ELSE	IF @Filter='byClinet_City_Net_Band'
	BEGIN
		select conf.*, cat.DefinationName 'TestCategory',typ.DefinationName 'TestType',kpi.DefinationName 'KPI',ISNULL(kpi.KeyCode,'') 'kpiKey'
		from AV_TestConfigurations conf
		inner join AD_Definations cat on cat.DefinationId=conf.TestCategoryId
		inner join AD_Definations typ on typ.DefinationId=conf.TestTypeId
		inner join AD_Definations kpi on kpi.DefinationId=conf.KpiId
	    where ClientId=@value1 and CityId=@value2 AND conf.NetworkModeId=@value3 AND conf.BandId=@value4
	END
	
END