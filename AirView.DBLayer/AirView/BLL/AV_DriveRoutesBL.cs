using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AV_DriveRoutesBL
    {
        AV_DriveRoutesDL drd = new AV_DriveRoutesDL();
        public int Manage(string filter, AV_DriveRoutes dr)
        {
            return drd.Manage(filter,Convert.ToInt64( dr.RouteId),dr.RoutePath, Convert.ToInt64(dr.CreatedBy), Convert.ToInt64(dr.SiteId), Convert.ToInt64(dr.ScopeId),dr.TestType,dr.IsSelected);
        }

        public List<AV_DriveRoutes> ToList(string filter, string value = null, string value1 = null, string value2 = null)
        {
            DataTable dt = drd.Get(filter, value, value1, value2);
            List<AV_DriveRoutes> conf = dt.ToList<AV_DriveRoutes>();
            return conf;
        }
    }
}
