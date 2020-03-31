
CREATE PROCEDURE [dbo].[AV_GetWorkOrder] 
@Filter nvarchar(50),
@value1 nvarchar(50)=null,
@value2 nvarchar(50)=null,
@value3 nvarchar(50)=null,
@value4 nvarchar(50)=null,
@Column nvarchar(500) = '*'
	--@TesterId numeric(18, 0),
	--@Status numeric(18, 0),
	--@IMEI nvarchar(50) = NULL
AS

DECLARE @sql as nvarchar(max)=''

BEGIN

if @Filter ='WorkOrderReport' begin
-- @value1 for TesterId
-- @value2 for Site.Status
-- @value3 for IMEI
	IF EXISTS(select * from Sec_UserDevices where IMEI=@value3 and UserId=@value1) 
	--BEGIN
		--IF (@Value1 NOT IN(10053,10124))
	 --  	BEGIN
		--	SELECT x.*
		--	FROM
		--	(
		--		Select sit.SiteId,sit.SiteCode,sit.ClusterId,usr.UserId 'TesterId',usr.FirstName+' '+ usr.LastName 'Tester' , Latitude,Longitude,
		--		sec.SectorId,sec.SectorCode,cl.ClientId,cl.ClientName 'Client',net.DefinationId 'NetTypeId',net.DefinationName 'NetType',
		--		band.DefinationId 'BandId',band.DefinationName 'Band',crier.DefinationId 'CarrierId',
		--		 crier.DefinationName 'Carrier',scop.DefinationId 'ScopeId',
		--		 scop.DefinationName 'Scope',Antenna,BeamWidth,Azimuth,PCI,sit.WoRefId 'WoRefId',market.DefinationName 'Market'
		--		from AV_Sites sit		
		--		INNER Join AV_Sectors As sec ON sit.SiteId = sec.SiteId
		--		INNER JOIN AV_NetLayerStatus AS nls ON nls.SiteId=sit.SiteId AND nls.NetworkModeId=sec.NetworkModeId AND nls.BandId=sec.BandId AND nls.CarrierId=sec.CarrierId
		--		INNER join AD_Definations crier on crier.DefinationId =nls.CarrierId 
		--		INNER join AD_Definations scop on scop.DefinationId =nls.ScopeId
		--		INNER join AD_Definations net on net.DefinationId =nls.NetworkModeId
		--		INNER join AD_Definations band on band.DefinationId =nls.BandId
		--		INNER join AD_Definations market on market.DefinationId =sit.CityId
		--		INNER join Sec_Users usr on usr.UserId= nls.TesterId
		--		INNER join AD_Clients cl on cl.ClientId=sit.ClientId
		--		INNER JOIN AV_WoDevices dvc ON dvc.[SiteId]=sec.SiteId AND dvc.NetworkId=sec.NetworkModeId AND dvc.BandId=sec.BandId AND dvc.CarrierId=sec.CarrierId
		--		INNER JOIN Sec_UserDevices AS sud ON sud.DeviceId=dvc.UserDeviceId
		--		where nls.TesterId=@value1 and nls.Status=@value2 and nls.IsPublished=1 AND sit.IsActive=1 AND nls.IsActive=1
		--		and dvc.IsDownlaoded=0 AND dvc.UserId=@value1 AND sud.IMEI=@value3
		--		group by sit.SiteId,sit.ClusterId,SiteCode,usr.UserId,usr.FirstName+' '+ usr.LastName  , 
		--		Latitude,Longitude,sec.SectorId,SectorCode,cl.ClientId,cl.ClientName,net.DefinationId,net.DefinationName,
		--		band.DefinationId,band.DefinationName,crier.DefinationId,
		--		crier.DefinationName,scop.DefinationId,scop.DefinationName ,Antenna,BeamWidth,Azimuth,PCI,sit.WoRefId,sit.WoCode,market.DefinationName
		--	 ) x
		--	 ORDER BY  x.SiteId,x.Scope,x.NetType,x.Band,x.Carrier,x.Azimuth
	 --  	END
	 --  	ELSE
	   	BEGIN
	   		
	   		--SELECT @value1,@value3
			SELECT x.*
			FROM
			(
				Select sit.SiteId,sit.SiteCode,sit.ClusterId,usr.UserId 'TesterId',usr.FirstName+' '+ usr.LastName 'Tester' , Latitude,Longitude,
				sec.SectorId,sec.SectorCode,cl.ClientId,cl.ClientName 'Client',ISNULL(net.DefinationId,0) 'NetTypeId',net.DefinationName 'NetType',
				ISNULL(band.DefinationId,0) 'BandId',band.DefinationName 'Band',ISNULL(crier.DefinationId,0) 'CarrierId',
				 crier.DefinationName 'Carrier',scop.DefinationId 'ScopeId',
				 scop.DefinationName 'Scope',Antenna,BeamWidth,Azimuth,PCI,sit.WoRefId 'WoRefId',market.DefinationName 'Market', sit.ScheduledOn
				 ,sec.SectorLatitude,sec.SectorLongitude,sts.DefinationName 'WOStatus', sts.ColorCode,sit.SiteTypeId,sittyp.DefinationName 'SiteType', sit.ProjectId,
				 1 'RecieverDistance', 
				 --ROUND(((ISNULL(sec.RFHeight,82)-6)/(((ISNULL(sec.ETilt,2)+ISNULL(sec.MTilt,2))+(ISNULL(sec.VerticalBeamwidth,2)/2))*0.01745506492))*0.0003048,2) 'InnerDistance', ROUND(((ISNULL(sec.RFHeight,82)-6)/(((ISNULL(sec.ETilt,2)+ISNULL(sec.MTilt,2))-(ISNULL(sec.VerticalBeamwidth,2)/2))*0.01745506492))*0.0003048,2) 'OuterDistance',
				 ROUND(((case when sec.RFHeight is null or sec.RFHeight = 0 then 82 else sec.RFHeight end-6)/(((ISNULL(sec.ETilt,2)+
				 case when  sec.MTilt is null or  sec.MTilt= 0 then  2 else  sec.MTilt end )+
				 (case when sec.VerticalBeamwidth is null or sec.VerticalBeamwidth = 0 then 2 else  sec.VerticalBeamwidth end /2))*0.01745506492))*0.0003048,2) 'InnerDistance',
				  ROUND(
				  ((case when sec.RFHeight is null or sec.RFHeight = 0 then 82 else  sec.RFHeight end -6)
				  /
				  ((
				  (case when sec.ETilt is null or sec.ETilt = 0 then 2 else  sec.ETilt end 
				  +case when sec.MTilt is null or sec.MTilt = 0 then 2 else sec.MTilt end) - (case when sec.VerticalBeamwidth is null or sec.VerticalBeamwidth = 0 then  2 else sec.VerticalBeamwidth end /2))
				  *
				  0.01745506492))*0.0003048,2) 'OuterDistance',
				sec.CellId,sec.RFHeight,sec.AntennaDownTilt,sec.VerticalBeamwidth				 
				from AV_Sites sit		
				INNER Join AV_Sectors As sec ON sit.SiteId = sec.SiteId
				INNER JOIN AV_NetLayerStatus AS nls ON nls.SiteId=sit.SiteId AND nls.NetworkModeId=sec.NetworkModeId AND nls.BandId=sec.BandId AND nls.CarrierId=sec.CarrierId
				LEFT join AD_Definations crier on crier.DefinationId =nls.CarrierId 
				INNER join AD_Definations scop on scop.DefinationId =nls.ScopeId
				INNER join AD_Definations sts on sts.DefinationId =sit.[Status]
				LEFT join AD_Definations net on net.DefinationId =nls.NetworkModeId
				LEFT join AD_Definations band on band.DefinationId =nls.BandId
				INNER join AD_Definations market on market.DefinationId =sit.CityId
				INNER join Sec_Users usr on usr.UserId= nls.TesterId
				INNER join AD_Clients cl on cl.ClientId=sit.ClientId
				INNER JOIN AV_WoDevices dvc ON dvc.[SiteId]=sec.SiteId AND dvc.NetworkId=sec.NetworkModeId AND dvc.BandId=sec.BandId AND dvc.CarrierId=sec.CarrierId
				INNER JOIN Sec_UserDevices AS sud ON sud.DeviceId=dvc.UserDeviceId
				LEFT join AD_Definations sittyp on sittyp.DefinationId =sit.SiteTypeId
				where nls.TesterId=@value1 and nls.IsPublished=1 AND sit.IsActive=1 AND nls.IsActive=1
				and dvc.IsDownlaoded=0 AND dvc.UserId=@value1 AND sud.IMEI=@value3
				group by sit.SiteId,sit.ClusterId,SiteCode,usr.UserId,usr.FirstName+' '+ usr.LastName  , Latitude,Longitude,sec.SectorId,SectorCode,cl.ClientId,cl.ClientName,net.DefinationId,net.DefinationName,
				band.DefinationId,band.DefinationName,crier.DefinationId,
				crier.DefinationName,scop.DefinationId,scop.DefinationName ,Antenna,BeamWidth,Azimuth,PCI,sit.WoRefId,sit.WoCode,market.DefinationName, sit.ScheduledOn,sec.SectorLatitude,sec.SectorLongitude,sts.DefinationName,sts.ColorCode
				,sit.SiteTypeId,sittyp.DefinationName,sit.ProjectId,sec.CellId,sec.RFHeight,sec.ETilt,sec.MTilt,sec.VerticalBeamwidth,sec.AntennaDownTilt
			 ) x
			 ORDER BY  x.SiteId,x.Scope,x.NetType,x.Band,x.Carrier,x.Azimuth
	   	END
	 --end
	 ELSE 
	 BEGIN 
		 select Message='Invalid IEMI or Tester'
	 END 
