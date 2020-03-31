using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using LumenWorks.Framework.IO.Csv;
using OfficeOpenXml;
using SWI.Libraries.Common;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;


using Microsoft.SqlServer.Types;
using WebApplication.Areas.Project.ObjectComparer;

namespace WebApplication.Areas.Project.Controllers
{
    public class ImportProjectPlansController : Controller
    {
        PM_ImportProjectPlanBL pbl = new PM_ImportProjectPlanBL();

        [IsLogin(CheckPermission = true)]
        public ActionResult Index(Int64 Project=0)
        {
            var oob = Permission.AllowProject(Convert.ToInt64(Project));
            if (oob == null || Project ==0)
            {
                TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                return RedirectToAction("index", "error", new { Area = "Project" });
            }
           else
            {
                
                 TempData["ProjectEntity"]  = oob; TempData.Keep("ProjectEntity");
            }

            ViewBag.Project = Project;
            return View();
        }

        //FA_Code USID    Common ID   REGION MARKET  SUB Market  Site Name   Street_Address CITY    State ZIP COUNTY Latitude    Longitude vMME   
        //Controlled Introduction Super Bowl  Site_Type DAS_or_Inbuilding   FirstNet RAN    iPlan Job   iPlan Status    iPlan Issue Date PACE Number TSS_Plan  

