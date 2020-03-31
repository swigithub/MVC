
CREATE PROCEDURE [dbo].[Test_getBands]
    @list Test_IdList READONLY
AS
BEGIN
    -- Just return the items we passed in
    SELECT Band ,BandId from AV_SiteTestLog where NetworkModeId in (select * from @list);
END