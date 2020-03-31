using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.Model;
using AirView.DBLayer.Security.Entities;
using Library.SWI.Project.BLL;
using Library.SWI.Project.Model;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class DefinationController : Controller
    {
        private DashboardBL loDashboardBL = new DashboardBL();

        public ActionResult All()
        {
            return View();
        }

        public ActionResult All2()
        {
            AD_ProjectsBL p = new AD_ProjectsBL();
            return View();
        }

        public ActionResult AllProject()
        {
            AD_ProjectsBL pBL = new AD_ProjectsBL();

                return Json(pBL.ListProject("ProjectsName"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChartTest(Int64 Id = 0)
        {
            ViewBag.Id = Id;
            return View();
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                AD_Projects p = new AD_Projects();
                AD_ProjectsBL rbl = new AD_ProjectsBL();
                p.ProjectID = Id;
                rbl.Manage("Delete", p);
            }
            catch (Exception ex)
            {
                TempData["msg_error"] = ex.Message;
            }

            return RedirectToAction("All");
        }

        [IsLogin(CheckPermission = true)]
        public ActionResult Details(Int64 Id)
        {
            var oob = Permission.AllowProject(Convert.ToInt64(Id));
            if (oob == null)
            {
                TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                return RedirectToAction("index", "error", new { Area = "Project" });
            }
            else
            {
                
                 TempData["ProjectEntity"]  = oob; TempData.Keep("ProjectEntity");
            }

            List<PM_KPI> ci = new List<PM_KPI>();
            ViewBag.Id = Id;
            return View(ci);
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult Details(Int64 Id,string Key)
        {
            PM_ProjectBL pd = new PM_ProjectBL();
            var Project = pd.ToSingle("ByProjectId", Id.ToString());

            AD_DefinationBL db = new AD_DefinationBL();
            var ProjectStatus = db.ToList("byDefinationType", "Project Status");
            Int64 UserId = ViewBag.UserId;
            var UserScopes = db.ToList("UserScopes", UserId.ToString());
            var Priorities = db.ToList("byDefinationType", "Priority");
            var FormTypes = db.ToList("byDefinationType", "FormType");
            var TaskTypes = db.ToList("byDefinationType", "Task Types");
            Sec_UserBL ud = new Sec_UserBL();
            List<Sec_User> Users = ud.ToList("ByProjectId", Id.ToString());
            Users = Users.Select(c => { c.Password = ""; return c; }).ToList();

            return Json(new { Project= Project, ProjectStatus= ProjectStatus, UserScopes= UserScopes
                        ,Priorities= Priorities,FormTypes= FormTypes,Users= Users,TaskTypes= TaskTypes
            },JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            var oob = Permission.AllowProject(Convert.ToInt64(id));
            if (oob == null)
            {
                TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                return RedirectToAction("index", "error", new { Area = "Project" });
            }
            else
            {
                
                 TempData["ProjectEntity"]  = oob; TempData.Keep("ProjectEntity");
            }

            AD_ProjectsBL pBL = new AD_ProjectsBL();
            var rec = pBL.Single("ById", id.ToString());
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getAll(AD_Projects SearchProject, int pageIndex, int pageSize)
        {
            AD_ProjectsBL p = new AD_ProjectsBL();
            string StartDate = string.Empty;
            string EndDate = string.Empty;

            string ProjectIds = SearchProject.ProjectIds != null ? string.Join(",", SearchProject.ProjectIds) : null;
            string CompanyIds = SearchProject.CompanyIds != null ? string.Join(",", SearchProject.CompanyIds) : null;
            string VendorIds = SearchProject.VendorIds != null ? string.Join(",", SearchProject.VendorIds) : null;
            string StatusId = SearchProject.StatusIds != null ? string.Join(",", SearchProject.StatusIds) : null;
            if (SearchProject.StartDate != null)
            {
                StartDate = Convert.ToDateTime(SearchProject.StartDate).ToString();
            }

            if (SearchProject.EndDate != null)
            {
                EndDate = Convert.ToDateTime(SearchProject.EndDate).ToString();
            }

            //return Json(p.ToList("SearchAll",
            //    SearchProject.ProjectId != null ? string.Join(",", SearchProject.ProjectId) : null,
            //    SearchProject.CompanyId != null ? string.Join(",", SearchProject.CompanyId) : null,
            //    SearchProject.VendorId != null ? string.Join(",", SearchProject.VendorId) : null,
            //    SearchProject.StatusId != null ? string.Join(",", SearchProject.StatusId) : null,
            //    FromDate, EndDate),

            return Json(p.ToList("All", ProjectIds, CompanyIds, VendorIds, StatusId, StartDate, EndDate, pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        //  [ChildActionOnly]
        public ActionResult GetHtmlPage(string path)
        {
            return new FilePathResult("~/Content/js/Plugins/GanttChartNew/gantt.html", "text/html");
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult GetEntities(string filter, string value)
        {
            PM_EntityBL db = new PM_EntityBL();
            var rec = db.ToList(filter, value);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        public ActionResult New(Int32 Id = 0)
        {
            ViewBag.Id = Id;
            if (Id > 0) {
                var oob = Permission.AllowProject(Convert.ToInt64(Id));
                if (oob == null)
                {
                    TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                    return RedirectToAction("index", "error", new { Area = "Project" });
                }
                else
                {
                    
                     TempData["ProjectEntity"]  = oob; TempData.Keep("ProjectEntity");
                }

            }
            return View();
        }


        [HttpPost]
        public ActionResult New(PM_Project p)
        {
            Response res = new Response();
            AL_AlertBL model = new AL_AlertBL();
            try
            {

                // Insert Project Status Change Notification - Start
                AL_GetAlertSubscription Temp = new AL_GetAlertSubscription();
                Temp = model.IsSubscribed("IsSubscribed", (int)ViewBag.UserId, 0, (int)p.ProjectId, "Project Status Change");
                if (Temp.IsSubscribed)
                {
                    int status = model.InsertAlert("Insert_Alert", (int)p.ProjectId, (int)ViewBag.UserId, 0, (int)p.ProjectId, "Project Status Change", (int)p.StatusId);
                }
                // Insert Project Status Change Notification - End

                int x = 0;
                PM_ProjectBL pb = new PM_ProjectBL();
                PM_GazetteHolidaysBL gh = new PM_GazetteHolidaysBL();
                PM_WorkGroupsBL PWG = new PM_WorkGroupsBL();
                PM_TaskStagesBL  pM_TaskStagesBL = new PM_TaskStagesBL();
               if (p.WorkingDays != null) { 
                    p.WorkingDays= p.WorkingDays.Substring(0, p.WorkingDays.Length - 1);
                }
                if (p.ProjectId > 0)
                {
                    pb.Manage("Update", p);

                    Int64 UID = ViewBag.UserId;
                    pb.ProjectUserPermission("UpdateUserPermissions", p.ManagerId, p.ProjectId);
                    //pb.ProjectUserPermission("UpdateUserPermissions", UID, p.ProjectId);
                    Permission.UpdateEntity(ViewBag.UserId);
                    PWG.Manage("Insert", p.ProjectId, p.WorkGroups);
                    if (p.GH != null)
                    {
                        p.GH[0].ProjectId = p.ProjectId;
                        gh.Manage("Delete", p.GH[0]);
                        gh.InsertBulk(p.ProjectId, p.GH);

                        
                    }
                    else
                    {
                        PM_GazetteHolidays g = new PM_GazetteHolidays();
                        g.ProjectId = p.ProjectId;
                        g.Date = DateTime.Now;
                        gh.Manage("Delete", g);
                        pb.ProjectUserPermission("UpdateUserPermissions", p.ManagerId, p.ProjectId);
                        //pb.ProjectUserPermission("UpdateUserPermissions", UID, p.ProjectId);
                        Permission.UpdateEntity(ViewBag.UserId);
                
                    }

                    // Update AND Add Task Stages
                    if(p.TS?.Count > 0)
                    {
                        bool IsEXE = pM_TaskStagesBL.UpdateOrAdd("UpdateOrAdd", p.ProjectId, p.TS);
                    }

                   

                    res.Message = "Update successfully";

                }
                else
                {

                    
                    p.IsActive = true;
                    var PId = pb.Manage("Insert", p);
                    PWG.Manage("Insert", PId, p.WorkGroups);

                    Int64 UID = ViewBag.UserId;
                    pb.ProjectUserPermission("InsertUserPermissions",p.ManagerId, PId);
                    pb.ProjectUserPermission("InsertUserPermissions",UID, PId);
                    Permission.UpdateEntity(ViewBag.UserId);
                    if (p.GH !=null){
                            gh.InsertBulk(PId, p.GH);
                   
                    }

                    if(p?.TS?.Count > 0)
                    {
                        var IsInserted = pM_TaskStagesBL.InsertBulk(PId, p.TS);
                    }
                    res.Value = PId;
                    Permission.AddProject(PId);
                    res.Message = "Save successfully";

                }
                res.Status = "success";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                if(ex.Message=="Object cannot be cast from DBNull to other types.")
                {
                    res.Message = "You Can't Insert Project who is already entered with same name and same client ";
                }
                else
                {
                    res.Message = ex.Message;
                }
      
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NewTest(String p)
        {
            Response res = new Response();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult New2(Int32 Id = 0)
        {
            ViewBag.Id = Id;
            if(Id != 0)
            {
                var oob = Permission.AllowProject(Convert.ToInt64(Id));
                if (oob == null)
                {
                    TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                    return RedirectToAction("index", "error", new { Area = "Project" });
                }
                else
                {
                    
                     TempData["ProjectEntity"]  = oob; TempData.Keep("ProjectEntity");
                }

            }
            return View();
        }

        [HttpPost]
        public ActionResult New2(AD_Projects project, List<string> Ids)
        {
            if (Ids != null)
            {
                project.ProjectScopeID = string.Join(",", Ids);
            }
            else
            {
                project.ProjectScopeID = "0";
            }

            try
            {
                AD_ProjectsBL pBL = new AD_ProjectsBL();
                Int64 ProjectID = 0;

                if (project.ProjectID > 0)
                {
                    ProjectID = pBL.Manage("Update", project);
                }
                else
                {
                    ProjectID = pBL.Manage("Insert", project);
                }
                return Json(new { Success = true, Message = "Saved" });
            }
            catch (Exception ex)
            {
                TempData["msg_error"] = ex.Message;
                return View();
            }
        }
        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult ToList(string Filter, string Value)
        {
            PM_ProjectBL pd = new PM_ProjectBL();
            var User = Session["user"] as LoginInformation;

            var rec = pd.ToList(Filter,true.ToString() ,User.UserId);
            if(Filter == "ByStatus") { 
            List<Sec_UserProjects> lst = new List<Sec_UserProjects>();
            var items = rec.Where(l2 => !User.ProjectPermissions.Any(l1 => l1.ProjectId == l2.ProjectId )).ToList();
            if(items.Count>0)
            {
                foreach(var item in items) { 
                Permission.AddProject(item.ProjectId);
                }
            }
            }
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult ToSingle(string Filter, string Value)

        {
            PM_ProjectBL pd = new PM_ProjectBL();
            PM_GazetteHolidaysBL gh = new PM_GazetteHolidaysBL();
            PM_TaskStagesBL pM_TaskStagesBL = new PM_TaskStagesBL();
            var rec = pd.ToSingle(Filter, Value);
            if(Value != "0") { 
           
                var ghh= gh.ToList("ByProjectId", Value);
                if(ghh.Count > 0)
                {
                    rec.GH = ghh;
                }
              var obj= pM_TaskStagesBL?.ToList("GetByProjectId", long.Parse(Value));
                if(obj.Count > 0)
                {
                    rec.TS = obj;
                }
            }


            
          //  rec.TS = pM_TaskStagesBL?.ToList("GetByProjectId", long.Parse(Value));
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        public bool UpdateActiveStatus(int Id, bool status)
        {
            try
            {
                AD_Projects p = new AD_Projects();
                AD_ProjectsBL rbl = new AD_ProjectsBL();

                p.ProjectID = Id;
                p.IsActive = status;
                rbl.Manage("UpdateStatus", p);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult GetTaskStages(long ProjectId)
        {
            PM_TaskStagesBL pM_TaskStagesBL = new PM_TaskStagesBL();
            var List = pM_TaskStagesBL.ToList("GetByProjectId", ProjectId);
            return Json(List);
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult GetTaskOrProjectStages(long ProjectId, long TaskId)
        {
            PM_TaskStagesBL pM_TaskStagesBL = new PM_TaskStagesBL();
            var List = pM_TaskStagesBL.ToList("GetStagesForTaskOrProject", ProjectId, TaskId);
            return Json(List);
        }


        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult AddTaskStages(long ProjectId, long TaskId, string Stages)
        {
            Response res = new SWI.AirView.Models.Response();
            PM_TaskStagesBL pM_TaskStagesBL = new PM_TaskStagesBL();
            List<PM_TaskStages> TaskStage = new JavaScriptSerializer().Deserialize<List<PM_TaskStages>>(Stages);
            var List = pM_TaskStagesBL.UpdateOrAdd("UpdateOrAddForTaskWorkFlow", ProjectId, TaskId, TaskStage);
            res.Message = "Workflow saved successfully!";
            res.Status = "Ok";
            return Json(res);
        }


        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult IsStageUsed(int? StageId)
        {
            Response res = new SWI.AirView.Models.Response();
            res.Message = "There is an error";
            res.Status = "error";
            if(StageId == null || StageId < 1)
            {
                res.Status = "UnwantedStage";
                res.Message = "Stage deleted succesfully!";
                return Json(res);
            }
            PM_TaskStagesBL pM_TaskStagesBL = new PM_TaskStagesBL();
            var val = pM_TaskStagesBL.Single("IsStageUsed", StageId.Value);
            if (val >= 1)
            {
                res.Status = "used";
                res.Message = "This stage is used in some site tasks.";
            }
            else
            {
                res.Status = "free";
                res.Message = "Stage deleted succesfully!";
            }
            return Json(res);
        }
    }
}