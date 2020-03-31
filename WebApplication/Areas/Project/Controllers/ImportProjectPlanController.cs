using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using LumenWorks.Framework.IO.Csv;
using SWI.Libraries.Common;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class ImportProjectPlanController : Controller
    {


        PM_ImportProjectPlanBL pbl = new PM_ImportProjectPlanBL();

        // GET: Project/ImportProjectPlan
        [IsLogin(CheckPermission = false)]
        public ActionResult Index()
        {
            return View();
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult UploadProjectPlan(HttpPostedFileBase UploadPlan)
        {
            //HttpFileCollectionBase UploadPlans = Request.Files;
            //HttpPostedFileBase UploadPlan = null;
            //if (UploadPlans.Count > 0)
            //{
            //    UploadPlan = UploadPlans[0];
            //    //var tst = Request.Form["UploadPlan"];
            //}

            dbDataTable ddt = new dbDataTable();
            DataTable FileRecord = ddt.Tb_ImportProjectPlant();
            dbDataTable dbdt = new dbDataTable();
            DataTable dtPlan = new DataTable();
            PM_ImportProjectPlanDL dd = new PM_ImportProjectPlanDL();

            var model = new List<PM_ImportProjectPlan>();
            try
            {
                if (UploadPlan != null && UploadPlan.ContentLength > 0)
                {
                    if (UploadPlan.FileName.EndsWith(".csv") || UploadPlan.FileName.EndsWith(".CSV"))
                    {
                        Stream stream = UploadPlan.InputStream;
                        using (CsvReader csvPlanReader = new CsvReader(new StreamReader(stream), true))
                        {
                            FileRecord.Load(csvPlanReader);

                        }
                        List<PM_ImportProjectPlan> plan = FileRecord.ToList<PM_ImportProjectPlan>();
                        DataTable dt = dbdt.PM_ImportList();
                        foreach (var p in plan)
                        {
                            myDataTable.AddRow(dt, "Value1", p.FACode, "Value2", p.Milestone, "Value3", p.Stage, "Value4", p.Plan, "Value5", p.Forecast, "Value6", p.Target, "Value7", p.Actual,
                                                   "Value8", p.Status  //, "Value9", p.OldPlan, "Value10", p.OldForecast, "Value11", p.OldTarget, "Value12", p.OldActual, "Value13", p.OldStatus
                                              );
                        }
                        dtPlan = dd.GetDataTable("Import_Project_Plan", "0", dt);
                        model = dtPlan.ToList<PM_ImportProjectPlan>();
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
            return PartialView("~/Areas/Project/Views/ImportProjectPlan/_ProjectPlan.cshtml", model);
        }


        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult SaveUploadPlan()
        {
            var FACode = Request["FACode"].Split(',');
            var Milestone = Request["Milestone"].Split(',');
            var Stage = Request["Stage"].Split(',');
            var Plan = Request["item.Plan"].Split(',');
            var Forecast = Request["item.Forecast"].Split(',');
            var Target = Request["item.Target"].Split(',');
            var Actual = Request["item.Actual"].Split(',');
            var Status = Request["Status"].Split(',');

            dbDataTable ddt = new dbDataTable();
            //DataTable dtRecord = ddt.Tb_ImportProjectPlant();
            //DataTable dtPlan = new DataTable();
            PM_ImportProjectPlanDL dd = new PM_ImportProjectPlanDL();

            try
            {
                // List<PM_ImportProjectPlan> plan = dtRecord.ToList<PM_ImportProjectPlan>();
                DataTable dt = ddt.PM_ImportList();
                for (int i = 0; i < FACode.Length; i++)
                {
                    myDataTable.AddRow(dt, "Value1", FACode[i], "Value2", Milestone[i], "Value3", Stage[i], "Value4", Plan[i], "Value5", Forecast[i], "Value6", Target[i], "Value7", Actual[i], "Value8", Status[i]);
                }
                var _Flag = dd.Manage("Save_Project_Plan", "20021", dt);

                if (_Flag)
                {
                    TempData["msg_success"] = "Save successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg_error"] = ex.Message;
            }
            return View("~/Areas/Project/Views/ImportProjectPlan/Index.cshtml");
        }


        [IsLogin(CheckPermission = false)]
        public ActionResult UploadWRIssues(HttpPostedFileBase UploadIssues)
        {
            dbDataTable ddt = new dbDataTable();
            DataTable FileRecord = ddt.Tb_ImportWR_Issues();
            dbDataTable dbdt = new dbDataTable();

            DataTable dtPlan = new DataTable();
            try
            {
                if (UploadIssues != null && UploadIssues.ContentLength > 0)
                {
                    if (UploadIssues.FileName.EndsWith(".csv") || UploadIssues.FileName.EndsWith(".CSV"))
                    {
                        Stream stream = UploadIssues.InputStream;
                        using (CsvReader csvPlanReader = new CsvReader(new StreamReader(stream), true))
                        {
                            FileRecord.Load(csvPlanReader);

                        }
                        List<MP_Import_WR_Issues> plan = FileRecord.ToList<MP_Import_WR_Issues>();
                        DataTable dt = dbdt.PM_ImportList();
                        foreach (var p in plan)
                        {
                            myDataTable.AddRow(dt, "Value1", p.FACode, "Value2", p.eNB, "Value3", p.othereNB, "Value4", p.Schedule, "Value5", p.Actual, "Value6", p.MW, "Value7", p.Status,
                                                   "Value8", p.Alarm, "Value9", p.Issues, "Value10", p.WhoFix, "Value11", p.Notes, "Value12", p.ContentType, "Value13", p.Attachments, "Value14", p.Created, "Value15", p.CreatedBy
                                              );

                        }
                        if (pbl.Manage("Import_WR_Issues", "20021", dt))
                        {
                            TempData["msg_success"] = "Save successfully.";
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
            return View("~/Areas/Project/Views/ImportProjectPlan/Index.cshtml");

        }


        [IsLogin(CheckPermission = false)]
        public ActionResult UploadWRSite(HttpPostedFileBase UploadSites)
        {
            dbDataTable ddt = new dbDataTable();
            DataTable FileRecord = ddt.Tb_ImportWR_Site();
            dbDataTable dbdt = new dbDataTable();

            DataTable dtPlan = new DataTable();
            try
            {
                if (UploadSites != null && UploadSites.ContentLength > 0)
                {
                    if (UploadSites.FileName.EndsWith(".csv") || UploadSites.FileName.EndsWith(".CSV"))
                    {
                        Stream stream = UploadSites.InputStream;
                        using (CsvReader csvPlanReader = new CsvReader(new StreamReader(stream), true))
                        {
                            FileRecord.Load(csvPlanReader);

                        }
                        List<MP_Import_WR_Site> plan = FileRecord.ToList<MP_Import_WR_Site>();

                        
                        DataTable dt = dbdt.PM_ImportList();
                        foreach (var p in plan)
                        {
                            myDataTable.AddRow(dt, "Value1", p.FACode, "Value2", p.eNB, "Value3", p.othereNB, "Value4", p.Reasons, "Value5", p.Schedule, "Value6", p.Actual, "Value7", p.MW,
                                                   "Value8", p.Status, "Value9", p.Alarm, "Value10", p.Issues, "Value11", p.Notes, "Value12", p.AddlNotes, "Value13", p.NetAct, "Value14", p.USID, "Value15", p.ContentType, "Value16", p.Created, "Value17", p.CreatedBy
                                              );
                        }

                        if (pbl.Manage("Import_WR_Site", "20021", dt))
                        {
                            TempData["msg_success"] = "Save successfully.";
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
            //return Json(new { dtPlan = dtPlan }, JsonRequestBehavior.AllowGet);
            return View("~/Areas/Project/Views/ImportProjectPlan/Index.cshtml");
        }
    }
}

