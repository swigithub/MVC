
CREATE Procedure spUtilizationPerDay

AS
Begin


SELECT	DATENAME(WEEKDAY, CAST(LEFT(sit.DriveCompletedOn,12) as datetime))+', '+ 
		DATENAME(MONTH, CAST(LEFT(sit.DriveCompletedOn,12) as datetime)) + ' ' +
		CAST(DAY(CAST(LEFT(sit.DriveCompletedOn,12) as datetime)) AS VARCHAR(2)) + ' ' + 
		CAST(YEAR(CAST(LEFT(sit.DriveCompletedOn,12) as datetime)) AS VARCHAR(4)) AS 'DriveCompletedOn', Count(sit.TesterId) 'DriverPerDay',
	(
		SELECT  count(NewSite.SiteCode) FROM
		(
			SELECT SiteCode  
			from av_sites newSit
			WHERE (SiteCode NOT LIKE '%[-]%' AND SiteCode NOT LIKE '%[_]%') 
			AND (CAST(LEFT(sit.DriveCompletedOn,12) as datetime) = CAST(LEFT(newSit.DriveCompletedOn,12) as datetime))
			AND  newSit.DriveCompletedOn BETWEEN '2017-06-28 14:32:18.610' AND '2017-10-28 14:32:18.610'
		) NewSite
	) Sites,0 as 'WeightedSites',0 as 'Ratio',0 as 'WeightedRatio',0 as 'Revenue',0 as 'Cost',0 as 'NetRevenue'

FROM dbo.AV_Sites sit
INNER JOIN dbo.AD_Definations as d on sit.CityId=d.DefinationId
where DriveCompletedOn BETWEEN '2017-06-28 14:32:18.610' AND '2017-10-28 14:32:18.610'
GROUP BY CAST(LEFT(sit.DriveCompletedOn,12) as datetime)

End









---------------------------
-----------------------------
--SELECT CAST(LEFT(sit.DriveCompletedOn,12) as datetime) AS 'DriveCompletedOn', count(sit.TesterId) 
--FROM dbo.AV_Sites sit
--INNER JOIN dbo.AV_NetLayerStatus as d on sit.TesterId=d.TesterId
--where sit.DriveCompletedOn BETWEEN '2017-06-28 14:32:18.610' AND '2017-10-28 14:32:18.610'
--GROUP BY CAST(LEFT(sit.DriveCompletedOn,12) as datetime), sit.TesterId


-----------------------
--Select distinct sit.TesterId
--FROM dbo.AV_Sites sit
--INNER JOIN dbo.AV_NetLayerStatus as d on sit.TesterId=d.TesterId 
--where  sit.TesterId > 0

--Select TesterId from AV_Sites where TesterId > 0
--Select TesterId from AV_NetLayerStatus where TesterId > 0