CREATE TABLE [dbo].[PM_Tasks] (
    [TaskId]             NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [PTaskId]            NUMERIC (18)   NULL,
    [ProjectId]          NUMERIC (18)   NULL,
    [TaskTypeId]         NUMERIC (18)   NULL,
    [StatusId]           NUMERIC (18)   NULL,
    [PriorityId]         NUMERIC (18)   NULL,
    [PredecessorId]      NUMERIC (18)   NULL,
    [Title]              NVARCHAR (50)  NULL,
    [PlannedDate]        DATETIME       NULL,
    [TargetDate]         DATETIME       NULL,
    [ActualStartDate]    DATETIME       NULL,
    [ActualEndDate]      DATETIME       NULL,
    [EstimatedStartDate] DATETIME       NULL,
    [EstimatedEndDate]   DATETIME       NULL,
    [IsStartMilestone]   BIT            NULL,
    [IsEndMilestone]     BIT            NULL,
    [Description]        NVARCHAR (250) NULL,
    [IsEstimate]         BIT            NULL,
    [ForecastedSites]    INT            NULL,
    [CompletionPercent]  FLOAT (53)     NULL,
    [BudgetCost]         FLOAT (53)     NULL,
    [ActualCost]         FLOAT (53)     NULL,
    [MapCode]            NVARCHAR (50)  NULL,
    [MapColumn]          NVARCHAR (50)  NULL,
    [Color]              NVARCHAR (50)  NULL,
    [ScopeId]            NUMERIC (18)   NULL,
    [IsActive]           BIT            NULL,
    [SortOrder]          INT            NULL,
    [Level]              INT            NULL,
    [Duration]           NUMERIC (18)   NULL,
    CONSTRAINT [PK_PM_Tasks] PRIMARY KEY CLUSTERED ([TaskId] ASC)
);


GO
CREATE trigger [dbo].[PM_Tasks_Insert] on [dbo].[PM_Tasks]
after insert,update 
as
begin
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
 
-- select * from PM_Tasks where PM_Tasks.TaskId  in(SELECT TaskId FROM #CTE3 ) --AND PTaskId <>0 
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
print ''
end