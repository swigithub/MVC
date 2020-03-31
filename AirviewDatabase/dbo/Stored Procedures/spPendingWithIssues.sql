	CREATE PROCEDURE [dbo].[spPendingWithIssues]
	AS
	BEGIN


	 SELECT x.SiteId,x.WoRefId
   ,d.DefinationName As 'Market'
   ,CASE 
		WHEN charindex('_',x.SiteCode)=0 AND charindex('-',x.SiteCode)=0 THEN x.SiteCode 
		WHEN charindex('-',x.SiteCode)>0 THEN LEFT(x.SiteCode,charindex('-',x.SiteCode)-1) 
		ELSE LEFT(SiteCode,charindex('_',SiteCode)-1) 
	END 'OriginalSite'
	,st.DefinationName 'Status'	
	,NetWork_Layers =(SELECT DefinationName from AD_Definations where DefinationId=nls.BandId),x.SubmittedOn
	,x.SiteCode AS 'RedriveSite'
	,x.SubmittedOn
	,x.DriveCompletedOn
	,x.ReportSubmittedOn
	,'yes' As [Check]
	,isRedrive

	FROM AV_sites x  
	INNER JOIN	AD_Definations as d on x.CityId=d.DefinationId
	INNER JOIN  AD_Definations AS st ON st.DefinationId=x.Status
	inner join  AV_NetLayerStatus  AS nls ON x.SiteId=nls.SiteId
	WHERE SiteCode like '%-R1'  

	End