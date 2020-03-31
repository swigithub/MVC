	CREATE procedure spWorkOrderStatus
	AS
	Begin
	
	SELECT s.WoRefId, c.ClientName AS 'Client'
	,Region= (SELECT def.DefinationName from AD_Definations def INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId AND def.DefinationId = 4)
	,d.DefinationName As 'Market'
	,s.SiteCode, ur.FirstName +' '+ ur.LastName as Drive_Tester
	,s.ReceivedOn, s.SubmittedOn, s.ScheduledOn, s.DriveCompletedOn,s.ReportSubmittedOn, s.PublishedOn
	,NetWork_Layers =(SELECT DefinationName from AD_Definations where DefinationId=nls.BandId)
	,st.DefinationName 'Status'
	,s.Description
	,0 AS SectorCount
	,0 AS LayerCount
	,0 AS Distance_from_Site
	,0 AS DT_Hours
	
	FROM AV_Sites s
	INNER JOIN	AD_Clients as c on c.ClientId= s.ClientId
	INNER JOIN	AD_Definations as d on s.CityId=d.DefinationId
	INNER JOIN  AD_Definations AS st ON st.DefinationId=s.Status
	inner join  AV_NetLayerStatus  AS nls ON s.SiteId=nls.SiteId
	INNER JOIN  Sec_Users as ur on s.TesterId=ur.UserId
	
	END