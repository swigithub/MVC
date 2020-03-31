	CREATE procedure spUtilizationPerDriver

	AS
	Begin


	SELECT  ur.FirstName +' '+ ur.LastName as Drive_Tester
	,DaysPerDriver = (SELECT Count(COALESCE(DATEDIFF(DAY, CAST(LEFT(s.AssignedOn,12) as date), CAST(LEFT(s.DriveCompletedOn,12) as date))+1,0)))
	,COUNT(s.SiteId) AS SiteDriven 
	,0 As WeightedSiteDriven
	,0 AS SitesPerDriver
	,0 AS WeightedPerDriver
	,0 AS Cost
	,0 AS Revenue
	,0 AS NetRevenue
	FROM dbo.AV_Sites as s 
	INNER JOIN	dbo.AD_Definations as d on s.CityId=d.DefinationId
	INNER JOIN Sec_Users as ur on s.TesterId=ur.UserId

	GROUP BY ur.FirstName,ur.LastName
	
	END
	-------------
	--SELECT *FROM AV_Sites
	-------------

	--SELECT  COALESCE(DATEDIFF(DAY, CAST(LEFT(AssignedOn,12) as date), CAST(LEFT(DriveCompletedOn,12) as date)),0) as daysPerDriver from AV_Sites
	
	--select AssignedOn, DriveCompletedOn from AV_Sites where TesterId =10049

	--select CAST(LEFT(AssignedOn,12) as date), count(AssignedOn) from AV_Sites where TesterId =10049
	--group by CAST(LEFT(AssignedOn,12) as date)



	--select *from Sec_Users where FirstName like '%Saim%' --10088
	--select *from Sec_Users where FirstName like '%Don%'  --10049


	--PRINT DATEDIFF(DAY, '3/1/2011', '3/1/2011')+1


	--Drive_Tester	
	--daysPerDriver	
	--SiteDriven	
	--WeightedSiteDriven	
	--SitesPerDriver	
	--WeightedPerDriver	
	--Cost	
	--Revenue	
	--NetRevenue