-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
/*
 
 
 */
CREATE PROCEDURE [dbo].[AD_ManageDefinations]
 @Filter nvarchar(50)
,@List List READONLY
	
AS
BEGIN
	
   if @Filter='Insert'
   BEGIN
   		DECLARE @DefinationName AS NVARCHAR(250)
   		DECLARE @PDefinationId AS NUMERIC
   		
   		SELECT @DefinationName=x.Value2, @PDefinationId=x.Value3 FROM @List x
   		
   		IF NOT EXISTS(SELECT ad.DefinationId FROM AD_Definations AS ad WHERE ad.DefinationName=@DefinationName AND ad.PDefinationId=@PDefinationId)
   		BEGIN
			INSERT INTO AD_Definations(DefinationName,PDefinationId,DefinationTypeId,KeyCode,DisplayType,ColorCode,InputType,[MaxLength],SortOrder,IsActive,DisplayText)
			SELECT l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12 FROM @List l
   		END
   		ELSE
   		BEGIN
   			RAISERROR('Record Already Exists!',16,1)
   		END
   END
END