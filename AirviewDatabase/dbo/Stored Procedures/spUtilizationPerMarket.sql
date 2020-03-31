CREATE Procedure spUtilizationPerMarket

AS
Begin

SELECT ad.DefinationName AS 'Market',Count(sit.TesterId) 'DriveForAllDrivers',
(
	SELECT  count(DISTINCT unq.siteCode) FROM
	(
		SELECT SiteCode	from av_sites unq
		WHERE   unq.CityId=sit.CityId AND 
				(SiteCode  like '%[-]%' OR SiteCode  like '%[_]%') AND
		        unq.SubmittedOn BETWEEN '2017-07-28 14:32:18.610' AND '2017-10-28 14:32:18.610'
	) unq
) UniqueSites,
(
	SELECT  count(d.siteCode) FROM
	(
		SELECT SiteCode	from av_sites d
		WHERE   d.CityId=sit.CityId AND 
				(SiteCode  like '%[-]%' OR SiteCode  like '%[_]%') AND
		        d.SubmittedOn BETWEEN '2017-07-28 14:32:18.610' AND '2017-10-28 14:32:18.610'
	) d
) Drive, 0 AS WeightedDrive,0 AS RatioOnWeighted, 0 AS Ratio

FROM AV_Sites sit
	INNER JOIN AD_definations ad on ad.DefinationId= sit.CityId
	WHERE sit.SubmittedOn BETWEEN '2017-07-28 14:32:18.610' AND '2017-10-28 14:32:18.610'
	GROUP BY sit.CityId,ad.DefinationName, sit.TesterId

	END