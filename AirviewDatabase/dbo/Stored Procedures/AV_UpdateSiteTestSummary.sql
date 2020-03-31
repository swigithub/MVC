	-- =============================================
	-- Author:		<Author,,Name>
	-- Create date: <Create Date,,>
	-- Description:	<Description,,>
	-- =============================================

	--declare @p1 dbo.Tbl_AV_SiteTestSummary insert into @p1 values(642397,100,1,1,1,1,1,1,1,1,N'True',N'True',N'True',N'True',N'True',N'True',N'True',N'True',N'/Content/AirViewLogs/TMO/Safi-111/WCDMA_UMTS 1900_9766/Capture.PNG',1,1,1,1,1,1,N'/Content/AirViewLogs/TMO/Safi-111/WCDMA_UMTS 1900_9766/Capture.PNG',N'/Content/AirViewLogs/TMO/Safi-111/WCDMA_UMTS 1900_9766/Capture.PNG',N'/Content/AirViewLogs/TMO/Safi-111/WCDMA_UMTS 1900_9766/Capture.PNG',N'True',N'True',N'True',N'True',N'True',N'True',N'/Content/AirViewLogs/TMO/Safi-111/WCDMA_UMTS 1900_9766/Capture.PNG') exec AV_UpdateSiteTestSummary @List=@p1,@UserId=11
	CREATE PROCEDURE [dbo].[AV_UpdateSiteTestSummary]
	@List Tbl_AV_SiteTestSummary READONLY,
	@UserId NUMERIC(18,0)
	AS
	BEGIN
	IF (SELECT x.[Status] FROM AV_NetLayerStatus x WHERE CAST(x.SiteId AS NVARCHAR(15))+CAST(x.NetworkModeId AS NVARCHAR(15))+CAST(x.BandId AS NVARCHAR(15))+CAST(x.CarrierId AS NVARCHAR(15))=(SELECT TOP 1 CAST(asts.SiteId AS NVARCHAR(15))+CAST(asts.NetworkModeId AS NVARCHAR(15))+CAST(asts.BandId AS NVARCHAR(15))+CAST(asts.CarrierId AS NVARCHAR(15)) FROM AV_SiteTestSummary AS asts WHERE asts.SummaryId=(SELECT TOP 1 x.summaryid  FROM @List x)))=92
	BEGIN
	DECLARE @SummaryId int
	DECLARE @LatencyRate float
	DECLARE @DownlinkRate float
	DECLARE @UplinkRate float
	DECLARE @DownlinkMaxResult float
	DECLARE @UplinkMaxResult float
	DECLARE @PingAverageResult float
	DECLARE @OoklaDownlinkResult float
	DECLARE @OoklaUplinkResult float
	DECLARE @OoklaPingResult float
	DECLARE @MoStatus bit
	DECLARE @MtStatus bit
	DECLARE @VMoStatus bit
	DECLARE @VMtStatus bit
	DECLARE @CWHandoverStatus bit
	DECLARE @CCWHandoverStatus bit
	DECLARE @ICWHandoverStatus BIT
	DECLARE @ICCWHandoverStatus bit
	DECLARE @OoklaTestFilePath nvarchar(500)
	DECLARE @OoklaRssi FLOAT
	DECLARE @OoklaSinr FLOAT
	DECLARE @TestLatitude FLOAT
	DECLARE @TestLongitude FLOAT
	DECLARE @TestRssi FLOAT
	DECLARE @TestSinr FLOAT
	DECLARE @StationaryTestFilePath NVARCHAR(500)
	DECLARE @CwTestFilePath NVARCHAR(500)
	DECLARE @CcwTestFilePath NVARCHAR(500)
	DECLARE @PingStatus BIT
	DECLARE @Downlinktatus BIT
	DECLARE @UplinkStatus bit	
	DECLARE @E911Status bit
	DECLARE @IsE911Performed bit
	DECLARE @MimoStatus bit
	DECLARE @SMoStatus bit
	DECLARE @SMtStatus bit
	DECLARE @MimoTestFilePath NVARCHAR(500)
	DECLARE @SpeedTestFilePath NVARCHAR(500)
	DECLARE @CaActiveTestFilePath NVARCHAR(500)
	DECLARE @CaDeavticeTestFilePath NVARCHAR(500)
	DECLARE @CaSpeedTestFilePath NVARCHAR(500)
	DECLARE @LaaSpeedTestFilePath NVARCHAR(500)
	DECLARE @LaaSmTestFilePath NVARCHAR(500)
	DECLARE @PhyDLSpeedMin FLOAT
	DECLARE @PhyDLSpeedMax FLOAT
	DECLARE @PhyDLSpeedAvg FLOAT
	DECLARE @PhyULSpeedMin FLOAT
	DECLARE @PhyULSpeedMax FLOAT
	DECLARE @PhyULSpeedAvg FLOAT
	DECLARE @IntraHOInteruptTime FLOAT
    DECLARE @IntreHOInteruptTime FLOAT
	DECLARE @callSetupTime FLOAT
	DECLARE @PhyDLStatus bit
	DECLARE @PhyULStatus bit
	DECLARE @CADLSpeed FLOAT
    DECLARE @CAULSpeed FLOAT

	DECLARE db_cluster CURSOR FOR  
	select * from @List
	SELECT x.SummaryId, x.DownlinkMaxResult, x.UplinkMaxResult, x.PingAverageREsult, x.OoklaPingResult, x.OoklaDownlinkResult, x.OoklaUplinkResult,x.MoStatus,x.MtStatus,x.VMoStatus, x.VMtStatus,x.CWHandoverStatus, x.CCWHandoverStatus, x.ICWHandoverStatus, x.ICCWHandoverStatus, x.OoklaTestFilePath, x.LatencyRate, x.DownlinkRate,x.UplinkRate, x.OoklaRssi, x.OoklaSinr, x.TestLatitude, x.TestLongitude, x.TestRssi, x.TestSinr,
	x.StationaryTestFilePath, x.CwTestFilePath, x.CcwTestFilePath, x.PingStatus, x.DownlinkStatus, x.UplinkStatus,x.E911Status,x.IsE911Performed,x.MimoStatus,x.SMtStatus,x.SMoStatus,x.MimoTestFilePath,x.SpeedTestFilePath,x.CaActiveTestFilePath,x.CaDeavticeTestFilePath,x.CaSpeedTestFilePath,x.LaaSpeedTestFilePath,x.LaaSmTestFilePath,x.PhyDLSpeedMin,x.PhyDLSpeedMax,x.PhyDLSpeedAvg,x.PhyULSpeedMin,x.PhyULSpeedMax,x.PhyULSpeedAvg,x.IntraHOInteruptTime,x.callSetupTime,x.IntreHOInteruptTime,x.PhyDLStatus,x.PhyULStatus,x.CADLSpeed,x.CAULSpeed
	FROM @List x
	OPEN db_cluster   
	FETCH NEXT FROM db_cluster INTO @SummaryId, @DownlinkMaxResult, @UplinkMaxResult,@PingAverageREsult, @OoklaPingResult, @OoklaDownlinkResult, @OoklaUplinkResult, @MoStatus, @MtStatus,@VMoStatus, @VMtStatus, @CWHandoverStatus, @CCWHandoverStatus, @ICWHandoverStatus, @ICCWHandoverStatus, @OoklaTestFilePath, @LatencyRate, @DownlinkRate, @UplinkRate,@OoklaRssi,@OoklaSinr, @TestLatitude, @TestLongitude, @TestRssi, @TestSinr,
	@StationaryTestFilePath, @CwTestFilePath , @CcwTestFilePath, @PingStatus, @Downlinktatus, @UplinkStatus,@E911Status,@IsE911Performed,@MimoStatus,@SMtStatus,@SMoStatus,@MimoTestFilePath,@SpeedTestFilePath,@CaActiveTestFilePath ,@CaDeavticeTestFilePath ,@CaSpeedTestFilePath ,@LaaSpeedTestFilePath ,@LaaSmTestFilePath,@PhyDLSpeedMin,@PhyDLSpeedMax,@PhyDLSpeedAvg,@PhyULSpeedMin,@PhyULSpeedMax,@PhyULSpeedAvg,@IntraHOInteruptTime,@callSetupTime,@IntreHOInteruptTime,@PhyDLStatus,@PhyULStatus,@CADLSpeed,@CAULSpeed
	WHILE @@FETCH_STATUS = 0   
	BEGIN
		UPDATE AV_SiteTestSummary
		SET LatencyRate=@LatencyRate,
		DownlinkRate=@DownlinkRate,
		UplinkRate=@UplinkRate,
		DownlinkMaxResult=@DownlinkMaxResult,
		UplinkMaxResult=@UplinkMaxResult,
		PingAverageResult=@PingAverageResult,
		OoklaPingResult=@OoklaPingResult,
		OoklaDownlinkResult=@OoklaDownlinkResult,
		OoklaUplinkResult=@OoklaUplinkResult,
		MoStatus=@MoStatus,
		MtStatus=@MtStatus,
		VMoStatus=@VMoStatus,
		VMtStatus=@VMtStatus,
		callSetupTime=@callSetupTime,
		IntraHOInteruptTime=@IntraHOInteruptTime,
		IntreHOInteruptTime=@IntreHOInteruptTime,
		PhyDLSpeedMin=@PhyDLSpeedMin,
		PhyDLSpeedMax=@PhyDLSpeedMax,
		PhyDLSpeedAvg=@PhyDLSpeedAvg,
		PhyULSpeedMin=@PhyULSpeedMin,
		PhyULSpeedMax=@PhyULSpeedMax,
		PhyULSpeedAvg =@PhyULSpeedAvg,
		CADLSpeed=	@CADLSpeed,
		CAULSpeed=@CAULSpeed,
		PingStatus=CAST(@PingStatus AS BIT),
        PhyDLStatus=CAST(@PhyDLStatus AS BIT),
		PhyULStatus=CAST(@PhyULStatus AS BIT),
		DownlinkStatus = CAST(@Downlinktatus AS BIT),
		UplinkStatus = CAST(@UplinkStatus AS BIT),		
		CwHandoverStatus=CAST(@CWHandoverStatus AS BIT),
		Ccwhandoverstatus=CAST(@CCWHandoverStatus AS BIT),
		ICwHandoverStatus=CAST(@ICWHandoverStatus AS BIT),
		ICcwHandoverStatus=CAST(@ICCWHandoverStatus AS BIT),
		OoklaTestFilePath=(CASE WHEN @OoklaTestFilePath IS NULL THEN OoklaTestFilePath when @OoklaTestFilePath = 'remove' then null ELSE @OoklaTestFilePath  END),
		StationaryTestFilePath =(CASE WHEN @StationaryTestFilePath IS NULL THEN StationaryTestFilePath when @StationaryTestFilePath ='remove'then null ELSE @StationaryTestFilePath END),
		
		CwTestFilePath =(CASE WHEN @CwTestFilePath IS NULL THEN CwTestFilePath when @CwTestFilePath ='remove' then null ELSE @CwTestFilePath END),
		CcwTestFilePath=(CASE WHEN @CcwTestFilePath IS NULL THEN CcwTestFilePath when @CcwTestFilePath='remove'then null  ELSE @CcwTestFilePath END),
		OoklaRssi=@OoklaRssi,
		OoklaSinr=@OoklaSinr,
		TestLatitude = @TestLatitude,
		TestLongitude = @TestLongitude,
		MimoStatus=@MimoStatus,
		SMtStatus=@SMtStatus,
		SMoStatus=@SMoStatus,
		
		MimoTestFilePath =(CASE WHEN @MimoTestFilePath IS NULL THEN MimoTestFilePath when @MimoTestFilePath ='remove' then null ELSE @MimoTestFilePath END),	
		SpeedTestFilePath =(CASE WHEN @SpeedTestFilePath IS NULL THEN SpeedTestFilePath  when @SpeedTestFilePath='remove' then null ELSE @SpeedTestFilePath END),	
		CaActiveTestFilePath =(CASE WHEN @CaActiveTestFilePath IS NULL THEN CaActiveTestFilePath  when @CaActiveTestFilePath='remove' then null ELSE @CaActiveTestFilePath END),
		CaDeavticeTestFilePath =(CASE WHEN @CaDeavticeTestFilePath IS NULL THEN CaDeavticeTestFilePath  when @CaDeavticeTestFilePath='remove' then null ELSE @CaDeavticeTestFilePath END),	
		CaSpeedTestFilePath =(CASE WHEN @CaSpeedTestFilePath IS NULL THEN CaSpeedTestFilePath  when @CaSpeedTestFilePath='remove' then null ELSE @CaSpeedTestFilePath END),	
		LaaSpeedTestFilePath =(CASE WHEN @LaaSpeedTestFilePath IS NULL THEN LaaSpeedTestFilePath  when @LaaSpeedTestFilePath='remove' then null ELSE @LaaSpeedTestFilePath END),	
		LaaSmTestFilePath=(CASE WHEN @LaaSmTestFilePath IS NULL THEN LaaSmTestFilePath  when @LaaSmTestFilePath='remove' then null ELSE @LaaSmTestFilePath END)		

		WHERE SummaryId=@SummaryId	

		DECLARE @NetworkMode AS NVARCHAR(50)=''
		SET @NetworkMode=(SELECT asts.NetworkMode FROM AV_SiteTestSummary AS asts WHERE asts.SummaryId=@SummaryId)
		PRINT @SummaryId
		PRINT @TestLatitude
		PRINT @TestLongitude
		
		IF @NetworkMode='GSM'
		BEGIN
			UPDATE AV_SiteTestSummary
			SET GsmRssi = @TestRssi, GsmRxQual = @TestSinr
			WHERE SummaryId=@SummaryId
		END
		ELSE IF @NetworkMode='WCDMA'
		BEGIN
			UPDATE AV_SiteTestSummary
			SET WcdmaRscp = @TestRssi, WcdmaEcio = @TestSinr
			WHERE SummaryId=@SummaryId
		END
		ELSE IF @NetworkMode='LTE'
		BEGIN
			UPDATE AV_SiteTestSummary
			SET LteRsrp = @TestRssi, LteRsnr = @TestSinr
			WHERE SummaryId=@SummaryId
		END
		   
	FETCH NEXT FROM db_cluster INTO @SummaryId, @DownlinkMaxResult, @UplinkMaxResult, @PingAverageREsult, @OoklaPingResult, @OoklaDownlinkResult, @OoklaUplinkResult,@MoStatus, @MtStatus,@VMoStatus, @VMtStatus, @CWHandoverStatus, @CCWHandoverStatus, @ICWHandoverStatus, @ICCWHandoverStatus, @OoklaTestFilePath, @LatencyRate, @DownlinkRate, @UplinkRate,@OoklaRssi,@OoklaSinr, @TestLatitude, @TestLongitude, @TestRssi, @TestSinr,
	@StationaryTestFilePath,@CwTestFilePath, @CcwTestFilePath, @PingStatus, @Downlinktatus, @UplinkStatus,@E911Status,@IsE911Performed,@MimoStatus,@SMtStatus,@SMoStatus ,@MimoTestFilePath,@SpeedTestFilePath,@CaActiveTestFilePath ,@CaDeavticeTestFilePath ,@CaSpeedTestFilePath ,@LaaSpeedTestFilePath ,@LaaSmTestFilePath,@PhyDLSpeedMin,@PhyDLSpeedMax,@PhyDLSpeedAvg,@PhyULSpeedMin,@PhyULSpeedMax,@PhyULSpeedAvg,@IntraHOInteruptTime,@callSetupTime,@IntreHOInteruptTime,@PhyDLStatus,@PhyULStatus,@CADLSpeed,@CAULSpeed

	END   

	CLOSE db_cluster   
	DEALLOCATE db_cluster
	END
	ELSE
	BEGIN
		RAISERROR('Site Test Summary Edit not Allowed!',16,1)
	END	
	END