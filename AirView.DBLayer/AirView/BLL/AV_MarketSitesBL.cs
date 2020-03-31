using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
   public  class AV_MarketSitesBL
    {
      
        AV_MarketSitesDL msd = new AV_MarketSitesDL();
        public List<AV_MarketSites> ToList(string filter, int SiteId, string SiteCode, double Latitude, double Longitude,long CityId)
        {
            try
            {
                DataTable dt = msd.Get(filter, SiteId, SiteCode, Latitude, Longitude, CityId);
                List<AV_MarketSites> Markets = dt.ToList<AV_MarketSites>();
                return Markets;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
