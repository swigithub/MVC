using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
    public class AV_WoDevicesDL
    {
        public bool Manage(int WoDeviceId,int BandId, int CarrierId,  bool IsDownlaoded, int NetworkId, int UserId, int UserDeviceId, int SiteId, int ScopeId, string WoRefId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageWoDevices");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@WoDviceId",WoDeviceId, "@BandId", BandId, "@CarrierId", CarrierId,  "@IsDownlaoded", IsDownlaoded, "@NetworkId", NetworkId, "@UserId", UserId, "@UserDeviceId", UserDeviceId, "@SiteId", SiteId, "@ScopeId", ScopeId, "@WoRefId", WoRefId));
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



        public DataTable Get(string Filter, Int64 SiteId, Int64 NetworkId, Int64 BandId, Int64 CarrierId, Int64 ScopeId,Int64 UserId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetWoDevices");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId, "@NetworkId", NetworkId,"@BandId", BandId, "@CarrierId", CarrierId,
                                                                    "@ScopeId",ScopeId, "@UserId", UserId));
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
