create PROCEDURE PM_ManageSiteTaskInventory
@Filter VARCHAR(50)='',
@SiteTaskInventoryList PM_SiteTaskInventory READONLY,
@SiteTaskId NUMERIC(18,0)=NULL,
@SiteId NUMERIC(18,0)=NULL,
@UserId NUMERIC(18,0)=NULL,
@SiteTaskInventoryId NUMERIC(18,0)=NULL,
@FileName VARCHAR(50)='',
@SubDirectory VARCHAR(50)='',
@ContentLength INT=0
AS
BEGIN
	
	
	IF @Filter='Add_SiteTask_Inventory'
	BEGIN
	
		DECLARE @TempSiteTaskId NUMERIC(18,0)=NULL
		DECLARE @TempSiteId NUMERIC(18,0)=NULL
		DECLARE @TempSiteTaskInventoryId NUMERIC(18,0)=NULL
		DECLARE @TempCategoryId NUMERIC(18,0)=NULL
		DECLARE @TempItem VARCHAR(50)=NULL
		DECLARE @TempQuantity INT=NULL
		DECLARE @TempBarCode VARCHAR(50)=NULL
		DECLARE @TempDescription VARCHAR(300)=NULL
		DECLARE @IsModified INT=NULL

		-- Creating Temp Table
			DECLARE @TempTable AS TABLE
			(
				SubDirectory VARCHAR(50),
				InventoryId  NUMERIC(18,0)
			)
		--

		DECLARE INVENTORY_CURSOR CURSOR FOR 
		SELECT Value1,Value2,Value3,Value4,Value5,Value6,Value7,Value8,Value11 
		FROM  @SiteTaskInventoryList

		OPEN INVENTORY_CURSOR

		FETCH NEXT FROM INVENTORY_CURSOR
		INTO @TempSiteTaskId,@TempCategoryId,@TempSiteId,@TempSiteTaskInventoryId,@IsModified,@TempItem,@TempQuantity,@TempBarCode,@TempDescription

		WHILE @@FETCH_STATUS = 0    
		BEGIN 

			IF @TempSiteTaskInventoryId<>0 
			BEGIN
				IF @IsModified<>0
				BEGIN
					UPDATE PM_SiteTaskInventory SET 
					CategoryId=@TempCategoryId,
					ItemId=@TempItem,
					Quantity=@TempQuantity,
					BarCode=@TempBarCode,
					Description=@TempDescription,
					ModifiedBy=@UserId,
					ModifiedOn=GETDATE()
			

					WHERE SiteTaskInventoryId=@TempSiteTaskInventoryId and 
						  SiteTaskId=@TempSiteTaskId and 
						  SiteId=@TempSiteId

				END
			END
			ELSE
			BEGIN

				INSERT INTO PM_SiteTaskInventory
						  (SiteTaskId, CategoryId, SiteId, ItemId, Quantity, BarCode, Description,CreatedBy,CreatedOn)
				VALUES(	@TempSiteTaskId,@TempCategoryId,@TempSiteId, @TempItem,@TempQuantity,@TempBarCode,@TempDescription,@UserId,GETDATE())

				-- INSERTING TO TEMP TABLE

				DECLARE @NewInventoryID NUMERIC(18,0)=SCOPE_IDENTITY()
				INSERT INTO @TempTable VALUES(Convert(varchar(50),@TempCategoryId)+'_'+Convert(varchar(50),@TempItem),@NewInventoryID)

				--
			END

			FETCH NEXT FROM INVENTORY_CURSOR
			INTO @TempSiteTaskId,@TempCategoryId,@TempSiteId,@TempSiteTaskInventoryId,@IsModified,@TempItem,@TempQuantity,@TempBarCode,@TempDescription

		END

		CLOSE INVENTORY_CURSOR
		DEALLOCATE INVENTORY_CURSOR
		-- UPDATING ATTACHMENTS RELATION ID

		DECLARE @TempSubDirectory VARCHAR(50)
		DECLARE @TempNewInventoryId NUMERIC(18,0)

		DECLARE ATTACHMENT_CURSOR CURSOR FOR
		SELECT SubDirectory,InventoryId
		FROM @TempTable

		OPEN ATTACHMENT_CURSOR

		FETCH NEXT FROM ATTACHMENT_CURSOR
		INTO @TempSubDirectory,@TempNewInventoryId

		WHILE @@FETCH_STATUS=0
		BEGIN
			UPDATE PM_SiteTaskInventoryAttachments 
			SET
			   SiteTaskInventoryId=@TempNewInventoryId,
			   SubDirectory=@TempSubDirectory

			WHERE SubDirectory=@TempSubDirectory
	
			FETCH NEXT FROM ATTACHMENT_CURSOR
			INTO @TempSubDirectory,@TempNewInventoryId
		END

		CLOSE ATTACHMENT_CURSOR
		DEALLOCATE ATTACHMENT_CURSOR
		
		SELECT SubDirectory,InventoryId FROM @TempTable
			
	END
	IF @Filter='Add_Attachments'
	BEGIN

		INSERT INTO PM_SiteTaskInventoryAttachments(SiteTaskInventoryId,FileNameWithExtension,SubDirectory,ContentLength)
		VALUES(@SiteTaskInventoryId,@FileName,@SubDirectory,@ContentLength)

		SELECT SiteTaskInventoryAttachmentId
		FROM PM_SiteTaskInventoryAttachments
		WHERE SiteTaskInventoryId=@SiteTaskInventoryId AND SubDirectory=@SubDirectory

	END
	IF @Filter='DeleteAttachmentById'
	BEGIN
		IF @SiteTaskInventoryId<>0
		BEGIN
			DELETE FROM PM_SiteTaskInventoryAttachments
			WHERE SiteTaskInventoryId=@SiteTaskInventoryId AND FileNameWithExtension=@FileName
		END

		ELSE
		BEGIN
			DELETE FROM PM_SiteTaskInventoryAttachments
			WHERE SubDirectory=@SubDirectory
		END
	END
	IF @Filter='DeleteSiteTaskInventory'
	BEGIN
		DELETE FROM PM_SiteTaskInventoryAttachments
		WHERE SiteTaskInventoryId=@SiteTaskInventoryId

		DELETE FROM PM_SiteTaskInventory
		WHERE SiteTaskInventoryId=@SiteTaskInventoryId
	END
END