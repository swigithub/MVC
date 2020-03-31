CREATE PROCEDURE [dbo].[AV_InsertWorkorder] 
     @Filter NVARCHAR(50) -- 'NewWorkOrder','MarketSites','UEWorkorder'
	,@Workorder Tb_AV_Workorder READONLY
	,@SubmittedById INT
	,@IMEI NVARCHAR(50)=null,
	@SiteCount as numeric(18,0)=0,
	@CellPath as NVARCHAR(500)=''
AS
BEGIN
	--SELECT @Filter,@SubmittedById,@IMEI,@SiteCount,@CellPath
	--SELECT * FROM @Workorder
	DECLARE @ReceivedOn DATETIME
	DECLARE @ClusterCode NVARCHAR(50)
	DECLARE @Region NVARCHAR(50)
	DECLARE @Market NVARCHAR(50)
	DECLARE @Client NVARCHAR(50)
	DECLARE @SiteCode NVARCHAR(50)
	DECLARE @SiteLatitude NVARCHAR(50)
	DECLARE @SiteLongitude NVARCHAR(50)
	DECLARE @Description NVARCHAR(MAX)
	DECLARE @SectorCode NVARCHAR(50)
	DECLARE @NetworkMode NVARCHAR(50)
	DECLARE @Scope NVARCHAR(50)
	DECLARE @Band NVARCHAR(50)
	DECLARE @Carrier NVARCHAR(50)
	DECLARE @Antenna NVARCHAR(50)
	DECLARE @Beamwidth NVARCHAR(50)
	DECLARE @Azimuth NVARCHAR(50)
	DECLARE @PCI NVARCHAR(50)
	DECLARE @MRBTS NVARCHAR(50)
	DECLARE @SiteAddress NVARCHAR(500)
	
	DECLARE @ClusterID INT=0
	DECLARE @SiteID INT=0
	DECLARE @siteStatusID INT=0
	DECLARE @DriveCompletedOn DATETIME
	DECLARE @IsActive BIT=0

	DECLARE @BandWidth NVARCHAR(50)
	DECLARE @CellId nvarchar(50)
	DECLARE @RFHeight NVARCHAR(50)
	DECLARE @MTilt NVARCHAR(50)
	DECLARE @ETilt NVARCHAR(50)
	DECLARE @SiteName nvarchar(50)
	DECLARE @SiteTypeId nvarchar(50)
	DECLARE @SiteClassId nvarchar(50)
	
	DECLARE @SectorLatitude nvarchar(50)
	DECLARE @SectorLongitude nvarchar(50)
	
	DECLARE @ClusterName AS NVARCHAR(50)
	DECLARE @CellFilePath AS NVARCHAR(250)
	DECLARE @SurveyId AS NVARCHAR(50)
	DECLARE @SiteSurveyId AS NUMERIC=0;
	DECLARE @SiteType AS NVARCHAR(50)
	DECLARE @ProjectId AS NUMERIC=0;
	
	DECLARE @DocumentSurveyId AS NUMERIC=0;
	DECLARE @SectorId AS NUMERIC=0
	DECLARE @CLSClusterId AS NUMERIC=0
	DECLARE @VerticalBeamwidth AS NUMERIC=0

	DECLARE @Project nvarchar(50)=''
	DECLARE @SiteClass nvarchar(50)=''
	DECLARE @Checklist nvarchar(50)=''
	DECLARE @NetLayerStatusId NUMERIC(18,0)=0

	
	
	
	SELECT @siteStatusID=ISNULL(ac.Status,0),@DriveCompletedOn=ac.DriveCompletedOn,@IsActive=CAST(ISNULL(ac.IsActive,0) AS BIT) FROM AV_Sites AS ac WHERE ac.IsActive=1 AND ac.SiteCode=(SELECT TOP 1 x.SiteCode FROM @Workorder x);
	
	DECLARE @clsNetworkModeId AS NUMERIC =ISNULL((SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationTypeId=8 AND ad.DefinationName='NA' AND ad.IsActive=1),0)
	DECLARE @clsBandId AS NUMERIC =ISNULL((SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationTypeId=10 AND ad.DefinationName='NA' AND ad.IsActive=1),0)
	DECLARE @clsCarrierId AS NUMERIC =ISNULL((SELECT TOP 1 ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationTypeId=11 AND ad.DefinationName='NA' AND ad.IsActive=1),0)
	
	DECLARE @xNetLayerId as numeric(18,0)=0
