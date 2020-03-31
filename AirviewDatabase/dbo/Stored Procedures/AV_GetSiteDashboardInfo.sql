-- [dbo].[AV_GetSiteDashboardInfo] 22191,0,0,0,0,'Dashboard_Site_All'
-- [dbo].[AV_GetSiteDashboardInfo] 22191,15,74,181,36,'Dashboard_Site_Id'
-- [dbo].[AV_GetSiteDashboardInfo] 11036,15,74,181,36,'Dashboard_Site_Sector','Alpha'
CREATE PROCEDURE AV_GetSiteDashboardInfo
	@SiteId AS NUMERIC,
	@NetworkModeId AS INT=0,
	@BandId AS INT=0,
	@CarrierId AS INT=0,
	@ScopeId AS INT=0,
	@FilterOption AS NVARCHAR(25)
	,@Sector AS NVARCHAR(25)=null
AS

DECLARE @SectorId AS NUMERIC = 0

SELECT @SectorId=as1.SectorId FROM AV_Sectors AS as1
WHERE as1.SiteId=@SiteId AND as1.NetworkModeId=@NetworkModeId AND as1.BandId=@BandId AND as1.CarrierId=@CarrierId AND as1.SectorCode=@Sector
IF @FilterOption='Dashboard_Site_All'
BEGIN
	--Team Members
	SELECT DISTINCT nls.UploadedById 'UserId',ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') 'Username','Coordinator' 'UserType',su.Picture
	FROM AV_Sites AS sit
	inner join AV_NetLayerStatus AS nls ON sit.SiteId=nls.SiteId
	INNER JOIN Sec_Users AS su ON su.UserId=nls.UploadedById
	WHERE sit.SiteCode LIKE '%'+(SELECT CASE WHEN charindex('_',x.SiteCode)=0 AND charindex('-',x.SiteCode)=0 THEN x.SiteCode WHEN charindex('-',x.SiteCode)>0 THEN LEFT(x.SiteCode,charindex('-',x.SiteCode)-1) ELSE LEFT(x.SiteCode,charindex('_',x.SiteCode)-1) END from AV_Sites x where x.SiteId=@SiteId)+'%'
	AND su.UserId!=11
	UNION ALL
	SELECT DISTINCT nls.UploadedById 'UserId',ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') 'Username','Tester' 'UserType',su.Picture
	FROM AV_Sites AS sit
	inner join AV_NetLayerStatus AS nls ON sit.SiteId=nls.SiteId
	INNER JOIN Sec_Users AS su ON su.UserId=nls.TesterId
	WHERE sit.SiteCode LIKE '%'+(SELECT CASE WHEN charindex('_',x.SiteCode)=0 AND charindex('-',x.SiteCode)=0 THEN x.SiteCode WHEN charindex('-',x.SiteCode)>0 THEN LEFT(x.SiteCode,charindex('-',x.SiteCode)-1) ELSE LEFT(x.SiteCode,charindex('_',x.SiteCode)-1) END from AV_Sites x where x.SiteId=@SiteId)+'%'
	AND su.UserId!=11
	UNION ALL
	SELECT DISTINCT su.UserId 'UserId',ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') 'Username','Client POC' 'UserType',su.Picture
	FROM Sec_Users su
	INNER JOIN Sec_UserRoles sur ON sur.UserId=su.UserId
	INNER JOIN Sec_UserCities AS suc ON suc.UserId=su.UserId
	WHERE suc.CityId=(SELECT x.cityId FROM AV_Sites x where x.SiteId=@SiteId)
	AND sur.RoleId=10018 and su.IsActive=1
	UNION ALL
	SELECT DISTINCT su.UserId 'UserId',ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') 'Username','SWI POC' 'UserType',su.Picture
	FROM Sec_Users su
	INNER JOIN Sec_UserRoles sur ON sur.UserId=su.UserId
	INNER JOIN Sec_UserCities AS suc ON suc.UserId=su.UserId
	WHERE suc.CityId=(SELECT x.cityId FROM AV_Sites x where x.SiteId=@SiteId)
	AND sur.RoleId=10020 and su.IsActive=1

	--Client/Vendor Info
	SELECT ac.ClientName,ac.Logo,ad.DefinationName 'ClientType'
	FROM AV_Sites sit
	INNER JOIN AD_Clients AS ac ON ac.ClientId=sit.ClientId
	INNER JOIN AD_Definations AS ad ON ad.DefinationId=ac.ClientTypeId
	WHERE sit.IsActive=1 AND sit.SiteId=@SiteId
	UNION ALL
	SELECT ac.ClientName,ac.Logo,ad.DefinationName 'ClientType'
	FROM AV_Sites sit
	INNER JOIN AD_Clients AS ac ON ac.ClientId=(SELECT y.PClientId FROM AD_Clients AS y WHERE y.ClientId=(SELECT x.ClientId FROM AV_Sites x where x.SiteId=@SiteId))
	INNER JOIN AD_Definations AS ad ON ad.DefinationId=ac.ClientTypeId
	WHERE sit.IsActive=1 AND sit.SiteId=@SiteId


	--Throughtput Charts
	SELECT asts.[Site] 'SiteCode', 'Ping' 'TestType',NULL 'TimeStamp', ISNULL(MIN(asts.PingMinResult),0) 'MinResult', ISNULL(AVG(asts.PingAverageResult),0) 'AvgResult' ,ISNULL(MAX(asts.PingMaxResult),0) 'MaxResult', '' 'Sector'
	FROM AV_SiteTestSummary AS asts
	WHERE asts.[Site] LIKE '%'+(SELECT CASE WHEN charindex('_',x.SiteCode)=0 AND charindex('-',x.SiteCode)=0 THEN x.SiteCode WHEN charindex('-',x.SiteCode)>0 THEN LEFT(x.SiteCode,charindex('-',x.SiteCode)-1) ELSE LEFT(x.SiteCode,charindex('_',x.SiteCode)-1) END from AV_Sites x where x.SiteId=@SiteId)+'%'
	GROUP BY asts.[Site]
	
	SELECT asts.[Site] 'SiteCode', 'DL' 'TestType', NULL 'TimeStamp',ISNULL(MIN(asts.DownlinkMinResult),0) 'MinResult', ISNULL(AVG(asts.DownlinkAvgResult),0) 'AvgResult' , ISNULL(MAX(asts.DownlinkMaxResult),0) 'MaxResult', '' 'Sector'
	FROM AV_SiteTestSummary AS asts
	WHERE asts.[Site] LIKE '%'+(SELECT CASE WHEN charindex('_',x.SiteCode)=0 AND charindex('-',x.SiteCode)=0 THEN x.SiteCode WHEN charindex('-',x.SiteCode)>0 THEN LEFT(x.SiteCode,charindex('-',x.SiteCode)-1) ELSE LEFT(x.SiteCode,charindex('_',x.SiteCode)-1) END from AV_Sites x where x.SiteId=@SiteId)+'%'
	GROUP BY asts.[Site]	
	
	SELECT asts.[Site] 'SiteCode', 'UL' 'TestType', NULL 'TimeStamp',ISNULL(MIN(asts.UplinkMinResult),0) 'MinResult', ISNULL(AVG(asts.UplinkAvgResult),0) 'AvgResult' , ISNULL(MAX(asts.UplinkMaxResult),0) 'MaxResult', '' 'Sector'
	FROM AV_SiteTestSummary AS asts
	WHERE asts.[Site] LIKE '%'+(SELECT CASE WHEN charindex('_',x.SiteCode)=0 AND charindex('-',x.SiteCode)=0 THEN x.SiteCode WHEN charindex('-',x.SiteCode)>0 THEN LEFT(x.SiteCode,charindex('-',x.SiteCode)-1) ELSE LEFT(x.SiteCode,charindex('_',x.SiteCode)-1) END from AV_Sites x where x.SiteId=@SiteId)+'%'
	GROUP BY asts.[Site]

	--MO/MT Status
	SELECT '' 'SiteCode', '' 'Sector', NULL 'MoStatus', NULL 'MtStatus', NULL 'VMoStatus', NULL 'VMtStatus'

	--Handover Status
	SELECT '' 'SiteCode','' 'Sector', NULL 'PciId', NULL 'CwHandoverStatus', NULL 'Ccwhandoverstatus',NULL 'ICwHandoverStatus', NULL 'ICcwhandoverstatus'

	-- Ookla
	select '' 'SectorCode','' 'OklaPingResult','' 'OoklaDownlinkResult','' 'OoklaUplinkResult','' 'OoklaTestFilePath'
	
