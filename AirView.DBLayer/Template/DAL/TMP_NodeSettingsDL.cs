using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Template.DAL
{
    public class TMP_NodeSettingsDL
    {
        public int Manage(string Filter, decimal NodeSettingsId, decimal NodeId, decimal DefinationId, string KeyName, string MappedId, string Value, string Settings, string SortOrder, string QueryWhereClause)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_ManageNodeSettings");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@NodeSettingsId", NodeSettingsId, "@NodeId", NodeId, "@DefinationId", DefinationId, "@KeyName", KeyName, "@MappingId", MappedId, "@Value", Value, "@Settings", Settings, "@SortOrder", SortOrder, "@QueryWhereClause", QueryWhereClause));
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
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_GetNodeSettings");
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
