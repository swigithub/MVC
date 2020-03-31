-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*
  [dbo].[AV_GetSiteConfiguration] 153114,7,122
  
 */
CREATE PROCEDURE [dbo].[AV_GetSiteConfiguration]
	@Filter NVARCHAR(50),
	@Value1 NVARCHAR(50)=NULL,
	@Value2 NVARCHAR(50)=NULL,
	@Value3 NVARCHAR(50)=NULL,
	@Value4 NVARCHAR(50)=NULL
AS
BEGIN
	declare @ClientId varchar(50)
	declare @CityId varchar(50)

	declare @TestType int
	
	IF @Filter='GET_Configuration_By_Device'
		BEGIN
			select @ClientId=sit.ClientId,@CityId=sit.CityId from AV_Sites sit	
			where sit.SiteId=@Value1

			select conf.Id, conf.ClientId, conf.CityId, conf.RevisionId,
			conf.TestTypeId, conf.KpiId, conf.KpiValue,conf.TestCategoryId,
			conf.ConfigurationDate, cat.DefinationName 'TestCategory',typ.DefinationName 'TestType',kpi.DefinationName 'KPI',ISNULL(kpi.KeyCode,'') 'kpiKey'
			from AV_TestConfigurations conf
			inner join AD_Definations cat on cat.DefinationId=conf.TestCategoryId
			inner join AD_Definations typ on typ.DefinationId=conf.TestTypeId
			inner join AD_Definations kpi on kpi.DefinationId=conf.KpiId
			where ClientId=@ClientId and CityId=@CityId  AND NetworkModeId=@Value2 AND BandId=@Value3
			--And conf.TestTypeId in 
			--(SELECT Adef.MapColumn FROM AV_Netlayerstatus as NLS
			--Inner Join AV_Sitescript ASS
			--On NLS.LayerStatusId = ASS.NetLayerId AND NLS.SiteId = ASS.SiteId
			--Inner Join AD_Definations Adef
			--on Adef.DefinationId = ASS.EventTypeId
			--Inner Join AV_ClusterSchedule ACS
			--On ACS.LayerStatusId = ASS.NetLayerId and ASS.SequenceId = ACS.SequenceId
			--WHERE NLS.SiteId=@Value1 and NLS.NetworkModeId = @Value2 and NLS.BandId = @Value3 and ACS.UserDeviceId = @Value4)
		END
	ELSE	
	IF @Filter='GET_Configuration'
	BEGIN
		

		select @ClientId=sit.ClientId,@CityId=sit.CityId from AV_Sites sit	
		where sit.SiteId=@Value1
		
		print @ClientId+' '+@CityId
		IF(ISNULL(@Value4, 0))<1
		BEGIN
		if (SELECT COUNT(*) from AV_SiteConfigurations where ClientId=@ClientId and SiteId=@Value1 AND NetworkModeId=@Value2 AND BandId=@Value3)>0
		begin	
			select conf.Id, conf.ClientId, conf.CityId, conf.SiteId, conf.RevisionId,
				   conf.TestTypeId, conf.KpiId, conf.KpiValue, conf.TestCategoryId,
				   conf.ConfigurationDate,cat.DefinationName 'TestCategory',typ.DefinationName 'TestType',kpi.DefinationName 'KPI',ISNULL(kpi.KeyCode,'') 'kpiKey'
			from AV_SiteConfigurations conf
			inner join AD_Definations cat on cat.DefinationId=conf.TestCategoryId
			inner join AD_Definations typ on typ.DefinationId=conf.TestTypeId
			inner join AD_Definations kpi on kpi.DefinationId=conf.KpiId
			where conf.SiteId=@Value1
			and conf.RevisionId=(select max(x.RevisionId) from AV_SiteConfigurations x where x.SiteId=@Value1 and x.ClientId=@ClientId AND NetworkModeId=@Value2 AND BandId=@Value3)
			AND conf.ClientId=@ClientId and conf.CityId=@CityId AND NetworkModeId=@Value2 AND BandId=@Value3
		end
		else
		begin
			--print @ClientId+' '+@CityId
			
			IF(Select IsMaster from AV_ClusterSchedule where SiteId = @Value1 and UserDeviceId = @Value4)>0
				BEGIN
					select conf.Id, conf.ClientId, conf.CityId, conf.RevisionId,
					conf.TestTypeId, conf.KpiId, conf.KpiValue,conf.TestCategoryId,
					conf.ConfigurationDate, cat.DefinationName 'TestCategory',typ.DefinationName 'TestType',kpi.DefinationName 'KPI',ISNULL(kpi.KeyCode,'') 'kpiKey'
					from AV_TestConfigurations conf
					inner join AD_Definations cat on cat.DefinationId=conf.TestCategoryId
					inner join AD_Definations typ on typ.DefinationId=conf.TestTypeId
					inner join AD_Definations kpi on kpi.DefinationId=conf.KpiId
					where ClientId=@ClientId and CityId=@CityId
					AND NetworkModeId=@Value2 AND BandId=@Value3
				END
			ELSE
			BEGIN
				
					select conf.Id, conf.ClientId, conf.CityId, conf.RevisionId,
					conf.TestTypeId, conf.KpiId, conf.KpiValue,conf.TestCategoryId,
					conf.ConfigurationDate, cat.DefinationName 'TestCategory',typ.DefinationName 'TestType',kpi.DefinationName 'KPI',ISNULL(kpi.KeyCode,'') 'kpiKey'
					from AV_TestConfigurations conf
					inner join AD_Definations cat on cat.DefinationId=conf.TestCategoryId
					inner join AD_Definations typ on typ.DefinationId=conf.TestTypeId
					inner join AD_Definations kpi on kpi.DefinationId=conf.KpiId
					where ClientId=@ClientId and CityId=@CityId  AND NetworkModeId=@Value2 AND BandId=@Value3
					--And conf.TestTypeId in 
					--(SELECT Adef.MapColumn FROM AV_Netlayerstatus as NLS
					--Inner Join AV_Sitescript ASS
					--On NLS.LayerStatusId = ASS.NetLayerId AND NLS.SiteId = ASS.SiteId
					--Inner Join AD_Definations Adef
					--on Adef.DefinationId = ASS.EventTypeId
					--Inner Join AV_ClusterSchedule ACS
					--On ACS.LayerStatusId = ASS.NetLayerId and ASS.SequenceId = ACS.SequenceId
					--WHERE NLS.SiteId=@Value1 and NLS.NetworkModeId = @Value2 and NLS.BandId = @Value3 and ACS.UserDeviceId = @Value4)
				END
					
			
						--select conf.Id, conf.ClientId, conf.CityId, conf.RevisionId,
						--conf.TestTypeId, conf.KpiId, conf.KpiValue,conf.TestCategoryId,
						--conf.ConfigurationDate, cat.DefinationName 'TestCategory',typ.DefinationName 'TestType',kpi.DefinationName 'KPI',ISNULL(kpi.KeyCode,'') 'kpiKey'
						--from AV_TestConfigurations conf
						--inner join AD_Definations cat on cat.DefinationId=conf.TestCategoryId
						--inner join AD_Definations typ on typ.DefinationId=conf.TestTypeId
						--inner join AD_Definations kpi on kpi.DefinationId=conf.KpiId
						--where ClientId=@ClientId and CityId=@CityId  AND NetworkModeId=@Value2 AND BandId=@Value3
						--And conf.TestTypeId in 
						--(SELECT Adef.MapColumn FROM AV_Netlayerstatus as NLS
						--Inner Join AV_Sitescript ASS
						--On NLS.LayerStatusId = ASS.NetLayerId AND NLS.SiteId = ASS.SiteId
						--Inner Join AD_Definations Adef
						--on Adef.DefinationId = ASS.EventTypeId
						--Inner Join AV_ClusterSchedule ACS
						--On ACS.LayerStatusId = ASS.NetLayerId and ASS.SequenceId = ACS.SequenceId
						--WHERE NLS.SiteId=@Value1 and NLS.NetworkModeId = @Value2 and NLS.BandId = @Value3 and ACS.UserDeviceId = @Value4)
				
			
		  

			 --select conf.Id, conf.ClientId, conf.CityId, conf.RevisionId,
				--	 conf.TestTypeId, conf.KpiId, conf.KpiValue,conf.TestCategoryId,
				--	  conf.ConfigurationDate, cat.DefinationName 'TestCategory',typ.DefinationName 'TestType',kpi.DefinationName 'KPI',ISNULL(kpi.KeyCode,'') 'kpiKey'
				--from AV_TestConfigurations conf
				--inner join AD_Definations cat on cat.DefinationId=conf.TestCategoryId
				--inner join AD_Definations typ on typ.DefinationId=conf.TestTypeId
				--inner join AD_Definations kpi on kpi.DefinationId=conf.KpiId
			 --  where ClientId=1 and CityId=103  AND NetworkModeId=@Value2 AND BandId=@Value3
			 --  and conf.TestTypeId = (SELECT ADF.MapColumn FROM AV_Sitescript AVS 
				--Inner Join AD_Definations ADF
				--ON ADF.DefinationId = AVS.EventTypeId
				--WHERE AVS.SiteId=@Value1)
			end
		END
		ELSE
		BEGIN
			
					select conf.Id, conf.ClientId, conf.CityId, conf.RevisionId,
					conf.TestTypeId, conf.KpiId, conf.KpiValue,conf.TestCategoryId,
					conf.ConfigurationDate, cat.DefinationName 'TestCategory',typ.DefinationName 'TestType',kpi.DefinationName 'KPI',ISNULL(kpi.KeyCode,'') 'kpiKey'
					from AV_TestConfigurations conf
					inner join AD_Definations cat on cat.DefinationId=conf.TestCategoryId
					inner join AD_Definations typ on typ.DefinationId=conf.TestTypeId
					inner join AD_Definations kpi on kpi.DefinationId=conf.KpiId
					where ClientId=@ClientId and CityId=@CityId  AND NetworkModeId=@Value2 AND BandId=@Value3
					--And conf.TestTypeId in 
					--(SELECT Adef.MapColumn FROM AV_Netlayerstatus as NLS
					--Inner Join AV_Sitescript ASS
					--On NLS.LayerStatusId = ASS.NetLayerId AND NLS.SiteId = ASS.SiteId
					--Inner Join AD_Definations Adef
					--on Adef.DefinationId = ASS.EventTypeId
					--Inner Join AV_ClusterSchedule ACS
					--On ACS.LayerStatusId = ASS.NetLayerId and ASS.SequenceId = ACS.SequenceId
					--WHERE NLS.SiteId=@Value1 and NLS.NetworkModeId = @Value2 and NLS.BandId = @Value3 and ACS.UserDeviceId = @Value4)
		END
	END
	ELSE IF @Filter='GET_SCRIPT_SETTINGS'
	BEGIN
		/*
		 * @Value1: PDefinitionId
		 * @Value2: SiteId
		 * @Value3: NetworkModeId
		 * @Value4: BandId
		 */		 
		 
		 SELECT @ClientId=as1.ClientId,@CityId=as1.CityId
		 FROM AV_Sites AS as1 WHERE as1.SiteId=@Value2
		 
		 
		--SELECT ad.DefinationName 'TestType','' 'KPI',ad.DefinationTypeId 'KpiValue'
		--  FROM AD_Definations AS ad
		--WHERE ad.DefinationId=@value1		
		--UNION ALL
		--SELECT DISTINCT ad.DefinationName 'TestType',atc.KpiValue 'KPI',ad.DefinationTypeId 'KpiValue'
		--FROM AD_Definations AS ad
		--LEFT JOIN AV_TestConfigurations AS atc ON atc.KpiId=ad.DefinationId
		--WHERE ad.pDefinationId=0--ad.pDefinationId=@value1 --AND atc.SiteId=398360
		--AND atc.NetworkModeId=@value3 AND atc.BandId=@Value4 AND atc.ClientId=@ClientId AND atc.CityId=@CityId	
	END
END