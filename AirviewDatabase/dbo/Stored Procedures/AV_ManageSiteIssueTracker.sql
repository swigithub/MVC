-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--[dbo].[AV_ManageSiteIssueTracker] 'Insert',0,3,10034,15,28,88,36,'by mobi','UN_RESOLVED'
CREATE PROCEDURE [dbo].[AV_ManageSiteIssueTracker] --'UpdateStatus',5,0,0,0,0,0,0,'','RESOLVED'
@Filter nvarchar(50)=null,
@TrackingId numeric(18, 0)=0,
@SiteId numeric(18, 0)=null,
@TesterId numeric(18, 0) =null,
@NetworkModeId numeric(18, 0)=null,
@BandId numeric(18, 0)=null,
@CarrierId numeric(18, 0) =null,
@ScopId numeric(18, 0)=null,
@Description nvarchar(500) =null,
@Status nvarchar(50)  =NULL,
@ImagePath nvarchar(300)  =NULL,
@IssueType nvarchar(300)  =NULL,
@UserId INT=0
AS
BEGIN
	if @Filter='Insert'
	BEGIN
		DECLARE @IssueTypeId AS INT=0;
		IF @IssueType NOT LIKE '%[^0-9]%'
		BEGIN
			SET @IssueTypeId=@IssueType			
		END
		ELSE
		BEGIN
			SET @IssueTypeId=(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.KeyCode=@IssueType)
		END
		
	
	   INSERT INTO [dbo].[AV_SiteIssueTracker]([SiteId],[TesterId],[NetworkModeId],[BandId],[CarrierId],[ScopeId],[Description],[Status],ImagePath, IssueType)
	   VALUES (@SiteId,CASE WHEN @UserId>0 THEN @UserId ELSE @TesterId END,@NetworkModeId,@BandId,@CarrierId,@ScopId,@Description,@Status,@ImagePath,@IssueTypeId)	  
	END
	ELSE if	@Filter='UpdateStatus'
	BEGIN
		update AV_SiteIssueTracker
		set Status=@Status ,ResolvedDate=GETDATE()
		where TrackingId=@TrackingId
	END
END