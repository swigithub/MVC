-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE PM_GetEntities
	-- Add the parameters for the stored procedure here
@Filter varchar(50),
@value varchar(50)
AS
BEGIN

IF @Filter ='ProjectEntities'
begin

select * from PM_ProjectEntity 
End
END