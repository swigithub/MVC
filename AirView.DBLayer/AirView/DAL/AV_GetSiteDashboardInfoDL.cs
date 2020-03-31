using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.DAL
{
    
    public class AV_GetSiteDashboardInfoDL
    {
        public DataSet GetDataSet(Int64 SiteId, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId,string FilterOption,string Sector)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSiteDashboardInfo");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@SiteId", SiteId, "@NetworkModeId", NetworkModeId, "@BandId", BandId, "@CarrierId", CarrierId, "@ScopeId", ScopeId, "@Sector", Sector, "@FilterOption", FilterOption));
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
