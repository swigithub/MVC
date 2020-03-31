using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using SWI.AirView.Models;
using SWI.Libraries.Common;
using SWI.Security.Filters;
using System.Web.UI.WebControls;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.DAL;
using System.Collections.Generic;
using LumenWorks.Framework.IO.Csv;
using AirView.DBLayer.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.AD.BLL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.AirView.DAL;
using Library.SWI.Survey.BLL;
using SWI.Libraries.Security.DAL;
using WebApplication.Models;

namespace SWI.AirView.Controllers
{
    [IsLogin, ErrorHandling]
    public class WorkOrderController : Controller
    {
        // GET: WorkOrder
        public ActionResult Index()
        {
            return View();
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult List(string value1, string value2, string value3)
        {
            WorkOrderBL wob = new WorkOrderBL();
            var dt = wob.Search(value1, value2, value3);
            return PartialView("~/Views/WorkOrder/_List.cshtml", dt);
        }

        [IsLogin(CheckPermission = false),HttpGet]
        public JsonResult CheckSiteCode(string filter, string value1)
        {
            Response res = new Response();
            WorkOrderDL woDL = new WorkOrderDL();
            //List<string> rec = sb.ToList("By_NetLayer_SiteCode", SiteCode, NetworkModeId, BandId, CarrierId, ScopeId).Select(x => x.SiteCode).ToList();

            int count = woDL.SiteCodeExist(filter, value1);
            if (count > 0)
            {
                return Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Error" }, JsonRequestBehavior.AllowGet);
        }





        [IsLogin(CheckPermission = false)]
        public void ExportWorkOrderStatus(DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                var grid = new GridView();
                AV_NetLayerReportExportBL rptNtlExp = new AV_NetLayerReportExportBL();
                Int64 UserId = ViewBag.UserId;
                var rec = rptNtlExp.Get(null, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo), "Total", null, null, null, null, "WorkOrderStatus", UserId);
                grid.DataSource = rec.Select(m => new { m.WO_Ref, m.Client, m.Region, m.Market, m.Site, m.Drive_Tester, m.Received, m.Submitted, m.Scheduled, m.Drive_Completed, m.ReportSubmittedOn, m.Approved, m.Network_Layers, m.Status, m.Description, m.SectorCount, m.LayerCount, m.Distance_from_Site, m.DT_Minutes });
                grid.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                string FileName = "WorkOrderStatus_" + DateTime.Now;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception)
            { }


        }

