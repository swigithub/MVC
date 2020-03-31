CREATE  PROCEDURE [dbo].[Test_GetPci]
(

@siteId NUMERIC(18,0)
)
AS
BEGIN
 SELECT DISTINCT PciId,pciColor,TestType
  FROM AV_SiteTestLog 
   WHERE SiteId = @siteId
   AND TestType IN('CW','CCW')
  -- UNION ALL
  -- SELECT DISTINCT Carrier 'PciId',ChColor 'pciColor','Carrier' TestType
  --FROM AV_SiteTestLog 
  -- WHERE SiteId = @siteId
  -- AND TestType IN('CW','CCW')	
END