-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PM_ManageProjectSites
	@FILTER NVARCHAR(50),
	--@List List READONLY,
	@PrjSites [dbo].[ProjectSites] READONLY,
		
	@UserId numeric(18,0)=NULL,
	@AlarmId numeric(18,0)=NULL,
	@ProjectSiteId numeric(18,0)=NULL,
	@StatusId numeric(18,0)=NULL,
	@MSWindowId numeric(18,0)=NULL,
	@CreatedBy numeric(18,0)=NULL,
	@Notes NVARCHAR(50)=NULL,
	@ExtendedeNB NVARCHAR(250)=NULL,
	@EquipmentId NVARCHAR(250)=NULL,
	@AOTSCR NVARCHAR(250)=NULL,
	@FilePath NVARCHAR(500)=NULL,
	@ENB NVARCHAR(250)=NULL,
	@ItemTypeId NUMERIC=NULL,
	@ActivityTypeId NUMERIC=NULL,
	@GngId NUMERIC=NULL,
	@IsAddionalSite bit = 0,
	@IsActive bit = 0

	--@Value1 NVARCHAR(50)=NULL,
	--@Value2 NVARCHAR(50)=NULL,
	--@Value3 NVARCHAR(50)=NULL,
	--@Value4 NVARCHAR(50)=NULL,
	--@Value5 NVARCHAR(50)=NULL,
	--@Value6 NVARCHAR(50)=NULL,
	--@Value7 NVARCHAR(50)=NULL,
	--@Value8 NVARCHAR(50)=NULL,
	--@Value9 NVARCHAR(50)=NULL,
	--@Value10 NVARCHAR(50)=NULL,
	--@Value11 NVARCHAR(50)=NULL,
	--@Value12 NVARCHAR(50)=NULL,
	--@Value13 NVARCHAR(50)=NULL,
	--@Value14 NVARCHAR(50)=NULL,
	--@Value15 NVARCHAR(50)=NULL,
	--@Value16 nvarchar(50)=NULL,
	--@Value17 nvarchar(50)=NULL
AS



DECLARE @ProjectId NVARCHAR(50)=NULL
DECLARE @SiteCode NVARCHAR(50)=NULL
DECLARE @ReceivedOn NVARCHAR(50)=NULL
DECLARE @SiteTypeId NVARCHAR(50)=NULL
DECLARE @SiteClassId NVARCHAR(50)=NULL
DECLARE @ClusterCode NVARCHAR(50)=NULL
DECLARE @CityId NVARCHAR(50)=NULL
DECLARE @ColorId NVARCHAR(50)=NULL
DECLARE @CreatedOn NVARCHAR(50)=NULL
--DECLARE @CreatedBy NVARCHAR(50)=NULL
DECLARE @Latitude NVARCHAR(50)=null
DECLARE @Longitude NVARCHAR(50)=null
DECLARE @Description NVARCHAR(50)=null
DECLARE @SiteName NVARCHAR(50)=null
DECLARE @ClientId NVARCHAR(50)=NULL
DECLARE @Address NVARCHAR(250)=NULL

DECLARE @USID AS NVARCHAR(50)=NULL
DECLARE @SubMarket AS NVARCHAR(50)=NULL
DECLARE @VMME AS NVARCHAR(50)
DECLARE @ControlledIntro AS NVARCHAR(50)
DECLARE @SuperBowl AS NVARCHAR(50)
DECLARE @isDASInBuild AS NVARCHAR(50)
DECLARE @FirstNetRAN AS NVARCHAR(50)
DECLARE @IPlanJob AS NVARCHAR(50)
DECLARE @PaceNo AS NVARCHAR(50)
DECLARE @IPlanIssueDate AS NVARCHAR(50)
DECLARE @FACode AS NVARCHAR(50)
DECLARE @ScopeId AS NVARCHAR(50)



