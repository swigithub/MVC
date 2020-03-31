using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AV_GetRoutePlanBL
    {
        AV_GetRoutePlanDL rp = new AV_GetRoutePlanDL();
        public List<AV_GetRoutePlan> ToList(string filter, string TesterId=null)
        {
            DataTable dt = rp.Get(filter, TesterId);
            List<AV_GetRoutePlan> conf = dt.ToList<AV_GetRoutePlan>();
            return conf;
        }
    }
}
