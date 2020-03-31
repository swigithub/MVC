using AirView.DBLayer.Project.BLL;
using Library.SWI.Project.BLL;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class TodoController : Controller
    {
        // GET: Project/Todo
        PM_TodoBL bl = new PM_TodoBL();
        [IsLogin(CheckPermission = false)]
        public ActionResult Index(Int64 ProjectId, string DateRange = null)
        {
            var DateArrays = DateRange.Split('-');
            string WhereClause = "";
            if (DateArrays?.Length == 2)
            {
                WhereClause = $" AND CONVERT(varchar, ToDoDateTime, 1) >= '{DateArrays[0].ToString().Trim()}' AND CONVERT(varchar, ToDoDateTime, 1) <= '{DateArrays[1].ToString().Trim()}'";
            }
            else
            {
                WhereClause = $" AND CONVERT(varchar, ToDoDateTime, 1) >= '{DateTime.Now.AddMonths(-1).AddDays(1).ToString("MM/dd/yyyy")}' AND CONVERT(varchar, ToDoDateTime, 1) <= '{DateTime.Now.ToString("MM/dd/yyyy")}'";
            }
            
            var result = bl.GetTodo("Get_Todo", ProjectId, ViewBag.UserId, WhereClause);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult GetSitesList(Int64 ProjectId)
        {
            PM_ProjectBL bl = new PM_ProjectBL();
            var Sites = bl.Get("ProjectSitesByProjectId", ProjectId).Select(p => new { SiteName = p.SiteName, SiteId = p.ProjectSiteId }).ToList();
            return Json(Sites, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult GetSitesTask(Int64 ProjectSiteId)
        {
            PM_ProjectBL bl = new PM_ProjectBL();
            var Tasks = bl.GetTasks("ProjectSiteTasks", ProjectSiteId).Select(p => new { TaskId = p.TaskId, Title = p.Title }).ToList();
            return Json(Tasks, JsonRequestBehavior.AllowGet);
        }
    }
}