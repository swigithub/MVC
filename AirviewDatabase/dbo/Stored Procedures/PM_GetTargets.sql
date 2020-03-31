CREATE PROCEDURE PM_GetTargets
	@ProjectId NUMERIC(18,0),
	@MilestoneId NUMERIC(18,0),
	@StageId NUMERIC(18,0)=0,
	@StartDate DATE,
	@EndDate DATE,
	@Filter NVARCHAR(50) 
AS
	--EXEC PM_GetTargets 20021,50078,0,'2016-01-01','2019-02-01','week'
	DECLARE @strSQL AS NVARCHAR(MAX)=''
	
	IF @Filter='day'
	BEGIN	

	
		Declare @count numeric(18,0)
		if @StageId >0
		begin

         set @count=  (select COUNT(DISTINCT (CONVERT(VARCHAR, CONVERT(DATETIME,rst.TargetValue),23)))  from (SELECT TOP 31  y.TargetDate TargetValue,y.SiteCount,y.CreatedOn
		FROM PM_Targets y
		WHERE (y.ProjectId=ProjectId AND y.MilestoneId=@MilestoneId and y.StageId=@StageId)   AND y.TargetType=@Filter AND CONVERT(varchar(10), y.TargetDate, 101) <=@EndDate AND CONVERT(varchar(10), y.TargetDate, 101) >=@StartDate
		
		ORDER BY  y.CreatedOn desc) rst )


	select  rst.TargetValue ,rst.SiteCount  from (SELECT TOP 31 ROW_NUMBER() OVER(ORDER BY y.CreatedOn desc) AS Row1, y.TargetDate TargetValue,y.SiteCount,y.CreatedOn
		FROM PM_Targets y
 WHERE (y.ProjectId=ProjectId AND y.MilestoneId=@MilestoneId and y.StageId=@StageId)   AND y.TargetType=@Filter AND CONVERT(varchar(10), y.TargetDate, 101) <=@EndDate AND CONVERT(varchar(10), y.TargetDate, 101) >=@StartDate
		
		ORDER BY  y.CreatedOn desc) rst where rst.Row1 <=@count
		end 
		else
	begin
       set @count=  (select COUNT(DISTINCT (CONVERT(VARCHAR, CONVERT(DATETIME,rst.TargetValue),23)))  from (SELECT TOP 31  y.TargetDate TargetValue,y.SiteCount,y.CreatedOn
		FROM PM_Targets y
		WHERE (y.ProjectId=ProjectId AND y.MilestoneId=@MilestoneId )   AND y.TargetType=@Filter AND CONVERT(varchar(10), y.TargetDate, 101) <=@EndDate AND CONVERT(varchar(10), y.TargetDate, 101) >=@StartDate
		
		ORDER BY  y.CreatedOn desc) rst )


	select  rst.TargetValue ,rst.SiteCount  from (SELECT TOP 31 ROW_NUMBER() OVER(ORDER BY y.CreatedOn desc) AS Row1, y.TargetDate TargetValue,y.SiteCount,y.CreatedOn
		FROM PM_Targets y
 WHERE (y.ProjectId=ProjectId AND y.MilestoneId=@MilestoneId )   AND y.TargetType=@Filter AND CONVERT(varchar(10), y.TargetDate, 101) <=@EndDate AND CONVERT(varchar(10), y.TargetDate, 101) >=@StartDate
		
		ORDER BY  y.CreatedOn desc) rst where rst.Row1 <=@count
	End
	END
	ELSE IF @Filter='week'
	BEGIN
		SET @strSQL=
		'SELECT TOP 12 y.TargetValue,SUM(ISNULL(y.SiteCount,0)) SiteCount' +
		' FROM PM_Targets y'+
		' WHERE (y.ProjectId='+CAST(@ProjectId AS NVARCHAR(15))+' AND y.MilestoneId='+CAST(@MilestoneId AS NVARCHAR(15))+')'+
		CASE WHEN ISNULL(@StageId,0)>0 THEN ' OR y.StageId=' +  CAST(@StageId AS NVARCHAR(15)) ELSE '' END+
		' AND y.TargetType='''+@Filter +''''+		
		' AND y.RevisionId=ISNULL((SELECT MAX(x.RevisionId) '+
		'	FROM PM_Targets x'+
		'	WHERE (x.ProjectId='+CAST(@ProjectId AS NVARCHAR(15))+' AND x.MilestoneId='+CAST(@MilestoneId AS NVARCHAR(15))+') '+
		CASE WHEN ISNULL(@StageId,0)>0 THEN ' OR x.StageId=' +  CAST(@StageId AS NVARCHAR(15)) ELSE '' END+
		' AND x.TargetType='''+@Filter+'''' +
		'	AND x.TargetValue IN(SELECT TOP 12 DATEPART(WEEK,Dates.Dt)' +
		'						FROM   '+
		'						('+
		'							SELECT   DATEADD(DAY,sv.number,'''+CONVERT (varchar(10), @StartDate, 101)+''') AS Dt' +
		'							FROM   master.dbo.spt_values AS sv'+
		'							WHERE   sv.type = ''P'' AND sv.number <= DATEDIFF(DAY,'''+CONVERT (varchar(10), @StartDate, 101)+''','''+CONVERT (varchar(10), @EndDate, 101)+''')'+
		'						) AS Dates' +
		'						GROUP BY DATEPART(WEEK,Dates.Dt)' +
		'						ORDER BY DATEPART(WEEK,Dates.Dt) DESC)'+
		'				 ),0)'+
		' GROUP BY y.TargetValue'
	END
	ELSE IF @Filter='month'
	BEGIN
		SET @strSQL=
		'SELECT TOP 12 y.TargetValue,SUM(ISNULL(y.SiteCount,0)) SiteCount' +
		' FROM PM_Targets y'+
		' WHERE (y.ProjectId='+CAST(@ProjectId AS NVARCHAR(15))+' AND y.MilestoneId='+CAST(@MilestoneId AS NVARCHAR(15))+')'+
		CASE WHEN ISNULL(@StageId,0)>0 THEN ' OR y.StageId=' +  CAST(@StageId AS NVARCHAR(15)) ELSE '' END+
		' AND y.TargetType='''+@Filter +''''+		
		' AND y.RevisionId=ISNULL((SELECT MAX(x.RevisionId) '+
		'	FROM PM_Targets x'+
		'	WHERE (x.ProjectId='+CAST(@ProjectId AS NVARCHAR(15))+' AND x.MilestoneId='+CAST(@MilestoneId AS NVARCHAR(15))+') '+
		CASE WHEN ISNULL(@StageId,0)>0 THEN ' OR x.StageId=' +  CAST(@StageId AS NVARCHAR(15)) ELSE '' END+
		' AND x.TargetType='''+@Filter+'''' +
		'	AND x.TargetValue IN(SELECT TOP 12 DATEPART(month,Dates.Dt)' +
		'						FROM   '+
		'						('+
		'							SELECT   DATEADD(DAY,sv.number,'''+CONVERT (varchar(10), @StartDate, 101)+''') AS Dt' +
		'							FROM   master.dbo.spt_values AS sv'+
		'							WHERE   sv.type = ''P'' AND sv.number <= DATEDIFF(DAY,'''+CONVERT (varchar(10), @StartDate, 101)+''','''+CONVERT (varchar(10), @EndDate, 101)+''')'+
		'						) AS Dates' +
		'						GROUP BY DATEPART(month,Dates.Dt)' +
		'						ORDER BY DATEPART(month,Dates.Dt) DESC)'+
		'				 ),0)'+
		' GROUP BY y.TargetValue'
	END
	ELSE IF @Filter='quarter'
	BEGIN
		SET @strSQL=
		'SELECT TOP 12 y.TargetValue,SUM(ISNULL(y.SiteCount,0)) SiteCount' +
		' FROM PM_Targets y'+
		' WHERE (y.ProjectId='+CAST(@ProjectId AS NVARCHAR(15))+' AND y.MilestoneId='+CAST(@MilestoneId AS NVARCHAR(15))+')'+
		CASE WHEN ISNULL(@StageId,0)>0 THEN ' OR y.StageId=' +  CAST(@StageId AS NVARCHAR(15)) ELSE '' END+
		' AND y.TargetType='''+@Filter +''''+		
		' AND y.RevisionId=ISNULL((SELECT MAX(x.RevisionId) '+
		'	FROM PM_Targets x'+
		'	WHERE (x.ProjectId='+CAST(@ProjectId AS NVARCHAR(15))+' AND x.MilestoneId='+CAST(@MilestoneId AS NVARCHAR(15))+') '+
		CASE WHEN ISNULL(@StageId,0)>0 THEN ' OR x.StageId=' +  CAST(@StageId AS NVARCHAR(15)) ELSE '' END+
		' AND x.TargetType='''+@Filter+'''' +
		'	AND x.TargetValue IN(SELECT TOP 12 DATEPART(quarter,Dates.Dt)' +
		'						FROM   '+
		'						('+
		'							SELECT   DATEADD(DAY,sv.number,'''+CONVERT (varchar(10), @StartDate, 101)+''') AS Dt' +
		'							FROM   master.dbo.spt_values AS sv'+
		'							WHERE   sv.type = ''P'' AND sv.number <= DATEDIFF(DAY,'''+CONVERT (varchar(10), @StartDate, 101)+''','''+CONVERT (varchar(10), @EndDate, 101)+''')'+
		'						) AS Dates' +
		'						GROUP BY DATEPART(quarter,Dates.Dt)' +
		'						ORDER BY DATEPART(quarter,Dates.Dt) DESC)'+
		'				 ),0)'+
		' GROUP BY y.TargetValue'
	END
	ELSE IF @Filter='year'
	BEGIN
		SET @strSQL=
		'SELECT TOP 12 y.TargetValue,SUM(ISNULL(y.SiteCount,0)) SiteCount' +
		' FROM PM_Targets y'+
		' WHERE (y.ProjectId='+CAST(@ProjectId AS NVARCHAR(15))+' AND y.MilestoneId='+CAST(@MilestoneId AS NVARCHAR(15))+')'+
		CASE WHEN ISNULL(@StageId,0)>0 THEN ' OR y.StageId=' +  CAST(@StageId AS NVARCHAR(15)) ELSE '' END+
		' AND y.TargetType='''+@Filter +''''+		
		' AND y.RevisionId=ISNULL((SELECT MAX(x.RevisionId) '+
		'	FROM PM_Targets x'+
		'	WHERE (x.ProjectId='+CAST(@ProjectId AS NVARCHAR(15))+' AND x.MilestoneId='+CAST(@MilestoneId AS NVARCHAR(15))+') '+
		CASE WHEN ISNULL(@StageId,0)>0 THEN ' OR x.StageId=' +  CAST(@StageId AS NVARCHAR(15)) ELSE '' END+
		' AND x.TargetType='''+@Filter+'''' +
		'	AND x.TargetValue IN(SELECT TOP 12 DATEPART(year,Dates.Dt)' +
		'						FROM   '+
		'						('+
		'							SELECT   DATEADD(DAY,sv.number,'''+CONVERT (varchar(10), @StartDate, 101)+''') AS Dt' +
		'							FROM   master.dbo.spt_values AS sv'+
		'							WHERE   sv.type = ''P'' AND sv.number <= DATEDIFF(DAY,'''+CONVERT (varchar(10), @StartDate, 101)+''','''+CONVERT (varchar(10), @EndDate, 101)+''')'+
		'						) AS Dates' +
		'						GROUP BY DATEPART(year,Dates.Dt)' +
		'						ORDER BY DATEPART(year,Dates.Dt) DESC)'+
		'				 ),0)'+
		' GROUP BY y.TargetValue'
	END
	select @strSQL	
	EXEC (@strSQL)