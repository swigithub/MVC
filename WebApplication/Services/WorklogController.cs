using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using SWI.AirView.Models;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication.Services
{
    public class WorklogController : ApiController
    {
        [System.Web.Http.HttpGet, System.Web.Http.Route("swi/pm/worklog/_lookupdata")]
        public Response LookupData(int projectid = 0,Int64 userid=0)
        {
            Response response = new Response();
            try
            {

                PM_WorkLogBL WorkLogBL = new PM_WorkLogBL();
                List<PM_Projects> project = new List<PM_Projects>();
                var ds = WorkLogBL.LookupData("Get_LookupData", projectid,userid);
                if (ds.Tables.Count > 0)
                {
                    DataTable Project = ds.Tables[0];
                    if (Project != null && Project.Rows.Count > 0)
                    {
                        project = Project.ToList<PM_Projects>().ToList();
                        DataTable Workgroups = ds.Tables[1];
                        List<PM_Workgroup> workgroups = new List<PM_Workgroup>();
                        if (Workgroups != null && Workgroups.Rows.Count > 0)
                        {
                            workgroups = Workgroups.ToList<PM_Workgroup>().ToList();
                            DataTable Resources = ds.Tables[2];
                            List<PM_TaskEntry> resources = new List<PM_TaskEntry>();
                            if (Resources != null && Resources.Rows.Count > 0)
                            {
                                resources = Resources.ToList<PM_TaskEntry>().ToList();
                                foreach (var item in workgroups)
                                {
                                    item.Resources = resources.Where(x => x.GroupId == item.WorkgroupId).ToList();
                                }
                            }
                            }
                        project[0].WorkGroups = workgroups;
                    }


                }
                response.Status = "true";
                response.Message = "OK";
                response.Value = project;
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "false";
                response.Message = ex.ToString();
                return response;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.Route("swi/pm/worklog/_getworklogs")]
        public Response GetWorklogs(Int64 projectid = 0, string workgroups = "", string users = "", string logtype = "",string startdate="",string enddate="")
        {
            Response response = new Response();
            try
            {
                PM_WorkLogBL tb = new PM_WorkLogBL();
                var wlogs = new List<PM_WorkLog>();
                 if(string.IsNullOrEmpty(logtype))
                    wlogs = tb.ToList("getworklogs_summary_table", projectid, workgroups, users, logtype, startdate, enddate);
                 else
                    wlogs = tb.ToList("getworklogs", projectid, workgroups, users, logtype, startdate, enddate);
                response.Status = "true";
                response.Message = "OK";
                response.Value = wlogs;

                return response;
            }
            catch (Exception ex)
            {
                response.Status = "false";
                response.Message = ex.Message.ToString();
                return response;
            }
        }
      
        [System.Web.Http.HttpPost, System.Web.Http.Route("swi/pm/worklog/_approveworklogs")]
        public Response approveworklogs(IList<PM_WorkLog> worklogs)
        {
               Response response = new Response();
            try
            {
                dbDataTable dbtd = new dbDataTable();
                DataTable dt = dbtd.List();
                foreach (var logs in worklogs)
                {
                    myDataTable.AddRow(dt, "Value1", logs.WLogId, "Value2", logs.IsAttended, "Value3", logs.LogDate, "Value4", logs.UserId, "Value5", logs.IsApproved,"Value6",logs.Comment);
                }
                PM_WorkLogDL dal = new PM_WorkLogDL();
                response.Value = dal.Manage("approveworklogs", dt);
                response.Message = "OK";
                response.Status = Convert.ToString(response.Value);
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false.ToString();
                response.Message = ex.Message.ToString();
                return response;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.Route("swi/pm/worklog/_getfilterdropdowns")]
        public Response GetFilterDropdown(string filter="",Int64 projectid = 0, string workgroups = "", string users = "", string logtype = "", string startdate = "", string enddate = "",string userid="0",string sitetaskid="0",string projectsiteid="0",string workgroupid="0")
        {
            Response response = new Response();
            try
            {
                PM_WorkLogBL tb = new PM_WorkLogBL();
                PM_WorkLogDL dal = new PM_WorkLogDL();
                DataTable dt = dal.GetDataTable(filter, projectid, workgroups, users, logtype, startdate, enddate,userid,sitetaskid,projectsiteid,workgroupid);
               // var wlogs = tb.ToList(filter, projectid, workgroups, users, logtype, startdate, enddate);
              
                response.Status = "true";
                response.Message = "OK";
                response.Value = dt;

                return response;
            }
            catch (Exception ex)
            {
                response.Status = "false";
                response.Message = ex.Message.ToString();
                return response;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.Route("swi/pm/worklog/_advancesearch")]
        public Response advancesearch(Int64 projectid = 0, string workgroups = "", string users = "", string logtype = "", string startdate = "", string enddate = "",string userid="0",string siteid="0",string taskid="0")
        {
            Response response = new Response();
            try
            {
                PM_WorkLogBL tb = new PM_WorkLogBL();
                var wlogs = new List<PM_WorkLog>();
                if (string.IsNullOrEmpty(logtype))
                    wlogs = tb.ToList("advancesearch_summary_table", projectid, workgroups, users, logtype, startdate, enddate,userid,taskid,siteid);
                else
                    wlogs = tb.ToList("advancesearch", projectid, workgroups, users, logtype, startdate, enddate, userid, taskid, siteid);
                response.Status = "true";
                response.Message = "OK";
                response.Value = wlogs;

                return response;
            }
            catch (Exception ex)
            {
                response.Status = "false";
                response.Message = ex.Message.ToString();
                return response;
            }
        }



    }
}
