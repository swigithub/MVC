using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SWI.Libraries.Common;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.BLL
{
    public class PM_TargetsBL
    {
        PM_TargetsDL tdl = new PM_TargetsDL();
        public List<PM_Targets> ToList(string Filter, string value1 = null, string value2 = null, Int64 ProjectId = 0, Int64 TargetId = 0)
        {
            DataTable dt = tdl.GetDataTable(Filter, value1, value2, ProjectId, TargetId);
            return dt.ToList<PM_Targets>();
        }
       

          public List<PM_Targets> FcHistoryList(string Filter, string value1 = null, string value2 = null, Int64 ProjectId = 0, Int64 MilestoneId = 0, Int64 StageId = 0)
        {
            DataTable dt = tdl.GetDataTableHistory(Filter, value1, value2, ProjectId, MilestoneId, StageId);
            return dt.ToList<PM_Targets>();
        }

        public bool Manage(string Filter, string value, DataTable dt)
        {
            try
            {
                return tdl.Manage(Filter, value, dt);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