END
else if @Filter ='WorkOrderReportbyProjectId' begin
-- @value1 for TesterId
-- @value2 for Site.Status
-- @value3 for IMEI
	IF EXISTS(select * from Sec_UserDevices where IMEI=@value3 and UserId=@value1) 
	--BEGIN
		--IF (@Value1 NOT IN(10053,10124))
	 --  	BEGIN
		--	SELECT x.*
		--	FROM
		--	(
		--		Select sit.SiteId,sit.SiteCode,sit.ClusterId,usr.UserId 'TesterId',usr.FirstName+' '+ usr.LastName 'Tester' , Latitude,Longitude,
		--		sec.SectorId,sec.SectorCode,cl.ClientId,cl.ClientName 'Client',net.DefinationId 'NetTypeId',net.DefinationName 'NetType',
		--		band.DefinationId 'BandId',band.DefinationName 'Band',crier.DefinationId 'CarrierId',
		--		 crier.DefinationName 'Carrier',scop.DefinationId 'ScopeId',
		--		 scop.DefinationName 'Scope',Antenna,BeamWidth,Azimuth,PCI,sit.WoRefId 'WoRefId',market.DefinationName 'Market'
		--		from AV_Sites sit		
		--		INNER Join AV_Sectors As sec ON sit.SiteId = sec.SiteId
		--		INNER JOIN AV_NetLayerStatus AS nls ON nls.SiteId=sit.SiteId AND nls.NetworkModeId=sec.NetworkModeId AND nls.BandId=sec.BandId AND nls.CarrierId=sec.CarrierId
		--		INNER join AD_Definations crier on crier.DefinationId =nls.CarrierId 
		--		INNER join AD_Definations scop on scop.DefinationId =nls.ScopeId
		--		INNER join AD_Definations net on net.DefinationId =nls.NetworkModeId
		--		INNER join AD_Definations band on band.DefinationId =nls.BandId
		--		INNER join AD_Definations market on market.DefinationId =sit.CityId
		--		INNER join Sec_Users usr on usr.UserId= nls.TesterId
		--		INNER join AD_Clients cl on cl.ClientId=sit.ClientId
		--		INNER JOIN AV_WoDevices dvc ON dvc.[SiteId]=sec.SiteId AND dvc.NetworkId=sec.NetworkModeId AND dvc.BandId=sec.BandId AND dvc.CarrierId=sec.CarrierId
		--		INNER JOIN Sec_UserDevices AS sud ON sud.DeviceId=dvc.UserDeviceId
		--		where nls.TesterId=@value1 and nls.Status=@value2 and nls.IsPublished=1 AND sit.IsActive=1 AND nls.IsActive=1
		--		and dvc.IsDownlaoded=0 AND dvc.UserId=@value1 AND sud.IMEI=@value3
		--		group by sit.SiteId,sit.ClusterId,SiteCode,usr.UserId,usr.FirstName+' '+ usr.LastName  , 
		--		Latitude,Longitude,sec.SectorId,SectorCode,cl.ClientId,cl.ClientName,net.DefinationId,net.DefinationName,
		--		band.DefinationId,band.DefinationName,crier.DefinationId,
		--		crier.DefinationName,scop.DefinationId,scop.DefinationName ,Antenna,BeamWidth,Azimuth,PCI,sit.WoRefId,sit.WoCode,market.DefinationName
		--	 ) x
		--	 ORDER BY  x.SiteId,x.Scope,x.NetType,x.Band,x.Carrier,x.Azimuth
	 --  	END
	 --  	ELSE
	   	BEGIN
	   		
	   		--SELECT @value1,@value3
			SELECT x.*
			FROM
			(
				Select sit.SiteId,sit.SiteCode,sit.ClusterId,usr.UserId 'TesterId',usr.FirstName+' '+ usr.LastName 'Tester' , Latitude,Longitude,
				sec.SectorId,sec.SectorCode,cl.ClientId,cl.ClientName 'Client',ISNULL(net.DefinationId,0) 'NetTypeId',net.DefinationName 'NetType',
				ISNULL(band.DefinationId,0) 'BandId',band.DefinationName 'Band',ISNULL(crier.DefinationId,0) 'CarrierId',
				 crier.DefinationName 'Carrier',scop.DefinationId 'ScopeId',
				 scop.DefinationName 'Scope',Antenna,BeamWidth,Azimuth,PCI,sit.WoRefId 'WoRefId',market.DefinationName 'Market', sit.ScheduledOn
				 ,sec.SectorLatitude,sec.SectorLongitude,sts.DefinationName 'WOStatus', sts.ColorCode,sit.SiteTypeId,sittyp.DefinationName 'SiteType', sit.ProjectId,
				 1 'RecieverDistance', 
				 --ROUND(((ISNULL(sec.RFHeight,82)-6)/(((ISNULL(sec.ETilt,2)+ISNULL(sec.MTilt,2))+(ISNULL(sec.VerticalBeamwidth,2)/2))*0.01745506492))*0.0003048,2) 'InnerDistance', ROUND(((ISNULL(sec.RFHeight,82)-6)/(((ISNULL(sec.ETilt,2)+ISNULL(sec.MTilt,2))-(ISNULL(sec.VerticalBeamwidth,2)/2))*0.01745506492))*0.0003048,2) 'OuterDistance',
				 ROUND(((case when sec.RFHeight is null or sec.RFHeight = 0 then 82 else sec.RFHeight end-6)/(((ISNULL(sec.ETilt,2)+
				 case when  sec.MTilt is null or  sec.MTilt= 0 then  2 else  sec.MTilt end )+
				 (case when sec.VerticalBeamwidth is null or sec.VerticalBeamwidth = 0 then 2 else  sec.VerticalBeamwidth end /2))*0.01745506492))*0.0003048,2) 'InnerDistance',
				  ROUND(
				  ((case when sec.RFHeight is null or sec.RFHeight = 0 then 82 else  sec.RFHeight end -6)
				  /
				  ((
				  (case when sec.ETilt is null or sec.ETilt = 0 then 2 else  sec.ETilt end 
				  +case when sec.MTilt is null or sec.MTilt = 0 then 2 else sec.MTilt end) - (case when sec.VerticalBeamwidth is null or sec.VerticalBeamwidth = 0 then  2 else sec.VerticalBeamwidth end /2))
				  *
				  0.01745506492))*0.0003048,2) 'OuterDistance',
				sec.CellId,sec.RFHeight,sec.AntennaDownTilt,sec.VerticalBeamwidth				 
				from AV_Sites sit		
				INNER Join AV_Sectors As sec ON sit.SiteId = sec.SiteId
				INNER JOIN AV_NetLayerStatus AS nls ON nls.SiteId=sit.SiteId AND nls.NetworkModeId=sec.NetworkModeId AND nls.BandId=sec.BandId AND nls.CarrierId=sec.CarrierId
				LEFT join AD_Definations crier on crier.DefinationId =nls.CarrierId 
				INNER join AD_Definations scop on scop.DefinationId =nls.ScopeId
				INNER join AD_Definations sts on sts.DefinationId =sit.[Status]
				LEFT join AD_Definations net on net.DefinationId =nls.NetworkModeId
				LEFT join AD_Definations band on band.DefinationId =nls.BandId
				INNER join AD_Definations market on market.DefinationId =sit.CityId
				INNER join Sec_Users usr on usr.UserId= nls.TesterId
				INNER join AD_Clients cl on cl.ClientId=sit.ClientId
				INNER JOIN AV_WoDevices dvc ON dvc.[SiteId]=sec.SiteId AND dvc.NetworkId=sec.NetworkModeId AND dvc.BandId=sec.BandId AND dvc.CarrierId=sec.CarrierId
				INNER JOIN Sec_UserDevices AS sud ON sud.DeviceId=dvc.UserDeviceId
				LEFT join AD_Definations sittyp on sittyp.DefinationId =sit.SiteTypeId
				where nls.TesterId=@value1 and nls.IsPublished=1 AND sit.IsActive=1 AND nls.IsActive=1
				and dvc.IsDownlaoded=0 AND dvc.UserId=@value1 AND sud.IMEI=@value3 and sit.ProjectId=@value4
				group by sit.SiteId,sit.ClusterId,SiteCode,usr.UserId,usr.FirstName+' '+ usr.LastName  , Latitude,Longitude,sec.SectorId,SectorCode,cl.ClientId,cl.ClientName,net.DefinationId,net.DefinationName,
				band.DefinationId,band.DefinationName,crier.DefinationId,
				crier.DefinationName,scop.DefinationId,scop.DefinationName ,Antenna,BeamWidth,Azimuth,PCI,sit.WoRefId,sit.WoCode,market.DefinationName, sit.ScheduledOn,sec.SectorLatitude,sec.SectorLongitude,sts.DefinationName,sts.ColorCode
				,sit.SiteTypeId,sittyp.DefinationName,sit.ProjectId,sec.CellId,sec.RFHeight,sec.ETilt,sec.MTilt,sec.VerticalBeamwidth,sec.AntennaDownTilt
			 ) x
			 ORDER BY  x.SiteId,x.Scope,x.NetType,x.Band,x.Carrier,x.Azimuth
	   	END
	 --end
	 ELSE 
	 BEGIN 
		 select Message='Invalid IEMI or Tester'
	 END 
