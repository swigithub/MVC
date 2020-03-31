using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using Library.SWI.Project.BLL;
using Library.SWI.Project.Model;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.Common;
using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class TrackerController : Controller
    {
        // GET: Project/Tracker   
        [IsLogin(CheckPermission = false)]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [IsLogin(CheckPermission = true)]
        public ActionResult  Index( List<PM_Tracker> Data)
        {
            try {
                PM_TrackerDL dd = new PM_TrackerDL();
                dbDataTable dbdt = new dbDataTable();
                DataTable dt = dbdt.PM_ImportLists();
                foreach (var p in Data)
                {
                    p.TrackerGroup = p.TrackerGroup.Split('|').ToArray()[0];
                    myDataTable.AddRow(dt,
                        "Value1", p.TrackerGroup, "Value2", p.Title, "Value3", p.Unit, "Value4", p.Budget, "Value5", p.Actual, "Value6", p.TrackerId, "Value7", p.IsDeleted, "Value8", p.TaskId
                        );
                }


                var _flag = dd.Manage("Insert_Trackers", "22", dt);
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Error to Add Trackers", key = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { msg = "Trackers Added successfully", key = true}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult Save(List<PM_SiteTaskTracker> Data)
        {
            try
            {
                if(Data != null) { 
                PM_SiteTaskTrackersDL dd = new PM_SiteTaskTrackersDL();
                dbDataTable dbdt = new dbDataTable();
                DataTable dt = dbdt.PM_ImportLists();
                foreach (var p in Data)
                {
                        p.TrackerGroup = p.TrackerGroup.Split('|').ToArray()[0];
                    myDataTable.AddRow(dt,
                        "Value1", p.TrackerGroup, "Value2", p.TaskTrackerId, "Value3", p.TaskId, "Value4", p.Actual, "Value5", p.SiteTaskTrackerid, "Value6", p.IsDeleted,"Value7",p.SiteId
                        );
                }


                var _flag = dd.Manage("Insert_SiteTaskTrackers", "22", dt);
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Error to Add Trackers", key = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { msg = "Trackers Added successfully", key = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult NewTrackerGroup(PM_TrackerGroups Data)
        {
            try { 
            PM_TrackerGroupsDL dd = new PM_TrackerGroupsDL();
                if (Data.GroupCode == null)
                {
                    Data.GroupCode = "";
                }
                var _flag = dd.Manage("Insert_Groups", Data.ProjectId, Data.TaskId, Data.Title, Data.ParentId, Data.GroupCode);
            }catch(Exception ex)
            {
                return Json(new { msg = "Error to Add Trackers", key = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { msg = "Trackers Added successfully", key = true }, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetTaskTrackersTitles(string TrackerGroupId,int? TaskId)
        {
            try
            {
                PM_TrackerBL tg = new PM_TrackerBL();
                var obj = tg.ToList("GetTrackerTitles", TrackerGroupId,TaskId.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult TrackerGroupsDynatree(string Filter, int TaskId)
        {
            PM_TrackerGroupsBL tg = new PM_TrackerGroupsBL();
            var dt = tg.ToList(Filter, TaskId.ToString());
            List<TrackerModel> Mlist = new List<TrackerModel>();
            List<TrackerModel> Tlist = new List<TrackerModel>();

            //PM_TaskBL tb = new PM_TaskBL();
            //var dt = tb.ToList(filter, string.Empty, string.Empty, ProjectId, 0);
                List<TrackerModel> MyList = new List<TrackerModel>();
                foreach (var item in dt)
                {
                    TrackerModel mil = new TrackerModel();
                    mil.ParentId = Convert.ToInt32(item.ParentId);
                   if(item.GroupCode!=""&& item.GroupCode != null) { 
                    mil.Title = item.Title+" | "+ item.GroupCode;
                }
                else
                {
                    mil.Title = item.Title;
                }
                mil.TrackerGroupId = item.TrackerGroupId;
                    MyList.Add(mil);
                }
                foreach (var item in MyList.Where(x => x.ParentId == 0))
                {
                    TrackerModel mil = new TrackerModel();
                    mil.Tracker = FlatToHierarchy(MyList, item.TrackerGroupId);
                    mil.TrackerGroupId = item.TrackerGroupId;
                    mil.Title = item.Title ;
                    Mlist.Add(mil);
                }
                return Json(Mlist, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        private List<TrackerModel> FlatToHierarchy(IEnumerable<TrackerModel> list, long parentId = 0)
        {

           

                List < TrackerModel > Childs = (from i in list
                    where i.ParentId == parentId
                    select new TrackerModel
                    {
                        TrackerGroupId = i.TrackerGroupId,
                        Title = i.Title,
                        ParentId = i.ParentId,
                        Tracker = FlatToHierarchy(list, Convert.ToInt64(i.TrackerGroupId))
                    }).ToList();

            return Childs;
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult TrackerDynatree(string Filter, int TaskId)
        {
            PM_TrackerGroupsBL tg = new PM_TrackerGroupsBL();
            var obj = tg.ToList(Filter, TaskId.ToString());
            ViewBag.TrackerGroups = obj;
            return PartialView("~\\Areas\\Project\\Views\\Dashboard\\_trackerGroup.cshtml");
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetTaskTrackersGroups(string Filter,int TaskId )
        {
            try
            {
                PM_TrackerGroupsBL tg = new PM_TrackerGroupsBL();
                var obj = tg.ToList(Filter, TaskId.ToString());


                return Json(obj, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetTaskTrackers(string Filter, int TaskId)
        {
            try
            {
                PM_TrackerBL tg = new PM_TrackerBL();
                var obj = tg.ToList(Filter, TaskId.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetSiteTaskTrackers(string Filter, int TaskId , int SiteId)
        {
            try
            {
                PM_TrackerBL tg = new PM_TrackerBL();
                var obj = tg.ToList(Filter, TaskId.ToString(),SiteId.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}