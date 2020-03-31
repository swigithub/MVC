using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AV_TestBL
    {
        AV_TestDL td = new AV_TestDL();
        public List<AV_Test> ToList(string SiteId)
        {

            DataTable dt = td.Get(SiteId);
            List<AV_Test> conf = dt.ToList<AV_Test>();

            return conf;
        }
    }
}
