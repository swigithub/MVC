CREATE PROCEDURE [dbo].[sp_pending-issue]
		
AS
select * from AV_Sectors
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

select WoRefId as 'Wo_Ref', ad.DefinationName as 'Market',CASE WHEN charindex('_',SiteCode)=0 AND charindex('-',SiteCode)=0 THEN SiteCode WHEN charindex('-',SiteCode)>0 THEN LEFT(SiteCode,charindex('-',SiteCode)-1) ELSE LEFT(SiteCode,charindex('_',SiteCode)-1) END SiteCode ,'Pending With Issues' as Status,def.DefinationName AS 'Network_Layers',SubmittedOn as Submitted,0 as redriveWO,SiteCode as 'redrive site',ReportSubmittedOn as redriveSub,0 as redriveStatus,0 as Redrivereportsumit ,0 as redrivelayers,'yes' as checks 
 from AV_Sites sit
inner join AD_Definations ad on ad.DefinationId=sit.CityId
INNER JOIN AV_Sectors AS sec ON sec.SiteId=sit.SiteId
INNER JOIN AD_Definations AS def ON def. DefinationId=sec.BandId
 where Status=93 
And SiteCode  Like '%-R%'
   
END