END


-- [dbo].[AV_GetWorkOrder] 'bySiteId',10946
ELSE if @Filter ='bySiteId'
begin
-- @value1 for SiteId
	SELECT x.*
	FROM
	(
		Select sit.SiteId,SiteCode,sit.ClusterId,usr.UserId 'TesterId',usr.FirstName+' '+ usr.LastName 'Tester' , Latitude,Longitude,sec.SectorId,SectorCode,cl.ClientId,cl.ClientName 'Client',net.DefinationId 'NetTypeId',net.DefinationName 'NetType',
		band.DefinationId 'BandId',band.DefinationName 'Band',crier.DefinationId 'CarrierId',
		 crier.DefinationName 'Carrier',scop.DefinationId 'ScopeId',
		 scop.DefinationName 'Scope',Antenna,BeamWidth,Azimuth,PCI,sit.WoRefId 'WoRefId', sit.ScheduledOn,market.DefinationName 'Market',stt.DefinationName 'WOStatus',stt.ColorCode
		 ,sec.SectorLatitude,sec.SectorLongitude,sit.SiteTypeId,sittyp.DefinationName 'SiteType', sit.ProjectId,
		1 'RecieverDistance', 
		ROUND(((ISNULL(case when  sec.RFHeight=0 then 82 else sec.RFHeight end,82)-6)/(((ISNULL(case when sec.ETilt=0 then 2 else sec.ETilt end,2)+ISNULL(case when sec.MTilt=0 then 2 else sec.MTilt end,2))+(ISNULL(case when sec.VerticalBeamwidth=0 then 2 else sec.VerticalBeamwidth end,2)/2))*0.01745506492))*0.0003048,2) 'InnerDistance',
		ROUND(((ISNULL(case when  sec.RFHeight=0 then 82 else sec.RFHeight end,82)-6)/(((ISNULL(case when sec.ETilt=0 then 2 else sec.ETilt end,2)+ISNULL(case when sec.MTilt=0 then 2 else sec.MTilt end,2))-(ISNULL(case when sec.VerticalBeamwidth=0 then 2 else sec.VerticalBeamwidth end,2)/2))*0.01745506492))*0.0003048,2) 'OuterDistance',
		sec.CellId,sec.RFHeight,sec.AntennaDownTilt,sec.VerticalBeamwidth				 
		from AV_Sites sit
		Left Join (Select Distinct * from AV_Sectors) As sec ON sit.SiteId = sec.SiteId		
		left join AD_Definations crier on crier.DefinationId =CarrierId 
		
		left join AD_Definations net on net.DefinationId =NetworkModeId
		left join AD_Definations band on band.DefinationId =sec.BandId	
		INNER JOIN AV_NetLayerStatus AS nls ON nls.SiteId=sit.SiteId AND nls.NetworkModeId=sec.NetworkModeId AND nls.BandId=sec.BandId AND nls.CarrierId=sec.CarrierId
		left join AD_Definations scop on scop.DefinationId =nls.ScopeId
		INNER join AD_Definations market on market.DefinationId =sit.CityId
		inner join AD_Definations stt on stt.DefinationId =sit.[Status]
		left join AD_Clients cl on cl.ClientId=sit.ClientId
		INNER JOIN AV_WoDevices dvc ON dvc.[SiteId]=sec.SiteId AND dvc.NetworkId=sec.NetworkModeId AND dvc.BandId=sec.BandId AND dvc.CarrierId=sec.CarrierId	
		INNER JOIN Sec_UserDevices AS sud ON sud.DeviceId=dvc.UserDeviceId 
		left join Sec_Users usr on usr.UserId= dvc.UserId
		LEFT join AD_Definations sittyp on sittyp.DefinationId =sit.SiteTypeId
		where  sit.SiteId=@value1 --AND dvc.UserId=@value2 AND usr.UserId=@value2
		 AND sit.IsActive=1 AND nls.IsActive=1
		group by sit.SiteId,sit.ClusterId,SiteCode,usr.UserId,usr.FirstName+' '+ usr.LastName  , Latitude,Longitude,sec.SectorId,SectorCode,cl.ClientId,cl.ClientName,net.DefinationId,net.DefinationName,
		band.DefinationId,band.DefinationName,crier.DefinationId,
		crier.DefinationName,scop.DefinationId,scop.DefinationName ,Antenna,BeamWidth,Azimuth,PCI,sit.WoRefId,sit.WoCode, sit.ScheduledOn, market.DefinationName,sec.SectorLatitude,sec.SectorLongitude,
		stt.DefinationName,stt.ColorCode,sit.SiteTypeId,sittyp.DefinationName,sit.ProjectId,sec.CellId,sec.RFHeight,sec.ETilt,sec.MTilt,sec.VerticalBeamwidth,sec.AntennaDownTilt
	 ) x
	 ORDER BY x.Scope,x.NetType,x.Band,x.Carrier,x.Azimuth
