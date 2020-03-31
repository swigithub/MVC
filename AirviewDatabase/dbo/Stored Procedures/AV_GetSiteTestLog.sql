-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_GetSiteTestLog]
	 @Filter nvarchar(50)
	,@SiteId numeric(18,0)
	,@NetworkmodeId numeric(18,0)
	,@BandId numeric(18,0)
	,@CarrierId numeric(18,0)
	,@ScopeId numeric(18,0)
AS
BEGIN
	if @Filter='Export'
	begin
		SELECT stl.TestType,  stl.SubNetworkMode 'ActualNetMode', stl.ActualBand, stl.ActualCarrier, stl.[Site], stl.Scope, stl.Region, stl.City 'Market', stl.NetworkMode, stl.Band,
		       stl.Carrier, stl.CellId, stl.LacId, stl.PciId, stl.MccId, stl.MncId, stl.GsmRssi, stl.GsmRxQual,
		       stl.WcdmaRscp, stl.WcdmaEcio, stl.LteRsrp, stl.LteRsrq, stl.LteRsnr,
		       stl.DistanceFromSite, stl.AngleToSite, stl.Latitude, stl.Longitude      
		from AV_SiteTestLog stl
		where stl.SiteId=@SiteId and stl.NetworkModeId=@NetworkmodeId and stl.BandId=@BandId and stl.CarrierId=@CarrierId and stl.ScopeId=@ScopeId
		
	end 
-- [dbo].[AV_GetSiteTestLog] 'PingTrace',398364,15,73,76,36
	else if @Filter='PingTrace'
	BEGIN
		
		SELECT  stl.TestType,   stl.Sector,stl.StackTrace,stl.[TimeStamp]
		from AV_SiteTestLog stl
		where stl.SiteId=@SiteId and stl.NetworkModeId=@NetworkmodeId and stl.BandId=@BandId 
		and stl.CarrierId=@CarrierId and stl.ScopeId=@ScopeId AND stl.TestType='Ping'		
		ORDER BY stl.[TimeStamp]
	end 
END