using Swi.Airview.Xcelerate.DataTransferObject.Modules.DashboradNReport;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.BusinessIntelligence.Controllers
{
    [IsLogin(CheckPermission = false)]
    public class DashboardsnReportsController : Controller
    {
        // GET: BusinessIntelligence/DashboardsnReports
        public ActionResult LandingPage(string moduleType=null, int? moduleId=null,int? renderId=null,string viewMode=null)
        {
            GenericTemplateParams GenericTemplateParamsObj = new GenericTemplateParams();
            GenericTemplateParamsObj.ModuleType = moduleType;
            GenericTemplateParamsObj.ModuleTypeId = moduleId;
            GenericTemplateParamsObj.RenderId = renderId;
            GenericTemplateParamsObj.ViewMode = viewMode;
            return View(GenericTemplateParamsObj);
        }
        public ActionResult ETLView()
        {
            return View();
        }
        public ActionResult DemoView()
        {
            return View();
        }
        public PartialViewResult GetPartialForEntity1() {

            return PartialView("~/Areas/BusinessIntelligence/Views/DashboardAndReport/design-dashboard-and-report.cshtml");
        }
    }
}