END

--	[dbo].[AV_GetWorkOrder] 'Search', '((Market = ''Orlando'' AND Region = ''South'' AND SubmittedOn >= ''11/20/2017''  ))'

ELSE IF @Filter='Search'
BEGIN
	SET @sql='SELECT * FROM (Select Cast(WoRefId As nvarchar(100)) As WoRefNo,  SiteId, SiteCode, Latitude, Longitude, AV_Sites.ClusterId, 
	AV_Sites.ClientId, Description,AV_Sites.Status, AV_Sites.SubmittedOn SubmittedOn, AV_Sites.ScheduledOn AssignedOn, 
	AV_Sites.CompletedOn,AV_Sites.ScheduledOn, AV_Sites.ReceivedOn,AD_Definations.DefinationName AS Market, DEF.DefinationName AS Region,
	users.Picture TesterPicture,AD_Clients.ClientName,users.UserId TesterId,AV_Sites.IsDownloaded,sts.DefinationName StatusName,
	AV_Sites.DriveCompletedOn, sts.KeyCode StatusKeyCode,AV_Sites.ReportSubmittedOn,AV_Sites.IsActive,scp.DefinationName AS Scope 
	from AV_Sites 
	LEFT JOIN AV_Clusters ON AV_Clusters.ClusterId = AV_Sites.ClusterId
	INNER JOIN AD_Clients ON AD_Clients.ClientId = AV_Sites.ClientId 
	INNER JOIN AD_Definations ON AD_Definations.DefinationId = AV_Clusters.CityId 
	INNER JOIN AD_Definations As DEF ON DEF.DefinationId = AD_Definations.PDefinationId
	INNER JOIN AD_Definations scp ON scp.DefinationId = AV_Sites.ScopeId 
	LEFT OUTER Join Sec_Users users ON users.UserId = AV_Sites.TesterId 
	INNER JOIN AD_Definations AS sts ON sts.DefinationId=AV_Sites.[Status]	
	WHERE AV_Sites.IsActive=1) x WHERE '+ @value1 + ' Order By x.SubmittedOn ASC'

	
	EXEC(@sql) 
	
	-- ((Market = 'Tampa' AND Region = 'South'  ))



        --------OLD Query----------------------
		--Select Cast(WoRefId As nvarchar(100)) As WoRefNo,  SiteId, SiteCode, Latitude, Longitude, AV_Sites.ClusterId, AV_Sites.ClientId, Description,
		--AV_Sites.Status, AV_Sites.SubmittedOn 'SubmittedOn', AV_Sites.ScheduledOn 'AssignedOn', AV_Sites.ScheduledOn, AV_Sites.CompletedOn,AV_Sites.ScheduledOn, AV_Sites.ReceivedOn, 
		----Status, SubmittedOn, AssignedOn, ScheduledOn, CompletedOn,ScheduledOn, AV_Sites.ReceivedOn,
		--AD_Definations.DefinationName AS Market, DEF.DefinationName AS Region,
		----users.FirstName + ' ' + users.LastName AS Tester,
		--substring(
  --      (
  --          Select DISTINCT ', '+usr.FirstName + ' ' + usr.LastName AS [text()]
  --          From Sec_Users usr
  --          INNER JOIN AV_NetLayerStatus nls ON nls.SiteId=AV_Sites.SiteId AND nls.TesterId=usr.UserId            
  --          For XML PATH ('')
  --      ), 2, 1000) 'Tester',		
		--users.Picture TesterPicture,AD_Clients.ClientName,users.UserId 'TesterId',
		--AV_Sites.IsDownloaded,sts.DefinationName 'StatusName',AV_Sites.DriveCompletedOn, sts.KeyCode 'StatusKeyCode',AV_Sites.ReportSubmittedOn,AV_Sites.IsActive
		-- from AV_Sites 
		--LEFT JOIN AV_Clusters ON AV_Clusters.ClusterId = AV_Sites.ClusterId
		--LEFT JOIN AD_Clients ON AD_Clients.ClientId = AV_Sites.ClientId
		--LEFT JOIN AD_Definations ON AD_Definations.DefinationId = AV_Clusters.CityId
		--LEFT JOIN AD_Definations As DEF ON DEF.DefinationId = AD_Definations.PDefinationId
		--LEFT Join Sec_Users users ON users.UserId = AV_Sites.TesterId
		--LEFT JOIN AD_Definations AS sts ON sts.DefinationId=AV_Sites.[Status]
		
		--WHERE AV_Sites.IsActive=1 AND
		--(
		--	--(CAST(AV_Sites.SubmittedOn AS date) BETWEEN CAST(@value1 AS DATE) AND CAST(@value2 AS DATE))

		--	(CAST(AV_Sites.SubmittedOn AS date) BETWEEN '11/01/2017' AND '11/20/2017')
		--)
		--Order By AV_Sites.SubmittedOn ASC	
