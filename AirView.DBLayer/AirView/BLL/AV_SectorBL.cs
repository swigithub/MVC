using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SWI.Libraries.AirView.BLL
{
    public class AV_SectorBL
    {
        AV_SectorDL sd = new AV_SectorDL();
        public List<AV_Sector> ToList(string filter, string SiteId, string NetworkModeId = null, string BandId = null, string CarrierId = null, string ScopeId = null)
        {
            try
            {
                DataTable dt = sd.Get(filter, SiteId, NetworkModeId, BandId, CarrierId, ScopeId);
                if (dt!=null)
                {
                    List<AV_Sector> sec = dt.ToList<AV_Sector>();
                    return sec;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