        [IsLogin(CheckPermission = false)]
        public void NetLayerStatusExport(DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                var grid = new GridView();
                AV_NetLayerReportExportBL rptNtlExp = new AV_NetLayerReportExportBL();
                Int64 UserId = ViewBag.UserId;
                var rec = rptNtlExp.Get(null, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo), "Total", null, null, null, null, "NetLayerStatus", UserId);
                grid.DataSource = rec.Select(m => new { m.WO_Ref, m.Client, m.Region, m.Market, m.Site, m.Drive_Tester, m.Received, m.Submitted, m.Scheduled, m.Drive_Completed, m.ReportSubmittedOn, m.Approved, m.Network_Layers, m.Status, m.Description, m.SectorCount, m.LayerCount, m.Distance_from_Site, m.DT_Minutes });
                grid.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                string FileName = "WorkOrderStatus_" + DateTime.Now;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception)
            { }


        }

        public ActionResult New(int Id = 0)
        {
            AD_DefinationBL db = new AD_DefinationBL();
            Sec_UserSettingsDL udl = new Sec_UserSettingsDL();

            ViewBag.FormType = (TempData["FormType"] != null) ? "Edit" : "New";

            ViewBag.Bands = db.ToList("Bands");

            ViewBag.Id = Id;

            ViewBag.Carriers = db.ToList("Carriers");

            ViewBag.Regions = db.ToList("UserRegions", Convert.ToString(ViewBag.UserId));
            DataTable Table = udl.GetDataTable("UserProjects", Convert.ToString(ViewBag.UserId), null, null);
            ViewBag.Projects = Table.ToList<PM_Projects>();
            ViewBag.Cities = db.ToList("UserCities", Convert.ToString(ViewBag.UserId));
            ViewBag.SubCheckList = db.ToList("byDefinationType", "Sub CheckList");

            Common.SelectedList sl = new Common.SelectedList();
            ViewBag.NetworkModes = db.SelectedList("NetworkModes", null, "-NetworkMode-");
            var hjh = db.MultiSelecet("NetworkModes", null, "-NetworkMode-");
            ViewBag.NetworkModesm = hjh;
            ViewBag.States = sl.Definations("UserStates", Convert.ToString(ViewBag.UserId), "-State-"); //sl.States();
            TSS_SurveyDocumentBL sdb = new TSS_SurveyDocumentBL();
            ViewBag.Surveys = sdb.ToList("GetAll_byIsActive", true.ToString(),false,0,0,null,ViewBag.UserId); ;
            ViewBag.Scopes = sl.Definations("UserScopes", Convert.ToString(ViewBag.UserId));// sl.Scopes();
            ViewBag.Clients = sl.Clients("UserClients", Convert.ToString(ViewBag.UserId));
            ViewBag.SiteTypes = sl.Definations("SiteTypes");
            ViewBag.SiteClasses = sl.Definations("SiteClasses");
            ViewBag.CheckList = sl.Definations("byDefinationType", "CheckList", "-CheckList-");
            ViewBag.Sectors = sl.Sectors();
            return View();
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult Workorder(Int64 Id = 0)
        {
            AD_DefinationBL db = new AD_DefinationBL();
            Sec_UserSettingsDL udl = new Sec_UserSettingsDL();

            ViewBag.FormType = (TempData["FormType"] != null) ? "Edit" : "New";

            ViewBag.Bands = db.ToList("Bands");

            ViewBag.Id = Id;

            ViewBag.Carriers = db.ToList("Carriers");


            DataTable Table = udl.GetDataTable("UserProjects", Convert.ToString(ViewBag.UserId), null, null);
            ViewBag.Projects = Table.ToList<PM_Projects>();
            ViewBag.Cities = db.ToList("UserCities", Convert.ToString(ViewBag.UserId));
            ViewBag.SubCheckList = db.ToList("byDefinationType", "Sub CheckList");

            Common.SelectedList sl = new Common.SelectedList();
            ViewBag.NetworkModes = db.SelectedList("NetworkModes", null, "-NetworkMode-");
            var hjh = db.MultiSelecet("NetworkModes", null, "-NetworkMode-");
            ViewBag.NetworkModesm = hjh;
            ViewBag.States = sl.Definations("UserStates", Convert.ToString(ViewBag.UserId), "-State-"); //sl.States();
            ViewBag.Regions = db.ToList("UserRegions", Convert.ToString(ViewBag.UserId));
            TSS_SurveyDocumentBL sdb = new TSS_SurveyDocumentBL();
            ViewBag.Surveys = sdb.ToList("GetAll_byIsActive", true.ToString()); ;
            ViewBag.Scopes = sl.Definations("UserScopes", Convert.ToString(ViewBag.UserId));// sl.Scopes();
            ViewBag.Clients = sl.Clients("UserClients", Convert.ToString(ViewBag.UserId));
            ViewBag.SiteTypes = sl.Definations("SiteTypes");
            ViewBag.SiteClasses = sl.Definations("SiteClasses");
            ViewBag.CheckList = sl.Definations("byDefinationType", "CheckList", "-CheckList-");
            ViewBag.Sectors = sl.Sectors();
            WorkorderEdit we = new WorkorderEdit();
            return View("Workorder", we);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(Workorder wo, List<Workorder> wolst, List<AV_TSSCheckList> tss, long SiteTypeIds = 0, long SiteClassIds = 0, long ProjectIds = 0 , string Clonetype = "")
        {
            Response res = new Response();  
            try
            {
                if (Clonetype == "Clone")
                {
                    wo.SiteTypeId = SiteTypeIds.ToString();
                    wo.SiteClassId = SiteClassIds;
                    wo.ProjectId = ProjectIds;
                }

                if (wo.SiteId != 0 )
                {
                    wo.SiteTypeId = SiteTypeIds.ToString();
                    wo.SiteClassId = SiteClassIds;
                    wo.ProjectId = ProjectIds;
                }
                ClientsBL ub = new ClientsBL();
                var ClientPrefix = ub.ToList("AllRecords").Where(x => x.ClientId == Convert.ToDecimal(wo.Client)).FirstOrDefault().ClientPrefix;
                wolst[0].ClientPrefix = ClientPrefix;
               Common.SelectedList sl = new Common.SelectedList();
               var obj = sl.Definations("UserScopes", Convert.ToString(ViewBag.UserId));// sl.Scopes();

                TempData["clusters"] = wolst;
                TempData["SiteCluster"] = wo.siteCode + "," + wo.clusterCode + "," + ClientPrefix;
                foreach (var df in obj)
                {
                    if (wo.Scope == df.Value && df.Text == "TSS" || wo.Scope == df.Value && df.Text == "CLS")
                    {
                        foreach (var item in wolst)
                        {
                            item.VerticalBeamWidth = "0";
                            item.RFHeight = 0;
                            item.MTilt = 0;
                            item.ETilt = 0;
                            item.BandWidth = "0";
                            item.SectorLatitude = 0;
                            item.SectorLongitude = 0;
                            item.CellId = "0";
                            item.MRBTS = "0";

                        }
                    }
                }
                WorkOrderBL wb = new WorkOrderBL();
                if (wolst.Count > 0 && wolst[0].clusterId != null)
                {
                    var fname = wo.siteCode + "," + wo.clusterCode;
                    if (wo.SiteId == 0)
                    {
                        wb.Insert("NewWorkOrder", wo, wolst, ViewBag.UserId, null, fname);
                    }
                    else
                    {
                        wb.ChangeFolderName(wo, wolst);
                        wb.Insert("Edit_Work_Order", wo, wolst, ViewBag.UserId, null, fname);
                    }
                }
                else
                {
                    if (wo.SiteId == 0)
                    {
                        wb.Insert("NewWorkOrder", wo, wolst, ViewBag.UserId, null);
                    }
                    else
                    {
            
                        wb.ChangeFolderName(wo,wolst);
                        wb.Insert("Edit_Work_Order", wo, wolst, ViewBag.UserId, null);
                    }
                }
                res.Status = "success";
                res.Message = "save successfully";





            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(new { response = res }, JsonRequestBehavior.AllowGet);

        }
        [IsLogin(CheckPermission = false)]
        public ActionResult NewFiles()
        {
            Response res = new Response();
            var wolt = TempData["clusters"] as List<Workorder>;
            var wo = TempData["SiteCluster"].ToString();
            var wo2 = wo.Split(',');
            try
            {
                HttpFileCollectionBase files = HttpContext.Request.Files;
                var wolst = HttpContext.Request["mydata"];
                for (int i = 0; i < files.Count; i++)
                {

                    #region File Save

                    string Path = "/Content/AirViewLogs/" + wo2[2] + "/" + wolt[i].clusterId;
                    HttpPostedFileBase file = HttpContext.Request.Files[i] as HttpPostedFileBase;
                    if (!Directory.Exists(HttpContext.Server.MapPath("~" + Path)))
                    {
                        // if it doesn't exist, create
                        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~" + Path));
                    }
                    string fname = HttpContext.Server.MapPath("~" + Path + "/" + wolt[i].clusterName + "_" + wolt[i].networkmodename);
                    HttpContext.Request.Files[i].SaveAs(fname + ".csv");



                    #endregion



                    #region site count
                    List<CSVParser> values = System.IO.File.ReadAllLines(fname + ".csv")
                                          .Skip(1)
                                          .Select(v => CSVParser.FromCsv(v)).Distinct()
                                          .ToList();
                    List<string> a = new List<string>();
                    foreach (var item in values)
                    {
                        a.Add(item.siteCode);
                    }
                    //  a = new List<string>();
                    //// Int64 SiteCount = 0;
                    // Stream stream = files[i].InputStream;
                    // DataTable sitesTable = new DataTable();
                    // using (CsvReader csvReader =
                    //     new CsvReader(new StreamReader(stream), true))
                    // {
                    //     sitesTable.Load(csvReader);
                    // }
                    // DataView view = new DataView(sitesTable);
                    // DataTable tblsectors = view.ToTable(true, new string[] { "clusterCode", "siteCode" });
                    // if (tblsectors.Rows.Count > 0)
                    // {
                    //     for (int j = 0; j < tblsectors.Rows.Count; j++)
                    //     {
                    //         DataRow crow = tblsectors.Rows[j];
                    //         string abc = crow["siteCode"].ToString();
                    //         a.Add(abc);
                    //     }
                    //     a = a.Distinct().ToList();
                    // }
                    WorkOrderBL wb = new WorkOrderBL();
                    wb.Insert("InsertSiteCount", a.Distinct().ToList().Count, Path + "/" + wolt[i].clusterName + "_" + wolt[i].networkmodename + ".csv");

                    #endregion

                }
                res.Status = "success";
                res.Message = "save successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;

            }



            return RedirectToAction("New");
        }
        public ActionResult CloneNew(int Id = 0)
        {
            ViewBag.FormType = "Edit";
            AV_SitesBL sb = new AV_SitesBL();
            AV_Site Site = new AV_Site();
            List<AV_Sector> Sectors = new List<AV_Sector>();
            sb.SiteWithSectors(Id, ref Site, ref Sectors);
            ViewBag.Site = Site;
            Common.SelectedList sl = new Common.SelectedList();
            AD_DefinationBL db = new AD_DefinationBL();
            ViewBag.NetworkModes = db.SelectedList("NetworkModes", null, "-NetworkMode-");
            ViewBag.Bands = db.ToList("Bands");
            ViewBag.Carriers = db.ToList("Carriers");
            ViewBag.Scopes = sl.Scopes();
            ViewBag.Sectors = sl.Sectors();
            List<SWI.Libraries.Common.SelectedList> Markets = new List<SelectedList>();
            if (ViewBag.IsAdmin)
            {
                ViewBag.UserMarkets = db.SelectedList("AllCities");
                Markets = ViewBag.UserMarkets;
            }
            else
            {
                ViewBag.UserMarkets = db.SelectedList("UserCities", Convert.ToString(ViewBag.UserId));
                Markets = ViewBag.UserMarkets;
            }


            //bands network mode etc dropdowns
            WorkorderEdit we = new WorkorderEdit();
            Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
            we.Site = Site;
            we.Site.StateId = we.Site.State;
            we.Site.RegionId = we.Site.Region;
            we.Site.MarketId = we.Site.Market;
            we.Site.SiteType = we.Site.SiteTypeId;
            we.Site.SiteClass = we.Site.SiteClassId;
            we.Bands = ViewBag.Bands;
            ViewBag.Id = Id;

            we.Carriers = ViewBag.Carriers;

            DataTable Table = udl.GetDataTable("UserProjects", Convert.ToString(ViewBag.UserId), null, null);
            we.Projects = Table.ToList<PM_Projects>();
            we.Cities = db.ToList("UserCities", Convert.ToString(ViewBag.UserId));
            we.SubCheckList = db.ToList("byDefinationType", "Sub CheckList");

            we.NetworkModes = ViewBag.NetworkModes;
            var hjh = db.MultiSelecet("NetworkModes", null, "-NetworkMode-");
            we.NetworkModesm = hjh;
            we.States = sl.Definations("UserStates", Convert.ToString(ViewBag.UserId), "-State-");  //sl.States();
            we.Regions = db.ToList("UserRegions", Convert.ToString(ViewBag.UserId));
            // we.Regions = we.Regions.Where(x => x.PDefinationId ==we.State).ToList();
            we.UserMarkets = Markets;
            //   we.UserMarkets = we.UserMarkets;    //.Where(x=>x.Value==Convert.ToString(we.Region)).ToList();
            TSS_SurveyDocumentBL sdb = new TSS_SurveyDocumentBL();
            we.Surveys = sdb.ToList("GetAll_byIsActive", true.ToString());
            we.Clients = sl.Clients("UserClients", Convert.ToString(ViewBag.UserId));
            we.Scopes = sl.Definations("UserScopes", Convert.ToString(ViewBag.UserId));// sl.Scopes();
            we.SiteTypes = sl.Definations("SiteTypes");
            we.SiteClasses = sl.Definations("SiteClasses");
            we.CheckList = sl.Definations("byDefinationType", "CheckList", "-CheckList-");
            we.Sectors = Sectors;
            we.Sectors[0].Bands = we.Bands;
            we.Sectors[0].Carriers = we.Carriers;

            return View("CloneNew", we); // return View(Sectors);
        }
        public ActionResult Edit(int Id = 0)
        {
            ViewBag.FormType = "Edit";
            AV_SitesBL sb = new AV_SitesBL();
            AV_Site Site = new AV_Site();
            List<AV_Sector> Sectors = new List<AV_Sector>();
            sb.SiteWithSectors(Id, ref Site, ref Sectors);
            ViewBag.Site = Site;
            Common.SelectedList sl = new Common.SelectedList();
            AD_DefinationBL db = new AD_DefinationBL();
            ViewBag.NetworkModes = db.SelectedList("NetworkModes", null, "-NetworkMode-");
            ViewBag.Bands = db.ToList("Bands");
            ViewBag.Carriers = db.ToList("Carriers");
            ViewBag.Scopes = sl.Scopes();
            ViewBag.Sectors = sl.Sectors();
            List<SWI.Libraries.Common.SelectedList> Markets = new List<SelectedList>();
            if (ViewBag.IsAdmin)
            {
                ViewBag.UserMarkets = db.SelectedList("AllCities");
                Markets = ViewBag.UserMarkets;
            }
            else
            {
                ViewBag.UserMarkets = db.SelectedList("UserCities", Convert.ToString(ViewBag.UserId));
                Markets = ViewBag.UserMarkets;
            }


            //bands network mode etc dropdowns
            WorkorderEdit we = new WorkorderEdit();
            Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
            we.Site = Site;
            we.Site.StateId = we.Site.State;
            we.Site.RegionId = we.Site.Region;
            we.Site.MarketId = we.Site.Market;
            we.Site.SiteType = we.Site.SiteTypeId;
            we.Site.SiteClass = we.Site.SiteClassId;
            we.Bands = ViewBag.Bands;
            ViewBag.Id = Id;

            we.Carriers = ViewBag.Carriers;

            DataTable Table = udl.GetDataTable("UserProjects", Convert.ToString(ViewBag.UserId), null, null);
            we.Projects = Table.ToList<PM_Projects>();
            we.Cities = db.ToList("UserCities", Convert.ToString(ViewBag.UserId));
            we.SubCheckList = db.ToList("byDefinationType", "Sub CheckList");

            we.NetworkModes = ViewBag.NetworkModes;
            var hjh = db.MultiSelecet("NetworkModes", null, "-NetworkMode-");
            we.NetworkModesm = hjh;
            we.States = sl.Definations("UserStates", Convert.ToString(ViewBag.UserId), "-State-");  //sl.States();
            we.Regions = db.ToList("UserRegions", Convert.ToString(ViewBag.UserId));
            // we.Regions = we.Regions.Where(x => x.PDefinationId ==we.State).ToList();
            we.UserMarkets = Markets;
            //   we.UserMarkets = we.UserMarkets;    //.Where(x=>x.Value==Convert.ToString(we.Region)).ToList();
            TSS_SurveyDocumentBL sdb = new TSS_SurveyDocumentBL();
            we.Surveys = sdb.ToList("GetAll_byIsActive", true.ToString());
            we.Clients = sl.Clients("UserClients", Convert.ToString(ViewBag.UserId));
            we.Scopes = sl.Definations("UserScopes", Convert.ToString(ViewBag.UserId));// sl.Scopes();
            we.SiteTypes = sl.Definations("SiteTypes");
            we.SiteClasses = sl.Definations("SiteClasses");
            we.CheckList = sl.Definations("byDefinationType", "CheckList", "-CheckList-");
            we.Sectors = Sectors;
            we.Sectors[0].Bands = we.Bands;
            we.Sectors[0].Carriers = we.Carriers;

            return View("Workorder", we); // return View(Sectors);
        }
        [HttpPost]
        public ActionResult Edit(AV_Site Site, Workorder wo, List<Workorder> Sectors)
        {
            //Workorder Site, List<AV_Sector> Sectors
            Response res = new Response();
            try
            {
                DataTable Sectorsdt = new DataTable();
                //  Sectorsdt = Sectors.ToDataTable();
                Sectorsdt.Columns.Remove("TestStatus");
                Sectorsdt.Columns.Remove("Latitude");
                Sectorsdt.Columns.Remove("Longitude");
                Sectorsdt.Columns.Remove("isActive");
                Sectorsdt.Columns.Remove("sectorColor");
                WorkOrderDL dl = new WorkOrderDL();
                //  dl.Edit(Site, Sectorsdt);
                res.Status = "success";
                res.Message = "save successfully";
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
            DataTable FileRecord = ddt.Tb_AV_Workorder();

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

                        WorkOrderDL wdl = new WorkOrderDL();
                        wdl.Insert("MarketSites", FileRecord, ViewBag.UserId, null);
                        TempData["msg_success"] = "Save successfully.";
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



            return RedirectToAction("New");
        }

        public ActionResult UniqueMarketSiteCodes()
        {
            Common.SelectedList sl = new Common.SelectedList();

            AD_DefinationBL db = new AD_DefinationBL();
            if (ViewBag.IsAdmin)
            {
                ViewBag.ddCities = db.SelectedList("AllCities");
            }
            else
            {
                ViewBag.ddCities = db.SelectedList("UserCities", Convert.ToString(ViewBag.UserId));
            }
            //ViewBag.ddCities = sl.Cities(true);
            return PartialView("~/views/WorkOrder/_UniqueMarketSiteCodes.cshtml");
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult UniqueMarketSiteCodes(string Filter, string value)
        {
            AV_MarketSitesBL msb = new AV_MarketSitesBL();

            if (Filter == "UniqueSiteCode")
            {
                var rec = msb.ToList(Filter, 0, "", 0, 0, Convert.ToInt64(value));

                return Json(rec, JsonRequestBehavior.AllowGet);
            }
            else if (Filter == "AllbySiteCode")
            {
                var rec = msb.ToList(Filter, 0, value, 0, 0, 0);
                var json = Json(rec, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }

            return null;

        }




        [IsLogin(CheckPermission = false)]
        public JsonResult SiteExistOrNot(string SiteCode, string NetworkModeId, string BandId, string CarrierId, string ScopeId)

        {
            int i = 0;
            AV_SitesBL sb = new AV_SitesBL();
            List<string> rec = sb.ToList("By_NetLayer_SiteCode", SiteCode, NetworkModeId, BandId, CarrierId, ScopeId).Select(x=>x.SiteCode).ToList();      
            foreach(var item in rec)
            {
                if(item == SiteCode)
                {
                    i = 1;
                }
            }    
            if(i == 0)
            {
                rec.Clear();
            }
            return Json(rec,JsonRequestBehavior.AllowGet);
        }


        [IsLogin(CheckPermission = false)]
        public JsonResult SiteWithSectors(Int64 Id)
        {
            WorkOrderBL wb = new WorkOrderBL();
            var rec = wb.GetWO(Id.ToString());

            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        #region Getting TSS_survey_document by client & market
        public JsonResult GetSurvey(int Id)
        {
            try
            {

                AD_SurveyBL wb = new AD_SurveyBL();
                var rec = wb.GetSurvey();

                return Json(rec, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }


        #endregion



        //[IsLogin(CheckPermission = false, Return = "")]
        //public bool UpdateActiveStatus(Int32 SiteId)
        //{
        //    try
        //    {
        //        AV_Site s = new AV_Site();
        //        AV_SitesBL sbl = new AV_SitesBL();

        //        s.SiteId = SiteId;
        //        s.IsActive = false;
        //        sbl.Manage("UpdateStatus", s);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

    }
}