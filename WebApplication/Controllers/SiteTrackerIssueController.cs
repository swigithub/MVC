using SWI.AirView.Models;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.Security.Filters;
using System;
using System.Web.Mvc;

namespace SWI.AirView.Controllers
{
    /*----MoB!----*/
    [IsLogin, ErrorHandling]
    public class SiteTrackerIssueController : Controller
    {
        // GET: SiteTrackerIssue



        [IsLogin(Return = "",CheckPermission =false),HttpPost]
        public ActionResult New(AV_SiteIssueTracker sit)
        {
            Response r = new Response();
            try
            {
                AV_SiteIssueTrackerBL sitd = new AV_SiteIssueTrackerBL();
                if (sitd.Manage("Insert", sit, ViewBag.UserId))
                {
                  
                    r.Message = "success";
                    r.Status = "success";
                }
               
            }
            catch (Exception ex)
            {

                r.Status = "error";
                r.Message = ex.Message;
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }

        //[IsLogin(Return = "")]
        //public ActionResult Notifications()
        //{
        //    AV_SiteIssueTrackerBL sitb = new AV_SiteIssueTrackerBL();
        //    var rec = sitb.ToList("byStatus", "UN_RESOLVED").OrderByDescending(m=>m.ReportDate.Date).ToList();
           
        //    return PartialView("~/views/SiteTrackerIssue/_Notifications.cshtml", rec);
        //}


        /*----MoB!----*/
        [IsLogin(Return = "")]

        public ActionResult NetLayer(AV_SiteIssueTracker sit)
        {
            //AV_SiteIssueTracker s = new AV_SiteIssueTracker();
            //s.SiteId


            AV_SiteIssueTrackerBL sitb = new AV_SiteIssueTrackerBL();
            ViewBag.SiteId = sit.SiteId;
            ViewBag.TesterId = sit.TesterId;
            ViewBag.NetworkModeId = sit.NetworkModeId;
            ViewBag.BandId = sit.BandId;
            ViewBag.CarrierId = sit.CarrierId;
            ViewBag.ScopeId = sit.ScopeId;
            var rec = sitb.ToList("byNetLayer", sit);
            Common.SelectedList sl = new Common.SelectedList();
            ViewBag.IssueTypes = sl.IssueTypes();

            return PartialView("~/views/SiteTrackerIssue/_NetLayer.cshtml", rec);
        }

        [IsLogin(CheckPermission = false, Return = "")]
        [HttpPost]
        public string UpdateStatus(int TrakingId, string Status)
        {
            try
            {
                AV_SiteIssueTrackerBL sitd = new AV_SiteIssueTrackerBL();
                AV_SiteIssueTracker issue = new AV_SiteIssueTracker();
                issue.TrackingId = TrakingId;
                issue.Status = Status;
                return sitd.Manage("UpdateStatus", issue,ViewBag.UserId).ToString();
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }


        public ActionResult TesterTracker( string SiteId , string NetworkModeId,  string BandId , string CarrierId ) {
            AV_WoTrackerBL wtb = new AV_WoTrackerBL();
         var rec=   wtb.ToList("ByNetLayer", SiteId,NetworkModeId,BandId,CarrierId);

            return PartialView("~/views/SiteTrackerIssue/_TesterTracker.cshtml", rec);
        }

        public ActionResult StatusTracker(string SiteId, string NetworkModeId, string BandId, string CarrierId)
        {
            AV_WoTrackerBL wtb = new AV_WoTrackerBL();
            var rec = wtb.ToList("StatusTracker", SiteId, NetworkModeId, BandId, CarrierId);
            return PartialView("~/views/SiteTrackerIssue/_StatusTracker.cshtml", rec);
        }
    }
}