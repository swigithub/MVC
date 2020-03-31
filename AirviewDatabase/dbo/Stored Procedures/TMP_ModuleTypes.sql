-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE TMP_ModuleTypes
	@ModuleType nvarchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @ModuleType = 'report'
		select  * from AD_Definations where PDefinationId = 233510
	ELSE IF @ModuleType = 'dashboard'
		Select * from AD_Definations where PDefinationId  = 233509
END