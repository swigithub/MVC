-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TSS_ManageSiteContacts]
@Filter NVARCHAR(50)
,@List List READONLY
,@SiteId NUMERIC(18,0)=NULL,
@IsSpecialAccess BIT=0,
@DateTime DateTime=NULL,
@Instruction VARCHAR(500)=NULL
AS
BEGIN
	IF @Filter='Insert'
	BEGIN
		
		DECLARE @GetSiteId NUMERIC(18,0)=(SELECT TOP(1) Value1 FROM @List)

		DELETE FROM TSS_SiteContacts 
		WHERE SiteId=@GetSiteId

		INSERT INTO TSS_SiteContacts (SiteId,Title,Gender,FullName,GateNo,ContactNo,ContactTypeID,DesignationID,IsHoldingKeys,Comment)
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value15 FROM @List l	
		-- Update Av_Sites For Access Information
		
		UPDATE AV_Sites 
		SET
		IsSpecialAccess=@IsSpecialAccess,
		AccessDateTime=@DateTime,
		AccessInstructions=@Instruction
		WHERE SiteId=@GetSiteId
		--
	END
	IF @Filter='Get_Site_Contact'
	BEGIN
		SELECT SrId,SiteId,Title,Gender,FullName,GateNo,ContactNo,CType.DefinationName as ContactType,Designation.DefinationName Designation,IsHoldingKeys,Comment 
		FROM TSS_siteContacts SC
		LEFT JOIN AD_Definations CType on CType.DefinationId= SC.ContactTypeID
		LEFT JOIN AD_Definations Designation on Designation.DefinationId= SC.DesignationID
		WHERE SiteId=@SiteId
	END

	IF @Filter='GET_Site_Contact_For_Edit'
	BEGIN
	SELECT * 
		FROM TSS_siteContacts WHERE SiteId=@SiteId
	END

	IF @Filter='GET_Site_Access_Info'
	BEGIN
		SELECT IsSpecialAccess,AccessDateTime,AccessInstructions 
		FROM AV_Sites 
		WHERE SiteId=@SiteId
	END
	IF @Filter='Get_Site_AccessInfo_API'
	BEGIN
		SELECT IsSpecialAccess,AccessDateTime,AccessInstructions 
		FROM AV_Sites 
		WHERE SiteId=@SiteId
	END

END