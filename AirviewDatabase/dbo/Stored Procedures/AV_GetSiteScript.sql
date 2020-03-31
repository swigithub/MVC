-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_GetSiteScript]
	 @Filter NVARCHAR(50)
	,@Value1 NVARCHAR(50)=NULL
	,@Value2 NVARCHAR(50)=NULL
	,@Value3 NVARCHAR(50)=NULL
	,@Value4 NVARCHAR(50)=NULL
	,@Value5 NVARCHAR(50)=NULL
	,@Value6 NVARCHAR(50)=NULL
AS
BEGIN
--	[dbo].[AV_GetSiteScript] 'ByNetworkLayer',398379,15,74,125,36
	IF @Filter='ByNetworkLayer'
	BEGIN
		IF ISNULL(@Value6,0)=0
		BEGIN		
			--SELECT def.KeyCode 'Event',
			--(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventCommand				
			--WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventCommand
			--ELSE ss.EventValue END) 'EventValue',
			--ss.IsValue ,ss.IsL3Enabled,ss.SequenceId,pdef.DefinationId 'PEventId',pdef.DefinationName 'PEvent',def.DefinationId 'CEventId',def.DefinationName 'CEvent'
			--FROM AV_SiteScript AS ss
			--INNER JOIN AV_NetLayerStatus AS nls ON nls.LayerStatusId=ss.NetLayerId
			--INNER JOIN AD_Definations AS def ON def.DefinationId=ss.EventTypeId
			--LEFT JOIN AD_Definations AS pdef ON pdef.DefinationId=def.PDefinationId
			--WHERE nls.SiteId=@Value1 AND nls.NetworkModeId=@Value2 AND nls.BandId=@Value3 AND nls.CarrierId=@Value4 AND nls.ScopeId=@Value5

			SELECT
			(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN pdef.KeyCode			
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN pdef.KeyCode	
			ELSE def.KeyCode END) 'Event',
			(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventCommand	
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Reselection' and x.IsActive=1) THEN ss.EventCommand				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventCommand
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Reselection' and x.IsActive=1) THEN ss.EventCommand	
			ELSE ss.EventValue END) 'EventValue',
			ss.IsValue ,ss.IsL3Enabled,ss.SequenceId,pdef.DefinationId 'PEventId',pdef.DefinationName 'PEvent',def.DefinationId 'CEventId',def.DefinationName 'CEvent',ss.SrId 'ScriptId',
			(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Reselection' and x.IsActive=1) THEN ss.EventCommand1		
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Reselection' and x.IsActive=1) THEN ss.EventCommand1
			ELSE ss.EventValue END) 'EventValue1'
			FROM AV_SiteScript AS ss
			INNER JOIN AV_NetLayerStatus AS nls ON nls.LayerStatusId=ss.NetLayerId
			INNER JOIN AD_Definations AS def ON def.DefinationId=(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventValue				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventValue
			ELSE ss.EventTypeId END) 
			LEFT JOIN AD_Definations AS pdef ON pdef.DefinationId=(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventTypeId				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventTypeId
			ELSE def.PDefinationId END) 
			WHERE nls.SiteId=@Value1 AND nls.NetworkModeId=@Value2 AND nls.BandId=@Value3 AND nls.CarrierId=@Value4 AND nls.ScopeId=@Value5
			ORDER BY ss.SortOrder
		END
		ELSE IF ISNULL(@Value6,0)>0
		BEGIN		
			--SELECT def.KeyCode 'Event',
			--(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventCommand				
			--WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventCommand
			--ELSE ss.EventValue END) 'EventValue',
			--ss.IsValue ,ss.IsL3Enabled,ss.SequenceId,pdef.DefinationId 'PEventId',pdef.DefinationName 'PEvent',def.DefinationId 'CEventId',def.DefinationName 'CEvent'
			--FROM AV_SiteScript AS ss
			--INNER JOIN AV_NetLayerStatus AS nls ON nls.LayerStatusId=ss.NetLayerId
			--INNER JOIN AD_Definations AS def ON def.DefinationId=ss.EventTypeId
			--LEFT JOIN AD_Definations AS pdef ON pdef.DefinationId=def.PDefinationId
			--INNER JOIN AV_ClusterSchedule csch ON csch.SiteId=nls.SiteId AND ss.SequenceId=csch.SequenceId

			SELECT
			(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN pdef.KeyCode			
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN pdef.KeyCode	
			ELSE def.KeyCode END) 'Event',
			--(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventCommand				
			--WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventCommand
			--ELSE ss.EventValue END) 'EventValue',
			(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventCommand	
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Reselection' and x.IsActive=1) THEN ss.EventCommand				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventCommand
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Reselection' and x.IsActive=1) THEN ss.EventCommand	
			ELSE ss.EventValue END) 'EventValue',
			ss.IsValue ,ss.IsL3Enabled,ss.SequenceId,pdef.DefinationId 'PEventId',pdef.DefinationName 'PEvent',def.DefinationId 'CEventId',def.DefinationName 'CEvent',ss.SrId 'ScriptId',
			(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Reselection' and x.IsActive=1) THEN ss.EventCommand1		
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Reselection' and x.IsActive=1) THEN ss.EventCommand1
			ELSE ss.EventValue END) 'EventValue1'
			FROM AV_SiteScript AS ss
			INNER JOIN AV_NetLayerStatus AS nls ON nls.LayerStatusId=ss.NetLayerId
			INNER JOIN AV_ClusterSchedule csch ON csch.SiteId=nls.SiteId AND ss.SequenceId=csch.SequenceId
			INNER JOIN AD_Definations AS def ON def.DefinationId=(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventValue				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventValue
			ELSE ss.EventTypeId END) 
			LEFT JOIN AD_Definations AS pdef ON pdef.DefinationId=(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventTypeId				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventTypeId
			ELSE def.PDefinationId END) 
			WHERE nls.SiteId=@Value1 AND nls.NetworkModeId=@Value2 AND nls.BandId=@Value3 AND nls.CarrierId=@Value4 AND nls.ScopeId=@Value5
			AND csch.DeviceScheduleId=@Value6
			ORDER BY ss.SortOrder
		END
	END
	
	
	--	[dbo].[AV_GetSiteScript] 'ByNetLayerStatusId',424323
	IF @Filter='ByNetLayerStatusId'
	BEGIN
		SELECT ss.SrId,def.DefinationId 'EventTypeId', def.KeyCode 'Event',ss.EventValue,ss.IsValue ,ss.IsL3Enabled,ss.Color,def.MapColumn,def.DisplayType,def.DefinationName,
		def1.DefinationName 'pDefinationName',def1.DefinationId 'pDefinationId',ss.EventValue1,tsk.ScriptKpiId 'TestKpi'
		FROM AV_SiteScript AS ss
		INNER JOIN AD_Definations AS def ON def.DefinationId=ss.EventTypeId
		LEFT JOIN AD_Definations AS def1 ON def1.DefinationId=def.PDefinationId
		left join AV_ScriptTestKPI tsk on tsk.SiteScriptId=ss.SrId
		WHERE ss.NetLayerId=@Value1
		ORDER BY ss.SortOrder
	END


	IF @Filter='Script_ByNetworkLayer'
	BEGIN
		IF ISNULL(@Value6,0)=0
		BEGIN		
			
			SELECT
			(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN pdef.KeyCode			
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN pdef.KeyCode	
			ELSE def.KeyCode END) 'Event',
			(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventCommand	
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Reselection' and x.IsActive=1) THEN ss.EventCommand				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventCommand
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Reselection' and x.IsActive=1) THEN ss.EventCommand	
			ELSE ss.EventValue END) 'EventValue',
			ss.IsValue ,ss.IsL3Enabled,ss.SequenceId,pdef.DefinationId 'PEventId',pdef.DefinationName 'PEvent',def.DefinationId 'CEventId',def.DefinationName 'CEvent',ss.SrId 'ScriptId',
			(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Reselection' and x.IsActive=1) THEN ss.EventCommand1		
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Reselection' and x.IsActive=1) THEN ss.EventCommand1
			ELSE ss.EventValue END) 'EventValue1'
			FROM AV_SiteScript AS ss
			INNER JOIN AV_NetLayerStatus AS nls ON nls.LayerStatusId=ss.NetLayerId
			INNER JOIN AD_Definations AS def ON def.DefinationId=(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventValue				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventValue
			ELSE ss.EventTypeId END) 
			LEFT JOIN AD_Definations AS pdef ON pdef.DefinationId=(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventTypeId				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventTypeId
			ELSE def.PDefinationId END) 
			WHERE nls.SiteId=@Value1 AND nls.NetworkModeId=@Value2 AND nls.BandId=@Value3 AND nls.CarrierId=@Value4 AND nls.ScopeId=@Value5
			ORDER BY ss.SortOrder

			DECLARE @_tb TABLE(
            Id numeric(18,0)
            );

			insert into @_tb (Id) 
			select ss.SrId
			FROM AV_SiteScript AS ss
			INNER JOIN AV_NetLayerStatus AS nls ON nls.LayerStatusId=ss.NetLayerId
			WHERE nls.SiteId=@Value1 AND nls.NetworkModeId=@Value2 AND nls.BandId=@Value3 AND nls.CarrierId=@Value4 AND nls.ScopeId=@Value5
			ORDER BY ss.SortOrder
			--select * from @_tb
		    DECLARE @listStr NVARCHAR(MAX)=''
            SELECT @listStr = COALESCE(@listStr+',' ,'') + CAST(l.Id AS NVARCHAR) 
            FROM @_tb AS l 
	    
	        SET @listStr = @listStr+','
	    
		    select _sfe.Title,_def.DefinationName 'DataType',_sfe.[Required],_sfe.NodeTypeId,_sfe.FormId,_sfe.ActualValue,_sfe.DefaultValue   from AV_SiteScriptFormEntry _sfe 
			inner join AD_Definations _def on _def.DefinationId = _sfe.DataType
			where  Charindex(cast(_sfe.NodeTypeId as varchar(max))+',', @listStr) > 0
			and _sfe.IsDeleted=0



		END
		ELSE IF ISNULL(@Value6,0)>0
		BEGIN		
			

			SELECT
			(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN pdef.KeyCode			
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN pdef.KeyCode	
			ELSE def.KeyCode END) 'Event',
		    (CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventCommand	
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Reselection' and x.IsActive=1) THEN ss.EventCommand				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventCommand
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Reselection' and x.IsActive=1) THEN ss.EventCommand	
			ELSE ss.EventValue END) 'EventValue',
			ss.IsValue ,ss.IsL3Enabled,ss.SequenceId,pdef.DefinationId 'PEventId',pdef.DefinationName 'PEvent',def.DefinationId 'CEventId',def.DefinationName 'CEvent',ss.SrId 'ScriptId',
			(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Reselection' and x.IsActive=1) THEN ss.EventCommand1		
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Reselection' and x.IsActive=1) THEN ss.EventCommand1
			ELSE ss.EventValue END) 'EventValue1'
			FROM AV_SiteScript AS ss
			INNER JOIN AV_NetLayerStatus AS nls ON nls.LayerStatusId=ss.NetLayerId
			INNER JOIN AV_ClusterSchedule csch ON csch.SiteId=nls.SiteId AND ss.SequenceId=csch.SequenceId
			INNER JOIN AD_Definations AS def ON def.DefinationId=(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventValue				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventValue
			ELSE ss.EventTypeId END) 
			LEFT JOIN AD_Definations AS pdef ON pdef.DefinationId=(CASE WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN ss.EventTypeId				
			WHEN ss.EventTypeId=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN ss.EventTypeId
			ELSE def.PDefinationId END) 
			WHERE nls.SiteId=@Value1 AND nls.NetworkModeId=@Value2 AND nls.BandId=@Value3 AND nls.CarrierId=@Value4 AND nls.ScopeId=@Value5
			AND csch.DeviceScheduleId=@Value6
			ORDER BY ss.SortOrder


			DECLARE @_tbl TABLE(
            Id numeric(18,0)
            );

			insert into @_tbl (Id) 
			select ss.SrId
			FROM AV_SiteScript AS ss
			INNER JOIN AV_NetLayerStatus AS nls ON nls.LayerStatusId=ss.NetLayerId
			WHERE nls.SiteId=@Value1 AND nls.NetworkModeId=@Value2 AND nls.BandId=@Value3 AND nls.CarrierId=@Value4 AND nls.ScopeId=@Value5
			ORDER BY ss.SortOrder
			--select * from @_tbl
			   DECLARE @_listStr NVARCHAR(MAX)=''
               SELECT @_listStr = COALESCE(@_listStr+',' ,'') + CAST(l.Id AS NVARCHAR) 
               FROM @_tbl AS l 
	    
	           SET @_listStr = @_listStr+','
	    
		    select _sfe.Title,_def.DefinationName 'DataType',_sfe.[Required],_sfe.NodeTypeId,_sfe.FormId,_sfe.ActualValue,_sfe.DefaultValue  from AV_SiteScriptFormEntry _sfe
			inner join AD_Definations _def on _def.DefinationId = _sfe.DataType
			where  Charindex(cast(_sfe.NodeTypeId as varchar(max))+',', @_listStr) > 0
			and _sfe.IsDeleted=0

		END
	END
END