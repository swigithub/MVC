using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
    public class ClientsDL
    {
        public bool Insert(DataTable Clients)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "insert_client");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@List", Clients));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch
            {
                // DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public int Manage(string Filter, decimal? ClientId, string ClientName="", bool IsActive=true,string Logo="", decimal ClientTypeId=0, decimal? PClientId=0, string ClientPrefix="")
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageClients");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ClientId", ClientId, "@ClientName", ClientName, "@IsActive", IsActive,"@Logo",Logo, "@ClientTypeId", ClientTypeId
                    , "@PClientId", PClientId, "@ClientPrefix", ClientPrefix));
                DataContext.EndTransaction(loCommand);
                int result = Convert.ToInt32(loCommand.Parameters["@RETURN_VALUE"].Value);
                return result;
            }
            catch
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get(string filter,string Value = null,string Value1=null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetClients");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@value", Value,"@value1",Value1));
            }
            catch 
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataSet GetDataSet(string filter, string Value1 = null, string Value2 = null, string Value3 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetClients");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@FILTER", filter, "@value", Value1, "@Value2", Value2, "@Value3", Value3));
            }
            catch(Exception ex)
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
