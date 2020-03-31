using SWI.Libraries.Common;
using System;

using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
    public class AV_WoTrackerDL
    {
        public bool Save(Int64 SiteId, Int64 SectorId, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, string WoRefId, double Latitude, double Longitude, Int64 TesterId, string TestType,string AppVersion,string AndroidVersion, string IMEI=null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_InsertWoTracker");

                loCommand = DataContext.StartTransaction(loCommand);

                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@SiteId",SiteId, "@SectorId",SectorId, "@NetworkModeId",NetworkModeId, "@BandId",BandId, "@CarrierId",CarrierId, "@WoRefId",WoRefId, "@Latitude",Latitude, "@Longitude",Longitude, "@TesterId",TesterId, "@TestType",TestType, "@AppVersion", AppVersion, "@AndroidVersion", AndroidVersion, "@IMEI", IMEI));
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


        public DataTable Get(string Filter, string SiteId = null, string NetworkModeId = null, string BandId = null, string CarrierId = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetWoTracker");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId, "@NetworkModeId",NetworkModeId, "@BandId", BandId, "@CarrierId", CarrierId));
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
