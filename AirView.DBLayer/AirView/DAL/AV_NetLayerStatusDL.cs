using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_NetLayerStatusDL
    {
        public bool Manage(string Filter, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 SiteId, Int64 UserId,string value1=null, string value2 = null, string value3 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageNetLayerStatus");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@NetworkModeId", NetworkModeId,   "@BandId", BandId,"@CarrierId", CarrierId,
                                                            "@SiteId", SiteId, "@UserId", UserId, "@value1", value1,"@value2", value2, "@value3", value3));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch (Exception ex)
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable Get(string filter,Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 SiteId,string @Value1)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetNetLayerStatus");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@SiteId", SiteId, "@NetworkModeId", NetworkModeId, "@BandId", BandId,
                                                                    "@CarrierId", @CarrierId , "@Value1", Value1));
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
