using AirView.DBLayer.Common;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AirView.DBLayer.Project.BLL
{
    public class PM_GazetteHolidaysBL
    {
        PM_GazetteHolidaysDL gh = new PM_GazetteHolidaysDL();
        public bool Manage(string filter, PM_GazetteHolidays g)
        {
            return gh.Manages(filter, g.ProjectId, g.Title, g.Date, g.IsOffday);
        }
        public bool InsertBulk(long? ProjectId, List<PM_GazetteHolidays> pM_GH)
        {
            pM_GH.ForEach(x => x.ProjectId = ProjectId.Value);
            var ModelIntoDataTable = CopyDataTable.CopyToDataTable(pM_GH);
            return gh.Manage("PM_GazetteHolidays", ModelIntoDataTable);
        }

        public List<PM_GazetteHolidays> ToList(string filter, string value, string value2 = null, string value3 = null, string value4 = null, string value5 = null)
        {
            try
            {
                DataTable dt = gh.Get(filter, value, value2, value3, value4);
                List<PM_GazetteHolidays> rec = dt.ToList<PM_GazetteHolidays>();
                return rec;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