IF @Filter='NewWorkOrder'
BEGIN
	
	IF @siteStatusID IN(0,90,91,450)
	BEGIN
		
		DECLARE db_cluster CURSOR FOR  
		SELECT x.ReceivedOn, x.ClusterCode, x.Region, x.Market, x.Client, x.SiteCode, x.SiteLatitude, x.SiteLongitude, x.Description, x.SectorCode, x.NetworkMode, x.Scope, x.Band, x.Carrier, x.Antenna, x.Beamwidth, x.Azimuth, x.PCI,
		x.BandWidth, x.CellId, x.RFHeight, x.MTilt, x.ETilt, x.SiteName, x.SiteTypeId, x.SiteClassId, x.SiteAddress, x.MRBTS, x.SectorLatitude, x.SectorLongitude, x.ClusterName, x.CellFilePath, ISNULL(x.SurveyId,0), ISNULL(x.ProjectId,0),x.VerticalBeamwidth
		FROM @Workorder x
		ORDER BY x.NetworkMode,x.Band,x.Carrier,x.Scope,
		CASE WHEN x.SectorCode='Alpha' THEN 1
			 WHEN x.SectorCode='Beta' THEN 2
			 WHEN x.SectorCode='Gamma' THEN 3
			 WHEN x.SectorCode='Delta' THEN 4
			 WHEN x.SectorCode='Epsilon' THEN 5
			 WHEN x.SectorCode='DiGamma' THEN 6
			 WHEN x.SectorCode='Iota' THEN 7
		END
		--ORDER BY x.Scope,x.networkMode,x.Band,x.Carrier,x.Azimuth
		OPEN db_cluster   
		FETCH NEXT FROM db_cluster INTO @ReceivedOn, @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band, @Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI,
		@BandWidth, @CellId, @RFHeight, @MTilt, @ETilt, @SiteName, @SiteTypeId, @SiteClassId, @SiteAddress, @MRBTS, @SectorLatitude, @SectorLongitude, @ClusterName, @CellFilePath, @SurveyId, @ProjectId,@VerticalBeamwidth

		WHILE @@FETCH_STATUS = 0   
		BEGIN   
			
			DECLARE @WOStatusID AS INT=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.KeyCode='PENDING_SCHEDULED')
			DECLARE @tmpScope AS NVARCHAR(10)=(SELECT ad.DefinationName FROM AD_Definations AS ad WHERE ad.DefinationId=@Scope AND isactive=1)
			
				
		
			--Check Cluster: If Already Exists.
			IF NOT EXISTS(SELECT ac.ClusterId FROM AV_Clusters AS ac WHERE ac.CityId=@Market) AND @tmpScope IN('SSV','NI','TSS','IND')
			BEGIN
				INSERT INTO AV_Clusters(ClusterCode,CityId,ClientId,IsActive)
				VALUES(ISNULL(@ClusterCode,'-'),@Market,@Client,CAST(1 AS BIT))
			
				SELECT @ClusterID=@@IDENTITY;	
			END
			ELSE IF NOT EXISTS(SELECT ac.ClusterId FROM AV_Clusters AS ac WHERE ac.ClusterCode=@ClusterCode) AND @tmpScope IN('CLS')
			BEGIN
				INSERT INTO AV_Clusters(ClusterCode,CityId,ClientId,IsActive)
				VALUES(ISNULL(@ClusterCode,'-'),@Market,@Client,CAST(1 AS BIT))
			
				SELECT @ClusterID=@@IDENTITY;	
			END
			ELSE
			BEGIN
				SELECT @ClusterID=ac.ClusterId FROM AV_Clusters AS ac WHERE ac.CityId=@Market;
			END
		
			IF @ClusterID>0
			BEGIN
				----Check Site: If Already Exists.			
				
				DECLARE @maxWoID as int=0
				DECLARE @woRefID as nvarchar(25)
				IF NOT EXISTS(SELECT as1.SiteId FROM AV_Sites AS as1 WHERE as1.SiteCode=@SiteCode) AND @tmpScope IN('SSV','NI','TSS','IND')
				BEGIN
					SET @maxWoID=(Select ISNULL(MAX(WoCode),0) + 1 from AV_Sites Where ClientId=@Client AND (YEAR(AV_Sites.SubmittedOn)* 100 + MONTH(AV_Sites.SubmittedOn) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))					
					SET @woRefID = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientId=@Client)+'-'+ @tmpScope + '-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
					RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@maxWoID as nvarchar(15)),5))
				
					INSERT INTO AV_Sites(SiteCode,Latitude,Longitude,ClusterId,ClientId,[Description],[Status],SubmittedOn,SubmittedById,IsActive,IsPublished,IsDownloaded,
					WoCode,WoRefId,CityId,ReceivedOn,SiteName,SiteTypeId,SiteClassId,RevisionId,ScopeId,SiteAddress, ProjectId)
					VALUES(RTRIM(LTRIM(@SiteCode)),@SiteLatitude,@SiteLongitude,@ClusterID,@Client,@Description,@WOStatusID,GETDATE(),@SubmittedById,CAST(1 AS BIT),CAST(0 AS BIT),CAST(0 AS BIT),
					@maxWoID,@woRefID,@Market,@ReceivedOn,@SiteName,@SiteTypeId,@SiteClassId,0,@Scope,@SiteAddress, @ProjectId)
				
					SELECT @SiteID=@@IDENTITY;
				END
				ELSE IF NOT EXISTS(SELECT as1.SiteId FROM AV_Sites AS as1 WHERE as1.SiteCode=@SiteCode AND as1.ClusterId=@ClusterID) AND @tmpScope IN('CLS')
				BEGIN		
					SET @maxWoID=(Select ISNULL(MAX(WoCode),0) + 1 from AV_Sites Where ClientId=@Client AND (YEAR(AV_Sites.SubmittedOn)* 100 + MONTH(AV_Sites.SubmittedOn) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))					
					SET @woRefID = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientId=@Client)+'-'+ @tmpScope + '-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
					RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@maxWoID as nvarchar(15)),5))
				
					INSERT INTO AV_Sites(SiteCode,Latitude,Longitude,ClusterId,ClientId,[Description],[Status],SubmittedOn,SubmittedById,IsActive,IsPublished,IsDownloaded,
					WoCode,WoRefId,CityId,ReceivedOn,SiteName,SiteTypeId,SiteClassId,RevisionId,ScopeId,SiteAddress, ProjectId)
					VALUES('',0,0,@ClusterID,@Client,@Description,@WOStatusID,GETDATE(),@SubmittedById,CAST(1 AS BIT),CAST(0 AS BIT),CAST(0 AS BIT),
					@maxWoID,@woRefID,@Market,@ReceivedOn,'',@SiteTypeId,@SiteClassId,0,@Scope,@SiteAddress, @ProjectId)
				
					SELECT @SiteID=@@IDENTITY;
				END
				ELSE
				BEGIN
					SELECT @SiteID=ac.SiteId FROM AV_Sites AS ac WHERE ac.SiteCode=@SiteCode;
				END
			
				IF @SiteID>0
				BEGIN
					IF @tmpScope IN('SSV','NI','IND')
					BEGIN
						--IF (@siteStatusID=90)
						--BEGIN
							IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID AND as1.NetworkModeId=@NetworkMode AND as1.BandId=@Band AND as1.CarrierId=@Carrier AND as1.SectorCode=@SectorCode AND as1.Azimuth=@Azimuth AND as1.PCI=@PCI)
							BEGIN
								--DECLARE @ColorHexCode as nvarchar(10)=(SELECT x.ColorCode FROM AD_Definations x WHERE x.DefinationName=@SectorCode)
								DECLARE @ColorHexCode as nvarchar(10)=(SELECT x.SectorColor FROM AV_SectorColors x WHERE x.ClientId=@Client AND x.ScopeId=@Scope AND x.SectorCode=@SectorCode)

								INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,BandWidth,CellId,RFHeight,MTilt,ETilt,SectorColor,MRBTS,SectorLatitude,SectorLongitude,VerticalBeamwidth)
								VALUES(@SectorCode,@NetworkMode,@Scope,@Band,@Carrier,@Antenna,@Beamwidth,@Azimuth,@PCI,@SiteID,@BandWidth,@CellId,@RFHeight,@MTilt,@ETilt,@ColorHexCode,@MRBTS, @SectorLatitude, @SectorLongitude,@VerticalBeamwidth);
							END
					
							IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID AND x.NetworkModeId=@NetworkMode AND x.BandId=@Band AND x.CarrierId=@Carrier)
							BEGIN
							
								INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive)
								VALUES(@SiteID,@NetworkMode,@Scope,@Band,@Carrier,@ReceivedOn,GETDATE(),@SubmittedById,@WOStatusID,CAST(1 AS BIT));		
								
								
								SELECT @xNetLayerId=@@IDENTITY

								--SELECT @NetLayerId,@tmpNetworkModeId,@BandId
						
								insert into AV_SiteScript(SiteId,NetLayerId,RevisionId,EventTypeId,EventValue,IsValue,IsL3Enabled,Color,SequenceId,EventCommand,EventValue1,EventCommand1,SortOrder)	
								SELECT @SiteId, @xNetLayerId,0,x.EventTypeId,x.EventValue,x.IsValue,IsL3Enabled,Color,SequenceId,EventCommand,EventValue1,EventCommand1, ROW_NUMBER() OVER (ORDER BY x.SrId)
								FROM AV_TestScript x
								WHERE NetworkModeId=@NetworkMode AND BandId=@Band				
							END
						--END
						--ELSE
						--BEGIN
						--	RAISERROR('Site Edit not Allowed after Site Scheduling!',16,1)
						--END
					END
					ELSE IF @tmpScope='CLS'
					BEGIN	
						INSERT INTO AV_SiteClusters(SiteId,ClusterId,ClusterName,NetworkId,CellFilePath)
						VALUES(@SiteID,@ClusterCode,@ClusterName,@NetworkMode,@CellFilePath);

						DECLARE @siteClusterID as numeric = (SELECT @@IDENTITY)
												
												
						IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID AND as1.SiteClusterID=@siteClusterID)
						BEGIN
						--SELECT 'sector',@SiteClusterID,@Siteid
							INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,BandWidth,CellId,RFHeight,MTilt,ETilt,SectorColor,MRBTS,SectorLatitude,SectorLongitude,SiteClusterID,VerticalBeamwidth)
							VALUES('Alpha',@clsNetworkModeId,@Scope,@clsBandId,@clsCarrierId,'',0,0,0,@SiteID,0,0,0,0,0,'#ffffff',0, @SiteLatitude, @SiteLongitude,@siteClusterID,0);
						END
						
						IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID AND x.SiteClusterID=@siteClusterID)
						BEGIN				
							--SELECT 'NetLayer',@SiteClusterID,@Siteid		
							INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive,siteClusterID)
							VALUES(@SiteID,@clsNetworkModeId,@Scope,@clsBandId,@clsCarrierId,@ReceivedOn,GETDATE(),@SubmittedById,@WOStatusID,CAST(1 AS BIT),@siteClusterID);						
						END
					END
					ELSE IF @tmpScope='TSS'
					BEGIN					
						
						DECLARE @IdCount INT,@iteration INT=1,@SurveyIdNew NUMERIC(18,0)
						DECLARE @TempTableForLoop TABLE
						(
						Id INT IDENTITY(1,1),
						SurveySplitedId NUMERIC(18,0)
						)
						INSERT INTO @TempTableForLoop SELECT Item FROM SplitString(@SurveyId,',')
						
						SELECT @IdCount=COUNT(*) FROM @TempTableForLoop

						WHILE @iteration<=@IdCount
						BEGIN
						PRINT 'Enter For Iteration No ' + Convert(VARCHAR,@iteration) 
						SELECT @SurveyIdNew=SurveySplitedId FROM (
						SELECT  Id,SurveySplitedId FROM @TempTableForLoop
						) AS ff WHERE ff.Id=@iteration
						
						 

						INSERT INTO TSS_SiteSurvey(SiteId,SurveyId)
						VALUES(@SiteID,@SurveyIdNew);

												
						SELECT @SiteSurveyId=@@IDENTITY;
						
						IF (@SiteSurveyId>0)
						BEGIN		
							INSERT INTO TSS_SiteSections(SiteSurveyId,SectionId,QuestionsAnswered,TotalQuestions,StatusId,IsActive,IsRepeatable,IsApplicable,PSectionId,IsInclude)
							--SELECT @SiteSurveyId, ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',isnull (ts.IsRepeatable,0),isnull (ts.IsApplicable,0),ts.PSectionId , 1
							--FROM TSS_Sections AS ts
							--WHERE ts.SurveyId=@SurveyIdNew AND ts.IsActive=1
							SELECT  @SiteSurveyId,ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId AND tq.IsActive=1),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',isnull (ts.IsRepeatable,0) 'IsRepeatable',isnull (ts.IsApplicable,0),ts.PSectionId , 1
							FROM TSS_Sections AS ts
							WHERE ts.SurveyId=@SurveyIdNew AND psectionid=0 AND ts.IsActive=1
							UNION ALL
							SELECT  @SiteSurveyId, ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId AND tq.IsActive=1),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',isnull (ts.IsRepeatable,0) 'IsRepeatable',isnull (ts.IsApplicable,0),ts.PSectionId , 1
							FROM TSS_Sections AS ts
							WHERE ts.SurveyId=@SurveyIdNew  AND ts.IsActive=1 AND psectionid IN
							(
								SELECT ts.sectionid
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@SurveyIdNew AND psectionid=0 AND ts.IsRepeatable=0 AND ts.IsActive=1
							)
							
							
							UPDATE TSS_SiteSections
							SET PSiteSectionId = (SELECT tss.SiteSectionId FROM TSS_SiteSections AS tss WHERE tss.SiteSurveyId=@SiteSurveyId AND tss.SectionId=TSS_SiteSections.PSectionId AND tss.IsActive=1)
							WHERE SiteSurveyId=@SiteSurveyId AND IsActive=1
							AND SectionId IN
							(
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@SurveyIdNew AND ts.IsActive=1 AND psectionid IN
								(
									SELECT ts.sectionid
									FROM TSS_Sections AS ts
									WHERE ts.SurveyId=@SurveyIdNew AND ts.IsActive=1 AND psectionid=0 AND ts.IsRepeatable=0
								)
							)
							
							
							INSERT INTO TSS_SiteQuestions(SiteSurveyId,SiteSectionId,QuestionId,IsRequired,IsNoteRequired,IsImageRequired,IsBarCodeRequired,IsAnswered,IsInclude)
							SELECT @SiteSurveyId,tss.SiteSectionId,tq.QuestionId,tq.IsRequired, tq.IsNoteRequired,tq.IsImageRequired, tq.IsBarCodeRequired,CAST(0 AS BIT),1
							FROM TSS_Questions AS tq
							INNER JOIN TSS_SiteSections AS tss ON tss.SectionId=tq.SectionId
							WHERE tss.IsActive=1 AND tq.IsActive=1 AND tss.SiteSurveyId=@SiteSurveyId
							AND tss.SectionId IN
							(
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@SurveyIdNew AND psectionid=0 AND ts.IsActive=1
								UNION ALL
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@SurveyIdNew AND ts.IsActive=1 AND psectionid IN
								(
									SELECT ts.sectionid
									FROM TSS_Sections AS ts
									WHERE ts.SurveyId=@SurveyIdNew AND ts.IsActive=1 AND psectionid=0 AND ts.IsRepeatable=0
								)
							)							
						
							IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID)
							BEGIN
								INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,BandWidth,CellId,RFHeight,MTilt,ETilt,SectorColor,MRBTS,SectorLatitude,SectorLongitude,VerticalBeamwidth)
								VALUES('Alpha',@clsNetworkModeId,@Scope,@clsBandId,@clsCarrierId,'',0,0,0,@SiteID,0,0,0,0,0,'#ffffff',0, @SiteLatitude, @SiteLongitude,0);
							END
						
							IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID and x.SiteSurveyId = @SiteSurveyId)
							BEGIN							
								INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive,SiteSurveyId)
								VALUES(@SiteID,@clsNetworkModeId,@Scope,@clsBandId,@clsCarrierId,@ReceivedOn,GETDATE(),@SubmittedById,@WOStatusID,CAST(1 AS BIT),@SiteSurveyId);						
							END
						END
						
					set @iteration=@iteration+1
				END
				
					------
					END

				END
			END	   
		FETCH NEXT FROM db_cluster INTO @ReceivedOn, @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band, @Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI,
		@BandWidth, @CellId, @RFHeight, @MTilt, @ETilt, @SiteName, @SiteTypeId, @SiteClassId, @SiteAddress, @MRBTS, @SectorLatitude, @SectorLongitude, @ClusterName, @CellFilePath, @SurveyId, @ProjectId,@VerticalBeamwidth
		END   

		CLOSE db_cluster   
		DEALLOCATE db_cluster
	END
	ELSE IF @siteStatusID IN(92) AND DATEDIFF(MINUTE,@DriveCompletedOn,GETDATE())>30
	BEGIN
		
		DECLARE db_cluster CURSOR FOR  
		SELECT x.ReceivedOn, x.ClusterCode, x.Region, x.Market, x.Client, x.SiteCode, x.SiteLatitude, x.SiteLongitude, x.Description, x.SectorCode, x.NetworkMode, x.Scope, x.Band, x.Carrier, x.Antenna, x.Beamwidth, x.Azimuth, x.PCI,
		x.BandWidth, x.CellId, x.RFHeight, x.MTilt, x.ETilt, x.SiteName, x.SiteTypeId, x.SiteClassId, x.SiteAddress, x.MRBTS, x.SectorLatitude, x.SectorLongitude, x.ClusterName, x.CellFilePath, ISNULL(x.SurveyId,0), ISNULL(x.ProjectId,0),x.VerticalBeamwidth
		FROM @Workorder x
		--ORDER BY x.Scope,x.networkMode,x.Band,x.Carrier,x.Azimuth
		OPEN db_cluster   
		FETCH NEXT FROM db_cluster INTO @ReceivedOn, @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band, @Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI,
		@BandWidth, @CellId, @RFHeight, @MTilt, @ETilt, @SiteName, @SiteTypeId, @SiteClassId, @SiteAddress, @MRBTS, @SectorLatitude, @SectorLongitude, @ClusterName, @CellFilePath, @SurveyId, @ProjectId,@VerticalBeamwidth

		WHILE @@FETCH_STATUS = 0   
		BEGIN   
			DECLARE @WOStatusID1 AS INT=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.KeyCode='PENDING_SCHEDULED')
			DECLARE @tmpScope1 AS NVARCHAR(10)=(SELECT ad.DefinationName FROM AD_Definations AS ad WHERE ad.DefinationId=@Scope)
			
			--Check Cluster: If Already Exists.
			IF NOT EXISTS(SELECT ac.ClusterId FROM AV_Clusters AS ac WHERE ac.CityId=@Market) AND @tmpScope IN('SSV','NI','TSS','IND')
			BEGIN
				INSERT INTO AV_Clusters(ClusterCode,CityId,ClientId,IsActive)
				VALUES(ISNULL(@ClusterCode,'-'),@Market,@Client,CAST(1 AS BIT))
			
				SELECT @ClusterID=@@IDENTITY;	
			END
			ELSE IF NOT EXISTS(SELECT ac.ClusterId FROM AV_Clusters AS ac WHERE ac.ClusterCode=@ClusterCode) AND @tmpScope IN('CLS')
			BEGIN
				INSERT INTO AV_Clusters(ClusterCode,CityId,ClientId,IsActive)
				VALUES(ISNULL(@ClusterCode,'-'),@Market,@Client,CAST(1 AS BIT))
			
				SELECT @ClusterID=@@IDENTITY;	
			END
			ELSE
			BEGIN
				SELECT @ClusterID=ac.ClusterId FROM AV_Clusters AS ac WHERE ac.CityId=@Market;
			END
		
			IF @ClusterID>0
			BEGIN
				----Check Site: If Already Exists.	
				--DECLARE @DriveCount AS INT=(SELECT as1. FROM AV_Sites AS as1 WHERE as1.SiteCode=@SiteCode)												
				DECLARE @maxWoID1 as int=0
				SET @maxWoID1=(Select ISNULL(MAX(WoCode),0) + 1 from AV_Sites Where ClientId=@Client AND (YEAR(AV_Sites.SubmittedOn)* 100 + MONTH(AV_Sites.SubmittedOn) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))
				DECLARE @woRefID1 as nvarchar(25)
				SET @woRefID1 = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientId=@Client)+'-'+ @tmpScope1 + '-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
				RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@maxWoID1 as nvarchar(15)),5))
				
				IF NOT EXISTS(SELECT as1.SiteId FROM AV_Sites AS as1 WHERE as1.SiteCode=@SiteCode) AND @tmpScope IN('SSV','NI','TSS','IND')
				BEGIN				
					INSERT INTO AV_Sites(SiteCode,Latitude,Longitude,ClusterId,ClientId,[Description],[Status],SubmittedOn,SubmittedById,IsActive,IsPublished,IsDownloaded,
					WoCode,WoRefId,CityId,ReceivedOn,SiteName,SiteTypeId,SiteClassId,RevisionId,ScopeId,SiteAddress, ProjectId)
					VALUES(@SiteCode+'-D1',@SiteLatitude,@SiteLongitude,@ClusterID,@Client,@Description,@WOStatusID1,GETDATE(),@SubmittedById,CAST(1 AS BIT),CAST(0 AS BIT),CAST(0 AS BIT),
					@maxWoID1,@woRefID1,@Market,@ReceivedOn,@SiteName,@SiteTypeId,@SiteClassId,0,@Scope,@SiteAddress, @ProjectId)
				
					SELECT @SiteID=@@IDENTITY;
				END
				ELSE IF NOT EXISTS(SELECT as1.SiteId FROM AV_Sites AS as1 WHERE as1.SiteCode=@SiteCode AND as1.ClusterId=@ClusterID) AND @tmpScope IN('CLS')
				BEGIN					
					INSERT INTO AV_Sites(SiteCode,Latitude,Longitude,ClusterId,ClientId,[Description],[Status],SubmittedOn,SubmittedById,IsActive,IsPublished,IsDownloaded,
					WoCode,WoRefId,CityId,ReceivedOn,SiteName,SiteTypeId,SiteClassId,RevisionId,ScopeId,SiteAddress, ProjectId)
					VALUES('D1',0,0,@ClusterID,@Client,@Description,@WOStatusID1,GETDATE(),@SubmittedById,CAST(1 AS BIT),CAST(0 AS BIT),CAST(0 AS BIT),
					@maxWoID1,@woRefID1,@Market,@ReceivedOn,@SiteName,@SiteTypeId,@SiteClassId,0,@Scope,@SiteAddress, @ProjectId)
				
					SELECT @SiteID=@@IDENTITY;
				END
				ELSE
				BEGIN
					SELECT @SiteID=ac.SiteId FROM AV_Sites AS ac WHERE ac.SiteCode=@SiteCode;
				END	
			
				IF @SiteID>0
				BEGIN
					IF @tmpScope IN('SSV','NI','IND')
					BEGIN	
					--IF (@siteStatusID=90)
					--BEGIN
						IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID AND as1.NetworkModeId=@NetworkMode AND as1.BandId=@Band AND as1.CarrierId=@Carrier AND as1.SectorCode=@SectorCode AND as1.Azimuth=@Azimuth AND as1.PCI=@PCI)
						BEGIN
							--DECLARE @ColorHexCode1 as nvarchar(10)=(SELECT x.ColorCode FROM AD_Definations x WHERE x.DefinationName=@SectorCode)
							DECLARE @ColorHexCode1 as nvarchar(10)=(SELECT x.SectorColor FROM AV_SectorColors x WHERE x.ClientId=@Client AND x.ScopeId=@Scope AND x.SectorCode=@SectorCode)

							INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,BandWidth,CellId,RFHeight,MTilt,ETilt,SectorColor,MRBTS,SectorLatitude,SectorLongitude,VerticalBeamwidth)
							VALUES(@SectorCode,@NetworkMode,@Scope,@Band,@Carrier,@Antenna,@Beamwidth,@Azimuth,@PCI,@SiteID,@BandWidth,@CellId,@RFHeight,@MTilt,@ETilt,@ColorHexCode1,@MRBTS,@SectorLatitude,@SectorLongitude,@VerticalBeamwidth);
						END
					
						IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID AND x.NetworkModeId=@NetworkMode AND x.BandId=@Band AND x.CarrierId=@Carrier)
						BEGIN							
							INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive)
							VALUES(@SiteID,@NetworkMode,@Scope,@Band,@Carrier,@ReceivedOn,GETDATE(),@SubmittedById,@WOStatusID,CAST(1 AS BIT));		
							
							SELECT @xNetLayerId=@@IDENTITY

								--SELECT @NetLayerId,@tmpNetworkModeId,@BandId
						
							insert into AV_SiteScript(SiteId,NetLayerId,RevisionId,EventTypeId,EventValue,IsValue,IsL3Enabled,Color,SequenceId,EventCommand,EventValue1,EventCommand1,SortOrder)	
							SELECT @SiteId, @xNetLayerId,0,x.EventTypeId,x.EventValue,x.IsValue,IsL3Enabled,Color,SequenceId,EventCommand,EventValue1,EventCommand1, ROW_NUMBER() OVER (ORDER BY x.SrId)
							FROM AV_TestScript x
							WHERE NetworkModeId=@NetworkMode AND BandId=@Band					
						END
					--END
					--ELSE
					--BEGIN
					--	RAISERROR('Site Edit not Allowed after Site Scheduling!',16,1)
					--END
					END
					ELSE IF @tmpScope='CLS'
					BEGIN
						
						INSERT INTO AV_SiteClusters(SiteId,ClusterId,ClusterName,NetworkId,CellFilePath)
						VALUES(@SiteID,@ClusterCode,@ClusterName,@NetworkMode,@CellFilePath);

						DECLARE @siteClusterID1 as numeric = (SELECT @@IDENTITY)
						
						IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID AND as1.SiteClusterID=@siteClusterID1)
						BEGIN
							INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,BandWidth,CellId,RFHeight,MTilt,ETilt,SectorColor,MRBTS,SectorLatitude,SectorLongitude,siteClusterId,VerticalBeamwidth)
							VALUES('Alpha',@clsNetworkModeId,@Scope,@clsBandId,@clsCarrierId,'',0,0,0,@SiteID,0,0,0,0,0,'#ffffff',0, @SiteLatitude, @SiteLongitude,@siteClusterID1,0);
						END
						
						IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID AND x.SiteClusterID=@siteClusterID1)
						BEGIN							
							INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive,siteClusterID)
							VALUES(@SiteID,@clsNetworkModeId,@Scope,@clsBandId,@clsCarrierId,@ReceivedOn,GETDATE(),@SubmittedById,@WOStatusID,CAST(1 AS BIT),@siteClusterID1);						
						END
					END
					ELSE IF @tmpScope='TSS'
					BEGIN	
					
					DECLARE @IdCount2 INT,@iteration2 INT=1,@SurveyIdNew2 NUMERIC(18,0)
						DECLARE @TempTableForLoop2 TABLE
						(
						Id INT IDENTITY(1,1) ,
						SurveySplitedId NUMERIC(18,0)
						)
						INSERT INTO @TempTableForLoop2 SELECT Item FROM SplitString(@SurveyId,',')
						
						SELECT @IdCount2=COUNT(*) FROM @TempTableForLoop2

						WHILE @iteration2<=@IdCount2
						BEGIN
						SELECT @SurveyIdNew2=SurveySplitedId FROM (
						SELECT  Id,SurveySplitedId FROM @TempTableForLoop2
						) AS ff WHERE ff.Id=@iteration2
											
						INSERT INTO TSS_SiteSurvey(SiteId,SurveyId)
						VALUES(@SiteID,@SurveyIdNew2);

												

						SELECT @SiteSurveyId=@@IDENTITY;
						
						IF (@SiteSurveyId>0)
						BEGIN						
							INSERT INTO TSS_SiteSections(SiteSurveyId,SectionId,QuestionsAnswered,TotalQuestions,StatusId,IsActive,IsRepeatable,IsApplicable,PSectionId,IsInclude)
							SELECT @SiteSurveyId, ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId AND tq.IsActive=1),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',ts.IsRepeatable ,isnull (ts.IsApplicable,0),ts.PSectionId,1
							FROM TSS_Sections AS ts
							WHERE ts.SurveyId=@SurveyIdNew2 AND ts.IsActive=1
							
							INSERT INTO TSS_SiteQuestions(SiteSurveyId,SiteSectionId,QuestionId,IsRequired,IsNoteRequired,IsImageRequired,IsBarCodeRequired,IsAnswered,IsInclude)
							SELECT @SiteSurveyId,tss.SiteSectionId,tq.QuestionId,tq.IsRequired, tq.IsNoteRequired,tq.IsImageRequired, tq.IsBarCodeRequired,CAST(0 AS BIT),1
							FROM TSS_Questions AS tq
							INNER JOIN TSS_SiteSections AS tss ON tss.SectionId=tq.SectionId
							WHERE tss.IsActive=1 AND tq.IsActive=1							
						
							IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID)
							BEGIN
								INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,BandWidth,CellId,RFHeight,MTilt,ETilt,SectorColor,MRBTS,SectorLatitude,SectorLongitude,VerticalBeamwidth)
								VALUES('Alpha',@clsNetworkModeId,@Scope,@clsBandId,@clsCarrierId,'',0,0,0,@SiteID,0,0,0,0,0,'#ffffff',0, @SiteLatitude, @SiteLongitude,0);
							END
						
							IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID and x.SiteSurveyId = @SiteSurveyId)
							BEGIN							
								INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive,SiteSurveyId)
								VALUES(@SiteID,@clsNetworkModeId,@Scope,@clsBandId,@clsCarrierId,@ReceivedOn,GETDATE(),@SubmittedById,@WOStatusID,CAST(1 AS BIT),@SiteSurveyId);						
							END
						END
						set @iteration2=@iteration2+1
						End
					END
				END
			END	   
		FETCH NEXT FROM db_cluster INTO @ReceivedOn, @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band, @Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI,
		@BandWidth, @CellId, @RFHeight, @MTilt, @ETilt, @SiteName, @SiteTypeId, @SiteClassId, @SiteAddress, @MRBTS, @SectorLatitude, @SectorLongitude, @ClusterName, @CellFilePath, @SurveyId, @ProjectId,@VerticalBeamwidth
		END   

		CLOSE db_cluster   
		DEALLOCATE db_cluster
	END
	ELSE
	BEGIN
		RAISERROR('Site Edit not Allowed!',16,1)
	END
