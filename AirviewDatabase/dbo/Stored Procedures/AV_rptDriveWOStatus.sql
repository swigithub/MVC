-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- [dbo].[AV_rptDriveWOStatus] 'Drive_WO_Status','SubmittedOn = 19/05/2017'
CREATE PROCEDURE [dbo].[AV_rptDriveWOStatus] 
@Filter varchar(200),
@Where nvarchar(500)
AS
IF @Filter='Drive_WO_Status'
BEGIN

DECLARE @sql as nvarchar(max)=('
	select( d.DefinationName ) as Market,
	COUNT(s.SiteId) AS TotalSites ,
	SUM(CASE WHEN s.Status = 92 THEN 1 ELSE 0 END) AS DriveCompletedSites,
	SUM(CASE WHEN s.Status = 89 THEN 1 ELSE NULL END) AS CompletedSites, 
	SUM(CASE WHEN s.Status = 451 THEN 1 ELSE 0 END) AS ReportSubmitted,
	SUM(CASE WHEN s.Status= 90 THEN 1 ELSE NULL END) AS PendingSites ,
	SUM(CASE WHEN s.Status = 93 THEN 1 ELSE 0 END) AS PendingWithIssuesSites,
	SUM(CASE WHEN s.Status = 91 THEN 1 ELSE NULL END) AS InProcessSites ,
	SUM(CASE WHEN s.Status = 450 THEN 1 ELSE 0 END) AS InProgress
	

from dbo.AV_Sites as s inner join
dbo.AD_Definations as d on s.CityId=d.DefinationId
')

EXEC(@sql+'where s.IsActive=1 and '+@Where +'group by d.DefinationName')--SubmittedOn = '19/05/2017'
END