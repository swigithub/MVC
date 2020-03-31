-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE PM_GetProjectSites
 @FILTER NVARCHAR(50)
,@List List READONLY,
@UserId nvarchar(1000)='',
@value1 nvarchar(1000)='',
@value2 nvarchar(1000)='',
@value3 nvarchar(1000)='',
@value4 nvarchar(1000)='',
@value5 nvarchar(1000)='',
@value6 nvarchar(1000)='',
@value7 nvarchar(1000)='',
@ExtendedeNB NVARCHAR(250)=NULL,
@EquipmentId NVARCHAR(250)=NULL,
@AOTSCR NVARCHAR(250)=NULL,
@FilePath NVARCHAR(500)=NULL,
@ENB NVARCHAR(250)=NULL,
@ItemTypeId NUMERIC=NULL,
@ActivityTypeId NUMERIC=NULL,
@Offset AS INT=0,
@Page AS INT=5,
@ProjectId int=0,
@ProjectSiteId int=0,
@SiteCode NVARCHAR(250)=NULL,
@IsActive bit=1

AS

BEGIN
	
	IF @Filter='Get_All'
	BEGIN
	 select TOP 1 *,sc.DefinationName as 'SiteClass',st.DefinationName AS 'SiteType',pr.ProjectName AS 'Project',ci.DefinationName AS 'City',cl.ClientName AS 'Client' from PM_ProjectSites ps
	 left join AD_Definations sc on sc.DefinationId=ps.SiteClassId 
	 left join AD_Definations st on st.DefinationId=ps.SiteTypeId
	 left join PM_Projects pr on pr.ProjectId=ps.ProjectId
	 left join AD_Definations ci on ci.DefinationId=ps.CityId
	 left join AD_Clients cl on cl.ClientId=ps.ClientId
	 Where ps.IsActive = 1

	END
	IF @Filter='Get_All_By_ProjectId'
	
	BEGIN
	 select *,rg.DefinationId 'Region',rg.PDefinationId 'State',ps.CityId 'City'  from PM_ProjectSites ps
	  left join AD_Definations ct on ct.DefinationId=ps.CityId
	  left join AD_Definations rg on rg.DefinationId=ct.PDefinationId
	  where ProjectId=@value1
	END
	IF @Filter='Get_By_Id'
	
	BEGIN
	 select *,rg.DefinationId 'Region',rg.PDefinationId 'State',ps.CityId 'City'  from PM_ProjectSites ps
	  left join AD_Definations ct on ct.DefinationId=ps.CityId
	  left join AD_Definations rg on rg.DefinationId=ct.PDefinationId
	  where ProjectSiteId=@value1
	END

	else IF @FILTER = 'Paging'
	BEGIN		
		IF @value2 = '-1'
		BEGIN
				SELECT	ps.ProjectSiteId,ps.PMRefId,rg.DefinationName 'Region', ci.DefinationName AS 'City', ps.SubMarket, ps.FACode, ps.ScopeId, ps.StatusId,
				ps.SiteDate, ps.ClusterCode, st.DefinationName AS 'SiteType', ps.USID, ps.CommonId, ps.vMME, ps.ControlledIntro,
				ps.SuperBowl, ps.isDASInBuild, ps.FirstNetRAN, ps.PaceNo, ps.IPlanJob,ps.IPlanIssueDate, sts.DefinationName 'Status', ps.IsActive
				from PM_ProjectSites ps
				 inner JOIN AD_Definations st on st.DefinationId=ps.SiteTypeId
				 INNER JOIN AD_Definations ci on ci.DefinationId=ps.CityId
				 LEFT JOIN AD_Definations ss on ss.DefinationId=ps.ScopeId
				 INNER JOIN AD_Definations rg on rg.DefinationId=ci.PDefinationId
				 inner JOIN AD_Definations sts on sts.DefinationId=ps.StatusId
				where (ps.ProjectId=@ProjectId AND ps.IsActive=@IsActive) and (ps.FACode like '%'+@Value3+'%' or ps.SiteName like '%'+@Value3+'%' or ss.DefinationName like '%'+@Value3+'%' or rg.DefinationName like '%'+@Value3+'%' or ci.DefinationName like '%'+@Value3+'%')
				Order By ps.ProjectSiteId DESC
		 END ELSE
		 BEGIN
				SELECT	ps.ProjectSiteId,ps.PMRefId,rg.DefinationName 'Region', ci.DefinationName AS 'City', ps.SubMarket, ps.FACode, ps.ScopeId, ps.StatusId,
				ps.SiteDate, ps.ClusterCode, st.DefinationName AS 'SiteType', ps.USID, ps.CommonId, ps.vMME, ps.ControlledIntro,
				ps.SuperBowl, ps.isDASInBuild, ps.FirstNetRAN, ps.PaceNo, ps.IPlanJob,ps.IPlanIssueDate, sts.DefinationName 'Status', ps.IsActive
				from PM_ProjectSites ps
				 inner JOIN AD_Definations st on st.DefinationId=ps.SiteTypeId
				 INNER JOIN AD_Definations ci on ci.DefinationId=ps.CityId
				 LEFT JOIN AD_Definations ss on ss.DefinationId=ps.ScopeId
				 INNER JOIN AD_Definations rg on rg.DefinationId=ci.PDefinationId
				 inner JOIN AD_Definations sts on sts.DefinationId=ps.StatusId
				where (ps.ProjectId=@ProjectId AND ps.IsActive=@IsActive) and (ps.FACode like '%'+@Value3+'%' or ss.DefinationName like '%'+@Value3+'%' or ps.SiteName like '%'+@Value3+'%' or rg.DefinationName like '%'+@Value3+'%' or ci.DefinationName like '%'+@Value3+'%')
				Order By ps.ProjectSiteId DESC OFFSET @offset ROWS FETCH NEXT (CASE WHEN @Value2 IS NULL THEN @Page ELSE @Value2 END) ROWS ONLY 
		END
			    

		 SELECT count(1) 'TotalRecord'
		 from PM_ProjectSites ps
		-- INNER JOIN AD_Definations st on st.DefinationId=ps.SiteTypeId
		 INNER JOIN AD_Definations ci on ci.DefinationId=ps.CityId
		 LEFT JOIN AD_Definations ss on ss.DefinationId=ps.ScopeId
		 INNER JOIN AD_Definations rg on rg.DefinationId=ci.PDefinationId
		 inner JOIN AD_Definations sts on sts.DefinationId=ps.StatusId
		where (ps.ProjectId=@ProjectId AND ps.IsActive=@IsActive) and (ss.DefinationName like '%'+@Value3+'%' or rg.DefinationName like '%'+@Value3+'%' or ci.DefinationName like '%'+@Value3+'%')
	END
	ELSE IF @Filter='GET_SiteLog'
	BEGIN
	select slog.ActivityTypeId,slog.AlarmId,slog.Description,slog.CreatedOn,slog.IsAdditional,slog.ItemFilePath,slog.ItemTypeId,slog.ItemTypeId,slog.MSWindowId,slog.ProjectSiteId,slog.StatusId,slog.UserId,slog.GngId as 'GNGId' ,def.DefinationName 'Status', users.FirstName +' '+users.LastName 'UserName',users.Picture from PM_SiteLog as slog
	inner join AD_Definations def on  slog.StatusId = def.DefinationId
	inner join dbo.PM_ProjectSites ps 
	on ps.ProjectSiteId = slog.ProjectSiteId
	left join Sec_Users users on slog.UserId = users.UserId
	where slog.ProjectSiteId = @value1 AND ps.IsActive = 1
	order by slog.SiteLogId desc
	END	
	ELSE IF @Filter='IsSiteCodeExistInProject'
	BEGIN
		select COUNT(*) from PM_ProjectSites sites 
		Where sites.SiteCode = @SiteCode AND sites.ProjectId = @ProjectId AND sites.ProjectSiteId != @ProjectSiteId
	END		
	ELSE IF @Filter='IsSiteCodeExistInProjectForNew'
	BEGIN
		select COUNT(*) from PM_ProjectSites sites 
		Where sites.SiteCode = @SiteCode AND sites.ProjectId = @ProjectId
	END		
	ELSE IF @Filter = 'ProjectFACodeList'
	BEGIN
		SELECT FACode FROM PM_ProjectSites PM WHERE PM.ProjectId = @ProjectId
	END
	
	ELSE IF @Filter = 'ProjectSitesByProjectId'
	BEGIN
		SELECT SiteName, ProjectSiteId, * FROM PM_ProjectSites PM WHERE PM.ProjectId = @ProjectId ORDER BY PM.ProjectSiteId DESC
	END
	ELSE IF @Filter = 'ProjectSiteTasks'
	BEGIN
		SELECT st.TaskId, t.Title FROM PM_SiteTasks st  
	    LEFT JOIN PM_Tasks t ON t.TaskId = st.TaskId
		WHERE st.ProjectSiteId = @ProjectSiteId
	END
		ELSE IF @Filter = 'GetEntitiesByFilters'
	BEGIN
	declare @StatusIds as varchar(2000)=@value1
	declare @TypeIds as varchar(2000)=@value2
    declare @ClientsIds as varchar(2000)=@value3
    declare @MarketsIds as varchar(2000)=@value6
		if @StatusIds =''
		begin
		Declare @IdCount01 int,@iteration01 int=1,@Id01 numeric(18,0)
		Declare @TempTableForLoop01 Table(Id int IDENTITY(1,1),WGId numeric(18,0))
		insert into @TempTableForLoop01 select d.DefinationId as 'Item' from Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='Project Status' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
		select @IdCount01=COUNT(*) from @TempTableForLoop01
			while @iteration01<=@IdCount01
				begin
						select @Id01=WGId from (
						select  Id,WGId from @TempTableForLoop01
						) as ff where ff.Id=@iteration01
                        set @StatusIds=@StatusIds+','+CONVERT(nvarchar(1000),@Id01);
						set @iteration01=@iteration01+1;
                end
				set @StatusIds=left (right (@StatusIds, len (@StatusIds)-1), len (@StatusIds)-1) ;
				end

		if @TypeIds =''
		begin
				
		Declare @IdCount1_1 int,@iteration1_1 int=1,@Id1_1 numeric(18,0)
		Declare @TempTableForLoop1_1 Table(Id int IDENTITY(1,1),WGId numeric(18,0))
		insert into @TempTableForLoop1_1 select d.DefinationId as 'Item' from Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='SiteType' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
		select @IdCount1_1=COUNT(*) from @TempTableForLoop1_1
			while @iteration1_1<=@IdCount1_1
				begin
						select @Id1_1=WGId from (
						select  Id,WGId from @TempTableForLoop1_1
						) as ff where ff.Id=@iteration1_1
                        set @TypeIds=@TypeIds+','+CONVERT(nvarchar(1000),@Id1_1);
					
						set @iteration1_1=@iteration1_1+1;
                end
				
				set @TypeIds=left (right (@TypeIds, len (@TypeIds)-1), len (@TypeIds)-1) ;
			
				end
		if @ClientsIds =''
		begin
		Declare @IdCount_21 int,@iteration_21 int=1,@Id_21 numeric(18,0)
		Declare @TempTableForLoop_21 Table(Id int IDENTITY(1,1),WGId numeric(18,0))
		insert into @TempTableForLoop_21 select c.ClientId as 'Item'from AD_Clients c where c.IsActive=1
		select @IdCount_21=COUNT(*) from @TempTableForLoop_21
			while @iteration_21<=@IdCount_21
				begin
						select @Id_21=WGId from (
						select  Id,WGId from @TempTableForLoop_21
						) as ff where ff.Id=@iteration_21
                        set @ClientsIds=@ClientsIds+','+CONVERT(nvarchar(1000),@Id_21);
						set @iteration_21=@iteration_21+1;
                end
				set @ClientsIds=left (right (@ClientsIds, len (@ClientsIds)-1), len (@ClientsIds)-1) ;
				end
		if @MarketsIds =''
		begin
		Declare @IdCount_22 int,@iteration_22 int=1,@Id_22 numeric(18,0)
		Declare @TempTableForLoop_22 Table(Id int IDENTITY(1,1),WGId numeric(18,0))
		insert into @TempTableForLoop_22 select d.DefinationId as 'Item' from Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='City' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
		select @IdCount_22=COUNT(*) from @TempTableForLoop_22
			while @iteration_22<=@IdCount_22
				begin
						select @Id_22=WGId from (
						select  Id,WGId from @TempTableForLoop_22
						) as ff where ff.Id=@iteration_22
                        set @MarketsIds=@MarketsIds+','+CONVERT(nvarchar(1000),@Id_22);
						set @iteration_22=@iteration_22+1;
                end
				set @MarketsIds=left (right (@MarketsIds, len (@MarketsIds)-1), len (@MarketsIds)-1) ;
				end

	    SET @StatusIds+=','
	    SET @TypeIds+=','
	    SET @ClientsIds+=','
		SET @MarketsIds+=','

		SELECT ps.ProjectSiteId 'EntityId',ps.SiteCode 'EntityCode',ps.SiteName 'Name',ss.DefinationName 'Status',ss.ColorCode 'StatusColor',st.DefinationName AS 'EntityCategory',ci.DefinationName AS 'Market',
		ps.SubMarket ,(select count(pst.SiteTaskId) from PM_SiteTasks pst where pst.ProjectSiteId=ps.ProjectSiteId
		 and pst.StatusId=(select  distinct top 1 DefinationId from AD_Definations ad where ad.DefinationName !='Completed' and KeyCode='PROJECT_TASK_STATUS' ) ) 'PendingTasksCount', (select  distinct top 1 Color from AD_Definations ad where ad.DefinationName !='Completed' and KeyCode='PROJECT_TASK_STATUS' )'TaskStatusPendingColor',(select  distinct top 1 Color from AD_Definations ad where ad.DefinationName ='Completed' and KeyCode='PROJECT_TASK_STATUS' )'TaskStatusCompletedColor',(select count(pst.SiteTaskId) from PM_SiteTasks pst where pst.ProjectSiteId=ps.ProjectSiteId
		 and pst.StatusId=(select  distinct top 1 DefinationId from AD_Definations ad where ad.DefinationName='Completed' and KeyCode='PROJECT_TASK_STATUS' ) )  'CompletedTasksCount',
		(select Count(*) from  PM_Issues i where i.ProjectSiteId = ps.ProjectSiteId) 'NumberOfIssues'
		FROM PM_ProjectSites AS ps
		INNER JOIN Sec_UserProjects u ON ps.ProjectId = u.ProjectId   
		left join AD_Definations ss on ss.DefinationId=ps.StatusId
	   	left join AD_Definations sc on sc.DefinationId=ps.SiteClassId 
	    left join AD_Definations st on st.DefinationId=ps.SiteTypeId
	    left join PM_Projects pr on pr.ProjectId=ps.ProjectId
	    left join AD_Definations ci on ci.DefinationId=ps.CityId
	    left join AD_Clients cl on cl.ClientId=ps.ClientId
	   	WHERE ps.ProjectId = @ProjectId AND u.UserId = CONVERT(numeric(18,0),@UserId)  and ps.CreatedOn  <= CONVERT(datetime,@value4)  AND ps.CreatedOn >= CONVERT(datetime,@value5) and ps.IsActive=1 
	    AND Charindex(cast(ps.SiteTypeId as varchar(max))+',', @TypeIds) > 0
		AND Charindex(cast(ps.StatusId as varchar(max))+',', @StatusIds) > 0
		and  Charindex(cast(ps.ClientId as varchar(max))+',', @ClientsIds) > 0
		and  Charindex(cast(ps.MarketId as varchar(max))+',', @MarketsIds) > 0
		ORDER BY ps.ProjectSiteId DESC
	END
	ELSE IF @Filter = 'GetEntitiesBySeachKey'
	BEGIN
		SELECT ps.ProjectSiteId 'EntityId',ps.SiteCode 'EntityCode',ps.SiteName 'Name',ss.DefinationName 'Status',ss.ColorCode 'StatusColor',st.DefinationName AS 'EntityCategory',ci.DefinationName AS 'Market',
		ps.SubMarket ,(select count(pst.SiteTaskId) from PM_SiteTasks pst where pst.ProjectSiteId=ps.ProjectSiteId
		 and pst.StatusId=(select  distinct top 1 DefinationId from AD_Definations ad where ad.DefinationName !='Completed' and KeyCode='PROJECT_TASK_STATUS' ) ) 'PendingTasksCount', (select  distinct top 1 Color from AD_Definations ad where ad.DefinationName !='Completed' and KeyCode='PROJECT_TASK_STATUS' )'TaskStatusPendingColor',(select  distinct top 1 Color from AD_Definations ad where ad.DefinationName ='Completed' and KeyCode='PROJECT_TASK_STATUS' )'TaskStatusCompletedColor',(select count(pst.SiteTaskId) from PM_SiteTasks pst where pst.ProjectSiteId=ps.ProjectSiteId
		 and pst.StatusId=(select  distinct top 1 DefinationId from AD_Definations ad where ad.DefinationName='Completed' and KeyCode='PROJECT_TASK_STATUS' ) )  'CompletedTasksCount',
		(select Count(*) from  PM_Issues i where i.ProjectSiteId = ps.ProjectSiteId) 'NumberOfIssues'
		FROM PM_ProjectSites AS ps
		INNER JOIN Sec_UserProjects u ON ps.ProjectId = u.ProjectId   
		left join AD_Definations ss on ss.DefinationId=ps.StatusId
	   	left join AD_Definations sc on sc.DefinationId=ps.SiteClassId 
	    left join AD_Definations st on st.DefinationId=ps.SiteTypeId
	    left join PM_Projects pr on pr.ProjectId=ps.ProjectId
	    left join AD_Definations ci on ci.DefinationId=ps.CityId
	    left join AD_Clients cl on cl.ClientId=ps.ClientId
	   	WHERE ps.ProjectId = @ProjectId AND u.UserId = @UserId and ps.IsActive=1 and ps.SiteName like '%'+@value6+'%' ORDER BY ps.ProjectSiteId DESC
	END
	ELSE IF @Filter = 'GetAllEntities'
	BEGIN
		SELECT ps.ProjectSiteId 'EntityId',ps.SiteCode 'EntityCode',ps.SiteName 'Name',ss.DefinationName 'Status',ss.ColorCode 'StatusColor',st.DefinationName AS 'EntityCategory',ci.DefinationName AS 'Market',
		ps.SubMarket ,(select count(pst.SiteTaskId) from PM_SiteTasks pst where pst.ProjectSiteId=ps.ProjectSiteId
		 and pst.StatusId=(select  distinct top 1 DefinationId from AD_Definations ad where ad.DefinationName !='Completed' and KeyCode='PROJECT_TASK_STATUS' ) ) 'PendingTasksCount', (select  distinct top 1 Color from AD_Definations ad where ad.DefinationName !='Completed' and KeyCode='PROJECT_TASK_STATUS' )'TaskStatusPendingColor',(select  distinct top 1 Color from AD_Definations ad where ad.DefinationName ='Completed' and KeyCode='PROJECT_TASK_STATUS' )'TaskStatusCompletedColor',(select count(pst.SiteTaskId) from PM_SiteTasks pst where pst.ProjectSiteId=ps.ProjectSiteId
		 and pst.StatusId=(select  distinct top 1 DefinationId from AD_Definations ad where ad.DefinationName='Completed' and KeyCode='PROJECT_TASK_STATUS' ) )  'CompletedTasksCount',
		(select Count(*) from  PM_Issues i where i.ProjectSiteId = ps.ProjectSiteId) 'NumberOfIssues'
		FROM PM_ProjectSites AS ps
		INNER JOIN Sec_UserProjects u ON ps.ProjectId = u.ProjectId   
		left join AD_Definations ss on ss.DefinationId=ps.StatusId
	   	left join AD_Definations sc on sc.DefinationId=ps.SiteClassId 
	    left join AD_Definations st on st.DefinationId=ps.SiteTypeId
	    left join PM_Projects pr on pr.ProjectId=ps.ProjectId
	    left join AD_Definations ci on ci.DefinationId=ps.CityId
	    left join AD_Clients cl on cl.ClientId=ps.ClientId
	    	WHERE ps.ProjectId = @ProjectId AND u.UserId = @UserId and ps.IsActive=1   ORDER BY ps.ProjectSiteId DESC
	END
	ELSE IF @Filter = 'EntitiesFilters'
	BEGIN
		select d.DefinationId as 'StatusId',d.DefinationName as 'StatusName' from Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='Project Status' and d.IsActive=1 and udt.UserId=CONVERT(numeric(18,0),@value7)
		
	    select d.DefinationId as 'TypeId',d.DefinationName as 'TypeName' from Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='SiteType' and d.IsActive=1 and udt.UserId=CONVERT(numeric(18,0),@value7)
		
		select * from AD_Clients where IsActive=1
	 --  select d.DefinationId as 'MarketId',d.DefinationName as 'MarketName' from Ad_DefinationTypes dt
  --      inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		--inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
  --       where dt.DefinationType ='City' and d.IsActive=1 and udt.UserId=CONVERT(numeric(18,0),@value7)
  	SELECT DISTINCT def.DefinationId as 'MarketId',def.DefinationName as 'MarketName',def.PDefinationId 'SubMarketParentId', rgn.DefinationName 'SubMarket'
			from AD_Definations def
			INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId			
			inner join Sec_UserCities uc on uc.CityId=def.DefinationId
			INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=def.PDefinationId
			INNER JOIN AD_Definations AS stt ON stt.DefinationId=rgn.PDefinationId
			where adt.DefinationType='City' and uc.UserId=11 AND def.IsActive=1
		ORDER BY def.PDefinationId,def.DefinationId
	END
END