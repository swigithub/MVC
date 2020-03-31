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
    class AV_MarketSitesDL
    {
        public DataTable Get(string filter,int SiteId,string SiteCode, double Latitude,double Longitude,long CityId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetMarketSites");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@SiteId",SiteId, "@SiteCode",SiteCode, "@Latitude",Latitude, "@Longitude",Longitude, "@CityId",CityId));
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
