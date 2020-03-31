-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PM_Project_KPI
@Filter nvarchar(100)='',
@value nvarchar(100)='',
@StartDate nvarchar(100)='',
@EndDate nvarchar(100)='',
@List List readonly,
@Type nvarchar(50) ='',
@Site nvarchar(50) ='',
@Carrier nvarchar(50) ='',
@Sector nvarchar(50) =''	
AS
BEGIN
--[dbo].[PM_Project_KPI] 'ByDefinationTypeId','555'
if @Filter='ByDefinationTypeId'
begin
	select def.DefinationId,def.DefinationName,def.ColorCode,def.DisplayText,def.PDefinationId,def.KeyCode, dt.DefinationType,def.DefinationTypeId from AD_Definations def
	inner join AD_DefinationTypes dt on dt.DefinationTypeId = def.DefinationTypeId
	where dt.DefinationType = 'NetworkMode' or dt.DefinationType='Conditions' or dt.DefinationType='KPI_DataType' or dt.DefinationType='KPI_Actions'
	 or dt.DefinationType='Column_Type'  or dt.DefinationType='Kpi_Level'  or dt.DefinationType='Formula_Operator'  or dt.DefinationType='Band' 
	--.DefinationTypeId=8 or def.DefinationTypeId=120064 or def.DefinationTypeId=40039 or def.DefinationTypeId=90066 or def.DefinationTypeId=90065 or def.DefinationTypeId=90064  
	--or def.DefinationTypeId=100065  or DefinationTypeId=10 

	-- select * from AD_DefinationTypes where DefinationTypeId= 10
end
else if @Filter='Get_KPI'
begin
	select *,ad.DefinationName 'DataTypeName' from PM_ProjectKPI kp
	inner join AD_Definations ad on kp.DataType=ad.DefinationId
	 where kp.TaskId=@value
END
else if @Filter='Insert_KPI'
BEGIN
	DECLARE @KPI NVARCHAR(50)=null
	DECLARE @Kpi_Name NVARCHAR(50)=null
	DECLARE  @Kpi_Type NVARCHAR(50)=null
	DECLARE  @Level NVARCHAR(50)=null
	DECLARE @Technology NVARCHAR(50)=null
	DECLARE  @DataType NVARCHAR(50)=null
	DECLARE  @TaskId NVARCHAR(50)=null
	DECLARE  @IsActive bit=0
	DECLARE  @Formula nvarchar(250)=null
	DECLARE  @BandId nvarchar(250)=null
	DECLARE  @Weightage nvarchar(250)=null
	DECLARE  UpdateK CURSOR READ_ONLY
	FOR
	SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11 FROM @List AS l 
	--OPEN CURSOR.
	OPEN UpdateK 
	--FETCH THE RECORD INTO THE VARIABLES.
	FETCH NEXT FROM UpdateK INTO @KPI,@Kpi_Name,@Kpi_Type,@Level,@Technology,@DataType,@TaskId,@IsActive,@Formula,@BandId,@Weightage
	--LOOP UNTIL RECORDS ARE AVAILABLE.
	WHILE @@FETCH_STATUS = 0
	BEGIN
			if @KPI=0
			begin
			insert into PM_ProjectKPI (Kpi_Name,Kpi_Type,[Level],Technology,DataType,TaskId,Formula,BandId,Weightage) SELECT @Kpi_Name,@Kpi_Type,@Level,@Technology,@DataType,@TaskId,@Formula,@BandId,@Weightage
			end
			else if @KPI !=0 and @IsActive=0
			begin
			delete from  PM_ProjectKPI 
			WHERE KPI=@KPI;
			end
	else 
			begin
			UPDATE PM_ProjectKPI 
			SET Kpi_Name = @Kpi_Name, Kpi_Type=@Kpi_Type,[Level] = @Level, Technology=@Technology,DataType=@DataType,Formula=@Formula,BandId=@BandId,
			Weightage=@Weightage
			WHERE KPI=@KPI;
			end
		--FETCH THE NEXT RECORD INTO THE VARIABLES.
		FETCH NEXT FROM UpdateK INTO @KPI,@Kpi_Name,@Kpi_Type,@Level,@Technology,@DataType,@TaskId,@IsActive,@Formula,@BandId,@Weightage
	END
 
	--CLOSE THE CURSOR.
	CLOSE UpdateK
	DEALLOCATE UpdateK
END
else if @Filter='Get_Threshold'
begin
	select *,KP.Kpi_Name 'KPIName' from PM_KPI_Threshold th   
	inner join PM_ProjectKPI KP ON KP.KPI = th.KPI
	where th.KPI=@value
