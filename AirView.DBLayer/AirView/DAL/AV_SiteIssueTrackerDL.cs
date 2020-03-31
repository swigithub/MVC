using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_SiteIssueTrackerDL
    {
        public bool Manage(string Filter ,Int64 TrackingId, Int64 SiteId, Int64 TesterId, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopId, string Description, string Status,string ImagePath,string IssueType,Int64 UserId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageSiteIssueTracker");

                loCommand = DataContext.StartTransaction(loCommand);

                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TrackingId", TrackingId, "@SiteId", SiteId, "@TesterId", TesterId, "@NetworkModeId", NetworkModeId, "@BandId", BandId, "@CarrierId", CarrierId, "@ScopId", ScopId, "@Description", Description, "@Status", Status, "@ImagePath", ImagePath, "@IssueType", IssueType,"@UserId",UserId));
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


        public DataTable Get(string filter, AV_SiteIssueTracker sit)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSiteIssueTracker");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@SiteId", sit.SiteId, "@TesterId", sit.TesterId, "@NetworkModeId",sit.NetworkModeId, "@BandId",sit.BandId, "@CarrierId",sit.CarrierId, "@ScopId",sit.ScopeId, "@Status",sit.Status));
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