        //TSS_Forecast TSS_Submitted   Site_Specific_Material_Available_Forecast Site_Specific_Material_Available_Actual Pre_Install_Planned Pre_Install_Fcst   
        //Pre_Install_Actual Mig_Date_Planned    Mig_Date_Forecast Migration Date EPL Ordered EPL Called Out  EPL Delivered   EPL Status

        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult UploadProjectPlan(HttpPostedFileBase uploadMasterEx)
        {
            string fullUrl = Request.UrlReferrer.ToString();
            string ProjectId = fullUrl.Split('=').Last();
            dbDataTable ddt = new dbDataTable();
            DataTable FileRecord = ddt.Tb_MasterEx();

            dbDataTable dbdt = new dbDataTable();
            DataTable dtPlan = new DataTable();

            PM_ImportProjectPlanDL dd = new PM_ImportProjectPlanDL();
            // var model = new List<PM_ImportProjectPlans>();

            List<PM_ImportProjectPlansResult> returedData = new List<PM_ImportProjectPlansResult>();
            try
            {
                if (uploadMasterEx != null && uploadMasterEx.ContentLength > 0 && ProjectId != "")
                {
                    if (uploadMasterEx.FileName.EndsWith(".csv") || uploadMasterEx.FileName.EndsWith(".CSV"))
                    {
                        Stream stream = uploadMasterEx.InputStream;
                        using (CsvReader csvPlanReader = new CsvReader(new StreamReader(stream), true))
                        {
                            FileRecord.Load(csvPlanReader);

                        }
                        List<PM_ImportProjectPlans> plan = FileRecord.ToList<PM_ImportProjectPlans>();
                        string[] columnNames = FileRecord.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
                        string[] Headers = { "FACode", "Cluster", "Market", "Latitude", "Longitude", "SiteName" , "SiteType", "SiteClass", "Scope", "Description",
                        "Task","PlanDate","ForecastStartDate","ForecastEndDate","TargetDate","ActualStartDate","ActualEndDate","Status"
                        ,"Priority","Completion"};
                      
                            var areEqual = Headers.Intersect(columnNames).Count();
                        if(areEqual !=20)
                         return Json(new { msg = "File with invalid format !", key = false }, JsonRequestBehavior.AllowGet);


                            DataTable dt = dbdt.PM_ImportLists();
                        int Counter = 1;
                        foreach (var p in plan)
                        {
                            myDataTable.AddRow(dt,
                                "Value1", p.FACode, "Value2", p.Task, "Value3", p.PlanDate, "Value4", p.ForecastStartDate, "Value5", p.ForecastEndDate,
                                "Value6", p.TargetDate, "Value7", p.ActualStartDate,
                                "Value8", p.ActualEndDate, "Value9", p.Status, "Value10", p.Resources, "Value11", p.Cluster, "Value12", p.Market, "Value13",
                                p.Latitude, "Value14", p.Longitude, "Value15", p.SiteName, "Value16", p.SiteType, "Value17", p.SiteClass, "Value18", p.Scope, "Value19",
                                p.Description, "Value20", p.Status, "Value21", p.Priority,"Value22",p.Completion,"Value23",DateTime.Now, "Value24",ViewBag.UserId
                                , "Value25",DateTime.Now,"Value26",ViewBag.UserId,"Value27", Counter,"Value28","","Value29",""
                                );
                            Counter++;
                        }

                       

                        


                        //////////
                        DataTable dt2 = dbdt.PM_ImportLists();
                      //  var Errors = "";
                       // List<string> ErrorList = new List<string>();
                        //foreach (var p in plan)
                        //{
                        //    if (string.IsNullOrEmpty(p.Priority) == false && string.IsNullOrEmpty(p.Completion.ToString()) == false && string.IsNullOrEmpty(p.Status) == false && string.IsNullOrEmpty(p.Task) == false && string.IsNullOrEmpty(p.Latitude) == false && string.IsNullOrEmpty(p.Longitude) == false && string.IsNullOrEmpty(p.SiteType) == false && string.IsNullOrEmpty(p.CITY) == false && string.IsNullOrEmpty(p.SiteName) == false && string.IsNullOrEmpty(p.Scope) == false && string.IsNullOrEmpty(p.Cluster) == false)
                        //    {
                        //        myDataTable.AddRow(dt,
                        //        "Value1", p.FACode, "Value2", p.Task, "Value3", p.PlanDate, "Value4", p.ForecastStartDate, "Value5", p.ForecastEndDate,
                        //        "Value6", p.TargetDate, "Value7", p.ActualStartDate,
                        //        "Value8", p.ActualEndDate, "Value9", p.Status, "Value10", p.Resources, "Value11", p.Cluster, "Value12", p.Market, "Value13",
                        //        p.Latitude, "Value14", p.Longitude, "Value15", p.SiteName, "Value16", p.SiteType, "Value17", p.SiteClass, "Value18", p.Scope, "Value19",
                        //        p.Description, "Value20", p.Status, "Value21", p.Priority, "Value22", p.Completion, "Value23", DateTime.Now, "Value24", ViewBag.UserId
                        //        , "Value25", DateTime.Now, "Value26", ViewBag.UserId
                        //        );
                        //    }
                        //    else
                        //    {
                        //        if (string.IsNullOrEmpty(p.Priority) == true)
                        //        {
                        //            Errors = "FA Code";
                        //        }
                        //        if (string.IsNullOrEmpty(p.Completion.ToString()) == true)
                        //        {
                        //            Errors = Errors + ", Task";
                        //        }
                        //        if (string.IsNullOrEmpty(p.Status) == true)
                        //        {
                        //            Errors = Errors + ", Category";
                        //        }
                        //        if (string.IsNullOrEmpty(p.Task) == true)
                        //        {
                        //            Errors = Errors + ", Type";
                        //        }
                        //        if (string.IsNullOrEmpty(p.Latitude) == true)
                        //        {
                        //            Errors = Errors + ", WhoFix";
                        //        }
                        //        if (string.IsNullOrEmpty(p.Longitude) == true)
                        //        {
                        //            Errors = Errors + ", Severity";
                        //        }
                        //        if (string.IsNullOrEmpty(p.SiteType) == true)
                        //        {
                        //            Errors = Errors + ", Description";
                        //        }
                        //        if (string.IsNullOrEmpty(p.SiteName) == true)
                        //        {
                        //            Errors = Errors + ", Status";
                        //        }
                        //        if (string.IsNullOrEmpty(p.CITY) == true)
                        //        {
                        //            Errors = Errors + ", AssignedTo";
                        //        }
                        //        if (string.IsNullOrEmpty(p.Scope) == true)
                        //        {
                        //            Errors = Errors + ", Priority";
                        //        }
                        //        if (string.IsNullOrEmpty(p.Cluster) == true)
                        //        {
                        //            Errors = Errors + ", TargetDate";
                        //        }
                               
                        //        Errors = Errors + " are required.";
                        //        myDataTable.AddRow(dt,
                        //        "Value1", p.FACode, "Value2", p.Task, "Value3", p.PlanDate, "Value4", p.ForecastStartDate, "Value5", p.ForecastEndDate,
                        //        "Value6", p.TargetDate, "Value7", p.ActualStartDate,
                        //        "Value8", p.ActualEndDate, "Value9", p.Status, "Value10", p.Resources, "Value11", p.Cluster, "Value12", p.Market, "Value13",
                        //        p.Latitude, "Value14", p.Longitude, "Value15", p.SiteName, "Value16", p.SiteType, "Value17", p.SiteClass, "Value18", p.Scope, "Value19",
                        //        p.Description, "Value20", p.Status, "Value21", p.Priority, "Value22", p.Completion, "Value23", DateTime.Now, "Value24", ViewBag.UserId
                        //        , "Value25", DateTime.Now, "Value26", ViewBag.UserId
                        //        );
                        //        //dt.Columns.Remove("ActualCarrier");
                        //        ErrorList.Add(Errors);
                        //        Errors = "";
                        //    }
                        //}
                        //if (dt2.Rows.Count > 0)
                        //{
                        //    using (var package = new ExcelPackage())
                        //    {
                        //        string handle = Guid.NewGuid().ToString();
                        //        MemoryStream memStream;
                        //        var Where = "Errors in Issues Log ";
                        //        //  FileName = dt.Rows[0][4] + "_" + dt.Rows[0][2] + "_" + dt.Rows[0][3] + "_" + dt.Rows[0][5] + "_" + DateTime.Now.ToString();
                        //        var FileName = Where + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                        //        var Sheet1 = package.Workbook.Worksheets.Add(Where.ToString());
                        //        //for (int t = 0; t < temp.Rows.Count; t++)
                        //        //{
                        //        //string TestType = (temp.Columns.Contains("SiteType")) ? temp.Rows[t]["SiteType"].ToString() : "";


                        //        //dt.DefaultView.RowFilter = "SiteType='" + TestType + "'";
                        //        DataTable CCW = dt2;
                        //        // CCW.Columns.Remove("SiteType");

                        //        //if (t==0)
                        //        //{ 
                        //        // Sheet1 = package.Workbook.Worksheets.Add("ProjectSummary");
                        //        //}
                        //        int x = FileRecord.Columns.Count;

                        //        for (int i = 0; i < FileRecord.Columns.Count; i++)
                        //        {

                        //            Sheet1.Cells[1, i + 1].Value = FileRecord.Columns[i].ColumnName;

                        //            if (CCW.Columns[i].DataType == typeof(System.DateTime))
                        //            {
                        //                Sheet1.Column(i + 1).Style.Numberformat.Format = "mm/dd/yyyy HH:mm";
                        //            }
                        //            if (i + 1 == x)
                        //            {
                        //                Sheet1.Cells[1, i + 2].Value = "Errors";
                        //            }
                        //        }

                        //        for (int i = 0; i < CCW.Rows.Count; i++)
                        //        {
                        //            int y = 26;
                        //            for (int j = 0; j < 26; j++)
                        //            {
                        //                if (CCW.Rows[i][j].ToString() != "2" && CCW.Rows[i][j].ToString() != "1/1/1900 12:00:00 AM")
                        //                {
                        //                    Sheet1.Cells[i + 2, j + 1].Value = CCW.Rows[i][j];
                        //                }
                        //                if (j + 1 == y)
                        //                {
                        //                    var value2 = ErrorList[i];
                        //                    int index1 = value2.IndexOf(',');
                        //                    if (index1 != -1)
                        //                    {
                        //                        ErrorList[i] = value2.Remove(index1, 1);
                        //                    }
                        //                    Sheet1.Cells[i + 2, j + 3].Value = ErrorList[i];
                        //                }
                        //            }
                        //        }

                        //        //}

                        //        memStream = new MemoryStream(package.GetAsByteArray());
                        //        TempData[handle] = memStream.ToArray();
                        //        return new JsonResult()
                        //        {
                        //            Data = new { FileGuid = handle, FileName = "ErrorIssuesLogs.xlsx" }
                        //        };

                        //    }



                        //}


                        /////////
                        var _flag = true; //dd.Manage("Import_ProjectPlan_Data", ProjectId, dt);

                      
                      //  _dt = dt.AsEnumerable().Skip(0).Take(1).CopyToDataTable();

                        if (_flag)
                        {
                            dtPlan = dd.GetDataTable("Save_Project_Plan", ProjectId, dt,ViewBag.UserId); //dtTopRow 3rd parameter commented

                            if (dtPlan.Rows.Count > 0)
                            {
                                //List<PM_ImportProjectPlans> _LatestRevision = new List<PM_ImportProjectPlans>();
                                returedData = dtPlan.ToList<PM_ImportProjectPlansResult>();
                                //foreach (var item in returedData)
                                //{
                                //    var _Converter = new PM_ImportProjectPlans();
                                //    _Converter.FACode = item.Value1;
                                //    _Converter.SiteName = item.Value15;
                                //    _Converter.SiteType = item.Value16;
                                //    _Converter.Task = item.Value2;
                                //    _Converter.PlanDate = item.Value3 != ""  ? item.Value3.Split(' ')[0] : "" ;
                                //    _Converter.ForecastStartDate = item.Value4 != "" ? item.Value4.Split(' ')[0] : "";
                                //    _Converter.ForecastEndDate = item.Value5 != "" ? item.Value5.Split(' ')[0] : "";
                                //    _Converter.TargetDate = item.Value6 != "" ? item.Value6.Split(' ')[0] : "";
                                //    _Converter.ActualStartDate = item.Value7 != "" ? item.Value7.Split(' ')[0] : "";
                                //    _Converter.ActualEndDate = item.Value8 != "" ? item.Value8.Split(' ')[0] : "";
                                //    _Converter.Status = item.Value20;
                                //    _Converter.Priority = item.Value21;
                                //    _Converter.Completion = item.Value22 != "" ? Convert.ToInt64(item.Value22) : 0;
                                //    _LatestRevision.Add(_Converter);

                                //}

                                DataTable _dt = new DataTable();
                                _dt = dd.GetDataTable("Get_Revision_AllProperties", Convert.ToInt64(ProjectId), ViewBag.UserId,Convert.ToInt64(returedData[0].Value32));
                                List<PM_ImportProjectPlans> _LastRevision = _dt.ToList<PM_ImportProjectPlans>();


                                DataTable _dt2 = new DataTable();
                                _dt2 = dd.GetDataTable("Get_Last_Revision", Convert.ToInt64(ProjectId), ViewBag.UserId,0);
                                List<PM_ImportProjectPlans> _LatestRevision = _dt2.ToList<PM_ImportProjectPlans>();


                                List<PM_ImportProjectPlans> _differences = new List<PM_ImportProjectPlans>();
                                foreach(var item in _LatestRevision)
                                {
                                    PM_ImportProjectPlans _matchFound = _LastRevision.Where(x => x.Task == item.Task && x.SiteTaskId == item.SiteTaskId && x.Task !="" && x.SiteTaskId >0 ).FirstOrDefault();
                                    if(_matchFound != null)
                                    {
                                         PM_ImportProjectPlans _changes = _matchFound.EnumeratePropertyDifferences(item);
                                         PM_ImportProjectPlansResult _newObject=   returedData.Where(x => x.Value2 == item.Task && x.Value31.ToString() == item.SiteTaskId.ToString() && x.Value2 != "" && x.Value31 != "").FirstOrDefault();
                                         if(_newObject != null)
                                        {
                                            _newObject.Value2 = _changes.Task;
                                            _newObject.Value3 = _changes.PlanDate;// != "" ? _changes.PlanDate.Split(' ')[0] : "";
                                            _newObject.Value4 = _changes.ForecastStartDate;// != "" ? _changes.ForecastStartDate.Split(' ')[0] : "";
                                            _newObject.Value5 = _changes.ForecastEndDate;// != "" ? _changes.ForecastEndDate.Split(' ')[0] : "";
                                            _newObject.Value6 = _changes.TargetDate;// != "" ? _changes.TargetDate.Split(' ')[0] : "";
                                            _newObject.Value7 = _changes.ActualStartDate;// != "" ? _changes.ActualStartDate.Split(' ')[0] : "";
                                            _newObject.Value8 = _changes.ActualEndDate;//  != "" ? _changes.ActualEndDate.Split(' ')[0] : "";
                                            _newObject.Value9 = _changes.Status;
                                            _newObject.Value21 = _changes.Priority;
                                            _newObject.Value22 = _changes.Completion.ToString();
                                            _newObject._difference = _changes._difference;
                                        }
                                    }

                                }

                                // List<PM_ImportProjectPlans> _Intersection = _LastRevision.Intersect(_LatestRevision).ToList();
                              //  List<PM_ImportProjectPlans> _Intersection = _LastRevision.Select(f => f.Task).Intersect(_LatestRevision.Select(b => b.Task).ToList());
                                return Json(new { msg = "Data imported successfully", key = true, res = returedData }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { msg = "No data !", key = false, res = returedData }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { msg = "Error while import data", key = false }, JsonRequestBehavior.AllowGet);
                        }




                        //int totalRows = dt.Rows.Count;
                        //int subsetSize = 1000;

                        //int loopSize = totalRows / subsetSize;
                        //int remainderVal = totalRows % subsetSize;

                        //int startVal = 1;
                        //int endVal = 0;

                        //for (int i = 1; i <= loopSize; i++)
                        //{
                        //    startVal = endVal;
                        //    endVal = endVal + subsetSize;

                        //    DataTable dtChunk = new DataTable();
                        //    dtChunk = dt.AsEnumerable().Skip(startVal).Take(subsetSize).CopyToDataTable();

                        //    //dtPlan.Merge(dd.GetDataTable("Save_Project_Plan", "20021", dtChunk));
                        //    dd.Manage("Save_Project_Plan", "20021", dtChunk);
                        //}

                        //if (remainderVal >= 0)
                        //{
                        //    DataTable dtChunk = new DataTable();
                        //    startVal = endVal;

                        //    dtChunk = dt.AsEnumerable().Skip(startVal).Take(remainderVal).CopyToDataTable();
                        //    //dtPlan.Merge(dd.GetDataTable("Save_Project_Plan", "20021", dtChunk));
                        //    dd.Manage("Save_Project_Plan", "20021", dtChunk);
                        //}
                        //if (dtPlan.Rows.Count > 0)
                        //{
                        //    returedData = dtPlan.ToList<PM_ImportProjectPlans>();
                        //    return Json(new { msg = "Data has been imported successfully", key = true, res = returedData }, JsonRequestBehavior.AllowGet);
                        //}
                    }
                    else
                    {
                        return Json(new { key = false, msg = "Please! Choose csv file" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { key = false, msg = "No file selected" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, key = false, res = returedData }, JsonRequestBehavior.AllowGet);
            }
            //return PartialView("~/Areas/Project/Views/ImportProjectPlans/_ProjectPlans.cshtml",model);
            //returedData = dtPlan.ToList<PM_ImportProjectPlans>();

            // return Json(new { msg = "Data has been imported successfully", key = true, res = returedData }, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult UploadProjectPlanCustom(HttpPostedFileBase uploadMasterEx)
        {
            string fullUrl = Request.UrlReferrer.ToString();
            string ProjectId = fullUrl.Split('=').Last();
            dbDataTable ddt = new dbDataTable();
            DataTable FileRecord = ddt.Tb_MasterEx();

            dbDataTable dbdt = new dbDataTable();
            DataTable dtPlan = new DataTable();

            PM_ImportProjectPlanDL dd = new PM_ImportProjectPlanDL();
            // var model = new List<PM_ImportProjectPlans>();

            List<PM_ImportProjectPlans> returedData = new List<PM_ImportProjectPlans>();
            try
            {
                if (uploadMasterEx != null && uploadMasterEx.ContentLength > 0)
                {
                    if (uploadMasterEx.FileName.EndsWith(".csv") || uploadMasterEx.FileName.EndsWith(".CSV"))
                    {
                        Stream stream = uploadMasterEx.InputStream;
                        using (CsvReader csvPlanReader = new CsvReader(new StreamReader(stream), true))
                        {
                            FileRecord.Load(csvPlanReader);

                        }
                        List<PM_ImportProjectPlans> plan = FileRecord.ToImportList<PM_ImportProjectPlans>();

                        DataTable dt = dbdt.PM_ImportLists();
                        foreach (var p in plan)
                        {
                            myDataTable.AddRow(dt,
                                "Value1", p.FACode.Trim(), "Value2", p.USID.Trim(), "Value3", p.CommonID.Trim(), "Value4", p.REGION.Trim(), "Value5", p.MARKET.Trim(),
                                "Value6", p.SUBMarket.Trim(), "Value7", p.SiteName.Trim(),
                                "Value8", p.StreetAddress.Trim(), "Value9", p.CITY.Trim(), "Value10", p.State.Trim(), "Value11", p.ZIP.Trim(),
                                "Value12", p.COUNTY.Trim(), "Value13", p.Latitude.Trim(),
                                "Value14", p.Longitude.Trim(), "Value15", p.vMME.Trim(), "Value16", p.ControlledIntroduction.Trim(), "Value17", p.SuperBowl.Trim(),
                                "Value18", p.SiteType.Trim(), "Value19", p.DASorInbuilding.Trim(),
                                "Value20", p.FirstNetRAN.Trim(), "Value21", p.iPlanJob.Trim(), "Value22", p.iPlanStatus.Trim(),
                                "Value23", p.iPlanIssueDate != "" ? Convert.ToDateTime(p.iPlanIssueDate).ToString("yyyy-MM-dd") : "",
                                "Value24", p.PACENumber.Trim(),
                                "Value25", p.TSSPlan != "" ? Convert.ToDateTime(p.TSSPlan).ToString("yyyy-MM-dd") : "",
                                "Value26", p.TSSForecast != "" ? Convert.ToDateTime(p.TSSForecast).ToString("yyyy-MM-dd") : "",
                                "Value27", p.TSSSubmitted != "" ? Convert.ToDateTime(p.TSSSubmitted).ToString("yyyy-MM-dd") : "",
                                "Value28", p.SiteSpecificMaterialAvailableForecast != "" ? Convert.ToDateTime(p.SiteSpecificMaterialAvailableForecast).ToString("yyyy-MM-dd") : "",
                                "Value29", p.SiteSpecificMaterialAvailableActual != "" ? Convert.ToDateTime(p.SiteSpecificMaterialAvailableActual).ToString("yyyy-MM-dd") : "",
                                "Value30", p.PreInstallPlanned != "" ? Convert.ToDateTime(p.PreInstallPlanned).ToString("yyyy-MM-dd") : "",
                                "Value31", p.PreInstallFcst != "" ? Convert.ToDateTime(p.PreInstallFcst).ToString("yyyy-MM-dd") : "",
                                "Value32", p.PreInstallActual != "" ? Convert.ToDateTime(p.PreInstallActual).ToString("yyyy-MM-dd") : "",
                                "Value33", p.MigDatePlanned != "" ? Convert.ToDateTime(p.MigDatePlanned).ToString("yyyy-MM-dd") : "",
                                "Value34", p.MigDateForecast != "" ? Convert.ToDateTime(p.MigDateForecast).ToString("yyyy-MM-dd") : "",
                                "Value35", p.MigrationDate != "" ? Convert.ToDateTime(p.MigrationDate).ToString("yyyy-MM-dd") : "",
                                "Value36", p.EPLOrdered.Trim(), "Value37", p.EPLCalledOut.Trim(), "Value38", p.EPLDelivered.Trim(), "Value39", p.EPLStatus.Trim()
                                );
                        }


                        var _flag = dd.Manage("Import_ProjectPlan_Data", ProjectId, dt);

                        //DataTable dtTopRow = new DataTable();
                        //dtTopRow = dt.AsEnumerable().Skip(0).Take(1).CopyToDataTable();

                        if (_flag)
                        {
                            dtPlan = dd.GetDataTable("Custom", ProjectId);

                            if (dtPlan.Rows.Count > 0)
                            {
                                returedData = dtPlan.ToList<PM_ImportProjectPlans>();
                                return Json(new { msg = "Data imported successfully", key = true, res = returedData }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { msg = "Data imported successfully", key = true, res = returedData }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { msg = "Error while Import data", key = false }, JsonRequestBehavior.AllowGet);
                        }



                        //int totalRows = dt.Rows.Count;
                        //int subsetSize = 1000;

                        //int loopSize = totalRows / subsetSize;
                        //int remainderVal = totalRows % subsetSize;

                        //int startVal = 1;
                        //int endVal = 0;

                        //for (int i = 1; i <= loopSize; i++)
                        //{
                        //    startVal = endVal;
                        //    endVal = endVal + subsetSize;

                        //    DataTable dtChunk = new DataTable();
                        //    dtChunk = dt.AsEnumerable().Skip(startVal).Take(subsetSize).CopyToDataTable();

                        //    //dtPlan.Merge(dd.GetDataTable("Save_Project_Plan", "20021", dtChunk));
                        //    dd.Manage("Save_Project_Plan", "20021", dtChunk);
                        //}

                        //if (remainderVal >= 0)
                        //{
                        //    DataTable dtChunk = new DataTable();
                        //    startVal = endVal;

                        //    dtChunk = dt.AsEnumerable().Skip(startVal).Take(remainderVal).CopyToDataTable();
                        //    //dtPlan.Merge(dd.GetDataTable("Save_Project_Plan", "20021", dtChunk));
                        //    dd.Manage("Save_Project_Plan", "20021", dtChunk);
                        //}
                        //if (dtPlan.Rows.Count > 0)
                        //{
                        //    returedData = dtPlan.ToList<PM_ImportProjectPlans>();
                        //    return Json(new { msg = "Data has been imported successfully", key = true, res = returedData }, JsonRequestBehavior.AllowGet);
                        //}
                    }
                    else
                    {
                        return Json(new { key = false, msg = "Please! Choose csv file" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { key = false, msg = "No file selected" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Error Occured !", key = false, res = returedData }, JsonRequestBehavior.AllowGet);
            }
            //return PartialView("~/Areas/Project/Views/ImportProjectPlans/_ProjectPlans.cshtml",model);
            //returedData = dtPlan.ToList<PM_ImportProjectPlans>();

            // return Json(new { msg = "Data has been imported successfully", key = true, res = returedData }, JsonRequestBehavior.AllowGet);
        }



        #region Revisions

        [IsLogin(CheckPermission = false)]
        public JsonResult GetRevisions(long ProjectId=0)
        {

            DataTable dtPlan = new DataTable();
            PM_ImportProjectPlanDL dd = new PM_ImportProjectPlanDL();
            
            List<PM_ImportProjectPlans> returedData = new List<PM_ImportProjectPlans>();
            try
            {
                dtPlan =  dd.GetDataTable("Get_By_ProjectId", ProjectId, ViewBag.UserId);
                returedData = dtPlan.ToList<PM_ImportProjectPlans>();
                return Json(new { msg = "Success !", key = true, res = returedData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, key = false, res = returedData }, JsonRequestBehavior.AllowGet);
            }
           
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult GetRevisionsDetails(long ProjectId = 0,long RevisionId=0)
        {


            DataTable _dtPlannew = new DataTable();
            DataTable _dtPlanupdated = new DataTable();
            PM_ImportProjectPlanDL dd = new PM_ImportProjectPlanDL();

            List<PM_ImportProjectPlans> _LatestRevision = new List<PM_ImportProjectPlans>();
            try
            {
                _dtPlannew = dd.GetDataTable("Get_By_RevisionId", ProjectId, ViewBag.UserId,RevisionId);
                _LatestRevision = _dtPlannew.ToList<PM_ImportProjectPlans>();

                _dtPlanupdated = dd.GetDataTable("Get_Revision_AllProperties", ProjectId, ViewBag.UserId, RevisionId);
                List<PM_ImportProjectPlans> _LastRevision = _dtPlanupdated.ToList<PM_ImportProjectPlans>();
                foreach (var item in _LatestRevision)
                {
                    PM_ImportProjectPlans _matchFound = _LastRevision.Where(x => x.Task == item.Task && x.SiteTaskId == item.SiteTaskId && x.Task != "" && x.SiteTaskId > 0).FirstOrDefault();
                    if (_matchFound != null)
                    {
                         _matchFound = _matchFound.EnumeratePropertyDifferences(item);
                     
                    }

                }


                return Json(new { msg = "Success !", key = true, res = _LatestRevision }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, key = false, res = _LatestRevision }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion



        [HttpPost]
        //public ActionResult Update(HttpPostedFileBase file)
        //{
        //    // Verify that the user selected a file
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        var fileName = Path.GetFileName(file.FileName);
        //        // store the file inside ~/App_Data/uploads folder
        //        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
        //        ViewBag.Message = "File Uploaded Successfully";
        //        file.SaveAs(path);
        //    }

        //    //Start the SSIS here
        //    try
        //    {
        //        Application app = new Application();

        //        Package package = null;
        //        package = app.LoadPackage(@"C:\Users\Chris\Documents\Visual Studio 2008\Projects\Integration Services Project1\Integration Services Project1
        //    \bin\Package.dtsx", null);

        //        // Execute Package
        //        DTSExecResult results = package.Execute();

        //        if (results == DTSExecResult.Failure)
        //        {
        //            foreach (DtsError local_DtsError in package.Errors)
        //            {
        //                ViewBag.Message("Package Execution results:{0}",
        //                local_DtsError.Description.ToString());
        //            }
        //        }
        //    }
        //    catch (DtsException ex)
        //    {
        //        //ViewBag.Message("{0} Exception caught.", ex);

        //    }


        //    // redirect back to the index action to show the form once again
        //    return RedirectToAction("Index");
        //}


        [IsLogin(CheckPermission = false)]
        public JsonResult UploadIssuesLog(HttpPostedFileBase uploadWRIssues)
        {
            string fullUrl = Request.UrlReferrer.ToString();
            string ProjectId = fullUrl.Split('=').Last();
            dbDataTable ddt = new dbDataTable();
            DataTable FileRecord = ddt.Tb_ImportWR_Fail();
            dbDataTable dbdt = new dbDataTable();

            DataTable dtPlan = new DataTable();
            try
            {
                if (uploadWRIssues != null && uploadWRIssues.ContentLength > 0 && ProjectId != "")
                {
                    if (uploadWRIssues.FileName.EndsWith(".csv") || uploadWRIssues.FileName.EndsWith(".CSV"))
                    {
                        Stream stream = uploadWRIssues.InputStream;
                        using (CsvReader csvPlanReader = new CsvReader(new StreamReader(stream), true))
                        {
                            FileRecord.Load(csvPlanReader);

                        }
                        List<MP_Import_WR_Fail> plan = FileRecord.ToImportList<MP_Import_WR_Fail>();
                        DataTable dt = dbdt.PM_ImportLists();
                        DataTable dt2 = dbdt.PM_ImportLists();
                        var Errors = "";
                        List<string> ErrorList = new List<string>();
                        foreach (var p in plan)
                        {
                            if (string.IsNullOrEmpty(p.FACode)==false && string.IsNullOrEmpty(p.Task) == false && string.IsNullOrEmpty(p.Category) == false && string.IsNullOrEmpty(p.Type) == false && string.IsNullOrEmpty(p.WhoFix) == false && string.IsNullOrEmpty(p.Severity) == false && string.IsNullOrEmpty(p.Description) == false && string.IsNullOrEmpty(p.Status) == false && string.IsNullOrEmpty(p.AssignedTo) == false && string.IsNullOrEmpty(p.Priority) == false && string.IsNullOrEmpty(p.TargetDate) == false && string.IsNullOrEmpty(p.RequestDate) == false && string.IsNullOrEmpty(p.RequestedBy) == false && string.IsNullOrEmpty(p.CreatedBy) == false && string.IsNullOrEmpty(p.CreateDate )== false)
                            {
                                myDataTable.AddRow(dt,

                                    "Value1", p.eNB
                                   , "Value2", p.Task
                                   , "Value3", p.FACode
                                   , "Value4", p.ExtendedENB
                                   , "Value5", p.EquipmentId
                                   , "Value6", p.AOTSCR
                                   , "Value7", p.Category
                                   , "Value8", p.Type
                                   , "Value9", p.WhoFix
                                   , "Value10", p.IsUnavoidable
                                   , "Value11", p.ActivityType
                                   , "Value12", p.Alarms
                                   , "Value13", p.Severity
                                   , "Value14", p.MW
                                   , "Value15", p.AttachmentType
                                   , "Value16", p.Attachment
                                   , "Value17", p.Description
                                   , "Value18", p.Status
                                   , "Value19", p.AssignedTo
                                   , "Value20", p.Priority
                                   , "Value21", p.ScheduleDate
                                   , "Value22", p.ActualDate
                                   , "Value23", p.TargetDate
                                   , "Value24", p.RequestedBy
                                   , "Value25", p.RequestDate
                                   , "Value26", p.CreateDate
                                   , "Value27", p.CreatedBy
                                  );
                            }
                            else
                            {
                                if(string.IsNullOrEmpty(p.FACode) == true)
                                {
                                    Errors = "FA Code";
                                }if (string.IsNullOrEmpty(p.Task) ==true)
                                {
                                    Errors = Errors+ ", Task";
                                }
                                if (string.IsNullOrEmpty(p.Category) == true)
                                {
                                    Errors = Errors + ", Category";
                                }
                               if (string.IsNullOrEmpty(p.Type) == true)
                                {
                                    Errors = Errors + ", Type";
                                }
                                if (string.IsNullOrEmpty(p.WhoFix) == true)
                                {
                                    Errors = Errors + ", WhoFix";
                                }
                             if (string.IsNullOrEmpty(p.Severity) == true)
                                {
                                    Errors = Errors + ", Severity";
                                }
                                if (string.IsNullOrEmpty(p.Description) == true)
                                {
                                    Errors = Errors + ", Description";
                                }
                                if (string.IsNullOrEmpty(p.Status) == true)
                                {
                                    Errors = Errors + ", Status";
                                }
                                 if (string.IsNullOrEmpty(p.AssignedTo) == true)
                                {
                                    Errors = Errors + ", AssignedTo";
                                }
                                if (string.IsNullOrEmpty(p.Priority) == true)
                                {
                                    Errors = Errors + ", Priority";
                                }
                                 if (string.IsNullOrEmpty(p.TargetDate) == true)
                                {
                                    Errors = Errors + ", TargetDate";
                                }
                                if (string.IsNullOrEmpty(p.RequestDate) == true)
                                {
                                    Errors = Errors + ", RequestDate";
                                }
                                if (string.IsNullOrEmpty(p.RequestedBy) == true)
                                {
                                    Errors = Errors + ", RequestedBy";
                                }
                               if (string.IsNullOrEmpty(p.CreatedBy) == true)
                                {
                                    Errors = Errors + ", CreatedBy";
                                }
                                 if (string.IsNullOrEmpty(p.CreateDate) == true)
                                {
                                    Errors = Errors + ", CreateDate";
                                }
                                Errors = Errors + " are required.";
                                myDataTable.AddRow(dt2,
                                  "Value1", p.eNB
                                 , "Value2", p.Task
                                 , "Value3", p.FACode
                                 , "Value4", p.ExtendedENB
                                 , "Value5", p.EquipmentId
                                 , "Value6", p.AOTSCR
                                 , "Value7", p.Category
                                 , "Value8", p.Type
                                 , "Value9", p.WhoFix
                                 , "Value10", p.IsUnavoidable
                                 , "Value11", p.ActivityType
                                 , "Value12", p.Alarms
                                 , "Value13", p.Severity
                                 , "Value14", p.MW
                                 , "Value15", p.AttachmentType
                                 , "Value16", p.Attachment
                                 , "Value17", p.Description
                                 , "Value18", p.Status
                                 , "Value19", p.AssignedTo
                                 , "Value20", p.Priority
                                 , "Value21", p.ScheduleDate
                                 , "Value22", p.ActualDate
                                 , "Value23", p.TargetDate
                                 , "Value24", p.RequestedBy
                                 , "Value25", p.RequestDate
                                 , "Value26", p.CreateDate
                                 , "Value27", p.CreatedBy
                                );
                                //dt.Columns.Remove("ActualCarrier");
                                ErrorList.Add(Errors);
                                Errors = "";
                            }
                    }
                       if(dt2.Rows.Count >0)
                        {
                            using (var package = new ExcelPackage())
                            {
                                string handle = Guid.NewGuid().ToString();
                                MemoryStream memStream;
                                var Where = "Errors in Issues Log ";
                                //  FileName = dt.Rows[0][4] + "_" + dt.Rows[0][2] + "_" + dt.Rows[0][3] + "_" + dt.Rows[0][5] + "_" + DateTime.Now.ToString();
                                var FileName = Where + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                                var Sheet1 = package.Workbook.Worksheets.Add(Where.ToString());
                                //for (int t = 0; t < temp.Rows.Count; t++)
                                //{
                                //string TestType = (temp.Columns.Contains("SiteType")) ? temp.Rows[t]["SiteType"].ToString() : "";


                                //dt.DefaultView.RowFilter = "SiteType='" + TestType + "'";
                                DataTable CCW = dt2;
                                // CCW.Columns.Remove("SiteType");

                                //if (t==0)
                                //{ 
                                // Sheet1 = package.Workbook.Worksheets.Add("ProjectSummary");
                                //}
                                int x = FileRecord.Columns.Count;

                                for (int i = 0; i < FileRecord.Columns.Count; i++)
                                {
                                    
                                    Sheet1.Cells[1, i + 1].Value = FileRecord.Columns[i].ColumnName;

                                    if (CCW.Columns[i].DataType == typeof(System.DateTime))
                                    {
                                        Sheet1.Column(i + 1).Style.Numberformat.Format = "mm/dd/yyyy HH:mm";
                                    }
                                    if (i+1 == x){
                                        Sheet1.Cells[1, i + 2].Value ="Errors";
                                    }
                                }
                               
                                for (int i = 0; i < CCW.Rows.Count; i++)
                                {
                                    int y = 26;
                                    for (int j = 0; j < 26; j++)
                                    {
                                        if (CCW.Rows[i][j].ToString() != "2" && CCW.Rows[i][j].ToString() != "1/1/1900 12:00:00 AM")
                                        {
                                            Sheet1.Cells[i + 2, j + 1].Value = CCW.Rows[i][j];
                                        }
                                        if (j+1 == y)
                                        {
                                            var value2 = ErrorList[i];
                                            int index1 = value2.IndexOf(',');
                                            if (index1 != -1)
                                            {
                                                ErrorList[i] = value2.Remove(index1, 1);
                                            }
                                            Sheet1.Cells[i + 2, j + 3].Value = ErrorList[i];
                                        }
                                    }
                                }

                                //}

                                memStream = new MemoryStream(package.GetAsByteArray());
                                TempData[handle] = memStream.ToArray();
                                return new JsonResult()
                                {
                                    Data = new { FileGuid = handle, FileName = "ErrorIssuesLogs.xlsx" }
                                };

                            }

                            

                        }
                        //eNB FACode othereNB   SiteName   CC  
                        //Market  Schedule    Actual  MW  Status  Alarm
                        //Issues  WhoFix Notes   ContentType    AppCreatedBy 
                        //AppModifiedBy Attachments WorkflowInstanceID
                        //FileType   PMO Modified

                        //if (pbl.Manage("Import_WR_Issues", ProjectId, dt))
                        //{
                        //    return Json(new { msg = "WR Issues Uploaded Successfully!", key = true, }, JsonRequestBehavior.AllowGet);
                        //}
                        //else
                        //{
                        //    return Json(new { msg = "Error to import data", key = false }, JsonRequestBehavior.AllowGet);
                        //}
                        pbl.Manage("Import_WR_Issues", ProjectId, dt);
                        return Json(new { msg = "WR Issues Uploaded Successfully!", key = true, }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { key = false, msg = "Please! Choose csv file" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { key = false, msg = "No file selected" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { key = false, msg = ex.Message, JsonRequestBehavior.AllowGet });
            }
            //return Json(new { dtPlan = dtPlan }, JsonRequestBehavior.AllowGet);
            //return Json(new { msg = "WR Issues Successfully Uploaded!", key = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult UploadSiteLog(HttpPostedFileBase uploadWRSiteLog)
        {
            string fullUrl = Request.UrlReferrer.ToString();
            string ProjectId = fullUrl.Split('=').Last();
            dbDataTable ddt = new dbDataTable();
            DataTable FileRecord = ddt.Tb_ImportWR_Ex();
            dbDataTable dbdt = new dbDataTable();

            DataTable dtPlan = new DataTable();
            try
            {
                if (uploadWRSiteLog != null && uploadWRSiteLog.ContentLength > 0 && ProjectId != "")
                {
                    if (uploadWRSiteLog.FileName.EndsWith(".csv") || uploadWRSiteLog.FileName.EndsWith(".CSV"))
                    {
                        Stream stream = uploadWRSiteLog.InputStream;
                        using (CsvReader csvPlanReader = new CsvReader(new StreamReader(stream), true))
                        {
                            FileRecord.Load(csvPlanReader);

                        }
                        List<MP_Import_WR_Ex> plans = FileRecord.ToImportList<MP_Import_WR_Ex>();
                        DataTable dt = dbdt.PM_ImportLists();
                        DataTable dt2 = dbdt.PM_ImportLists();
                        string Errors = "";
                        List<string> ErrorList = new List<string>();
                        foreach (var p in plans)
                        {
                            if (string.IsNullOrEmpty(p.FACode) == false && string.IsNullOrEmpty(p.ActivityType) == false && string.IsNullOrEmpty(p.GNG) == false && string.IsNullOrEmpty(p.Scheduled) == false && string.IsNullOrEmpty(p.Notes) == false && string.IsNullOrEmpty(p.Status) == false && string.IsNullOrEmpty(p.IsAdditional) == false && string.IsNullOrEmpty(p.Created) == false && string.IsNullOrEmpty(p.CreatedBy) == false)
                            {
                                myDataTable.AddRow(dt,
                                 "Value1", p.FACode.Trim(), "Value2", p.ActivityType.Trim(), "Value3", p.Alarms.Trim(), "Value4", p.GNG.Trim(), "Value5", p.Scheduled.Trim(),
                                 "Value6", p.Attachment.Trim(), "Value7", p.MW.Trim(),
                                 "Value8", p.AttachmentType.Trim(), "Value9", p.Notes.Trim(), "Value10", p.eNB.Trim(), "Value11", p.ExtendedENB.Trim(),
                                 "Value12", p.EquipmentId.Trim(), "Value13", p.AOTSCR.Trim(),
                                 "Value14", p.USID.Trim(), "Value15", p.Status.Trim(), "Value16", p.IsAdditional.Trim(), "Value17", p.Created.Trim(),
                                 "Value18", p.CreatedBy.Trim());
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(p.FACode) == true)
                                {
                                    Errors = "FA Code";
                                }
                                if (string.IsNullOrEmpty(p.ActivityType) == true)
                                {
                                    Errors = Errors + ", ActivityType";
                                }
                                if (string.IsNullOrEmpty(p.GNG) == true)
                                {
                                    Errors = Errors + ", GNG";
                                }
                                if (string.IsNullOrEmpty(p.Scheduled) == true)
                                {
                                    Errors = Errors + ", Scheduled";
                                }
                                if (string.IsNullOrEmpty(p.Notes) == true)
                                {
                                    Errors = Errors + ", Notes";
                                }
                                if (string.IsNullOrEmpty(p.Status) == true)
                                {
                                    Errors = Errors + ", Status";
                                }
                                if (string.IsNullOrEmpty(p.IsAdditional) == true)
                                {
                                    Errors = Errors + ", IsAdditional";
                                }
                                if (string.IsNullOrEmpty(p.Created) == true)
                                {
                                    Errors = Errors + ", Created";
                                }
                                if (string.IsNullOrEmpty(p.CreatedBy) == true)
                                {
                                    Errors = Errors + ", CreatedBy";
                                }
                                Errors = Errors + " are required.";
                                myDataTable.AddRow(dt2,
                                 "Value1", p.FACode.Trim(), "Value2", p.ActivityType.Trim(), "Value3", p.Alarms.Trim(), "Value4", p.GNG.Trim(), "Value5", p.Scheduled.Trim(),
                                 "Value6", p.Attachment.Trim(), "Value7", p.MW.Trim(),
                                 "Value8", p.AttachmentType.Trim(), "Value9", p.Notes.Trim(), "Value10", p.eNB.Trim(), "Value11", p.ExtendedENB.Trim(),
                                 "Value12", p.EquipmentId.Trim(), "Value13", p.AOTSCR.Trim(),
                                 "Value14", p.USID.Trim(), "Value15", p.Status.Trim(), "Value16", p.IsAdditional.Trim(), "Value17", p.Created.Trim(),
                                 "Value18", p.CreatedBy.Trim());
                                ErrorList.Add(Errors);
                                Errors = "";
                            }
                        }
                        if (dt2.Rows.Count > 0)
                        {
                           // pbl.Manage("Import_WR_Site", "40024", dt);
                            using (var package = new ExcelPackage())
                            {
                                string handle = Guid.NewGuid().ToString();
                                MemoryStream memStream;
                                var Where = "Errors in Issues Log ";
                                //  FileName = dt.Rows[0][4] + "_" + dt.Rows[0][2] + "_" + dt.Rows[0][3] + "_" + dt.Rows[0][5] + "_" + DateTime.Now.ToString();
                                var FileName = Where + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                                var Sheet1 = package.Workbook.Worksheets.Add(Where.ToString());
                                //for (int t = 0; t < temp.Rows.Count; t++)
                                //{
                                //string TestType = (temp.Columns.Contains("SiteType")) ? temp.Rows[t]["SiteType"].ToString() : "";


                                //dt.DefaultView.RowFilter = "SiteType='" + TestType + "'";
                                DataTable CCW = dt2;
                                // CCW.Columns.Remove("SiteType");

                                //if (t==0)
                                //{ 
                                // Sheet1 = package.Workbook.Worksheets.Add("ProjectSummary");
                                //}
                                int x = FileRecord.Columns.Count;

                                for (int i = 0; i < FileRecord.Columns.Count; i++)
                                {

                                    Sheet1.Cells[1, i + 1].Value = FileRecord.Columns[i].ColumnName;

                                    if (CCW.Columns[i].DataType == typeof(System.DateTime))
                                    {
                                        Sheet1.Column(i + 1).Style.Numberformat.Format = "mm/dd/yyyy HH:mm";
                                    }
                                    if (i + 1 == x)
                                    {
                                        Sheet1.Cells[1, i + 2].Value = "Errors";
                                    }
                                }

                                for (int i = 0; i < CCW.Rows.Count; i++)
                                {
                                    int y = 18;
                                    for (int j = 0; j < 18; j++)
                                    {
                                        if (CCW.Rows[i][j].ToString() != "2" && CCW.Rows[i][j].ToString() != "1/1/1900 12:00:00 AM")
                                        {
                                            Sheet1.Cells[i + 2, j + 1].Value = CCW.Rows[i][j];
                                        }
                                        if (j + 1 == y)
                                        {
                                            var value2 = ErrorList[i];
                                            int index1 = value2.IndexOf(',');
                                            if (index1 != -1)
                                            {
                                                ErrorList[i] = value2.Remove(index1, 1);
                                            }
                                            Sheet1.Cells[i + 2, j + 3].Value = ErrorList[i];
                                        }
                                    }
                                }

                                //}

                                memStream = new MemoryStream(package.GetAsByteArray());
                                TempData[handle] = memStream.ToArray();
                                return new JsonResult()
                                {
                                    Data = new { FileGuid = handle, FileName = "ErrorSiteLogs.xlsx" }
                                };

                            }



                        }
                        //FACode eNB othereNB   SiteName   CC  Market
                        //Schedule    Actual  MW  Status  Alarm
                        //Issues  WhoFix Notes   AddlNotes  NetAct
                        //Effort  Migrated    TicketRequest  TechName   TechNumber
                        //TechEmail USID    ExpirationDate Notification    ContentType
                        //AppCreatedBy  AppModifiedBy WorkflowInstanceID    FileType   ModifiedOn
                        //PMO Created CreatedBy

                        if (pbl.Manage("Import_WR_Site", ProjectId, dt))
                        {
                            return Json(new { msg = "Site Logs Uploaded Successfully!", key = true }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { msg = "Error to import data", key = false }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { key = false, msg = "Please! Choose csv file" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { key = false, msg = "No file selected" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { key = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new { msg = "Site Logs Successfully Uploaded!", key = true }, JsonRequestBehavior.AllowGet);
        }

    }
}