END
else if @Filter='Insert_Threshold'
BEGIN
	DECLARE @_KPI NVARCHAR(50)=null
	DECLARE  @Condition NVARCHAR(50)=null
	DECLARE  @Threshold_Name NVARCHAR(50)=null
	DECLARE @ThresholdTo NVARCHAR(50)=null
	DECLARE  @Color NVARCHAR(50)=null
	DECLARE  @Action NVARCHAR(50)=null
	DECLARE  @_IsActive bit=0
	DECLARE  @Threshold NVARCHAR(50)=null
	DECLARE  UpdateC CURSOR READ_ONLY
	FOR
	SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8 FROM @List AS l 
	--OPEN CURSOR.
	OPEN UpdateC 
	--FETCH THE RECORD INTO THE VARIABLES.
	FETCH NEXT FROM UpdateC INTO @_KPI,@Condition,@Threshold_Name,@ThresholdTo,@Color,@Action,@_IsActive,@Threshold
	--LOOP UNTIL RECORDS ARE AVAILABLE.
	WHILE @@FETCH_STATUS = 0
	BEGIN
		 
		BEGIN
			if @Threshold=0 
			begin
			insert into PM_KPI_Threshold (KPI,Condition,Threshold_Name,ThresholdTo,Color,Action) SELECT @_KPI,@Condition,@Threshold_Name,@ThresholdTo,@Color,@Action 
			end
			else if @_IsActive=0 and @Threshold !=0 
			begin
			delete from PM_KPI_Threshold WHERE Threshold=@Threshold;
			end
			else 
			begin
			UPDATE PM_KPI_Threshold 
	SET Condition = @Condition, Threshold_Name=@Threshold_Name,ThresholdTo = @ThresholdTo, Color=@Color,Action=@Action
	WHERE Threshold=@Threshold;
			end
		END
		--FETCH THE NEXT RECORD INTO THE VARIABLES.
		FETCH NEXT FROM UpdateC INTO @_KPI,@Condition,@Threshold_Name,@ThresholdTo,@Color,@Action,@_IsActive,@Threshold
	END
 
	--CLOSE THE CURSOR.
	CLOSE UpdateC
	DEALLOCATE UpdateC
END
else if @Filter='Insert_KPIDATA'

BEGIN
	DECLARE @_KPIDATA_Id NVARCHAR(50)=null
	DECLARE  @KPI_Id NVARCHAR(50)=null
	DECLARE  @KPI_Value NVARCHAR(50)=null
	DECLARE @KPI_Date NVARCHAR(50)=null
	DECLARE  @KPI_TaskId NVARCHAR(50)=null
	
	DECLARE  UpdateK CURSOR READ_ONLY
	FOR
	SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5 FROM @List AS l 
	--OPEN CURSOR.
	OPEN UpdateK 
	--FETCH THE RECORD INTO THE VARIABLES.
	FETCH NEXT FROM UpdateK INTO @_KPIDATA_Id,@KPI_Id,@KPI_Value,@KPI_Date,@KPI_TaskId
	--LOOP UNTIL RECORDS ARE AVAILABLE.
	WHILE @@FETCH_STATUS = 0
	BEGIN
		 
		BEGIN
			if @_KPIDATA_Id=0 
			begin
			insert into PM_KPI_DATA (KPI_Id,KPI_Value,[Date],TaskId) SELECT  @KPI_Id,@KPI_Value,@KPI_Date,@KPI_TaskId --from @List l
			end
			else 
			begin
			update PM_KPI_DATA set KPI_Id=@KPI_Id,KPI_Value=@KPI_Value,[Date]=@KPI_Date,TaskId=@KPI_TaskId where KPIData_Id=@_KPIDATA_Id
			end
			END
		--FETCH THE NEXT RECORD INTO THE VARIABLES.
		FETCH NEXT FROM UpdateK INTO @_KPIDATA_Id,@KPI_Id,@KPI_Value,@KPI_Date,@KPI_TaskId 
	END
 
	--CLOSE THE CURSOR.
	CLOSE UpdateK
	DEALLOCATE UpdateK
END


else if @Filter='Get_KPIData'
begin
	select kpd.*,al.DefinationName 'KpiType',kp.Kpi_Name 'KpiName',ad.DefinationName 'DataTypeName',kp.Formula 'Formula' from PM_KPI_DATA kpd
	left join PM_ProjectKPI kp ON kp.KPI = kpd.KPI_Id
	inner join AD_Definations ad on kp.DataType=ad.DefinationId
	inner join AD_Definations al on al.DefinationId=kp.Kpi_Type
	where kpd.TaskId=50076 and  kpd.Date BETWEEN @StartDate AND @EndDate 
