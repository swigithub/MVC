using AirView.DBLayer.Template.BLL;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class ProjectReportController : Controller
    {
        private TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
        // GET: Project/ProjectReport
        [IsLogin(CheckPermission = false)]
        public ActionResult Index(int Id)
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

            ViewBag.Id = Id;
            string ModuleKeyCode = "MD_PROJECT_RPT";
            // var ProjectId = TemplatesBL.ToList("GetProjectIdBySiteId", Id.ToString()).FirstOrDefault();
            int? ProjectId = Id;
            if (ProjectId != null)
            {
                var templateData = TemplatesBL.ToList("IsTemplateExist", ModuleKeyCode).Where(x => x.ProjectId == ProjectId).FirstOrDefault();
                if (templateData != null && ProjectId != null)
                {
                    return Redirect($"/Project/Template/Dashboard?Id={templateData.TemplateId}&ProjectId={templateData.ProjectId}");
                }
            }
            return View();
        }
    }
}