-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_ManageWoDevices]
	@WoDeviceId NUMERIC(18, 0) = NULL,
	@BandId NUMERIC(18, 0) = NULL,
	@CarrierId NUMERIC(18, 0) = NULL,
	@DownloadDate DATETIME = NULL,
	@IsDownlaoded BIT = NULL,
	@NetworkId NUMERIC(18, 0) = NULL,
	@UserId NUMERIC(18, 0) = NULL,
	@UserDeviceId NUMERIC(18, 0) = NULL,
	@SiteId NUMERIC(18, 0) = NULL,
	@ScopeId NUMERIC(18, 0) = NULL,
	@WoRefId NVARCHAR(50) = NULL
AS
BEGIN
	
		INSERT INTO [dbo].[AV_WoDevices]([BandId],[CarrierId],[DownloadDate],[IsDownlaoded],[NetworkId],[UserId],[UserDeviceId],[SiteId],[ScopeId],[WoRefId])
		VALUES( @BandId,@CarrierId,@DownloadDate,@IsDownlaoded,@NetworkId,@UserId,@UserDeviceId,@SiteId,@ScopeId,@WoRefId)
END