-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
/*----MoB!----*/
CREATE PROCEDURE AV_Get_Region
	@value varchar(max) = null
AS
BEGIN
	--select def.DefinationId,  def2.DefinationName +' ' +def.DefinationName as Region from AD_Definations def
	--left join AD_Definations def2 on def2.DefinationId=def.PDefinationId
	--where def2.DefinationTypeId=4
	select distinct def.DefinationId,  def2.DefinationName +' ' +def.DefinationName as Region from AD_Definations def
	left join AD_Definations def2 on def2.DefinationId=def.PDefinationId
	inner join Sec_UserDefinationType udt on udt.DefinationTypeId = def.DefinationTypeId
	inner join Sec_Users u on u.UserId = udt.UserId
	where def2.DefinationTypeId=4 and (u.UserId = @value or @value=0 )
END