using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using Library.SWI.Project.BLL;
using Newtonsoft.Json;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApplication.Areas.Project.View_Models;

namespace WebApplication.Services
{
    public class ProjectAPIController : ApiController
    {
        [System.Web.Http.Route("swi/getprojects"), System.Web.Http.HttpPost]
        public HttpResponseMessage ToList(string _filter="",bool _active=true,Int64  _userId=0)
        {

            PM_ProjectBL pd = new PM_ProjectBL();
            var rec = pd.ToList(_filter, Convert.ToString(_active),_userId);

            if (rec != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, rec);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "No Data");
            }
        }

        [System.Web.Http.Route("swi/getprojectTracking"), System.Web.Http.HttpPost]
        public HttpResponseMessage GetMilstonesBarchart(Int64 projectId, string filter)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetMileStoneValues(filter, projectId);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record with Id " + projectId.ToString() + " not found");
            }
        }

        [System.Web.Http.Route("swi/getGeoTracking"), System.Web.Http.HttpPost]
        public HttpResponseMessage GetMarketRegionMap(string filter, Int64 ProjectId, Int64 MilestoneId = 0, string filterOption = null, string Value1 = "0")
        {
            PM_DashboardDL pd = new PM_DashboardDL();

            DataTable dt = pd.GetMileStoneValues(filter, ProjectId, 0, null, Value1);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record with Id " + ProjectId.ToString() + " not found");
            }

        }

        [System.Web.Http.Route("swi/getRegionIssues"), System.Web.Http.HttpPost]
        public HttpResponseMessage GetRegionIssue(Int64 ProjectId)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetMileStoneValues("Get_Region_Issue", ProjectId, 0);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record with Id " + ProjectId.ToString() + " not found");
            }

        }


        //++++++++++++++++++++++++++++++++++++++
        [System.Web.Http.Route("swi/geProjectTasksList"), System.Web.Http.HttpPost]
        public HttpResponseMessage GetTaskList(Int64 projectId, string filterOption)
        {
            List<MilestoneModel> Mlist = new List<MilestoneModel>();
            List<MilestoneModel> Tlist = new List<MilestoneModel>();
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetMileStoneValues("Get_Project_Tasks", projectId, 0, filterOption);
            foreach (DataRow item in dt.Rows)
            {
                MilestoneModel mil = new MilestoneModel();
                MilestoneModel tsk = new MilestoneModel();

                tsk.TaskId = Convert.ToInt64(item["TaskId"].ToString());
                tsk.PTaskId = Convert.ToInt64(item["PTaskId"].ToString());
                tsk.Task = item["Task"].ToString();
                tsk.Color = item["Color"].ToString();
                tsk.ProjectId = Convert.ToInt64(item["ProjectId"].ToString());
                tsk.ActualSites = Convert.ToInt32(item["ActualSites"].ToString());
                tsk.TotalSites = Convert.ToInt32(item["TotalSites"].ToString());
                mil.TaskId = Convert.ToInt64(item["TaskId"].ToString());
                mil.PTaskId = Convert.ToInt64(item["PTaskId"].ToString());
                mil.Task = item["Task"].ToString();
                mil.Color = item["Color"].ToString();
                mil.ProjectId = Convert.ToInt64(item["ProjectId"].ToString());
                mil.ActualSites = Convert.ToInt32(item["ActualSites"].ToString());
                mil.TotalSites = Convert.ToInt32(item["TotalSites"].ToString());
                if (tsk.PTaskId == 0)
                {
                    Mlist.Add(mil);
                }
                else
                {
                    Tlist.Add(tsk);
                }

            }

            foreach (var mile in Mlist)
            {
                foreach (var task in Tlist)
                {
                    if (task.PTaskId == mile.TaskId)
                    {
                        mile.Tasks = Tlist;
                        break;
                    }

                }
            }

            if (Mlist != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mlist);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record with Id " + projectId.ToString() + " not found");
            }
        }


        [System.Web.Http.Route("swi/getWOStages"), System.Web.Http.HttpPost]
        public HttpResponseMessage GetStages(Int64 ProjectId, Int64 MilestoneId)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetStages("Get_WO_Stages", ProjectId, MilestoneId);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record with Id " + ProjectId.ToString() + " not found");
            }
        }


        [System.Web.Http.Route("swi/getProjectTasks"), System.Web.Http.HttpPost]
        public HttpResponseMessage GetTasks(Int64 ProjectId, string Filter, string Value1)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetMileStoneValues(Filter, ProjectId, 0, null, Value1);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record with Id " + ProjectId.ToString() + " not found");
            }
        }

        //[System.Web.Http.Route("swi/PM_Dashboard_Sites"), System.Web.Http.HttpGet]
        //public void PM_Dashboard_Sites(string Filter, Int64 ProjectId = 0, string Value1 = null)
        //{
        //    PM_DashboardDL pd = new PM_DashboardDL();
        //    List<PM_SiteTaskParents> PM_SiteTaskParents = new List<PM_SiteTaskParents>();

        //    DataTable dt = pd.GetDashboardCharts(Filter, ProjectId, 0, Value1, null, null, null, null, null, null, 0, 0, null, null);
        //    var parent = dt.ToList<PM_SiteTaskParents>();
        //    foreach(var item in parent)
        //    {
        //        DataTable child = pd.GetDashboardCharts(Filter, ProjectId, 0, Value1, null, null, null, null, null, null, 0, 0, null, null);
        //        var childLayer = child.ToList<PM_SiteTaskParents>();
        //        if (childLayer.Any())
        //        {

        //        }
        //    }
        //}


        //++++++++++++++++++++++++++++++++++++

        [System.Web.Http.Route("swi/PM_Dashboard"), System.Web.Http.HttpPost]
        public HttpResponseMessage PM_Dashboard(string Filter, Int64 ProjectId = 0, Int64 MilestoneId = 0, string Value1 = null, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string SearchFilter = null, string FilterOption = null, int Page = 0, int Offset = 0, string MapStatus = null, string MapType = null)
        {
            PM_DashboardDL pd = new PM_DashboardDL();

            //int Offset = (Page - 1) * 5;
            //Filter, ProjectId, MilestoneId, Value1, TaskIds, LocationIds, FromDate, ToDate, SearchFilter, FilterOption, 0, 0, MapStatus, MapType
            DataTable dt = pd.GetDashboardCharts(Filter, ProjectId, MilestoneId, Value1, TaskIds, LocationIds, FromDate, ToDate, SearchFilter, FilterOption, Page, Offset, MapStatus, MapType);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record with Id " + ProjectId.ToString() + " not found");
            }

        }

        [System.Web.Http.Route("swi/PM_SummayCharts"), System.Web.Http.HttpPost]
        public projectSummaryDTO GetSummaryCharts(string Filter, Int64 ProjectId = 0, Int64 MilestoneId = 0, string Value1 = "0", string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string SearchFilter = null, string FilterOption = null, string Request = "")
        {
            projectSummaryDTO obj = new projectSummaryDTO();

            PM_DashboardDL pd = new PM_DashboardDL();
            DataSet ds = pd.GetDashboardWO(Filter, ProjectId, 5, 0, "", TaskIds, LocationIds, FromDate, ToDate, null);

            DataTable dtSiteStatus = ds.Tables[0];
            DataTable dtIssueDistribution = ds.Tables[1];
            DataTable dtIssueAccountibility = ds.Tables[2];

            obj.SiteStatus = dtSiteStatus.ToList<SiteStatusDTO>();
            obj.IssueDistribution = dtIssueDistribution.ToList<IssueDistributionDTO>();
            obj.IssueAccountibility = dtIssueAccountibility.ToList<IssueAccountibilityyDTO>();

            //http://localhost:18460/swi/PM_SummayCharts?&filter=PROGRAM_SUMMARY&ProjectId=20021&MilestoneId=0&Value1=0&TaskIds=50076&LocationIds=163408,163409,163410,163411,163412,163413,163414,163415,163416,163405,163406,163407163408,163409,163410,163411,163412,163413,163414,163415,163416,163405,163406,163407&FromDate=12/31/2017&ToDate=1/31/2018

            return obj;
        }





        [System.Web.Http.Route("swi/PM_Tasks"), System.Web.Http.HttpPost]  //Filters:ByTaskTypeId,ByTaskTypeKeyCode,ByPTaskId,ByTaskId,ByProjectId
        public HttpResponseMessage PM_Tasks(string filter, string pType, string pId, bool Resources = false, Int64 ProjectId = 0, Int64 TaskId = 0)
        {
            PM_TaskBL tb = new PM_TaskBL();
            var result = tb.ToList(filter, pType, pId, ProjectId, TaskId);
            if (Resources)
            {
                PM_ProjectResourceBL prb = new PM_ProjectResourceBL();
                foreach (var item in result)
                {
                    item.ProjectResources = prb.ToList("byTaskId", item.TaskId.ToString(), ProjectId, TaskId);
                }
            }
            //return Json(rec, JsonRequestBehavior.AllowGet);

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record for Id " + TaskId.ToString() + " not found");
            }
        }

        [System.Web.Http.Route("swi/Locations"), System.Web.Http.HttpPost]
        public HttpResponseMessage UserLocations(string filter, string UserId)
        {
            AD_DefinationBL db = new AD_DefinationBL();
            var result = db.ToList(filter, UserId);

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "No Location(s) found");
            }
        }


        [System.Web.Http.Route("swi/getTodo"), System.Web.Http.HttpPost]
        public HttpResponseMessage GetTodo(Int64 ProjectId, Int64 UserId)
        {
            PM_TodoBL bl = new PM_TodoBL();
            var result = bl.GetTodo("Get_Todo", ProjectId, UserId);
            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "To do List is empty");
            }
        }

        [System.Web.Http.Route("swi/SaveTodo"), System.Web.Http.HttpPost]
        public HttpResponseMessage SaveTodo(PM_Todo todo)
        {
            PM_TaskBL pb = new PM_TaskBL();

            if (todo.TodoId > 0)
            {
                bool result = pb.SaveTodo(todo, "Update_Todo");
                if (result)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK,
                           new { Message = "To do List Updated", Value = 1 });
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "To do List Not Updated");
                }
            }
            else
            {
                bool result = pb.SaveTodo(todo, "Insert_Todo");
                if (result)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK,
                           new { Message = "To do List Added", Value = 1 });
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "To do List Not Added");
                }
            }


        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("swi/PlanNewTask")]
        [System.Web.Http.Route("swi/ChangePlanStatus")]
        public HttpResponseMessage PlanningNew(PM_SiteTasks siteTask)
        {
            PM_TaskBL pb = new PM_TaskBL();
            var result = pb.InsertPM_siteTask("Update_Task_Plan", siteTask);
            if (result != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK,
                       new { Message = "Site Task Planed", Value = 1 });
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Site Task Not Planed");
            }
        }

        [System.Web.Http.Route("swi/PM_Worklog")]
        public HttpResponseMessage New(PM_WorkLog worklog)
        {
            PM_WorkLogBL bal = new PM_WorkLogBL();
            var result = bal.Manage("INSERT_WorkLog", worklog);
            if (result)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK,
                       new { Message = "WorkLog added", Value = 1 });
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "WorkLog not added");
            }
        }

        [System.Web.Http.Route("swi/PM_ManageIssueLog"), System.Web.Http.HttpPost]
        public HttpResponseMessage ChangeIssueStatus(PM_IssuesLog IssueLog)
        {
            //IssueLog.UserId = ViewBag.UserId;
            PM_IssueBL bal = new PM_IssueBL();
            if (IssueLog.IssueLogId != 0)
            {
                var res = bal.ManageIssueLog("Insert_IssueLog_Status", IssueLog);
                if (res)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK,
                           new { Message = "Issue status Loged", Value = 1 });
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Issue status not Loged");
                }
            }
            else
            {
                var res = bal.ManageIssueLog("Insert_IssueLog_Status", IssueLog);
                if (res)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK,
                           new { Message = "Issue status Loged", Value = 1 });
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Issue status not Loged");
                }
            }
        }

        [System.Web.Http.Route("swi/GetIssueLog"), System.Web.Http.HttpPost]
        public HttpResponseMessage GetIssueLog(string Filter, Int64 IssueId) //"GET_IssueLog"
        {
            PM_IssueBL bal = new PM_IssueBL();
            var result = bal.GetIssueLog(Filter, IssueId);
            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record for Id " + IssueId.ToString() + " not found");
            }
        }

        [System.Web.Http.Route("swi/GetIssue"), System.Web.Http.HttpPost]
        public HttpResponseMessage GetIssue_ById(string Filter, Int64 ProjectId = 0, Int64 IssueId = 0)
        {
            PM_IssueBL bal = new PM_IssueBL();
            var result = bal.ToList(Filter, ProjectId, IssueId);  //GET_Issue

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record for Id " + IssueId.ToString() + " not found");
            }
        }

        [System.Web.Http.Route("swi/AddProjectSite"), System.Web.Http.HttpPost]
        public HttpResponseMessage AddProjectSite(List<ProjectSites> wo)
        {
            PM_ProjectSitesBL ub = new PM_ProjectSitesBL();
            List<Response> resp = new List<Response>();
            try
            {


                if (wo.Count > 0)
                {
                    var sitelst = ub.ToList("Get_All_By_ProjectId", wo[0].ProjectId.ToString());
                    wo = wo.Where(c => sitelst.All(w => w.FACode != c.FACode)).ToList();

                    if (wo.Count > 0)
                    {
                        bool result = ub.Insert("Insert", wo, 0, 0, null);

                        if (result)
                        {
                            resp.Add(new Response() { Message = "Project Site Data Saved", Status = "OK", Value = "1" });
                            return this.Request.CreateResponse(HttpStatusCode.OK, resp);
                        }
                        else
                        {
                            resp.Add(new Response() { Message = "Project Site Data Not Saved", Status = "Error", Value = "0" });
                            return this.Request.CreateResponse(HttpStatusCode.OK, resp);
                        }
                    }
                    else
                    {
                        resp.Add(new Response() { Message = "SiteName should be unique in a Project", Status = "Error", Value = "0" });
                        return this.Request.CreateResponse(HttpStatusCode.OK, resp);
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Add(new Response() { Message = ex.Message.ToString(), Status = "danger", Value = "-1" });
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound,resp.ToString());
        }

        [System.Web.Http.Route("swi/UpdateProjectSite"), System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateProjectSite(ProjectSites wo)
        {
            PM_ProjectSitesBL ub = new PM_ProjectSitesBL();
            Response res = new Response();
            try
            {
                if (wo.ProjectSiteId > 0)
                {
                    bool result = ub.Update("Update", wo, wo.UserId);
                    if (result)
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK,
                               new { Message = "Project Site Updated", Value = 1 });
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Project Site Not Updated ");
                    }
                }
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Error to save data");
        }


        //[System.Web.Http.Route("swi/AddUpdateProjectSite"), System.Web.Http.HttpPost]
        //public HttpResponseMessage AddUpdateProjectSite(ProjectSites wo)
        //{
        //    PM_ProjectSitesBL ub = new PM_ProjectSitesBL();
        //    Response res = new Response();
        //    try
        //    {
        //        if (wo.ProjectSiteId > 0)
        //        {
        //            bool result = ub.Update("Update", wo, wo.UserId);
        //            if (result)
        //            {
        //                return this.Request.CreateResponse(HttpStatusCode.OK,
        //                       new { Message = "Project Site Updated", Value = 1 });
        //            }
        //            else
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
        //                    "Project Site Not Updated ");
        //            }
        //        }

        //        bool result2 = ub.Update("Insert", wo, wo.UserId);
        //        if (result2)
        //        {
        //            var arrayForTest = new List<Response>();
        //            Response simplebj = new Response();
        //            simplebj.Message = "test message";
        //            arrayForTest.Add(simplebj);
        //            return this.Request.CreateResponse(HttpStatusCode.OK,
        //                   new { arrayForTest = arrayForTest });
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
        //                "Project Site Data Not Saved ");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Status = "danger";
        //        res.Message = ex.Message;
        //    }
        //    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
        //                    "Error to save data");
        //}


       
    }
}