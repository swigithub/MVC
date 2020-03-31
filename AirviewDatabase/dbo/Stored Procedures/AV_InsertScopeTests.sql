create PROCEDURE [dbo].[AV_InsertScopeTests]
	@List Tb_AV_ScopeTests READONLY
AS
BEGIN
	insert into AV_ScopeTests
	select * from @List
END