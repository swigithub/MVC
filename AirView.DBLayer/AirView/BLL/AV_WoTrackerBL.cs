using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/

    public class AV_WoTrackerBL
    {
        AV_WoTrackerDL wtd = new AV_WoTrackerDL();
        public List<AV_WoTracker> ToList(string Filter, string SiteId = null, string NetworkModeId = null, string BandId = null, string CarrierId = null)
        {
            try
            {
                DataTable dt =wtd.Get( Filter,  SiteId,  NetworkModeId ,  BandId ,  CarrierId );
                List<AV_WoTracker> tracker = dt.ToList<AV_WoTracker>();
                return tracker;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
