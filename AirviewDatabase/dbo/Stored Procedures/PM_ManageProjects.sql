-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PM_ManageProjects
 @Filter NVARCHAR(50)=''
,@ProjectId NUMERIC(18,0)=0
,@ProjectName NVARCHAR(50)=''
,@ScopeId NVARCHAR(50)=0
,@ClientId NUMERIC(18,0)=0
,@EndClientId NUMERIC(18,0)=0
,@ActualStartDate DATETIME=NULL
,@ActualEndDate DATETIME=NULL
,@StatusId NUMERIC(18,0)=0
,@Color NVARCHAR(50)=NULL
,@Description NVARCHAR(500)=''
,@IsActive BIT=NULL
,@PriorityId NUMERIC(18,0)=0
,@IsEstimate BIT=NULL
,@BudgetCost FLOAT=NULL
,@TargetDate DATETIME=NULL
,@TaskTypeId NUMERIC(18,0)=NULL
,@CompletionPercent FLOAT=NULL
,@IsWoLinked BIT=0
,@PlannedDate DATETIME=NULL
,@WorkingDays NVARCHAR(50)=NULL
,@ProjectManagerId NUMERIC(18,0)=0
,@MID NUMERIC(18,0)=0,
@PID NUMERIC(18,0)=0,
@EntityId numeric(18, 0) = 1,
@CurrencyId numeric(18, 0) = 0,
@CategoryId numeric(18, 0) = 0,
@IsWorkflowAllowed bit = 0
AS
BEGIN
		DECLARE @tProjectId AS INT=0
		DECLARE @tClientId AS INT=0
		DECLARE @tEndClientId AS INT=0
		DECLARE @tClient AS NVARCHAR(150)=''
		DECLARE @tEndClient AS NVARCHAR(150)=''
		
	IF @Filter='Insert'
	BEGIN
		IF NOT EXISTS(SELECT pm.ProjectId FROM PM_Projects AS pm WHERE pm.ProjectName=@ProjectName AND pm.ClientId=@ClientId AND pm.EndClientId=@EndClientId)
		BEGIN
			INSERT INTO PM_Projects (ProjectName,ClientId ,EndClientId, EstimateStartDate, EstimateEndDate ,ActualStartDate, ActualEndDate ,StatusId ,Color ,[Description] ,IsActive,PriorityId,IsEstimate,BudgetCost,TargetDate,TaskTypeId,CompletionPercent,IsWoLinked,PlannedDate,WorkingDays,ManagerId, EntityId, IsWorkflowAllowed,CategoryId,CurrencyId)
			VALUEs(@ProjectName,@ClientId, @EndClientId, @ActualStartDate, @ActualEndDate,@ActualStartDate, @ActualEndDate,@StatusId, @Color, @Description, @IsActive,@PriorityId,@IsEstimate,@BudgetCost,@TargetDate,@TaskTypeId,@CompletionPercent,@IsWoLinked,@PlannedDate,@WorkingDays,@ProjectManagerId, @EntityId, @IsWorkflowAllowed,@CategoryId,@CurrencyId)
		END		
		
		SET @tProjectId =(SELECT @@IDENTITY)

		SELECT @tClientId=pp.ClientId, @tClient=ac.ClientName FROM PM_Projects AS pp INNER JOIN AD_Clients AS ac ON pp.ClientId=ac.ClientId WHERE pp.ProjectId=@tProjectId
		SELECT @tEndClientId=pp.ClientId, @tEndClient=ac.ClientName FROM PM_Projects AS pp INNER JOIN AD_Clients AS ac ON pp.EndClientId=ac.ClientId WHERE pp.ProjectId=@tProjectId
		
		
		
		IF @tClientId!=@tEndClientId
		BEGIN				
			INSERT INTO PM_ChartSettings
			(
				ProjectId,
				PanelName,
				ChartName,
				ChartType,
				DataSeries,
				ColorCode,
				SeriesType,
				IsActive
			)
			SELECT DISTINCT x.*
			FROM
			(
				SELECT  @tProjectId 'ProjectId', pcs.PanelName, pcs.ChartName, pcs.ChartType,
					   CASE WHEN pcs.IsClient=1 AND pcs.IsUnavoidable=0 THEN @tClient 
							WHEN pcs.IsClient=1 AND pcs.IsUnavoidable=1 THEN @tClient+ ' Un-Avoidable'
							WHEN pcs.IsEndClient=1 AND pcs.IsUnavoidable=0 THEN @tEndClient 
							WHEN pcs.IsEndClient=1 AND pcs.IsUnavoidable=1 THEN @tEndClient+ ' Un-Avoidable' 
					   ELSE pcs.DataSeries END 'DataSeries',
					   pcs.ColorCode, pcs.SeriesType, pcs.IsActive
				  FROM PM_ChartSettingsTemplate AS pcs
			) x
		END
		ELSE
		BEGIN
			INSERT INTO PM_ChartSettings
			(
				ProjectId,
				PanelName,
				ChartName,
				ChartType,
				DataSeries,
				ColorCode,
				SeriesType,
				IsActive
			)
			SELECT DISTINCT x.*
			FROM
			(
				SELECT  @tProjectId 'ProjectId', pcs.PanelName, pcs.ChartName, pcs.ChartType,
					   CASE WHEN pcs.IsClient=1 AND pcs.IsUnavoidable=0 THEN @tClient 
							WHEN pcs.IsClient=1 AND pcs.IsUnavoidable=1 THEN @tClient+ ' Un-Avoidable'							 
					   ELSE pcs.DataSeries END 'DataSeries',
					   pcs.ColorCode, pcs.SeriesType, pcs.IsActive
				  FROM PM_ChartSettingsTemplate AS pcs
				  WHERE (pcs.IsEndClient IS NULL OR pcs.IsEndClient<>1) AND (pcs.IsClient=1 OR pcs.IsClient IS NULL)
			) x
		END
		
	    select @tProjectId;
	END	
	else IF @Filter='Update'
	BEGIN
		SET @tProjectId=@ProjectId;
		
		DELETE FROM PM_ChartSettings
		WHERE ProjectId=@ProjectId
		
		SELECT @tClientId=pp.ClientId, @tClient=ac.ClientName FROM PM_Projects AS pp INNER JOIN AD_Clients AS ac ON pp.ClientId=ac.ClientId WHERE pp.ProjectId=@tProjectId
		SELECT @tEndClientId=pp.ClientId, @tEndClient=ac.ClientName FROM PM_Projects AS pp INNER JOIN AD_Clients AS ac ON pp.EndClientId=ac.ClientId WHERE pp.ProjectId=@tProjectId

		IF @tClient!=@tEndClient
		BEGIN		
			INSERT INTO PM_ChartSettings
			(
				ProjectId,
				PanelName,
				ChartName,
				ChartType,
				DataSeries,
				ColorCode,
				SeriesType,
				IsActive
			)
			SELECT DISTINCT x.*
			FROM
			(
				SELECT  @ProjectId 'ProjectId', pcs.PanelName, pcs.ChartName, pcs.ChartType,
					   CASE WHEN pcs.IsClient=1 AND pcs.IsUnavoidable=0 THEN @tClient 
							WHEN pcs.IsClient=1 AND pcs.IsUnavoidable=1 THEN @tClient+ ' Un-Avoidable'
							WHEN pcs.IsEndClient=1 AND pcs.IsUnavoidable=0 THEN @tEndClient 
							WHEN pcs.IsEndClient=1 AND pcs.IsUnavoidable=1 THEN @tEndClient+ ' Un-Avoidable' 
					   ELSE pcs.DataSeries END 'DataSeries',
					   pcs.ColorCode, pcs.SeriesType, pcs.IsActive
				  FROM PM_ChartSettingsTemplate AS pcs
			) x
		END
		ELSE
		BEGIN
			INSERT INTO PM_ChartSettings
			(
				ProjectId,
				PanelName,
				ChartName,
				ChartType,
				DataSeries,
				ColorCode,
				SeriesType,
				IsActive
			)
			SELECT DISTINCT x.*
			FROM
			(
				SELECT  @ProjectId 'ProjectId', pcs.PanelName, pcs.ChartName, pcs.ChartType,
					   CASE WHEN pcs.IsClient=1 AND pcs.IsUnavoidable=0 THEN @tClient 
							WHEN pcs.IsClient=1 AND pcs.IsUnavoidable=1 THEN @tClient+ ' Un-Avoidable'							 
					   ELSE pcs.DataSeries END 'DataSeries',
					   pcs.ColorCode, pcs.SeriesType, pcs.IsActive
				  FROM PM_ChartSettingsTemplate AS pcs
				  WHERE (pcs.IsEndClient IS NULL OR pcs.IsEndClient<>1) AND (pcs.IsClient=1 OR pcs.IsClient IS NULL)
			) x
		END
		
		UPDATE PM_Projects
		SET    ProjectName       = @ProjectName,
		       ClientId          = @ClientId,
		       EndClientId       = @EndClientId,
		       StatusId          = @StatusId,
		       Color             = @Color,
		       [DESCRIPTION]     = @Description,
		       IsActive          = @IsActive,
		       PriorityId        = @PriorityId,
		       IsEstimate        = @IsEstimate,
		       BudgetCost        = @BudgetCost,		       
		       TaskTypeId        = @TaskTypeId,		       
			   EstimateStartDate = @ActualStartDate,
		       EstimateEndDate = @ActualEndDate,
		       ActualStartDate = @ActualStartDate,
		       ActualEndDate = @ActualEndDate
		       ,TargetDate=@TargetDate
		       ,IsWoLinked = @IsWoLinked
		       ,CompletionPercent=@CompletionPercent
		       ,PlannedDate=@PlannedDate
			   ,WorkingDays=@WorkingDays
			   ,ManagerId = @ProjectManagerId		 
			   ,EntityId = @EntityId
			   ,IsWorkflowAllowed = @IsWorkflowAllowed  
			   ,CategoryId=@CategoryId
			   ,CurrencyId=@CurrencyId   
		WHERE  ProjectId         = @ProjectId
	END	
	else IF @Filter='InsertUserPermissions'
	BEGIN
		--Insert into Sec_UserProjects (UserId, ProjectId) values (@MID,@PID)
		IF @PID>0
		BEGIN	
			IF NOT EXISTS(SELECT UserId FROM Sec_UserProjects WHERE ProjectId=@PID AND UserId=@MID)
			BEGIN
				Insert into Sec_UserProjects (UserId, ProjectId) values (@MID,@PID)
			END
		END
	END
	else IF @Filter='UpdateUserPermissions'
	BEGIN
	--delete Sec_UserProjects where ProjectId = @PID
	
		IF @PID>0
		BEGIN	
			IF NOT EXISTS(SELECT UserId FROM Sec_UserProjects WHERE ProjectId=@PID AND UserId=@MID)
			BEGIN
				Insert into Sec_UserProjects (UserId, ProjectId) values (@MID,@PID)
			END
		END	
	END		
END