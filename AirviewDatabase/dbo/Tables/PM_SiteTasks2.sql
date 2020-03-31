CREATE TABLE [dbo].[PM_SiteTasks2] (
    [SiteTaskId]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ProjectSiteId]      NUMERIC (18) NULL,
    [TaskId]             NUMERIC (18) NULL,
    [PTaskId]            NUMERIC (18) NULL,
    [RevisionId]         INT          CONSTRAINT [DF_PM_SiteTasks_RevisionId_040FAE1C] DEFAULT ((0)) NULL,
    [StatusId]           NUMERIC (18) NULL,
    [PriorityId]         NUMERIC (18) NULL,
    [EstimatedStartDate] DATETIME     NULL,
    [EstimatedEndDate]   DATETIME     NULL,
    [PlannedDate]        DATETIME     NULL,
    [TargetDate]         DATETIME     NULL,
    [ActualStartDate]    DATETIME     NULL,
    [ActualEndDate]      DATETIME     NULL,
    [CompletionPercent]  FLOAT (53)   NULL,
    [BudgetCost]         FLOAT (53)   NULL,
    [ActualCost]         FLOAT (53)   NULL,
    [IsActive]           BIT          NULL,
    CONSTRAINT [PK_PM_SiteTasks_040FAEA4] PRIMARY KEY CLUSTERED ([SiteTaskId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_PM_SiteTasks_1]
    ON [dbo].[PM_SiteTasks2]([ProjectSiteId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PM_SiteTasks]
    ON [dbo].[PM_SiteTasks2]([TaskId] ASC);


GO

CREATE TRIGGER [dbo].[PM_UpdateProjectTrend] ON [dbo].[PM_SiteTasks2]
AFTER UPDATE
AS
BEGIN
	DECLARE @ProjectId AS NUMERIC(18,0)=20021
	DECLARE @ProjectSiteId AS NUMERIC(18,0)=0
	DECLARE @SiteTaskId AS NUMERIC(18,0)=0
	DECLARE @NewPlanDate AS DATETIME=NULL
	DECLARE @NewForecastDate AS DATETIME=NULL
	DECLARE @NewTargetDate AS DATETIME=NULL
	DECLARE @NewActualDate AS DATETIME=NULL

	DECLARE @RegionId AS NUMERIC(18,0)=0
	DECLARE @MarketId AS NUMERIC(18,0)=0
	DECLARE @MilestoneId AS NUMERIC(18,0)=0
	DECLARE @StageId AS NUMERIC(18,0)=0

	DECLARE @PrvPlanDate AS DATETIME=NULL
	DECLARE @PrvForecastDate AS DATETIME=NULL
	DECLARE @PrvTargetDate AS DATETIME=NULL
	DECLARE @PrvActualDate AS DATETIME=NULL

	SELECT @ProjectSiteId=INSERTED.ProjectSiteId, @SiteTaskId=INSERTED.SiteTaskId, @NewPlanDate=INSERTED.PlannedDate, @NewForecastDate=INSERTED.EstimatedStartDate,
	@NewTargetDate=INSERTED.TargetDate, @NewActualDate=INSERTED.ActualEndDate
	FROM INSERTED;

	SELECT @PrvPlanDate=DELETED.PlannedDate, @PrvForecastDate=DELETED.EstimatedStartDate,
	@PrvTargetDate=DELETED.TargetDate, @PrvActualDate=DELETED.ActualEndDate
	FROM DELETED;


	SELECT @RegionId=stt.PDefinationId, @MarketId=sit.CityId, @MilestoneId=(CASE WHEN ISNULL(stk.PTaskId,0)=0 THEN stk.TaskId ELSE stk.PTaskId END),
	@StageId=(CASE WHEN ISNULL(stk.PTaskId,0)=0 THEN 0 ELSE stk.TaskId END)
	--,@PrvPlanDate=stk.PlannedDate, @PrvForecastDate=stk.EstimatedStartDate, @PrvTargetDate=stk.TargetDate, @PrvActualDate=stk.ActualEndDate 
	FROM PM_SiteTasks stk
	INNER JOIN PM_ProjectSites sit ON sit.ProjectSiteId=stk.ProjectSiteId
	INNER JOIN AD_Definations AS cty ON cty.DefinationId=sit.CityId
	INNER JOIN AD_Definations AS stt ON stt.DefinationId=cty.PDefinationId
	WHERE stk.ProjectSiteId=@ProjectSiteId AND stk.SiteTaskId=@SiteTaskId

	DECLARE @IsPlanExists AS BIT=0
	DECLARE @IsForecastExists AS BIT=0
	DECLARE @IsTargetExists AS BIT=0
	DECLARE @IsActualExists AS BIT=0

	--SELECT @RegionId,@marketid,@MilestoneId,@StageId,@NewForecastDate,@PrvForecastDate


	--Project Plan Update
IF @NewPlanDate IS NOT NULL 
BEGIN
IF @PrvPlanDate!=@NewPlanDate
BEGIN
	UPDATE PM_ProjectTrend
	SET PlannedSites = (CASE WHEN ISNULL(PlannedSites,0)>0 THEN ISNULL(PlannedSites,0)-1 ELSE 0 END)
	WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@PrvPlanDate;

	UPDATE PM_ProjectTrend
	SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvPlanDate),0),
		CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvPlanDate),0),
		CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvPlanDate),0),
		CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvPlanDate),0)
	WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@PrvPlanDate;
