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
    public class PM_TrackerBL
    {
        public bool Manage(string Filter, string Value, DataTable List)
        {
            PM_TrackerDL td = new PM_TrackerDL();
            return td.Manage( Filter,Value,  List);

        }
        public List<PM_Tracker> ToList(string Filter, string Value = null,string Value2="")
        {
            PM_TrackerDL tDL = new PM_TrackerDL();
            DataTable dt = tDL.Get(Filter, Value,Value2);
            return dt.ToList<PM_Tracker>();
        }
    }
}
