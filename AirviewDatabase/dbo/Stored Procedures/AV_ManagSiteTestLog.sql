-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- exec AV_ManagSiteTestLog @Filter=N'DisablePcis',@SiteId=N'648664',@value='0',@value1=N'14_104_107',@value2=N'501',@AngleFrom='',@AngleTo='',@DistanceFrom='0',@DistanceTo='51'
CREATE PROCEDURE AV_ManagSiteTestLog
    @Filter NVARCHAR(50),
	@SiteId NUMERIC(18,0) =NULL,
	@NetworkModeId NUMERIC(18,0) =NULL,
	@BandId NUMERIC(18,0) =NULL,
	@ScopeId NUMERIC(18,0) =NULL,
	@CarrierId NUMERIC(18,0) =NULL,
	@value NVARCHAR(50)=NULL, -- true,false
	@value1 NVARCHAR(500)=NULL, -- fromTime
	@value2 NVARCHAR(500)=NULL, -- toTime
	--@List as [dbo].[List] READONLY,
	@AngleFrom NVARCHAR(500)=NULL, 
	@AngleTo NVARCHAR(500)=NULL, 
	@DistanceFrom NVARCHAR(500)=NULL, 
	@DistanceTo NVARCHAR(500)=NULL

AS
BEGIN

	DECLARE @Scope AS NVARCHAR(5)=(SELECT ad.DefinationName FROM AV_Sites sit INNER JOIN AD_Definations AS ad ON ad.DefinationId=sit.ScopeId WHERE sit.SiteId=@SiteId AND sit.IsActive=1)
			
	
   		
	IF @Filter='DisableMapRoot'
	BEGIN
		SELECT 0;
	END
	
	Else IF @Filter='DisableServerTimestamp'
	BEGIN
		--SELECT 0;
		IF @Scope='SSV'
		BEGIN
			UPDATE AV_SiteTestLog
			SET IsActive = (CASE WHEN @value='True' then CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
			WHERE SiteId=@SiteId AND BandId=@BandId AND CarrierId=@CarrierId AND NetworkModeId=@NetworkModeId
			----AND serverTimeStamp=CAST(@value1 AS DATETIME)
			AND Charindex(CONVERT(VARCHAR(24),serverTimeStamp,120)+',', @Value1) > 0;
		END
		ELSE IF @Scope='NI'
		BEGIN
			UPDATE AV_SiteTestLog
			SET IsActive = (CASE WHEN @value='True' then CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
			WHERE SiteId=@SiteId --AND BandId=@BandId AND CarrierId=@CarrierId AND NetworkModeId=@NetworkModeId
			----AND serverTimeStamp=CAST(@value1 AS DATETIME)
			AND Charindex(CONVERT(VARCHAR(24),serverTimeStamp,120)+',', @Value1) > 0;
		END
	END
	Else IF @Filter='DisablePcis'
	BEGIN
	IF @value2 <>''
		BEGIN
		 DECLARE @Sqlqury AS varchar(Max)=('UPDATE AV_SiteTestLog
			SET IsActive = (CASE WHEN '+@value+'= 1'+' then CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
			WHERE SiteId='+convert(nvarchar(30),@SiteId)+' AND BandId='+convert(nvarchar(30),@BandId)+' AND CarrierId='+convert(nvarchar(30),@CarrierId)+' AND NetworkModeId='+ convert(nvarchar(30),@NetworkModeId)+' ')

	SET @value2 = @value2 +',';
		IF @Scope='SSV'
		BEGIN
			-- @value as  true,false
			-- @value1 as pcis like 43,116,132
			UPDATE AV_SiteTestLog
			SET IsActive = (CASE WHEN @value='True' then CAST(1 AS BIT) ELSE CAST(0 AS BIT) END)
			WHERE SiteId=@SiteId AND BandId=@BandId AND CarrierId=@CarrierId AND NetworkModeId=@NetworkModeId
            AND Charindex(CAST(pciid AS NVARCHAR(MAX))+',', @Value1) > 0
			OR ( @DistanceFrom > 0 or @DistanceTo > 0);
		
		END
		ELSE IF @Scope='NI'
		BEGIN
		set @NetworkModeId = (select top 1 * from SplitString(@value1,'_'))
    set @CarrierId = (select top 1 * from SplitString(@value1,'_') order by Item desc)
	set @BandId = (SELECT TOP 1 * FROM ( SELECT TOP 2 * FROM SplitString(@value1,'_') ORDER BY Item ) z ORDER BY item DESC)   
		IF @DistanceFrom <>'' and @DistanceTo <>''
		BEGIN 
			set @Sqlqury +=' And DistanceFromSite between '+@DistanceFrom+' and '+@DistanceTo+''
			End
		
	    IF @AngleFrom <>'' and @AngleTo <>''
			Begin
		   set @Sqlqury +=' And AngleToSite  between '+@AngleFrom+' and '+@AngleTo+''
			End
			 set @Sqlqury +=' AND Charindex(CAST(pciid AS NVARCHAR(MAX))+'','''+','''+@Value2 +''' ) > 0;' 
			
		END
		
	    END
		END

		exec(@sqlqury)
	--ELSE IF @Filter='RemoveSiteTestLogs'
	--BEGIN
	
	--DECLARE @xSiteId AS NVARCHAR(50)=''
	--DECLARE @xTimeStamp AS DATETIME=''


 --       DECLARE db_cluster2 CURSOR Read_Only
	--	FOR  
	--	SELECT l.Value1,l.Value2
	--	FROM @List AS l
	--	OPEN db_cluster2 
	--	FETCH NEXT FROM db_cluster2 INTO @xSiteId, @xTimeStamp
	--	WHILE @@FETCH_STATUS = 0   
	--	BEGIN	
	--	BEGIN	
	--		Update AV_SiteTestLog Set IsActive=@value
	--		FROM @List p
	--		WHERE SiteId = @xSiteId AND format(serverTimeStamp, 'yyyy-MM-dd HH:mm:ss')=format(@xTimeStamp, 'yyyy-MM-dd HH:mm:ss')	
	--	END			
	--	FETCH NEXT FROM db_cluster2 INTO @xSiteId, @xTimeStamp
	--	END   
	--	CLOSE db_cluster2   
	--	DEALLOCATE db_cluster2				
	--END




	-- RemoveNIPcis

END