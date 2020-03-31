-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_ManageSiteScript]
 @Filter NVARCHAR(50)
,@List List READONLY

AS
BEGIN
	DECLARE @SiteId AS NUMERIC=0
	DECLARE @NetLayerId AS NUMERIC=0
	DECLARE @NetModeId AS NUMERIC=0
	DECLARE @BandId AS NUMERIC=0
	DECLARE @CarrierId AS NUMERIC=0
	
	IF @Filter='Insert'
	BEGIN
	Declare @IDList Table(RowID int not null primary key identity(1,1),ID int);
	DECLARE @RowsToProcess  int
DECLARE @CurrentRow     int
DECLARE @SelectCol1     int
		SELECT TOP 1 @SiteId=Value1, @NetLayerId=Value2 FROM @List
	
		SELECT TOP 1 @NetModeId=anls.NetworkModeId, @BandId=anls.BandId, @CarrierId=anls.CarrierId
		FROM AV_NetLayerStatus AS anls
		WHERE anls.LayerStatusId=@NetLayerId
		
		--SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1

		--SELECT Value5 FROM @List WHERE Value4=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1)
	
		--DELETE FROM AV_SiteScript WHERE NetLayerId=(SELECT TOP 1 l.Value2 FROM @List l)
	--SELECT * FROM @List

		INSERT INTO AV_SiteScript (SiteId,NetLayerId,RevisionId,EventTypeId,EventValue,IsValue,IsL3Enabled,Color,SequenceId,EventCommand,EventValue1,EventCommand1,SortOrder)
		Output INSERTED.SrId Into @IDList(ID)
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,
		(CASE WHEN l.Value4=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Lock' and x.IsActive=1) THEN
			(SELECT TOP 1 adlc.CommandCode FROM AV_DeviceLockCommands AS adlc WHERE adlc.MenuType='Band' AND adlc.BandId=l.Value5)
		WHEN l.Value4=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Reselection' and x.IsActive=1) THEN
			(SELECT TOP 1 adlc.CommandCode FROM AV_DeviceLockCommands AS adlc WHERE adlc.MenuType='Band' AND adlc.BandId=l.Value5)
		WHEN l.Value4=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Reselection' and x.IsActive=1) THEN
			(SELECT TOP 1 adlc.CommandCode FROM AV_DeviceLockCommands AS adlc WHERE adlc.MenuType='Carrier' AND adlc.BandId=(SELECT x.PDefinationId FROM AD_Definations x WHERE x.DefinationId=l.Value5 and x.IsActive=1))
		WHEN l.Value4=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Lock' and x.IsActive=1) THEN
			(SELECT TOP 1 adlc.CommandCode FROM AV_DeviceLockCommands AS adlc WHERE adlc.MenuType='Carrier' AND adlc.BandId=(SELECT x.PDefinationId FROM AD_Definations x WHERE x.DefinationId=l.Value5 and x.IsActive=1))
		ELSE '' END),l.Value10,
		(CASE WHEN l.Value4=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Band Reselection' and x.IsActive=1) THEN
			(SELECT TOP 1 adlc.CommandCode FROM AV_DeviceLockCommands AS adlc WHERE adlc.MenuType='Band' AND adlc.BandId=l.Value10)
		WHEN l.Value4=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName='Carrier Reselection' and x.IsActive=1) THEN
			(SELECT TOP 1 adlc.CommandCode FROM AV_DeviceLockCommands AS adlc WHERE adlc.MenuType='Carrier' AND adlc.BandId=(SELECT x.PDefinationId FROM AD_Definations x WHERE x.DefinationId=l.Value10 and x.IsActive=1))
		ELSE '' END),l.Value12
		FROM @List AS l where l.Value13 = 'undefined' and l.Value14 ='false'


			
		SET @RowsToProcess=@@ROWCOUNT
		--select * from @IDList
		PRINT(@RowsToProcess)

		SET @CurrentRow=0
	--	DELETE FROM AV_ScriptTestKPI WHERE SiteId=(SELECT TOP 1 l.Value1 FROM @List l) and NetLayerId=(SELECT TOP 1 l.Value2 FROM @List l)
WHILE @CurrentRow<@RowsToProcess
BEGIN
    SET @CurrentRow=@CurrentRow+1
    SELECT 
        @SelectCol1=ID
        FROM @IDList

        WHERE RowID=@CurrentRow
		insert into AV_ScriptTestKPI (SiteId,NetLayerId,SiteScriptId,ScriptKpiId,IsEnabled) 
		select (SELECT TOP 1 l.Value1 FROM @List l),(SELECT TOP 1 l.Value2 FROM @List l),@SelectCol1,(SELECT 
        l.Value11
        FROM @List l
        WHERE l.Value12=@CurrentRow),1
    --LOGIC--

