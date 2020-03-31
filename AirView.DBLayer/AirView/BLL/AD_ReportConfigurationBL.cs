using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AD_ReportConfigurationBL
    {
        AD_ReportConfigurationDL rc = new AD_ReportConfigurationDL();
        public List<AD_ReportConfiguration> ToList(string filter, string value = null, string value2 = null)
        {

            DataTable dt = rc.Get(filter, value, value2 );
            List<AD_ReportConfiguration> conf = dt.ToList<AD_ReportConfiguration>();
            
            return conf;
        }
    }

  
}
