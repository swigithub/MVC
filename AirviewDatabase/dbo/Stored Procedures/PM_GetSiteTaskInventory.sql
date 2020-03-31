create PROCEDURE PM_GetSiteTaskInventory
@Filter VARCHAR(50)='',
@UETypeId NUMERIC(18,0)=null,
@SiteId NUMERIC(18,0)=null,
@SiteTaskId NUMERIC(18,0)=null,
@SiteTaskInventoryId NUMERIC(18,0)=null
AS
BEGIN

	IF @Filter='Get_UE_Type'
	BEGIN
	
		SELECT DefinationId as TypeId,DefinationName as TypeName
		FROM AD_Definations 
		WHERE DefinationTypeId=32

	END

	ELSE IF @Filter='Get_UE_Items'
	BEGIN

		SELECT UEId,Manufacturer+' | '+Model+' | '+SerialNo as ItemName
		FROM	AD_UserEquipments 
		WHERE UETypeId	=@UETypeId
 
	END

	ELSE IF @Filter='Get_SiteTask_Inventory'
	BEGIN

		SELECT SiteTaskInventoryId, SiteTaskId, CategoryId, ItemId, Quantity, BarCode, Description
		FROM	PM_SiteTaskInventory
		WHERE SiteTaskId=@SiteTaskId and SiteId=@SiteId

	END

	ELSE IF @Filter='Get_SiteTask_Inventory_Attachments'
	BEGIN

		SELECT SiteTaskInventoryAttachmentId, FileNameWithExtension, SiteTaskInventoryId,ContentLength
		FROM     PM_SiteTaskInventoryAttachments
		WHERE SiteTaskInventoryId=@SiteTaskInventoryId

	END

	ELSE IF @Filter='Get_Header_Info'
	BEGIN
		SELECT	TOP(1) PS.SiteCode AS BusCode,T.Title AS TaskName,
			 CONVERT(varchar,STI.ModifiedOn,107) AS UpdatedOn,
			 ISNULL(UU.FirstName+' '+UU.LastName,'') AS UpdatedBy,
			 CONVERT(varchar,STI.CreatedOn,107) AS CreatedOn,
			 ISNULL(CU.FirstName+' '+CU.LastName,'') AS CreatedBy
		FROM PM_SiteTaskInventory STI 
			 INNER JOIN PM_ProjectSites PS ON PS.ProjectSiteId=STI.SiteId
			 INNER JOIN PM_Tasks T ON T.TaskId=STI.SiteTaskId
			 INNER JOIN Sec_Users CU ON CU.UserId=STI.CreatedBy
			 LEFT JOIN Sec_Users UU ON UU.UserId=STI.ModifiedBy 
		WHERE STI.SiteId=@SiteId and STI.SiteTaskId=@SiteTaskId
		ORDER BY SiteTaskInventoryId DESC
	END


END