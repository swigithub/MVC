

CREATE procedure [dbo].[spWObyMarket]
AS
Begin
	
SELECT ad.DefinationName, 
(
	SELECT  count(NewSite.SiteCode) FROM
	(
		SELECT SiteCode  
		from av_sites newSit
		--where sitecode like '%A2G3006A%'
		WHERE newSit.CityId=sit.CityId  AND  
		      (SiteCode NOT LIKE '%[-]%' AND SiteCode NOT LIKE '%[_]%') AND 
			  newSit.SubmittedOn BETWEEN '2017-07-28 14:32:18.610' AND '2017-10-28 14:32:18.610'
	) NewSite
) NewSite,

(
	SELECT  count(DISTINCT UniqueSiteID.siteCode) FROM
	(
		SELECT SiteCode	from av_sites unq
		--where sitecode like '%A2G3006A%'
		WHERE   unq.CityId=sit.CityId AND 
				(SiteCode  like '%[-]%' OR SiteCode  like '%[_]%') AND
		        unq.SubmittedOn BETWEEN '2017-07-28 14:32:18.610' AND '2017-10-28 14:32:18.610'
	) UniqueSiteID
) UniqueSiteID,
count(sit.CityId) 'WoCount'

FROM AV_Sites sit
	INNER JOIN AD_definations ad on ad.DefinationId= sit.CityId
	WHERE sit.SubmittedOn BETWEEN '2017-07-28 14:32:18.610' AND '2017-10-28 14:32:18.610'
	GROUP BY sit.CityId,ad.DefinationName
End