using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
    public class AV_TestDL
    {
        public DataTable Get(string SiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_Test");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@SiteId", SiteId));
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
        public int CountTestsofsite(string Filter, Int64 SiteId , Int64 LayerStatusId=0, string Sectors=null, string Tests=null, string SiteCode=null,string Task=null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_TransferSiteLogs");

                loCommand = DataContext.StartTransaction(loCommand);

                bool result =DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SourceSiteId", SiteId, "@LayerStatusId", LayerStatusId,  "@Sectors", Sectors, "@Tests", Tests, "@DestSiteCode", SiteCode, "@Tasks", Task));
                DataContext.EndTransaction(loCommand);
                return (int)loCommand.ExecuteScalar();

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
    }
}
