using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
  public  class TransferSiteLogsDL
    {
   
        public bool Transfer(string Filter, Int64 SiteId, Int64 LayerStatusId, string Sectors, string Tests, string SiteCode, string Task)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_TransferSiteLogs");

                loCommand = DataContext.StartTransaction(loCommand);

                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SourceSiteId", SiteId, "@LayerStatusId", LayerStatusId, "@Sectors", Sectors, "@Tests", Tests, "@DestSiteCode", SiteCode, "@Tasks", Task));
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

    }
}
