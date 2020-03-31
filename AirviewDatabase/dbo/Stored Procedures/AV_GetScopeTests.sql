-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AV_GetScopeTests]
 @Filter nvarchar(50)
,@ClientId numeric(18, 0)=null
,@CityId numeric(18, 0)=null
,@NetworkModeId numeric(18, 0)=null
,@ScopeId numeric(18, 0)=null
AS
BEGIN
--  [dbo].[AV_GetScopeTests] 'byClientId_CityId_ScopeId',1,468,0,36
	if @Filter='byClientId_CityId_ScopeId'
	begin
		select st.*,test.DefinationName 'Test'
		from AV_ScopeTests st
		inner join AD_Definations test on test.DefinationId=st.TestTypeId
		where st.ClientId=@ClientId --and st.CityId=@CityId 
		and st.ScopeId=@ScopeId
	end

 -- [dbo].[AV_GetScopeTests] 'byNetwokModeId_ScopeId_ClientId_CityId',1,103,15,13152
	if @Filter='byNetwokModeId_ScopeId_ClientId_CityId'
	begin
		select st.*,test.DefinationName,test.KeyCode,test.DisplayText
		from AV_ScopeTests st
		inner join AD_Definations test on test.DefinationId=st.TestTypeId
		where st.NetworkModeId=@NetworkModeId and st.ScopeId=@ScopeId and st.ClientId=@ClientId --and st.CityId=@CityId
		order by test.SortOrder
	end
END