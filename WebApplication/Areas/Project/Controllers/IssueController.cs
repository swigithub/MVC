using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.Model;
using Newtonsoft.Json;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class IssueController : Controller
    {
        PM_IssueBL bal = new PM_IssueBL();
        [IsLogin(CheckPermission = false)]
        public ActionResult Index()
        {
            return View();
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult New()
        {
            
            return PartialView("~/Areas/Project/Views/Issue/_New.cshtml");
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult ChangeIssueStatus()
        {
            return PartialView("~/Areas/Project/Views/Issue/_ChangeIssueStatus.cshtml");
        }

        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult ChangeIssueStatus(PM_IssuesLog IssueLog)
        {
            IssueLog.UserId = ViewBag.UserId;

            if (IssueLog.IssueLogId != 0)
            {
                var res = bal.ManageIssueLog("Insert_IssueLog_Status", IssueLog);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var res = bal.ManageIssueLog("Insert_IssueLog_Status", IssueLog);
                return Json(res, JsonRequestBehavior.AllowGet);
            }


        }


        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult New(PM_Issues Issu)
        {

            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            var issues = HttpContext.Request["Issue"];
            var File = HttpContext.Request["file"];
            PM_Issues Issue = JsonConvert.DeserializeObject<PM_Issues>(issues);

            HttpFileCollectionBase files = HttpContext.Request.Files;
            if (Request.Files != null)
            {
                if (Request.Files.Count > 0)
                {


                    var file = Request.Files[0];
                    actualFileName = file.FileName;
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    int size = file.ContentLength;

                    try
                    {
                        string FilePath = Path.Combine(Server.MapPath("~/Content/Files/") + fileName).ToString();
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/Files/"), fileName));
                        Issue.FilePath = "/Content/Files/" + fileName.ToString(); //FilePath;


                    }
                    catch (Exception ex)
                    {
                        Message = "File upload failed! Please try again";
                    }


                }
            }





            if (Issue.IssueId != 0)
            {
                var res = bal.Manage(bal.Filter_Update, Issue);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var res = bal.Manage(bal.Filter_Insert, Issue, ViewBag.UserId);
                return Json(res, JsonRequestBehavior.AllowGet);
            }


        }

        [IsLogin(CheckPermission = false),HttpGet]
        public ActionResult GetUser(Int64 projectId)
        {
            var result = bal.GetUsers("GET_PROJECT_ISSUES_USERS", projectId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]

        public ActionResult GetTasks(Int64 projectId, Int64 TaskId)
        {
            var result = bal.GetTasks("GET_PROJECT_TASK", projectId, TaskId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GetIssueLog(Int64 IssueId)
        {
            var result = bal.GetIssueLog("GET_IssueLog", IssueId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GetIssue_ById(Int64 IssueId)
        {
            var result = bal.GetIssue("GET_Issue", IssueId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}