
CREATE PROCEDURE [dbo].[AD_GenerateDates]
	@StartDate DATE,
	@EndDate DATE,
	@Filter NVARCHAR(50) 
AS

--SELECT   @StartDate = '2017-12-03', @EndDate = '2018-01-23'3

IF @Filter='day'
BEGIN
	SELECT   DATEPART(DAY,Dates.Dt) AS TargetValue, MIN(Dates.Dt) AS StartDate, MIN(Dates.Dt) AS EndDate
	FROM   (
		  SELECT   DATEADD(DAY,sv.number,@StartDate) AS Dt
		  FROM   master.dbo.spt_values AS sv -- You can use TALLY table instead of this
		  WHERE   sv.type = 'P' AND sv.number <= DATEDIFF(DAY,@StartDate,@EndDate)
	   ) AS Dates
	GROUP BY DATEPART(DAY,Dates.Dt)
	ORDER BY   MIN(Dates.Dt) DESC
END
ELSE IF @Filter='week'
BEGIN
	SELECT 'W'+CAST(DATEPART(WEEK,Dates.Dt) AS NVARCHAR(5))+'-'+CAST(DATEPART(year,Dates.Dt) AS NVARCHAR(5)) AS TargetValue, MIN(Dates.Dt) AS StartDate, MAX(Dates.Dt) AS EndDate
	FROM   (
		  SELECT   DATEADD(DAY,sv.number,@StartDate) AS Dt
		  FROM   master.dbo.spt_values AS sv -- You can use TALLY table instead of this
		  WHERE   sv.type = 'P' AND sv.number <= DATEDIFF(DAY,@StartDate,@EndDate)
	   ) AS Dates
	GROUP BY 'W'+CAST(DATEPART(WEEK,Dates.Dt) AS NVARCHAR(5))+'-'+CAST(DATEPART(year,Dates.Dt) AS NVARCHAR(5))
	ORDER BY   MIN(Dates.Dt),MAX(Dates.Dt) DESC
END
ELSE IF @Filter='month'
BEGIN
	SELECT   LEFT(CAST(DATENAME(month,Dates.Dt) AS NVARCHAR(5)),3)+'-'+CAST(DATEPART(year,Dates.Dt) AS NVARCHAR(5)) AS TargetValue, MIN(Dates.Dt) AS StartDate, MAX(Dates.Dt) AS EndDate
	FROM   (
		  SELECT   DATEADD(DAY,sv.number,@StartDate) AS Dt
		  FROM   master.dbo.spt_values AS sv -- You can use TALLY table instead of this
		  WHERE   sv.type = 'P' AND sv.number <= DATEDIFF(DAY,@StartDate,@EndDate)
	   ) AS Dates
	GROUP BY LEFT(CAST(DATENAME(month,Dates.Dt) AS NVARCHAR(5)),3)+'-'+CAST(DATEPART(year,Dates.Dt) AS NVARCHAR(5))
	ORDER BY   MIN(Dates.Dt),MAX(Dates.Dt) DESC
END
ELSE IF @Filter='quarter'
BEGIN
	SELECT   'Q'+CAST(DATEPART(quarter,Dates.Dt) AS NVARCHAR(5))+'-'+CAST(DATEPART(year,Dates.Dt) AS NVARCHAR(5)) AS TargetValue, MIN(Dates.Dt) AS StartDate, MAX(Dates.Dt) AS EndDate
	FROM   (
		  SELECT   DATEADD(DAY,sv.number,@StartDate) AS Dt
		  FROM   master.dbo.spt_values AS sv -- You can use TALLY table instead of this
		  WHERE   sv.type = 'P' AND sv.number <= DATEDIFF(DAY,@StartDate,@EndDate)
	   ) AS Dates
	GROUP BY 'Q'+CAST(DATEPART(quarter,Dates.Dt) AS NVARCHAR(5))+'-'+CAST(DATEPART(year,Dates.Dt) AS NVARCHAR(5))
	ORDER BY   MIN(Dates.Dt),MAX(Dates.Dt) DESC
END
ELSE IF @Filter='year'
BEGIN
	SELECT   DATEPART(Year,Dates.Dt) AS TargetValue, MIN(Dates.Dt) AS StartDate, MAX(Dates.Dt) AS EndDate
	FROM   (
		  SELECT   DATEADD(DAY,sv.number,@StartDate) AS Dt
		  FROM   master.dbo.spt_values AS sv -- You can use TALLY table instead of this
		  WHERE   sv.type = 'P' AND sv.number <= DATEDIFF(DAY,@StartDate,@EndDate)
	   ) AS Dates
	GROUP BY DATEPART(Year,Dates.Dt)
	ORDER BY   MIN(Dates.Dt),MAX(Dates.Dt) DESC
END