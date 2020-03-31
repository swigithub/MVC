-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_GetSiteIssueTracker] --'ByTesterId','12'
	@Filter nvarchar(50),
	@SiteId numeric(18, 0)=null,
	@TesterId numeric(18, 0) =null,
	@NetworkModeId numeric(18, 0)=null,
	@BandId numeric(18, 0)=null,
	@CarrierId numeric(18, 0) =null,
	@ScopId numeric(18, 0)=null,
	@Status nvarchar(50)  =NULL
AS
BEGIN
	
	 if @Filter='ByTesterId' 
	 begin
		 select sit.* ,us.FirstName+' '+us.LastName 'Tester',us.Picture,ad.DefinationName 'IssueTypeName'
		 from AV_SiteIssueTracker sit
		 inner join Sec_Users us on us.UserId=sit.TesterId
		 INNER JOIN AD_Definations AS ad ON ad.DefinationId=sit.IssueType
		 where SiteId=@SiteId and TesterId=@TesterId
	 END
	 ELSE IF @Filter='byStatus' 
	 BEGIN
	 	select sit.* ,us.FirstName+' '+us.LastName 'Tester',us.Picture
		 from AV_SiteIssueTracker sit
		 inner join Sec_Users us on us.UserId=sit.TesterId
	 	WHERE sit.Status=@Status
	 END
	 
	 
--   [dbo].[AV_GetSiteIssueTracker] 'byNetLayer',10577,15,73 ,76,36
	 ELSE if @Filter='byNetLayer' 
	 BEGIN
	 
		 select sit.* ,us.FirstName+' '+us.LastName 'Tester',us.Picture,ad.DefinationName 'IssueTypeName'
		 from AV_SiteIssueTracker sit
		 inner join Sec_Users us on us.UserId=sit.TesterId
		 INNER JOIN AD_Definations AS ad ON ad.DefinationId=sit.IssueType
		 where SiteId=@SiteId and sit.NetworkModeId=@NetworkModeId AND sit.BandId=@BandId AND sit.CarrierId=@CarrierId AND sit.ScopeId=@ScopId
	 END


	
END