END	
ELSE IF @Filter='MarketSites'
BEGIN
		DECLARE db_cluster2 CURSOR FOR  
		SELECT x.ReceivedOn, x.ClusterCode, x.Region, x.Market, x.Client, x.SiteCode, x.SiteLatitude, x.SiteLongitude, x.Description, x.SectorCode, x.NetworkMode, x.Scope, x.Band, x.Carrier, x.Antenna, x.Beamwidth, x.Azimuth, x.PCI,
		x.BandWidth, x.CellId, x.RFHeight, x.MTilt, x.ETilt, x.SiteName, x.SiteTypeId, x.SiteClassId
		FROM @Workorder x		
		OPEN db_cluster2   
		FETCH NEXT FROM db_cluster2 INTO @ReceivedOn, @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band, @Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI,
		@BandWidth, @CellId, @RFHeight, @MTilt, @ETilt, @SiteName, @SiteTypeId, @SiteClassId

		WHILE @@FETCH_STATUS = 0   
		BEGIN   
		DECLARE @ClientID as numeric=(SELECT x.ClientId FROM AD_Clients x WHERE x.ClientName=@Client)
		DECLARE @RegionID as numeric=(SELECT x.PDefinationId FROM AD_Definations x WHERE x.DefinationName=@Market)
		DECLARE @CityId as numeric=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@Market)
		DECLARE @NetworkModeId as numeric=(SELECT x.PDefinationId FROM AD_Definations x WHERE x.DefinationName=@Band)
		DECLARE @BandId as numeric=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@Band AND x.PDefinationId=@NetworkModeId)
		DECLARE @CarrierId as numeric=(SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@Carrier AND x.PDefinationId=@BandId)
		
		IF ISNULL(@CarrierId,0)=0
		BEGIN
			INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,[MaxLength],SortOrder,IsActive)
			SELECT @Carrier,@BandId,11,'CARRIER',0,0,CAST(1 AS BIT)
		END

		DECLARE @ColorCode as nvarchar(50)=''
		
		IF ISNULL((SELECT DISTINCT TOP 1 SectorColor FROM AV_MarketSites WHERE PCI=@PCI),'')=''
		BEGIN
			SET @ColorCode=(SELECT TOP 1 x.ColorCode FROM AD_Definations x WHERE x.DefinationTypeId=(SELECT y.DefinationTypeId FROM AD_DefinationTypes y WHERE y.DefinationType='Color') AND x.IsActive=1 order by newid());
		END
		ELSE
		BEGIN
			SET @ColorCode=(SELECT DISTINCT TOP 1 SectorColor FROM AV_MarketSites WHERE PCI=@PCI)
		END
			
		INSERT INTO AV_MarketSites1(ClientId,RegionId,CityId,SiteCode,Latitude,Longitude,SectorCode,NetworkModeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,RFHeight,MTilt,ETilt,
		BandWidth,SectorColor,CellId)
		SELECT @ClientID,@RegionID,@CityId,@SiteCode,@SiteLatitude,@SiteLongitude,@SectorCode,@NetworkModeId,@BandId,@CarrierId,@Antenna,@Beamwidth,@Azimuth,@PCI,CAST(@RFHeight as int),@MTilt,
		@ETilt,
		@BandWidth,
		@ColorCode,
		--(SELECT TOP 1 x.ColorCode FROM AD_Definations x WHERE x.DefinationTypeId=(SELECT y.DefinationTypeId FROM AD_DefinationTypes y WHERE y.DefinationType='Color') AND x.IsActive=1 order by newid()),
		@CellId
			
		FETCH NEXT FROM db_cluster2 INTO @ReceivedOn, @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band, @Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI,
		@BandWidth, @CellId, @RFHeight, @MTilt, @ETilt, @SiteName, @SiteTypeId, @SiteClassId
		END   

		CLOSE db_cluster2   
		DEALLOCATE db_cluster2
