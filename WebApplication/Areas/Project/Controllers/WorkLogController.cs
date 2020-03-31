using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.Model;
using SWI.Libraries.Common;
using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class WorkLogController : Controller
    {
        // GET: Project/WorkLog
        [IsLogin(CheckPermission = false)]
        public ActionResult Index(Int64 ProjectId = 0)
        {
            Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
            DataTable UserProjects = udl.GetDataTable("UserProjects", ViewBag.UserId.ToString(), null, null);
            if (ProjectId == 0 || UserProjects.ToList<PM_Projects>().Where(x => x.ProjectId == ProjectId).Count() == 0)
            return RedirectToAction("all", "Defination");

           //DataTable Table = udl.GetDataTable("All_Projects", null, null, null);
            


            Sec_UserBL ud = new Sec_UserBL();
            Sec_User user = ud.Single("ById", ViewBag.UserId.ToString());
            return View(user);
        }
        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult New(List<PM_WorkLog> worklogs)
        {
            PM_WorkLogBL bal = new PM_WorkLogBL();
            var id = Session["user"] ;
            var userId = (LoginInformation)id ;
            worklogs = worklogs.Where(X => X.IsApproved == false).ToList();
            
            foreach(var worklog in worklogs)
            {
                worklog.UserId = Convert.ToInt64(userId.UserId);
                
                if(worklog.WLogId > 0)
                {
                    var result = bal.Manage("EditWorkLog", worklog);
                }
                else { 
                    
                var result = bal.Manage("INSERT_WorkLog", worklog);
                }
            }
          
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult Edit(PM_WorkLog worklog)
        {
            PM_WorkLogBL bal = new PM_WorkLogBL();
            var id = Session["user"];
            var userId = (LoginInformation)id;
            worklog.UserId = Convert.ToInt64(userId.UserId);
            var result = bal.Manage("EditWorkLog", worklog);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GetWorkLogList(Int64 TaskId, Int64 ProjectSiteId, string LogType, Int64 ProjectId)
        {
            PM_WorkLogBL WorkLogBL = new PM_WorkLogBL();
            if (string.IsNullOrEmpty(LogType))
            {
                LogType = "Issue";
            }
            var WorkLogs = WorkLogBL.Get("Get_TaskWorklog", TaskId, ProjectSiteId, LogType, ProjectId);
            return Json(WorkLogs, JsonRequestBehavior.AllowGet);
        }


        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult GetWorklogs(string filter, string value, string value2, string SelectOption)
        {
            PM_WorkLogBL tb = new PM_WorkLogBL();
            value = string.IsNullOrEmpty(value) ? DateTime.Now.ToShortDateString() : value;
            value2 = string.IsNullOrEmpty(value2) ? DateTime.Now.ToShortDateString() : value2;
            var wlogs = tb.ToList(filter, value, value2, SelectOption);
            //var fcHistory = tb.FcHistoryList(filter, value, value2, ProjectId, MilestoneId, StageId);

            var result = new { Worklogs = wlogs };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult WorkLogsCost(string filter, string value, string value2)
        {
            PM_WorkLogBL tb = new PM_WorkLogBL();
            value = string.IsNullOrEmpty(value) ? DateTime.Now.ToShortDateString() : value;
            value2 = string.IsNullOrEmpty(value2) ? DateTime.Now.ToShortDateString() : value2;
            var wlogsCostChart = tb.ToList("Work_Log_Charts", value, value2);
           
            var wlogsCost = tb.ToList(filter, value, value2);

            List<p_pm_worklog> chartsManagenData = wlogsCost.GroupBy(x => x.Name).Select(p => new p_pm_worklog
            {
                Name = p.Key,
                childp_pm_worklog = p.GroupBy(pp => pp.LogType).Select(xc => new p_pm_worklog()
                {
                    LogHours = xc.Sum(x => x.LogHours),
                    LogType = xc.Key,
                    Name = p.Key

                }).ToList()

            }).ToList();

            var fnlobj = chartsManagenData.Select(x => x.childp_pm_worklog).ToList();

            var result = new { WlogsCost = wlogsCost, wlogsCostChart= wlogsCostChart };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false, Return = "")]
        public bool ApproveWorklog(string Filter, Int32 WLogId, bool IsApproved)
        {
            try
            {
                PM_WorkLog wlog = new PM_WorkLog();
                PM_WorkLogBL dal = new PM_WorkLogBL();

                var id = Session["user"];
                var userId = (LoginInformation)id;

                wlog.WLogId = WLogId;
                wlog.UserId = Convert.ToInt64(userId.UserId);
                wlog.LogDate = DateTime.Now; // ApprovalDate is sent in "@LogDate" Parameter
                wlog.IsApproved = IsApproved;

                dal.Manage(Filter, wlog);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}