END
else if @Filter='Get_KPI_ByType'
begin
	select * from  PM_ProjectKpi pk
	inner join AD_Definations ad on ad.DefinationId=pk.[Level] 
	 where ad.DefinationName=@value
END
end









--USE [AirView]
--GO
--/****** Object:  Trigger [dbo].[PM_Tasks_Insert]    Script Date: 8/9/2018 10:51:26 PM ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--ALTER trigger [dbo].[PM_Tasks_Insert] on [dbo].[PM_Tasks]
--after insert,update 
--as
--begin
--declare @PTaskId int=0
--declare @TaskId int=0
--declare @ProjectId int=0
--declare @MinDate NVARCHAR(100)=''
--declare @MaxDate NVARCHAR(100)=''
--declare @Projectd int=0
--declare @DateDifference int=0
--declare @OldDate nvarchar(100)=''
--declare @StartDateDifference int=0
--declare @OldStartDate nvarchar(100)=''
--declare @NewStartDate nvarchar(100)=''
--declare @TaskDate nvarchar(100)=''
--declare @PredecessorId  int=0
--select @PredecessorId =Inserted.PredecessorId from Inserted
--SELECT @PTaskId = INSERTED.PTaskId from INSERTED
--SELECT @TaskId = INSERTED.TaskId from INSERTED


----begin
--IF(@TaskId >0)
--BEGIN
--begin try
--select @TaskDate = INSERTED.ActualEndDate from INSERTED
--select @NewStartDate = INSERTED.ActualStartDate from INSERTED
--select @OldStartDate = Deleted.ActualStartDate from Deleted
--SELECT @StartDateDifference = DATEDIFF(day, @OldStartDate, @NewStartDate) 
--SELECT @OldDate = Deleted.ActualEndDate from Deleted
--SELECT @DateDifference = DATEDIFF(day, @OldDate, @TaskDate) --INSERTED.PredecessorId from INSERTED

--IF(@DateDifference > 0)
--BEGIN
--PRINT ('-----TASK')
--PRINT(@TaskId)
--PRINT(@PTaskId)
--print(@OldDate)
--print(@TaskDate)
--print(@DateDifference)
--create table #CTE2(
--PTaskId INT,
--TaskId INT,
--ActualEndDate date
--)
--;WITH TaskCTE AS 
--( 
----initialization 
--SELECT PTaskId,TaskId,ActualEndDate
--FROM pm_tasks 
--WHERE PTaskId =@TaskId and PTaskId <>0 --<> 0 --TaskId = 80114
--UNION ALL 
----recursive execution 
--SELECT e.PTaskId,e.TaskId,e.ActualEndDate 
--FROM pm_tasks e INNER JOIN TaskCTE m  
--ON e.PTaskId = m.TaskId 
--) 
--INSERT INTO  #CTE2 select * from TaskCTE
--option (maxrecursion 0)
--select * from  #CTE2
--print('---temp2--')
--update  pm_tasks  set ActualEndDate= DATEADD(day,@DateDifference,ActualEndDate)
--,ActualStartDate= DATEADD(day,@StartDateDifference,ActualStartDate)
--,EstimatedStartDate= DATEADD(day, @StartDateDifference,EstimatedStartDate)
--,EstimatedEndDate= DATEADD(day,@DateDifference,EstimatedEndDate)
-- where TaskId  in(SELECT TaskId FROM #CTE2 )
--Drop Table #CTE2
--END
--END try
--begin catch
--Drop Table #CTE2
----print('error')
--end catch
----END
----print(@DateDifference)
----Drop Table #CTE2
----print ''
--end

-------------

--IF(@PTaskId >0)
--begin

--create table #CTE3(
--PTaskId INT,
--TaskId INT,
----MinDate date,
----MaxDate date
--)
--;WITH TaskCTE AS 
--( 
----initialization 
--SELECT er.PTaskId,er.TaskId--,'2018-08-07 00:00:00.000' 'MinDate','2018-08-07 00:00:00.000' 'MaxDate'
--FROM pm_tasks  er
--WHERE er.TaskId =@PTaskId --and er.PTaskId <>0 --<> 0 --TaskId = 80114
--UNION ALL 
----recursive execution 
--SELECT e.PTaskId,e.TaskId--,'2018-08-07 00:00:00.000' 'MinDate','2018-08-07 00:00:00.000' 'MaxDate'
--FROM pm_tasks e INNER JOIN TaskCTE m  
--ON e.TaskId = m.PTaskId 
--) 
--INSERT INTO  #CTE3 select * from TaskCTE
--option (maxrecursion 0)
--select * from  #CTE3
--set @MinDate= (select distinct min(ActualStartDate) from PM_Tasks where PTaskId = @PTaskId)
--set @MaxDate =(select distinct max(ActualEndDate) from PM_Tasks where PTaskId = @PTaskId)
--update PM_Tasks  set 
--PM_Tasks.ActualStartDate=@MinDate,  --(select distinct min(ActualStartDate) from PM_Tasks where PTaskId = PM_Tasks.TaskId),   
--PM_Tasks.ActualEndDate=@MaxDate, --(select distinct max(ActualEndDate) from PM_Tasks where PTaskId = PM_Tasks.TaskId),
--PM_Tasks.EstimatedStartDate= @MinDate,--(select distinct min(ActualStartDate) from PM_Tasks where PTaskId = PM_Tasks.TaskId),
--PM_Tasks.EstimatedEndDate=@MaxDate --(select distinct max(ActualEndDate) from PM_Tasks where PTaskId = PM_Tasks.TaskId)
-- where PM_Tasks.TaskId  in(SELECT TaskId FROM #CTE3 ) --AND PTaskId <>0
 
-- --select * from PM_Tasks where PM_Tasks.TaskId  in(SELECT TaskId FROM #CTE3 ) --AND PTaskId <>0 
-- drop table #CTE3
----set @MinDate= (select distinct min(ActualStartDate) from PM_Tasks where PTaskId = @PTaskId)
----set @MaxDate =(select distinct max(ActualEndDate) from PM_Tasks where PTaskId = @PTaskId)
----update PM_Tasks set ActualStartDate=@MinDate,ActualEndDate=@MaxDate where TaskId= @PTaskId
----print(@PTaskId)
----PRINT(@MinDate)
----PRINT(@MaxDate)
----PRINT('-----')
--end
----select @MinDate,@MaxDate,@PTaskId
----milestone end
--------

--IF(@PredecessorId >0)
--begin
--begin try
--SELECT @OldDate = Deleted.ActualEndDate from Deleted
--select @TaskDate = INSERTED.ActualEndDate from INSERTED
--set @DateDifference = DATEDIFF(day, @OldDate, @TaskDate) --INSERTED.PredecessorId from INSERTED

--IF(@DateDifference >0)
--BEGIN
--PRINT ('-----PRE')
--PRINT(@PredecessorId)
--print(@OldDate)
--print(@TaskDate)
--print(@DateDifference)
--create table #CTE(
--PredecessorId INT,
--TaskId INT,
--ActualEndDate date
--)
--;WITH TaskCTE AS 
--( 
----initialization 
--SELECT PredecessorId,TaskId,ActualEndDate
--FROM pm_tasks 
--WHERE PredecessorId =@TaskId --<> 0 --TaskId = 80114
--UNION ALL 
----recursive execution 
--SELECT e.PredecessorId,e.TaskId,e.ActualEndDate 
--FROM pm_tasks e INNER JOIN TaskCTE m  
--ON e.PredecessorId = m.TaskId AND e.PredecessorId <> 0
--) 
--INSERT INTO  #CTE select * from TaskCTE
--option (maxrecursion 0)
----select * from  #CTE
--update  pm_tasks  set ActualEndDate= DATEADD(day,@DateDifference,ActualEndDate),ActualStartDate= DATEADD(day,@DateDifference,ActualStartDate)
--,EstimatedStartDate= DATEADD(day, @DateDifference,EstimatedStartDate),EstimatedEndDate= DATEADD(day,@DateDifference,EstimatedEndDate)
-- where TaskId  in(SELECT TaskId FROM #CTE )
--Drop Table #CTE
--END
--END try
--begin catch
----Drop Table #CTE
----print('error')
--end catch
----print(@DateDifference)
----Drop Table #CTE
--end
---------


------
----project start
--SELECT @Projectd = INSERTED.ProjectId from INSERTED
--set  @MinDate= (select distinct min(ActualStartDate) from PM_Tasks where ProjectId = @Projectd)
--set @MaxDate =(select distinct max(ActualEndDate) from PM_Tasks where ProjectId = @Projectd)
--update PM_Projects set ActualStartDate=@MinDate,ActualEndDate=@MaxDate where ProjectId= @Projectd
--print (@Projectd)
--PRINT(@MinDate)
--PRINT(@MaxDate)
--end