

CREATE PROCEDURE [dbo].[AD_ManageProjectConfiguration] 
	
	@List List READONLY
AS
BEGIN

   delete from AD_ProjectConfigurations where ProjectId=(SELECT top 1 Value1 FROM @List  ) and TypeId=(SELECT top 1 Value2 FROM @List  )

   INSERT INTO AD_ProjectConfigurations(ProjectId,TypeId,TypeValue)
   SELECT l.Value1,l.Value2,l.Value3 FROM @List l
	
END