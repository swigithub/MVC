CREATE PROCEDURE [dbo].[spWOSubmitReport]
		
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT CAST(LEFT(SubmittedOn,12) as datetime) AS 'SubmittedOn',
COUNT(s.SiteId) AS Submitted ,
	SUM(CASE WHEN s.Status = 451 THEN 1 ELSE 0 END) AS ReportSubmitted,
	SUM(CASE WHEN s.Status= 90 THEN 1 ELSE 0 END) AS PendingSchedule ,
	SUM(CASE WHEN s.Status = 91 THEN 1 ELSE 0 END) AS Scheduled ,
	SUM(CASE WHEN s.Status = 450 THEN 1 ELSE 0 END) AS InProgress,
	SUM(CASE WHEN s.Status = 92 THEN 1 ELSE 0 END) AS DriveCompleted,
	SUM(CASE WHEN s.Status = 93 THEN 1 ELSE 0 END) AS PendingWithIssues,
	SUM(CASE WHEN s.Status = 89 THEN 1 ELSE 0 END) AS Completed
FROM dbo.AV_Sites as s 
INNER JOIN dbo.AD_Definations as d on s.CityId=d.DefinationId
AND SubmittedOn BETWEEN '2017-07-28 14:32:18.610' AND '2017-10-28 14:32:18.610'
GROUP BY CAST(LEFT(SubmittedOn,12) as datetime)
   
END