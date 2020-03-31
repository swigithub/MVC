 -- [dbo].[AV_GetBands] 'Get_All',648746 
CREATE PROCEDURE [dbo].[AV_GetBands]
@Filter NVARCHAR(50) 
,@SITEID numeric(18,0)
AS
BEGIN
		PRINT 'All'
	DECLARE @Scope AS NVARCHAR(50)
	
	SELECT @Scope=(SELECT ad.DefinationName FROM AD_Definations AS ad WHERE ad.IsActive=1 AND ad.DefinationId=as1.ScopeId)
	FROM AV_Sites AS as1
	WHERE as1.SiteId=@SITEID
	
--Select Distinct Band.DefinationName 'BandName', Sectors.BandId BandId, NM.DefinationName 'NetworkMode', Scope.DefinationName Scope, Carrier.DefinationName 'Carrier',
--Sectors.CarrierId,Sectors.NetworkModeId,Sectors.ScopeId
--,nls.ReceivedOn,nls.UploadedOn 'SubmittedOn',nls.ScheduledOn,nls.DriveCompletedOn,nls.CompletedOn,nls.STATUS, adsts.DefinationName 'StatusName',adsts.ColorCode 'StatusColor',
--ISNULL(nls.TesterId,0) 'TesterId', 
----ISNULL(awd.UserDeviceId,0) 'UserDeviceId',
--ISNULL(su.FirstName,'') + ' ' + ISNULL(su.LastName,'') 'TesterName',
--ISNULL((SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=ISNULL(nls.redriveTypeId,0)),'') 'RedriveType',
--ISNULL((SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=ISNULL(nls.redriveReasonId,0)),'') 'RedriveReason',
--ISNULL(nls.PWoRefID,'') 'PWoRefID',CAST(ISNULL(nls.isRedrive,0) AS BIT) 'isReDrive',as1.SiteCode

--from AV_Sectors Sectors
--INNER JOIN AV_Sites AS as1 ON as1.SiteId=sectors.SiteId
--Inner Join AD_Definations Band On Band.DefinationId = Sectors.BandId
--Inner Join AD_Definations NM On NM.DefinationId = Sectors.NetworkModeId
--Inner Join AD_Definations Scope On Scope.DefinationId = Sectors.ScopeId
--Inner Join AD_Definations Carrier On Carrier.DefinationId = Sectors.CarrierId
--INNER JOIN AV_NetLayerStatus AS nls ON nls.SiteId=Sectors.SiteId AND nls.NetworkModeId=sectors.NetworkModeId AND nls.BandId=Sectors.BandId AND nls.CarrierId=sectors.CarrierId
--LEFT JOIN AD_Definations AS adsts ON adsts.DefinationId=nls.Status
----LEFT JOIN AV_WoDevices AS awd ON awd.SiteId=sectors.SiteId AND awd.NetworkId=sectors.NetworkModeId AND awd.BandId=Sectors.BandId AND awd.CarrierId=sectors.CarrierId
-- --AND awd.ScopeId=sectors.ScopeId
-- LEFT OUTER JOIN Sec_Users AS su ON su.UserId=nls.TesterId
--Where Sectors.SiteId = @SITEID AND nls.IsActive=1
--ORDER BY Sectors.BandId

IF @Filter='Get_All'
BEGIN
	Select Distinct Band.DefinationName 'BandName', Sectors.BandId BandId, NM.DefinationName 'NetworkMode', Scope.DefinationName Scope, Carrier.DefinationName 'Carrier',
	Sectors.CarrierId,Sectors.NetworkModeId,Sectors.ScopeId
	,nls.ReceivedOn,nls.UploadedOn 'SubmittedOn',nls.ScheduledOn,nls.DriveCompletedOn,nls.CompletedOn,nls.STATUS, adsts.DefinationName 'StatusName',adsts.ColorCode 'StatusColor',
	ISNULL(nls.TesterId,0) 'TesterId', 
	--ISNULL(awd.UserDeviceId,0) 'UserDeviceId',
	ISNULL(su.FirstName,'') + ' ' + ISNULL(su.LastName,'') 'TesterName',
	ISNULL((SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=ISNULL(nls.redriveTypeId,0)),'') 'RedriveType',
	ISNULL((SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=ISNULL(nls.redriveReasonId,0)),'') 'RedriveReason',
	ISNULL(nls.PWoRefID,'') 'PWoRefID',CAST(ISNULL(nls.isRedrive,0) AS BIT) 'isReDrive',sit.SiteCode,mkt.DefinationName 'City',rgn.DefinationName 'Region',nls.IsActive,nls.LayerStatusId
	,cli.ClientPrefix
	from AV_Sectors Sectors
	inner join AV_Sites sit on sit.SiteId=Sectors.SiteId
	Inner Join AD_Definations Band On Band.DefinationId = Sectors.BandId
	Inner Join AD_Definations NM On NM.DefinationId = Sectors.NetworkModeId
	Inner Join AD_Definations Scope On Scope.DefinationId = Sectors.ScopeId
	Inner Join AD_Definations Carrier On Carrier.DefinationId = Sectors.CarrierId
	--INNER JOIN AD_Definations AS ntm ON ntm.DefinationId=sectors.NetworkModeId
	INNER JOIN AD_Definations AS mkt ON mkt.DefinationId=sit.CityId
	INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=mkt.PDefinationId
	INNER JOIN AV_NetLayerStatus AS nls ON nls.SiteId=Sectors.SiteId AND nls.NetworkModeId=sectors.NetworkModeId AND nls.BandId=Sectors.BandId AND nls.CarrierId=sectors.CarrierId
	LEFT JOIN AD_Definations AS adsts ON adsts.DefinationId=nls.Status
	--LEFT JOIN AV_WoDevices AS awd ON awd.SiteId=sectors.SiteId AND awd.NetworkId=sectors.NetworkModeId AND awd.BandId=Sectors.BandId AND awd.CarrierId=sectors.CarrierId
	 --AND awd.ScopeId=sectors.ScopeId
	 LEFT OUTER JOIN Sec_Users AS su ON su.UserId=nls.TesterId
 
	 INNER JOIN AD_Clients AS cli ON cli.ClientId=sit.ClientId
	Where Sectors.SiteId = @SITEID --AND nls.IsActive=1
	ORDER BY Sectors.BandId			
