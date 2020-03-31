-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
/*
[ReportId] [numeric](18, 0) NULL,
	[ClientId] [numeric](18, 0) NULL,
	[CityId] [numeric](18, 0) NULL,
	[KeyValue] [nvarchar](250) NULL,
	[KeyId] [numeric](18, 0) NULL*/
-- =============================================
CREATE PROCEDURE [dbo].[AD_InsertReportConfiguration]
	@List Tb_AD_ReportConfiguration READONLY
AS
BEGIN

	INSERT INTO AD_ReportConfiguration(ReportId,ClientId, CityId, KeyValue, KeyId,isActive, IsPanelItem,Keycode,fontColor,ScopeId )
   SELECT x.ReportId,x.ClientId, x.CityId, x.KeyValue, x.KeyId,x.isActive,0 'IsPanelItem',ad.KeyCode,x.fontColor,x.ScopeId
     FROM @List x INNER JOIN AD_Definations AS ad ON ad.DefinationId=x.KeyId
END