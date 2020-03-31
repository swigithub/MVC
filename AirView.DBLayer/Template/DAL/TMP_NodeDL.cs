using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Template.DAL
{
    public class TMP_NodeDL
    {

        public int Manage(string Filter, int NodeId, int TemplateId, string NodeTitle, int Height, int Width, int x_axis, int y_axis, int PageTyppeId, string NodeUrl, string NodeSQL, bool IsActive)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_ManageNodes");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@NodeId", NodeId, "@TemplateId", TemplateId, "@NodeTitle", NodeTitle, "@Height", Height, "@Width", Width, "@X_axis", x_axis, "@Y_axis", y_axis, "@PageTyppeId", PageTyppeId, "@NodeUrl", NodeUrl, "@NodeSQL", NodeSQL, "@IsActive", IsActive));
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


        public DataTable Get(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_GetNodes");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", Value));
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
