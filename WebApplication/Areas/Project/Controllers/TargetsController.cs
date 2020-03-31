using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.Model;
using Library.SWI.Project.BLL;
using LumenWorks.Framework.IO.Csv;
using SWI.AirView.Models;
using SWI.Libraries.Common;
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
    public class TargetsController : Controller
    {
        // GET: Project/Targets
        [IsLogin(CheckPermission = true)]
        public ActionResult Index(Int64 id = 0)
        {
            TempData["ProjectId"] = Convert.ToString(id);
            if (id > 0)
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
            }
            ViewBag.Id = id;
            return View();
        }


        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult ToList(string filter, string value, string value2)
        {
            PM_TaskBL tb = new PM_TaskBL();
            var rec = tb.ToList(filter, value = "0", value2 = "0", 0, 0);
            //if (Resources)
            //{
            //    PM_ProjectResourceBL prb = new PM_ProjectResourceBL();
            //    foreach (var item in rec)
            //    {
            //        item.ProjectResources = prb.ToList("byTaskId", item.TaskId.ToString(), ProjectId, TaskId);
            //    }
            //}
            return Json(rec, JsonRequestBehavior.AllowGet);
        }



        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult GetTargetDates(string filter, string value, string value2, Int64 ProjectId, Int64 MilestoneId = 0, Int64 StageId = 0)
        {
            PM_TargetsBL tb = new PM_TargetsBL();

            var tDates = tb.ToList(filter, value, value2, 0, 0);
            var fcHistory = tb.FcHistoryList(filter, value, value2, ProjectId, MilestoneId, StageId);

            var result = new { targetDates = tDates, fcHistory = fcHistory };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult NewTarget(List<PM_Targets> target)
        {
            Response res = new Response();
            PM_TargetsBL tb = new PM_TargetsBL();
            dbDataTable dbdt = new dbDataTable();
            try
            {
                DataTable dt = dbdt.List();
                foreach (var tr in target)
                {
                    myDataTable.AddRow(dt, "Value1", tr.ProjectId, "Value2", tr.MilestoneId, "Value3", tr.StageId, "Value4", tr.TargetType, "Value5", tr.StartDate, "Value6", tr.TargetValue, "Value7", tr.SiteCount, "Value8", tr.UserId, "Value9", tr.StartDate, "Value10", tr.EndDate);
                }

                if (tb.Manage("Insert", "", dt))
                {
                    res.Value = true;
                    res.Status = "200";
                    res.Message = "Success";
                }
                else
                {
                    res.Value = false;
                    res.Status = "error";
                    res.Message = "record not save.";
                }
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult FileUpload(HttpPostedFileBase Upload)
        {
            dbDataTable ddt = new dbDataTable();
            DataTable FileRecord = ddt.Tb_PM_Target();
            PM_TargetsBL tb = new PM_TargetsBL();
            PM_ProjectBL P = new PM_ProjectBL();
            PM_TaskBL ttb = new PM_TaskBL();
            dbDataTable dbdt = new dbDataTable();
            TempData.Keep("ProjectId");
            string ProjectId = TempData["ProjectId"] as string;
            try
            {
                if (Upload != null && Upload.ContentLength > 0)
                {
                    if (Upload.FileName.EndsWith(".csv") || Upload.FileName.EndsWith(".CSV"))
                    {
                        Stream stream = Upload.InputStream;
                        using (CsvReader csvReader =
                          new CsvReader(new StreamReader(stream), true))
                        {
                            FileRecord.Load(csvReader);
                        }
                        List<PM_Target_File> target = FileRecord.ToList<PM_Target_File>();
                        var dtt = ttb.ToList("Get_Project_Tasks", string.Empty, string.Empty,Convert.ToInt64(ProjectId), 0).ToArray();
                        string ProjectName = P.ToList("ByProjectId", ProjectId,0).Select(x=>x.ProjectName).FirstOrDefault();
                        var id = Session["user"];
                        var userId = (LoginInformation)id;
                        DataTable dt = dbdt.List();
                        foreach (var tr in target)
                        {  
                            if(tr.Project == ProjectName) { 
                            var results = Array.FindAll(dtt, s => s.Title.Equals(tr.Task)).FirstOrDefault();
                            if (results != null) {
                                    if(tr.TargetType.ToLower() == "day")
                                    {
                                        myDataTable.AddRow(dt, "Value1", ProjectId, "Value2", results.TaskId, "Value3", null, "Value4", tr.TargetType, "Value5", tr.TargetValue, "Value6", null, "Value7", tr.SiteCount, "Value8", userId.UserId.ToString());
                                    }
                                    else
                                    {
                                        myDataTable.AddRow(dt, "Value1", ProjectId, "Value2", results.TaskId, "Value3", null, "Value4", tr.TargetType, "Value5", null, "Value6", tr.TargetValue, "Value7", tr.SiteCount, "Value8", userId.UserId.ToString());
                                    }
                                        }
                            }
                        }
                        if (tb.Manage("Insert", "", dt))
                        {
                            TempData["msg_success"] = "Save successfully.";
                        }
                        else { 
                        TempData["msg_nothing"] = "No row effect";
                        }
                    }
                    else
                    {
                        TempData["msg_error"] = "Select .csv File";
                    }
                }
                else
                {
                    TempData["msg_error"] = "No file selected";
                }
            }
            catch (Exception ex)
            {
                TempData["msg_error"] = ex.Message;
            }
            return RedirectToAction("Index", new { @id = Convert.ToInt64(ProjectId) });
        }
    }
}