END	

	IF ISNULL((SELECT COUNT(ppt.ReportDate)
				FROM PM_ProjectTrend AS ppt
				WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate=@NewPlanDate),0)>0
	BEGIN
		UPDATE PM_ProjectTrend
		SET PlannedSites = ISNULL(PlannedSites,0)+1
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@NewPlanDate;
	
		UPDATE PM_ProjectTrend
		SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewPlanDate),0),
			CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewPlanDate),0),
			CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewPlanDate),0),
			CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewPlanDate),0)
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@NewPlanDate;
	END
	ELSE
	BEGIN
		INSERT INTO PM_ProjectTrend(ProjectId,MilestoneId,StageId,RegionId,MarketId,ReportDate,PlannedSites,ForecastSites,TargetSites,ActualSites,CumPlannedSites,CumForecastSites,
		CumTargetSites,CumActualSites,RequiredRunRate,ScheduledSites,PlannedStatusSites,OngoingStatusSites,MigratedStatusSites,CompletedStatusSites)
		SELECT @ProjectId,@MilestoneId,@StageId,@RegionId,@MarketId,@NewPlanDate,1,0,0,0,0,0,0,0,0,0,0,0,0,0
	
		UPDATE PM_ProjectTrend
		SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewPlanDate),0),
			CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewPlanDate),0),
			CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewPlanDate),0),
			CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewPlanDate),0)
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@NewPlanDate;
	END
END
	--Project Forecast Update	
IF @NewForecastDate IS NOT NULL 
BEGIN
IF @PrvForecastDate!=@NewForecastDate
BEGIN
	UPDATE PM_ProjectTrend
	SET ForecastSites = (CASE WHEN ISNULL(ForecastSites,0)>0 THEN ISNULL(ForecastSites,0)-1 ELSE 0 END)
	WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@PrvForecastDate;

	UPDATE PM_ProjectTrend
	SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvForecastDate),0),
		CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvForecastDate),0),
		CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvForecastDate),0),
		CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvForecastDate),0)
	WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@PrvForecastDate;
END
	IF ISNULL((SELECT COUNT(ppt.ReportDate)
				FROM PM_ProjectTrend AS ppt
				WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate=@NewForecastDate),0)>0
	BEGIN
		UPDATE PM_ProjectTrend
		SET ForecastSites = ISNULL(ForecastSites,0)+1
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@NewForecastDate;
	
		UPDATE PM_ProjectTrend
		SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewForecastDate),0),
			CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewForecastDate),0),
			CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewForecastDate),0),
			CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewForecastDate),0)
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@NewForecastDate;
	END
	ELSE
	BEGIN
		INSERT INTO PM_ProjectTrend(ProjectId,MilestoneId,StageId,RegionId,MarketId,ReportDate,PlannedSites,ForecastSites,TargetSites,ActualSites,CumPlannedSites,CumForecastSites,
		CumTargetSites,CumActualSites,RequiredRunRate,ScheduledSites,PlannedStatusSites,OngoingStatusSites,MigratedStatusSites,CompletedStatusSites)
		SELECT @ProjectId,@MilestoneId,@StageId,@RegionId,@MarketId,@NewForecastDate,0,1,0,0,0,0,0,0,0,0,0,0,0,0
	
		UPDATE PM_ProjectTrend
		SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewForecastDate),0),
			CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewForecastDate),0),
			CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewForecastDate),0),
			CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewForecastDate),0)
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@NewForecastDate;
	END
END
	--Project Target Update
IF @NewTargetDate IS NOT NULL 
BEGIN
IF @PrvTargetDate!=@NewTargetDate
BEGIN
	UPDATE PM_ProjectTrend
	SET TargetSites = (CASE WHEN ISNULL(TargetSites,0)>0 THEN ISNULL(TargetSites,0)-1 ELSE 0 END)
	WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@PrvTargetDate;

	UPDATE PM_ProjectTrend
	SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvTargetDate),0),
		CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvTargetDate),0),
		CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvTargetDate),0),
		CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvTargetDate),0)
	WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@PrvTargetDate;
