using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Template.DAL
{
    public class TMP_TemplatesDL
    {
        public int Manage(string Filter, int TemplateId, string TemplateTitle, int ProjectId, int ScopeId, string backgroundColor, string pageType, string parameters, bool IsActive, string TemplateType, bool? IsDefault, int? ModuleId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_ManageTemplates");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TemplateId", TemplateId, "@TemplateTitle", TemplateTitle, "@ProjectId", ProjectId, "@backgroundColor", backgroundColor, "@ScopeId", ScopeId, "@pageType", pageType, "@parameter", parameters, "@IsActive", IsActive, "@TemplateType", TemplateType, "@IsDefault", IsDefault, "@ModuleId", ModuleId));
                DataContext.EndTransaction(loCommand);
                int result = Convert.ToInt32(loCommand.Parameters["@RETURN_VALUE"].Value);
                return result;
            }
            catch (Exception)
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }


        public DataTable Get(string filter, string Value = null, string ProjectId = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_GetTemplates");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", Value, "@ProjectId", ProjectId));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    }
}
