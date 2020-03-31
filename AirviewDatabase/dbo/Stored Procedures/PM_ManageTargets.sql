-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PM_ManageTargets
 @Filter NVARCHAR(50)
,@Value nvarchar(50)=NULL

,@List List READONLY
AS
BEGIN
	
	DECLARE @RETURN_VALUE int = 0 	
	
	IF @Filter='Insert'
	BEGIN
		DECLARE @ProjectId AS NUMERIC(18,0)
		DECLARE @MilestoneId AS NUMERIC(18,0)	
		DECLARE @StageId AS NUMERIC(18,0)
		DECLARE @TargetType AS NVARCHAR(15)
		DECLARE @TargetDate AS DATETIME
		DECLARE @TargetValue AS NVARCHAR(15)
		DECLARE @SiteCount AS INT
		DECLARE @UserId AS NUMERIC(18,0)
					
		--"Value1": tr.ProjectId, "Value2": tr.MilestoneId, "Value3": tr.StageId, 
		--"Value4": tr.TargetType, "Value5": tr.StartDate, "Value6": date/weekno/mno/qno/yno, "Value7": sitecount, "Value8": tr.UserId);
		
		DECLARE db_cluster2 CURSOR FOR  
		SELECT l.Value1,l.Value2,l.Value3,LOWER(l.Value4),l.Value5,l.Value6,l.Value7,l.Value8 FROM @List AS l
		OPEN db_cluster2 
		FETCH NEXT FROM db_cluster2 INTO @ProjectId, @MilestoneId, @StageId,@TargetType, @TargetDate, @TargetValue,@SiteCount,@UserId
		WHILE @@FETCH_STATUS = 0   
		BEGIN   
			DECLARE @tmpTargetValue AS NVARCHAR(15)=''
			DECLARE @tmpTargetYear AS INT=0
			DECLARE @tmpTargetStartDate as datetime
			DECLARE @tmpTargetEndDate as datetime
			
			SELECT ROW_NUMBER() OVER (ORDER BY ss.item) 'rowId',ss.item
			INTO #temp
			FROM dbo.SplitString(@TargetValue,'-') AS ss
			
			IF @TargetType='week'
			BEGIN
				SET @tmpTargetValue=(SELECT REPLACE(x.item,'W','') FROM #temp x WHERE x.rowId=2)	
				SET @tmpTargetYear=(SELECT REPLACE(x.item,'W','') FROM #temp x WHERE x.rowId=1)			
				
				SELECT @tmpTargetStartDate=DATEADD(wk, DATEDIFF(wk, 6, '1/1/' + CAST(@tmpTargetYear AS CHAR(4))) + (CAST(REPLACE(@tmpTargetValue,'W','') as int)-1), 6);
				SELECT @tmpTargetEndDate=DATEADD(wk, DATEDIFF(wk, 5, '1/1/' +  CAST(@tmpTargetYear AS CHAR(4))) + (CAST(REPLACE(@tmpTargetValue,'W','') as int)-1), 5);				
			END
			ELSE IF @TargetType='month'
			BEGIN
				SET @tmpTargetValue=(SELECT 
										CASE WHEN x.item='Jan' THEN '1' WHEN x.item='Feb' THEN '2' WHEN x.item='Mar' THEN '3' WHEN x.item='Apr' THEN '4'
											 WHEN x.item='May' THEN '5' WHEN x.item='Jun' THEN '6' WHEN x.item='Jul' THEN '7' WHEN x.item='Aug' THEN '8'
											 WHEN x.item='Sep' THEN '9' WHEN x.item='Oct' THEN '10' WHEN x.item='Nov' THEN '11' WHEN x.item='Dec' THEN '12'
										ELSE NULL
										END
				                     FROM #temp x
				                     WHERE x.rowId=2)
				SET @tmpTargetYear=(SELECT x.item FROM #temp x WHERE x.rowId=1)	

				SELECT @tmpTargetStartDate=dateadd(month, CAST(@tmpTargetValue as int) - 1, dateadd(year, @tmpTargetYear - 1900, 0));
				SELECT @tmpTargetEndDate=dateadd(month, CAST(@tmpTargetValue as int), dateadd(year, @tmpTargetYear - 1900, -1));
			END
			ELSE IF @TargetType='quarter'
			BEGIN
				SET @tmpTargetValue=(SELECT REPLACE(x.item,'Q','') FROM #temp x WHERE x.rowId=2)	
				SET @tmpTargetYear=(SELECT REPLACE(x.item,'Q','') FROM #temp x WHERE x.rowId=1)	

				SELECT @tmpTargetStartDate=DATEADD(QUARTER, CAST(REPLACE(@tmpTargetValue,'Q','') as int) - 1, CAST(@tmpTargetYear AS CHAR(4)));
				SELECT @tmpTargetEndDate=DATEADD(DAY, -1, DATEADD(QUARTER, CAST(REPLACE(@tmpTargetValue,'Q','') as int), CAST(@tmpTargetYear AS CHAR(4))));
			END
			ELSE IF @TargetType='year'
			BEGIN
				SELECT @tmpTargetStartDate=DATEADD(yy, DATEDIFF(yy, 0, @TargetDate), 0)
				SELECT @tmpTargetEndDate=DATEADD(yy, DATEDIFF(yy, 0, @TargetDate) + 1, -1)
				
				SET @tmpTargetValue=@TargetValue
			END
			
			--SET @tmpTargetYear=(SELECT x.item FROM #temp x WHERE x.rowId=2)
			DROP TABLE #temp
			
			DECLARE @maxRevisionId AS INT=0
			
			IF @TargetType='day'
			BEGIN
				IF NOT EXISTS (SELECT pt.SrId FROM PM_Targets AS pt WHERE pt.ProjectId=@ProjectId AND pt.MilestoneId=@MilestoneId AND pt.TargetType=@TargetType AND pt.TargetDate=@TargetDate)
				BEGIN
					SET @maxRevisionId=0;
				END
				ELSE
				BEGIN
					SELECT @maxRevisionId=ISNULL(MAX(pt.RevisionId),0)+1 FROM PM_Targets AS pt WHERE pt.ProjectId=@ProjectId AND pt.MilestoneId=@MilestoneId AND pt.TargetType=@TargetType AND pt.TargetDate=@TargetDate
				END
			END
			ELSE
			BEGIN
				IF NOT EXISTS (SELECT pt.SrId FROM PM_Targets AS pt WHERE pt.ProjectId=@ProjectId AND pt.MilestoneId=@MilestoneId AND pt.TargetType=@TargetType AND pt.TargetValue=@tmpTargetValue AND pt.TargetYear=@tmpTargetYear)
				BEGIN
					SET @maxRevisionId=0;
				END
				ELSE
				BEGIN
					SELECT @maxRevisionId=ISNULL(MAX(pt.RevisionId),0)+1 FROM PM_Targets AS pt WHERE pt.ProjectId=@ProjectId AND pt.MilestoneId=@MilestoneId AND pt.TargetType=@TargetType AND pt.TargetValue=@tmpTargetValue AND pt.TargetYear=@tmpTargetYear
				END
			END

			INSERT INTO [dbo].[PM_Targets] ([ProjectId],[MilestoneId],[StageId] ,[TargetType],[TargetDate] ,[TargetValue],[SiteCount],[UserId] ,[CreatedOn],RevisionId,TargetYear,TargetFromDate,TargetEndDate)
			SELECT @ProjectId, @MilestoneId, @StageId, @TargetType, CASE WHEN @TargetType='day' THEN @TargetDate ELSE NULL END, CASE WHEN @TargetType='day' THEN NULL ELSE @tmpTargetValue END, @SiteCount ,@UserId, GETDATE(),@maxRevisionId,@tmpTargetYear,@tmpTargetStartDate,@tmpTargetEndDate
			
		FETCH NEXT FROM db_cluster2 INTO @ProjectId, @MilestoneId, @StageId,@TargetType, @TargetDate, @TargetValue,@SiteCount,@UserId
		END   
		CLOSE db_cluster2   
		DEALLOCATE db_cluster2
		
		
		
	
	SELECT @RETURN_VALUE = SCOPE_IDENTITY()	
	END

END

--@Value1: ProjectId, @Filter: Upload