END
ELSE IF @Filter='UEWorkOrder' --AND @IsActive=1
BEGIN
	IF @siteStatusID IN(0,90,91,450)
	BEGIN
	SELECT DISTINCT * from @Workorder
		DECLARE db_cluster CURSOR FOR  
		
		SELECT x.ReceivedOn, x.ClusterCode, x.Region, x.Market, x.Client, x.SiteCode, x.SiteLatitude, x.SiteLongitude, x.Description, x.SectorCode, x.NetworkMode, x.Scope, x.Band, x.Carrier, 'n/a', x.Beamwidth, x.Azimuth, x.PCI,
		x.BandWidth, x.CellId, x.RFHeight, x.MTilt, x.ETilt, x.SiteName, x.SiteTypeId, x.SiteClassId, x.SiteAddress, x.MRBTS, x.SectorLatitude, x.SectorLongitude,x.CellId,x.RFHeight,x.MTilt,x.VerticalBeamwidth,x.SurveyId
		FROM @Workorder x				
		ORDER BY x.NetworkMode,x.Band,x.Carrier,x.Scope,
		CASE WHEN x.SectorCode='Alpha' THEN 1
			 WHEN x.SectorCode='Beta' THEN 2
			 WHEN x.SectorCode='Gamma' THEN 3
			 WHEN x.SectorCode='Delta' THEN 4
			 WHEN x.SectorCode='Epsilon' THEN 5
			 WHEN x.SectorCode='DiGamma' THEN 6
			 WHEN x.SectorCode='Iota' THEN 7
		END
		--ORDER BY x.Scope,x.networkMode,x.Band,x.Carrier,x.Azimuth
		OPEN db_cluster   
		FETCH NEXT FROM db_cluster INTO @ReceivedOn, @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band, @Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI,
		@BandWidth, @CellId, @RFHeight, @MTilt, @ETilt, @SiteName, @SiteTypeId, @SiteClassId, @SiteAddress, @MRBTS, @SectorLatitude, @SectorLongitude,@CellId,@RFHeight,@MTilt,@VerticalBeamwidth,@SurveyId

		WHILE @@FETCH_STATUS = 0   
		BEGIN   
			DECLARE @mNetworkModeID AS NUMERIC=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationName=@NetworkMode AND ad.DefinationTypeId=8 AND ad.IsActive=1)
			DECLARE @mBandId AS NUMERIC=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationName=@Band AND ad.PDefinationId=@mNetworkModeID AND ad.DefinationTypeId=10 AND ad.IsActive=1)
			DECLARE @mCarrierId AS NUMERIC=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationName=@Carrier AND ad.PDefinationId=@mBandId AND ad.DefinationTypeId=11 AND ad.IsActive=1)
			DECLARE @mScopeId AS NUMERIC=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationName=@Scope AND ad.DefinationTypeId=12 AND ad.IsActive=1)
			DECLARE @mMarketId AS NUMERIC=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationName=@Market AND ad.DefinationTypeId=7 AND ad.IsActive=1)
			DECLARE @mClientId AS NUMERIC=(SELECT ad.ClientId FROM AD_Clients AS ad WHERE ad.ClientName=@Client AND IsActive=1)
			DECLARE @mPending_Schedule AS INT =(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.KeyCode='PENDING_SCHEDULED' AND isActive=1)
			
			
			--PRINT 'NEW' +@Market + ISNULL(CAST(@mMarketId AS NVARCHAR(50)),'-1')
			--Check Cluster: If Already Exists.
			IF NOT EXISTS(SELECT ac.ClusterId FROM AV_Clusters AS ac WHERE ac.CityId=@mMarketId AND IsActive=1)
			BEGIN				
				INSERT INTO AV_Clusters(ClusterCode,CityId,ClientId,IsActive)
				VALUES(ISNULL(@ClusterCode,'-'),@mMarketId,@mClientId,CAST(1 AS BIT))
			
				SELECT @ClusterID=@@IDENTITY;	
				
				
			END
			ELSE
			BEGIN
				SELECT @ClusterID=ac.ClusterId FROM AV_Clusters AS ac WHERE ac.CityId=@mMarketId AND IsActive=1;
				
				--PRINT 'EXISTS' + CAST(@ClusterId AS NVARCHAR(50))
			END
		
			IF @ClusterID>0
			BEGIN
				----Check Site: If Already Exists.
				DECLARE @mWOStatusID AS INT=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.KeyCode='PENDING_SCHEDULED' AND isActive=1)
				DECLARE @mtmpScope AS NVARCHAR(10)=(SELECT ad.DefinationName FROM AD_Definations AS ad WHERE ad.DefinationId=@mScopeId AND isActive=1)
				
				IF NOT EXISTS(SELECT as1.SiteId FROM AV_Sites AS as1 WHERE as1.SiteCode=@SiteCode)
				BEGIN		
						
					DECLARE @mMaxWoID as int=0
					SET @mMaxWoID=(Select ISNULL(MAX(WoCode),0) + 1 from AV_Sites Where ClientId=@mClientId AND (YEAR(AV_Sites.SubmittedOn)* 100 + MONTH(AV_Sites.SubmittedOn) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))
					DECLARE @mWoRefID as nvarchar(25)
					SET @mWoRefID = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientId=@mClientId AND AD_Clients.IsActive=1)+'-'+ @mtmpScope + '-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
					RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@mMaxWoID as nvarchar(15)),5))
				
					INSERT INTO AV_Sites(SiteCode,Latitude,Longitude,ClusterId,ClientId,[Description],[Status],SubmittedOn,SubmittedById,IsActive,IsPublished,IsDownloaded,
					WoCode,WoRefId,CityId,ReceivedOn,SiteName,SiteTypeId,SiteClassId,RevisionId,ScopeId,SiteAddress)
					VALUES(RTRIM(LTRIM(@SiteCode)),@SiteLatitude,@SiteLongitude,@ClusterID,@mClientId,@Description,@mWOStatusID,GETDATE(),@SubmittedById,CAST(1 AS BIT),CAST(0 AS BIT),CAST(0 AS BIT),
					@mMaxWoID,@mWoRefID,@mMarketId,@ReceivedOn,@SiteName,@SiteTypeId,@SiteClassId,0,@mScopeId,@SiteAddress)
				
					SELECT @SiteID=@@IDENTITY;
				END
				ELSE
				BEGIN
					SELECT @SiteID=ac.SiteId FROM AV_Sites AS ac WHERE ac.SiteCode=@SiteCode;
					
					IF @IsActive=1
					BEGIN
						UPDATE AV_Sites
						SET Latitude = @SiteLatitude, Longitude = @SiteLongitude
						WHERE SiteId=@SiteID;
					END
				END
				IF @mtmpScope='TSS'
					BEGIN					
						
						DECLARE @UE_IdCount INT,@UE_iteration INT=1,@UE_SurveyIdNew NUMERIC(18,0)
						DECLARE @UE_TempTableForLoop TABLE
						(
						Id INT IDENTITY(1,1),
						SurveySplitedId NUMERIC(18,0)
						)
						INSERT INTO @UE_TempTableForLoop SELECT Item FROM SplitString(@SurveyId,',')
						SELECT * FROM @UE_TempTableForLoop
						SELECT @UE_IdCount=COUNT(*) FROM @UE_TempTableForLoop

						WHILE @UE_iteration<=@UE_IdCount
						BEGIN
						PRINT 'Enter For Iteration No ' + Convert(VARCHAR,@UE_iteration) 
						SELECT @UE_SurveyIdNew=SurveySplitedId FROM (
						SELECT  Id,SurveySplitedId FROM @UE_TempTableForLoop
						) AS ff WHERE ff.Id=@UE_iteration
						
						 

						INSERT INTO TSS_SiteSurvey(SiteId,SurveyId)
						VALUES(@SiteID,@UE_SurveyIdNew);

												
						SELECT @SiteSurveyId=@@IDENTITY;
						
						IF (@SiteSurveyId>0)
						BEGIN		
							INSERT INTO TSS_SiteSections(SiteSurveyId,SectionId,QuestionsAnswered,TotalQuestions,StatusId,IsActive,IsRepeatable,IsApplicable,PSectionId,IsInclude)
							
							SELECT  @SiteSurveyId,ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId AND tq.IsActive=1),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',isnull (ts.IsRepeatable,0) 'IsRepeatable',isnull (ts.IsApplicable,0),ts.PSectionId , 1
							FROM TSS_Sections AS ts
							WHERE ts.SurveyId=@UE_SurveyIdNew AND psectionid=0 AND ts.IsActive=1
							UNION ALL
							SELECT  @SiteSurveyId, ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId AND tq.IsActive=1),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',isnull (ts.IsRepeatable,0) 'IsRepeatable',isnull (ts.IsApplicable,0),ts.PSectionId , 1
							FROM TSS_Sections AS ts
							WHERE ts.SurveyId=@UE_SurveyIdNew  AND ts.IsActive=1 AND psectionid IN
							(
								SELECT ts.sectionid
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@UE_SurveyIdNew AND psectionid=0 AND ts.IsRepeatable=0 AND ts.IsActive=1
							)
							
							
							UPDATE TSS_SiteSections
							SET PSiteSectionId = (SELECT tss.SiteSectionId FROM TSS_SiteSections AS tss WHERE tss.SiteSurveyId=@SiteSurveyId AND tss.SectionId=TSS_SiteSections.PSectionId AND tss.IsActive=1)
							WHERE SiteSurveyId=@SiteSurveyId AND IsActive=1
							AND SectionId IN
							(
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@UE_SurveyIdNew AND ts.IsActive=1 AND psectionid IN
								(
									SELECT ts.sectionid
									FROM TSS_Sections AS ts
									WHERE ts.SurveyId=@UE_SurveyIdNew AND ts.IsActive=1 AND psectionid=0 AND ts.IsRepeatable=0
								)
							)
							
							
							INSERT INTO TSS_SiteQuestions(SiteSurveyId,SiteSectionId,QuestionId,IsRequired,IsNoteRequired,IsImageRequired,IsBarCodeRequired,IsAnswered,IsInclude)
							SELECT @SiteSurveyId,tss.SiteSectionId,tq.QuestionId,tq.IsRequired, tq.IsNoteRequired,tq.IsImageRequired, tq.IsBarCodeRequired,CAST(0 AS BIT),1
							FROM TSS_Questions AS tq
							INNER JOIN TSS_SiteSections AS tss ON tss.SectionId=tq.SectionId
							WHERE tss.IsActive=1 AND tq.IsActive=1 AND tss.SiteSurveyId=@SiteSurveyId
							AND tss.SectionId IN
							(
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@UE_SurveyIdNew AND psectionid=0 AND ts.IsActive=1
								UNION ALL
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@UE_SurveyIdNew AND ts.IsActive=1 AND psectionid IN
								(
									SELECT ts.sectionid
									FROM TSS_Sections AS ts
									WHERE ts.SurveyId=@UE_SurveyIdNew AND ts.IsActive=1 AND psectionid=0 AND ts.IsRepeatable=0
								)
							)			
							IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID)
							BEGIN
								INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,BandWidth,CellId,RFHeight,MTilt,ETilt,SectorColor,MRBTS,SectorLatitude,SectorLongitude,VerticalBeamwidth)
								VALUES('Alpha',@clsNetworkModeId,@mScopeId,@clsBandId,@clsCarrierId,'',0,0,0,@SiteID,0,0,0,0,0,'#ffffff',0, @SiteLatitude, @SiteLongitude,0);
							END
							IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID and x.SiteSurveyId = @SiteSurveyId)
							BEGIN	
							
								INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive,SiteSurveyId)
								VALUES(@SiteID,@clsNetworkModeId,@mScopeId,@clsBandId,@clsCarrierId,@ReceivedOn,GETDATE(),@SubmittedById,@mPending_Schedule,CAST(1 AS BIT),@SiteSurveyId);						
							END
						END
						
					set @UE_iteration=@UE_iteration+1
				END
				
					------
					END
				ELSE
							
				BEGIN
					--IF (@siteStatusID=90)
					--BEGIN
						IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID AND as1.NetworkModeId=@mNetworkModeID AND as1.BandId=@mBandID AND as1.CarrierId=@mCarrierID AND as1.SectorCode=@SectorCode)
						BEGIN
							--DECLARE @mColorHexCode as nvarchar(10)=(SELECT x.ColorCode FROM AD_Definations x WHERE x.DefinationName=@SectorCode)
							DECLARE @mColorHexCode as nvarchar(10)=(SELECT x.SectorColor FROM AV_SectorColors x WHERE x.ClientId=@mClientId AND x.ScopeId=@mScopeId AND x.SectorCode=@SectorCode)

							INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,BandWidth,CellId,RFHeight,MTilt,ETilt,SectorColor,MRBTS,SectorLatitude,SectorLongitude,VerticalBeamwidth,AntennaDowntilt)
							VALUES(@SectorCode,@mNetworkModeID,@mScopeId,@mBandID,@mCarrierID,@Antenna,@Beamwidth,@Azimuth,@PCI,@SiteID,@BandWidth,@CellId,82,4,4,@mColorHexCode,@MRBTS,@SectorLatitude,@SectorLongitude,@VerticalBeamwidth,4);
						END
						ELSE
						BEGIN
							IF @IsActive=1
							BEGIN
								UPDATE AV_Sectors
								SET BandWidth = @BandWidth, Azimuth = @Azimuth, PCI = @PCI, SectorLatitude = @SectorLatitude, SectorLongitude = @SectorLongitude
								WHERE SiteId=@SiteID AND NetworkModeId=@mNetworkModeID AND BandId=@mBandId AND CarrierId=@mCarrierId AND SectorCode=@SectorCode;
							
								UPDATE AV_SiteTestSummary
								SET Latitude = @SiteLatitude, Longitude = @SiteLongitude, BeamWidth = @Beamwidth, Azimuth = @Azimuth, PciId = @PCI							
								WHERE SiteId=@SiteID AND NetworkModeId=@mNetworkModeID AND BandId=@mBandId AND CarrierId=@mCarrierId AND Sector=@SectorCode;
							END
						END						
					
						IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID AND x.NetworkModeId=@mNetworkModeID AND x.BandId=@mBandID AND x.CarrierId=@mCarrierID)
						BEGIN							
							INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive)
							VALUES(@SiteID,@mNetworkModeID,@mScopeId,@mBandID,@mCarrierID,@ReceivedOn,GETDATE(),@SubmittedById,@mWOStatusID,CAST(1 AS BIT));
							
							DECLARE @fNetLayerId as numeric(18,0)=0
							SELECT @fNetLayerId=@@IDENTITY

							--SELECT @NetLayerId,@tmpNetworkModeId,@BandId
						
							insert into AV_SiteScript(SiteId,NetLayerId,RevisionId,EventTypeId,EventValue,IsValue,IsL3Enabled,Color,SequenceId,EventCommand,EventValue1,EventCommand1,SortOrder)	
							SELECT @SiteId, @fNetLayerId,0,x.EventTypeId,x.EventValue,x.IsValue,IsL3Enabled,Color,SequenceId,EventCommand,EventValue1,EventCommand1,ROW_NUMBER() OVER (ORDER BY x.SrId)
							FROM AV_TestScript x
							WHERE NetworkModeId=@mNetworkModeID AND BandId=@mBandID							
						END
						
					--END
					--ELSE
					--BEGIN
					--	RAISERROR('Site Edit not Allowed after Site Scheduling!',16,1)
					--END
				END
			END	   
		FETCH NEXT FROM db_cluster INTO @ReceivedOn, @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band, @Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI,
		@BandWidth, @CellId, @RFHeight, @MTilt, @ETilt, @SiteName, @SiteTypeId, @SiteClassId, @SiteAddress, @MRBTS, @SectorLatitude, @SectorLongitude,@CellId,@RFHeight,@MTilt,@VerticalBeamwidth,@SurveyId
		END   

		CLOSE db_cluster   
		DEALLOCATE db_cluster
	END
	
	DECLARE @UEID AS NUMERIC=(SELECT TOP 1 sud.DeviceId FROM Sec_UserDevices AS sud WHERE sud.UserId=@SubmittedById AND sud.IMEI=@IMEI AND sud.isActive=1)
	SELECT @UEID
	--Schedule Site
	DECLARE schedule_site CURSOR FOR  
		SELECT DISTINCT x.NetworkModeId, x.ScopeId, x.BandId, x.CarrierId,x.LayerStatusId
		FROM AV_NetLayerStatus x
		WHERE x.SiteId=@SiteID AND x.[Status]=90		
		
		OPEN schedule_site   
		FETCH NEXT FROM schedule_site INTO @NetworkMode, @Scope, @Band, @Carrier,@NetLayerStatusId
	WHILE @@FETCH_STATUS = 0   
	BEGIN  
		SELECT 0;
	--	@SequenceId numeric=0,	
	--@Filter AS NVARCHAR(50)='n/a',
	--@List List READONLY ,
	--@SiteClusterId AS NUMERIC=0,
 --   @NetLayerId AS NUMERIC=0,
	--@DeviceScheduleId AS NUMERIC=0,
	--@isMaster AS bit=0
		DECLARE @currTimeStamp AS DATETIME=CAST(GETDATE() AS DATE)
		DECLARE @UE_Scope AS NVARCHAR(50)=(SELECT DefinationName FROM AD_Definations WHERE DefinationId=@Scope)
		
		IF @UE_Scope='TSS'
		BEGIN
		DECLARE @ScheduledStatusID AS INT = (SELECT DefinationId FROM AD_Definations WHERE KeyCode LIKE 'SCHEDULED')

		EXEC AV_ScheduleSite @SiteId=@SiteID,@TesterId=@SubmittedById,@TesterassignedById=@SubmittedById,@SchduledOn=@currTimeStamp,
								@Status=@ScheduledStatusID,@NetworkModeId=@NetworkMode,@BandId=@Band,@CarrierId=@Carrier,@UserDeviceId=@UEID,
								@TestTypes=N'',@SequenceId=0,@DeviceScheduleId=0,@LayerStatusId=@NetLayerStatusId
		END
		ELSE
		BEGIN

		EXEC AV_ScheduleSite @SiteID, @SubmittedById, @SubmittedById, @currTimeStamp, '91', @NetworkMode, @Band, @Carrier, @UEID, '',0,'UEWorkOrder',0,0,0,0
			 SELECT 1;  
		END
	FETCH NEXT FROM schedule_site INTO @NetworkMode, @Scope, @Band, @Carrier,@NetLayerStatusId
	END   

	CLOSE schedule_site   
	DEALLOCATE schedule_site
