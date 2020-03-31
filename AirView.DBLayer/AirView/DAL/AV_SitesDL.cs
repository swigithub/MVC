using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.DAL
{
   public class AV_SitesDL
    {
        
        public DataTable Get(string filter, string value,string NetworkModeId=null,string BandId = null, string CarrierId = null, string ScopeId=null,string value2=null,string value3=null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSite");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@NetworkModeId", NetworkModeId, "@BandId", BandId, "@CarrierId", CarrierId, "ScopeId", ScopeId,
                                                                    "@value", value, "@value2",value2, "@value3",value3));
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

        public DataSet GetDataSet(string filter, string value)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSite");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@FILTER", filter, "@value", value));
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

        public bool Manage(string Filer,  string value1 = null, string value2 = null, string value3 = null, string value4 = null, string value5 = null,Int64 UserId=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_SiteManage");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filer, "@Value1", value1, "@Value2", value2, "@Value3", value3, "@Value4", value4, "@Value5", value5, "@UserId", UserId));
                DataContext.EndTransaction(loCommand);
                return result;


            }
            catch(Exception ex)
            {
               
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GetStatus(string filter, string value, string NetworkModeId = null, string BandId = null, string CarrierId = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_SiteManage");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value2", NetworkModeId, "@Value3", BandId, "@Value4", CarrierId, "@Value1", value));
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
    }
}
