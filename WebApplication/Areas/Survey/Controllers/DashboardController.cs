using AirView.DBLayer.Survey.Model;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirView.DBLayer.Survey.BLL;

namespace WebApplication.Areas.Survey.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Survey/Dashboard
        public ActionResult Index()
        {
            return View();
        }


        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult SaveSiteAttendees(List<TSS_SiteAttendees> siteAttendees, int siteId, int siteSurveyId)
        {
            new TSS_DashboardBL().Manage(siteAttendees, siteId, siteSurveyId);
           
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        [IsLogin(CheckPermission = false)]
        public ActionResult GetSiteAttendees(int siteId, int siteSurveyId)
        {
            var res = new TSS_DashboardBL().GetSiteAttendees("Get_Site_Attendees_For_Dashboard", siteId, siteSurveyId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}