-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
/*
 
 
 */
CREATE PROCEDURE [dbo].[AD_InsertDefinations]
	@List Tbl_AD_Definations READONLY
	
AS
BEGIN
	
   
   insert into AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,DisplayType,ColorCode,InputType,[MaxLength],SortOrder,IsActive,DisplayText)
   SELECT * FROM @List
   
	

END