END




ELSE IF @Filter ='WorkOrderExport' begin
 -- @value1 For SiteId
		SELECT cls.ClusterCode 'clusterCode', rgn.DefinationName 'Region', cty.DefinationName 'Market', clt.ClientName 'Client', sit.SiteCode 'siteCode', stp.DefinationName 'SiteType',sec.CellId,
		sit.Latitude 'siteLatitude', sit.Longitude 'siteLongitude', sit.[Description],
		sec.SectorCode 'sectorCode', ntm.DefinationName 'networkMode', scp.DefinationName 'Scope', bnd.DefinationName 'Band', crr.DefinationName 'Carrier',
		sec.Antenna,sec.RFHeight,sec.BeamWidth,sec.VerticalBeamwidth 'VBeamWidth',sec.AntennaDownTilt 'AntennaDowntilt', sec.Azimuth, sec.PCI,CAST(CAST(sit.ReceivedOn AS DATE) AS DATETIME) 'ReceivedOn'
		,pro.ProjectName,sit.SiteName,sit.SiteAddress,adef.DefinationName 'SiteClass'
		FROM AV_Sites sit
		INNER JOIN AV_Clusters cls ON cls.ClusterId=sit.ClusterId
		INNER JOIN AV_Sectors sec ON sec.SiteId=sit.SiteId
		INNER JOIN AD_Clients clt ON clt.ClientId=sit.ClientId
		INNER JOIN AD_Definations cty ON cty.DefinationId=cls.CityId
		INNER JOIN AD_Definations rgn ON rgn.DefinationId=cty.PDefinationId
		INNER JOIN AD_Definations ntm ON ntm.DefinationId=sec.NetworkModeId
		INNER JOIN AD_Definations bnd ON bnd.DefinationId=sec.BandId
		INNER JOIN AD_Definations crr ON crr.DefinationId=sec.CarrierId
		INNER JOIN AD_Definations scp ON scp.DefinationId=sec.ScopeId
		LEFT JOIN AD_Definations stp ON stp.DefinationId=sit.SiteTypeId
		LEFT JOIN PM_Projects pro on pro.ProjectId = sit.ProjectId
		LEFT JOIN AD_Definations adef on adef.DefinationId = sit.SiteClassId
		WHERE sit.SiteId=@value1
		ORDER BY  ntm.DefinationName, bnd.DefinationName,crr.DefinationName,sec.SectorId