BEGIN	
	IF @Filter='Insert'	
	BEGIN
		/*
		* @Value1: ProjectId, @Value2: SiteCode, @Value3: ReceivedOn, @Value4: SiteTypeId, @Value5: ClusterCode, @Value6: CityId, @Value7: ColorId, @Value9: CreatedBy
		* @Value11: Latitude, @Value12: Longitude, @Value13: Description, @Value15: ClientId, @Value14: SiteName
		*/		
		--SELECT * FROM @PrjSites
		
		--DECLARE THE CURSOR FOR A QUERY.
		DECLARE addSite CURSOR READ_ONLY
		FOR
		--SELECT l.Value1,l.Value2,l.Value4,l.Value5,l.Value6,l.Value7,l.Value9,l.Value11,l.Value12,l.Value13,l.Value15,l.Value14 FROM @List l
		SELECT x.ProjectId, x.SiteCode, x.SiteTypeId, x.ClusterCode, x.CityId, x.ColorId, x.CreatedBy, x.Latitude, x.Longitude, x.Description, x.ClientId, x.SiteName,
		x.USID, x.SubMarket, x.VMME, x.ControlledIntro, x.SuperBowl, x.isDASInBuild, x.FirstNetRAN, x.IPlanJob, x.PaceNo, x.IPlanIssueDate,x.FACode,x.MarketId, x.ScopeId
		FROM @PrjSites x
		--OPEN CURSOR
		OPEN addSite 
		--FETCH THE RECORD INTO THE VARIABLES.
		FETCH NEXT FROM addSite INTO @ProjectId, @SiteCode,  @SiteTypeId, @ClusterCode, @CityId, @ColorId, @CreatedBy,@Latitude, @Longitude, @Description, @ClientId, @SiteName,
		@USID, @SubMarket, @VMME, @ControlledIntro, @SuperBowl, @isDASInBuild, @FirstNetRAN, @IPlanJob, @PaceNo, @IPlanIssueDate,@FACode,@CityId, @ScopeId
		--LOOP UNTIL RECORDS ARE AVAILABLE.
		WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION				 
			
			DECLARE @maxWoID as int=0
			DECLARE @woRefID as nvarchar(25)
			
			SELECT @ClientId=pp.ClientId FROM PM_Projects AS pp WHERE pp.ProjectId=@ProjectId
				
			SET @maxWoID=(Select ISNULL(MAX(PMCode),0) + 1 from PM_ProjectSites x Where ClientId=@ClientId AND (YEAR(x.CreatedOn)* 100 + MONTH(x.CreatedOn) = (YEAR(GETDATE())* 100 + MONTH(GETDATE()))))					
			SET @woRefID = ((select ClientPrefix from AD_Clients Where AD_Clients.ClientId=@ClientId)+ '-'+RIGHT('00'+CAST(MONTH(GETDATE()) as nvarchar(2)),2)+
			RIGHT('00'+CAST(YEAR(GETDATE()) as nvarchar(4)),2)+'-'+RIGHT('00000'+CAST(@maxWoID as nvarchar(15)),5))
					
			INSERT INTO [dbo].[PM_ProjectSites]([ScopeId],[ProjectId],[SiteDate],[SiteCode],[ReceivedOn],[SiteTypeId],[ClusterCode],[CityId],[MarketId],[ColorId],[CreatedOn],[CreatedBy],[IsActive],[Latitude],[Longitude],
			[Description],[ClientId],SiteName,PMRefId,PMCode,RevisionId,USID,
			SubMarket, vMME, ControlledIntro, SuperBowl, isDASInBuild, FirstNetRAN,
			IPlanJob, PaceNo, IPlanIssueDate,FACode, [StatusId], PriorityId)
			VALUES(@ScopeId, @ProjectId, GETDATE() ,@SiteCode, GETDATE(), @SiteTypeId, @ClusterCode, @CityId, @CityId, @ColorId,GETDATE(), @UserId, CAST(1 AS BIT), @Latitude, @Longitude,
			@Description, @ClientId, @SiteName,@woRefID,@maxWoID,0,@USID, @SubMarket, CAST(@VMME AS BIT), CAST(@ControlledIntro AS BIT), CAST(@SuperBowl AS BIT), @isDASInBuild,
			@FirstNetRAN, @IPlanJob, @PaceNo, (CASE WHEN @IPlanIssueDate IS NOT NULL AND @IPlanIssueDate!='' THEN CAST(@IPlanIssueDate AS DATETIME) ELSE NULL END),@SiteCode, (select top 1  DefinationId from AD_Definations where keycode='PROJECT SITE STATUS' and DefinationName='Active'),  (select top 1  DefinationId from AD_Definations where keycode='ISSUE_PRIORITY' and DefinationName='High'))
			
			--DECLARE @ProjectSiteId AS NUMERIC(18,0)=0
			SELECT @ProjectSiteId=@@IDENTITY;
			
			
			
			INSERT INTO PM_SiteTasks(ProjectSiteId,TaskId,RevisionId,StatusId,PriorityId,EstimatedStartDate,EstimatedEndDate,PlannedDate,TargetDate,ActualStartDate,ActualEndDate,
			CompletionPercent,BudgetCost,ActualCost,IsActive,PTaskId)
			SELECT @ProjectSiteId,pt.TaskId,0,pt.StatusId, pt.PriorityId,
			       NULL, NULL, NULL,
			      NULL, NULL, NULL,0,pt.BudgetCost,0,pt.IsActive,pt.PTaskId
			FROM PM_Tasks AS pt
			WHERE pt.ProjectId=@ProjectId AND pt.IsActive=1;
			COMMIT
			--FETCH THE NEXT RECORD INTO THE VARIABLES.
			FETCH NEXT FROM addSite INTO  @ProjectId, @SiteCode,  @SiteTypeId, @ClusterCode, @CityId, @ColorId, @CreatedBy,@Latitude, @Longitude, @Description, @ClientId, @SiteName,
		@USID, @SubMarket, @VMME, @ControlledIntro, @SuperBowl, @isDASInBuild, @FirstNetRAN, @IPlanJob, @PaceNo, @IPlanIssueDate,@FACode,@CityId, @ScopeId
		END 
		--CLOSE THE CURSOR.
		CLOSE addSite
		DEALLOCATE addSite
	END
	IF @Filter='IsActive'
	BEGIN
		UPDATE PM_ProjectSites
		SET IsActive=(CASE WHEN  IsActive = 0 THEN 1 ELSE 0 END)
		WHERE ProjectSiteId=@ProjectSiteId;		
	END
	IF @Filter='Update_MSWindowId'
	BEGIN	
		--TODO: Change Filter Name to "UPDATE_SITE_STATUS"
		UPDATE PM_ProjectSites
		SET StatusId = @StatusId, MSWindowId=@MSWindowId, AlarmId = @AlarmId
		WHERE ProjectSiteId=@ProjectSiteId
		
		SET @UserId=@CreatedBy

		INSERT INTO PM_SiteLog(ProjectSiteId,StatusId,MSWindowId,AlarmId,DESCRIPTION,UserID,CreatedOn,ActivityTypeId,GngId,ItemTypeId,ItemFilePath)
		SELECT @ProjectSiteId, @StatusId, @MSWindowId, @AlarmId, @Notes, @UserId, GETDATE(), @ActivityTypeId, @GngId, @ItemTypeId, @FilePath		
	END
	if @FILTER='Update'
	BEGIN
		/*ProjectId=@Value2,SiteCode=@Value3,ReceivedOn=@Value4,SiteTypeId=@Value5,CityId=@Value7,ClusterCode=@Value6,ColorId=@Value8,SiteName=@Value10,SiteClassId=@Value11,
		  Latitude=@Value12,Longitude=@Value13,[Description]=@Value14,Address=@Value15,ClientId=@Value16*/
				
		--DECLARE THE CURSOR FOR A QUERY.
		DECLARE PM_ProjectSite CURSOR READ_ONLY
		FOR
		--SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14,l.Value15 FROM @List AS l 
		SELECT x.ProjectId, x.SiteCode, x.SiteTypeId, x.ClusterCode, x.CityId, x.ColorId, x.CreatedBy, x.Latitude, x.Longitude, x.Description, x.ClientId, x.SiteName,
		x.USID, x.SubMarket, x.VMME, x.ControlledIntro, x.SuperBowl, x.isDASInBuild, x.FirstNetRAN, x.IPlanJob, x.PaceNo, x.IPlanIssueDate, x.ScopeId
		FROM @PrjSites x
		--OPEN CURSOR.
		OPEN PM_ProjectSite 
		--FETCH THE RECORD INTO THE VARIABLES.
		FETCH NEXT FROM PM_ProjectSite INTO @ProjectId, @SiteCode,  @SiteTypeId, @ClusterCode, @CityId, @ColorId, @CreatedBy,@Latitude, @Longitude, @Description, @ClientId, @SiteName,
		@USID, @SubMarket, @VMME, @ControlledIntro, @SuperBowl, @isDASInBuild, @FirstNetRAN, @IPlanJob, @PaceNo, @IPlanIssueDate, @ScopeId
		--LOOP UNTIL RECORDS ARE AVAILABLE.
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF CAST(@ProjectSiteId AS numeric(18,0))=0 
			BEGIN
				print 'not implemented'
			END 
			ELSE 
			BEGIN
				UPDATE PM_ProjectSites
				SET SiteTypeId = @SiteTypeId, ModifyOn = GETDATE(), ModifyBy = @UserId, ScopeId = @ScopeId,
			    ClusterCode = @ClusterCode, CityId = @CityId, ColorId = @ColorId, SiteCode = @SiteCode,FACode = @SiteCode,
			    MarketId=@CityId, Latitude = @Latitude, Longitude = @Longitude, [Description] = @Description, SiteName = @SiteName,
			    USID=@USID, SubMarket = @SubMarket, vMME = CAST(@VMME AS BIT), ControlledIntro = CAST(@ControlledIntro AS BIT), SuperBowl = CAST(@SuperBowl AS BIT),
			    isDASInBuild = @isDASInBuild, FirstNetRAN =  @FirstNetRAN, IPlanJob =  @IPlanJob, PaceNo =  @PaceNo, IPlanIssueDate =  @IPlanIssueDate
				WHERE ProjectSiteId=@ProjectSiteId
			END
			--FETCH THE NEXT RECORD INTO THE VARIABLES.
			FETCH NEXT FROM PM_ProjectSite INTO @ProjectId, @SiteCode,  @SiteTypeId, @ClusterCode, @CityId, @ColorId, @CreatedBy,@Latitude, @Longitude, @Description, @ClientId, @SiteName,
			@USID, @SubMarket, @VMME, @ControlledIntro, @SuperBowl, @isDASInBuild, @FirstNetRAN, @IPlanJob, @PaceNo, @IPlanIssueDate, @ScopeId
		END
 
		--CLOSE THE CURSOR.
		CLOSE PM_ProjectSite
		DEALLOCATE PM_ProjectSite

END
		
		
	IF @Filter='SetIsActive'
	BEGIN
		UPDATE PM_ProjectSites SET IsActive=@IsActive
		WHERE ProjectSiteId = @ProjectSiteId AND StatusId != 133317
		--UPDATE P SET P.IsActive=@IsActive
		--From PM_ProjectSites P
	 --   JOIN dbo.AD_Definations def on def.DefinationId = P.StatusId
		--WHERE P.ProjectSiteId = @ProjectSiteId AND def.DefinationName NOT IN ('Active')
	END			
END