END
ELSE IF @Filter = 'File_WO'
BEGIN
SELECT * from @Workorder
	DECLARE db_upload_wo CURSOR FOR  
		SELECT x.ClusterCode, x.Region, x.Market, x.Client, x.SiteCode, x.SiteLatitude, x.SiteLongitude, x.Description, x.SectorCode, x.NetworkMode, x.Scope, x.Band, x.Carrier,
			   x.Antenna, x.Beamwidth, x.Azimuth, x.PCI, x.ReceivedOn,x.SiteTypeId,x.CellId,x.RFHeight,x.MTilt,x.VerticalBeamwidth,x.Project, x.SiteType, x.SiteClass,  x.Checklist
			   ,x.SiteName,x.SiteAddress
		FROM @Workorder x		
		OPEN db_upload_wo   
		FETCH NEXT FROM db_upload_wo INTO @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band,
		@Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI, @ReceivedOn,@SiteType,@CellId,@RFHeight,@MTilt,@VerticalBeamwidth, @Project, @SiteType, @SiteClass, @Checklist,@SiteName,@SiteAddress


		WHILE @@FETCH_STATUS = 0   
		BEGIN   
			DECLARE @tmpClientID as numeric=ISNULL((SELECT x.ClientId FROM AD_Clients x WHERE x.ClientName=@Client AND x.IsActive=1),0)
			DECLARE @tmpSiteTypeID as numeric=ISNULL((SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@SiteType AND DefinationTypeId=28  AND x.IsActive=1),0)
			DECLARE @tmpSiteClassID as numeric=ISNULL((SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@SiteClass AND DefinationTypeId=29  AND x.IsActive=1),0)
			DECLARE @tmpRegionID as numeric=ISNULL((SELECT x.PDefinationId FROM AD_Definations x WHERE x.DefinationName=@Market  AND x.IsActive=1),0)
			DECLARE @tmpCityId as numeric=ISNULL((SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@Market AND x.IsActive=1),0)
			DECLARE @tmpScopeId as numeric=ISNULL((SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@Scope AND x.DefinationTypeId=12 AND x.IsActive=1),0)
			DECLARE @tmpNetworkModeId as numeric=ISNULL((SELECT x.PDefinationId FROM AD_Definations x WHERE x.DefinationName=@Band  AND x.IsActive=1),0)
			DECLARE @tmpBandId as numeric=ISNULL((SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@Band AND x.PDefinationId=@tmpNetworkModeId  AND x.IsActive=1),0)
			DECLARE @tmpCarrierId as numeric=ISNULL((SELECT x.DefinationId FROM AD_Definations x WHERE x.DefinationName=@Carrier AND x.PDefinationId=@tmpBandId AND x.IsActive=1),0)
			
			DECLARE @tmpSiteCode AS NVARCHAR(50)=''
			DECLARE @tmpSiteStatusId AS NUMERIC(18,0)=0
			
			SELECT @tmpSiteCode=ISNULL(x.SiteCode,''), @tmpSiteStatusId=ISNULL(x.[Status],0)
			FROM AV_Sites AS x
			WHERE x.SiteCode=@SiteCode

			SELECT @ProjectId=ProjectId
			FROM PM_Projects
			WHERE ProjectName=@Project

			SET @SiteClassId=(SELECT DefinationId FROM AD_DEfinations WHERE KeyCode='SITE_CLASS')
			
				
			DECLARE @tmpAlreadyExists AS NVARCHAR(500)='Already Existed Site Codes: '
			
			--IF @tmpSiteCode!=''
			--BEGIN
				
			--END
			--ELSE
			--BEGIN
				
			--END

			--SELECT @tmpClientID,@tmpRegionID,@tmpCityId,@tmpScopeId,@tmpNetworkModeId,@tmpBandId,@tmpCarrierId
			
			
			IF NOT EXISTS(SELECT ac.ClusterId FROM AV_Clusters AS ac WHERE ac.CityId=@tmpCityId AND ac.IsActive=1)
			BEGIN
				INSERT INTO AV_Clusters(ClusterCode,CityId,ClientId,IsActive)
				VALUES(ISNULL(@ClusterCode,'-'),@tmpCityId,@tmpClientID,CAST(1 AS BIT))
			
				SELECT @ClusterID=@@IDENTITY;	
			END
			ELSE
			BEGIN
				SELECT @ClusterID=ac.ClusterId FROM AV_Clusters AS ac WHERE ac.CityId=@tmpCityId AND ac.IsActive=1;
			END			
			
			IF @ClusterID>0
			BEGIN	
				----Check Site: If Already Exists.
				DECLARE @WOStatusID6 AS NUMERIC(18,0)=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.KeyCode='PENDING_SCHEDULED' AND ad.IsActive=1)
				DECLARE @tmpScope6 AS NVARCHAR(10)=(SELECT ad.DefinationName FROM AD_Definations AS ad WHERE ad.DefinationId=@tmpScopeId AND ad.IsActive=1)
				DECLARE @curSiteStatusId AS NUMERIC(18,0)=0
				--PRINT @WOStatusID6
				
				IF NOT EXISTS(SELECT as1.SiteId FROM AV_Sites AS as1 WHERE as1.SiteCode=@SiteCode)
				BEGIN				
					DECLARE @maxWoID6 as int=0
					SET @maxWoID6=(Select ISNULL(MAX(WoCode),0) + 1 from AV_Sites Where ClientId=@tmpClientID AND IsActive=1 AND (YEAR(AV_Sites.SubmittedOn)* 100 + MONTH(AV_Sites.SubmittedOn) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))
					DECLARE @woRefID6 as nvarchar(25)
					SET @woRefID6 = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientId=@tmpClientID AND AD_Clients.IsActive=1)+'-'+ @tmpScope6 + '-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
					RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@maxWoID6 as nvarchar(15)),5))
				
					--PRINT 3;
					INSERT INTO AV_Sites(SiteCode,Latitude,Longitude,ClusterId,ClientId,[Description],[Status],SubmittedOn,SubmittedById,IsActive,IsPublished,IsDownloaded,
					WoCode,WoRefId,CityId,ReceivedOn,SiteName,SiteTypeId,SiteClassId,RevisionId,ScopeId,SiteAddress,ProjectId)
					VALUES(RTRIM(LTRIM(@SiteCode)),@SiteLatitude,@SiteLongitude,@ClusterID,@tmpClientID,@Description,@WOStatusID6,GETDATE(),@SubmittedById,CAST(1 AS BIT),CAST(0 AS BIT),CAST(0 AS BIT),
					@maxWoID6,@woRefID6,@tmpCityId,@ReceivedOn,@SiteName,@tmpSiteTypeID,@SiteClassId,0,@tmpScopeId,@SiteAddress,@ProjectId)
				
					SELECT @SiteID=@@IDENTITY;
					
					SET @curSiteStatusId=@WOStatusID6
				END
				ELSE
				BEGIN
					SELECT @SiteID=ac.SiteId FROM AV_Sites AS ac WHERE ac.SiteCode=@SiteCode;
					SET @curSiteStatusId=ISNULL((SELECT x.[Status] FROM AV_Sites x WHERE x.SiteId=@SiteID AND x.IsActive=1),0)
					
					IF @curSiteStatusId=90
					BEGIN
						UPDATE AV_Sites
						SET Latitude = @SiteLatitude, Longitude = @SiteLongitude, [Description] = @Description, SiteTypeId = @SiteTypeId, SiteClassId = @SiteClassId, SiteAddress = @SiteAddress
						WHERE SiteId=@SiteId
					END
					ELSE
					BEGIN
						SET @tmpAlreadyExists =  @tmpAlreadyExists + ',' + @SiteCode;
					END
				END
				
				IF @SiteID>0
				BEGIN
					IF @Scope IN('SSV','NI','IND')
					BEGIN
					IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.isActive=1 AND as1.SiteId=@SiteID AND as1.NetworkModeId=@tmpNetworkModeId AND as1.BandId=@tmpBandId AND as1.CarrierId=@tmpCarrierId AND as1.SectorCode=@SectorCode)
					BEGIN
						--SELECT 'Sector: '+ CAST(@tmpNetworkModeId AS NVARCHAR(50))
						--DECLARE @ColorHexCode1 as nvarchar(10)=(SELECT x.ColorCode FROM AD_Definations x WHERE x.DefinationName=@SectorCode)
						DECLARE @ColorHexCode2 as nvarchar(10)=(SELECT x.SectorColor FROM AV_SectorColors x WHERE x.ClientId=@tmpClientID AND x.ScopeId=@tmpScopeId AND x.SectorCode=@SectorCode)
						--PRINT 1;
						INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,BandWidth,CellId,RFHeight,MTilt,ETilt,SectorColor,MRBTS,SectorLatitude,SectorLongitude,VerticalBeamwidth,AntennaDownTilt)
						VALUES(@SectorCode,@tmpNetworkModeId,@tmpScopeId,@tmpBandId,@tmpCarrierId,@Antenna,@Beamwidth,@Azimuth,@PCI,@SiteID,@BandWidth,@CellId,@RFHeight,@MTilt,@ETilt,@ColorHexCode2,@MRBTS,@SiteLatitude,@SiteLongitude,@VerticalBeamwidth,@MTilt);
					END
					ELSE
					BEGIN
						
						IF @curSiteStatusId=90
						BEGIN
							--SELECT 'Sector: '+ CAST(@tmpNetworkModeId AS NVARCHAR(50)),@tmpNetworkModeId,@tmpBandId,@tmpCarrierId
							UPDATE AV_Sectors
							SET BandWidth = @BandWidth, Azimuth = @Azimuth, PCI = @PCI, SectorLatitude = @SectorLatitude, SectorLongitude = @SectorLongitude,
							CellId=@CellId,RFHeight=@RFHeight,VerticalBeamwidth=@VerticalBeamwidth,AntennaDownTilt=@MTilt
							WHERE SiteId=@SiteID AND NetworkModeId=@tmpNetworkModeId AND BandId=@tmpBandId AND CarrierId=@tmpCarrierId AND SectorCode=@SectorCode;
						END
					END
					
				IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID AND x.NetworkModeId=@tmpNetworkModeId AND x.BandId=@tmpBandId AND x.CarrierId=@tmpCarrierId AND x.IsActive=1)
					BEGIN	
						--SELECT 'Net Layer: '+ CAST(@tmpNetworkModeId AS NVARCHAR(50))									
						INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive)
						VALUES(@SiteID,@tmpNetworkModeId,@tmpScopeId,@tmpBandId,@tmpCarrierId,@ReceivedOn,GETDATE(),@SubmittedById,@WOStatusID6,CAST(1 AS BIT));
						
						DECLARE @NetLayerId as numeric(18,0)=0
						SELECT @NetLayerId=@@IDENTITY

						--SELECT @NetLayerId,@tmpNetworkModeId,@BandId
						
						insert into AV_SiteScript(SiteId,NetLayerId,RevisionId,EventTypeId,EventValue,IsValue,IsL3Enabled,Color,SequenceId,EventCommand,EventValue1,EventCommand1,SortOrder)	
						SELECT @SiteId, @NetLayerId,0,x.EventTypeId,x.EventValue,x.IsValue,IsL3Enabled,Color,SequenceId,EventCommand,EventValue1,EventCommand1,ROW_NUMBER() OVER (ORDER BY x.SrId)
						FROM AV_TestScript x
						WHERE NetworkModeId=@tmpNetworkModeId AND BandId=@tmpBandId					
					END
				END
					ELSE IF @Scope='TSS'
					BEGIN				
						
						DECLARE @tIdCount INT,@titeration INT=1,@tSurveyIdNew NUMERIC(18,0)
						--DECLARE @TempTableForLoops TABLE
						--(
						--Id INT IDENTITY(1,1),
						--SurveySplitedId nvarchar(20)
						--)
						
						SELECT ROW_NUMBER() over(order by item) 'id', Item 'SurveySplitedId' INTO #TempTableForLoops FROM SplitString(@Checklist,',')
						--INSERT INTO @TempTableForLoops 
						
					
					SELECT * from #TempTableForLoops

						SELECT @tIdCount=COUNT(*) FROM #TempTableForLoops

						SELECT @tIdCount
						
						WHILE @titeration<=@tIdCount
						BEGIN
						PRINT 'Enter For Iteration No ' + Convert(VARCHAR,@titeration) 
						SELECT @tSurveyIdNew=SurveySplitedId 
						FROM
						(
							SELECT  Id,y.SurveyId 'SurveySplitedId' FROM #TempTableForLoops x inner join TSS_SurveyDocuments y on x.SurveySplitedId=y.SurveyCode AND x.Id=@titeration
						) AS ff WHERE ff.Id=@titeration
						
						SELECT  Id,y.SurveyId 'SurveySplitedId' FROM #TempTableForLoops x inner join TSS_SurveyDocuments y on x.SurveySplitedId=y.SurveyCode AND x.Id=@titeration
						SELECT @tSurveyIdNew
						--IF @titeration=@tIdCount
						--BEGIN
						--	DELETE FROM @TempTableForLoops
						--END
						 

						INSERT INTO TSS_SiteSurvey(SiteId,SurveyId)
						VALUES(@SiteID,@tSurveyIdNew);

												
						SELECT @SiteSurveyId=@@IDENTITY;
						
						IF (@SiteSurveyId>0)
						BEGIN		
							INSERT INTO TSS_SiteSections(SiteSurveyId,SectionId,QuestionsAnswered,TotalQuestions,StatusId,IsActive,IsRepeatable,IsApplicable,PSectionId,IsInclude)
							--SELECT @SiteSurveyId, ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',isnull (ts.IsRepeatable,0),isnull (ts.IsApplicable,0),ts.PSectionId , 1
							--FROM TSS_Sections AS ts
							--WHERE ts.SurveyId=@SurveyIdNew AND ts.IsActive=1
							SELECT  @SiteSurveyId,ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId AND tq.IsActive=1),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',isnull (ts.IsRepeatable,0) 'IsRepeatable',isnull (ts.IsApplicable,0),ts.PSectionId , 1
							FROM TSS_Sections AS ts
							WHERE ts.SurveyId=@tSurveyIdNew AND psectionid=0 AND ts.IsActive=1
							UNION ALL
							SELECT  @SiteSurveyId, ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId AND tq.IsActive=1),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',isnull (ts.IsRepeatable,0) 'IsRepeatable',isnull (ts.IsApplicable,0),ts.PSectionId , 1
							FROM TSS_Sections AS ts
							WHERE ts.SurveyId=@tSurveyIdNew  AND ts.IsActive=1 AND psectionid IN
							(
								SELECT ts.sectionid
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@tSurveyIdNew AND psectionid=0 AND ts.IsRepeatable=0 AND ts.IsActive=1
							)
							
							
							UPDATE TSS_SiteSections
							SET PSiteSectionId = (SELECT tss.SiteSectionId FROM TSS_SiteSections AS tss WHERE tss.SiteSurveyId=@SiteSurveyId AND tss.SectionId=TSS_SiteSections.PSectionId AND tss.IsActive=1)
							WHERE SiteSurveyId=@SiteSurveyId AND IsActive=1
							AND SectionId IN
							(
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@tSurveyIdNew AND ts.IsActive=1 AND psectionid IN
								(
									SELECT ts.sectionid
									FROM TSS_Sections AS ts
									WHERE ts.SurveyId=@tSurveyIdNew AND ts.IsActive=1 AND psectionid=0 AND ts.IsRepeatable=0
								)
							)
							
							
							INSERT INTO TSS_SiteQuestions(SiteSurveyId,SiteSectionId,QuestionId,IsRequired,IsNoteRequired,IsImageRequired,IsBarCodeRequired,IsAnswered,IsInclude)
							SELECT @SiteSurveyId,tss.SiteSectionId,tq.QuestionId,tq.IsRequired, tq.IsNoteRequired,tq.IsImageRequired, tq.IsBarCodeRequired,CAST(0 AS BIT),1
							FROM TSS_Questions AS tq
							INNER JOIN TSS_SiteSections AS tss ON tss.SectionId=tq.SectionId
							WHERE tss.IsActive=1 AND tq.IsActive=1 AND tss.SiteSurveyId=@SiteSurveyId
							AND tss.SectionId IN
							(
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@tSurveyIdNew AND psectionid=0 AND ts.IsActive=1
								UNION ALL
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@tSurveyIdNew AND ts.IsActive=1 AND psectionid IN
								(
									SELECT ts.sectionid
									FROM TSS_Sections AS ts
									WHERE ts.SurveyId=@tSurveyIdNew AND ts.IsActive=1 AND psectionid=0 AND ts.IsRepeatable=0
								)
							)							
				
							IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID)
							BEGIN
								INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,BandWidth,CellId,RFHeight,MTilt,ETilt,SectorColor,MRBTS,SectorLatitude,SectorLongitude,VerticalBeamwidth)
								VALUES('Alpha',@clsNetworkModeId,@tmpScopeId,@clsBandId,@clsCarrierId,'',0,0,0,@SiteID,0,0,0,0,0,'#ffffff',0, @SiteLatitude, @SiteLongitude,0);
							END
						
							IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID and x.SiteSurveyId = @SiteSurveyId)
							BEGIN							
								INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive,SiteSurveyId)
								VALUES(@SiteID,@clsNetworkModeId,@tmpScopeId,@clsBandId,@clsCarrierId,@ReceivedOn,GETDATE(),@SubmittedById,90,CAST(1 AS BIT),@SiteSurveyId);						
							END
						END
						
					set @titeration=@titeration+1
					
				END
						
					DROP TABLE #TempTableForLoops
					SET @titeration=1;
					SET @tIdCount=0;
					------
					END
				END		
			END
			
		FETCH NEXT FROM db_upload_wo INTO  @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band,
		@Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI, @ReceivedOn,@SiteType,@CellId,@RFHeight,@MTilt,@VerticalBeamwidth, @Project, @SiteType, @SiteClass, @Checklist,@SiteName,@SiteAddress
		END   

		CLOSE db_upload_wo   
		DEALLOCATE db_upload_wo
		
		IF @tmpAlreadyExists!='Already Existed Site Codes: '
		BEGIN
			RAISERROR(@tmpAlreadyExists,16,1);
		END
		ELSE
		BEGIN
			RETURN 1
		END
		
