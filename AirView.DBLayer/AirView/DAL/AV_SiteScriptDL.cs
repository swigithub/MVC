using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_SiteScriptDL
    {
        public bool Manage(string Filter,DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageSiteScript");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List",dt));
                DataContext.EndTransaction(loCommand);
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

        public bool CLSScript(string Filter, DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageSiteScript");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", dt));
                DataContext.EndTransaction(loCommand);
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

        public DataTable Get(string filter, string value, string value2 = null, string value3 = null, string value4 = null, string value5 = null,string value6=null)
        {



            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSiteScript");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value1", value, "@Value2", value2, "@Value3", value3, "@Value4", value4, "@Value5", value5, "@Value6", value6));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataSet _Get(string filter, string value, string value2 = null, string value3 = null, string value4 = null, string value5 = null, string value6 = null)
        {



            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSiteScript");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value1", value, "@Value2", value2, "@Value3", value3, "@Value4", value4, "@Value5", value5, "@Value6", value6));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public dynamic _Manage(string Filter, DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageSiteScriptNodesProperties");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", dt));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    }
}