END
ELSE IF @FilterOption='Dashboard_Site_Id'
BEGIN
	--Team Members
	SELECT DISTINCT nls.UploadedById 'UserId',ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') 'Username','Coordinator' 'UserType',su.Picture
	FROM AV_Sites AS sit
	inner join AV_NetLayerStatus AS nls ON sit.SiteId=nls.SiteId
	INNER JOIN Sec_Users AS su ON su.UserId=nls.UploadedById
	WHERE sit.SiteId=@SiteId
	AND su.UserId!=11
	UNION ALL
	SELECT DISTINCT nls.UploadedById 'UserId',ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') 'Username','Tester' 'UserType',su.Picture
	FROM AV_Sites AS sit
	inner join AV_NetLayerStatus AS nls ON sit.SiteId=nls.SiteId
	INNER JOIN Sec_Users AS su ON su.UserId=nls.TesterId
	WHERE sit.SiteId=@SiteId
	AND su.UserId!=11
	UNION ALL
	SELECT DISTINCT su.UserId 'UserId',ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') 'Username','Client POC' 'UserType',su.Picture
	FROM Sec_Users su
	INNER JOIN Sec_UserRoles sur ON sur.UserId=su.UserId
	INNER JOIN Sec_UserCities AS suc ON suc.UserId=su.UserId
	WHERE suc.CityId=(SELECT x.cityId FROM AV_Sites x where x.SiteId=@SiteId)
	AND sur.RoleId=10018
	UNION ALL
	SELECT DISTINCT su.UserId 'UserId',ISNULL(su.FirstName,'')+' '+ISNULL(su.LastName,'') 'Username','SWI POC' 'UserType',su.Picture
	FROM Sec_Users su
	INNER JOIN Sec_UserRoles sur ON sur.UserId=su.UserId
	INNER JOIN Sec_UserCities AS suc ON suc.UserId=su.UserId
	WHERE suc.CityId=(SELECT x.cityId FROM AV_Sites x where x.SiteId=@SiteId)
	AND sur.RoleId=10020

	--Client/Vendor Info
	SELECT ac.ClientName,ac.Logo,ad.DefinationName 'ClientType'
	FROM AV_Sites sit
	INNER JOIN AD_Clients AS ac ON ac.ClientId=sit.ClientId
	INNER JOIN AD_Definations AS ad ON ad.DefinationId=ac.ClientTypeId
	WHERE sit.IsActive=1 AND sit.SiteId=@SiteId
	UNION ALL
	SELECT ac.ClientName,ac.Logo,ad.DefinationName 'ClientType'
	FROM AV_Sites sit
	INNER JOIN AD_Clients AS ac ON ac.ClientId=(SELECT y.PClientId FROM AD_Clients AS y WHERE y.ClientId=(SELECT x.ClientId FROM AV_Sites x where x.SiteId=@SiteId))
	INNER JOIN AD_Definations AS ad ON ad.DefinationId=ac.ClientTypeId
	WHERE sit.IsActive=1 AND sit.SiteId=@SiteId


	--Throughtput Charts
	SELECT asts.[Site] 'SiteCode', 'Ping' 'TestType','1900-01-01' 'TimeStamp', ISNULL(MIN(asts.PingMinResult),0) 'MinResult', ISNULL(AVG(asts.PingAverageResult),0) 'AvgResult' ,ISNULL(MAX(asts.PingMaxResult),0) 'MaxResult', asts.Sector
	FROM AV_SiteTestSummary AS asts
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId
	GROUP BY asts.[Site], asts.Sector
	
	SELECT asts.[Site] 'SiteCode', 'DL' 'TestType', '1900-01-01' 'TimeStamp',ISNULL(MIN(asts.DownlinkMinResult),0) 'MinResult', ISNULL(AVG(asts.DownlinkAvgResult),0) 'AvgResult' , ISNULL(MAX(asts.DownlinkMaxResult),0) 'MaxResult', asts.Sector
	FROM AV_SiteTestSummary AS asts
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId
	GROUP BY asts.[Site], asts.Sector	
	
	SELECT asts.[Site] 'SiteCode', 'UL' 'TestType', '1900-01-01' 'TimeStamp',ISNULL(MIN(asts.UplinkMinResult),0) 'MinResult', ISNULL(AVG(asts.UplinkAvgResult),0) 'AvgResult' , ISNULL(MAX(asts.UplinkMaxResult),0) 'MaxResult', asts.Sector
	FROM AV_SiteTestSummary AS asts
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId
	GROUP BY asts.[Site], asts.Sector
	
	

	--MO/MT Status
	SELECT asts.[Site] 'SiteCode', asts.Sector, asts.MoStatus, asts.MtStatus, asts.VMoStatus, asts.VMtStatus
	FROM AV_SiteTestSummary AS asts
	WHERE asts.[SiteId]=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId

	--Handover Status
	SELECT asts.[Site] 'SiteCode', asts.Sector, asts.PciId, asts.CwHandoverStatus, asts.Ccwhandoverstatus,
		   asts.ICwHandoverStatus, asts.ICcwhandoverstatus
	FROM AV_SiteTestSummary AS asts
	WHERE asts.[SiteId]=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId


	-- Ookla
	select sts.Sector 'SectorCode',sts.OoklaPingResult 'OoklaPingResult',sts.OoklaDownlinkResult 'OoklaDownlinkResult',sts.OoklaUplinkResult 'OoklaUplinkResult',sts.OoklaTestFilePath 'OoklaTestFilePath'
    from AV_SiteTestSummary sts where sts.SiteId=@SiteId AND sts.NetworkModeId=@NetworkModeId AND sts.BandId=@BandId AND sts.CarrierId=@CarrierId
