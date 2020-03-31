
--exec AD_GetProjects 'ById', 1

CREATE PROCEDURE [dbo].[AD_ChartSettings]
	@Filter NVARCHAR(50),
	@value  NVARCHAR(50)=NULL,
	@List List READONLY
AS
BEGIN
		
--  [dbo].[AD_GetProjects] 'All'		
  if @Filter='By_ProjectId'
	BEGIN 
	select * from PM_ChartSettings	where ProjectId=@value AND IsActive=1
	End
	 if @Filter='Insert'
	BEGIN
		 DECLARE @Value1 NVARCHAR(50)=null
		DECLARE  @Value2 NVARCHAR(50)=null
		DECLARE  @Value3 NVARCHAR(50)=null
		DECLARE  @Value4 NVARCHAR(50)=null
		DECLARE  @Value5 NVARCHAR(50)=null

		DECLARE  UpdateC CURSOR READ_ONLY
		FOR
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5 FROM @List AS l 
		--OPEN CURSOR.
		OPEN UpdateC 
		--FETCH THE RECORD INTO THE VARIABLES.
		FETCH NEXT FROM UpdateC INTO @Value1,@Value2,@Value3,@Value4,@Value5
		--LOOP UNTIL RECORDS ARE AVAILABLE.
		WHILE @@FETCH_STATUS = 0
		BEGIN		 
			BEGIN
				IF @Value1>0
				BEGIN
					UPDATE PM_ChartSettings
					SET ColorCode=@Value2,SeriesType=@Value3
					WHERE SrId=@Value1
				END
				ELSE
				BEGIN
					UPDATE PM_Tasks
					SET Color=@Value5
					WHERE TaskId=@Value4
				END
			END
			--FETCH THE NEXT RECORD INTO THE VARIABLES.
			FETCH NEXT FROM UpdateC INTO @Value1,@Value2,@Value3,@Value4,@Value5
		END
 
		--CLOSE THE CURSOR.
		CLOSE UpdateC
		DEALLOCATE UpdateC

END
	
END