END
	IF ISNULL((SELECT COUNT(ppt.ReportDate)
				FROM PM_ProjectTrend AS ppt
				WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate=@NewTargetDate),0)>0
	BEGIN
		UPDATE PM_ProjectTrend
		SET TargetSites = ISNULL(TargetSites,0)+1
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND StageId=@StageId AND ReportDate=@NewTargetDate;
	
		UPDATE PM_ProjectTrend
		SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewTargetDate),0),
			CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewTargetDate),0),
			CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewTargetDate),0),
			CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewTargetDate),0)
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@NewTargetDate;
	END
	ELSE
	BEGIN
		INSERT INTO PM_ProjectTrend(ProjectId,MilestoneId,StageId,RegionId,MarketId,ReportDate,PlannedSites,ForecastSites,TargetSites,ActualSites,CumPlannedSites,CumForecastSites,
		CumTargetSites,CumActualSites,RequiredRunRate,ScheduledSites,PlannedStatusSites,OngoingStatusSites,MigratedStatusSites,CompletedStatusSites)
		SELECT @ProjectId,@MilestoneId,@StageId,@RegionId,@MarketId,@NewTargetDate,0,0,1,0,0,0,0,0,0,0,0,0,0,0
	
		UPDATE PM_ProjectTrend
		SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewTargetDate),0),
			CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewTargetDate),0),
			CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewTargetDate),0),
			CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewTargetDate),0)
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@NewTargetDate;
	END
END
	--Project Actual Update
IF @NewActualDate IS NOT NULL 
BEGIN
IF @PrvActualDate!=@NewActualDate
BEGIN
	UPDATE PM_ProjectTrend
	SET ActualSites = (CASE WHEN ISNULL(ActualSites,0)>0 THEN ISNULL(ActualSites,0)-1 ELSE 0 END)
	WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@PrvActualDate;

	UPDATE PM_ProjectTrend
	SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvActualDate),0),
		CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvActualDate),0),
		CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvActualDate),0),
		CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@PrvActualDate),0)
	WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@PrvActualDate;
END
	IF ISNULL((SELECT COUNT(ppt.ReportDate)
				FROM PM_ProjectTrend AS ppt
				WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate=@NewActualDate),0)>0
	BEGIN
		UPDATE PM_ProjectTrend
		SET ActualSites = ISNULL(ActualSites,0)+1
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@NewActualDate;
	
		UPDATE PM_ProjectTrend
		SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewActualDate),0),
			CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewActualDate),0),
			CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewActualDate),0),
			CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewActualDate),0)
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@NewActualDate;
	END
	ELSE
	BEGIN
		INSERT INTO PM_ProjectTrend(ProjectId,MilestoneId,StageId,RegionId,MarketId,ReportDate,PlannedSites,ForecastSites,TargetSites,ActualSites,CumPlannedSites,CumForecastSites,
		CumTargetSites,CumActualSites,RequiredRunRate,ScheduledSites,PlannedStatusSites,OngoingStatusSites,MigratedStatusSites,CompletedStatusSites)
		SELECT @ProjectId,@MilestoneId,@StageId,@RegionId,@MarketId,@NewActualDate,0,0,0,1,0,0,0,0,0,0,0,0,0,0
	
		UPDATE PM_ProjectTrend
		SET CumPlannedSites = ISNULL((SELECT SUM(ppt.PlannedSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewActualDate),0),
			CumForecastSites = ISNULL((SELECT SUM(ppt.ForecastSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewActualDate),0),
			CumTargetSites = ISNULL((SELECT SUM(ppt.TargetSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewActualDate),0),
			CumActualSites = ISNULL((SELECT SUM(ppt.ActualSites) FROM PM_ProjectTrend AS ppt WHERE ppt.ProjectId=@ProjectId AND ppt.RegionId=@RegionId AND ppt.MarketId=@MarketId AND ppt.MilestoneId=@MilestoneId AND ISNULL(ppt.StageId,0)=@StageId AND ppt.ReportDate<=@NewActualDate),0)
		WHERE ProjectId=@ProjectId AND RegionId=@RegionId AND MarketId=@MarketId AND MilestoneId=@MilestoneId AND ISNULL(StageId,0)=@StageId AND ReportDate=@NewActualDate;
	END
END
END
GO
DISABLE TRIGGER [dbo].[PM_UpdateProjectTrend]
    ON [dbo].[PM_SiteTasks2];