END

		update  AV_SiteScript  set EventValue=l.Value5,EventValue1=l.Value10,IsValue=l.Value6,IsL3Enabled=l.Value7,SequenceId=l.Value9,EventTypeId=l.Value4,SortOrder=l.Value12
		FROM @List AS l where l.Value13 <> 'undefined' and AV_SiteScript.SrId = l.Value13 and l.Value14 ='false'

		DECLARE @listStr nVARCHAR(MAX)
        SELECT @listStr = COALESCE(@listStr+',' ,'') + l.Value13
        FROM @List AS l where l.Value13 <> 'undefined' and l.Value14 ='true'
	    
	    SET @listStr = @listStr+','
	    
		delete from AV_SiteScript  where  Charindex(cast(srId as varchar(max))+',', @listStr) > 0

		--UPDATE AV_SiteScript
		--SET EventValue=(SELECT TOP 1 adlc.CommandCode FROM AV_DeviceLockCommands AS adlc WHERE adlc.MenuType='Band' AND adlc.NetworkModeId=@NetModeId AND adlc.BandId=@BandId)
		--WHERE EventValue='Band Lock'
	
		--SELECT 'Band',  adlc.CommandCode FROM AV_DeviceLockCommands AS adlc WHERE adlc.MenuType='Band' AND adlc.NetworkModeId=@NetModeId AND adlc.BandId=@BandId
		--SELECT 'Carrier', adlc.CommandCode FROM AV_DeviceLockCommands AS adlc WHERE adlc.MenuType='Carrier' AND adlc.NetworkModeId=@NetModeId AND adlc.BandId=@BandId
	
		--UPDATE AV_SiteScript
		--SET EventValue=(SELECT TOP 1 adlc.CommandCode FROM AV_DeviceLockCommands AS adlc WHERE adlc.MenuType='Carrier' AND adlc.NetworkModeId=@NetModeId AND adlc.BandId=@BandId)
		--WHERE EventValue='Carrier Lock'		
	END
	ELSE IF @Filter='InsertScripts'
	BEGIN
		SET @SiteId=	(SELECT TOP 1 Value7 FROM @List)
		SET @BandId=	(SELECT TOP 1 Value8 FROM @List)
		SET @CarrierId= (SELECT TOP 1 Value9 FROM @List)
		SET @NetModeId= (SELECT TOP 1 Value10 FROM @List)

		SET @NetLayerId=(SELECT TOP 1 LayerStatusId FROM AV_NetLayerStatus WHERE siteid=@SiteId and  NetworkModeId= @NetModeId and BandId=@BandId and CarrierId=@CarrierId) 

		DELETE FROM AV_SiteScript WHERE NetLayerId=@NetLayerId
		AND SequenceId IN(SELECT DISTINCT l.value6 FROM @List AS l)

		--SELECT @SiteId,@NetLayerId,0,l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,0 FROM @List AS l

		INSERT INTO AV_SiteScript (SiteId,NetLayerId,RevisionId,EventTypeId,EventValue,IsValue,IsL3Enabled,Color,SequenceId,EventCommand)
		SELECT @SiteId,@NetLayerId,0,l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,'' FROM @List AS l			
	END
	ELSE IF @Filter='CLSScripts'
	BEGIN
		DECLARE @SiteIdC AS NUMERIC=0
		DECLARE @NetLayerIdC AS NUMERIC=0
		DECLARE @NetModeIdC AS NUMERIC=0
		DECLARE @BandIdC AS NUMERIC=0
		DECLARE @CarrierIdC AS NUMERIC=0
		DECLARE @SequenceId AS NUMERIC=0
		DECLARE @SiteClusterId AS NUMERIC=0
		DECLARE @ScopeIdC as numeric=0
		DECLARE @Status as numeric=0
		DECLARE @IsMaster as bit=0
		
		SELECT TOP 1 @SiteIdC=Value1, @NetLayerIdC=Value2,@SequenceId=Value3,@SiteClusterId=Value4 FROM @List

		SELECT TOP 1 Value1, Value2,Value3,Value4 FROM @List

		 --DECLARE THE CURSOR FOR A QUERY.
		  DECLARE ClusterSchedule CURSOR READ_ONLY
		  FOR
		  SELECT l.Value1,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7 FROM @List AS l 
		  --OPEN CURSOR.
		  OPEN ClusterSchedule 
		  --FETCH THE RECORD INTO THE VARIABLES.
		  FETCH NEXT FROM ClusterSchedule INTO @SiteIdC,@SequenceId,@SiteClusterId,@NetLayerIdC,@ScopeIdC,@IsMaster 
		  --LOOP UNTIL RECORDS ARE AVAILABLE.
		  WHILE @@FETCH_STATUS = 0
		  BEGIN
				IF ISNULL((SELECT TOP 1 x.SiteClusterId FROM AV_ClusterSchedule x WHERE x.SiteId=@SiteIdC and x.SiteClusterId=@SiteClusterId AND x.SequenceId=@SequenceId and x.IsActive=1),0)=0 
				BEGIN
					SELECT @SequenceId
					INSERT INTO AV_ClusterSchedule (SiteId, SequenceId, SiteClusterId,LayerStatusId,ScopeId,Status,IsMaster)	
					SELECT @SiteIdC,@SequenceId,@SiteClusterId,@NetLayerIdC,@ScopeIdC,90,@IsMaster
				END 
				 --FETCH THE NEXT RECORD INTO THE VARIABLES.
				 FETCH NEXT FROM ClusterSchedule INTO @SiteIdC,@SequenceId,@SiteClusterId,@NetLayerIdC,@ScopeIdC,@IsMaster
		  END
 
		  --CLOSE THE CURSOR.
		  CLOSE ClusterSchedule
		  DEALLOCATE ClusterSchedule


		
		--ELSE	
		--BEGIN
		--	--delete from AV_ClusterSchedule where SiteId=@SiteIdC and LayerStatusId=@NetLayerIdC

		--	INSERT INTO AV_ClusterSchedule (SiteId, SequenceId, SiteClusterId,LayerStatusId,ScopeId,Status,IsMaster)
		--	SELECT l.Value1,l.Value3,l.Value4,l.Value5,l.Value6,90,l.Value7 FROM @List AS l
		--END	
	END
	
END