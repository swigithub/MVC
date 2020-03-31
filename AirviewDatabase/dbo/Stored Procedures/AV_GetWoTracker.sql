CREATE PROCEDURE [dbo].[AV_GetWoTracker]
 @Filter NVARCHAR(50),
 @SiteId NUMERIC(18,0),
 @NetworkModeId NUMERIC(18,0),
 @BandId NUMERIC(18,0),
 @CarrierId NUMERIC(18,0)
AS
BEGIN
 IF @Filter='ByNetLayer'
 BEGIN
  SELECT wt.*,su.Picture,su.FirstName + ' '+su.LastName 'TesterFullName',ISNULL(sec.SectorCode,'N/A') 'SectorCode'
  FROM AV_WoTracker AS wt
  INNER JOIN Sec_Users AS su ON su.UserId=wt.TesterId
  LEFT OUTER JOIN AV_Sectors AS sec ON sec.SectorId=wt.SectorId AND sec.SiteId=wt.SiteId
  WHERE wt.SiteId=@SiteId AND wt.NetworkModeId=@NetworkModeId AND wt.BandId=@BandId AND wt.CarrierId=@CarrierId
 END
 ELSE IF @Filter='StatusTracker'
 BEGIN
 	SELECT x.*
 	FROM
 	(
 	SELECT wt.ReceivedOn 'TrackerTimestamp',ac.Logo 'Picture',ac.ClientName 'TesterFullName','Received Network Layer from Client: '+ac.ClientName+'.' 'Description',
	wt.SiteId, wt.NetworkModeId, wt.BandId, wt.CarrierId, wt.ScopeId, wt.TesterId,'RECEIVED' 'Status',90 'StatusCode'
	FROM AV_NetLayerStatus AS wt 
	INNER JOIN AV_Sites AS as1 ON as1.SiteId=wt.SiteId
	INNER JOIN AD_Clients AS ac ON ac.ClientId=as1.ClientId
	WHERE wt.SiteId=@SiteId AND wt.NetworkModeId=@NetworkModeId AND wt.BandId=@BandId AND wt.CarrierId=@CarrierId
	UNION ALL
	SELECT wt.UploadedOn 'TrackerTimestamp',su.Picture,su.FirstName + ' '+su.LastName 'TesterFullName','Network Layer Uploaded in AirView.' 'Description',
	wt.SiteId, wt.NetworkModeId, wt.BandId, wt.CarrierId, wt.ScopeId, wt.TesterId,'UPLOADED' 'Status',90 'StatusCode'
	FROM AV_NetLayerStatus AS wt 
	INNER JOIN Sec_Users AS su ON su.UserId=wt.UploadedById
	WHERE wt.SiteId=@SiteId AND wt.NetworkModeId=@NetworkModeId AND wt.BandId=@BandId AND wt.CarrierId=@CarrierId AND wt.UploadedOn IS NOT NULL
	UNION ALL
	SELECT wt.AssignedOn 'TrackerTimestamp',su.Picture,su.FirstName + ' '+su.LastName 'TesterFullName','Network Layer Scheduled & Assigned to Drive Tester: ' + t.FirstName + ' '+t.LastName+'.' 'Description',
	wt.SiteId, wt.NetworkModeId, wt.BandId, wt.CarrierId, wt.ScopeId, wt.TesterId,'SCHEDULED' 'Status',91 'StatusCode'
	FROM AV_NetLayerStatus AS wt 
	INNER JOIN Sec_Users AS su ON su.UserId=wt.ScheduledById
	INNER JOIN Sec_Users AS t ON t.UserId=wt.TesterId
	WHERE wt.SiteId=@SiteId AND wt.NetworkModeId=@NetworkModeId AND wt.BandId=@BandId AND wt.CarrierId=@CarrierId AND wt.ScheduledOn IS NOT NULL
	UNION ALL
	SELECT DISTINCT awt.DownloadDate 'TrackerTimestamp',su.Picture,su.FirstName + ' '+su.LastName 'TesterFullName','Network Layer Downloaded by Drive Tester.' 'Description',
	wt.SiteId, wt.NetworkModeId, wt.BandId, wt.CarrierId, wt.ScopeId, wt.TesterId,'DOWNLOADED' 'Status',95 'StatusCode'
	FROM AV_NetLayerStatus AS wt
	INNER JOIN AV_WoDevices AS awt ON awt.SiteId=wt.SiteId AND awt.NetworkId=wt.NetworkModeId AND awt.BandId=wt.BandId AND awt.CarrierId=wt.CarrierId 
	INNER JOIN Sec_Users AS su ON su.UserId=wt.TesterId
	WHERE wt.SiteId=@SiteId AND wt.NetworkModeId=@NetworkModeId AND wt.BandId=@BandId AND wt.CarrierId=@CarrierId AND awt.IsDownlaoded=1
	UNION ALL
	SELECT wt.DriveCompletedOn 'TrackerTimestamp',su.Picture,su.FirstName + ' '+su.LastName 'TesterFullName','Network Layer Marked as Drive Completed by Drive Tester.' 'Description',
	wt.SiteId, wt.NetworkModeId, wt.BandId, wt.CarrierId, wt.ScopeId, wt.TesterId,'DRIVE_COMPLETED' 'Status',92 'StatusCode'
	FROM AV_NetLayerStatus AS wt 
	INNER JOIN Sec_Users AS su ON su.UserId=wt.TesterId
	WHERE wt.SiteId=@SiteId AND wt.NetworkModeId=@NetworkModeId AND wt.BandId=@BandId AND wt.CarrierId=@CarrierId AND wt.DriveCompletedOn IS NOT NULL	
	UNION ALL
	SELECT wt.pendingWithIssueOn 'TrackerTimestamp',su.Picture,su.FirstName + ' '+su.LastName 'TesterFullName','Network Layer Marked as Pending with Issue Due to Reason: '+ad.DefinationName+'. Description: '+wt.PendingIssueDesc 'Description',
	wt.SiteId, wt.NetworkModeId, wt.BandId, wt.CarrierId, wt.ScopeId, wt.TesterId,'PENDING_WITH_ISSUE' 'Status',93 'StatusCode'
	FROM AV_NetLayerStatus AS wt
	INNER JOIN Sec_Users AS su ON su.UserId=wt.pendingWithIssueById
	INNER JOIN AD_Definations AS ad ON ad.DefinationId=wt.StatusReason
	WHERE wt.SiteId=@SiteId AND wt.NetworkModeId=@NetworkModeId AND wt.BandId=@BandId AND wt.CarrierId=@CarrierId AND wt.SubmittedOn IS NOT NULL	
	UNION ALL
	SELECT wt.SubmittedOn 'TrackerTimestamp',su.Picture,su.FirstName + ' '+su.LastName 'TesterFullName','Report Submitted for Network Layer.' 'Description',
	wt.SiteId, wt.NetworkModeId, wt.BandId, wt.CarrierId, wt.ScopeId, wt.TesterId,'REPORT_SUBMITTED' 'Status',451 'StatusCode'
	FROM AV_NetLayerStatus AS wt 
	INNER JOIN Sec_Users AS su ON su.UserId=wt.SubmittedById
	WHERE wt.SiteId=@SiteId AND wt.NetworkModeId=@NetworkModeId AND wt.BandId=@BandId AND wt.CarrierId=@CarrierId AND wt.SubmittedOn IS NOT NULL
	UNION ALL
	SELECT wt.CompletedOn 'TrackerTimestamp',su.Picture,su.FirstName + ' '+su.LastName 'TesterFullName','Report Approved for Network Layer.' 'Description',
	wt.SiteId, wt.NetworkModeId, wt.BandId, wt.CarrierId, wt.ScopeId, wt.TesterId,'REPORT_APPROVED' 'Status',89 'StatusCode'
	FROM AV_NetLayerStatus AS wt 
	INNER JOIN Sec_Users AS su ON su.UserId=wt.AcceptedById
	WHERE wt.SiteId=@SiteId AND wt.NetworkModeId=@NetworkModeId AND wt.BandId=@BandId AND wt.CarrierId=@CarrierId AND wt.CompletedOn IS NOT NULL
	UNION ALL
	SELECT wt.TransactionDate 'TrackerTimestamp',su.Picture,su.FirstName + ' '+su.LastName 'TesterFullName','Network Layer Status Changed to: '+
	(
		CASE
			WHEN wt.StatusId=90 THEN 'Pending Schedule'
			WHEN wt.StatusId=91 THEN 'Scheduled'
			WHEN wt.StatusId=92 THEN 'Drive Completed'
			WHEN wt.StatusId=93 THEN 'Pending with Issue'
			WHEN wt.StatusId=450 THEN 'In Progress' +ISNULL((CASE WHEN wt.[Description] IS NOT NULL AND wt.TransactionSource='AV_InsertWoTracker' THEN ', Test Attempted: '+wt.[Description] ELSE '' END),'')
			WHEN wt.StatusId=451 THEN 'Report Submitted'
			WHEN wt.StatusId=89 THEN 'Report Approved'
		END
	) 'Description',
	wt.SiteId, wt.NetworkModeId, wt.BandId, wt.CarrierId, wt.ScopeId, wt.UserId,'STATUS_CHANGED' 'Status',wt.[StatusId] 'StatusCode'
	FROM AirViewLogs.dbo.Log_WOStatusTracker AS wt 
	INNER JOIN Sec_Users AS su ON su.UserId=wt.UserId
	WHERE wt.SiteId=@SiteId AND wt.NetworkModeId=@NetworkModeId AND wt.BandId=@BandId AND wt.CarrierId=@CarrierId
	--AND wt.TransactionSource='WO_STATUS_INPROGRESS'
 	) x
 	ORDER BY x.TrackerTimestamp ASC,
 	CASE WHEN x.StatusCode=90 THEN 1
		 WHEN x.StatusCode=91 THEN 2
		 WHEN x.StatusCode=450 THEN 3
		 WHEN x.StatusCode=95 THEN 4
		 WHEN x.StatusCode=92 THEN 5
		 WHEN x.StatusCode=93 THEN 6
		 WHEN x.StatusCode=451 THEN 7
		 WHEN x.StatusCode=89 THEN 8
		 WHEN x.StatusCode=512 THEN 9
	END
 END
END