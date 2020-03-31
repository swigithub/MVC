
CREATE PROCEDURE [dbo].[AV_Insert_SiteTestLog1] 

@List Tb_AV_SiteTestLog READONLY,
@TestStatus bit
AS
BEGIN
	DECLARE @TestType as nvarchar(50)=''
	DECLARE @Sitecode as nvarchar(50)=''
	DECLARE @SiteId as int=0
	DECLARE @SectorCode as nvarchar(50)=''
	DECLARE @NetworkMode as nvarchar(50)=''
	DECLARE @Band as nvarchar(50)=''
	DECLARE @Carrier as nvarchar(50)=''
	
	DECLARE @woID AS INT=0
	DECLARE @lNetModeId AS INT=0
	DECLARE @lBandId AS INT=0
	DECLARE @lCarrierId AS INT=0

	SET @TestType=(SELECT TOP 1 x.TestType FROM @List x)
	SET @Sitecode=(SELECT TOP 1 x.Site FROM @List x)
	SET @SiteId=(SELECT TOP 1 x.SiteId FROM AV_Sites x WHERE x.SiteCode=@Sitecode)
	SET @SectorCode=(SELECT TOP 1 x.Sector FROM @List x)
	SET @NetworkMode=(SELECT TOP 1 x.SubNetworkMode FROM @List x)
	SET @Band=(SELECT TOP 1 x.ActualBand FROM @List x)
	SET @Carrier=(SELECT TOP 1 x.ActualCarrier FROM @List x)
	
	SET @woID=(SELECT TOP 1 x.SiteRefId FROM @List x)
	SET @lNetModeId=(SELECT TOP 1 x.NetworkModeId FROM @List x)
	SET @lBandId=(SELECT TOP 1 x.BandId FROM @List x)
	SET @lCarrierId=(SELECT TOP 1 x.CarrierId FROM @List x)
	
	--SELECT * FROM @List

	--SELECT * INTO actualLogs FROM @List

	--SELECT @TestType 'Test',@Sitecode 'site',@SectorCode 'sector',@NetworkMode 'ntm',@Band 'bnd',@Carrier 'crr'
	--INTO testLog1
	
	DECLARE @acceptLog BIT=CAST(1 AS BIT)
	DECLARE @siteStatusId AS INT=0
	DECLARE @siteStatus NVARCHAR(50)=''
	
	SELECT @siteStatusId=x.[Status] FROM AV_NetLayerStatus x WHERE x.SiteId=@woID AND x.NetworkModeId=@lNetModeId AND x.BandId=@lBandId AND x.CarrierId=@lCarrierId;
	SELECT @siteStatus=ad.KeyCode FROM AD_Definations ad WHERE ad.DefinationId=@siteStatusId;
	
	IF (@siteStatus='SCHEDULED' OR @siteStatus='IN_PROGRESS' OR @siteStatus='PENDING_WITH_ISSUE' OR @siteStatus='DRIVE_COMPLETED')
	BEGIN
		SET @acceptLog=CAST(1 AS BIT)
	END
	ELSE IF (@siteStatus='REPORT_SUBMITTED' OR @siteStatus='COMPLETED')
	BEGIN
		SET @acceptLog=CAST(0 AS BIT)
	END
	--PRINT @acceptLog
	
	IF IsNull(@SiteId,0)<=0
	BEGIN
	
			RAISERROR('Site doesn''t exists',16,1)	--RETURN -1 	 --Site doesn't exists.
	END

	IF @TestType='IRAT'
	BEGIN
		UPDATE AV_SiteTestSummary
		SET IRATHandover = (SELECT TOP 1 CAST(x.TestResult as bit) FROM @List x ORDER by x.TimeStamp DESC)
		WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
	END
	
	IF (@acceptLog=CAST(1 AS BIT))
	BEGIN
		IF @TestType='Ookla'
		BEGIN
			UPDATE AV_SiteTestSummary
			SET OoklaLatitude=(SELECT TOP 1 x.Latitude FROM @List x ORDER BY x.TimeStamp DESC),
			OoklaLongitude=(SELECT TOP 1 x.Longitude FROM @List x ORDER BY x.TimeStamp DESC),
			OoklaRssi=(SELECT TOP 1 CASE WHEN @NetworkMode='LTE' THEN x.LteRsrp WHEN @NetworkMode='WCDMA' THEN x.WcdmaRscp WHEN @NetworkMode='GSM' THEN x.GsmRssi END FROM @List x ORDER BY x.TimeStamp DESC),
			OoklaSinr=(SELECT TOP 1 CASE WHEN @NetworkMode='LTE' THEN x.LteRsnr WHEN @NetworkMode='WCDMA' THEN x.WcdmaEcio WHEN @NetworkMode='GSM' THEN x.GsmRxQual END FROM @List x ORDER BY x.TimeStamp DESC)
			WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
		END
		
		ELSE IF @TestType='Ping'
		BEGIN
			
			SET @TestType='Ping / Latency Test'

			UPDATE AV_SiteTestSummary
			SET PingMinResult=(SELECT MIN(x.TestResult) FROM @List x),
			PingAverageResult=(SELECT AVG(x.TestResult) FROM @List x),
			PingMaxResult=(SELECT MAX(x.TestResult) FROM @List x),
			PingStatus=@TestStatus,
			GsmRssi=ISNULL((SELECT AVG(x.GsmRssi) FROM @List x),0),
			GsmRxQual=ISNULL((SELECT AVG(x.GsmRxQual) FROM @List x),0),
			WcdmaRssi=0,
			WcdmaRscp=ISNULL((SELECT AVG(x.WcdmaRscp) FROM @List x),0),
			WcdmaEcio=ISNULL((SELECT AVG(x.WcdmaEcio) FROM @List x),0),
			LteRssi=0,
			LteRsrp=ISNULL((SELECT AVG(x.LteRsrp) FROM @List x),0),
			LteRsrq=ISNULL((SELECT AVG(x.LteRsrq) FROM @List x),0),
			LteRsnr=ISNULL((SELECT AVG(x.LteRsnr) FROM @List x),0),
			LteCqi=ISNULL((SELECT TOP 1 x.LteCqi FROM @List x),0),
			--NRBand=ISNULL((SELECT TOP 1 x.NRBand FROM @List x),''),
			--	NRCarrier=ISNULL((SELECT TOP 1 x.NRCarrier FROM @List x),''),
			--	NRRsrp=ISNULL((SELECT TOP 1 x.NRRsrp FROM @List x),0),
			--	NRRsrq=ISNULL((SELECT TOP 1 x.NRRsrq FROM @List x),0),
			--	NRRsnr=ISNULL((SELECT TOP 1 x.NRRsnr FROM @List x),0),
			--	NRCqi=ISNULL((SELECT TOP 1 x.NRCqi FROM @List x),0),
			TestLatitude=(SELECT TOP 1 x.Latitude FROM @List x),
			TestLongitude=(SELECT TOP 1 x.Longitude FROM @List x),
			DistanceFromSite=(SELECT TOP 1 x.DistanceFromSite FROM @List x),
			AngleToSite=(SELECT TOP 1 x.AngleToSite FROM @List x)
			WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
			
			--PRINT 'SUCCS'
		END
		ELSE IF (@TestType='DL' OR @TestType='UL')
		BEGIN
			IF @TestType='DL'
			BEGIN
				UPDATE AV_SiteTestSummary
				SET DownlinkMinResult=(SELECT MIN(x.TestResult) FROM @List x),
				DownlinkAvgResult=(SELECT AVG(x.TestResult) FROM @List x),
				DownlinkMaxResult=(SELECT MAX(x.TestResult) FROM @List x),
				DownlinkStatus=@TestStatus,
				GsmRssi=ISNULL((SELECT AVG(x.GsmRssi) FROM @List x),0),
				GsmRxQual=ISNULL((SELECT AVG(x.GsmRxQual) FROM @List x),0),
				WcdmaRssi=0,
				WcdmaRscp=ISNULL((SELECT AVG(x.WcdmaRscp) FROM @List x),0),
				WcdmaEcio=ISNULL((SELECT AVG(x.WcdmaEcio) FROM @List x),0),
				LteRssi=0,
				LteRsrp=ISNULL((SELECT AVG(x.LteRsrp) FROM @List x),0),
				LteRsrq=ISNULL((SELECT AVG(x.LteRsrq) FROM @List x),0),
				LteRsnr=ISNULL((SELECT AVG(x.LteRsnr) FROM @List x),0),
				LteCqi=ISNULL((SELECT TOP 1 x.LteCqi FROM @List x),0),
				--NRBand=ISNULL((SELECT TOP 1 x.NRBand FROM @List x),''),
				--NRCarrier=ISNULL((SELECT TOP 1 x.NRCarrier FROM @List x),''),
				--NRRsrp=ISNULL((SELECT TOP 1 x.NRRsrp FROM @List x),0),
				--NRRsrq=ISNULL((SELECT TOP 1 x.NRRsrq FROM @List x),0),
				--NRRsnr=ISNULL((SELECT TOP 1 x.NRRsnr FROM @List x),0),
				--NRCqi=ISNULL((SELECT TOP 1 x.NRCqi FROM @List x),0),
				TestLatitude=(SELECT TOP 1 x.Latitude FROM @List x),
				TestLongitude=(SELECT TOP 1 x.Longitude FROM @List x),
				DistanceFromSite=(SELECT TOP 1 x.DistanceFromSite FROM @List x),
				AngleToSite=(SELECT TOP 1 x.AngleToSite FROM @List x)
				WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
			END
			ELSE IF @TestType='UL'
			BEGIN
				UPDATE AV_SiteTestSummary
				SET UplinkMinResult=(SELECT MIN(x.TestResult) FROM @List x),
				UplinkAvgResult=(SELECT AVG(x.TestResult) FROM @List x),
				UplinkMaxResult=(SELECT MAX(x.TestResult) FROM @List x),
				UplinkStatus=@TestStatus,
				GsmRssi=ISNULL((SELECT AVG(x.GsmRssi) FROM @List x),0),
				GsmRxQual=ISNULL((SELECT AVG(x.GsmRxQual) FROM @List x),0),
				WcdmaRssi=0,
				WcdmaRscp=ISNULL((SELECT AVG(x.WcdmaRscp) FROM @List x),0),
				WcdmaEcio=ISNULL((SELECT AVG(x.WcdmaEcio) FROM @List x),0),
				LteRssi=0,
				LteRsrp=ISNULL((SELECT AVG(x.LteRsrp) FROM @List x),0),
				LteRsrq=ISNULL((SELECT AVG(x.LteRsrq) FROM @List x),0),
				LteRsnr=ISNULL((SELECT AVG(x.LteRsnr) FROM @List x),0),
				LteCqi=ISNULL((SELECT TOP 1 x.LteCqi FROM @List x),0),
				--NRBand=ISNULL((SELECT TOP 1 x.NRBand FROM @List x),''),
				--NRCarrier=ISNULL((SELECT TOP 1 x.NRCarrier FROM @List x),''),
				--NRRsrp=ISNULL((SELECT TOP 1 x.NRRsrp FROM @List x),0),
				--NRRsrq=ISNULL((SELECT TOP 1 x.NRRsrq FROM @List x),0),
				--NRRsnr=ISNULL((SELECT TOP 1 x.NRRsnr FROM @List x),0),
				--NRCqi=ISNULL((SELECT TOP 1 x.NRCqi FROM @List x),0),
				TestLatitude=(SELECT TOP 1 x.Latitude FROM @List x),
				TestLongitude=(SELECT TOP 1 x.Longitude FROM @List x),
				DistanceFromSite=(SELECT TOP 1 x.DistanceFromSite FROM @List x),
				AngleToSite=(SELECT TOP 1 x.AngleToSite FROM @List x)
				WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
			END
		

			SET @TestType='Throughput'
		END
		ELSE IF (@TestType='MO' OR @TestType='MT')
		BEGIN
		
			DECLARE @moMtStatus as int=0

			SET @moMtStatus=(SELECT TOP 1 TestResult FROM @List x WHERE x.StackTrace LIKE 'After%')

			IF @TestType='MO'
			BEGIN
				UPDATE AV_SiteTestSummary
				SET MoStatus=@TestStatus,--CAST(@moMtStatus as bit)
				GsmRssi=ISNULL((SELECT AVG(x.GsmRssi) FROM @List x),0),
				GsmRxQual=ISNULL((SELECT AVG(x.GsmRxQual) FROM @List x),0),
				WcdmaRssi=0,
				WcdmaRscp=ISNULL((SELECT AVG(x.WcdmaRscp) FROM @List x),0),
				WcdmaEcio=ISNULL((SELECT AVG(x.WcdmaEcio) FROM @List x),0),
				LteRssi=0,
				LteRsrp=ISNULL((SELECT AVG(x.LteRsrp) FROM @List x),0),
				LteRsrq=ISNULL((SELECT AVG(x.LteRsrq) FROM @List x),0),
				LteRsnr=ISNULL((SELECT AVG(x.LteRsnr) FROM @List x),0),
				LteCqi=ISNULL((SELECT TOP 1 x.LteCqi FROM @List x),0),
				--NRBand=ISNULL((SELECT TOP 1 x.NRBand FROM @List x),''),
				--NRCarrier=ISNULL((SELECT TOP 1 x.NRCarrier FROM @List x),''),
				--NRRsrp=ISNULL((SELECT TOP 1 x.NRRsrp FROM @List x),0),
				--NRRsrq=ISNULL((SELECT TOP 1 x.NRRsrq FROM @List x),0),
				--NRRsnr=ISNULL((SELECT TOP 1 x.NRRsnr FROM @List x),0),
				--NRCqi=ISNULL((SELECT TOP 1 x.NRCqi FROM @List x),0),
				TestLatitude=(SELECT TOP 1 x.Latitude FROM @List x),
				TestLongitude=(SELECT TOP 1 x.Longitude FROM @List x)
				WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
			END
			ELSE IF @TestType='MT'
			BEGIN
				UPDATE AV_SiteTestSummary
				SET MtStatus=@TestStatus,
				GsmRssi=ISNULL((SELECT AVG(x.GsmRssi) FROM @List x),0),
				GsmRxQual=ISNULL((SELECT AVG(x.GsmRxQual) FROM @List x),0),
				WcdmaRssi=0,
				WcdmaRscp=ISNULL((SELECT AVG(x.WcdmaRscp) FROM @List x),0),
				WcdmaEcio=ISNULL((SELECT AVG(x.WcdmaEcio) FROM @List x),0),
				LteRssi=0,
				LteRsrp=ISNULL((SELECT AVG(x.LteRsrp) FROM @List x),0),
				LteRsrq=ISNULL((SELECT AVG(x.LteRsrq) FROM @List x),0),
				LteRsnr=ISNULL((SELECT AVG(x.LteRsnr) FROM @List x),0),
				LteCqi=ISNULL((SELECT TOP 1 x.LteCqi FROM @List x),0),
				--NRBand=ISNULL((SELECT TOP 1 x.NRBand FROM @List x),''),
				--NRCarrier=ISNULL((SELECT TOP 1 x.NRCarrier FROM @List x),''),
				--NRRsrp=ISNULL((SELECT TOP 1 x.NRRsrp FROM @List x),0),
				--NRRsrq=ISNULL((SELECT TOP 1 x.NRRsrq FROM @List x),0),
				--NRRsnr=ISNULL((SELECT TOP 1 x.NRRsnr FROM @List x),0),
				--NRCqi=ISNULL((SELECT TOP 1 x.NRCqi FROM @List x),0),
				TestLatitude=(SELECT TOP 1 x.Latitude FROM @List x),
				TestLongitude=(SELECT TOP 1 x.Longitude FROM @List x)
				WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
			END

			SET @TestType='MO/MT'
		END	
		ELSE IF (@TestType='VMO' OR @TestType='VMT')
		BEGIN
		
			DECLARE @vmoMtStatus as int=0

			SET @vmoMtStatus=(SELECT TOP 1 TestResult FROM @List x WHERE x.StackTrace LIKE 'After%')

			IF @TestType='VMO'
			BEGIN
				UPDATE AV_SiteTestSummary
				SET VMoStatus=@TestStatus--CAST(@moMtStatus as bit)
				WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
			END
			ELSE IF @TestType='VMT'
			BEGIN
				UPDATE AV_SiteTestSummary
				SET VMtStatus=@TestStatus--CAST(@moMtStatus as bit)
				WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
			END

			SET @TestType='VMO/MT'
		END
		ELSE IF (@TestType='E911')
		BEGIN
			UPDATE AV_SiteTestSummary
			SET E911Status=@TestStatus--CAST(@moMtStatus as bit)
			WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
		END
		ELSE IF (@TestType='CA')
		BEGIN
			UPDATE AV_SiteTestSummary
			SET CarrierAggregationStatus=@TestStatus--CAST(@moMtStatus as bit)
			WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
		END
		ELSE IF (@TestType='SMO')
		BEGIN
			UPDATE AV_SiteTestSummary
			SET SMoStatus=@TestStatus--CAST(@moMtStatus as bit)
			WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
		END
		ELSE IF (@TestType='SMT')
		BEGIN
			UPDATE AV_SiteTestSummary
			SET SMtStatus=@TestStatus--CAST(@moMtStatus as bit)
			WHERE Site=@Sitecode AND Sector=@SectorCode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier;
		END
		ELSE IF (@TestType='CW')
		BEGIN
			SET @TestType='CW'
		
			--IF @NetworkMode='LTE'
			--BEGIN
			--	--UPDATE AV_SiteTestSummary
			--	--SET MoStatus = 1, MtStatus = 1, VMoStatus = 1, VMtStatus = 1
			--	--WHERE Site=@Sitecode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier; 
			--END
			
			
			--UPDATE AV_SiteTestSummary
			--SET CwHandoverStatus=(
			--						SELECT CAST(x.IsHandover as bit) FROM @List x
			--						WHERE x.Site=AV_SiteTestSummary.Site AND x.Sector=AV_SiteTestSummary.Sector
			--						AND x.SubNetworkMode=AV_SiteTestSummary.NetworkMode AND x.ActualBand=AV_SiteTestSummary.Band AND x.ActualCarrier=AV_SiteTestSummary.Carrier
			--						AND x.IsHandover IN(0,1)
			--					)
			--WHERE Site=@Sitecode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier; 
		END
		ELSE IF (@TestType='CCW')
		BEGIN
			SET @TestType='CCW'

			--UPDATE AV_SiteTestSummary
			--SET CcwHandoverStatus=(
			--						SELECT CAST(x.IsHandover as bit) FROM @List x
			--						WHERE x.Site=AV_SiteTestSummary.Site AND x.Sector=AV_SiteTestSummary.Sector
			--						AND x.SubNetworkMode=AV_SiteTestSummary.NetworkMode AND x.ActualBand=AV_SiteTestSummary.Band AND x.ActualCarrier=AV_SiteTestSummary.Carrier
			--						AND x.IsHandover IN(0,1)
			--					)
			--WHERE Site=@Sitecode AND NetworkMode=@NetworkMode AND Band=@Band AND Carrier=@Carrier; 
		END
	END
	
		DECLARE @cwNetModeID int=0
		DECLARE @cwNetMode nvarchar(50)=''
		DECLARE @cwBandID int=0
		DECLARE @cwBand nvarchar(50)=''
		DECLARE @cwCarrierID int=0
		DECLARE @cwCarrier nvarchar(50)=''
		DECLARE @cwScopeID int=0
		DECLARE @cwScope nvarchar(50)=''

		SELECT DISTINCT @cwNetModeID=x.NetworkModeId, @cwNetMode=x.NetworkMode, @cwBandID=x.BandId, @cwBand=x.Band, @cwCarrierID=x.CarrierId, @cwCarrier=x.Carrier,@cwScopeID=x.ScopeId, @cwScope=x.Scope
		FROM AV_SiteTestSummary x
		WHERE x.Site=@Sitecode AND x.NetworkMode=@NetworkMode AND x.Band=@Band AND x.Carrier=@Carrier
		
		
	IF (@TestType='CW' OR @TestType='CCW')
		BEGIN
	
		--SELECT  @cwNetModeID, @cwNetMode, @cwBandID, @cwBand, @cwCarrierID, @cwCarrier,@cwScopeID, @cwScope
		
		DECLARE @ColorDefTypeId as int = (SELECT y.DefinationTypeId FROM AD_DefinationTypes y WHERE y.DefinationType='Color')
		
		DECLARE @CarrierList TABLE(Carrier NVARCHAR(15),CarrierColor NVARCHAR(50))
		
		IF EXISTS(SELECT TOP 1 y.SiteId FROM AV_SiteTestLog AS y WHERE y.SiteId=@SiteId AND y.NetworkModeId=@cwNetModeID AND y.BandId=@cwBandID AND y.CarrierId=@cwCarrierID AND y.TestType IN('CW','CCW'))
		BEGIN
			
			INSERT INTO @CarrierList(Carrier,CarrierColor)
			SELECT *
			FROM
			(
				SELECT DISTINCT (CASE WHEN x.NetworkMode='NR' then x.NRCarrier else x.Carrier END) 'Carrier',y.ChColor 'CarrierColor'
				--(SELECT TOP 1 y.ChColor
				--   FROM AV_SiteTestLog AS y WHERE y.SiteId=@SiteId --AND y.NetworkModeId=@cwNetModeID AND y.BandId=@cwBandID AND y.CarrierId=@cwCarrierID
				--AND (CASE WHEN y.NetworkMode='NR' then y.NRCarrier else y.Carrier END)=(CASE WHEN x.NetworkMode='NR' then x.NRCarrier else x.Carrier END)) 'CarrierColor'
				FROM @List x
				LEFT OUTER JOIN AV_SiteTestLog y ON y.SiteId=x.SiteRefId
				AND (CASE WHEN x.NetworkMode='NR' then x.NRCarrier else x.Carrier END)=(CASE WHEN y.NetworkMode='NR' then y.NRCarrier else y.Carrier END)
				WHERE y.SiteId=@SiteId AND y.NetworkModeId=@cwNetModeID AND y.BandId=@cwBandID AND y.CarrierId=@cwCarrierID
				AND y.TestType IN('CW','CCW')
			) x
		END
		ELSE
		BEGIN
			
			INSERT INTO @CarrierList(Carrier,CarrierColor)
			SELECT *
			FROM
			(
				SELECT DISTINCT (CASE WHEN x.NetworkMode='NR' then x.NRCarrier else x.Carrier END) 'Carrier',y.ChColor 'CarrierColor'
				--(SELECT TOP 1 y.ChColor
				--   FROM AV_SiteTestLog AS y WHERE y.SiteId=@SiteId --AND y.NetworkModeId=@cwNetModeID AND y.BandId=@cwBandID AND y.CarrierId=@cwCarrierID
				--AND (CASE WHEN y.NetworkMode='NR' then y.NRCarrier else y.Carrier END)=(CASE WHEN x.NetworkMode='NR' then x.NRCarrier else x.Carrier END)) 'CarrierColor'
				FROM @List x
				LEFT OUTER JOIN AV_SiteTestLog y ON y.SiteId=x.SiteRefId
				AND (CASE WHEN x.NetworkMode='NR' then x.NRCarrier else x.Carrier END)=(CASE WHEN y.NetworkMode='NR' then y.NRCarrier else y.Carrier END)
				WHERE y.SiteId=@SiteId AND y.NetworkModeId=@cwNetModeID AND y.BandId=@cwBandID AND y.CarrierId=@cwCarrierID
				--AND y.TestType IN('CW','CCW')
			) x
		END
		
		
		---------------------Carrier Plot Start------------------------
		DECLARE @tCarrierID AS NVARCHAR(15)=''
		DECLARE @tCarrierColor AS NVARCHAR(50)=''	
			
		DECLARE db_cluster2 CURSOR FOR  
		SELECT Carrier,CarrierColor FROM @CarrierList AS cl WHERE cl.CarrierColor IS NULL
		OPEN db_cluster2 
		FETCH NEXT FROM db_cluster2 INTO @tCarrierID,@tCarrierColor
		WHILE @@FETCH_STATUS = 0   
		BEGIN   
			DECLARE @ColorCode as nvarchar(50)=''
			DECLARE @randColorCode AS NVARCHAR(50)=''			
			
			IF (@tCarrierColor='' OR @tCarrierColor IS NULL)
			BEGIN
				SELECT TOP 1 @randColorCode=x.ColorCode
				FROM AD_Definations x
				WHERE x.DefinationTypeId=@ColorDefTypeId				
				AND x.ColorCode NOT IN(SELECT DISTINCT x.CarrierColor FROM @CarrierList x WHERE x.CarrierColor IS NOT NULL)
				AND x.IsActive=1			
				ORDER BY NEWID()
				
				UPDATE @CarrierList
				SET CarrierColor= @randColorCode
				WHERE Carrier=@tCarrierID
			END	
		FETCH NEXT FROM db_cluster2 INTO @tCarrierID,@tCarrierColor
		END   
		CLOSE db_cluster2   
		DEALLOCATE db_cluster2		
		----------------------Carrier Plot End
		
		--SELECT * FROM @CarrierList
		
		----SELECT * FROM @List 	
				
		SELECT ROW_NUMBER() OVER(ORDER BY x.Site) 'rowID', x.Site,x.Sector,x.NetworkMode,
		CASE WHEN x.Band=-1 AND x.ActualBand='GSM 1900' THEN 1900 ELSE x.Band END 'Band',
		x.Carrier,x.CellId,x.LacId,
		CASE WHEN x.PciId=-1 AND x.ActualBand='GSM 1900' THEN x.Carrier ELSE x.PciId END 'PciId',
		x.MccId,x.MncId,
		x.Latitude,x.Longitude,x.TimeStamp,
		x.GsmRssi,x.GsmRxQual,x.WcdmaRscp,x.WcdmaEcio,x.LteRsrp,x.LteRsrq,x.LteRsnr,x.LteCqi,x.DistanceFromSite,x.AngleToSite,x.FtpStatus,
		x.StackTrace,x.TestResult,x.SubNetworkMode,x.ActualBand,x.ActualCarrier, x.IsHandover,x.FloorId,x.RRCState,x.NRBand,x.NRCarrier,x.NRRsrp,x.NRRsrq,x.NRRsnr,x.NRCqi,x.NRPci,x.FromPCI,x.ToPCI
		INTO #tmpMobility
		FROM @List x
		ORDER BY x.TimeStamp
		
		--SELECT * FROM #tmpMobility
		
		--SELECT * INTO tmpMobility FROM  #tmpMobility

		SELECT x.RegionId,x.Region,x.CityId,x.City,x.TestCategoryId,x.TestCategory,x.TestTypeId,x.[TestType],y.[TimeStamp],x.ClusterId,x.Cluster,x.SiteId,x.Site,x.SectorId,x.Sector,
		y.CellId,y.LacId,y.PciId,y.MccId,y.MncId,
		y.Latitude,y.Longitude,x.ScopeId,x.Scope,x.NetworkModeId,y.NetworkMode,x.BandId,y.Band,x.CarrierId,y.Carrier,
		y.GsmRssi,y.GsmRxQual,0 'WcdmaRssi',y.WcdmaRscp,y.WcdmaEcio,0 'LteRssi',y.LteRsrp,y.LteRsrq,y.LteRsnr,y.LteCqi,y.DistanceFromSite,y.AngleToSite,y.FtpStatus,y.StackTrace,y.TestResult,
		y.SubNetworkMode,y.ActualBand,y.ActualCarrier,CAST(@acceptLog as bit) 'TestStatus',y.IsHandover,y.FloorId,y.RRCState,y.NRBand,y.NRCarrier,y.NRRsrp,y.NRRsrq,y.NRRsnr,y.NRCqi,y.NRPci,
		y.FromPCI,y.ToPCI
		INTO #mobility
		FROM
		(
			SELECT DISTINCT rgn.DefinationId 'regionId',rgn.DefinationName 'Region',cty.DefinationId 'CityId',cty.DefinationName 'City',
			(SELECT TOP 1 z.PDefinationId FROM AD_Definations z WHERE z.DefinationName=@TestType AND z.DefinationTypeId=14) 'TestCategoryId',
			(SELECT TOP 1 z.DefinationName FROM AD_Definations z WHERE z.DefinationId=(SELECT z.PDefinationId FROM AD_Definations z WHERE z.DefinationName=@TestType  AND z.DefinationTypeId=14)) 'TestCategory',
			(SELECT TOP 1 z.DefinationId FROM AD_Definations z WHERE z.DefinationName=@TestType  AND z.DefinationTypeId=14) 'TestTypeId',
			x.TestType,cls.ClusterId,cls.ClusterCode 'Cluster',sit.SiteId,x.Site,0 'SectorId','' 'Sector',
			@cwScopeID 'ScopeId',@cwScope 'Scope',@cwNetModeID 'NetworkModeId',@cwNetMode 'NetworkMode',
			@cwBandID 'BandId',@cwBand 'Band',@cwCarrierID 'CarrierId',@cwCarrier 'Carrier',x.FloorId,x.RRCState
			FROM @List x
			INNER JOIN AV_Sites sit ON sit.SiteCode=x.Site
			--INNER JOIN AV_Sectors sec ON sec.SectorId=(SELECT TOP 1 x.SectorId FROM AV_Sectors x WHERE x.SiteId=sit.SiteId) AND sec.SiteId=sit.SiteId
			INNER JOIN AV_Clusters cls on sit.ClusterId=cls.ClusterId			
			INNER JOIN AD_Definations cty ON cty.DefinationId=cls.CityId
			INNER JOIN AD_Definations rgn ON rgn.DefinationId=cty.PDefinationId
			--INNER JOIN AD_Definations scp ON scp.DefinationId=sec.ScopeId
			--INNER JOIN AD_Definations ntm ON ntm.DefinationId=sec.NetworkModeId
			--INNER JOIN AD_Definations bnd ON bnd.DefinationId=sec.BandId
			--INNER JOIN AD_Definations crr ON crr.DefinationId=sec.CarrierId
			WHERE sit.SiteCode=@Sitecode
		) x
		 INNER JOIN
		(
			SELECT x.Site,x.Sector,x.NetworkMode,x.Band,x.Carrier,x.CellId,x.LacId,x.PciId,x.MccId,x.MncId,
				x.Latitude,x.Longitude,x.TimeStamp,
				x.GsmRssi,x.GsmRxQual,x.WcdmaRscp,x.WcdmaEcio,x.LteRsrp,x.LteRsrq,x.LteRsnr,x.LteCqi,x.DistanceFromSite,x.AngleToSite,x.FtpStatus,
				x.StackTrace,x.TestResult,x.SubNetworkMode,x.ActualBand,x.ActualCarrier,
				CASE WHEN x.prvPCI>=0 AND x.PciId<>x.prvPCI THEN CAST(1 as bit) ELSE CAST(0 as bit) END 'IsHandover',x.FloorId,x.RRCState,x.NRBand,x.NRCarrier,x.NRRsrp,x.NRRsrq,x.NRRsnr,x.NRCqi,x.NRPci,x.FromPCI,x.ToPCI
			FROM
			(
				SELECT x.Site,x.Sector,x.NetworkMode,x.Band,x.Carrier,x.CellId,x.LacId,x.PciId,x.MccId,x.MncId,
				x.Latitude,x.Longitude,x.TimeStamp,
				x.GsmRssi,x.GsmRxQual,x.WcdmaRscp,x.WcdmaEcio,x.LteRsrp,x.LteRsrq,x.LteRsnr,x.LteCqi,x.DistanceFromSite,x.AngleToSite,x.FtpStatus,
				x.StackTrace,x.TestResult,x.SubNetworkMode,x.ActualBand,x.ActualCarrier, --x.IsHandover
				ISNULL((SELECT TOP 1 y.PciId FROM #tmpMobility y WHERE y.Site=x.Site AND y.rowId=x.rowId-1 ORDER BY y.rowId desc),-1) 'prvPCI',x.FloorId,x.RRCState,
				x.NRBand,x.NRCarrier,x.NRRsrp,x.NRRsrq,x.NRRsnr,x.NRCqi,x.NRPci,x.FromPCI,x.ToPCI
				FROM #tmpMobility x
				--ORDER BY x.rowID
			) x
		) y ON x.Site=y.Site AND x.NetworkMode=y.SubNetworkMode AND x.Band=y.ActualBand AND x.Carrier=y.ActualCarrier
		
		--SELECT * FROM #mobility
		
		DECLARE @avHandovers [dbo].[AV_Handovers]
		
		INSERT INTO @avHandovers
		SELECT DISTINCT @SiteId, x.NetworkModeId, x.BandId, x.CarrierId, x.prvPCI, x.nxtPCI, x.DriveType
		FROM
		(
			SELECT @SiteId 'SiteId', @cwNetModeID 'NetworkModeId', @cwBandID 'BandId', @cwCarrierID 'CarrierId',
			ISNULL((SELECT TOP 1 y.PciId FROM #tmpMobility y WHERE y.Site=x.Site AND y.rowId=x.rowId-1 ORDER BY y.rowId desc),-1) 'prvPCI',
			x.PciId 'nxtPCI',@TestType 'DriveType'
			FROM #tmpMobility x			
		) x
		
		

		--SELECT DISTINCT prvpci,nxtpci FROM @avHandovers
		
		--IF @TestType='CW'
		--BEGIN
		--	UPDATE av_sitetestsummary
		--	SET CwHandoverStatus=1
		--	WHERE PciId IN(SELECT x.PciId FROM #mobility x WHERE x.isHandover=1)
		--	AND SiteId=@SiteId AND NetworkModeId=@cwNetModeID AND BandId=@cwBandID AND CarrierId=@cwCarrierID
		--END
		--ELSE IF @TestType='CCW'
		--BEGIN
		--	UPDATE av_sitetestsummary
		--	SET CcwHandoverStatus=1
		--	WHERE PciId IN(SELECT x.PciId FROM #mobility x WHERE x.isHandover=1)
		--	AND SiteId=@SiteId AND NetworkModeId=@cwNetModeID AND BandId=@cwBandID AND CarrierId=@cwCarrierID
		--END
	
		DECLARE @ClientId as int=0		
		DECLARE @CityId as int=0

		SELECT @ClientId=x.ClientId, @CityId=x.CityId FROM AV_Sites x WHERE SiteId=@SiteId
		
		INSERT INTO AV_SiteTestLog(RegionId,Region,CityId,City,TestCategoryId,TestCategory,TestTypeId,[TestType],[TimeStamp],ClusterId,Cluster,SiteId,Site,SectorId,Sector,CellId,LacId,PciId,MccId,MncId,
		Latitude,Longitude,ScopeId,Scope,NetworkModeId,NetworkMode,BandId,Band,CarrierId,Carrier,GsmRssi,GsmRxQual,WcdmaRssi,WcdmaRscp,WcdmaEcio,LteRssi,LteRsrp,LteRsrq,LteRsnr,LteCqi,DistanceFromSite,AngleToSite,FtpStatus,StackTrace,TestResult,
		SubNetworkMode,ActualBand,ActualCarrier,TestStatus,IsHandover,pciColor,rsrpColor,rsrqColor,sinrColor,ChColor,IsActive,FloorId,RRCState,NRBand,NRCarrier,NRRsrp,NRRsrq,NRRsnr,NRCqi,NRPci,FromPCI,ToPCI)
		SELECT DISTINCT x.RegionId,x.Region,x.CityId,x.City,x.TestCategoryId,x.TestCategory,x.TestTypeId,x.[TestType],x.[TimeStamp],x.ClusterId,x.Cluster,x.SiteId,x.Site,x.SectorId,x.Sector,x.CellId,x.LacId,x.PciId,x.MccId,x.MncId,x.
		Latitude,x.Longitude,x.ScopeId,x.Scope,x.NetworkModeId,x.NetworkMode,x.BandId,x.Band,x.CarrierId,x.Carrier,x.GsmRssi,x.GsmRxQual,x.WcdmaRssi,x.WcdmaRscp,x.WcdmaEcio,x.LteRssi,x.LteRsrp,x.LteRsrq,x.LteRsnr,x.LteCqi,x.DistanceFromSite,x.AngleToSite,x.FtpStatus,x.StackTrace,x.TestResult,x.
		SubNetworkMode,x.ActualBand,x.ActualCarrier,x.TestStatus,x.IsHandover,
		CASE WHEN x.isIntraSitePCI=1 THEN y.SectorColor WHEN x.isIntraSitePCI=0 THEN ISNULL((SELECT DISTINCT TOP 1 z.SectorColor FROM AV_MarketSites z WHERE z.ClientId=@ClientId AND z.CityId=@CityId AND z.NetworkModeId=@cwNetModeID AND z.BandId=@cwBandID AND z.CarrierId=@cwCarrierID AND z.PCI=x.PciId),'#87898A') END 'pciColor',
		--CASE WHEN x.isIntraSitePCI=1 THEN y.SectorColor WHEN x.isIntraSitePCI=0 THEN 
		--ISNULL((CASE WHEN x.NetworkMode='NR' then 
		--(select top 1 ColorCode from AD_Definations d
		--INNER JOIN AD_DefinationTypes dt on dt.DefinationTypeId = d.DefinationTypeId
		--where dt.DefinationType = 'Color' ORDER BY NEWID())
		--else (SELECT DISTINCT TOP 1 z.SectorColor FROM AV_MarketSites z 
		--WHERE z.ClientId=@ClientId AND z.CityId=@CityId AND z.NetworkModeId=@cwNetModeID AND 
		--z.BandId=@cwBandID AND z.CarrierId=@cwCarrierID AND z.PCI=x.PciId)  END)
		--,'#87898A') END 'pciColor',
		
		--CASE WHEN x.isIntraSitePCI=1 THEN y.SectorColor WHEN x.isIntraSitePCI=0 THEN ISNULL((SELECT DISTINCT TOP 1 z.SectorColor FROM AV_MarketSites z WHERE z.ClientId=@ClientId AND z.CityId=@CityId AND z.PCI=x.PciId),'#87898A') END 'pciColor',
		--(SELECT DISTINCT TOP 1 z.rangeColor FROM AV_RFPlotLegends z WHERE z.ClientId=@ClientId AND z.NetworkModeId=x.NetworkModeId AND z.PlotTypeId=485 AND ((x.rsrpColor>z.rangeFrom AND x.rsrpColor<z.rangeTo) OR (x.rsrpColor=z.rangeFrom AND x.rsrpColor=z.rangeTo))) 'rsrpColor',
		--(SELECT DISTINCT TOP 1 z.rangeColor FROM AV_RFPlotLegends z WHERE z.ClientId=@ClientId AND z.NetworkModeId=x.NetworkModeId AND z.PlotTypeId=486 AND ((x.rsrqColor>z.rangeFrom AND x.rsrqColor<z.rangeTo) OR (x.rsrqColor=z.rangeFrom AND x.rsrqColor=z.rangeTo))) 'rsrqColor',
		--(SELECT DISTINCT TOP 1 z.rangeColor FROM AV_RFPlotLegends z WHERE z.ClientId=@ClientId AND z.NetworkModeId=x.NetworkModeId AND z.PlotTypeId=487 AND ((x.sinrColor>z.rangeFrom AND x.sinrColor<z.rangeTo) OR (x.sinrColor=z.rangeFrom AND x.sinrColor=z.rangeTo))) 'sinrColor'
		(SELECT DISTINCT TOP 1 z.rangeColor FROM AV_RFPlotLegends z WHERE z.ClientId=@ClientId AND z.NetworkModeId=x.NetworkModeId AND z.PlotTypeId=485 AND z.LegendTypeId=(CASE WHEN @CityId IN(468,3151,3159,13174,13210,13211,13212,13213,13214,13215) THEN 1 ELSE 0 END) AND ((x.rsrpColor>z.rangeFrom AND x.rsrpColor<z.rangeTo) OR (x.rsrpColor=z.rangeFrom AND x.rsrpColor=z.rangeTo))) 'rsrpColor',
		(SELECT DISTINCT TOP 1 z.rangeColor FROM AV_RFPlotLegends z WHERE z.ClientId=@ClientId AND z.NetworkModeId=x.NetworkModeId AND z.PlotTypeId=486 AND z.LegendTypeId=(CASE WHEN @CityId IN(468,3151,3159,13174,13210,13211,13212,13213,13214,13215) THEN 1 ELSE 0 END) AND ((x.rsrqColor>z.rangeFrom AND x.rsrqColor<z.rangeTo) OR (x.rsrqColor=z.rangeFrom AND x.rsrqColor=z.rangeTo))) 'rsrqColor',
		(SELECT DISTINCT TOP 1 z.rangeColor FROM AV_RFPlotLegends z WHERE z.ClientId=@ClientId AND z.NetworkModeId=x.NetworkModeId AND z.PlotTypeId=487 AND z.LegendTypeId=(CASE WHEN @CityId IN(468,3151,3159,13174,13210,13211,13212,13213,13214,13215) THEN 1 ELSE 0 END) AND ((x.sinrColor>z.rangeFrom AND x.sinrColor<z.rangeTo) OR (x.sinrColor=z.rangeFrom AND x.sinrColor=z.rangeTo))) 'sinrColor',
		(SELECT cl.CarrierColor FROM @CarrierList AS cl WHERE cl.Carrier=(CASE WHEN x.NetworkMode='NR' then x.NRCarrier else x.Carrier END)) 'chColor', CAST(@acceptLog AS BIT) 'isActive',x.FloorId,x.RRCState,
		x.NRBand,x.NRCarrier,x.NRRsrp,x.NRRsrq,x.NRRsnr,x.NRCqi,x.NRPci,x.FromPCI,x.ToPCI
		 
		FROM
		(
			SELECT DISTINCT x.RegionId,x.Region,x.CityId,x.City,x.TestCategoryId,x.TestCategory,x.TestTypeId,x.[TestType],x.[TimeStamp],x.ClusterId,x.Cluster,x.SiteId,x.Site,x.SectorId,x.Sector,
			x.CellId,x.LacId,(CASE WHEN x.NetworkMode='NR' then x.NRPCI else x.PciId END) 'PciId',x.MccId,x.MncId,
			x.Latitude,x.Longitude,x.ScopeId,x.Scope,x.NetworkModeId,x.NetworkMode,x.BandId,x.Band,x.CarrierId,x.Carrier,
			x.GsmRssi,x.GsmRxQual,x.WcdmaRssi,x.WcdmaRscp,x.WcdmaEcio,x.LteRssi,x.LteRsrp,x.LteRsrq,x.LteRsnr,x.LteCqi,x.DistanceFromSite,x.AngleToSite,x.FtpStatus,x.StackTrace,x.TestResult,
			x.SubNetworkMode,x.ActualBand,x.ActualCarrier,x.TestStatus,x.IsHandover,x.FloorId,x.RRCState,x.NRBand,x.NRCarrier,x.NRRsrp,x.NRRsrq,x.NRRsnr,x.NRCqi,x.NRPci,
			x.FromPCI,x.ToPCI,
			CASE WHEN (CASE WHEN x.SubNetworkMode='NR' then x.NRPCI else x.PciId END)=y.PCI THEN CAST(1 as bit) ELSE CAST(0 as bit) END 'isIntraSitePCI',
			ISNULL(CASE WHEN x.NetworkModeId=7 THEN x.GsmRssi WHEN x.NetworkModeId=14 THEN x.WcdmaRscp WHEN x.NetworkModeId=15 THEN x.LteRsrp WHEN x.NetworkModeId=233926 THEN x.NRRSRP END,0) 'rsrpColor',
			ISNULL(CASE WHEN x.NetworkModeId=7 THEN x.GsmRxQual WHEN x.NetworkModeId=14 THEN x.WcdmaEcio WHEN x.NetworkModeId=15 THEN x.LteRsrq WHEN x.NetworkModeId=233926 THEN x.NRRSRQ  END,0) 'rsrqColor',
			ISNULL(CASE WHEN x.NetworkModeId=15 THEN x.LteRsnr WHEN x.NetworkModeId=233926 THEN x.NRRSNR  END,0) 'sinrColor'
			FROM #mobility x
			LEFT OUTER JOIN AV_Sectors y ON x.SiteId=y.SiteId AND x.NetworkModeId=y.NetworkModeId AND x.BandId=y.BandId AND x.CarrierId=y.CarrierId 
			AND (CASE WHEN x.NetworkMode='NR' then x.NRPCI else x.PciId END)=y.PCI
			--WHERE y.SiteId=@SiteId --AND y.NetworkModeId=@cwNetModeID AND y.BandId=@cwBandID AND y.CarrierId=@cwCarrierID
			--WHERE x.NetworkModeId=@cwNetModeID AND x.BandId=@cwBandID AND x.CarrierId=@cwCarrierID
		) x
		LEFT OUTER JOIN AV_Sectors y ON x.SiteId=y.SiteId AND x.NetworkModeId=y.NetworkModeId AND x.BandId=y.BandId AND x.CarrierId=y.CarrierId AND x.PciId=y.PCI
		--WHERE y.SiteId=@SiteId --AND y.NetworkModeId=@cwNetModeID AND y.BandId=@cwBandID AND y.CarrierId=@cwCarrierID
		
		--SELECT DISTINCT x.TestType,x.PciId
		--INTO #IntraHandovers
		--FROM AV_SiteTestLog x WHERE x.TestType IN('CW','CCW')
		--AND x.SiteId=@woID AND x.NetworkModeId=@lNetModeId AND x.BandId=@lBandId AND x.CarrierId=@lCarrierId 
		--AND x.IsHandover=1
		
		--UPDATE AV_SiteTestSummary
		--SET CwHandoverStatus = CAST(1 AS BIT)
		--WHERE SiteId=@woID AND NetworkModeId=@lNetModeId AND BandId=@lBandId AND CarrierId=@lCarrierId
		--AND PciId IN(SELECT x.PciId FROM #IntraHandovers x WHERE x.TestType='CW');
		
		--UPDATE AV_SiteTestSummary
		--SET CcwHandoverStatus = CAST(1 AS BIT)
		--WHERE SiteId=@woID AND NetworkModeId=@lNetModeId AND BandId=@lBandId AND CarrierId=@lCarrierId
		--AND PciId IN(SELECT x.PciId FROM #IntraHandovers x WHERE x.TestType='CCW');
		
		--UPDATE AV_SiteTestSummary
		--SET CcwHandoverStatus = ISNULL((SELECT DISTINCT ISNULL(isHandover,0) FROM AV_SiteTestLog x WHERE x.TestType='CCW' AND x.SiteId=@woID AND x.NetworkModeId=@lNetModeId AND x.BandId=@lBandId AND x.CarrierId=@lCarrierId AND x.PciId=AV_SiteTestSummary.PciId AND x.IsHandover=1),0)
		--WHERE SiteId=@woID AND NetworkModeId=@lNetModeId AND BandId=@lBandId AND CarrierId=@lCarrierId;
		--PRINT 'CW'
		
		DROP TABLE #tmpMobility
		DROP TABLE #mobility		
		
		EXEC [dbo].[AV_ValidateHandover] @SiteId,@cwNetModeID,@cwBandID,@cwCarrierID,@TestType,@avHandovers

		UPDATE AV_NetLayerStatus
		Set Status=450
		WHERE SiteId=@SiteId AND NetworkModeId=@cwNetModeID ANd bandid=@cwBandID ANd CarrierId=@cwCarrierID
		
		DELETE FROM @avHandovers
		--TRUNCATE TABLE @avHandovers
		--TRUNCATE TABLE @List
		
		
			--INSERT INTO AV_NeighbourLogs
			--(SiteId,NetworkModeId,BandId,CarrierId,SectorId,[TimeStamp],NetMode1,Band1,CH1,PCI1,CI1,SS1,SP1,SQ1,SNR1,
			-- NetMode2,Band2,CH2,PCI2,CI2,SS2,SP2,SQ2,SNR2,
			-- NetMode3,Band3,CH3,PCI3,CI3,SS3,SP3,SQ3,SNR3,
			-- NetMode4,Band4,CH4,PCI4,CI4,SS4,SP4,SQ4,SNR4,
			-- NetMode5,Band5,CH5,PCI5,CI5,SS5,SP5,SQ5,SNR5,
			-- NetMode6,Band6,CH6,PCI6,CI6,SS6,SP6,SQ6,SNR6,
			-- NetMode7,Band7,CH7,PCI7,CI7,SS7,SP7,SQ7,SNR7,
			-- NetMode8,Band8,CH8,PCI8,CI8,SS8,SP8,SQ8,SNR8)
			--SELECT DISTINCT 
			-- @SiteId,@cwNetModeID,@cwBandID,@cwCarrierID,
			-- (SELECT sec.SectorId FROM AV_Sectors AS sec WHERE sec.SiteId=@SiteId AND sec.NetworkModeId=@cwNetModeID AND sec.BandId=@cwBandID AND sec.CarrierId=@cwCarrierID AND sec.SectorCode=@SectorCode AND sec.isActive=1),
			-- x.[TimeStamp],
			-- x.NetMode1,x.Band1,x.CH1,x.PCI1,x.CI1,x.SS1,x.SP1,x.SQ1,x.SNR1,
			-- x.NetMode2,x.Band2,x.CH2,x.PCI2,x.CI2,x.SS2,x.SP2,x.SQ2,x.SNR2,
			-- x.NetMode3,x.Band3,x.CH3,x.PCI3,x.CI3,x.SS3,x.SP3,x.SQ3,x.SNR3,
			-- x.NetMode4,x.Band4,x.CH4,x.PCI4,x.CI4,x.SS4,x.SP4,x.SQ4,x.SNR4,
			-- x.NetMode5,x.Band5,x.CH5,x.PCI5,x.CI5,x.SS5,x.SP5,x.SQ5,x.SNR5,
			-- x.NetMode6,x.Band6,x.CH6,x.PCI6,x.CI6,x.SS6,x.SP6,x.SQ6,x.SNR6,
			-- x.NetMode7,x.Band7,x.CH7,x.PCI7,x.CI7,x.SS7,x.SP7,x.SQ7,x.SNR7,
			-- x.NetMode8,x.Band8,x.CH8,x.PCI8,x.CI8,x.SS8,x.SP8,x.SQ8,x.SNR8
			--FROM @List x
		
		RETURN 1
	END
	ELSE
	BEGIN
		
		
	
		
		SELECT x.RegionId,x.Region,x.CityId,x.City,x.TestCategoryId,x.TestCategory,x.TestTypeId,x.[TestType],y.[TimeStamp],x.ClusterId,x.Cluster,x.SiteId,x.Site,x.SectorId,x.Sector,
		y.CellId,y.LacId,y.PciId,y.MccId,y.MncId,
		y.Latitude,y.Longitude,x.ScopeId,x.Scope,x.NetworkModeId,y.NetworkMode,x.BandId,y.Band,x.CarrierId,y.Carrier,
		y.GsmRssi,y.GsmRxQual,0 'WcdmaRssi',y.WcdmaRscp,y.WcdmaEcio,0 'LteRssi',y.LteRsrp,y.LteRsrq,y.LteRsnr,y.LteCqi,y.DistanceFromSite,y.AngleToSite,y.FtpStatus,y.StackTrace,y.TestResult,
		y.SubNetworkMode,y.ActualBand,y.ActualCarrier,CAST(@acceptLog as bit) 'TestStatus',CAST(0 as bit) 'IsHandover',y.FloorId,y.RRCState,y.NRBand,y.NRCarrier,y.NRRsrp,y.NRRsrq,y.NRRsnr,y.NRCqi,y.NRPci
		INTO #tempx
		FROM
		(
		SELECT DISTINCT rgn.DefinationId 'regionId',rgn.DefinationName 'Region',cty.DefinationId 'CityId',cty.DefinationName 'City',
		(SELECT z.PDefinationId FROM AD_Definations z WHERE z.DefinationName=@TestType AND z.DefinationTypeId=14) 'TestCategoryId',
		(SELECT z.DefinationName FROM AD_Definations z WHERE z.DefinationId=(SELECT z.PDefinationId FROM AD_Definations z WHERE z.DefinationName=@TestType  AND z.DefinationTypeId=14)) 'TestCategory',
		(SELECT z.DefinationId FROM AD_Definations z WHERE z.DefinationName=@TestType  AND z.DefinationTypeId=14) 'TestTypeId',
		x.TestType,cls.ClusterId,cls.ClusterCode 'Cluster',sit.SiteId,x.Site,sec.SectorId,x.Sector,
		--scp.DefinationId 'ScopeId',scp.DefinationName 'Scope',ntm.DefinationId 'NetworkModeId',x.SubNetworkMode 'NetworkMode',
		--bnd.DefinationId 'BandId',x.ActualBand 'Band',crr.DefinationId 'CarrierId',x.ActualCarrier 'Carrier'
		@cwScopeID 'ScopeId',@cwScope 'Scope',@cwNetModeID 'NetworkModeId',@cwNetMode 'NetworkMode',
			@cwBandID 'BandId',@cwBand 'Band',@cwCarrierID 'CarrierId',@cwCarrier 'Carrier',x.FloorId,x.RRCState
		FROM @List x
		INNER JOIN AV_Sites sit ON sit.SiteCode=x.Site
		INNER JOIN AV_Sectors sec ON sec.SectorCode=x.Sector AND sec.SiteId=sit.SiteId
		INNER JOIN AV_Clusters cls on sit.ClusterId=cls.ClusterId			
		INNER JOIN AD_Definations cty ON cty.DefinationId=cls.CityId
		INNER JOIN AD_Definations rgn ON rgn.DefinationId=cty.PDefinationId
		INNER JOIN AD_Definations scp ON scp.DefinationId=sec.ScopeId
		INNER JOIN AD_Definations ntm ON ntm.DefinationId=sec.NetworkModeId
		INNER JOIN AD_Definations bnd ON bnd.DefinationId=sec.BandId
		INNER JOIN AD_Definations crr ON crr.DefinationId=sec.CarrierId
		WHERE sit.SiteCode=@Sitecode AND sec.SectorCode=@SectorCode AND ntm.DefinationName=@NetworkMode AND bnd.DefinationName=@Band AND crr.DefinationName=@Carrier
		) x
		 INNER JOIN
		(
		SELECT x.Site,x.Sector,x.NetworkMode,x.Band,x.Carrier,x.CellId,x.LacId,x.PciId,x.MccId,x.MncId,
		x.Latitude,x.Longitude,x.TimeStamp,
		x.GsmRssi,x.GsmRxQual,x.WcdmaRscp,x.WcdmaEcio,x.LteRsrp,x.LteRsrq,x.LteRsnr,x.LteCqi,x.DistanceFromSite,x.AngleToSite,x.FtpStatus,
		x.StackTrace,x.TestResult,x.SubNetworkMode,x.ActualBand,x.ActualCarrier,x.FloorId,x.RRCState,x.NRBand,x.NRCarrier,x.NRRsrp,x.NRRsrq,x.NRRsnr,x.NRCqi,x.NRPci
		FROM @List x
		) y ON x.Site=y.Site AND x.Sector=y.Sector AND x.NetworkMode=y.SubNetworkMode AND x.Band=y.ActualBand AND x.Carrier=y.ActualCarrier

		--SELECT * INTO temp FROM #tempx
		INSERT INTO AV_SiteTestLog(RegionId,Region,CityId,City,TestCategoryId,TestCategory,TestTypeId,[TestType],[TimeStamp],ClusterId,Cluster,SiteId,Site,SectorId,Sector,CellId,LacId,PciId,MccId,MncId,
		Latitude,Longitude,ScopeId,Scope,NetworkModeId,NetworkMode,BandId,Band,CarrierId,Carrier,GsmRssi,GsmRxQual,WcdmaRssi,WcdmaRscp,WcdmaEcio,LteRssi,LteRsrp,LteRsrq,LteRsnr,LteCqi,DistanceFromSite,AngleToSite,FtpStatus,StackTrace,TestResult,
		SubNetworkMode,ActualBand,ActualCarrier,TestStatus,IsHandover,FloorId,RRCState,NRBand,NRCarrier,NRRsrp,NRRsrq,NRRsnr,NRCqi,NRPci,IsActive)
		SELECT DISTINCT *,@acceptLog FROM #tempx
		--select * from #tempx
	
		
		IF @TestType IN('IDLE')
		BEGIN
			INSERT INTO AV_NeighbourLogs
			(SiteId,NetworkModeId,BandId,CarrierId,SectorId,[TimeStamp],NetMode1,Band1,CH1,PCI1,CI1,SS1,SP1,SQ1,SNR1,
			 NetMode2,Band2,CH2,PCI2,CI2,SS2,SP2,SQ2,SNR2,
			 NetMode3,Band3,CH3,PCI3,CI3,SS3,SP3,SQ3,SNR3,
			 NetMode4,Band4,CH4,PCI4,CI4,SS4,SP4,SQ4,SNR4,
			 NetMode5,Band5,CH5,PCI5,CI5,SS5,SP5,SQ5,SNR5,
			 NetMode6,Band6,CH6,PCI6,CI6,SS6,SP6,SQ6,SNR6,
			 NetMode7,Band7,CH7,PCI7,CI7,SS7,SP7,SQ7,SNR7,
			 NetMode8,Band8,CH8,PCI8,CI8,SS8,SP8,SQ8,SNR8)
			SELECT DISTINCT 
			 @SiteId,@cwNetModeID,@cwBandID,@cwCarrierID,
			 (SELECT sec.SectorId FROM AV_Sectors AS sec WHERE sec.SiteId=@SiteId AND sec.NetworkModeId=@cwNetModeID AND sec.BandId=@cwBandID AND sec.CarrierId=@cwCarrierID AND sec.SectorCode=@SectorCode AND sec.isActive=1),
			 x.[TimeStamp],
			 x.NetMode1,x.Band1,x.CH1,x.PCI1,x.CI1,CASE WHEN @NetworkMode='WCDMA' then -113+(-1*(x.WcdmaRscp+15)) else x.SS1 end 'SS1',x.SP1,x.SQ1,x.SNR1,
			 x.NetMode2,x.Band2,x.CH2,x.PCI2,x.CI2,CASE WHEN @NetworkMode='WCDMA' then -113+(-1*(x.WcdmaRscp+25)) else x.SS2 end 'SS2',x.SP2,x.SQ2,x.SNR2,
			 x.NetMode3,x.Band3,x.CH3,x.PCI3,x.CI3,CASE WHEN @NetworkMode='WCDMA' then -113+(-1*(x.WcdmaRscp+30)) else x.SS3 end 'SS3',x.SP3,x.SQ3,x.SNR3,
			 x.NetMode4,x.Band4,x.CH4,x.PCI4,x.CI4,CASE WHEN @NetworkMode='WCDMA' then x.SS4 else x.SS4 end 'SS4',x.SP4,x.SQ4,x.SNR4,
			 x.NetMode5,x.Band5,x.CH5,x.PCI5,x.CI5,CASE WHEN @NetworkMode='WCDMA' then x.SS5 else x.SS5 end 'SS5',x.SP5,x.SQ5,x.SNR5,
			 x.NetMode6,x.Band6,x.CH6,x.PCI6,x.CI6,CASE WHEN @NetworkMode='WCDMA' then x.SS6 else x.SS6 end 'SS6',x.SP6,x.SQ6,x.SNR6,
			 x.NetMode7,x.Band7,x.CH7,x.PCI7,x.CI7,CASE WHEN @NetworkMode='WCDMA' then x.SS7 else x.SS7 end 'SS7',x.SP7,x.SQ7,x.SNR7,
			 x.NetMode8,x.Band8,x.CH8,x.PCI8,x.CI8,CASE WHEN @NetworkMode='WCDMA' then x.SS8 else x.SS8 end 'SS8',x.SP8,x.SQ8,x.SNR8
			FROM @List x
		END
		
		UPDATE AV_NetLayerStatus
		Set Status=450
		WHERE SiteId=@SiteId AND NetworkModeId=@cwNetModeID ANd bandid=@cwBandID ANd CarrierId=@cwCarrierID
		
		DROP TABLE #tempx
		
		RETURN 1
	END
   

  --select * from AV_SiteTestLog
END