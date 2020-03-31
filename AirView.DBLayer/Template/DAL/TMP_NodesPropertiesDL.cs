using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Template.DAL
{
    public class TMP_NodesPropertiesDL
    {

        public int Manage(string Filter, decimal FormId, decimal NodeTypeId, string Title, string ControlType, string DataType, string DefaultValue, string MaxLength, string Required, string IsAttachment, int SortOrder,int IsDeleted,string Comments)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_ManageNodesProperties");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@FormId", FormId, "@NodeTypeId", NodeTypeId, "@Title", Title, "@ControlType", ControlType, "@DataType", DataType, "@DefaultValue", DefaultValue, "@MaxLength", MaxLength, "@Required", Required, "@IsAttachment", IsAttachment, "@SortOrder", SortOrder, "@IsDeleted", IsDeleted, "@Comments", Comments));
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
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_GetNodesProperties");
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

        public DataTable Get(string filter, long Value = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_SiteScript_GetNodesProperties");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", Value));
            }
            catch(Exception e)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public int ManageSiteScriptsForm(string Filter, decimal FormId, decimal NodeTypeId, string Title, string ControlType, string DataType, string DefaultValue, string MaxLength, string Required, string IsAttachment, int SortOrder, int IsDeleted,long SrId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageSiteScriptNodesProperties");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@FormId", FormId, "@NodeTypeId", NodeTypeId, "@Title", Title,"@DataType", DataType, "@DefaultValue", DefaultValue, "@MaxLength", MaxLength, "@Required", Required, "@SortOrder", SortOrder, "@IsDeleted", IsDeleted));
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

    }
}