END
ELSE IF @FilterOption='Dashboard_Site_Sector'
BEGIN
	SELECT asts.[Site] 'SiteCode', 'Ping' 'TestType',asts.[TimeStamp] 'TimeStamp', 0 'MinResult', 0 'AvgResult' , ISNULL(asts.TestResult,0) 'MaxResult', asts.Sector
	FROM AV_SiteTestlog AS asts
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId AND asts.Sector=@Sector AND asts.TestType='Ping'	
	AND asts.IsActive=1
	
	SELECT asts.[Site] 'SiteCode', 'DL' 'TestType',asts.[TimeStamp] 'TimeStamp', 0 'MinResult', 0 'AvgResult' , ISNULL(asts.TestResult,0) 'MaxResult', asts.Sector
	FROM AV_SiteTestlog AS asts
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId AND asts.Sector=@Sector AND asts.TestType='DL'
	AND asts.IsActive=1
		
	SELECT asts.[Site] 'SiteCode', 'UL' 'TestType',asts.[TimeStamp] 'TimeStamp', 0 'MinResult', 0 'AvgResult' , ISNULL(asts.TestResult,0) 'MaxResult', asts.Sector
	FROM AV_SiteTestlog AS asts
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId AND asts.Sector=@Sector AND asts.TestType='UL'
	AND asts.IsActive=1


	SELECT asts.TestLatitude,asts.TestLongitude,asts.NetworkMode,asts.Band,asts.Carrier,
	CASE WHEN asts.NetworkMode='LTE' then asts.LteRsrp
		 WHEN asts.NetworkMode='WCDMA' then asts.WcdmaRscp
		 WHEN asts.NetworkMode='GSM' then asts.GsmRssi
	END 'SignalStrength',
	CASE WHEN asts.NetworkMode='LTE' then asts.LteRsrq
		 WHEN asts.NetworkMode='WCDMA' then asts.WcdmaEcio
		 WHEN asts.NetworkMode='GSM' then asts.GsmRxQual
	END 'SignalQuality',
	CASE WHEN asts.NetworkMode='LTE' then asts.LteRsnr
		 WHEN asts.NetworkMode='WCDMA' then 0
		 WHEN asts.NetworkMode='GSM' then 0
	END 'SignalNoise'
	from AV_SiteTestSummary asts
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId AND asts.Sector=@Sector
	
	SELECT asts.PCI
	INTO #tmpPCI
	FROM AV_Sectors AS asts
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId 
	
	SELECT asts.[TimeStamp],asts.PciId 'PCI',
	CASE WHEN asts.NetworkMode='LTE' then asts.LteRsrp
		 WHEN asts.NetworkMode='WCDMA' then asts.WcdmaRscp
		 WHEN asts.NetworkMode='GSM' then asts.GsmRssi
	ELSE 0
	END 'SignalStrength',ISNULL(sec.sectorColor,'#87898A') 'ColorCode'
	FROM AV_SiteTestLog AS asts
	INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId AND sec.PCI=asts.PciId
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId AND asts.SectorId=@SectorId AND asts.TestType='IDLE'
	UNION ALL
	SELECT asts.[TimeStamp],asts.PCI1 'PCI',
	asts.SS1 'SignalStrength',ISNULL(sec.sectorColor,'#87898A') 'ColorCode'
	FROM AV_NeighbourLogs AS asts
	INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId --AND sec.PCI=asts.Pci1
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId AND asts.SectorId=@SectorId
	AND asts.PCI1>0 AND asts.PCI1 IN(SELECT x.PCI FROM #tmpPCI x)
	UNION ALL
	SELECT asts.[TimeStamp],asts.PCI2 'PCI',
	asts.SS2 'SignalStrength',ISNULL(sec.sectorColor,'#87898A') 'ColorCode'
	FROM AV_NeighbourLogs AS asts
	INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId --AND sec.PCI=asts.Pci1
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId AND asts.SectorId=@SectorId
	AND asts.PCI2>0  AND asts.PCI2 IN(SELECT x.PCI FROM #tmpPCI x)
	UNION ALL
	SELECT asts.[TimeStamp],asts.PCI3 'PCI',
	asts.SS3 'SignalStrength',ISNULL(sec.sectorColor,'#87898A') 'ColorCode'
	FROM AV_NeighbourLogs AS asts
	INNER JOIN AV_Sectors AS sec ON sec.SiteId=asts.SiteId AND sec.NetworkModeId=asts.NetworkModeId AND sec.BandId=asts.BandId AND sec.CarrierId=asts.CarrierId --AND sec.PCI=asts.Pci1
	WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId AND asts.SectorId=@SectorId
	AND asts.PCI3>0 AND asts.PCI3 IN(SELECT x.PCI FROM #tmpPCI x)
	--start mo
	SELECT asts.BandId,asts.Band, asts.NetworkModeId,asts.NetworkMode, asts.[Site] 'SiteCode',asts.TestType,asts.[TimeStamp] 'TimeStamp', asts.Sector
	FROM AV_SiteTestlog AS asts 
		WHERE asts.SiteId=@SiteId AND asts.NetworkModeId=@NetworkModeId AND asts.BandId=@BandId AND asts.CarrierId=@CarrierId AND asts.Sector=@Sector and asts.TestType='MO' OR asts.TestType='MT' OR asts.TestType='SMO' OR asts.TestType='SMT' OR asts.TestType='IRAT'
	AND asts.IsActive=1
	--end mo
	DROP TABLE #tmpPCI
	
END