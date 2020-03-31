-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE AV_ManageSiteTaskAttachment
 @FILTER NVARCHAR(50)
,@List List READONLY,
@AttachmentId numeric(18,0)=0,
@FileName nvarchar(max)='',
@SiteTaskId numeric(18,0)=0,
@IsDeleted bit=0,
@CategoryId numeric(18,0)=0,
@ModifiedBy numeric(18,0)=0,
@Tags nvarchar(150)='',
@Description nvarchar(500) =''

AS
BEGIN
	
	IF @Filter='Save'
	
	BEGIN
	 --DECLARE THE CURSOR FOR A QUERY.
		  DECLARE Attachments CURSOR READ_ONLY
		  FOR
		  SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8 FROM @List AS l 
		  --OPEN CURSOR.
		  OPEN Attachments 
		  --FETCH THE RECORD INTO THE VARIABLES.
		  FETCH NEXT FROM Attachments INTO @AttachmentId,@SiteTaskId,@FileName,@Description,@Tags,@CategoryId,@ModifiedBy,@IsDeleted
		  --LOOP UNTIL RECORDS ARE AVAILABLE.
		  WHILE @@FETCH_STATUS = 0
		  BEGIN
				IF CAST(@AttachmentId AS numeric(18,0))=0 
				BEGIN
				print '--new--'
				INSERT INTO pm_sitetaskattachment (SiteTaskId,[FileName],[Description],Tags,CategoryId,CreatedOn,CreateBy,ModifiedOn,ModifiedBy,IsDeleted)
				 values(@SiteTaskId,@FileName,@Description,@Tags,@CategoryId,GETDATE(),@ModifiedBy,GETDATE(),@ModifiedBy,@IsDeleted)
	       END 
				else 
				BEGIN
				print '--update--'
				update pm_sitetaskattachment set [Description]=@Description,Tags=@Tags,CategoryId=@CategoryId,ModifiedBy=@ModifiedBy
				,ModifiedOn=GETDATE(),IsDeleted=@IsDeleted
				 WHERE AttachmentId=@AttachmentId
				END
				 --FETCH THE NEXT RECORD INTO THE VARIABLES.
				 FETCH NEXT FROM Attachments INTO @AttachmentId,@SiteTaskId,@FileName,@Description,@Tags,@CategoryId,@ModifiedBy,@IsDeleted
		  END
 
		  --CLOSE THE CURSOR.
		  CLOSE Attachments
		  DEALLOCATE Attachments

	END
	IF @Filter='GetAttachments'
	
	BEGIN
	select *,FileName 'file' from PM_SiteTaskAttachment where SiteTaskId=@SiteTaskId and IsDeleted=0

	END
END