END
ELSE
BEGIN

	IF @Scope IN('SSV','NI','IND')
	BEGIN
		Select Distinct Band.DefinationName 'BandName', Sectors.BandId BandId, NM.DefinationName 'NetworkMode', Scope.DefinationName Scope, Carrier.DefinationName 'Carrier',
		Sectors.CarrierId,Sectors.NetworkModeId,Sectors.ScopeId
		,nls.ReceivedOn,nls.UploadedOn 'SubmittedOn',nls.ScheduledOn,nls.DriveCompletedOn,nls.CompletedOn,nls.STATUS, adsts.DefinationName 'StatusName',adsts.ColorCode 'StatusColor',
		ISNULL(nls.TesterId,0) 'TesterId', 
		--ISNULL(awd.UserDeviceId,0) 'UserDeviceId',
		ISNULL(su.FirstName,'') + ' ' + ISNULL(su.LastName,'') 'TesterName',
		ISNULL((SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=ISNULL(nls.redriveTypeId,0)),'') 'RedriveType',
		ISNULL((SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=ISNULL(nls.redriveReasonId,0)),'') 'RedriveReason',
		ISNULL(nls.PWoRefID,'') 'PWoRefID',CAST(ISNULL(nls.isRedrive,0) AS BIT) 'isReDrive',sit.SiteCode,mkt.DefinationName 'City',rgn.DefinationName 'Region',nls.IsActive,nls.LayerStatusId
		,cli.ClientPrefix
		from AV_Sectors Sectors
		inner join AV_Sites sit on sit.SiteId=Sectors.SiteId
		Inner Join AD_Definations Band On Band.DefinationId = Sectors.BandId
		Inner Join AD_Definations NM On NM.DefinationId = Sectors.NetworkModeId
		Inner Join AD_Definations Scope On Scope.DefinationId = Sectors.ScopeId
		Inner Join AD_Definations Carrier On Carrier.DefinationId = Sectors.CarrierId
		INNER JOIN AD_Definations AS ntm ON ntm.DefinationId=sectors.NetworkModeId
		INNER JOIN AD_Definations AS mkt ON mkt.DefinationId=sit.CityId
		INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=mkt.PDefinationId
		INNER JOIN AV_NetLayerStatus AS nls ON nls.SiteId=Sectors.SiteId AND nls.NetworkModeId=sectors.NetworkModeId AND nls.BandId=Sectors.BandId AND nls.CarrierId=sectors.CarrierId
		LEFT JOIN AD_Definations AS adsts ON adsts.DefinationId=nls.Status
		--LEFT JOIN AV_WoDevices AS awd ON awd.SiteId=sectors.SiteId AND awd.NetworkId=sectors.NetworkModeId AND awd.BandId=Sectors.BandId AND awd.CarrierId=sectors.CarrierId
		 --AND awd.ScopeId=sectors.ScopeId
		 LEFT OUTER JOIN Sec_Users AS su ON su.UserId=nls.TesterId
		 INNER JOIN AD_Clients AS cli ON cli.ClientId=sit.ClientId
		Where Sectors.SiteId = @SITEID AND nls.IsActive=1
		ORDER BY Sectors.BandId	
	END
	ELSE IF @Scope IN('TSS')
	BEGIN
		Select DISTINCT sit.SiteId, tsd.SurveyId, ss.SiteSurveyId, tsd.SurveyTitle, tsd.[Description]  ,sit.ScopeId, scp.DefinationName 'Scope',srcat.DefinationName 'SurveyCategory', sscat.DefinationName 'SurveySubCategory',
		nls.ReceivedOn,nls.UploadedOn 'SubmittedOn',nls.ScheduledOn,nls.DriveCompletedOn,nls.CompletedOn,nls.STATUS, adsts.DefinationName 'StatusName',adsts.ColorCode 'StatusColor',
		ISNULL(nls.TesterId,0) 'TesterId', styp.DefinationName 'SiteType',
		ISNULL(su.FirstName,'') + ' ' + ISNULL(su.LastName,'') 'TesterName',
		ISNULL((SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=ISNULL(nls.redriveTypeId,0)),'') 'RedriveType',
		ISNULL((SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=ISNULL(nls.redriveReasonId,0)),'') 'RedriveReason',
		ISNULL(nls.PWoRefID,'') 'PWoRefID',CAST(ISNULL(nls.isRedrive,0) AS BIT) 'isReDrive',sit.SiteCode,mkt.DefinationName 'City',rgn.DefinationName 'Region',nls.IsActive,nls.LayerStatusId
		,cli.ClientPrefix, '' 'ClientPOC', anls.NetworkModeId, anls.BandId, anls.CarrierId,
		'' 'NetworkMode', tsd.SurveyTitle 'BandName','' 'Carrier'
		from AV_Sites sit
		INNER JOIN TSS_SiteSurvey ss on ss.SiteId=sit.SiteId
		INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=sit.SiteId
		INNER JOIN TSS_SurveyDocuments AS tsd ON tsd.SurveyId=ss.SurveyId
		INNER JOIN AD_Definations scp On scp.DefinationId = sit.ScopeId
		INNER JOIN AD_Definations srcat On srcat.DefinationId = tsd.CategoryId
		INNER JOIN AD_Definations sscat On sscat.DefinationId = tsd.SubCategoryId
		INNER JOIN AD_Definations AS mkt ON mkt.DefinationId=sit.CityId
		INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=mkt.PDefinationId
		INNER JOIN AD_Definations AS styp ON styp.DefinationId=sit.SiteTypeId
		INNER JOIN AV_NetLayerStatus AS nls ON nls.SiteId=sit.SiteId and nls.SiteSurveyId=ss.SiteSurveyId
		LEFT JOIN AD_Definations AS adsts ON adsts.DefinationId=nls.Status
		LEFT OUTER JOIN Sec_Users AS su ON su.UserId=nls.TesterId
		INNER JOIN AD_Clients AS cli ON cli.ClientId=sit.ClientId
		WHERE sit.SiteId = @SITEID AND nls.IsActive=1
		ORDER BY tsd.SurveyId
	END
	ELSE IF @Scope IN('CLS')
	BEGIN
		Select DISTINCT sit.SiteId,ss.SiteClusterId, ss.ClusterId, ss.ClusterName, ''  'ClusterScope',ss.SiteCount 'SiteCount' ,sit.ScopeId, scp.DefinationName 'Scope',
		nls.ReceivedOn,nls.UploadedOn 'SubmittedOn',nls.ScheduledOn,nls.DriveCompletedOn,nls.CompletedOn,nls.STATUS, adsts.DefinationName 'StatusName',adsts.ColorCode 'StatusColor',
		ISNULL(nls.TesterId,0) 'TesterId', styp.DefinationName 'SiteType',
		ISNULL(su.FirstName,'') + ' ' + ISNULL(su.LastName,'') 'TesterName',
		ISNULL((SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=ISNULL(nls.redriveTypeId,0)),'') 'RedriveType',
		ISNULL((SELECT x.DefinationName FROM AD_Definations x WHERE x.DefinationId=ISNULL(nls.redriveReasonId,0)),'') 'RedriveReason',
		ISNULL(nls.PWoRefID,'') 'PWoRefID',CAST(ISNULL(nls.isRedrive,0) AS BIT) 'isReDrive',sit.SiteCode,mkt.DefinationName 'City',rgn.DefinationName 'Region',nls.IsActive,nls.LayerStatusId
		,cli.ClientPrefix, '' 'ClientPOC', nls.NetworkModeId, nls.BandId, nls.CarrierId,
		'' 'NetworkMode', '' 'BandName','' 'Carrier'
		from AV_Sites sit
		INNER JOIN AV_SiteClusters ss on ss.SiteId=sit.SiteId
		--INNER JOIN AV_NetLayerStatus AS anls ON anls.SiteId=sit.SiteId and anls.SiteClusterID=ss.SiteClusterId
		INNER JOIN AD_Definations scp On scp.DefinationId = sit.ScopeId
		INNER JOIN AD_Definations AS mkt ON mkt.DefinationId=sit.CityId
		INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=mkt.PDefinationId
		INNER JOIN AD_Definations AS styp ON styp.DefinationId=sit.SiteTypeId
		INNER JOIN AV_NetLayerStatus AS nls ON nls.SiteId=sit.SiteId  and nls.SiteClusterID=ss.SiteClusterId
		LEFT JOIN AD_Definations AS adsts ON adsts.DefinationId=nls.Status
		LEFT OUTER JOIN Sec_Users AS su ON su.UserId=nls.TesterId
		INNER JOIN AD_Clients AS cli ON cli.ClientId=sit.ClientId
		WHERE sit.SiteId = @SITEID AND nls.IsActive=1

	END
END

	
END