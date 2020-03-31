-- for view
--	 declare @siteId as int=52

--select * from AV_Clusters
--WHERE ClusterId=(select x.ClusterId from AV_Sites x where x.SiteId=@siteId)

--select * from AV_Sites
--where SiteId=@siteId

--select * from AV_Sectors
--where SiteId=@siteId

--select * from AV_SiteConfigurations
--where SiteId=@siteId

--select * from AV_SiteTestSummary
--where SiteId=@siteId

--select * from AV_SiteTestLog
--where SiteId=@siteId

-- [dbo].[AV_ManageSites] 'delete','10090'
CREATE PROCEDURE AV_ManageSites 
	-- Add the parameters for the stored procedure here
	@Filter varchar(max),
	@SiteId varchar(50) =null,
	@SiteCode varchar(max)  =null,
	@Latitude float =null,
	@Longitude float =null,
	@ClusterId numeric(18,0) =null,
	@ClientId varchar(max) =null,
	@Description varchar(max) =null,
	@Status varchar(max) =null,
	@SubmittedById numeric(18,0) =null,
	@Market nvarchar(50) =NULL,
	@ReceivedOn DATETIME =NULL,
	@Scope NVARCHAR(50)=NULL
	,@List List READONLY 
	
	
	
AS
begin
	if	@Filter='insert' 
	 BEGIN
	 		DECLARE @ScopeId AS INT=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationName=@Scope)

			DECLARE @maxWoID as int=0
			DECLARE @Client AS INT=(select AD_Clients.ClientId from AD_Clients Where AD_Clients.ClientName =@ClientId)
			SET @maxWoID=(Select ISNULL(MAX(WoCode),0) + 1 from AV_Sites Where ClientId=@Client AND (YEAR(AV_Sites.SubmittedOn)* 100 + MONTH(AV_Sites.SubmittedOn) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))
			DECLARE @woRefID as nvarchar(25)
			SET @woRefID = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientName =@ClientId)+'-' + @Scope + '-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
			RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@maxWoID as nvarchar(15)),5))
			SET @Status=(SELECT x.DefinationId FROM AD_Definations x WHERE x.KeyCode=@Status)
			DECLARE @RETURN_VALUE numeric(18,0) = 0 
			
			
			SELECT @ScopeId, @woRefID, @maxWoID,@Scope
			--IF EXISTS(select site.SiteCode, sec.NetworkModeId, sec.BandId, sec.CarrierId from AV_Sites site
			--			Inner Join AV_Sectors sec On sec.SiteId = site.SiteId  Where site.SiteCode = @SiteCode)
			IF EXISTS(select site.SiteCode from AV_Sites site WHERE site.SiteCode = @SiteCode)
			BEGIN
				SELECT @RETURN_VALUE = SiteId from AV_Sites WHERE SiteCode = @SiteCode
				RETURN @RETURN_VALUE;
				
				--RAISERROR('Work Order Already Exists!',16,1)
			END
			ELSE
			BEGIN
			INSERT INTO AV_Sites(WoCode, SiteCode,Latitude,Longitude, ClusterId, ClientId, SubmittedById, Description, Status,WoRefId,CityId,ReceivedOn,RevisionId,ScopeId)
			 VALUES(@maxWoID, RTRIM(LTRIM(@SiteCode)),@Latitude,@Longitude,@ClusterId,(select ClientId from AD_Clients Where AD_Clients.ClientName =@ClientId), @SubmittedById,@Description, @Status,@woRefID,(Select DefinationId from AD_Definations Where DefinationName = @Market),@ReceivedOn,0,@ScopeId)

			SELECT @RETURN_VALUE = SCOPE_IDENTITY()
			RETURN @RETURN_VALUE;
			end
     end
	
	 
	 
		else if	@Filter='delete' 
		 begin
			delete from AV_SiteConfigurations
			where SiteId=@SiteId

			delete from AV_SiteTestSummary
			where SiteId=@SiteId

			delete from AV_SiteTestLog
			where SiteId=@SiteId

			delete from AV_Sectors
			where SiteId=@siteId

			delete from AV_Sites
			where SiteId=@SiteId
		 END
		 
		 else if	@Filter='ManageStatus'
	begin
		--Value1 'SiteId',Value2 'StatusId',Value3 'Date'
		--select 0--*  from @List
		
		---------------------Carrier Plot Start
		DECLARE @tmpLayerId AS NVARCHAR(15)=''
		DECLARE @tmpStatusId AS NVARCHAR(15)=''	
		DECLARE @tmpDate AS NVARCHAR(15)=''
			
		DECLARE db_cluster2 CURSOR FOR  
		SELECT Value1,Value2,Value3 FROM @List AS cl		
		OPEN db_cluster2 
		FETCH NEXT FROM db_cluster2 INTO @tmpLayerId,@tmpStatusId,@tmpDate
		WHILE @@FETCH_STATUS = 0   
		BEGIN  
			DECLARE @tmpStatus AS NVARCHAR(70)=''
			SELECT @tmpStatus=ad.KeyCode FROM AD_Definations AS ad WHERE ad.DefinationId=@tmpStatusId;
			
			SELECT * FROM AD_Definations AS adt
			WHERE adt.DefinationTypeId=17 		
			
			--IF @tmpStatus='PENDING_SCHEDULED'	
			--BEGIN
			--	UPDATE AV_NetLayerStatus
			--	SET [Status] = @tmpStatusId, SubmittedOn = CAST(@tmpDate AS DATETIME)
			--	WHERE LayerStatusId=@tmpLayerId
			--END			
			--ELSE 
			IF @tmpStatus='SCHEDULED'	
			BEGIN
				UPDATE AV_NetLayerStatus
				SET [Status] = @tmpStatusId, ScheduledOn = CAST(@tmpDate AS DATETIME), AssignedOn = CAST(@tmpDate AS DATETIME)
				WHERE LayerStatusId=@tmpLayerId
			END
			--ELSE IF @tmpStatus='IN_PROGRESS'	
			--BEGIN
			--	UPDATE AV_NetLayerStatus
			--	SET [Status] = @tmpStatusId, SubmittedOn = CAST(@tmpDate AS DATETIME)
			--	WHERE LayerStatusId=@tmpLayerId
			--END
			ELSE IF @tmpStatus='DRIVE_COMPLETED'	
			BEGIN
				UPDATE AV_NetLayerStatus
				SET [Status] = @tmpStatusId, DriveCompletedOn = CAST(@tmpDate AS DATETIME)
				WHERE LayerStatusId=@tmpLayerId
			END
			ELSE IF @tmpStatus='REPORT_SUBMITTED'	
			BEGIN
				UPDATE AV_NetLayerStatus
				SET [Status] = @tmpStatusId, SubmittedOn = CAST(@tmpDate AS DATETIME)
				WHERE LayerStatusId=@tmpLayerId
			END
		FETCH NEXT FROM db_cluster2 INTO @tmpLayerId,@tmpStatusId,@tmpDate
		END   
		CLOSE db_cluster2   
		DEALLOCATE db_cluster2
		
		----------------------Carrier Plot End	
	end 
END