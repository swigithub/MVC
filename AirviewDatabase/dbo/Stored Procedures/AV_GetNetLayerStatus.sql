-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_GetNetLayerStatus]
 @Filter NVARCHAR(100),
 @NetworkModeId NUMERIC(18,0),
 @SiteId NUMERIC(18,0),
 @BandId NUMERIC(18,0),
 @CarrierId NUMERIC(18,0),  
 @ScopeId NUMERIC(18,0)=NULL
 ,@Value1 nvarchar(50)=null
 
AS
BEGIN
	IF @Filter='Get_PendingWithIssue'
		BEGIN
			SELECT [Status], StatusReason	,PendingIssueDesc,NetworkModeId, ScopeId,
			       BandId, CarrierId, SiteId
			FROM AV_NetLayerStatus
			WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId
		END
		
		ELSE IF @Filter='Get_ReDrive'
		BEGIN
			SELECT isRedrive, redriveTypeId,redriveReasonId,redriveComments,NetworkModeId, ScopeId,BandId, CarrierId, SiteId
			FROM AV_NetLayerStatus
			WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId --AND ScopeId=@ScopeId
		END
		ELSE IF @Filter='Get_Observation'
		BEGIN
			SELECT netLayerObservations,NetworkModeId, ScopeId,BandId, CarrierId, SiteId
			FROM AV_NetLayerStatus
			WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId --AND ScopeId=@ScopeId
		END
		
		ELSE IF @Filter='Get_byId'
		BEGIN
			SELECT netLayerObservations,NetworkModeId, ScopeId,BandId, CarrierId, SiteId
			FROM AV_NetLayerStatus
			WHERE SiteId=@SiteId AND NetworkModeId=@NetworkModeId AND BandId=@BandId AND CarrierId=@CarrierId --AND ScopeId=@ScopeId
		END
		
		ELSE IF @Filter='Get_byLayerStatusId'
		BEGIN
			SELECT *
			FROM AV_NetLayerStatus
			WHERE LayerStatusId=@Value1
		END
		
		ELSE IF @Filter='Get_Layers_By_SiteId'
		BEGIN
			--SELECT s.SiteId, s.SiteCode, nls.LayerStatusId, Convert(nvarchar(50),nls.BandId )+'-'+ Convert(nvarchar(50),nls.CarrierId) +'-'+ Convert(nvarchar(50),def.DefinationName) 'NetLayer'
			--FROM   AV_Sites s
			--INNER JOIN AV_NetLayerStatus nls ON s.SiteId = nls.SiteId 
			--INNER JOIN AD_Definations def ON def.DefinationId = s.ScopeId
			--inner join Sec_UserScopes us on us.ScopeId=def.DefinationId
			--WHERE s.SiteId=@SiteId
			
			SELECT anls.SiteId,anls.LayerStatusId,scp.DefinationName 'Scope',Convert(nvarchar(50),anls.NetworkModeId )+'-'+Convert(nvarchar(50),anls.BandId )+'-'+ Convert(nvarchar(50),anls.CarrierId) +'-'+ Convert(nvarchar(50),anls.ScopeId) 'NetLayerId',
			bnd.DefinationName + ' ('+ crr.DefinationName+')' 'NetLayerName'
			FROM AV_NetLayerStatus AS anls
			Inner Join AD_Definations nmt On nmt.DefinationId = anls.NetworkModeId
			Inner Join AD_Definations bnd On bnd.DefinationId = anls.BandId
			Inner Join AD_Definations crr On crr.DefinationId = anls.CarrierId
			Inner Join AD_Definations scp On scp.DefinationId = anls.ScopeId
			WHERE anls.SiteId=@SiteId AND anls.IsActive=1
			
		END
END