CREATE PROCEDURE [dbo].[AV_Insert_BeamTestLog]

@List Tb_AV_BeamTestLog ReadOnly,
@TestStatus NVARCHAR(50)=NULL
AS
BEGIN

INSERT INTO AV_BeamTestLog (TimeStamp,SiteId,NetworkModeId,ScopeId,BandId,CarrierId,LayerStatusId,BeamGroupId,BeamId,BMGColor,BMColor,
		Latitude,Longitude,PCIId,SSBIndex,NRRSRP0,NRRSRP1,NRRSRP2,NRRSRP3,NRRSRQ0,NRRSRQ1,NRRSRQ2,NRRSRQ3,NRRSNR0,NRRSNR1,NRRSNR2,NRRSNR3,SectorId )

SELECT TimeStamp,SiteId,NetworkModeId,ScopeId,BandId,CarrierId,LayerStatusId,BeamGroupId,BeamId,BMGColor,BMColor,
		Latitude,Longitude,PCIId,SSBIndex,NRRSRP0,NRRSRP1,NRRSRP2,NRRSRP3,NRRSRQ0,NRRSRQ1,NRRSRQ2,NRRSRQ3,NRRSNR0,NRRSNR1,NRRSNR2,NRRSNR3 ,SectorId
		FROM @List


DECLARE @SiteId NUMERIC(18,0)
DECLARE @SectorId NUMERIC(18,0)
DECLARE @BandId NUMERIC(18,0)
DECLARE @NetwordModeId NUMERIC(18,0)
DECLARE @CarrierId NUMERIC(18,0)
DECLARE @LayerStatusId NUMERIC(18,0)

 SELECT TOP(1) @SiteId=SiteId,@SectorId=SectorId,@NetwordModeId=NetworkModeId,@CarrierId=CarrierId,@BandId=BandId
 FROM @List

 SET @LayerStatusId=(Select LayerStatusId FROM AV_NetLayerStatus WHERE SiteId=@SiteId)
 -- Update Layer Status Id

	UPDATE AV_BeamTestLog 
	SET LayerStatusId=@LayerStatusId
	WHERE SiteId=@SiteId AND NetworkModeId=@NetwordModeId AND BandId=@BandId AND SectorId=@SectorId AND CarrierId=@CarrierId
 --

 SELECT DISTINCT beamgroupid,beamid,bmgcolor,bmcolor
 INTO #bmColors
 FROM AV_BeamTestLog
 WHERE SiteId=@SiteId AND NetworkModeId=@NetwordModeId AND BandId=@BandId AND SectorId=@SectorId AND CarrierId=@CarrierId
 
 SELECT DISTINCT beamgroupid,beamid,SiteId,SectorId,NetworkModeId,BandId,CarrierId ,bmgcolor,bmcolor
 FROM AV_BeamTestLog
 WHERE SiteId=@SiteId AND NetworkModeId=@NetwordModeId AND BandId=@BandId AND SectorId=@SectorId AND CarrierId=@CarrierId

-- Update Beam Group Colors 

DECLARE @Id INT
DECLARE @ColorCode NVARCHAR(20)
DECLARE @Temp AS TABLE
(
	Id INT
)

	INSERT INTO @Temp
	SELECT DISTINCT BeamGroupId FROM @List
	GROUP BY BeamGroupId

	DECLARE Color_Cursor CURSOR FOR 
	SELECT Id FROM @Temp

	OPEN Color_Cursor

	FETCH NEXT FROM Color_Cursor 
	INTO @Id

	WHILE @@FETCH_STATUS=0
	BEGIN

		SET @ColorCode=(SELECT TOP(1) ColorCode FROM AD_Definations WHERE KeyCode='COLOR' ORDER BY NEWID())

		UPDATE AV_BeamTestLog 
		SET BMGColor=(CASE WHEN ISNULL((SELECT COUNT(bc.beamgroupid) FROM #bmColors AS bc WHERE bc.beamgroupid=@Id),0)>0 THEN (CASE WHEN ISNULL((SELECT TOP 1 bc.bmgcolor FROM #bmColors AS bc WHERE bc.beamgroupid=@Id),@ColorCode)='' THEN @ColorCode ELSE ISNULL((SELECT TOP 1 bc.bmgcolor FROM #bmColors AS bc WHERE bc.beamgroupid=@Id),@ColorCode) END) ELSE @ColorCode END) 
		WHERE BeamGroupId=@Id AND SiteId=@SiteId AND NetworkModeId=@NetwordModeId AND BandId=@BandId AND SectorId=@SectorId AND CarrierId=@CarrierId
		
		SELECT 'BG',@Id,@BandId,@NetwordModeId,@CarrierId,@SectorId, @ColorCode,(CASE WHEN ISNULL((SELECT COUNT(bc.beamgroupid) FROM #bmColors AS bc WHERE bc.beamgroupid=@Id),0)>0 THEN (CASE WHEN ISNULL((SELECT TOP 1 bc.bmgcolor FROM #bmColors AS bc WHERE bc.beamgroupid=@Id),@ColorCode)='' THEN @ColorCode ELSE ISNULL((SELECT TOP 1 bc.bmgcolor FROM #bmColors AS bc WHERE bc.beamgroupid=@Id),@ColorCode) END) ELSE @ColorCode END)

		FETCH NEXT FROM Color_Cursor 
		Into @Id

	END
	CLOSE Color_Cursor
	DEALLOCATE Color_Cursor

-- Update Beam Colors

	
	INSERT INTO @Temp
	SELECT DISTINCT BeamId FROM @List 

	DECLARE Color_Cursor CURSOR FOR 
	SELECT Id FROM @Temp

		OPEN Color_Cursor

		FETCH NEXT FROM Color_Cursor 
		INTO @Id

		WHILE @@FETCH_STATUS=0
		BEGIN

			SET @ColorCode=(SELECT TOP(1) ColorCode FROM AD_Definations WHERE KeyCode='COLOR' ORDER BY NEWID())

			UPDATE AV_BeamTestLog 
			SET BMColor=(CASE WHEN ISNULL((SELECT COUNT(bc.beamid) FROM #bmColors AS bc WHERE bc.beamid=@Id),0)>0 THEN (CASE WHEN ISNULL((SELECT TOP 1 bc.bmcolor FROM #bmColors AS bc WHERE bc.beamid=@Id),@ColorCode)='' THEN @ColorCode ELSE (SELECT TOP 1 bc.bmcolor FROM #bmColors AS bc WHERE bc.beamid=@Id) END) ELSE @ColorCode END) 
			WHERE BeamId=@Id AND SiteId=@SiteId AND NetworkModeId=@NetwordModeId AND BandId=@BandId AND SectorId=@SectorId AND CarrierId=@CarrierId


			SELECT 'BM',@Id,@BandId,@NetwordModeId,@CarrierId,@SectorId,(CASE WHEN ISNULL((SELECT COUNT(bc.beamid) FROM #bmColors AS bc WHERE bc.beamid=@Id),0)>0 THEN (CASE WHEN ISNULL((SELECT TOP 1 bc.bmcolor FROM #bmColors AS bc WHERE bc.beamid=@Id),@ColorCode)='' THEN @ColorCode ELSE (SELECT TOP 1 bc.bmcolor FROM #bmColors AS bc WHERE bc.beamid=@Id) END) ELSE @ColorCode END)
			FETCH NEXT FROM Color_Cursor 
			Into @Id

		END
		CLOSE Color_Cursor
		DEALLOCATE Color_Cursor

END