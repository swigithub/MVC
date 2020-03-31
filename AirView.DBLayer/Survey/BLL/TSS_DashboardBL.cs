using AirView.DBLayer.Survey.DAL;
using AirView.DBLayer.Survey.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.BLL
{
   public class TSS_DashboardBL
    {
        private TSS_DashboardDL ddl = new TSS_DashboardDL();
        public dynamic Manage(List<TSS_SiteAttendees> SiteAttendees, int siteId, int siteSurveyId)
        {

           
             var dt = new dbDataTable().List();
            foreach (var res in SiteAttendees)
            {
                myDataTable.AddRow(dt, "Value1", res.SiteId, "Value2", res.SiteSurveyId, "Value3", res.Name, "Value4", res.Designation, "Value5", res.Company, "Value6", res.Signature);
            }

            ddl.Manage("SurveyAttendee", dt, siteId, siteSurveyId);
            return true;
         
        }

        public List<TSS_SiteAttendees> GetSiteAttendees(string filter, int siteid, int sitesurveyid)
        {
            var dt = ddl.GetAttendees(filter, siteid, sitesurveyid);
            var attendeeslist = dt.ToList<TSS_SiteAttendees>();
            return attendeeslist;
        }

    }
}
