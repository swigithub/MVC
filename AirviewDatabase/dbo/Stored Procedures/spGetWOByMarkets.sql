
CREATE Procedure spGetWOByMarkets
	--@FromDate datetime,
	--@ToDate datetime
As

Begin
	select CONVERT(VARCHAR(10), CAST(LEFT(sit.SubmittedOn,12) as datetime), 103) 'WO_Date'
			,cty.DefinationName 'City'
			,COUNT(DISTINCT sit.SiteId) 'WO_Count'
			,COUNT(sit.SiteId) 'Layer_Count'
			--,ISNULL((SELECT COUNT(nls.SiteId) FROM AV_NetLayerStatus nls WHERE nls.SiteId=sit.SiteId and nls.IsActive=1),0) 'Layer_Count'
	from av_sites sit
	INNER JOIN AD_Definations cty on cty.DefinationId=sit.CityId
	INNER JOIN AV_NetLayerStatus nls on nls.SiteId=sit.SiteId
	WHERE sit.IsActive=1 and nls.IsActive=1 
	AND sit.SubmittedOn between '2017-06-01' AND '2017-06-30'
	GROUP BY CAST(LEFT(sit.SubmittedOn,12) as datetime), cty.DefinationName
	ORDER BY cty.DefinationName, CAST(LEFT(sit.SubmittedOn,12) as datetime)
End