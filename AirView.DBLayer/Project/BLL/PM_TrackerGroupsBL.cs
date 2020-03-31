using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.BLL
{
    public class PM_TrackerGroupsBL
    {
        public List<PM_TrackerGroups> ToList(string Filter, string Value = null)
        {
            PM_TrackerDL tDL = new PM_TrackerDL();
            DataTable dt = tDL.Get(Filter, Value);
            return dt.ToList<PM_TrackerGroups>();
        }
    }
}
