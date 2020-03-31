using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using AirView.Filters.MobileAuth;
using ibrary.SWI.Project.DAL;
using Library.SWI.Project.BLL;
using Library.SWI.Project.DAL;
using Library.SWI.Project.Model;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class TaskController : Controller
    {
        [IsLogin(CheckPermission = true)]
        public ActionResult Index(Int64 ProjectId = 0)
        {
            if (ProjectId > 0)
            {
                var oob = Permission.AllowProject(Convert.ToInt64(ProjectId));
                if (oob == null)
                {
                    TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                    return RedirectToAction("index", "error", new { Area = "Project" });
                }
                else
                {

                    TempData["ProjectEntity"] = oob; TempData.Keep("ProjectEntity");
                }

                ViewBag.ProjectId = ProjectId;
            }
            else
            {
                ViewBag.ProjectId = 20021;
            }

            ViewBag.UserId = ViewBag.UserId;
            return View();
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult New()
        {
            return PartialView("~/Areas/Project/Views/Task/_New.cshtml");
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult PlanningNew()
        {
            return PartialView("~/Areas/Project/Views/Dashboard/_PlanningView.cshtml");
        }
        //[HttpPost, IsLogin(CheckPermission = false)]
        //public ActionResult PlanningNew(PM_SiteTasks siteTask)
        //{
        //    PM_TaskBL pb = new PM_TaskBL();
        //    siteTask.CreatedBy = ViewBag.UserId;
        //    var result = pb.InsertPM_siteTask("Update_SiteTask_PLAN", siteTask);

        //    return PartialView("~/Areas/Project/Views/Dashboard/_PlanningView.cshtml");
        //}

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult PlanningNew(List<PM_SiteTasks> SiteTasks)
        {
            PM_TaskBL pb = new PM_TaskBL();
            var result = pb.InsertPM_siteTask("Update_SiteTask_PLAN_BULK", SiteTasks, ViewBag.UserId);

            return PartialView("~/Areas/Project/Views/Dashboard/_PlanningView.cshtml");
        }
        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult StatusChange(PM_SiteTasks siteTask)
        {
            Response res = new Response();
            siteTask.CreatedBy = ViewBag.UserId;
            try
            {
                PM_TaskBL pb = new PM_TaskBL();
                res.Value = pb.InsertPM_siteTask("Update_SiteTask_PLAN", siteTask);
                res.Status = "success";
                res.Message = "Save successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult TaskEntry(List<PM_TaskEntry> task)
        {
            Response res = new Response();
            try
            {

                dbDataTable dbdt = new dbDataTable();
                var dt1 = dbdt.TaskList();
                PM_TaskEntryDL te = new PM_TaskEntryDL();
                foreach (var item in task)
                {

                    myDataTable.AddRow(dt1, "Value1", item.EntryId, "Value2", item.ProjectId,
                          "Value3", item.ProjectSiteId, "Value4", item.TaskId, "Value5", item.FormId,
                          "Value6", item.FormValue, "Value7", ViewBag.UserId, "Value8", DateTime.Now, "Value15", item.Comments
                           );

                }
                res.Value = te.Manage("Insert", dt1);
                res.Status = "success";
                res.Message = "Save successfully";

            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, IsLogin(Return = "NoCheck", CheckPermission = false)]
        [MobileAuth]
        public ActionResult TaskEntryMobile(string data, string Key)
        {
            Response res = new Response();
            try
            {
                List<PM_TaskEntry> task = new JavaScriptSerializer().Deserialize<List<PM_TaskEntry>>(data);
                dbDataTable dbdt = new dbDataTable();
                var dt1 = dbdt.TaskList();
                PM_TaskEntryDL te = new PM_TaskEntryDL();
                foreach (var item in task)
                {

                    myDataTable.AddRow(dt1, "Value1", item.EntryId, "Value2", item.ProjectId,
                          "Value3", item.ProjectSiteId, "Value4", item.TaskId, "Value5", item.FormId,
                          "Value6", item.FormValue, "Value7", item.UserID, "Value8", DateTime.Now
                           );

                }
                te.Manage("Insert", dt1);
                res.Status = "success";
                res.Message = "Save successfully";

            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res);
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult New(List<PM_Task> t)
        {
            Response res = new Response();
            try
            {
                if (t.Count > 0)
                {
                    AD_DefinationBL db = new AD_DefinationBL();
                    List<AD_Defination> ProjectStatus = db.ToList("byDefinationType", "Project Task Status");
                    PM_TaskBL pb = new PM_TaskBL();

                    dbDataTable dbdt = new dbDataTable();
                    PM_TaskDL ptd = new PM_TaskDL();
                    var dt1 = dbdt.TaskList();
                    int Counter = 1;
                    foreach (var item in t)
                    {
                        var Status = item.Status.Split('_');
                        if (Status.Length > 0)
                        {
                            switch (Status[1].ToLower())
                            {
                                case "active":
                                    var active = ProjectStatus.Where(x => x.DefinationName.ToLower() == "active").FirstOrDefault();
                                    if (active != null)
                                    {
                                        item.StatusId = active.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                case "suspended":
                                    var suspend = ProjectStatus.Where(x => x.DefinationName.ToLower() == "suspended").FirstOrDefault();
                                    if (suspend != null)
                                    {
                                        item.StatusId = suspend.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                case "done":
                                    var done = ProjectStatus.Where(x => x.DefinationName.ToLower() == "completed").FirstOrDefault();
                                    if (done != null)
                                    {
                                        item.StatusId = done.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                case "failed":
                                    var failed = ProjectStatus.Where(x => x.DefinationName.ToLower() == "cancelled").FirstOrDefault();
                                    if (failed != null)
                                    {
                                        item.StatusId = failed.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                case "undefined":
                                    var undefined = ProjectStatus.Where(x => x.DefinationName.ToLower() == "undefined").FirstOrDefault();
                                    if (undefined != null)
                                    {
                                        item.StatusId = undefined.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                case "planned":
                                    var planned = ProjectStatus.Where(x => x.DefinationName.ToLower() == "planned").FirstOrDefault();
                                    if (planned != null)
                                    {
                                        item.StatusId = planned.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                    //default :
                                    //   item.StatusId = 0;
                            }

                            if (item.StatusId == 0)
                            {
                                item.StatusId = ProjectStatus.Where(x => x.DefinationName.ToLower() == "planned").FirstOrDefault().PDefinationId;
                                item.Status = "STATUS_PLANNED";
                            }

                        }
                        var Parentid = item.PTaskId;
                        var Preid = item.PredecessorId;
                        if (string.IsNullOrEmpty(item.PTaskId) || item.PTaskId.StartsWith("tmp_"))
                        {

                            Parentid = "0";
                        }
                        else if (string.IsNullOrEmpty(item.PredecessorId) || item.PredecessorId.StartsWith("tmp_"))
                        {
                            Preid = "0";
                        }
                        myDataTable.AddRow(dt1, "Value1", item.TaskId, "Value2", Parentid,
                            "Value3", item.ProjectId, "Value4", item.TaskTypeId, "Value5", item.StatusId,
                            "Value6", item.PriorityId, "Value7", Preid, "Value8", item.Title
                            , "Value9", item.PlannedDate, "Value10", item.TargetDate, "Value11", item.ActualStartDate, "Value12", item.ActualEndDate,
                             "Value13", item.EstimatedStartDate, "Value14", item.EstimatedEndDate, "Value15", item.Description,
                            "Value16", item.IsEstimate, "Value17", item.ForecastedSites, "Value18", item.CompletionPercent, "Value19", item.BudgetCost,
                            "Value20", item.ActualCost, "Value21", item.MapCode, "Value22", item.MapColumn, "Value23", item.Color,
                            "Value24", item.IsActive, "Value25", item.ScopeId, "Value26", item.IsStartMilestone, "Value27", item.IsEndMilestone, "Value28", Counter
                            , "Value29", item.Level, "Value30", item.Duration, "Value31", "Web"
                            );
                        Counter++;
                    }

                    DataTable NewTasks = ptd.Manage("Insert2", dt1);
                    List<PM_Task> ParentUpdate = new List<PM_Task>();

                    List<int> NewTasksId = (from row in NewTasks.AsEnumerable() select Convert.ToInt32(row["ID"])).ToList();
                    for (int i = 0; i < t.Count; i++)
                    {
                        t[i].TaskId = NewTasksId[i];
                    }
                    for (int i = 0; i < t.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(t[i].PTaskId) && t[i].PTaskId.StartsWith("tmp_"))
                        {

                            t[i].PTaskId = t.Where(x => x.id == t[i].PTaskId).FirstOrDefault().TaskId.ToString();
                            ParentUpdate.Add(t[i]);

                        }
                        else if (!string.IsNullOrEmpty(t[i].PredecessorId) && t[i].PredecessorId.StartsWith("tmp_"))
                        {
                            t[i].PredecessorId = t.Where(x => x.id == t[i].PredecessorId).FirstOrDefault().TaskId.ToString();
                            ParentUpdate.Add(t[i]);
                        }

                    }

                    if (ParentUpdate.Count > 0)
                    {
                        var dt3 = dbdt.TaskList();
                        foreach (var item in ParentUpdate)
                        {
                            myDataTable.AddRow(dt3, "Value1", item.TaskId, "Value2", item.PTaskId,
                                "Value3", item.ProjectId, "Value4", item.TaskTypeId, "Value5", item.StatusId,
                                "Value6", item.PriorityId, "Value7", item.PredecessorId, "Value8", item.Title
                                , "Value9", item.PlannedDate, "Value10", item.TargetDate, "Value11", item.ActualStartDate, "Value12", item.ActualEndDate,
                                 "Value13", item.EstimatedStartDate, "Value14", item.EstimatedEndDate, "Value15", item.Description,
                                "Value16", item.IsEstimate, "Value17", item.ForecastedSites, "Value18", item.CompletionPercent, "Value19", item.BudgetCost,
                                "Value20", item.ActualCost, "Value21", item.MapCode, "Value22", item.MapColumn, "Value23", item.Color,
                                "Value24", item.IsActive, "Value25", item.ScopeId, "Value26", item.IsStartMilestone, "Value27", item.IsEndMilestone, "Value28", item.SortOrder
                                 , "Value29", item.Level
                                );
                        }
                        DataTable NewTasks2 = ptd.Manage("UpdateParent", dt3);
                        List<int> NewTasksIds = (from row in NewTasks2.AsEnumerable() select Convert.ToInt32(row["ID"])).ToList();

                    }
                    //PM_ProjectResourceDL prd = new PM_ProjectResourceDL();
                    //var dt = dbdt.List();
                    //for (int i = 0; i < t.Count; i++)
                    /*{
                        foreach (var item2 in t[i].ProjectResources)
                        {

                            if (item2.TaskId == 0)
                            {
                                //myDataTable.AddRow(dt, "Value1", t[i].ProjectId, "Value2", NewTasksId[i], "Value3", ViewBag.UserId, "Value4", item2.AssignToId);

                            }
                            else
                            {
                                //myDataTable.AddRow(dt, "Value1", t[i].ProjectId, "Value2", t[i].TaskId, "Value3", ViewBag.UserId, "Value4", item2.AssignToId);

                            }
                        }
                    }*/
                    //.Manage("Insert2", dt);
                }

                res.Status = "success";
                res.Message = "Save successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, IsLogin(CheckPermission = false)]
        public ActionResult PlanProject(string Filter, Int64 ProjectId = 0, Int64 RevisionId = 0, DateTime? FromDate = null, DateTime? ToDate = null, string LocationIds = null, string TaskIds = null, string SiteStatus = null, Int64 UserId = 0)
        {

            PM_ProjectBL pal = new PM_ProjectBL();
            AD_DefinationBL dbl = new AD_DefinationBL();
            Sec_UserBL ud = new Sec_UserBL();
            List<PM_ProjectSite> result = pal.PM_PlanProject(Filter, ProjectId, RevisionId, FromDate, ToDate, LocationIds, TaskIds, SiteStatus, UserId);
            List<SelectedList> listStatus = dbl.SelectedList("byDefinationType", "Project Status");
            ViewBag.StatusId = listStatus;

            List<SelectedList> listUsers = ud.SelectedList("ByProjectId", ProjectId.ToString());
            List<PM_CompanyHierarchy> lstCompUsers = ListCompanyUsers("ByProjectId", ProjectId.ToString());

            List<Client> listOfClient = lstCompUsers.GroupBy(xx => xx.ClientId).Select(x => new Client()
            {
                ClientId = x.Key,
                ClientName = lstCompUsers.Where(XVC => XVC.ClientId == x.Key).FirstOrDefault().ClientName,
                userRolesList = lstCompUsers.Where(xr => xr.ClientId == x.Key).GroupBy(zx => zx.RoleId).Select(urol => new UserRoles()
                {
                    RoleId = urol.Key,
                    RoleName = lstCompUsers.FirstOrDefault(zzzz => zzzz.RoleId == urol.Key).RoleName,
                    userList = lstCompUsers.Where(zzzz1 => zzzz1.RoleId == urol.Key && zzzz1.ClientId == x.Key).Select(usr => new Users()
                    {
                        FirstName = usr.FirstName,
                        LastName = usr.LastName,
                        UserId = usr.UserId,
                        UserName = usr.UserName

                    }).ToList()
                }).ToList()
            }).ToList();
            foreach (var item in result)
            {
                item.SelectedListStatus = listStatus;
                item.Clients = listOfClient;
                //item.ActualEndDate = item.ActualStartDate!=null? Convert.ToDateTime(item.ActualStartDate).ToShortDateString():null;
            }
            return PartialView("~/Areas/Project/Views/Task/_PlanProject.cshtml", result);
        }
        [HttpPost, IsLogin(CheckPermission = false)]
        public List<PM_CompanyHierarchy> ListCompanyUsers(string filter, string value)
        {
            Sec_UserBL db = new Sec_UserBL();
            Client c = new Client();
            var listUsersNew = db.ToListNew(filter, value);
            return listUsersNew;
        }




        [IsLogin(CheckPermission = false)]
        public JsonResult ListUsers(string filter, string value)
        {
            Sec_UserBL db = new Sec_UserBL();
            var listUsersNew = db.ToListNew(filter, value);
            return Json(listUsersNew, JsonRequestBehavior.AllowGet);
        }




        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult SaveProjectPlan(Int64 ProjectId, List<PM_ProjectSiteDto> listOfProjectPlan)
        {

            dbDataTable ddt = new dbDataTable();
            //PM_ProjectBL pal = new PM_ProjectBL();
            PM_ProjectDL pdl = new PM_ProjectDL();
            try
            {
                DataTable dt = ddt.List();
                foreach (var item in listOfProjectPlan)
                {
                    string userIds = string.Empty;

                    if (item.SelectedListUsers != null)
                    {
                        userIds = string.Join(",", item.SelectedListUsers);
                    }
                    else
                    {
                        userIds = "0";
                    }

                    myDataTable.AddRow(dt, "Value1", item.ProjectSiteId, "Value2", item.ProjectId, "Value3", item.PlannedDate, "Value4", item.EstimatedStartDate,
                        "Value5", item.TargetDate, "Value6", item.ActualEndDate, "Value7", item.StatusId, "Value8", userIds, "Value9", item.TaskId, "Value10", item.EstimatedEndDate, "Value11", item.ActualStartDate);
                }
                var _Flag = pdl.ManageProjectPlan("Bulk_Project_Plan", ProjectId, dt);

                if (_Flag)
                {
                    // TempData["msg_success"] = "Save successfully.";
                    return Json(new { msg = "Project plan created successfully!", key = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msg = "Error Occured!", key = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, key = false }, JsonRequestBehavior.AllowGet);
            }
            //return View("~/Areas/Project/Views/Task/Index.cshtml");
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult setTaskSortOrder(Int64 TaskId, string SortDirection)
        {
            Response res = new Response();
            try
            {
                PM_TaskDL pb = new PM_TaskDL();
                if (TaskId > 0)
                {
                    res.Value = pb.ManageTaskSortOrder("UpdateSortOrder", TaskId, SortDirection);
                }

                res.Status = "success";
                res.Message = "Save successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, IsLogin(CheckPermission = false)]

        public JsonResult getMaxSortOrder(string filter, string value = "0", string value2 = "0")
        {
            PM_TaskBL tb = new PM_TaskBL();
            var rec = tb.ToList(filter, value, value2);

            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult getAllTask(string filter, Int64 ProjectId = 0)
        {
            List<MilestoneModel> Mlist = new List<MilestoneModel>();
            List<MilestoneModel> Tlist = new List<MilestoneModel>();

            PM_TaskBL tb = new PM_TaskBL();
            var dt = tb.ToList(filter, string.Empty, string.Empty, ProjectId, 0);

            if (filter == "Get_Project_Tasks")
            {
                List<MilestoneModel> MyList = new List<MilestoneModel>();
                foreach (var item in dt)
                {
                    MilestoneModel mil = new MilestoneModel();
                    mil.PTaskId = Convert.ToInt64(item.PTaskId);
                    mil.Task = item.Title;
                    mil.TaskId = item.TaskId;
                    MyList.Add(mil);
                }
                foreach (var item in MyList.Where(x => x.PTaskId == 0))
                {
                    MilestoneModel mil = new MilestoneModel();
                    mil.Tasks = FlatToHierarchy(MyList, item.TaskId);
                    mil.TaskId = item.TaskId;
                    mil.Task = item.Task;
                    Mlist.Add(mil);
                }
                return Json(Mlist, JsonRequestBehavior.AllowGet);

            }


            if (dt.Count() > 0)
            {
                foreach (PM_Task obj in dt)
                {
                    if (obj.PTaskId == "0")
                    {
                        MilestoneModel mil = new MilestoneModel();

                        mil.TaskId = Convert.ToInt64(obj.TaskId.ToString());
                        mil.PTaskId = Convert.ToInt64(obj.PTaskId.ToString());
                        mil.Task = obj.Title.ToString();
                        Mlist.Add(mil);
                    }
                    else
                    {
                        MilestoneModel tsk = new MilestoneModel();

                        tsk.TaskId = Convert.ToInt64(obj.TaskId.ToString());
                        tsk.PTaskId = Convert.ToInt64(obj.PTaskId.ToString());
                        tsk.Task = obj.Title.ToString();

                        Tlist.Add(tsk);
                    }
                }

                foreach (var mile in Mlist)
                {
                    foreach (var task in Tlist)
                    {
                        if (task.PTaskId == mile.TaskId)
                        {
                            mile.Tasks.Add(task);
                        }
                    }
                }

            }
            return Json(Mlist, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        private List<MilestoneModel> FlatToHierarchy(IEnumerable<MilestoneModel> list, long parentId = 0)
        {

            return (from i in list
                    where i.PTaskId == parentId
                    select new MilestoneModel
                    {
                        TaskId = i.TaskId,
                        Task = i.Task,
                        PTaskId = i.PTaskId,
                        Tasks = FlatToHierarchy(list, Convert.ToInt64(i.TaskId))
                    }).ToList();
        }
        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult ToList(string filter, string value, string value2, bool Resources = false, Int64 ProjectId = 0, Int64 TaskId = 0)
        {
            PM_TaskBL tb = new PM_TaskBL();
            var rec = tb.ToList(filter, value, value2, ProjectId, TaskId);
            if (Resources)
            {
                PM_ProjectResourceBL prb = new PM_ProjectResourceBL();

                foreach (var item in rec)
                {
                    item.ProjectResources = prb.ToList("byTaskId", item.TaskId.ToString(), ProjectId, TaskId);
                }
            }
            if (filter == "ByTaskTypeKeyCode")
            {
                AD_DefinationBL db = new AD_DefinationBL();
                List<AD_Defination> ProjectStatus = db.ToList("byDefinationType", "Project Task Status");
                foreach (var item in rec)
                {

                    var Splited = ProjectStatus.Where(x => x.DefinationId == item.StatusId).FirstOrDefault();
                    if (Splited != null)
                    {
                        var CurrentStatus = Splited.DefinationName.ToLower();
                        switch (CurrentStatus)
                        {
                            case "active":
                                item.Status = "STATUS_ACTIVE";
                                break;
                            case "suspended":
                                item.Status = "STATUS_SUSPENDED";
                                break;
                            case "completed":
                                item.Status = "STATUS_DONE";
                                break;
                            case "cancelled":
                                item.Status = "STATUS_FAILED";
                                break;
                            case "undefined":
                                item.Status = "STATUS_UNDEFINED";
                                break;
                            case "planned":
                                item.Status = "STATUS_PLANNED";
                                break;
                                //default :
                                //   item.StatusId = 0;
                        }

                        //   item.Status = GanttStatusses.Where(x=>x.Split('_')[1].ToLower() == Splited.DefinationName.ToLower()).FirstOrDefault();
                    }
                    if (item.Status == null || item.Status == "")
                    {
                        item.Status = "STATUS_PLANNED";
                        item.StatusId = ProjectStatus.Where(x => x.DefinationName == "Planned").FirstOrDefault().PDefinationId;
                    }
                }
            }

            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult ToSingle(string filter, string value = "0", string value2 = "0")
        {
            PM_TaskBL tb = new PM_TaskBL();
            var rec = tb.ToList(filter, value, value2).FirstOrDefault();
            if (rec != null)
            {
                PM_ProjectResourceBL prb = new PM_ProjectResourceBL();
                rec.ProjectResources = prb.ToList("byTaskId", rec.TaskId.ToString());
            }
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult SaveTodo(PM_Todo todo)
        {
            PM_TaskBL pb = new PM_TaskBL();
            todo.CreatedById = ViewBag.UserId;
            todo.CreatedOn = DateTime.Now;
            if (todo.TodoId == 0)
            {
                var result = pb.SaveTodo(todo, "Insert_Todo");
                return Json(result == true ? "success" : "fail", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = pb.SaveTodo(todo, "Update_Todo");
                return Json(result == true ? "success" : "fail", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult PostEditToDo(PM_Todo todo)
        {
            PM_TaskBL pb = new PM_TaskBL();
            var result = pb.EditTodo(todo, "Edit_Todo");
            return Json(result == true ? "success" : "fail", JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GroupSitePlaning(Int64 ProjectId)
        {

            return PartialView("~/Areas/Project/Views/Task/_GroupSitePlan.cshtml");
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult SetResources(List<PM_TaskEntry> model)
        {
            Response res = new Response();
            try
            {
                PM_TaskEntryBL te = new PM_TaskEntryBL();
                for (int x = 0; x < model.Count; x++)
                {

                    if (model[x].PMTRId != 0)
                    {
                        res.Value = te.ManageResources("UpdateResources", model[x].ResourceId, model[x].GroupId, model[x].RACIId, model[x].ProjectId, model[x].TaskId, model[x].PMTRId, model[x].RatePerHour);
                    }
                    else
                    {
                        res.Value = te.ManageResources("InsertResources", model[x].ResourceId, model[x].GroupId, model[x].RACIId, model[x].ProjectId, model[x].TaskId, model[x].RatePerHour);
                    }

                    res.Status = "success";
                    res.Message = "Save successfully";
                }
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json("res", JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult GetResources(int ProjectId, int TaskId)
        {
            PM_TaskEntryBL te = new PM_TaskEntryBL();
            var jsonReturn = te.GET_Group_LIST("GetResources", ProjectId, TaskId).ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            //return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult PopulateGroupResources(int ProjectId)
        {

            PM_TaskEntryBL te = new PM_TaskEntryBL();


            var jsonReturn1 = te.GET_Group_LIST("PopulateGroupResources", ProjectId).ToList();


            return Json(jsonReturn1, JsonRequestBehavior.AllowGet);

        }
        [IsLogin(CheckPermission = false)]
        public JsonResult PopulateUserResources(int id, int TaskId)
        {

            PM_TaskEntryBL te = new PM_TaskEntryBL();


            var jsonReturn1 = te.GET_Group_LIST("PopulateUserResources", id, TaskId).ToList();


            return Json(jsonReturn1, JsonRequestBehavior.AllowGet);

        }
        [IsLogin(CheckPermission = false)]
        public JsonResult PopulateUserResourcesProject(int id)
        {

            PM_TaskEntryBL te = new PM_TaskEntryBL();


            var jsonReturn1 = te.GET_Group_LIST("PopulateUserResourcesProject", id).ToList();


            return Json(jsonReturn1, JsonRequestBehavior.AllowGet);

        }


        [IsLogin(CheckPermission = false)]
        public JsonResult DeleteResources(int id)
        {
            PM_TaskEntryBL te = new PM_TaskEntryBL();
            Response res = new Response();
            try
            {
                te.ManageResources("DeleteResources", id);
                res.Status = "success";
                res.Message = "Save successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
            //return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [IsLogin(Return = "NoCheck", CheckPermission = false)]
        [MobileAuth]
        public ActionResult DataEntryMobile(PM_TaskDataEntry TaskDataEntry, string Key)
        {
            ViewBag.MobileKey = Key;
            return View(TaskDataEntry);
        }

        // Site Task Inventory

        //Getting Data
        [IsLogin(CheckPermission = false)]
        public JsonResult GetUEtypes(string Filter, int TaskId)
        {
            PM_TaskEntryBL te = new PM_TaskEntryBL();
            var list = te.Get_UE_Type_List(Filter);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult GetUEItems(string Filter, long UETypeId)
        {
            PM_TaskEntryBL te = new PM_TaskEntryBL();
            var list = te.Get_UE_Items(Filter, UETypeId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult GetSiteTaskInventory(string Filter, long TaskId, long SiteId)
        {
            PM_TaskEntryBL te = new PM_TaskEntryBL();
            var list = te.Get_SiteTask_Inventory(Filter, TaskId, SiteId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult GetSiteTaskInventoryAttachments(string Filter, long SiteTaskInventoryId)
        {
            PM_TaskEntryBL te = new PM_TaskEntryBL();
            List<PM_SiteTaskInventoryAttachments> list = te.Get_SiteTask_Inventory_Attachments(Filter, SiteTaskInventoryId);
            foreach (var item in list)
            {
                SWI.AirView.Common.WebConfig wc = new SWI.AirView.Common.WebConfig();
                var SiteTaskInventoryPath = wc.AppSettings("SiteTaskInventoryPath");
                string host = Request.Url.Authority;
                string connectionType = Request.Url.Scheme;
                var path = Path.Combine(connectionType+"://" +host+"/" + SiteTaskInventoryPath + "/" + item.SiteTaskInventoryId);

                item.SubDirectory = path;
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult GetSiteTaskInventoryHeader(string Filter, long TaskId, long SiteId)
        {
            PM_TaskEntryBL te = new PM_TaskEntryBL();
            var list = te.Get_SiteTask_InventoryHeader(Filter, TaskId, SiteId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // Managing Data

        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult ManageSiteTaskInventory(string Filter, List<PM_SiteTaskInventory> STI_List, long SiteTaskId, long SiteId,long UserId)
        {
            PM_TaskEntryBL te = new PM_TaskEntryBL();
            dbDataTable ddt = new dbDataTable();

            DataTable dt = ddt.List();

            foreach (var item in STI_List)
            {
                myDataTable.AddRow(dt, "Value1", item.SiteTaskId, "Value2", item.CategoryId, "Value3", item.SiteId, "Value4", item.SiteTaskInventoryId,"Value5",item.IsModified, "Value6", item.ItemId, "Value7", item.Quantity, "Value8", item.BarCode, "Value11", item.Description);
            }
            
            DataTable res = te.Manage_SiteTask_Inventory(Filter, dt, SiteTaskId, SiteId,UserId);

            // Update Attachments Directory
            if (res.Rows.Count > 0)
            {
                for (int i = 0; i < res.Rows.Count; i++)
                {
                    var ExistingPath = res.Rows[i]["SubDirectory"].ToString();
                    var NewPath = res.Rows[i]["InventoryId"].ToString();
                    SWI.AirView.Common.WebConfig wc = new SWI.AirView.Common.WebConfig();
                    var SiteTaskInventoryPath = wc.AppSettings("SiteTaskInventoryPath");

                    var path = Path.Combine(Server.MapPath("~/" + SiteTaskInventoryPath + "/" + ExistingPath));

                    string pathString = System.IO.Path.Combine(path.ToString());
                    bool isExists = System.IO.Directory.Exists(pathString);

                    string NewPathString = (Path.Combine(Server.MapPath("~/" + SiteTaskInventoryPath + "/" +NewPath))).ToString();

                    if (isExists)
                    {
                        System.IO.Directory.Move(pathString, NewPathString);
                    }
                }
            }
            Response response = new Response();
            try
            {
                response.Status = "success";
                response.Message = "Save successfully";
            }
            catch (Exception ex)
            {
                response.Status = "danger";
                response.Message = ex.Message;
            }


            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult UploadAttachments(int SiteTaskInventoryId,long CategoryId,long ItemId)
        {
            PM_TaskEntryBL te = new PM_TaskEntryBL();
            bool isSavedSuccessfully = true;
            string fName = "";
            string SubDirectory = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        SWI.AirView.Common.WebConfig wc = new SWI.AirView.Common.WebConfig();
                        var SiteTaskInventoryPath = wc.AppSettings("SiteTaskInventoryPath");
                        if (SiteTaskInventoryId == 0)
                        {
                            SubDirectory = (CategoryId + "_" + ItemId).ToString();
                        }
                        else
                        {
                            SubDirectory = SiteTaskInventoryId.ToString();
                        }
                        var path = Path.Combine(Server.MapPath("~/" + SiteTaskInventoryPath+"/"+SubDirectory));
                        string pathString = System.IO.Path.Combine(path.ToString());
                        var fileName1 = Path.GetFileName(file.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                        var uploadpath = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(uploadpath);
                        DataTable res = te.Manage_SiteTask_Inventory("Add_Attachments",null,0,0,0,SiteTaskInventoryId,file.FileName,SubDirectory,file.ContentLength);
                        if (res.Rows.Count==0)
                        {
                            isSavedSuccessfully = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully)
            {
                return Json(new
                {
                    Message = fName
                });
            }
            else
            {
                return Json(new
                {
                    Message = "Error in saving file"
                });
            }

        }

        [IsLogin(CheckPermission =false),HttpPost]
        public JsonResult RemoveSiteTaskInventory(string Filter,int SiteTaskInventoryId,long CategoryId,long ItemId,string FileName)
        {
            PM_TaskEntryBL te = new PM_TaskEntryBL();
            try
            {
                if (Filter == "DeleteAttachmentById")
                {
                    SWI.AirView.Common.WebConfig wc = new SWI.AirView.Common.WebConfig();
                    var SiteTaskInventoryPath = wc.AppSettings("SiteTaskInventoryPath");
                    var SubDirectory = "";
                    if (SiteTaskInventoryId != 0)
                    {
                        SubDirectory = SiteTaskInventoryId.ToString();
                    }
                    else
                    {
                        SubDirectory = (CategoryId + "_" + ItemId).ToString();
                    }

                    var path = Path.Combine(Server.MapPath("~/" + SiteTaskInventoryPath + "/" + SubDirectory));
                    try
                    {
                        if ((System.IO.File.Exists(path + "/" + FileName)))
                        {
                            System.IO.File.Delete(path + "/" + FileName);
                        }
                        System.IO.DirectoryInfo di = new DirectoryInfo(path);
                        if (di.GetFiles().Length == 0)
                        {
                            System.IO.Directory.Delete(path, true);
                        }
                    }
                    catch{}
                    te.Manage_SiteTask_Inventory("DeleteAttachmentById", null, 0, 0, 0, SiteTaskInventoryId, FileName, SubDirectory, 0);
                }
                else
                {
                    SWI.AirView.Common.WebConfig wc = new SWI.AirView.Common.WebConfig();
                    var SiteTaskInventoryPath = wc.AppSettings("SiteTaskInventoryPath");
                    var SubDirectory = "";
                    if (SiteTaskInventoryId != 0)
                    {
                        SubDirectory = SiteTaskInventoryId.ToString();
                    }
                    else
                    {
                        SubDirectory = (CategoryId + "_" + ItemId).ToString();
                    }

                    var path = Path.Combine(Server.MapPath("~/" + SiteTaskInventoryPath + "/" + SubDirectory));
                    try
                    {
                        if (System.IO.Directory.Exists(path))
                        {
                            System.IO.DirectoryInfo di = new DirectoryInfo(path);

                            foreach (FileInfo file in di.GetFiles())
                            {
                                file.Delete();
                            }
                            System.IO.Directory.Delete(path, true);

                        }
                    }
                    catch { }
                    te.Manage_SiteTask_Inventory("DeleteSiteTaskInventory", null, 0, 0, 0, SiteTaskInventoryId, FileName, SubDirectory, 0);
                }
            }
            catch
            {

            }
            Response res = new Response()
            {
                Message = "Removed!",
                Status = "success"
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        // End Site Task Inventory
    }
}