END

ELSE IF @Filter = 'InsertSiteCount'
BEGIN
UPDATE AV_SiteClusters 
SET SiteCount = @SiteCount
WHERE CellFilePath=@CellPath;
END
--------------------
ELSE IF @Filter='Edit_Work_Order'
BEGIN
	
	SELECT * FROM @Workorder
		
		DECLARE db_cluster CURSOR FOR  
		SELECT x.ReceivedOn, x.ClusterCode, x.Region, x.Market, x.Client, x.SiteCode, x.SiteLatitude, x.SiteLongitude, x.Description, x.SectorCode, x.NetworkMode, x.Scope, x.Band, x.Carrier, x.Antenna, x.Beamwidth, x.Azimuth, x.PCI,
		x.BandWidth, x.CellId, x.RFHeight, x.MTilt, x.ETilt, x.SiteName, x.SiteTypeId, x.SiteClassId, x.SiteAddress, x.MRBTS, x.SectorLatitude, x.SectorLongitude, x.ClusterName, x.CellFilePath, ISNULL(x.SurveyId,0), ISNULL(x.ProjectId,0),
		x.SiteSurveyId, x.SectorId, x.SiteClusterId, x.VerticalBeamwidth,x.SiteId
		FROM @Workorder x
		ORDER BY x.NetworkMode,x.Band,x.Carrier,x.Scope,
		CASE WHEN x.SectorCode='Alpha' THEN 1
			 WHEN x.SectorCode='Beta' THEN 2
			 WHEN x.SectorCode='Gamma' THEN 3
			 WHEN x.SectorCode='Delta' THEN 4
			 WHEN x.SectorCode='Epsilon' THEN 5
			 WHEN x.SectorCode='DiGamma' THEN 6
			 WHEN x.SectorCode='Iota' THEN 7
			 WHEN x.SectorCode='Eta' THEN 8
			 WHEN x.SectorCode='Theta' THEN 9
		END
		--ORDER BY x.Scope,x.networkMode,x.Band,x.Carrier,x.Azimuth
		OPEN db_cluster   
		FETCH NEXT FROM db_cluster INTO @ReceivedOn, @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band, @Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI,
		@BandWidth, @CellId, @RFHeight, @MTilt, @ETilt, @SiteName, @SiteTypeId, @SiteClassId, @SiteAddress, @MRBTS, @SectorLatitude, @SectorLongitude, @ClusterName, @CellFilePath, @SurveyId, @ProjectId,
		@DocumentSurveyId, @SectorId, @CLSClusterId, @VerticalBeamwidth, @SiteID
		WHILE @@FETCH_STATUS = 0   
		BEGIN   
			SELECT @SiteID,@SectorId,@SiteLatitude,@SiteLongitude,@SectorLongitude,@SectorLatitude
			IF @SiteID>0
			BEGIN
				UPDATE AV_Sites
				SET					
					--SiteCode = /*{ SiteCode }*/,
					Latitude = @SiteLatitude ,
					Longitude = @SiteLongitude,				
					[Description] = @Description,	
					--CityId = @Market,				
					SiteName = @SiteName,
					SiteTypeId = @SiteTypeId,
					SiteClassId = @SiteClassId,					
					SiteAddress = @SiteAddress					
				WHERE SiteId=@SiteId;
				
				IF @SectorId>0
				BEGIN
					UPDATE AV_Sectors
					SET						
						--NetworkModeId = @NetworkModeId,
						--ScopeId = @ScopeId,
						--BandId = @BandId,
						--CarrierId = @CarrierId,
						
						
						AntennaDownTilt=@MTilt,
						Antenna = @Antenna,
						BeamWidth = @Beamwidth,
						Azimuth = @Azimuth,
						PCI = @PCI,
						RFHeight = @RFHeight,
						MTilt = @MTilt,
						ETilt = @ETilt,
						BandWidth = @BandWidth,						
						CellId = @CellId,
						MRBTS = @MRBTS,						
						SectorLatitude = @SectorLatitude,
						SectorLongitude = @SectorLongitude,
						VerticalBeamwidth = @VerticalBeamwidth
						--SiteClusterId = /*{ SiteClusterId }*/
					WHERE SectorId=@SectorId;
				END
			END		

			DECLARE @tmpScopeEdit AS NVARCHAR(10)=(SELECT ad.DefinationName FROM AD_Definations AS ad WHERE ad.DefinationId=@Scope AND isactive=1)
		 IF @tmpScopeEdit='TSS'
		 BEGIN
			SELECT @SiteID,@SurveyId
			DECLARE @EIdCount INT,@Eiteration INT=1,@ESurveyIdNew NUMERIC(18,0)
						DECLARE @ETempTableForLoop TABLE
						(
						Id INT IDENTITY(1,1),
						SurveySplitedId NUMERIC(18,0)
						)
						INSERT INTO @ETempTableForLoop SELECT Item FROM SplitString(@SurveyId,',')
						
						SELECT * FROM @ETempTableForLoop

						--================== DELETE SURVEY IF REMOVED IN EDIT SECREEN =================-- 

					-- DELETING Questions

						DELETE  FROM TSS_SiteQuestions Where SiteSectionId in 
						(
							SELECT SiteSectionId FROM TSS_SiteSections Where  SiteSurveyId in 
							(
							 SELECT SiteSurveyId FROM TSS_SiteSurvey WHERE SiteId=@SiteId and SiteSurveyId not in (SELECT SurveySplitedId FROM @ETempTableForLoop)
							 )
						)

						--DELETING SECTIONS 

						DELETE FROM TSS_SiteSections Where SiteSurveyId  in 
						(
						 SELECT SiteSurveyId FROM TSS_SiteSurvey where SiteId=@SiteId and SiteSurveyId not in  (SELECT SurveySplitedId FROM @ETempTableForLoop)
						 )
						-- DELETING NET LAYER REFERENCE

						DELETE FROM AV_NetLayerStatus where SiteId=@SiteID and SiteSurveyId not in (SELECT SurveySplitedId FROM @ETempTableForLoop)

						-- DELETITNG SITESURVEY

						DELETE FROM TSS_SiteSurvey WHERE SiteId=@SiteID and SiteSurveyId not in (SELECT SurveySplitedId FROM @ETempTableForLoop)


					-- =========================================================================== --

						SELECT @EIdCount=COUNT(*) FROM @ETempTableForLoop

						WHILE @Eiteration<=@EIdCount
						BEGIN
						PRINT 'Enter For Iteration No ' + Convert(VARCHAR,@iteration) 
						SELECT @ESurveyIdNew=SurveySplitedId FROM (
						SELECT  Id,SurveySplitedId FROM @ETempTableForLoop
						) AS ff WHERE ff.Id=@Eiteration
						

						 
						IF NOT EXISTS (SELECT * FROM TSS_SiteSurvey WHERE SiteId=@SiteID and SiteSurveyId=@ESurveyIdNew)
						BEGIN
						INSERT INTO TSS_SiteSurvey(SiteId,SurveyId)
						VALUES(@SiteID,@ESurveyIdNew);

												
						SELECT @SiteSurveyId=@@IDENTITY;
						
						IF (@SiteSurveyId>0)
						BEGIN		
							INSERT INTO TSS_SiteSections(SiteSurveyId,SectionId,QuestionsAnswered,TotalQuestions,StatusId,IsActive,IsRepeatable,IsApplicable,PSectionId,IsInclude)
							--SELECT @SiteSurveyId, ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',isnull (ts.IsRepeatable,0),isnull (ts.IsApplicable,0),ts.PSectionId , 1
							--FROM TSS_Sections AS ts
							--WHERE ts.SurveyId=@SurveyIdNew AND ts.IsActive=1
							SELECT  @SiteSurveyId,ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId AND tq.IsActive=1),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',isnull (ts.IsRepeatable,0) 'IsRepeatable',isnull (ts.IsApplicable,0),ts.PSectionId , 1
							FROM TSS_Sections AS ts
							WHERE ts.SurveyId=@ESurveyIdNew AND psectionid=0 AND ts.IsActive=1
							UNION ALL
							SELECT  @SiteSurveyId, ts.SectionId,0,ISNULL((SELECT COUNT(tq.QuestionId) FROM TSS_Questions AS tq WHERE tq.SectionId=ts.SectionId AND tq.IsActive=1),0) 'TotalQuestions',90 'StatusId',CAST(1 AS BIT) 'IsActive',isnull (ts.IsRepeatable,0) 'IsRepeatable',isnull (ts.IsApplicable,0),ts.PSectionId , 1
							FROM TSS_Sections AS ts
							WHERE ts.SurveyId=@ESurveyIdNew  AND ts.IsActive=1 AND psectionid IN
							(
								SELECT ts.sectionid
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@ESurveyIdNew AND psectionid=0 AND ts.IsRepeatable=0 AND ts.IsActive=1
							)
							
							
							UPDATE TSS_SiteSections
							SET PSiteSectionId = (SELECT tss.SiteSectionId FROM TSS_SiteSections AS tss WHERE tss.SiteSurveyId=@SiteSurveyId AND tss.SectionId=TSS_SiteSections.PSectionId AND tss.IsActive=1)
							WHERE SiteSurveyId=@SiteSurveyId AND IsActive=1
							AND SectionId IN
							(
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@ESurveyIdNew AND ts.IsActive=1 AND psectionid IN
								(
									SELECT ts.sectionid
									FROM TSS_Sections AS ts
									WHERE ts.SurveyId=@ESurveyIdNew AND ts.IsActive=1 AND psectionid=0 AND ts.IsRepeatable=0
								)
							)
							
							
							INSERT INTO TSS_SiteQuestions(SiteSurveyId,SiteSectionId,QuestionId,IsRequired,IsNoteRequired,IsImageRequired,IsBarCodeRequired,IsAnswered,IsInclude)
							SELECT @SiteSurveyId,tss.SiteSectionId,tq.QuestionId,tq.IsRequired, tq.IsNoteRequired,tq.IsImageRequired, tq.IsBarCodeRequired,CAST(0 AS BIT),1
							FROM TSS_Questions AS tq
							INNER JOIN TSS_SiteSections AS tss ON tss.SectionId=tq.SectionId
							WHERE tss.IsActive=1 AND tq.IsActive=1 AND tss.SiteSurveyId=@SiteSurveyId
							AND tss.SectionId IN
							(
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@ESurveyIdNew AND psectionid=0 AND ts.IsActive=1
								UNION ALL
								SELECT ts.SectionId
								FROM TSS_Sections AS ts
								WHERE ts.SurveyId=@ESurveyIdNew AND ts.IsActive=1 AND psectionid IN
								(
									SELECT ts.sectionid
									FROM TSS_Sections AS ts
									WHERE ts.SurveyId=@ESurveyIdNew AND ts.IsActive=1 AND psectionid=0 AND ts.IsRepeatable=0
								)
							)							
						
							IF NOT EXISTS(SELECT as1.SectorId FROM AV_Sectors AS as1 WHERE as1.SiteId=@SiteID)
							BEGIN
								INSERT INTO AV_Sectors(SectorCode,NetworkModeId,ScopeId,BandId,CarrierId,Antenna,BeamWidth,Azimuth,PCI,SiteId,BandWidth,CellId,RFHeight,MTilt,ETilt,SectorColor,MRBTS,SectorLatitude,SectorLongitude,VerticalBeamwidth)
								VALUES('Alpha',@clsNetworkModeId,@Scope,@clsBandId,@clsCarrierId,'',0,0,0,@SiteID,0,0,0,0,0,'#ffffff',0, @SiteLatitude, @SiteLongitude,0);
							END
						
							IF NOT EXISTS (SELECT x.SiteId FROM AV_NetLayerStatus x WHERE x.SiteId=@SiteID and x.SiteSurveyId = @SiteSurveyId)
							BEGIN							
								INSERT INTO AV_NetLayerStatus(SiteId,NetworkModeId,ScopeId,BandId,CarrierId,ReceivedOn,UploadedOn,UploadedById,[Status],IsActive,SiteSurveyId)
								VALUES(@SiteID,@clsNetworkModeId,@Scope,@clsBandId,@clsCarrierId,@ReceivedOn,GETDATE(),@SubmittedById,90,CAST(1 AS BIT),@SiteSurveyId);						
							END
						END
						END
						
							set @Eiteration=@Eiteration+1
						END
					
		 END
			   
		FETCH NEXT FROM db_cluster INTO @ReceivedOn, @ClusterCode, @Region, @Market, @Client, @SiteCode, @SiteLatitude, @SiteLongitude, @Description, @SectorCode, @NetworkMode, @Scope, @Band, @Carrier, @Antenna, @Beamwidth, @Azimuth, @PCI,
		@BandWidth, @CellId, @RFHeight, @MTilt, @ETilt, @SiteName, @SiteTypeId, @SiteClassId, @SiteAddress, @MRBTS, @SectorLatitude, @SectorLongitude, @ClusterName, @CellFilePath, @SurveyId, @ProjectId,
		@DocumentSurveyId, @SectorId, @CLSClusterId, @VerticalBeamwidth, @SiteID
		END   

		CLOSE db_cluster   
		DEALLOCATE db_cluster
	
	
END
--------------------
ELSE
	BEGIN
		RAISERROR('Site Edit not Allowed!',16,1)
	END
END