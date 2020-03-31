-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/


create PROCEDURE Sec_GetUserDefinationTypes 
    @Filter nvarchar(500),
	@Value  nvarchar(500),
	@Value2  nvarchar(500)
AS
BEGIN
if @Filter='GetByUserId'
begin
select * from Sec_UserDefinationType where UserId=CONVERT(int,@Value) 
	End
	else if @Filter='GetDefinationTypeByUId'
begin
select dt.DefinationType,dt.DefinationTypeId,dt.PDefinationTypeId from Sec_UserDefinationType udt 
inner join AD_DefinationTypes as dt on dt.DefinationTypeId=udt.DefinationTypeId
 where udt.UserId=CONVERT(int,@Value) 

	End

END