--EXEC AV_ValidateHandover 132818,15,73,76,311,123,'CW'
CREATE PROCEDURE [dbo].[AV_ValidateHandover]
	@SiteId AS INT,
	@NetworkModeId AS INT,
	@BandId AS INT,
	@CarrierId AS INT,
	@DriveType AS NVARCHAR(10)='',
	@avHandovers AV_Handovers READONLY
AS
DECLARE @prvPCI AS INT=0
DECLARE @nxtPCI AS INT=0
	
IF @DriveType='CW'
BEGIN
	SELECT ROW_NUMBER() OVER (ORDER BY asts.summaryid) 'rowId', asts.SiteId, asts.NetworkModeId, asts.BandId, asts.CarrierId, asts.Sector, asts.PciId, asts.Azimuth
	INTO #tmpCW
	FROM AV_SiteTestSummary AS asts
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId
	ORDER BY asts.Azimuth ASC
	
	SELECT x.SiteId, x.NetworkModeId, x.BandId, x.CarrierId, x.Sector, x.PciId 'prvPCI', x.Azimuth,
	ISNULL((SELECT TOP 1 y.PciId FROM #tmpCW y WHERE y.rowId=x.rowId+1),(SELECT TOP 1 y.PciId FROM #tmpCW y WHERE y.rowId=1)) 'nxtPCI'
	INTO #CW
	FROM #tmpCW x	
	
	--SELECT * from #CW

	--SELECT DISTINCT x.prvPCI,x.nxtPCI
	--FROM @avHandovers x
	--WHERE x.prvPCI!=x.nxtPCI

	DROP TABLE #tmpCW
	
	----------------
	
	DECLARE db_cluster1 CURSOR FOR  
	SELECT x.prvPCI,x.nxtPCI
	FROM @avHandovers x
	WHERE x.prvPCI!=x.nxtPCI
	OPEN db_cluster1   
	FETCH NEXT FROM db_cluster1 INTO @prvPCI, @nxtPCI

	WHILE @@FETCH_STATUS = 0   
	BEGIN   
	
		IF (SELECT COUNT(x.siteid) FROM #CW x WHERE x.prvPCI=@prvPCI AND x.nxtPCI=@nxtPCI AND @prvPCI>=0 AND @nxtPCI>=0)=1
		BEGIN
			--SELECT 'INTRA_HANDOVER_SUCCESS'
			
			UPDATE AV_SiteTestSummary
			SET CwHandoverStatus = CAST(1 AS BIT)
			WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@prvPCI
		END
		ELSE IF (SELECT COUNT(x.siteid) FROM #CW x WHERE x.prvPCI=@prvPCI AND x.nxtPCI!=@nxtPCI AND @prvPCI>=0 AND @nxtPCI>=0)=1
		BEGIN
			IF ISNULL((SELECT COUNT(x.siteid) FROM #CW x WHERE x.nxtPCI=@nxtPCI AND @prvPCI>=0 AND @nxtPCI>=0),0)=1
			BEGIN
				--SELECT 'INTRA_HANDOVER_FAILURE'
			
				UPDATE AV_SiteTestSummary
				SET CwHandoverStatus = (CASE WHEN CwHandoverStatus=CAST(1 as bit) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
				WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@prvPCI
			END
			ELSE IF ISNULL((SELECT COUNT(x.siteid) FROM #CW x WHERE x.nxtPCI=@nxtPCI AND @prvPCI>=0 AND @nxtPCI>=0),0)=0
			BEGIN
				--SELECT 'OUTGOING_INTER_HANDOVER'
			
				UPDATE AV_SiteTestSummary
				SET ICwHandoverStatus = CAST(1 AS BIT)
				WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@prvPCI
			END
		END
		ELSE IF (SELECT COUNT(x.siteid) FROM #CW x WHERE x.prvPCI!=@prvPCI AND x.nxtPCI=@nxtPCI AND @prvPCI>=0 AND @nxtPCI>=0)=1
		BEGIN
			--SELECT 'INCOMING_INTER_HANDOVER'
		
			UPDATE AV_SiteTestSummary
			SET ICwHandoverStatus = CAST(1 AS BIT)
			WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@nxtPCI
		END
		--ELSE IF (SELECT COUNT(x.siteid) FROM #CW x WHERE x.prvPCI!=@prvPCI AND x.nxtPCI!=@nxtPCI)=1
		--BEGIN
		--	SELECT 'INVALID_HANDOVER'
		--END
			
	FETCH NEXT FROM db_cluster1 INTO @prvPCI, @nxtPCI
	END   

	CLOSE db_cluster1   
	DEALLOCATE db_cluster1
	----------------
	
	DROP TABLE #CW
END
ELSE IF @DriveType='CCW'
BEGIN
	SELECT ROW_NUMBER() OVER (ORDER BY asts.summaryid DESC) 'rowId',asts.SiteId, asts.NetworkModeId, asts.BandId, asts.CarrierId, asts.Sector, asts.PciId, asts.Azimuth
	INTO #tmpCCW
	FROM AV_SiteTestSummary AS asts
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId
	ORDER BY asts.Azimuth DESC
	
	SELECT x.SiteId, x.NetworkModeId, x.BandId, x.CarrierId, x.Sector, x.PciId 'prvPCI', x.Azimuth,
	ISNULL((SELECT TOP 1 y.PciId FROM #tmpCCW y WHERE y.rowId=x.rowId+1),(SELECT TOP 1 y.PciId FROM #tmpCCW y WHERE y.rowId=1)) 'nxtPCI'
	INTO #CCW
	FROM #tmpCCW x
	
	DROP TABLE #tmpCCW
	
	----------------	
	DECLARE db_cluster2 CURSOR FOR  
	SELECT DISTINCT x.prvPCI,x.nxtPCI
	FROM @avHandovers x		
	OPEN db_cluster2   
	FETCH NEXT FROM db_cluster2 INTO @prvPCI, @nxtPCI

	WHILE @@FETCH_STATUS = 0   
	BEGIN   
	
		IF (SELECT COUNT(x.siteid) FROM #CCW x WHERE x.prvPCI=@prvPCI AND x.nxtPCI=@nxtPCI AND @prvPCI>=0 AND @nxtPCI>=0)=1
		BEGIN
			--SELECT 'INTRA_HANDOVER_SUCCESS'
		
			UPDATE AV_SiteTestSummary
			SET CcwHandoverStatus = CAST(1 AS BIT)
			WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@prvPCI
		END
		ELSE IF (SELECT COUNT(x.siteid) FROM #CCW x WHERE x.prvPCI=@prvPCI AND x.nxtPCI!=@nxtPCI AND @prvPCI>=0 AND @nxtPCI>=0)=1
		BEGIN
			IF ISNULL((SELECT COUNT(x.siteid) FROM #CCW x WHERE x.nxtPCI=@nxtPCI AND @prvPCI>=0 AND @nxtPCI>=0),0)=1
			BEGIN
				--SELECT 'INTRA_HANDOVER_FAILURE'
			
				UPDATE AV_SiteTestSummary
				SET CcwHandoverStatus = (CASE WHEN CcwHandoverStatus=CAST(1 as bit) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
				WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@prvPCI
			END
			ELSE IF ISNULL((SELECT COUNT(x.siteid) FROM #CCW x WHERE x.nxtPCI=@nxtPCI AND @prvPCI>=0 AND @nxtPCI>=0),0)=0
			BEGIN
				--SELECT 'OUTGOING_INTER_HANDOVER'
			
				UPDATE AV_SiteTestSummary
				SET ICcwHandoverStatus = CAST(1 AS BIT)
				WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@prvPCI
			END
		END
		ELSE IF (SELECT COUNT(x.siteid) FROM #CCW x WHERE x.prvPCI!=@prvPCI AND x.nxtPCI=@nxtPCI AND @prvPCI>=0 AND @nxtPCI>=0)=1
		BEGIN
			--SELECT 'INCOMING_INTER_HANDOVER'
		
			UPDATE AV_SiteTestSummary
			SET ICcwHandoverStatus = CAST(1 AS BIT)
			WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId AND PciId=@nxtPCI
		END
		--ELSE IF (SELECT COUNT(x.siteid) FROM #CCW x WHERE x.prvPCI!=@prvPCI AND x.nxtPCI!=@nxtPCI)=1
		--BEGIN
		--	SELECT 'INVALID_HANDOVER'
		--END
			
	FETCH NEXT FROM db_cluster2 INTO @prvPCI, @nxtPCI
	END   

	CLOSE db_cluster2   
	DEALLOCATE db_cluster2
	----------------
	
	DROP TABLE #CCW
END