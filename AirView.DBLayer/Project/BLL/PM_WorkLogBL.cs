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
    public class PM_WorkLogBL
    {
        PM_WorkLogDL dal = new PM_WorkLogDL();
        public bool Manage(string Filter, PM_WorkLog worklog)
        {

            return dal.Manage(Filter, worklog.WLogId, worklog.ProjectId, worklog.ProjectSiteId, worklog.TaskId, worklog.IssueId, worklog.LogType, worklog.UserId, worklog.LogDate, worklog.LogHours, worklog.Description, worklog.IsApproved);
        }

        public List<PM_WorkLog> ToList(string Filter, string value1 = null, string value2 = null, string SelectOption = null)
        {
            DataTable dt = dal.GetDataTable(Filter, value1, value2, SelectOption);
            return dt.ToList<PM_WorkLog>();
        }

        public List<PM_WorkLog> ToList(string Filter,Int64 projectid = 0, string workgroups = "", string users = "",string logtype="", string startdate = "", string enddate = "",string userid="0",string taskid="0",string siteid="0")
        {
            DataTable dt = dal.GetDataTable(Filter, projectid, workgroups, users,logtype, startdate, enddate,userid,taskid,siteid);
            return dt.ToList<PM_WorkLog>();
        }

        public List<PM_WorkLog> Get(string Filter, Int64 TaskId, Int64 ProjectSiteId, string LogType, Int64 ProjectId)
        {
            DataTable dt = dal.GetDataTable(Filter,TaskId, ProjectSiteId, LogType, ProjectId);
            return dt.ToList<PM_WorkLog>();
        }

        public DataSet LookupData(string Filter, Int64 ProjectId,Int64 userid)
        {
            DataSet dt = dal.GetDataset(Filter,ProjectId,userid);
            return dt;
        }

    }
}