END
ELSE if @Filter ='Status_Check'
	begin
		
		--@value1 =SiteId
		--@value2 =TesterId	
		--@value3 =IMEI
		select 0
	end
	ELSE if @Filter ='ClusterSchedule'
begin
--@value1 siteid
--@value2 tester
--@value3 imei
	DECLARE @isMaster as bit=0
	DECLARE @DeviceId as numeric=0

	SELECT @isMaster=cs.IsMaster, @DeviceId=cs.UserDeviceId  FROM Sec_UserDevices x
	INNER JOIN AV_ClusterSchedule cs on cs.TesterId=x.UserId and cs.UserDeviceId=x.DeviceId
	WHERE x.UserId=@value2 AND x.IMEI=@value3 and x.isActive=1
	and cs.IsActive=1 and cs.SiteId=@value1;

	IF @isMaster=0
	begin
		select * from AV_ClusterSchedule	cs
		inner join Sec_UserDevices ud on ud.DeviceId=cs.UserDeviceId
		where SiteId=@value1 and TesterId=@value2 and UserDeviceId=@DeviceId --and IsMaster=0
	END
	ELSE
	BEGIN
		select * from AV_ClusterSchedule	cs
		inner join Sec_UserDevices ud on ud.DeviceId=cs.UserDeviceId
		where SiteId=@value1 and TesterId=@value2 --and IsMaster=0
	END



END

	ELSE IF @Filter ='CheckSiteCode' 
	BEGIN
		SELECT COUNT(*) FROM AV_Sites WHERE SiteCode = @value1
	END
	

END