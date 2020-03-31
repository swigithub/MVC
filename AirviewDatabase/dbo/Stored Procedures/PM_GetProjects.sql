-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--exec PM_GetProjects @Filter=N'ByStatus',@Value=N'11'
CREATE PROCEDURE [dbo].[PM_GetProjects]
@Filter NVARCHAR(50)=''
,@Value NVARCHAR(50)=NULL
,@StatusIds NVARCHAR(50)=''
,@ProritysIds NVARCHAR(50)=''
,@ToDate NVARCHAR(50)=NULL
,@FromDate NVARCHAR(50)=NULL
,@ClientsIds NVARCHAR(50)='',
@UserId NVARCHAR(50)=0	
AS
BEGIN
	
--	 [dbo].[PM_GetProjects] 'ByStatus',1
	IF @Filter='ByStatus'
	BEGIN
	--if @Value <>'True' and @Value <>'False'
	--begin
		SELECT DISTINCT p.*, cl.ClientName 'Client',ecl.ClientName 'EndClient',def.DefinationName 'Status',def.ColorCode 'StatusColor',pro.DefinationName 'Priority'
		FROM PM_Projects AS p
		INNER JOIN AD_Clients AS cl ON cl.ClientId=p.ClientId
		LEFT JOIN AD_Clients AS ecl ON ecl.ClientId=p.EndClientId
		LEFT JOIN AD_Definations AS def ON def.DefinationId=p.StatusId
		LEFT JOIN AD_Definations AS pro ON pro.DefinationId=p.PriorityId
		inner join Sec_UserProjects u on u.ProjectId= p.ProjectID
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=def.DefinationTypeId
		where u.UserId=CONVERT(numeric(20,0),@UserId) and p.IsActive=@Value
		and def.IsActive=1
    --end
	--else
	--begin
	--	SELECT p.*, cl.ClientName 'Client',ecl.ClientName 'EndClient',def.DefinationName 'Status',def.ColorCode 'StatusColor',pro.DefinationName 'Priority'
	--	FROM PM_Projects AS p
	--	INNER JOIN AD_Clients AS cl ON cl.ClientId=p.ClientId
	--	LEFT JOIN AD_Clients AS ecl ON ecl.ClientId=p.EndClientId
	--	LEFT JOIN AD_Definations AS def ON def.DefinationId=p.StatusId
	--	LEFT JOIN AD_Definations AS pro ON pro.DefinationId=p.PriorityId
	--	--inner join Sec_UserProjects u on u.ProjectId= p.ProjectID
	--	where  p.IsActive=@Value
	--end
	END
	Else IF @Filter='ProjectByKey'
	BEGIN
	--if @Value <>'True' and @Value <>'False'
	--begin
		SELECT p.*, cl.ClientName 'Client',ecl.ClientName 'EndClient',def.DefinationName 'Status',def.ColorCode 'StatusColor',pro.DefinationName 'Priority',pro.ColorCode 'PriorityColor'
		FROM PM_Projects AS p
		INNER JOIN AD_Clients AS cl ON cl.ClientId=p.ClientId
		LEFT JOIN AD_Clients AS ecl ON ecl.ClientId=p.EndClientId
		LEFT JOIN AD_Definations AS def ON def.DefinationId=p.StatusId
		LEFT JOIN AD_Definations AS pro ON pro.DefinationId=p.PriorityId
		inner join Sec_UserProjects u on u.ProjectId= p.ProjectID
		where u.UserId=CONVERT(numeric(20,0),@UserId) and p.ProjectName like '%'+@Value+'%'  
			and def.IsActive=1
	END
	
	else IF @Filter='ByProjectId'
	BEGIN
		Declare @IdCount int,@iteration int=1,@WorkGroupId numeric(18,0),@workgroups varchar(500)=''
		Declare @TempTableForLoop Table(Id int IDENTITY(1,1),WGId numeric(18,0))
		insert into @TempTableForLoop select pmg.WorkGroupId as 'Item' from PM_WorkGroup as pmg where ProjectId = @Value
		select @IdCount=COUNT(*) from @TempTableForLoop
			while @iteration<=@IdCount
				begin
						select @WorkGroupId=WGId from (
						select  Id,WGId from @TempTableForLoop
						) as ff where ff.Id=@iteration
                        set @workgroups=@workgroups+','+CONVERT(nvarchar(1000),@WorkGroupId);
						set @iteration=@iteration+1;
                end
		
				declare @iteration2 int =1,@IdCount2 int;
				Declare @groups varchar(500)='',@GroupId numeric(18,0)
					Declare @TempTableForLoop2 Table(Id int IDENTITY(1,1),WGId numeric(18,0))
					insert into @TempTableForLoop2 select DISTINCT pmg.GroupId as 'Item' from PM_TaskResources as pmg where ProjectId = @Value and IsDeleted =0
	              set @IdCount2=(select COUNT(*) from @TempTableForLoop2)
			while @iteration2<=@IdCount2
				begin
						select @GroupId=WGId from (
						select  Id,WGId from @TempTableForLoop2
						) as gg where gg.Id=@iteration2
                        set @groups=@groups+','+CONVERT(nvarchar(1000),@GroupId);
						set @iteration2=@iteration2+1;
                end
		SELECT p.*,cl.ClientName 'Client',ecl.ClientName 'EndClient',def.DefinationName 'Status',def.ColorCode 'StatusColor',@workgroups as 'WorkGroups' ,@groups 'Groups'
		FROM PM_Projects AS p
		LEFT JOIN AD_Clients AS cl ON cl.ClientId=p.ClientId
		LEFT JOIN AD_Clients AS ecl ON ecl.ClientId=p.EndClientId
		LEFT JOIN AD_Definations AS def ON def.DefinationId=p.StatusId
		WHERE p.ProjectId=@Value
	END
	else IF @Filter='ProjectManagers'
	BEGIN
		Select UserId as ManagerId, FirstName +' '+LastName as ManagerName  from Sec_Users
		Where IsActive = 1 and IsManager = 1 and CompanyId = (Select CompanyId from Sec_Users
		where UserId = @Value)
	END
		else IF @Filter='ProjectManagersWithRolls'
	BEGIN
		Select u.UserId as ManagerId, u.FirstName +' '+u.LastName+' | '+u.Designation +' | '+r.Name as ManagerName  from Sec_Users u
	
		Inner Join Sec_UserRoles ur on ur.UserId = u.UserId
			inner join Sec_Roles r on r.RoleId=ur.RoleId
		Where u.IsActive = 1 and IsManager = 1  and  CompanyId = (Select CompanyId from Sec_Users
		where UserId = CONVERT(numeric(20,0),@UserId) )
	END
		else IF @Filter='Lookup'
	BEGIN
			
	--Get Status of Projects
	select d.DefinationId as 'StatusId',d.DisplayText as 'StatusName' ,(select COUNT(r.ProjectId) from (SELECT p.*, cl.ClientName 'Client',ecl.ClientName 'EndClient',def.DefinationName 'Status',def.ColorCode 'StatusColor',pro.DefinationName 'Priority'
		FROM PM_Projects AS p
		INNER JOIN AD_Clients AS cl ON cl.ClientId=p.ClientId
		LEFT JOIN AD_Clients AS ecl ON ecl.ClientId=p.EndClientId
		LEFT JOIN AD_Definations AS def ON def.DefinationId=p.StatusId
		LEFT JOIN AD_Definations AS pro ON pro.DefinationId=p.PriorityId
		inner join Sec_UserProjects u on u.ProjectId= p.ProjectID
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=def.DefinationTypeId
		where u.UserId=CONVERT(numeric(20,0),@UserId) and p.IsActive=@Value
		and def.IsActive=1 )r where r.StatusId = d.DefinationId) 'count' from Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='Project Status' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)

	--Get Priority of Projects
	 select d.DefinationId 'PriorityId',d.DefinationName 'PriorityName'  from Ad_DefinationTypes dt
     inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
	 inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
     where dt.DefinationType ='Priority' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
      --Get Clients of Projects
	 select c.ClientId,c.ClientName from AD_Clients c where c.IsActive=1

	END
		else IF @Filter='LookupByFilters'
	BEGIN
			if @StatusIds =''
		begin
		Declare @IdCount0 int,@iteration0 int=1,@Id0 numeric(18,0)
		Declare @TempTableForLoop0 Table(Id int IDENTITY(1,1),WGId numeric(18,0))
		insert into @TempTableForLoop0 select d.DefinationId as 'Item' from Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='Project Status' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
		select @IdCount0=COUNT(*) from @TempTableForLoop0
			while @iteration0<=@IdCount0
				begin
						select @Id0=WGId from (
						select  Id,WGId from @TempTableForLoop0
						) as ff where ff.Id=@iteration0
                        set @StatusIds=@StatusIds+','+CONVERT(nvarchar(1000),@Id0);
						set @iteration0=@iteration0+1;
                end
				set @StatusIds=left (right (@StatusIds, len (@StatusIds)-1), len (@StatusIds)-1) ;
				end

		if @ProritysIds =''
		begin
		Declare @IdCount1 int,@iteration1 int=1,@Id1 numeric(18,0)
		Declare @TempTableForLoop1 Table(Id int IDENTITY(1,1),WGId numeric(18,0))
		insert into @TempTableForLoop1 select d.DefinationId as 'Item' from Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='Priority' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
		select @IdCount1=COUNT(*) from @TempTableForLoop1
			while @iteration1<=@IdCount1
				begin
						select @Id1=WGId from (
						select  Id,WGId from @TempTableForLoop1
						) as ff where ff.Id=@iteration1
                        set @ProritysIds=@ProritysIds+','+CONVERT(nvarchar(1000),@Id1);
						set @iteration1=@iteration1+1;
                end
				set @ProritysIds=left (right (@ProritysIds, len (@ProritysIds)-1), len (@ProritysIds)-1) ;
				end
		if @ClientsIds =''
		begin
		Declare @IdCount_22 int,@iteration_22 int=1,@Id_22 numeric(18,0)
		Declare @TempTableForLoop_22 Table(Id int IDENTITY(1,1),WGId numeric(18,0))
		insert into @TempTableForLoop_22 select c.ClientId as 'Item'from AD_Clients c where c.IsActive=1
		select @IdCount_22=COUNT(*) from @TempTableForLoop_22
			while @iteration_22<=@IdCount_22
				begin
						select @Id_22=WGId from (
						select  Id,WGId from @TempTableForLoop_22
						) as ff where ff.Id=@iteration_22
                        set @ClientsIds=@ClientsIds+','+CONVERT(nvarchar(1000),@Id_22);
						set @iteration_22=@iteration_22+1;
                end
				set @ClientsIds=left (right (@ClientsIds, len (@ClientsIds)-1), len (@ClientsIds)-1) ;
				end

		SET @StatusIds+=','
	    SET @ProritysIds+=','
	    SET @ClientsIds+=','
			   	--select @StatusIds+'---'+@ProritysIds+'---'+@ClientsIds  
	--Get Status of Projects
	select d.DefinationId as 'StatusId',d.DisplayText as 'StatusName' , (Select count(r.ProjectId) from (
	   	SELECT p.*
		FROM PM_Projects AS p
		INNER JOIN AD_Clients AS cl ON cl.ClientId=p.ClientId
		LEFT JOIN AD_Clients AS ecl ON ecl.ClientId=p.EndClientId
		LEFT JOIN AD_Definations AS def ON def.DefinationId=p.StatusId
		LEFT JOIN AD_Definations AS pro ON pro.DefinationId=p.PriorityId
		inner join Sec_UserProjects u on u.ProjectId= p.ProjectID
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=def.DefinationTypeId
		where u.UserId=CONVERT(numeric(20,0),@UserId) and p.IsActive=@Value 
		and p.PlannedDate >= convert(datetime,@FromDate)  AND p.PlannedDate <= convert(datetime,@ToDate) 
		AND Charindex(cast(p.PriorityId as varchar(max))+',', @ProritysIds) > 0
		AND Charindex(cast(p.StatusId as varchar(max))+',', @StatusIds) > 0
		and  Charindex(cast(p.ClientId as varchar(max))+',', @ClientsIds) > 0)
		 r where  r.StatusId=d.DefinationId 
		) 'count' from Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='Project Status' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
		 --Get Priority of Projects
	 select d.DefinationId 'PriorityId',d.DefinationName 'PriorityName'  from Ad_DefinationTypes dt
     inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
	 inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
     where dt.DefinationType ='Priority' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
      --Get Clients of Projects
	 select c.ClientId,c.ClientName from AD_Clients c where c.IsActive=1
		
	END
		else IF @Filter='LookupByKey'
	BEGIN
	--Get Status of Projects
	select d.DefinationId as 'StatusId',d.DisplayText as 'StatusName' , (select count(*) from PM_Projects p inner join Sec_UserProjects u on u.ProjectId= p.ProjectID where p.StatusId=d.DefinationId and u.UserId=CONVERT(numeric(20,0),@UserId)AND p.ProjectName like '%'+@Value+'%') 'count' from Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='Project Status' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
		 --Get Priority of Projects
	 select d.DefinationId 'PriorityId',d.DefinationName 'PriorityName'  from Ad_DefinationTypes dt
     inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
	 inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
     where dt.DefinationType ='Priority' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
      --Get Clients of Projects
	 select c.ClientId,c.ClientName from AD_Clients c where c.IsActive=1

	END
    Else IF @Filter='ProjectByFilters'
		begin
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

		if @ProritysIds =''
		begin
				
		Declare @IdCount1_1 int,@iteration1_1 int=1,@Id1_1 numeric(18,0)
		Declare @TempTableForLoop1_1 Table(Id int IDENTITY(1,1),WGId numeric(18,0))
		insert into @TempTableForLoop1_1 select d.DefinationId as 'Item' from Ad_DefinationTypes dt
        inner join Ad_Definations as d on dt.DefinationTypeId=d.DefinationTypeId
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=d.DefinationTypeId
         where dt.DefinationType ='Priority' and d.IsActive=1 and udt.UserId=CONVERT(numeric(20,0),@UserId)
		select @IdCount1_1=COUNT(*) from @TempTableForLoop1_1
			while @iteration1_1<=@IdCount1_1
				begin
						select @Id1_1=WGId from (
						select  Id,WGId from @TempTableForLoop1_1
						) as ff where ff.Id=@iteration1_1
                        set @ProritysIds=@ProritysIds+','+CONVERT(nvarchar(1000),@Id1_1);
					
						set @iteration1_1=@iteration1_1+1;
                end
				
				set @ProritysIds=left (right (@ProritysIds, len (@ProritysIds)-1), len (@ProritysIds)-1) ;
			
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

	    SET @StatusIds+=','
	    SET @ProritysIds+=','
	    SET @ClientsIds+=','


		SELECT p.*, cl.ClientName 'Client',ecl.ClientName 'EndClient',def.DefinationName 'Status',def.ColorCode 'StatusColor',pro.DefinationName 'Priority',pro.ColorCode 'PriorityColor'
		FROM PM_Projects AS p
		INNER JOIN AD_Clients AS cl ON cl.ClientId=p.ClientId
		LEFT JOIN AD_Clients AS ecl ON ecl.ClientId=p.EndClientId
		LEFT JOIN AD_Definations AS def ON def.DefinationId=p.StatusId
		LEFT JOIN AD_Definations AS pro ON pro.DefinationId=p.PriorityId
		inner join Sec_UserProjects u on u.ProjectId= p.ProjectID
		inner join Sec_UserDefinationType udt on udt.DefinationTypeId=def.DefinationTypeId
		where u.UserId=CONVERT(numeric(20,0),@UserId) and p.IsActive=@Value 
		and p.PlannedDate >= convert(datetime,@FromDate)  AND p.PlannedDate <= convert(datetime,@ToDate) 
		AND Charindex(cast(p.PriorityId as varchar(max))+',', @ProritysIds) > 0
		AND Charindex(cast(p.StatusId as varchar(max))+',', @StatusIds) > 0
		and  Charindex(cast(p.ClientId as varchar(max))+',', @ClientsIds) > 0
		end 


END