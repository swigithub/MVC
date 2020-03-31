CREATE procedure AV_ProcessNemoLogs
	@FileID int
as 
begin
	DECLARE @TestType AS NVARCHAR(25)
	DECLARE @SiteId AS NUMERIC(18,0)
	DECLARE @SectorId AS NUMERIC(18,0)
	DECLARE @NetworkModeId AS NUMERIC(18,0)
	DECLARE @BandId AS NUMERIC(18,0)
	DECLARE @CarrierId AS NUMERIC(18,0)
	DECLARE @ScopeId AS NUMERIC(18,0)
	
	DECLARE @Iterations AS INT=0
	DECLARE @MinResult AS FLOAT=0
	DECLARE @MaxResult AS FLOAT=0
	DECLARE @AvgResult AS FLOAT=0
	DECLARE @KPI AS FLOAT=0
	
	DECLARE @prvPCI AS INT=0
	DECLARE @nxtPCI AS INT=0
	DECLARE @Mob_InterHO AS FLOAT=0
	
	SELECT @TestType=fi.fileType, @SiteId=fi.siteID, @SectorId=fi.sectorID, @NetworkModeId=fi.networkModeID, @BandId=fi.bandID, @CarrierId=fi.carrierID
	FROM AV_LogsInfo AS fi WHERE fi.fileID=@FileID
	
	UPDATE AV_NetLayerStatus
	SET [Status] = 450
	WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId
	
	DELETE FROM AV_NemoSiteLogs
	WHERE fileID_Fk IN(SELECT ali.fileID FROM AV_LogsInfo AS ali WHERE ali.SiteId=@SiteId AND ali.NetworkModeId=@NetworkModeId AND ali.BandId=@BandId AND ali.CarrierId=@CarrierId AND ali.sectorID=@SectorId AND ali.fileType=@TestType)
	AND fileID_Fk!=@FileID
		
	DELETE FROM AV_SectorTestLog
	WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND SectorId=@SectorId AND TestType=@TestType
		
	DELETE FROM AV_LogsInfo
	WHERE fileID IN(SELECT ali.fileID FROM AV_LogsInfo AS ali WHERE ali.SiteId=@SiteId AND ali.NetworkModeId=@NetworkModeId AND ali.BandId=@BandId AND ali.CarrierId=@CarrierId AND ali.sectorID=@SectorId AND ali.fileType=@TestType)
	AND fileID!=@FileID
	
	INSERT INTO AV_SectorTestLog(SiteId,NetworkModeId,BandId,CarrierId,ScopeId,SectorId,TestType,TestLatitude,TestLongitude,SignalStrength,SignalPower,SignalQuality,SignalNoise)
		SELECT y.siteID,y.networkModeID, y.bandID, y.carrierID, y.scopeID,y.sectorID,@TestType,
		(SELECT TOP 1 nmo.Latitude FROM AV_NemoSiteLogs nmo WHERE nmo.fileID_Fk=@FileID AND nmo.Latitude IS NOT NULL) 'TestLatitude',
		(SELECT TOP 1 nmo.Longitude FROM AV_NemoSiteLogs nmo WHERE nmo.fileID_Fk=@FileID AND nmo.Latitude IS NOT NULL) 'TestLongitude',
		(SELECT AVG(CAST(nmo.RSRP_RSCP_dBm AS FLOAT)) FROM AV_NemoSiteLogs nmo WHERE nmo.fileID_Fk=@FileID AND nmo.RSRP_RSCP_dBm IS NOT NULL) 'SignalStrength',0,
		(SELECT AVG(CAST(nmo.RSRQ_Ec_I0_db AS FLOAT)) FROM AV_NemoSiteLogs nmo WHERE nmo.fileID_Fk=@FileID AND nmo.RSRQ_Ec_I0_db IS NOT NULL) 'SignalQuality',
		(SELECT AVG(CAST(nmo.SNR AS FLOAT)) FROM AV_NemoSiteLogs nmo WHERE nmo.fileID_Fk=@FileID AND nmo.SNR IS NOT NULL AND nmo.SNR<>0) 'SignalNoise'
		FROM AV_LogsInfo y
		WHERE y.fileID=@FileID
	
	IF @TestType='PING'
	BEGIN
		SELECT @Iterations=COUNT(tfi.Id), @MinResult=MIN(CAST(tfi.RTT_ms AS FLOAT)),@MaxResult=MAX(CAST(tfi.RTT_ms AS FLOAT)),@AvgResult=AVG(CAST(tfi.RTT_ms AS FLOAT))
		FROM AV_NemoSiteLogs AS tfi
		WHERE tfi.fileID_Fk=@FileID
		AND tfi.RTT_ms IS NOT NULL AND tfi.RTT_ms!=''
		
		SELECT @KPI=asc1.KpiValue
		FROM AV_SiteConfigurations AS asc1
		WHERE asc1.SiteId=@SiteId AND asc1.NetworkModeId=@NetworkModeId AND asc1.BandId=@BandId
		AND asc1.RevisionId=(SELECT MAX(x.RevisionId) FROM AV_SiteConfigurations AS x WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId)
		AND asc1.KpiId=51
		
		UPDATE AV_SiteTestSummary
		SET PingIterations = @Iterations,
		PingMinResult = @MinResult,
		PingMaxResult = @MaxResult,
		PingAverageResult = @AvgResult,
		PingStatus = (CASE WHEN @AvgResult>@KPI THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END)
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND SectorId=@SectorId
	END
	ELSE IF @TestType='MO'
	BEGIN		
		
		UPDATE AV_SiteTestSummary
		SET MoStatus = (CASE WHEN (SELECT COUNT(*) FROM AV_NemoSiteLogs AS ansl WHERE ansl.[Event] LIKE '%Success%')>0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND SectorId=@SectorId
	END
	ELSE IF @TestType='MT'
	BEGIN		
		
		UPDATE AV_SiteTestSummary
		SET MtStatus = (CASE WHEN (SELECT COUNT(*) FROM AV_NemoSiteLogs AS ansl WHERE ansl.[Event] LIKE '%Success%')>0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND SectorId=@SectorId
	END
	ELSE IF @TestType='UL'
	BEGIN	
		SELECT @MinResult=MIN(CAST(tfi.PUSCH_UL_Throughput_bits AS FLOAT))/1000000.00,@MaxResult=MAX(CAST(tfi.PUSCH_UL_Throughput_bits AS FLOAT))/1000000.00,@AvgResult=AVG(CAST(tfi.PUSCH_UL_Throughput_bits AS FLOAT))/1000000.00
		FROM AV_NemoSiteLogs AS tfi
		WHERE tfi.fileID_Fk=@FileID
		AND tfi.PUSCH_UL_Throughput_bits IS NOT NULL AND tfi.PUSCH_UL_Throughput_bits != ''
		
		SELECT @KPI=asc1.KpiValue
		FROM AV_SiteConfigurations AS asc1
		WHERE asc1.SiteId=@SiteId AND asc1.NetworkModeId=@NetworkModeId AND asc1.BandId=@BandId
		AND asc1.RevisionId=(SELECT MAX(x.RevisionId) FROM AV_SiteConfigurations AS x WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId)
		AND asc1.KpiId=63
		
		UPDATE AV_SiteTestSummary
		SET UplinkMinResult = @MinResult,
		UplinkMaxResult = @MaxResult,
		UplinkAvgResult = @AvgResult,
		UplinkStatus = (CASE WHEN @MaxResult>=@KPI THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND SectorId=@SectorId
	END
	ELSE IF @TestType='DL'
	BEGIN	
		SELECT @MinResult=MIN(CAST(tfi.PDSCH_DL_Throughput_bits AS FLOAT))/1000000.00,@MaxResult=MAX(CAST(tfi.PDSCH_DL_Throughput_bits AS FLOAT))/1000000.00,@AvgResult=AVG(CAST(tfi.PDSCH_DL_Throughput_bits AS FLOAT))/1000000.00
		FROM AV_NemoSiteLogs AS tfi
		WHERE tfi.fileID_Fk=@FileID
		AND tfi.PDSCH_DL_Throughput_bits IS NOT NULL AND tfi.PDSCH_DL_Throughput_bits!=''
		
		SELECT @KPI=asc1.KpiValue
		FROM AV_SiteConfigurations AS asc1
		WHERE asc1.SiteId=@SiteId AND asc1.NetworkModeId=@NetworkModeId AND asc1.BandId=@BandId
		AND asc1.RevisionId=(SELECT MAX(x.RevisionId) FROM AV_SiteConfigurations AS x WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId)
		AND asc1.KpiId=63
		
		SELECT @KPI,@MinResult,@MaxResult,@AvgResult,@FileID
		
		UPDATE AV_SiteTestSummary
		SET DownlinkMinResult = @MinResult,
		DownlinkMaxResult = @MaxResult,
		DownlinkAvgResult = @AvgResult,
		DownlinkStatus = (CASE WHEN @MaxResult>=@KPI THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND SectorId=@SectorId
	END
	ELSE IF @TestType='http_Ssd'
	BEGIN			
		SELECT @MinResult=MIN(CAST(tfi.PDSCH_DL_Throughput_bits AS FLOAT)),@MaxResult=MAX(CAST(tfi.PDSCH_DL_Throughput_bits AS FLOAT)),@AvgResult=AVG(CAST(tfi.PDSCH_DL_Throughput_bits AS FLOAT))
		FROM AV_NemoSiteLogs AS tfi
		WHERE tfi.fileID_Fk=@FileID
		AND tfi.PDSCH_DL_Throughput_bits IS NOT NULL
		
		SELECT @KPI=asc1.KpiValue
		FROM AV_SiteConfigurations AS asc1
		WHERE asc1.SiteId=@SiteId AND asc1.NetworkModeId=@NetworkModeId AND asc1.BandId=@BandId
		AND asc1.RevisionId=(SELECT MAX(x.RevisionId) FROM AV_SiteConfigurations AS x WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId)
		AND asc1.KpiId=63
		
		UPDATE AV_SiteTestSummary
		SET DownlinkMinResult = @MinResult,
		DownlinkMaxResult = @MaxResult,
		DownlinkAvgResult = @AvgResult,
		DownlinkStatus = (CASE WHEN @MaxResult>=@KPI THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND SectorId=@SectorId
	END
	ELSE IF @TestType='CONNECTION_SETUP'
	BEGIN		
		SELECT @MinResult=MIN(CAST(tfi.Connection_Setup_Time_ms AS FLOAT)),@MaxResult=MAX(CAST(tfi.Connection_Setup_Time_ms AS FLOAT)),@AvgResult=AVG(CAST(tfi.Connection_Setup_Time_ms AS FLOAT))
		FROM AV_NemoSiteLogs AS tfi
		WHERE tfi.fileID_Fk=@FileID
		AND tfi.Connection_Setup_Time_ms IS NOT NULL AND tfi.Connection_Setup_Time_ms!=''
		
		SELECT @KPI=asc1.ConnectionSetupTime
		FROM AV_SiteTestSummary AS asc1
		WHERE asc1.SiteId=@SiteId AND asc1.NetworkModeId=@NetworkModeId AND asc1.BandId=@BandId AND asc1.CarrierId=@CarrierId AND asc1.SectorId=@SectorId
		--AND asc1.RevisionId=(SELECT MAX(x.RevisionId) FROM AV_SiteConfigurations AS x WHERE x.SiteId=@SiteId AND x.NetworkModeId=@NetworkModeId AND x.BandId=@BandId)
		--AND asc1.KpiId=53
		
		PRINT @KPI*1000
		PRINT @MaxResult
		
		UPDATE AV_SiteTestSummary
		SET MinConSetupTime = @MinResult/1000000.00,
		MaxConSetupTime = @MaxResult/1000000.00,
		AvgConSetupTime = @AvgResult/1000000.00,
		ConnectionSetupStatus = (CASE WHEN (@MaxResult/1000000.00)<=(@KPI*1000) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
		WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND SectorId=@SectorId
	END	
	ELSE IF @TestType='CW'
	BEGIN
		IF (SELECT COUNT(*) FROM AV_NemoSiteLogs AS ansl WHERE ansl.fileID_Fk=@FileID)>0
		BEGIN
			SELECT ROW_NUMBER() OVER (ORDER BY azimuth ASC) 'id', sector,PciId
			INTO #tempx
			FROM AV_SiteTestSummary 
			WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId --AND SectorId=@SectorId
			ORDER BY Azimuth ASC
			
			SELECT * FROM #tempx

			SELECT x.id,x.sector,x.pciid 'FromPCI',(SELECT y.pciid FROM #tempx y WHERE y.id=(CASE WHEN x.id+1>(SELECT COUNT(z.id) FROM #tempx z) THEN 1 ELSE x.id+1 ENd)) 'ToPCI'
			INTO #HOx
			FROM #tempx x

			--SELECT  x.*,(SELECT TOP 1 ansl.HO_Uplane_interruption_ms FROM AV_NemoSiteLogs AS ansl WHERE ansl.Id=(SELECT TOP 1 ansl.Id  FROM AV_NemoSiteLogs AS ansl WHERE  (ansl.Current_PCI IS NOT NULL AND ansl.Current_PCI!='') AND ansl.Current_PCI=x.FromPCI AND ansl.Attempted_Channel=x.ToPCI)+1) 'CWHOIntr'
			--FROM #HOx x
			
			SELECT  x.id,x.FromPCI 'prvPCI',x.ToPCI 'nxtPCI',
			(
				SELECT TOP 1 ansl.HO_Uplane_interruption_ms
				FROM AV_NemoSiteLogs AS ansl
				WHERE ansl.Id=(SELECT TOP 1 ansl.Id  FROM AV_NemoSiteLogs AS ansl 
								WHERE  (ansl.Current_PCI IS NOT NULL AND ansl.Current_PCI!='')
								AND ansl.Current_PCI=x.FromPCI AND ansl.Attempted_Channel=x.ToPCI
							  )+1
			) 'MobHOIntr'
			INTO #tmpHOx
			FROM #HOx x
			
			----------------
			
			
	
			DECLARE db_cluster2 CURSOR FOR  
			SELECT x.prvPCI,x.nxtPCI,x.MobHOIntr
			FROM #tmpHOx x
			WHERE x.prvPCI!=x.nxtPCI
			OPEN db_cluster2  
			FETCH NEXT FROM db_cluster2 INTO @prvPCI, @nxtPCI,@Mob_InterHO

			WHILE @@FETCH_STATUS = 0   
			BEGIN 
				UPDATE AV_SiteTestSummary
				SET CwHandoverStatus = CAST(1 AS BIT), HoInterruptionTime = @Mob_InterHO
				WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@prvPCI
				
				
			FETCH NEXT FROM db_cluster2 INTO @prvPCI, @nxtPCI,@Mob_InterHO
			END   

			CLOSE db_cluster2
			DEALLOCATE db_cluster2
			----------------

			DROP TABLE #tmpHOx
			DROP TABLE #HOx
			DROP TABLE #tempx
		END
	END
	ELSE IF @TestType='CCW'
	BEGIN
		IF (SELECT COUNT(*) FROM AV_NemoSiteLogs AS ansl WHERE ansl.fileID_Fk=@FileID)>0
		BEGIN
			SELECT ROW_NUMBER() OVER (ORDER BY azimuth DESC) 'id', sector,PciId
			INTO #temp
			FROM AV_SiteTestSummary 
			WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId --AND SectorId=@SectorId
			ORDER BY Azimuth DESC
			
			

			SELECT x.id,x.sector,x.pciid 'FromPCI',(SELECT y.pciid FROM #temp y WHERE y.id=(CASE WHEN x.id+1>(SELECT COUNT(z.id) FROM #temp z) THEN 1 ELSE x.id+1 ENd)) 'ToPCI'
			INTO #HO
			FROM #temp x

			--SELECT  x.*,(SELECT TOP 1 ansl.HO_Uplane_interruption_ms FROM AV_NemoSiteLogs AS ansl WHERE ansl.Id=(SELECT TOP 1 ansl.Id  FROM AV_NemoSiteLogs AS ansl WHERE  (ansl.Current_PCI IS NOT NULL AND ansl.Current_PCI!='') AND ansl.Current_PCI=x.FromPCI AND ansl.Attempted_Channel=x.ToPCI)+1) 'CWHOIntr'
			--FROM #HOx x
			
			SELECT  x.id,x.FromPCI 'prvPCI',x.ToPCI 'nxtPCI',
			(
				SELECT TOP 1 ansl.HO_Uplane_interruption_ms
				FROM AV_NemoSiteLogs AS ansl
				WHERE ansl.Id=(SELECT TOP 1 ansl.Id  FROM AV_NemoSiteLogs AS ansl 
								WHERE  (ansl.Current_PCI IS NOT NULL AND ansl.Current_PCI!='')
								AND ansl.Current_PCI=x.FromPCI AND ansl.Attempted_Channel=x.ToPCI
							  )+1
			) 'MobHOIntr'
			INTO #tmpHO
			FROM #HO x
			
			----------------
			
			SELECT * FROM #HO
	
			DECLARE db_cluster3 CURSOR FOR  
			SELECT x.prvPCI,x.nxtPCI,x.MobHOIntr
			FROM #tmpHO x
			WHERE x.prvPCI!=x.nxtPCI
			OPEN db_cluster3  
			FETCH NEXT FROM db_cluster3 INTO @prvPCI, @nxtPCI,@Mob_InterHO

			WHILE @@FETCH_STATUS = 0   
			BEGIN 
				UPDATE AV_SiteTestSummary
				SET CcwHandoverStatus = CAST(1 AS BIT), ccwHoInterruptionTime = @Mob_InterHO
				WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@prvPCI
				
				
			FETCH NEXT FROM db_cluster3 INTO @prvPCI, @nxtPCI,@Mob_InterHO
			END   

			CLOSE db_cluster3
			DEALLOCATE db_cluster3
			----------------

			DROP TABLE #tmpHO
			DROP TABLE #HO
			DROP TABLE #temp
		END
	END
END