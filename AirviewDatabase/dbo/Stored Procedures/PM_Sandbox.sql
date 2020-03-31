CREATE PROCEDURE PM_Sandbox
	@Filter NVARCHAR(50),
	@ProjectId AS NUMERIC(18,0),
	@FromDate AS Date=NULL,
	@ToDate AS Date=NULL,
	@Tasks AS NVARCHAR(500)='',
	@Markets AS NVARCHAR(500)='',	
	@XDataSeries NVARCHAR(50),
	@YDataSeries NVARCHAR(50),
	@DataValue FLOAT=0,
	@Selected nvarchar(500)='',
	@Unselected nvarchar(500)='',
	@Task as numeric(18,0)=0
AS
	SET @Tasks+=','
	SET @Markets+=','
	
	IF @Filter = 'Get_Sites'
	BEGIN
		SELECT st.ProjectId,st.ProjectSiteId,st.FACode,st.CommonId 'eNB'--,rgn.DefinationName 'Region',city.DefinationName 'Market',st.SubMarket,
		--sts.DefinationName 'Status', sts.ColorCode
		, CAST(1 AS BIT) 'IsSelected'
		from PM_ProjectSites as st 
		INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
	--	INNER JOIN AD_Definations city on city.DefinationId= st.CityId
	--	INNER JOIN AD_Definations rgn on rgn.DefinationId= city.PDefinationId
	--	INNER JOIN AD_Definations sts on sts.DefinationId = st.StatusId
		WHERE  st.IsActive=1
		AND pst.EstimatedStartDate=CAST(@xDataSeries as datetime)-- BETWEEN @FromDate AND @ToDate
		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
		AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
		AND st.ProjectId=@ProjectId
		UNION ALL
		SELECT st.ProjectId,st.ProjectSiteId,st.FACode,st.CommonId 'eNB'--,rgn.DefinationName 'Region',city.DefinationName 'Market',st.SubMarket,
		--sts.DefinationName 'Status', sts.ColorCode
		, CAST(0 AS BIT) 'IsSelected'
		from PM_ProjectSites as st 
		INNER JOIN PM_SiteTasks AS pst ON pst.ProjectSiteId=st.ProjectSiteId
		--INNER JOIN AD_Definations city on city.DefinationId= st.CityId
		--INNER JOIN AD_Definations rgn on rgn.DefinationId= city.PDefinationId
		--INNER JOIN AD_Definations sts on sts.DefinationId = st.StatusId
		WHERE  st.IsActive=1
		AND pst.EstimatedStartDate BETWEEN @FromDate AND @ToDate
		AND Charindex(cast(st.CityId as varchar(max))+',', @Markets) > 0
		AND Charindex(cast(pst.TaskId as varchar(max))+',', @Tasks) > 0
		AND st.ProjectId=@ProjectId
		--Order By SiteDate DESC
	END
	else if @Filter = 'Save_Sites'
	BEGIN
	if @YDataSeries='Actual' 
	begin
	 if @Unselected <> ''
	 begin
	 EXECUTE('update PM_SiteTasks  set ActualEndDate=null where   ProjectSiteId in ('+@Unselected+') and TaskId='+@Task )
	 end
	  if @Selected <> ''
	 begin
	 EXECUTE('update PM_SiteTasks  set ActualEndDate='+@XDataSeries+' where   ProjectSiteId in ('+@Selected+') and TaskId='+@Task )
	 end
	 end
	else if @YDataSeries='Forecast' 
	begin
	 if @Unselected <> ''
	 begin
	 EXECUTE('update PM_SiteTasks  set EstimatedStartDate=null where   ProjectSiteId in ('+@Unselected+') and TaskId='+@Task )
	 end
	  if @Selected <> ''
	 begin
	 EXECUTE('update PM_SiteTasks  set EstimatedStartDate='+@XDataSeries+' where   ProjectSiteId in ('+@Selected+') and TaskId='+@Task )
	 end
	 end
	 else if @YDataSeries='Target' 
	begin
	 if @Unselected <> ''
	 begin
	 EXECUTE('update PM_SiteTasks  set TargetDate=null where   ProjectSiteId in ('+@Unselected+') and TaskId='+@Task )
	 end
	  if @Selected <> ''
	 begin
	 EXECUTE('update PM_SiteTasks  set TargetDate='+@XDataSeries+' where   ProjectSiteId in ('+@Selected+') and TaskId='+@Task )
	 end
	 end
	END