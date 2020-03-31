-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_ManageFloorPlans]
 @FILTER NVARCHAR(50)
,@List List READONLY,
@IsActive bit= 0,
@PlanId numeric(18,0)=null,
@PlanFile nvarchar(max)=null,
@FloorId numeric(18,0)=null,
@SiteId numeric(18,0)=null


AS
BEGIN
	
	IF @Filter='Save'
	--BEGIN
	--	INSERT INTO AV_FloorPlan (SiteId,FloorId,PlanFile,IsActive)
	--	SELECT CAST(l.Value1 AS numeric(18,0)),CAST(l.Value2 AS numeric(18,0)),l.Value15,1
	--	from @List as l
	--END
	BEGIN
	 --DECLARE THE CURSOR FOR A QUERY.
		  DECLARE FloorSave CURSOR READ_ONLY
		  FOR
		  SELECT l.Value1,l.Value2,l.Value3,l.Value15,l.Value4 FROM @List AS l 
		  --OPEN CURSOR.
		  OPEN FloorSave 
		  --FETCH THE RECORD INTO THE VARIABLES.
		  FETCH NEXT FROM FloorSave INTO @SiteId,@FloorId,@IsActive,@PlanFile,@PlanId
		  --LOOP UNTIL RECORDS ARE AVAILABLE.
		  WHILE @@FETCH_STATUS = 0
		  BEGIN
				IF CAST(@PlanId AS numeric(18,0))=0 
				BEGIN
				INSERT INTO AV_FloorPlan (SiteId,FloorId,PlanFile,IsActive) values(@SiteId,@FloorId,@PlanFile,1)
	       END 
				else 
				BEGIN
				print 'update'
				update AV_FloorPlan set FloorId=@FloorId WHERE PlanId=@PlanId
				END
				 --FETCH THE NEXT RECORD INTO THE VARIABLES.
				 FETCH NEXT FROM FloorSave INTO @SiteId,@FloorId,@IsActive,@PlanFile,@PlanId
		  END
 
		  --CLOSE THE CURSOR.
		  CLOSE FloorSave
		  DEALLOCATE FloorSave

		--update AD_ClientContacts set ContactPerson=@ContactPerson , Designation=@Designation, Gender=@Gender,Title=@Title,ContactNo=@ContactNo,ContactType=@ContactType, IsPrimary=@IsPrimary,ClientId=@ClientId,UserId=@UserId,RegionId=@RegionId,CityId=@CityId,IsActive=@IsActive,ReportToId=@ReportToId where ContactId=@ContactId
		--set	@RETURN_VALUE=@ContactId
	END
	IF @Filter='IsActive'
	BEGIN
		update AV_FloorPlan set IsActive=@IsActive  where PlanId=@PlanId
	END
	
END