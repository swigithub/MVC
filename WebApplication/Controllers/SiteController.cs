using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.AirView.BLL;
using Newtonsoft.Json;
using SWI.Security.Filters;
using AirView.DBLayer.AirView.BLL;
using SWI.Libraries.Common;
using SWI.AirView.Models;
using SWI.Libraries.AirView.DAL;
using AirView.DBLayer.AirView.DAL;
using SWI.Libraries.Security.Entities;
using SWI.AirView.Common;
using System.Threading;
using SWI.Libraries.AD.BLL;
using Library.SWI.Survey.BLL;
using Library.SWI.Survey.Model;
using Library.SWI.Survey.DAL;
using AirView.DBLayer.Survey.BLL;
using AirView.DBLayer.Fleet.BLL;
using AirView.DBLayer.Fleet.Model;
using AirView.DBLayer.Template.BLL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Survey.Model;
using System.Text;
using AirView.DBLayer.AirView.Entities;
//using AirView.DBLayer.Survey.BLL;

namespace SWI.AirView.Controllers
{
    [IsLogin, ErrorHandling]
    public class SiteController : Controller
    {

        private DashboardBL loDashboardBL = new DashboardBL();
        private SitesBL loSitesBL = new SitesBL();
        // GET: Site

        //[HttpPost, IsLogin(Return = "login")]
        //public ActionResult Upload(HttpPostedFileBase upload)
        //{

        //    try
        //    {
        //        if (upload != null && upload.ContentLength > 0)
        //        {

        //            if (upload.FileName.EndsWith(".csv"))
        //            {
        //                Stream stream = upload.InputStream;
        //                DataTable sitesTable = new DataTable();
        //                using (CsvReader csvReader =
        //                    new CsvReader(new StreamReader(stream), true))
        //                {
        //                    sitesTable.Load(csvReader);
        //                }

        //                List<NetworkMode> lstNetworkModes = loSitesBL.GetAllNetworkModesBandsCarriers();
        //                AD_DefinationBL db = new AD_DefinationBL();
        //                AV_SectorDL secd = new AV_SectorDL();
        //                var scopes = db.ToList("Scopes");

        //                DataView view = new DataView(sitesTable);

        //                DataTable tblsectors = view.ToTable(true, new string[] { "clusterCode", "Region", "Market", "Client" });

        //                if (tblsectors.Rows.Count > 0)
        //                {
        //                    for (int j = 0; j < tblsectors.Rows.Count; j++)
        //                    {
        //                        DataRow crow = tblsectors.Rows[j];
        //                        //Save Sector IN Database
        //                        Int64 ClustorId = loSitesBL.SaveClustorAndGetId(crow["clusterCode"].ToString(), crow["Region"].ToString(), crow["Market"].ToString(), crow["Client"].ToString());
        //                        if (ClustorId > 0)
        //                        {

        //                            // DataTable sites = view.ToTable(true, new string[] { "siteCode", "siteLatitude", "siteLongitude", "Description" });
        //                            DataRow[] Sitesrows = sitesTable.Select("clusterCode = '" + crow["clusterCode"] + "'");

        //                            if (Sitesrows.Length > 0)
        //                            {
        //                                for (int i = 0; i < Sitesrows.Length; i++)
        //                                {
        //                                    DataRow dr = Sitesrows[i];
        //                                    //Insert in db and get id
        //                                    Int64 User = ViewBag.UserId;
        //                                    Int64 SiteId = loSitesBL.SaveSiteAndGetId("insert", "", dr["siteCode"].ToString(), Convert.ToDouble(dr["siteLatitude"]), Convert.ToDouble(dr["siteLongitude"]), ClustorId, dr["Client"].ToString(), dr["Description"].ToString(), "PENDING_SCHEDULED", ViewBag.UserId, crow["Market"].ToString(), Convert.ToDateTime(dr["ReceivedOn"].ToString()), dr["Scope"].ToString(), null);
        //                                    if (SiteId > 0)
        //                                    {
        //                                        //Int64 siteid = 0;
        //                                        DataRow[] rows = sitesTable.Select("siteCode = '" + dr["siteCode"] + "'");
        //                                        DataTable sectors = new DataTable();
        //                                        sectors.Columns.AddRange(new DataColumn[12]
        //                                        {
        //                                            new DataColumn("SectorCode", typeof (string)),
        //                                            new DataColumn("NetworkModeId", typeof (Int64)),
        //                                            new DataColumn("ScopeId", typeof (Int64)),
        //                                            new DataColumn("BandId", typeof (Int64)),
        //                                            new DataColumn("CarrierId", typeof (Int64)),
        //                                            new DataColumn("Antenna", typeof (string)),
        //                                            new DataColumn("BeamWidth", typeof (double)),
        //                                            new DataColumn("Azimuth", typeof (double)),
        //                                            new DataColumn("PCI", typeof (int)),
        //                                            new DataColumn("SiteId", typeof (Int64)),
        //                                            new DataColumn("Client", typeof (string)),
        //                                            new DataColumn("City", typeof (string))
        //                                        });
        //                                        DataRow r;
        //                                        Int64 ScopeId = scopes.Where(m => m.DefinationName == dr["Scope"].ToString()).FirstOrDefault().DefinationId;
        //                                        foreach (DataRow row in rows)
        //                                        {
        //                                            //List<NetworkMode> nm = lstNetworkModes.Where(x => x.NetworkModeName == row["networkMode"].ToString()).ToList();
        //                                            //var nm2 = nm[0].Bands.Where(x => x.BandName == row["Band"].ToString()).ToList();
        //                                            r = sectors.NewRow();
        //                                            r["SectorCode"] = row["sectorCode"].ToString();
        //                                            r["NetworkModeId"] = lstNetworkModes.Where(x => x.NetworkModeName == row["networkMode"].ToString()).ToList()[0].NetworkModeId;
        //                                            r["ScopeId"] = ScopeId;
        //                                            r["BandId"] = lstNetworkModes.Where(x => x.NetworkModeName == row["networkMode"].ToString()).ToList()[0].Bands.Where(x => x.BandName == row["Band"].ToString()).ToList()[0].BandId;
        //                                            r["CarrierId"] = lstNetworkModes.Where(x => x.NetworkModeName == row["networkMode"].ToString()).ToList()[0].Bands[0].Carriers.Where(m => m.CarrierName == row["Carrier"].ToString()).FirstOrDefault().CarrierId;
        //                                            r["Antenna"] = row["Antenna"].ToString();
        //                                            r["BeamWidth"] = Convert.ToDouble(row["Beamwidth"]);
        //                                            r["Azimuth"] = Convert.ToDouble(row["Azimuth"]);
        //                                            r["PCI"] = Convert.ToInt32(row["PCI"]);
        //                                            r["SiteId"] = SiteId;
        //                                            r["Client"] = row["Client"].ToString(); ;
        //                                            r["City"] = row["Market"].ToString(); ;

        //                                            sectors.Rows.Add(r);
        //                                        }
        //                                        //Insert sectors dataTable in DB
        //                                        bool flag = secd.Manage(sectors, "", "", "");
        //                                    }
        //                                    else
        //                                    {
        //                                        TempData["msg_error"] = "An error occurred while processing your request, please try again.";
        //                                        return RedirectToAction("Index", "Dashboard");
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            TempData["msg_error"] = "An error occurred while processing your request, please try again.";
        //                            return RedirectToAction("Index", "Dashboard");
        //                        }
        //                    }
        //                }

        //                TempData["msg_success"] = "Sites Uploaded Successfully.";
        //                return RedirectToAction("Index", "Dashboard");
        //            }
        //            else
        //            {
        //                TempData["msg_error"] = "Please Upload File with .csv extention.";
        //                return RedirectToAction("Index", "Dashboard");
        //            }
        //        }
        //        else
        //        {
        //            TempData["msg_error"] = "Please select File with valid sites data.";
        //            return RedirectToAction("Index", "Dashboard");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["msg_error"] = "An error occurred while processing your request, please try again. " + ex.Message;
        //        return RedirectToAction("Index", "Dashboard");
        //    }
        //}

        [HttpPost, IsLogin(Return = "login")]
        public ActionResult Upload(HttpPostedFileBase upload, string WOType)
        {

            try
            {
                if (upload != null && upload.ContentLength > 0)
                {

                    if (upload.FileName.EndsWith(".csv"))
                    {

                        Stream stream = upload.InputStream;
                        DataTable sitesTable = new DataTable();
                        using (CsvReader csvReader =
                        new CsvReader(new StreamReader(stream), true))
                        {
                            sitesTable.Load(csvReader);
                        }

                        dbDataTable ddt = new dbDataTable();
                        DataTable wodt = ddt.Tb_AV_Workorder();
                        if (WOType != "Audit Template")
                        {
                            #region Add Rows

                            foreach (DataRow r in sitesTable.Rows)
                            {
                                DataRow row = wodt.NewRow();
                                row["clusterCode"] = r["clusterCode"].ToString();  //wo.clusterCode;
                                row["Region"] = r["Region"].ToString();
                                row["Market"] = r["Market"].ToString();
                                row["Client"] = r["Client"].ToString();
                                row["siteCode"] = r["siteCode"].ToString();
                                row["SiteName"] = r["SiteName"].ToString();
                                row["Project"] = r["Project"].ToString();
                                row["SiteType"] = r["SiteType"].ToString();
                                row["SiteClass"] = r["SiteClass"].ToString();
                               

                                // CellId
                                row["CellId"] = r["CellId"].ToString();
                                row["siteLatitude"] = r["siteLatitude"].ToString();
                                row["siteLongitude"] = r["siteLongitude"].ToString();
                                row["Description"] = r["Description"].ToString();
                                row["SiteAddress"] = r["Address"].ToString();
                                row["sectorCode"] = r["sectorCode"].ToString();

                                row["networkMode"] = r["networkMode"].ToString();
                                row["Scope"] = r["Scope"].ToString();
                                row["Band"] = r["Band"].ToString();
                                row["Carrier"] = r["Carrier"].ToString();
                                row["Antenna"] = r["Antenna"].ToString();
                                //RFHeight
                                row["RFHeight"] = r["RFHeight"].ToString();

                                row["BeamWidth"] = r["BeamWidth"].ToString();
                                row["VerticalBeamWidth"] = r["VBeamWidth"];
                                row["Mtilt"] = r["AntennaDowntilt"].ToString();
                                //row["Etilt"] = r["Etilt"].ToString();
                                row["Azimuth"] = r["Azimuth"].ToString();
                                row["PCI"] = r["PCI"].ToString();
                                row["ReceivedOn"] = r["ReceivedOn"];
                                wodt.Rows.Add(row);
                            }
                            #endregion
                        }
                        else
                        {

                            #region Add Rows For TSS

                            List<TSS_WorkOrderCSV> list = sitesTable.ToList<TSS_WorkOrderCSV>();
                            var WorkOrders = list.Select(x => new { x.SiteType, x.siteLongitude, x.siteLatitude, x.siteCode, x.SiteClass, x.Scope, x.Region, x.ReceivedOn, x.Project, x.Market, x.Description, x.clusterCode, x.Client, x.Address, x.Checklist,x.SiteName }).Distinct().ToList();
                            List<TSS_WorkOrderCSV> AllWorkOrders = new List<TSS_WorkOrderCSV>();
                            foreach (var s in WorkOrders)
                            {
                                string SurveyCode = string.Join(",", list.Where(x => x.siteCode == s.siteCode).Select(p => p.SurveyCode).ToList());
                                DataRow row = wodt.NewRow();
                                row["SiteAddress"] = s.Address.ToString();
                                row["Project"] = s.Project.ToString();
                                row["ReceivedOn"] = s.ReceivedOn.ToString();
                                row["Scope"] = s.Scope.ToString();
                                row["SiteClass"] = s.SiteClass.ToString();
                                row["clusterCode"] = s.clusterCode.ToString();
                                row["Region"] = s.Region.ToString();
                                row["Market"] = s.Market.ToString();
                                row["Client"] = s.Client.ToString();
                                row["siteCode"] = s.siteCode.ToString();
                                row["SiteType"] = s.SiteType.ToString();
                                row["siteLatitude"] = s.siteLatitude.ToString();
                                row["siteLongitude"] = s.siteLongitude.ToString();
                                row["Description"] = s.Description.ToString();
                                row["Checklist"] = SurveyCode.ToString();
                                row["SiteName"] = s.SiteName.ToString();
                                wodt.Rows.Add(row);
                            }
                            
                            #endregion
                        }
                        WorkOrderDL wod = new WorkOrderDL();

                        if (wod.Insert("File_WO", wodt, ViewBag.UserId, ""))
                        {
                            TempData["msg_success"] = "Work Order uploaded successfully!";
                        }
                        else
                        {
                            TempData["msg_error"] = "Error occured during work order upload.";
                        }

                    }

                }
                else
                {
                    TempData["msg_error"] = "Please select File with valid sites data.";

                }
            }
            catch (Exception ex)
            {
                TempData["msg_error"] = "An error occurred while processing your request, please try again. " + ex.Message;
            }

            return RedirectToAction("Index", "Dashboard");
        }


        [HttpPost, IsLogin(Return = "login")]
        public ActionResult UploadSiteLogs(IEnumerable<HttpPostedFileBase> Files)
        {

            try
            {
                foreach (var file in Files)
                {
                    if (file.ContentLength > 0)
                    {

                        if (file.FileName.EndsWith(".csv"))
                        {
                            Stream stream = file.InputStream;
                            DataTable sitesTable = new DataTable();
                            using (CsvReader csvReader =
                                new CsvReader(new StreamReader(stream), true))
                            {
                                sitesTable.Load(csvReader);
                            }

                            // File name must be like Stationary_UL_A2A0444D_Alpha_225_2016-12-23_003738099_Passed

                            string[] rec = file.FileName.Split('_');
                            string TestType = null;

                            if (rec.Count() > 0)
                            {
                                TestType = rec[1];
                                bool Status = false;

                                if (rec[0] == "Mobility")
                                {
                                    Status = true;
                                }
                                else
                                {
                                    if (rec[rec.Length - 1].Remove(6) == "Passed")
                                    {
                                        Status = true;
                                    }
                                }
                                AV_SiteTestLogBL stlb = new AV_SiteTestLogBL();
                                stlb.SaveFromDT(sitesTable, TestType, Status);
                            }
                            else
                            {
                                TempData["msg_error"] = "you can't change file name.";
                                return RedirectToAction("Index", "Dashboard");
                            }




                            TempData["msg_success"] = "Site Logs Uploaded Successfully.";
                        }
                        else
                        {
                            TempData["msg_error"] = "Please Upload File with .csv extention.";
                            return RedirectToAction("Index", "Dashboard");
                        }
                    }
                    else
                    {
                        TempData["msg_error"] = "Please select File with valid sites data.";
                        return RedirectToAction("Index", "Dashboard");
                    }

                }

                return RedirectToAction("Index", "Dashboard");


            }
            catch (Exception ex)
            {
                TempData["msg_error"] = ex.Message;
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpPost, IsLogin(CheckPermission = false, Return = "")]
        public void GetSectors(Int64 BandId, Int64 SiteId, Int64 NetworkModId, Int64 ScopeId, Int64 CarrierId)
        {
            Response.Clear();
            Response.ClearContent();


            List<SectorsVM> lstSectors = loSitesBL.GetSectors("GetSectors", SiteId, NetworkModId, BandId, CarrierId, ScopeId);
            var sr = JsonConvert.SerializeObject(lstSectors);

            Response.ContentType = "json";
            Response.Write(sr);
        }


        [HttpPost]
        public ActionResult ChangeStatus(string SiteId, string Status, string comment)
        {
            Response res = new Response();
            try
            {
                AV_SitesDL sd = new AV_SitesDL();
                sd.Manage("UpdateStatus", SiteId, Status, comment, null, null, ViewBag.UserId);
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


        [IsLogin(Return = "login")]
        public void ExportSiteToCSV(int id = 0)
        {

            StringWriter sw = new StringWriter();
            WorkOrderBL wob = new WorkOrderBL();
            var data = wob.Export(id.ToString());

            ObjectProperties op = new ObjectProperties();
            GetWorkExport we = new GetWorkExport();
            string Columns = null;
            var ColumnsObj = op.PropertiesName(we).ToList();
            foreach (string item in ColumnsObj)
            {
                Columns = Columns + item + ",";
            }


            sw.WriteLine(Columns);

            string csvHeader = null;
            for (int i = 0; i < ColumnsObj.Count; i++)
            {
                csvHeader = csvHeader + "{" + i + "},";
            }

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=WorkOrder.csv");
            Response.ContentType = "text/csv"; //application/vnd.ms-excel
            // Response.ContentType = "/application/vnd.ms-excel"; 

            foreach (var item in data)
            {
                sw.WriteLine(string.Format(csvHeader,
                    item.clusterCode, item.Region, item.Market, item.Client, item.siteCode, item.SiteName, item.Project, item.SiteType, item.SiteClass, item.CellId, item.siteLatitude, item.siteLongitude, item.Description, item.Address, item.sectorCode, item.networkMode,
                    item.Scope, item.Band, item.Carrier, item.Antenna, item.RFHeight, item.BeamWidth, item.VBeamWidth, item.AntennaDowntilt, item.Azimuth, item.PCI, item.ReceivedOn
                    ));
            }
            Response.Write(sw.ToString());
            Response.End();

        }

        #region Get bands


        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult GetSiteBands(string id, string scope, string RequestFrom = "")
        {
            ViewBag.SiteId = id;
            try
            {
                
                    ViewBag.RequestFrom = RequestFrom;
                if (scope == "SSV" || scope == "NI" || scope == "IND")
                {

                    AV_MarketConfigurationBL bl = new AV_MarketConfigurationBL();
                    Int64 SiteId = Convert.ToInt64(id);
                    List<BandVM> lstBands = loSitesBL.GetSiteBands("", SiteId);
                    bool IsReport = bl.CheckForReportTemplate(Convert.ToInt32(id));
                    ViewBag.IsReport = IsReport;
                    return PartialView("~/Views/Dashboard/_SiteGridSSV.cshtml", lstBands);
                }
                else if (scope == "TSS")
                {
                    TSS_VMBL vmb = new TSS_VMBL();
                    var TSS = vmb.ToList("", id);
                    return PartialView("~/Views/Dashboard/_SiteGridTSS.cshtml", TSS);
                }
                else if (scope == "CLS")
                {

                    AV_MarketConfigurationBL bl = new AV_MarketConfigurationBL();
                    CLS_VMBL vmb = new CLS_VMBL();
                    var CLS = vmb.ToList("", id);
                    bool IsReport = bl.CheckForReportTemplate(Convert.ToInt32(id));
                    ViewBag.IsReport = IsReport;
                    return PartialView("~/Views/Dashboard/_SiteGridCLS.cshtml", CLS);
                }
                else
                {
                    return Content("No Scope");
                }
                //Int64 SiteId = Convert.ToInt64(id);
                ////var abc = BandsFactory.GetVM(scope);
                //List<BandVM> lstBands = loSitesBL.GetSiteBands("", SiteId);

                //lstBands[0].SiteId = (int)SiteId;
                //return PartialView(scope, lstBands);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }


        #endregion

        [IsLogin(CheckPermission = false)]
        //public ActionResult SiteSchedule(Int64 id, Int64 TesterId)
        public ActionResult SiteSchedule(Int64 SiteId, Int64 TesterId, int UserId, string Scope, int? SiteClusterId = 0)
        {

            //var rec = loSitesBL.GetSiteDataSingle(id);
            //if (rec.Status != 90.ToString())
            //{
            //    return Content("Already schedule");
            //}
            //Common.SelectedList sl = new Common.SelectedList();
            //ViewBag.UserDevices = sl.UserDevices(TesterId);
            //ViewBag.TestTypes = sl.Definations("byDefinationType", "Test Types", "-Select Test Type-");
            //ViewBag.TesterId = TesterId;

            //List<BandVM> lstBands = loSitesBL.GetSiteBands("", id);
            //ViewBag.SiteNetworkLayers = lstBands;

            //AV_WoDevicesBL wdb = new AV_WoDevicesBL();
            //ViewBag.SelectedLayer = wdb.ToList("BySiteId", id, 0, 0, 0, 0, TesterId);

            //if (lstBands.Count > 0)
            //{
            //    var FirstRow = lstBands.FirstOrDefault();
            //    AV_ScopeTestsBL stb = new AV_ScopeTestsBL();
            //    ViewBag.ScopeTests = stb.ToList("byClientId_CityId_ScopeId", rec.ClientId, rec.CityId, 0, FirstRow.ScopeId);


            //}
            //return PartialView("~/views/site/_SiteSchedule.cshtml", rec);
            try
            {
                /* == Check Fleet Management Permissions == */
                FM_VehicleBL model = new FM_VehicleBL();
                int UserRoleId = Convert.ToInt32(ViewBag.RoleID);
                FM_Permissions objectPermission = model.Check_Vehicle_Permission("FleetManagementPermission", UserRoleId);
                if (objectPermission.PermissionId > 0)
                {
                    ViewBag.FleetPermission = "True";
                }
                else
                {
                    ViewBag.FleetPermission = "False";
                }

                var rec = loSitesBL.GetSiteDataSingle(SiteId);
                List<BandVM> lstBands = loSitesBL.GetSiteBands("", SiteId);
                if (rec != null)
                {
                    AV_ScopeTestsBL stb = new AV_ScopeTestsBL();
                    ViewBag.ScopeTests = stb.ToList("byClientId_CityId_ScopeId", rec.ClientId, rec.CityId, 0, rec.ScopeId);

                    ViewBag.SiteNetworkLayers = lstBands;
                    ViewBag.Scope = rec.Scope;
                    Common.SelectedList sl = new Common.SelectedList();
                    List<SelectListItem> Users = new List<SelectListItem>();
                    List<Sec_UserDevices> UserDevices = new List<Sec_UserDevices>();
                    sl.UserAssinged_Testers_Devices(UserId, ref Users, ref UserDevices);
                    ViewBag.Users = Users;
                    ViewBag.UserDevices = UserDevices;

                    AV_WoDevicesBL wdb = new AV_WoDevicesBL();
                    ViewBag.SelectedLayer = wdb.ToList("BySiteId", SiteId, 0, 0, 0, 0, 0);
                    ViewBag.Devices = new List<SelectListItem> { new SelectListItem { Text="Select Device", Value="0" },
                };

                    ViewBag.TestTypes = sl.Definations("byDefinationType", "Test Types", "-Select Test Type-");
                }
                if (rec.Scope == "CLS")
                {
                    ViewBag.BandId = lstBands[0].BandId;
                    ViewBag.CarrierId = lstBands[0].CarrierId;
                    ViewBag.NetworkModeId = lstBands[0].NetworkModeId;
                    CLS_VMBL abc = new CLS_VMBL();
                    ViewBag.CLS = abc.ToListSectors("GetSectorsCLSSchedule", SiteId, (int)SiteClusterId);
                    ViewBag.ParentTester = abc.ToListSectors("ParentTester", SiteId, 0);
                    return PartialView("~/views/site/_GridScheduleCLS.cshtml", rec);
                }
                ViewBag.TesterId = TesterId;
                return PartialView("~/views/site/_GridSchedule.cshtml", rec);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GridSchedule(Int64 SiteId, int UserId, string Scope, int? SiteClusterId = 0)
        {
            try
            {
                /* == Check Fleet Management Permissions == */
                FM_VehicleBL model = new FM_VehicleBL();
                int UserRoleId = Convert.ToInt32(ViewBag.RoleID);
                FM_Permissions objectPermission = model.Check_Vehicle_Permission("FleetManagementPermission", UserRoleId);
                if (objectPermission.PermissionId > 0)
                {
                    ViewBag.FleetPermission = "True";
                }
                else
                {
                    ViewBag.FleetPermission = "False";
                }

                var rec = loSitesBL.GetSiteDataSingle(SiteId);
                List<BandVM> lstBands = loSitesBL.GetSiteBands("", SiteId);
                if (rec != null)
                {
                    AV_ScopeTestsBL stb = new AV_ScopeTestsBL();
                    ViewBag.ScopeTests = stb.ToList("byClientId_CityId_ScopeId", rec.ClientId, rec.CityId, 0, rec.ScopeId);

                    ViewBag.SiteNetworkLayers = lstBands;
                    ViewBag.Scope = Scope;
                    Common.SelectedList sl = new Common.SelectedList();
                    List<SelectListItem> Users = new List<SelectListItem>();
                    List<Sec_UserDevices> UserDevices = new List<Sec_UserDevices>();
                    sl.UserAssinged_Testers_Devices(UserId, ref Users, ref UserDevices);
                    ViewBag.Users = Users;
                    ViewBag.UserDevices = UserDevices;

                    AV_WoDevicesBL wdb = new AV_WoDevicesBL();
                    ViewBag.SelectedLayer = wdb.ToList("BySiteId", SiteId, 0, 0, 0, 0, 0);
                    ViewBag.Devices = new List<SelectListItem> { new SelectListItem { Text="Select Device", Value="0" },
                };

                    ViewBag.TestTypes = sl.Definations("byDefinationType", "Test Types", "-Select Test Type-");
                }
                if (Scope == "CLS")
                {
                    ViewBag.BandId = lstBands[0].BandId;
                    ViewBag.CarrierId = lstBands[0].CarrierId;
                    ViewBag.NetworkModeId = lstBands[0].NetworkModeId;
                    CLS_VMBL abc = new CLS_VMBL();
                    ViewBag.CLS = abc.ToListSectors("GetSectorsCLSSchedule", SiteId, (int)SiteClusterId);
                    ViewBag.ParentTester = abc.ToListSectors("ParentTester", SiteId, 0);
                    return PartialView("~/views/site/_GridScheduleCLS.cshtml", rec);
                }

                return PartialView("~/views/site/_GridSchedule.cshtml", rec);
            }
            catch (Exception ex)
            {

                return null;
            }


        }
        /*
        [HttpPost]
        public JsonResult SiteSchedule(int SiteId, int[] TesterId, string Date, int[] NetworkMode, int[] Band, int[] Carrier, int[] Devices, string[] TestTypes, string[] Layer, string Scope, int[] SequenceId, int[] NetLayerId, int[] DeviceScheduleId, bool[] IsMaster)
        {
            Response res = new Response();
            try
            {
                bool flag = false;
                if (Layer != null)
                {
                    for (int i = 0; i < NetworkMode.Length; i++)
                    {



                        string value = NetworkMode[i] + "_" + Band[i] + "_" + Carrier[i];
                        int pos = Array.IndexOf(Layer, value);
                        if (Scope == "CLS")
                        {
                            TestTypes[i] = "";
                            if (Devices[i] != 0)
                                flag = loSitesBL.AssignTesterCLS(Convert.ToInt64(SiteId), Convert.ToInt64(TesterId[0]), Convert.ToInt64(ViewBag.UserId), Convert.ToDateTime(Date), "91", NetworkMode[i], Band[i], Carrier[i], Devices[i], TestTypes[i], SequenceId[i], NetLayerId[i], DeviceScheduleId[i], IsMaster[i]);
                        }
                        else
                        {
                            if (pos > -1)
                            {
                                flag = loSitesBL.AssignTester(Convert.ToInt64(SiteId), Convert.ToInt64(TesterId[i]), Convert.ToInt64(ViewBag.UserId), Convert.ToDateTime(Date), "91", NetworkMode[i], Band[i], Carrier[i], Devices[i], TestTypes[i], null);

                            }
                        }

                    }

                    DirectoryHandler dh = new DirectoryHandler();

                    if (Scope == "TSS")
                    {
                        TSS_VMBL vmb = new TSS_VMBL();
                        TSS_SectionBL secb = new TSS_SectionBL();

                        var TSS = vmb.ToList("", SiteId.ToString());
                        foreach (var item in TSS)
                        {
                            var Sections = secb.ToList("By_SurveyId", item.SurveyId.ToString());
                            foreach (var sec in Sections)
                            {
                                string folder = item.ClientPrefix + "/" + item.SiteCode + "/" + item.SurveyTitle + "/" + sec.SectionTitle;
                                dh.CreateDirectory(Server.MapPath("~/Content/AirViewLogs/" + folder));
                            }

                        }
                    }
                    else
                    {
                        AV_WoDevicesDL wodl = new AV_WoDevicesDL();
                        AV_WoDevicesBL wdb = new AV_WoDevicesBL();
                        var rec = loSitesBL.GetSectors("GetSiteNetworkLayers", SiteId);
                        if (rec.Count > 0)
                        {
                            foreach (var item in rec)
                            {
                                string site = item.ClientPrefix + "/" + item.SiteCode + "/" + item.NetworkMode.DefinationName + "_" + item.Band.DefinationName + "_" + item.Carrier.DefinationName;
                                dh.CreateDirectory(Server.MapPath("~/Content/AirViewLogs/" + site));
                            }
                        }
                        else
                        {
                            ExceptionHandling.ErrorLogs(ViewBag.RequestUrl, "Site layers not found");
                        }

                    }
                    res.Status = "success";
                    res.Message = "success";

                }
                else
                {
                    res.Status = "danger";
                    res.Message = "Select Layer.";
                }




            }
            catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = ex.Message;
                ExceptionHandling.ErrorLogs(ViewBag.RequestUrl, ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        */
        /* == IF Vehcile Module is Active == */
        [HttpPost]
        public JsonResult SiteSchedule(int SiteId, int[] TesterId, string Date, int[] NetworkMode, int[] Band, int[] Carrier, int[] Devices, string[] TestTypes, string[] Layer, string Scope, int[] SequenceId, int[] NetLayerId, int[] DeviceScheduleId, bool[] IsMaster, int[] Vehicles, long[] LayerStatusId = null)
        {
            FM_VehicleBL model = new FM_VehicleBL();
            int UserRoleId = Convert.ToInt32(ViewBag.RoleID);
            FM_Permissions objectPermission = model.Check_Vehicle_Permission("FleetManagementPermission", UserRoleId);
            Response res = new Response();
           

            try
            {
                bool flag = false;
                if (Layer != null)
                {
                    for (int i = 0; i < NetworkMode.Length; i++)
                    {
                        // Find and Remove Tester id =0
                        var TesterList = TesterId.ToList();
                        if (TesterList.Count > NetworkMode.Length)
                        {
                            TesterList.RemoveAt(0);
                        }
                        TesterId = TesterList.ToArray();
                        //int index = Array.IndexOf(TesterId, 0);
                        //if (index > -1)
                        //{
                        //    TesterList.RemoveAt(index);
                        //}
                        //TesterId = TesterList.ToArray();
                        //
                        // Find and Remove Device id =0
                        var DeviceList = Devices.ToList();
                        if (DeviceList.Count > NetworkMode.Length)
                        {
                            DeviceList.RemoveAt(0);
                        }
                        Devices = DeviceList.ToArray();
                        //int Dind = Array.IndexOf(Devices, 0);
                        //if (Dind > -1)
                        //{
                        //    DeviceList.RemoveAt(Dind);
                        //}
                        //Devices = DeviceList.ToArray();
                        //




                        string value = NetworkMode[i] + "_" + Band[i] + "_" + Carrier[i];
                        int pos = Array.IndexOf(Layer, value);
                        if (Scope == "CLS")
                        {
                            TestTypes[i] = "";
                            if (Devices[i] != 0)
                            {
                                if(objectPermission.PermissionId > 0 )
                                {
                                    if (Vehicles == null)
                                    {
                                        flag = loSitesBL.AssignTesterCLS(0, Convert.ToInt64(SiteId), Convert.ToInt64(TesterId[0]), Convert.ToInt64(ViewBag.UserId), Convert.ToDateTime(Date), "91", NetworkMode[i], Band[i], Carrier[i], Devices[i], TestTypes[i], SequenceId[i], NetLayerId[i], DeviceScheduleId[i], IsMaster[i]);
                                    }
                                    else
                                    {
                                        flag = loSitesBL.AssignTesterCLS(Vehicles[i], Convert.ToInt64(SiteId), Convert.ToInt64(TesterId[0]), Convert.ToInt64(ViewBag.UserId), Convert.ToDateTime(Date), "91", NetworkMode[i], Band[i], Carrier[i], Devices[i], TestTypes[i], SequenceId[i], NetLayerId[i], DeviceScheduleId[i], IsMaster[i]);
                                    }
                        
                                        
                           
                                    
                                }

                                else
                                {
                                    flag = loSitesBL.AssignTesterCLS(Convert.ToInt64(SiteId), Convert.ToInt64(TesterId[0]), Convert.ToInt64(ViewBag.UserId), Convert.ToDateTime(Date), "91", NetworkMode[i], Band[i], Carrier[i], Devices[i], TestTypes[i], SequenceId[i], NetLayerId[i], DeviceScheduleId[i], IsMaster[i]);
                                }

                            }
                        }
                        else
                        {
                            if (pos > -1)
                            {
                                if (objectPermission.PermissionId > 0)
                                {
                                    flag = loSitesBL.AssignTester(Vehicles[i], Convert.ToInt64(SiteId), Convert.ToInt64(TesterId[i]), Convert.ToInt64(ViewBag.UserId), Convert.ToDateTime(Date), "91", NetworkMode[i], Band[i], Carrier[i], Devices[i], TestTypes[i], null, LayerStatusId[i]);
                                }
                                else
                                {
                                    flag = loSitesBL.AssignTester(Convert.ToInt64(SiteId), Convert.ToInt64(TesterId[i]), Convert.ToInt64(ViewBag.UserId), Convert.ToDateTime(Date), "91", NetworkMode[i], Band[i], Carrier[i], Devices[i], TestTypes[i], null, LayerStatusId[i]);
                                }


                            }
                        }

                    }

                    DirectoryHandler dh = new DirectoryHandler();

                    if (Scope == "TSS")
                    {
                        TSS_VMBL vmb = new TSS_VMBL();
                        TSS_SectionBL secb = new TSS_SectionBL();

                        var TSS = vmb.ToList("", SiteId.ToString());
                        foreach (var item in TSS)
                        {
                            var Sections = secb.ToList("By_SurveyId", item.SurveyId.ToString());
                            foreach (var sec in Sections)
                            {
                                string folder = item.ClientPrefix + "/" + item.SiteCode + "/" + item.SurveyTitle + "/" + sec.SectionTitle;
                                dh.CreateDirectory(Server.MapPath("~/Content/AirViewLogs/" + folder));
                            }

                        }
                    }
                    else if (Scope == "CLS")
                    {
                        AV_WoDevicesDL wodl = new AV_WoDevicesDL();
                        AV_WoDevicesBL wdb = new AV_WoDevicesBL();
                        var rec = loSitesBL.GetSectors("GetSiteNetworkLayers", SiteId);
                        if (rec.Count > 0)
                        {
                            foreach (var item in rec)
                            {
                                string site = item.ClientPrefix + "/" + item.SiteCode;
                                dh.CreateDirectory(Server.MapPath("~/Content/AirViewLogs/" + site));
                            }
                        }
                        else
                        {
                            ExceptionHandling.ErrorLogs(ViewBag.RequestUrl, "Site layers not found");
                        }
                    }
                    else
                    {
                        AV_WoDevicesDL wodl = new AV_WoDevicesDL();
                        AV_WoDevicesBL wdb = new AV_WoDevicesBL();
                        var rec = loSitesBL.GetSectors("GetSiteNetworkLayers", SiteId);
                        if (rec.Count > 0)
                        {
                            foreach (var item in rec)
                            {
                                string site = item.ClientPrefix + "/" + item.SiteCode + "/" + item.NetworkMode.DefinationName + "_" + item.Band.DefinationName + "_" + item.Carrier.DefinationName;
                                dh.CreateDirectory(Server.MapPath("~/Content/AirViewLogs/" + site));
                            }
                        }
                        else
                        {
                            ExceptionHandling.ErrorLogs(ViewBag.RequestUrl, "Site layers not found");
                        }

                    }
                    res.Status = "success";
                    res.Message = "success";

                }
                else
                {
                    res.Status = "danger";
                    res.Message = "Select Layer.";
                }




            }
            catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = ex.Message;
                ExceptionHandling.ErrorLogs(ViewBag.RequestUrl, ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult WoHold(int SiteId)
        {
            Response res = new Response();
            try
            {
                AV_SitesDL sd = new AV_SitesDL();
                if (sd.Manage("WoHold", SiteId.ToString(), null, null, null, null, ViewBag.UserId))
                {
                    res.Status = "success";
                    res.Message = "save successfully";
                }
                else
                {
                    res.Status = "danger";
                    res.Message = "Record not found";
                }


            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult ApproveWithComment(int SiteId, string Comment)
        {
            Response res = new Response();
            try
            {
                AV_SitesDL sd = new AV_SitesDL();
                if (sd.Manage("WoHold", SiteId.ToString(), Comment, null, null, null, ViewBag.UserId))
                {
                    res.Status = "success";
                    res.Message = "save successfully";
                    TempData["msg_success"] = "save successfully";
                }
                else
                {
                    res.Status = "danger";
                    res.Message = "Record not found";
                    TempData["msg_error"] = "Record not found";
                }


            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
                TempData["msg_error"] = ex.Message;
            }

            //  return Json(res, JsonRequestBehavior.AllowGet);
            return RedirectToAction("index", "dashboard");

        }
        public double toRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
        private double ToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public LatLng getDestinationPoint(LatLng source, double brng, double dist)
        {
            dist = dist / 6371;
            brng = toRadians(brng);

            double lat1 = toRadians((double)source.latitude), lon1 = toRadians((double)source.longitude);
            double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(dist) +
                                    Math.Cos(lat1) * Math.Sin(dist) * Math.Cos(brng));
            double lon2 = lon1 + Math.Atan2(Math.Sin(brng) * Math.Sin(dist) *
                                            Math.Cos(lat1),
                                            Math.Cos(dist) - Math.Sin(lat1) *
                                            Math.Sin(lat2));
            if (Double.IsNaN(lat2) || Double.IsNaN(lon2))
            {
                return null;
            }
            LatLng obj = new LatLng();
            obj.latitude = (decimal)ToDegree(lat2);
            obj.longitude = (decimal)ToDegree(lon2);
            return obj;
        }
        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult MarketSitesAngles(string filter, int SiteId, string SiteCode, double Latitude, double Longitude)
        {
            AV_MarketSitesBL msb = new AV_MarketSitesBL();
            List<Azmiuth> Azmiuth = new List<Azmiuth>();
            var rec = msb.ToList(filter, SiteId, SiteCode, Latitude, Longitude, 0);
            Azmiuth az;
            foreach (var item in rec)
            {
                LatLng nLatLong = new LatLng();
                nLatLong.latitude = (decimal)item.Latitude;
                nLatLong.longitude = (decimal)item.Longitude;
                //az.InnerDistance = item.InnerDistance;
                //az.OuterDistance = item.OuterDistance;
                //az.RecieverDistance = item.RecieverDistance;


                az = new Azmiuth();
                az.Latitude = Convert.ToDouble(item.Latitude);
                az.Longitude = Convert.ToDouble(item.Longitude);
                az.Site = item.SiteCode;
                az.NetworkmodeId = (int)item.NetworkModeId;
                az.BandId = (int)item.BandId;
                az.CarrierId = (int)item.CarrierId;
                az.SectorId = (int)item.SectorId;
                az.ScopeId = (int)item.ScopeId;
                az.BandId = (int)item.BandId;
                az.SiteId = (int)item.SiteId;
                az.Sector = item.SectorCode;

                az.PCI = item.PCI.ToString();
                az.StartAngle = item.Azimuth - (item.BeamWidth / 2);
                az.EndAngle = item.Azimuth + (item.BeamWidth / 2);
                if (az.EndAngle == 360)
                {
                    az.EndAngle = 0;
                }
                else if (az.EndAngle > 360)
                {
                    double tmpEnd = az.EndAngle - 360;
                    if (tmpEnd >= 0 && tmpEnd <= 1)
                    {
                        az.EndAngle = 1;
                    }
                    else
                    {
                        az.EndAngle = Convert.ToInt32(az.EndAngle - 360);
                    }
                }
                var center = (az.StartAngle + az.EndAngle) / 2.0;
                nLatLong = getDestinationPoint(nLatLong, center, (double)item.InnerDistance);
                az.nLatitude = (double)nLatLong.latitude;
                az.nLongitude = (double)nLatLong.longitude;
                az.Color = item.SectorColor;
                Azmiuth.Add(az);
            }
            return Json(Azmiuth, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SiteReport(int id = 0, string Scope = null, string AzmuthRadius = "100")
        {
            ViewBag.ApiMapKey = ApiMapKey();
            TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
            string ModuleKeyCode = "MD_SITE_RPT";
            var ProjectId = TemplatesBL.ToList("GetProjectIdBySiteId", id.ToString()).FirstOrDefault();
            var ScopeId = TemplatesBL.ToList("GetScopeIdByScopeName", Scope).FirstOrDefault();
            if (ProjectId != null)
            {
                var templateData = TemplatesBL.ToList("IsTemplateExist", ModuleKeyCode).Where(x => x.ProjectId == ProjectId?.ProjectId).FirstOrDefault();
                if (templateData != null && ProjectId != null)
                {
                    return Redirect($"/Project/Template/Report/{templateData.TemplateId}?ScopeId={ScopeId?.ScopeId}&AzmuthRadius={AzmuthRadius}&SiteId={id}");
                }
            }
            if (Scope == "NI")
            {

                AV_GetSiteReportBL drb = new AV_GetSiteReportBL();
                NetLayerReport nlr = new NetLayerReport();
                AD_DefinationBL db = new AD_DefinationBL();
                List<SiteReportPlotVM> PlotData = new List<SiteReportPlotVM>();
                List<AD_ReportConfiguration> rptConf = new List<AD_ReportConfiguration>();
                string kmlPath = Server.MapPath("~/Content/AirViewLogs/");

                var recFormal = drb.NIReport(id, ref PlotData, ref rptConf, kmlPath);
                DataTable dtTime = recFormal.Value;
                var timeStamp = dtTime.ToList<ReportTimeStamp>();
                ViewBag.svrTimeStamp = timeStamp;
                var rec = recFormal.Key;
                if (rec.Count > 0)
                {
                    if (rptConf.Count <= 0)
                    {
                        TempData["msg_error"] = "Provide Report Configuration";
                        return RedirectToAction("Index", "Dashboard");
                    }
                    var index = rptConf.FindIndex(p => p.KeyCode == "REPORT_URL");
                    AD_ReportConfiguration adrp = new AD_ReportConfiguration();
                    adrp = rptConf[index];
                    adrp.KeyValue = "";
                    rptConf[index] = adrp;
                    ViewBag.ReportConfiguration = rptConf;
                }



                nlr.PCI_Circles = PlotData.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.plotColorName, TestType = m.TestType }).ToList();
                ViewBag.PciDisLagend = nlr.PCI_Circles.Where(m => m.TestType == "CW" || m.TestType == "CCW").GroupBy(m => m.PCI)
                                          .Select(grp => grp.First())
                                          .ToList();
                var Sectors = db.ToList("Sectors");
                ViewBag.Sectors = Sectors;
                ViewBag.AzmuthRadius = AzmuthRadius;

                //List<ReportTimeStamp> swaps = drb.ToListNI("SectorSwapCharts", id);
                // rec[0].reportTimeStamps = swaps;
                return View("SiteReportNI", rec);
                foreach (var item in rec)
                {
                    LatLng nLatLong = new LatLng();
                    nLatLong.latitude = Convert.ToDecimal(item.Latitude);
                    nLatLong.longitude = Convert.ToDecimal(item.Longitude);
                    nLatLong = getDestinationPoint(nLatLong, item.AngleToSite, (double)item.CoverageDistance);
                    item.NLatitude = nLatLong.latitude;
                    item.NLongitude = nLatLong.longitude;
                }
                return View("TelenorNI", rec);

            }

            SiteReportVM site = loSitesBL.GetSiteReportSummary(id);
            SitesBL sb = new SitesBL();
            ViewBag.SectorColors = sb;

            return View(site);
        }
        [IsLogin(CheckPermission = false), HttpGet]
        public JsonResult ListNISectorSwap(string Filter, Int64 SiteId)
        {
            AV_GetSiteReportBL drb = new AV_GetSiteReportBL();
            var rec = drb.ToListNIDataSet(Filter, SiteId);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }



        [IsLogin(CheckPermission = false), HttpGet]
        public JsonResult ListNetLayersTimeStamp(string Filter, Int64 SiteId)
        {
            AV_GetSiteReportBL drb = new AV_GetSiteReportBL();
            var rec = drb.ToListNI(Filter, SiteId);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }



        [IsLogin(CheckPermission = false), HttpGet]
        public JsonResult ListNetLayers(string Filter, Int64 SiteId)
        {
            AV_GetSiteReportBL drb = new AV_GetSiteReportBL();
            var rec = drb.ToListNI(Filter, SiteId);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false), HttpGet]
        public JsonResult SaveObservation(string SiteId=null,string CWComments=null,string CCWComments=null,string PDSCHComments=null,string PUSCHComments=null)
        {
            AV_SiteTestSummaryBL bl = new AV_SiteTestSummaryBL();
            bool result=bl.Manage("SaveObservation", SiteId, null, null, null, null, null, null, null, null, null, null, null, null, 0, CWComments, CCWComments, PDSCHComments, PUSCHComments);
            Response res = new Response();
            if (result)
            {
                res.Message = "Saved";
                res.Status = "200";
            }
            else
            {
                res.Message="Something went Wrong";
                res.Status = "500";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Duration = 20, VaryByParam = "none")]
        public ActionResult netLayerReport(int SiteId = 0, int BandId = 0, int Carrier = 0, int NetworkMode = 0, int CircleRadios = 17, int AzmuthRadius = 200, string Auto = "1")
        {
            
            ViewBag.ApiMapKey = ApiMapKey();
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoStore();

            TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
            string ModuleKeyCode = "MD_NET_LAYER";
            var ProjectId = TemplatesBL.ToList("GetProjectIdBySiteId", SiteId.ToString()).FirstOrDefault();
            if (ProjectId != null)
            {
                var templateData = TemplatesBL.ToList("IsTemplateExist", ModuleKeyCode).Where(x => x.ProjectId == ProjectId?.ProjectId).FirstOrDefault();
                if (templateData != null && ProjectId != null)
                {
                    return Redirect($"/Project/Template/Report/{templateData.TemplateId}?SiteId={SiteId}&NetworkModeId={NetworkMode}&BandId={BandId}&CarrierId={Carrier}&AzmuthRadius={AzmuthRadius}");
                }
            }


            try
            {
                AV_SiteTestSummaryBL sum = new AV_SiteTestSummaryBL();
                List<SiteReportPlotVM> ReportPlot = new List<SiteReportPlotVM>();
                List<AV_SiteTestLog> siteTestLog = new List<AV_SiteTestLog>();
                List<AD_ReportConfiguration> rptConf = new List<AD_ReportConfiguration>();
                List<AV_MarketSites> MarketSites = new List<AV_MarketSites>();
                List<AV_RFPlotLegends> RFPlotLegends = new List<AV_RFPlotLegends>();
                List<AV_FloorPlan> FloorPlan = new List<AV_FloorPlan>();
                List<AV_BeamTestLog> BTLog = new List<AV_BeamTestLog>();
                List<BeamTestLegend> BTLegend = new List<BeamTestLegend>();
                List<RDACounts> RDACounts = new List<RDACounts>();
                List<OoklaDLSiteLevels> OoklaDLSiteLevels = new List<OoklaDLSiteLevels>();
                WebConfig wc = new WebConfig();
                string ViewUrl = "";
                Int64 UserId = 11;
                if (ViewBag.UserId != null)
                {
                    UserId = ViewBag.UserId;
                }

                string kmlPath = Server.MapPath("~/Content/AirViewLogs/");
                DateTime plotDate = Convert.ToDateTime(wc.AppSettings("NewReportsDate"));
                bool AfterDate = false;
                List<Libraries.Common.TempData> ServerTimeStamp = new List<Libraries.Common.TempData>();
                var rec = sum.ToList(SiteId, BandId, Carrier, NetworkMode, UserId, ref siteTestLog, ref ReportPlot, ref rptConf, ref MarketSites, plotDate, ref AfterDate, kmlPath, ref ServerTimeStamp, ref RFPlotLegends,ref FloorPlan,ref BTLog,ref BTLegend,ref RDACounts,ref OoklaDLSiteLevels);
                if (rec.Count > 0)
                {
                    if (rptConf.Count <= 0)
                    {
                        TempData["msg_error"] = "Provide Report Configuration";
                        return RedirectToAction("Index", "Dashboard");
                    }
                    var index = rptConf.FindIndex(p => p.KeyCode == "REPORT_URL");
                    if (index < 1)
                    {
                        TempData["msg_error"] = "Provide Report Configuration URL For This Scope";
                        return RedirectToAction("Index", "Dashboard");
                    }
                    AD_ReportConfiguration adrp = new AD_ReportConfiguration();
                    adrp = rptConf[index];
                    ViewUrl = adrp.KeyValue;


                    adrp.KeyValue = "";
                    rptConf[index] = adrp;
                    ViewBag.ReportConfiguration = rptConf;

                    AD_DefinationBL db = new AD_DefinationBL();
                    var Sectors = db.ToList("Sectors");
                    ViewBag.Sectors = Sectors;

                    ViewBag.CircleRadios = CircleRadios;
                    ViewBag.AzmuthRadius = AzmuthRadius;
                    ViewBag.Auto = Auto;
                    ViewBag.AfterDate = AfterDate;
                    ViewBag.domain = wc.AppSettings("Domain");

                    var First = rec.FirstOrDefault();
                    if (rec != null)
                    {
                        AV_NetLayerStatusBL nlsb = new AV_NetLayerStatusBL();
                        AV_NetLayerStatus nls = nlsb.ToSingle("Get_Observation", decimal.ToInt32(First.NetworkModeId), decimal.ToInt32(First.BandId), decimal.ToInt32(First.CarrierId), SiteId, null);
                        ViewBag.Observation = nls.netLayerObservations;
                    }

                    //var result = myList.GroupBy(test => test.id)
                    //.Select(grp => grp.First())
                    //.ToList();
                    NetLayerReport nlr = new NetLayerReport();

                    nlr.PCI_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.plotColorName, TestType = m.TestType, NetworkMode = m.NetworkMode }).ToList();

                    if (First.SiteUploadDate >= plotDate)
                    {
                        ViewBag.AfterDate = true;

                        ViewBag.chLagend = nlr.PCI_Circles.Where(m => m.TestType == "Carrier").GroupBy(m => m.PCI)
                                          .Select(grp => grp.First())
                                          .ToList();

                        ViewBag.PORSites = MarketSites.Where(m => m.IsPOR == true).GroupBy(m => m.SiteCode).Select(m => m.First()).ToList();

                    }
                    else
                    {
                        if (rec.FirstOrDefault().City == "Chicago")
                        {
                            ViewUrl = "netLayerReport.cshtml";
                        }
                        

                        nlr.CINR_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.rsnrColor.Name }).ToList();
                        nlr.RsRp_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.rsrpColor.Name }).ToList();
                        nlr.RSRQ_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.rsrqColor.Name }).ToList();
                        nlr.CCW_Circles = ReportPlot.Where(m => m.TestType == "CCW" && (m.PCI != null || m.PCI != "N/A" || m.PCI != "0")).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.plotColorName }).ToList();
                        nlr.CW_Circles = ReportPlot.Where(m => m.TestType == "CW" && (m.PCI != null || m.PCI != "N/A" || m.PCI != "0")).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.plotColorName }).ToList();
                        nlr.CH_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.Carrier, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.ChColorName }).ToList();

                        ViewBag.chLagend = nlr.CH_Circles.GroupBy(m => m.PCI)
                                           .Select(grp => grp.First()).ToList();

                    }



                    nlr.CW_Marker = ReportPlot.Where(m => m.TestType == "CW" && m.IsHandover == true).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.PCI }).ToList();
                    nlr.CCW_Marker = ReportPlot.Where(m => m.TestType == "CCW" && m.IsHandover == true).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.PCI }).ToList();
                    nlr.CwCcw_Marker = ReportPlot.Where(m => m.IsHandover == true).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.PCI }).ToList();
                    nlr.PCIs = ReportPlot.GroupBy(m => m.PCI).Select(m => m.First().PCI).ToList();
                    nlr.CwPCIs = ReportPlot.Where(m => m.TestType == "CW").GroupBy(m => m.PCI).Select(m => m.First().PCI).ToList();
                    nlr.CcwPCIs = ReportPlot.Where(m => m.TestType == "CCW").GroupBy(m => m.PCI).Select(m => m.First().PCI).ToList();
                    //   nlr.ServerTimestamp = ReportPlot.GroupBy(m => m.serverTimestamp).Select(m=>m.First().serverTimestamp).ToList();

                    var ServerTimeStampLst = ServerTimeStamp.GroupBy(test => test.value1).Select(grp => grp.First()).ToList();
                    ViewBag.ServerTimestamp = ServerTimeStampLst;


                    nlr.CWDropSignal = ReportPlot.Where(m => m.TestType == "CW" == true && m.NetworkMode == "N/A" && m.Band == "" && m.Carrier == "").Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.PCI }).ToList();

                    #region Azmiuth
                    Azmiuth az;
                    string Band = First.Band;
                    foreach (var item in rec)
                    {
                        az = new Azmiuth();
                        az.Latitude = Convert.ToDouble(item.Latitude);
                        az.Longitude = Convert.ToDouble(item.Longitude);
                        az.Site = item.Site;
                        az.Sector = item.Sector;
                        az.PCI = item.PciId.ToString();
                        az.Color = item.SectorColor;//.ColorCode;
                        az.StartAngle = item.Azimuth - (item.BeamWidth / 2);
                        az.EndAngle = item.Azimuth + (item.BeamWidth / 2);

                        if (az.EndAngle == 360)
                        {
                            az.EndAngle = 0;
                        }
                        else if (az.EndAngle > 360)
                        {
                            double tmpEnd = az.EndAngle - 360;
                            if (tmpEnd >= 0 && tmpEnd <= 1)
                            {
                                az.EndAngle = 1;
                            }
                            else
                            {
                                az.EndAngle = Convert.ToInt32(az.EndAngle - 360);
                            }
                        }
                        //var secRec = Sectors.Where(m => m.DefinationName == item.Sector).FirstOrDefault();
                        //if (secRec != null)
                        //{
                        //    az.Color = secRec.ColorCode;
                        //}


                        nlr.Azmiuth.Add(az);
                    }

                    foreach (var item in MarketSites)
                    {
                        az = new Azmiuth();
                        az.Latitude = Convert.ToDouble(item.Latitude);
                        az.Longitude = Convert.ToDouble(item.Longitude);
                        az.Site = item.SiteCode;
                        az.Sector = item.SectorCode;
                        az.IsPOR = item.IsPOR;
                        az.PCI = item.PCI.ToString();
                        az.StartAngle = item.Azimuth - (item.BeamWidth / 2);
                        az.EndAngle = item.Azimuth + (item.BeamWidth / 2);

                        if (az.EndAngle == 360)
                        {
                            az.EndAngle = 0;
                        }
                        else if (az.EndAngle > 360)
                        {
                            double tmpEnd = az.EndAngle - 360;
                            if (tmpEnd >= 0 && tmpEnd <= 1)
                            {
                                az.EndAngle = 1;
                            }
                            else
                            {
                                az.EndAngle = Convert.ToInt32(az.EndAngle - 360);
                            }
                        }

                        if (item.Band == Band)
                        {
                            az.Color = item.SectorColor;
                        }
                        else
                        {
                            az.Color = "white";
                        }


                        nlr.Azmiuth.Add(az);
                    }


                    #endregion



                    ViewBag.NetLayerReport = nlr;
                    //ViewBag.PciDisLagend = nlr.PCI_Circles.Where(m => m.TestType == "CW" || m.TestType == "CCW").GroupBy(m => m.PCI).Select(grp => grp.First()).ToList();
                    ViewBag.PciDisLagend = nlr.PCI_Circles.Where(m => m.NetworkMode=="NR" && (m.TestType == "CW" || m.TestType == "CCW")).ToList();
                    ViewBag.PciLTEDisLagend = nlr.PCI_Circles.Where(m => m.NetworkMode=="LTE" && (m.TestType == "CW" || m.TestType == "CCW")).ToList();

                    Common.SelectedList sl = new Common.SelectedList();
                    ViewBag.Fonts = sl.Fonts();

                    ViewBag.RSRQ_PLOTLegends = RFPlotLegends.Where(m => m.KeyCode == "RSRQ_PLOT").ToList();
                    ViewBag.RSRP_PLOTLegends = RFPlotLegends.Where(m => m.KeyCode == "RSRP_PLOT").ToList();
                    ViewBag.CINR_PLOTLegends = RFPlotLegends.Where(m => m.KeyCode == "CINR_PLOT").ToList();


                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (First.Scope == "NI")
                    {
                        return View("~/views/site/netLayerReportNI.cshtml", rec);
                    }
                    //TODO: SPRINT Report - Remove below written line - for test only
                    //ViewUrl = "netLayerReport3.cshtml";

                    if (ViewUrl == "NetLayerReport_Sprint.cshtml")
                    {//TODO: SPRINT Report
                        AV_SiteTestLogBL stlb = new AV_SiteTestLogBL();
                        ViewBag.TraceLogs = stlb.ToList("PingTrace", SiteId, NetworkMode, Convert.ToInt64(First.BandId), Carrier, Convert.ToInt64(First.ScopeId));
                    }
                    return View("~/views/site/" + ViewUrl, rec);
                }

            }
            catch (Exception ex)
            {
                
                return Content(ex.Message);

            }
            return Content("No record found");
        }
        private string ApiMapKey()
        {
            WebConfig wc = new WebConfig();
            string MapKey = wc.AppSettings("ApiMapKey").ToString();
            return MapKey;
        }

        [OutputCache(Duration = 20, VaryByParam = "none"),IsLogin(CheckPermission =false)]
        public ActionResult netLayerIndoorReport(int SiteId = 0, int BandId = 0, int Carrier = 0, int NetworkMode = 0, int CircleRadios = 17, int AzmuthRadius = 200, string Auto = "1")
        {
            
                ViewBag.ApiMapKey = ApiMapKey();
                HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                HttpContext.Response.Cache.SetValidUntilExpires(false);
                HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Response.Cache.SetNoStore();

                TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
                string ModuleKeyCode = "MD_NET_LAYER";
                var ProjectId = TemplatesBL.ToList("GetProjectIdBySiteId", SiteId.ToString()).FirstOrDefault();
                if (ProjectId != null)
                {
                    var templateData = TemplatesBL.ToList("IsTemplateExist", ModuleKeyCode).Where(x => x.ProjectId == ProjectId?.ProjectId).FirstOrDefault();
                    if (templateData != null && ProjectId != null)
                    {
                        return Redirect($"/Project/Template/Report/{templateData.TemplateId}?SiteId={SiteId}&NetworkModeId={NetworkMode}&BandId={BandId}&CarrierId={Carrier}&AzmuthRadius={AzmuthRadius}");
                    }
                }


                try
                {
                    AV_SiteTestSummaryBL sum = new AV_SiteTestSummaryBL();
                    List<SiteReportPlotVM> ReportPlot = new List<SiteReportPlotVM>();
                    List<AV_SiteTestLog> siteTestLog = new List<AV_SiteTestLog>();
                    List<AD_ReportConfiguration> rptConf = new List<AD_ReportConfiguration>();
                    List<AV_MarketSites> MarketSites = new List<AV_MarketSites>();
                    List<AV_RFPlotLegends> RFPlotLegends = new List<AV_RFPlotLegends>();
                    List<AV_FloorPlan> FloorPlan = new List<AV_FloorPlan>();
                    List<AV_BeamTestLog> BTLog = new List<AV_BeamTestLog>();
                    List<BeamTestLegend> BTLegend = new List<BeamTestLegend>();
                    List<RDACounts> RDACounts = new List<RDACounts>();
                    List<OoklaDLSiteLevels> OoklaDLSiteLevels = new List<OoklaDLSiteLevels>();
                    WebConfig wc = new WebConfig();
                    string ViewUrl = "";
                    Int64 UserId = 11;
                    if (ViewBag.UserId != null)
                    {
                        UserId = ViewBag.UserId;
                    }

                    string kmlPath = Server.MapPath("~/Content/AirViewLogs/");
                    DateTime plotDate = Convert.ToDateTime(wc.AppSettings("NewReportsDate"));
                    bool AfterDate = false;
                    List<Libraries.Common.TempData> ServerTimeStamp = new List<Libraries.Common.TempData>();
                    var rec = sum.ToList(SiteId, BandId, Carrier, NetworkMode, UserId, ref siteTestLog, ref ReportPlot, ref rptConf, ref MarketSites, plotDate, ref AfterDate, kmlPath, ref ServerTimeStamp, ref RFPlotLegends, ref FloorPlan, ref BTLog, ref BTLegend, ref RDACounts, ref OoklaDLSiteLevels);
                    if (rec.Count > 0)
                    {
                        if (rptConf.Count <= 0)
                        {
                            TempData["msg_error"] = "Provide Report Configuration";
                            return RedirectToAction("Index", "Dashboard");
                        }

                        var index = rptConf.FindIndex(p => p.KeyCode == "REPORT_URL");
                        if (index < 1)
                        {
                            TempData["msg_error"] = "Provide Report Configuration URL For This Scope";
                            return RedirectToAction("Index", "Dashboard");
                        }
                        AD_ReportConfiguration adrp = new AD_ReportConfiguration();
                        adrp = rptConf[index];
                        ViewUrl = adrp.KeyValue;


                        adrp.KeyValue = "";
                        rptConf[index] = adrp;
                        ViewBag.ReportConfiguration = rptConf;

                        AD_DefinationBL db = new AD_DefinationBL();
                        var Sectors = db.ToList("Sectors");
                        ViewBag.Sectors = Sectors;

                        ViewBag.CircleRadios = CircleRadios;
                        ViewBag.AzmuthRadius = AzmuthRadius;
                        ViewBag.Auto = Auto;
                        ViewBag.AfterDate = AfterDate;
                        ViewBag.domain = wc.AppSettings("Domain");

                        var First = rec.FirstOrDefault();
                        if (rec != null)
                        {
                            AV_NetLayerStatusBL nlsb = new AV_NetLayerStatusBL();
                            AV_NetLayerStatus nls = nlsb.ToSingle("Get_Observation", decimal.ToInt32(First.NetworkModeId), decimal.ToInt32(First.BandId), decimal.ToInt32(First.CarrierId), SiteId, null);
                            ViewBag.Observation = nls.netLayerObservations;
                        }

                        //var result = myList.GroupBy(test => test.id)
                        //.Select(grp => grp.First())
                        //.ToList();
                        NetLayerReport nlr = new NetLayerReport();

                        nlr.PCI_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.plotColorName, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();
                        nlr.CINR_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.rsnrColor.Name, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();
                        nlr.RsRp_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.rsrpColor.Name, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();
                        nlr.RSRQ_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.rsrqColor.Name, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();
                        nlr.CCW_Circles = ReportPlot.Where(m => m.TestType == "IndoorTesting" && (m.PCI != null || m.PCI != "N/A" || m.PCI != "0")).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.plotColorName, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();
                        nlr.CW_Circles = ReportPlot.Where(m => m.TestType == "IndoorTesting" && (m.PCI != null || m.PCI != "N/A" || m.PCI != "0")).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.plotColorName, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();
                        nlr.CH_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.Carrier, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.ChColorName, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();

                        if (First.SiteUploadDate >= plotDate)
                        {
                            ViewBag.AfterDate = true;

                            ViewBag.chLagend = nlr.PCI_Circles.Where(m => m.TestType == "Carrier").GroupBy(m => m.PCI)
                                              .Select(grp => grp.First())
                                              .ToList();

                            ViewBag.PORSites = MarketSites.Where(m => m.IsPOR == true).GroupBy(m => m.SiteCode).Select(m => m.First()).ToList();


                        }
                        else
                        {
                            if (rec.FirstOrDefault().City == "Chicago")
                            {
                                ViewUrl = "netLayerIndoorReport.cshtml";
                            }

                            nlr.CINR_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.rsnrColor.Name, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();
                            nlr.RsRp_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.rsrpColor.Name, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();
                            nlr.RSRQ_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.rsrqColor.Name, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();
                            nlr.CCW_Circles = ReportPlot.Where(m => m.TestType == "IndoorTesting" && (m.PCI != null || m.PCI != "N/A" || m.PCI != "0")).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.plotColorName, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();
                            nlr.CW_Circles = ReportPlot.Where(m => m.TestType == "IndoorTesting" && (m.PCI != null || m.PCI != "N/A" || m.PCI != "0")).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.plotColorName, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();
                            nlr.CH_Circles = ReportPlot.Select(m => new AV_NetLayerReportPlot { PCI = m.Carrier, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.ChColorName, TestType = m.TestType, FloorId = m.FloorId, ServerTimestamp = m.serverTimestamp }).ToList();

                            ViewBag.chLagend = nlr.CH_Circles.GroupBy(m => m.PCI)
                                               .Select(grp => grp.First()).ToList();

                        }



                        nlr.CW_Marker = ReportPlot.Where(m => m.TestType == "CW" && m.IsHandover == true).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.PCI }).ToList();
                        nlr.CCW_Marker = ReportPlot.Where(m => m.TestType == "CCW" && m.IsHandover == true).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.PCI }).ToList();
                        nlr.CwCcw_Marker = ReportPlot.Where(m => m.IsHandover == true).Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.PCI }).ToList();
                        nlr.PCIs = ReportPlot.GroupBy(m => m.PCI).Select(m => m.First().PCI).ToList();
                        nlr.CwPCIs = ReportPlot.Where(m => m.TestType == "CW").GroupBy(m => m.PCI).Select(m => m.First().PCI).ToList();
                        nlr.CcwPCIs = ReportPlot.Where(m => m.TestType == "CCW").GroupBy(m => m.PCI).Select(m => m.First().PCI).ToList();
                        //   nlr.ServerTimestamp = ReportPlot.GroupBy(m => m.serverTimestamp).Select(m=>m.First().serverTimestamp).ToList();

                        var ServerTimeStampLst = ServerTimeStamp.GroupBy(test => test.value1).Select(grp => grp.First()).ToList();
                        ViewBag.ServerTimestamp = ServerTimeStampLst;


                        nlr.CWDropSignal = ReportPlot.Where(m => m.TestType == "CW" == true && m.NetworkMode == "N/A" && m.Band == "" && m.Carrier == "").Select(m => new AV_NetLayerReportPlot { PCI = m.PCI, Latitude = m.Latitude, Longitude = m.Longitude, Color = m.PCI }).ToList();

                        #region Azmiuth
                        Azmiuth az;
                        string Band = First.Band;
                        foreach (var item in rec)
                        {
                            az = new Azmiuth();
                            az.Latitude = Convert.ToDouble(item.Latitude);
                            az.Longitude = Convert.ToDouble(item.Longitude);
                            az.Site = item.Site;
                            az.Sector = item.Sector;
                            az.PCI = item.PciId.ToString();
                            az.Color = item.SectorColor;//.ColorCode;
                            az.StartAngle = item.Azimuth - (item.BeamWidth / 2);
                            az.EndAngle = item.Azimuth + (item.BeamWidth / 2);
                            az.FloorPath = item.FloorPath;
                            if (az.EndAngle == 360)
                            {
                                az.EndAngle = 0;
                            }
                            else if (az.EndAngle > 360)
                            {
                                double tmpEnd = az.EndAngle - 360;
                                if (tmpEnd >= 0 && tmpEnd <= 1)
                                {
                                    az.EndAngle = 1;
                                }
                                else
                                {
                                    az.EndAngle = Convert.ToInt32(az.EndAngle - 360);
                                }
                            }
                            //var secRec = Sectors.Where(m => m.DefinationName == item.Sector).FirstOrDefault();
                            //if (secRec != null)
                            //{
                            //    az.Color = secRec.ColorCode;
                            //}


                            nlr.Azmiuth.Add(az);
                        }

                        foreach (var item in MarketSites)
                        {
                            az = new Azmiuth();
                            az.Latitude = Convert.ToDouble(item.Latitude);
                            az.Longitude = Convert.ToDouble(item.Longitude);
                            az.Site = item.SiteCode;
                            az.Sector = item.SectorCode;
                            az.IsPOR = item.IsPOR;
                            az.PCI = item.PCI.ToString();
                            az.StartAngle = item.Azimuth - (item.BeamWidth / 2);
                            az.EndAngle = item.Azimuth + (item.BeamWidth / 2);
                            az.FloorPath = item.FloorPath;
                            if (az.EndAngle == 360)
                            {
                                az.EndAngle = 0;
                            }
                            else if (az.EndAngle > 360)
                            {
                                double tmpEnd = az.EndAngle - 360;
                                if (tmpEnd >= 0 && tmpEnd <= 1)
                                {
                                    az.EndAngle = 1;
                                }
                                else
                                {
                                    az.EndAngle = Convert.ToInt32(az.EndAngle - 360);
                                }
                            }

                            if (item.Band == Band)
                            {
                                az.Color = item.SectorColor;
                            }
                            else
                            {
                                az.Color = "white";
                            }


                            nlr.Azmiuth.Add(az);
                        }


                        #endregion



                        ViewBag.NetLayerReport = nlr;
                        ViewBag.PciDisLagend = nlr.PCI_Circles.Where(m => m.TestType == "CW" || m.TestType == "CCW").GroupBy(m => m.PCI)
                                               .Select(grp => grp.First())
                                               .ToList();
                        Common.SelectedList sl = new Common.SelectedList();
                        ViewBag.Fonts = sl.Fonts();

                        ViewBag.RSRQ_PLOTLegends = RFPlotLegends.Where(m => m.KeyCode == "RSRQ_PLOT").ToList();
                        ViewBag.RSRP_PLOTLegends = RFPlotLegends.Where(m => m.KeyCode == "RSRP_PLOT").ToList();
                        ViewBag.CINR_PLOTLegends = RFPlotLegends.Where(m => m.KeyCode == "CINR_PLOT").ToList();


                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        if (First.Scope == "NI")
                        {
                            return View("~/views/site/netLayerReportNI.cshtml", rec);
                        }
                        //TODO: SPRINT Report - Remove below written line - for test only
                        //ViewUrl = "netLayerReport3.cshtml";

                        if (ViewUrl == "netLayerReport3.cshtml")
                        {//TODO: SPRINT Report
                            AV_SiteTestLogBL stlb = new AV_SiteTestLogBL();
                            ViewBag.TraceLogs = stlb.ToList("PingTrace", SiteId, NetworkMode, Convert.ToInt64(First.BandId), Carrier, Convert.ToInt64(First.ScopeId));
                        }
                        ViewUrl = "netLayerIndoorReport.cshtml";
                        return View("~/views/site/" + ViewUrl, rec);
                    }

                }
                catch (Exception ex)
                {

                    return Content(ex.Message);

                }
            return Content("No record found");
        }





        [IsLogin(CheckPermission = false)]
        public ActionResult ntlFieldTestNI(int SiteId = 0, int BandId = 0, string Carrier = null, string NetworkMode = null)
        {

            AV_SiteTestSummaryBL stb = new AV_SiteTestSummaryBL();
            var rec = stb.NetLayerSummary(SiteId, BandId, Carrier, NetworkMode, 0);
            ViewBag.mail = "yes";
            // string a = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            ViewBag.Domain = Request.Url.Scheme + "://" + Request.Url.Authority + "/";
            return PartialView("~/views/site/_ntlFieldTestNI.cshtml", rec);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult ntlFieldTest(int SiteId = 0, int BandId = 0, string Carrier = null, string NetworkMode = null)
        {

            AV_SiteTestSummaryBL stb = new AV_SiteTestSummaryBL();
            var rec = stb.NetLayerSummary(SiteId, BandId, Carrier, NetworkMode, 0);
            if (rec.Count > 0)
            {
                var First = rec.FirstOrDefault();
                AD_ReportConfigurationBL rcb = new AD_ReportConfigurationBL();
                List<AD_ReportConfiguration> conf = rcb.ToList("byCityId_ClientId", First.CityId.ToString(), First.ClientId.ToString());
                ViewBag.ReportConfiguration = conf;

                ViewBag.mail = "yes";
                ViewBag.Domain = Request.Url.Scheme + "://" + Request.Url.Authority + "/";
            }
            else
            {
                ViewBag.ReportConfiguration = null;
            }



            return PartialView("~/views/site/_ntlFieldTest.cshtml", rec);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Mail(string Subject, string Body, string value)
        {
            Response res = new Response();
            try
            {
                AV_GetEmailsBL bl = new AV_GetEmailsBL();
                var rec = bl.ToList("MarketPOC", value);


                Thread thread = new Thread(() => SendEmail(rec, Subject, Body));
                thread.Start();


                res.Status = "success";
                res.Message = "sent successfully";


            }
            catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = ex.Message;
            }


            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        void SendEmail(List<AV_GetEmails> rec, string Subject, string Body)
        {
            WebConfig wc = new WebConfig();
            string SmtpServer = wc.AppSettings("SmtpServer");
            string SmtpServerPort = wc.AppSettings("SmtpServerPort");
            string FromEmail = wc.AppSettings("FromEmail");
            string FromEmailPassword = wc.AppSettings("FromEmailPassword");
            string ToEmail = wc.AppSettings("ToEmail");
            FromEmailPassword = Encryption.Decrypt(FromEmailPassword, true);
            Email em = new Email(SmtpServer, Convert.ToInt32(SmtpServerPort), FromEmail, FromEmailPassword);
            try
            {
                if (SmtpServer != null && SmtpServerPort != null && FromEmail != null && FromEmailPassword != null && ToEmail != null)
                {

                    foreach (var item in rec)
                    {
                        em.SendEmail(Subject, Body, item.Email);
                    }
                }
            }
            catch (Exception)
            {

                //throw;
            }

        }



        public ActionResult EditNetLayerReport(int SiteId = 0, int BandId = 0, string Carrier = null, string NetworkMode = null)
        {
            try
            {
                AV_SiteTestSummaryBL sum = new AV_SiteTestSummaryBL();
                var rec = sum.AV_GetNetworkLayerProcessed(SiteId, BandId, Carrier, NetworkMode);

                return View(rec);
            }
            catch (Exception)
            {

                throw;
            }


        }

        [HttpPost]
        public ActionResult EditNetLayerReport(List<AV_SiteTestSummary> rec)
        {
            try
            {
                dbDataTable dbdt = new dbDataTable();
                var Summery = dbdt.Tbl_AV_SiteTestSummary();

                #region UploadImages
                DirectoryHandler dh = new DirectoryHandler();
                List<string> OoklaTestFilePath = new List<string>();
                List<string> StationaryTestFilePath = new List<string>();
                List<string> MimoTestFilePath = new List<string>();
                List<string> SpeedTestFilePath = new List<string>();
                List<string> CaActiveTestFilePath = new List<string>();
                List<string> CaDeavticeTestFilePath = new List<string>();
                List<string> CaSpeedTestFilePath = new List<string>();
                List<string> LaaSpeedTestFilePath = new List<string>();
                List<string> LaaSmTestFilePath = new List<string>();
                List<string> CwTestFilePath = new List<string>();
                List<string> CcwTestFilePath = new List<string>();
                string Path = null;

                foreach (string item in Request.Files)
                {
                    // string tempUploadPath = "~" + UploadPath;
                    HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;

                    if (file.ContentLength == 0)
                    {
                        if (item.Contains("OoklaTestFilePath"))
                        {
                            OoklaTestFilePath.Add(null);
                        }
                        else if (item.Contains("StationaryTestFilePath"))
                        {
                            StationaryTestFilePath.Add(null);
                        }
                        else if (item.Contains("MimoTestFilePath"))
                        {
                            MimoTestFilePath.Add(null);
                        }
                        else if (item.Contains("SpeedTestFilePath"))
                        {
                            SpeedTestFilePath.Add(null);
                        }
                        else if (item.Contains("CwTestFilePath"))
                        {
                            CwTestFilePath.Add(null);
                        }
                        else if (item.Contains("CcwTestFilePath"))
                        {
                            CcwTestFilePath.Add(null);
                        }

                        else if (item.Contains("CaActiveTestFilePath"))
                        {
                            CaActiveTestFilePath.Add(null);
                        }
                        else if (item.Contains("CaDeavticeTestFilePath"))
                        {
                            CaDeavticeTestFilePath.Add(null);
                        }
                        else if (item.Contains("CaSDDTestFilePath"))
                        {
                            CaSpeedTestFilePath.Add(null);
                        }
                        else if (item.Contains("LaaSPDTestFilePath"))
                        {
                            LaaSpeedTestFilePath.Add(null);
                        }
                        else if (item.Contains("LaaSmTestFilePath"))
                        {
                            LaaSmTestFilePath.Add(null);
                        }


                        continue;
                    }


                    if (file.ContentLength > 0)
                    {
                        string extension = System.IO.Path.GetExtension(file.FileName).ToLower();
                        if (extension == ".png" || extension == ".jpg")
                        {
                            string site = rec[0].Site + "/" + rec[0].NetworkMode + "_" + rec[0].Band + "_" + rec[0].Carrier;
                            dh.CreateDirectory(Server.MapPath("~/Content/AirViewLogs/TMO/" + site));
                            Path = Server.MapPath("~/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            if (item.Contains("OoklaTestFilePath"))
                            {
                                OoklaTestFilePath.Add("/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            }
                            else if (item.Contains("StationaryTestFilePath"))
                            {
                                StationaryTestFilePath.Add("/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            }
                            else if (item.Contains("CwTestFilePath"))
                            {
                                CwTestFilePath.Add("/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            }
                            else if (item.Contains("CcwTestFilePath"))
                            {
                                CcwTestFilePath.Add("/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            }
                            else if (item.Contains("MimoTestFilePath"))
                            {
                                MimoTestFilePath.Add("/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            }
                            else if (item.Contains("SpeedTestFilePath"))
                            {
                                SpeedTestFilePath.Add("/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            }

                            else if (item.Contains("CaActiveTestFilePath"))
                            {
                                CaActiveTestFilePath.Add("/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            }
                            else if (item.Contains("CaDeavticeTestFilePath"))
                            {
                                CaDeavticeTestFilePath.Add("/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            }
                            else if (item.Contains("CaSDDTestFilePath"))
                            {
                                CaSpeedTestFilePath.Add("/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            }
                            else if (item.Contains("LaaSPDTestFilePath"))
                            {
                                LaaSpeedTestFilePath.Add("/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            }
                            else if (item.Contains("LaaSmTestFilePath"))
                            {
                                LaaSmTestFilePath.Add("/Content/AirViewLogs/TMO/" + site + "/" + System.IO.Path.GetFileName(file.FileName));
                            }

                            file.SaveAs(Path);
                        }

                    }

                }
                #endregion

                #region Add Rows In DataTable
                int count = 0;
                foreach (var sum in rec)
                {

                    DataRow row;
                    row = Summery.NewRow();

                    row["SummaryId"] = sum.SummaryId;
                    row["LatencyRate"] = sum.LatencyRate;
                    row["DownlinkRate"] = sum.DownlinkRate;
                    row["UplinkRate"] = sum.UplinkRate;
                    row["DownlinkMaxResult"] = sum.DownlinkMaxResult;
                    row["UplinkMaxResult"] = sum.UplinkMaxResult;
                    row["PingAverageResult"] = sum.PingAverageResult;
                    row["OoklaDownlinkResult"] = sum.OoklaDownlinkResult;
                    row["OoklaUplinkResult"] = sum.OoklaUplinkResult;
                    row["OoklaPingResult"] = sum.OoklaPingResult;
                    row["MoStatus"] = sum.MoStatus;
                    row["MtStatus"] = sum.MtStatus;
                    row["SMoStatus"] = sum.SMoStatus;
                    row["SMtStatus"] = sum.SMtStatus;
                    row["VMoStatus"] = sum.VMoStatus;
                    row["VMtStatus"] = sum.VMtStatus;
                    row["CwHandoverStatus"] = sum.CwHandoverStatus;
                    row["Ccwhandoverstatus"] = sum.Ccwhandoverstatus;
                    row["ICwHandoverStatus"] = sum.ICwHandoverStatus;
                    row["ICcwhandoverstatus"] = sum.ICcwhandoverstatus;
                    if (sum.OoklaTestFilePath != "remove")
                    {
                        row["OoklaTestFilePath"] = OoklaTestFilePath[count];
                    }
                    else
                    {
                        row["OoklaTestFilePath"] = "remove";
                    }
                    row["OoklaRssi"] = sum.OoklaRssi;
                    row["OoklaSinr"] = sum.OoklaSinr;
                    row["TestLatitude"] = sum.TestLatitude;
                    row["TestLongitude"] = sum.TestLongitude;
                    row["TestRssi"] = sum.TestRssi;
                    row["TestSinr"] = sum.TestSinr;
                    if (sum.StationaryTestFilePath != "remove")
                    {
                        row["StationaryTestFilePath"] = StationaryTestFilePath[count];
                    }
                    else
                    {
                        row["StationaryTestFilePath"] = "remove";
                    }
                    if (sum.CwTestFilePath != "remove")
                    {
                        row["CwTestFilePath"] = CwTestFilePath[count];
                    }
                    else
                    {
                        row["CwTestFilePath"] = "remove";
                    }
                    if (sum.CcwTestFilePath != "remove")
                    {
                        row["CcwTestFilePath"] = CcwTestFilePath[count];
                    }
                    else
                    {
                        row["CcwTestFilePath"] = "remove";
                    }
                    if (sum.MimoTestFilePath != "remove")
                    {
                        row["MimoTestFilePath"] = MimoTestFilePath[count];
                    }
                    else
                    {
                        row["MimoTestFilePath"] = "remove";
                    }
                    if (sum.SpeedTestFilePath != "remove")
                    {
                        row["SpeedTestFilePath"] = SpeedTestFilePath[count];
                    }
                    else
                    {
                        row["SpeedTestFilePath"] = "remove";
                    }

                    if (sum.CaActiveTestFilePath != "remove")
                    {
                        row["CaActiveTestFilePath"] = CaActiveTestFilePath[count];
                    }
                    else
                    {
                        row["CaActiveTestFilePath"] = "remove";
                    }

                    if (sum.CaDeavticeTestFilePath != "remove")
                    {
                        row["CaDeavticeTestFilePath"] = CaDeavticeTestFilePath[count];
                    }
                    else
                    {
                        row["CaDeavticeTestFilePath"] = "remove";
                    }

                    if (sum.CaSpeedTestFilePath != "remove")
                    {
                        row["CaSpeedTestFilePath"] = CaSpeedTestFilePath[count];
                    }
                    else
                    {
                        row["CaSpeedTestFilePath"] = "remove";
                    }

                    if (sum.LaaSpeedTestFilePath != "remove")
                    {
                        row["LaaSpeedTestFilePath"] = LaaSpeedTestFilePath[count];
                    }
                    else
                    {
                        row["LaaSpeedTestFilePath"] = "remove";
                    }

                    if (sum.LaaSmTestFilePath != "remove")
                    {
                        row["LaaSmTestFilePath"] = LaaSmTestFilePath[count];
                    }
                    else
                    {
                        row["LaaSmTestFilePath"] = "remove";
                    }




                    row["PingStatus"] = sum.PingStatus;
                    row["PhyDLStatus"] = sum.PhyDLStatus;
                    row["PhyULStatus"] = sum.PhyULStatus;
                    row["DownlinkStatus"] = sum.DownlinkStatus;
                    row["UplinkStatus"] = sum.UplinkStatus;
                    row["E911Status"] = sum.E911Status;
                    row["IsE911Performed"] = sum.IsE911Performed;
                    row["MimoStatus"] = sum.MimoStatus;
                    row["SMoStatus"] = sum.SMtStatus;
                    row["SMtStatus"] = sum.SMoStatus;
                    row["PhyDLSpeedMax"] = sum.PhyDLSpeedMax;
                    row["PhyULSpeedMax"] = sum.PhyULSpeedMax;
                    row["IntraHOInteruptTime"] = sum.IntraHOInteruptTime;
                    row["IntreHOInteruptTime"] = sum.IntreHOInteruptTime;
                  
                    row["callSetupTime"] = sum.callSetupTime;
                    row["CADLSpeed"] = sum.CADLSpeed;
                    row["CAULSpeed"] = sum.CAULSpeed;
                    Summery.Rows.Add(row);
                    count++;
                }
                #endregion

                AV_SiteTestSummaryDL std = new AV_SiteTestSummaryDL();
                std.Update(Summery, ViewBag.UserId);
                TempData["msg_success"] = "Edit Report successfully";

            }
            catch (Exception ex)
            {
                TempData["msg_error"] = ex.Message;
            }
            return RedirectToAction("index", "dashboard");

        }



        [HttpPost]
        public ActionResult Status(int SiteId, bool Status)
        {
            Response res = new Response();

            try
            {
                AV_SitesDL sd = new AV_SitesDL();
                sd.Manage("ChangeStatus", SiteId.ToString(), Status.ToString(), null, null, null, ViewBag.UserId);
                res.Status = "success";
                res.Message = "save successfully";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = ex.Message;
                return Json(res, JsonRequestBehavior.AllowGet);
            }

        }


        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult DisableMapRoot(string filter, int SiteId, int NetworkModeId, int BandId, int CarrierId, int ScopeId, bool Status, string FromTime, string ToTime)
        {
            Response res = new Response();

            try
            {
                AV_SiteTestLogDL sld = new AV_SiteTestLogDL();
                sld.Manage(filter, SiteId, NetworkModeId, BandId, ScopeId, CarrierId, Status.ToString(), FromTime, ToTime);
                res.Status = "success";
                res.Message = "save successfully";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
                return Json(res, JsonRequestBehavior.AllowGet);
            }

        }



        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult DisablePcis(string filter, int SiteId, int NetworkModeId, int BandId, int CarrierId, int ScopeId, bool Status, string Pcis)
        {
            Response res = new Response();

            try
            {
                AV_SiteTestLogDL sld = new AV_SiteTestLogDL();
                sld.Manage(filter, SiteId, NetworkModeId, BandId, ScopeId, CarrierId, Status.ToString(), Pcis);
                res.Status = "success";
                res.Message = "save successfully";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
                return Json(res, JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult NetLayerSummary(int SiteId)
        {
            Response res = new Response();

            try
            {
                AV_SectorBL sb = new AV_SectorBL();
                var rec = sb.ToList("Draw_Sectors", SiteId.ToString());
                res.Status = "success";
                res.Message = "save successfully";
                return View(rec);
            }
            catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = ex.Message;
                return Json(res, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult ManageStatus()
        {
            Common.SelectedList sl = new Common.SelectedList();
            var status = sl.Definations("byDefinationType", "WO Status", "-Status-");
            status = status.Where(m => m.Text == "Select" || m.Text == "Scheduled" || m.Text == "Drive Completed" || m.Text == "In Progress").ToList();

            ViewBag.WoStatus = status;
            return View();
        }
        [HttpPost]
        public ActionResult ManageStatus(string Filter, int[] SiteId, int[] Status, string[] Date)
        {
            Response res = new Response();
            try
            {
                dbDataTable ddt = new dbDataTable();
                DataTable List = ddt.List();
                #region Add Rows
                if (SiteId != null)
                {
                    for (int i = 0; i < SiteId.Length; i++)
                    {
                        myDataTable.AddRow(List, "Value1", SiteId[i], "Value2", Status[i], "Value3", Date[i], "Value4", Filter);
                    }

                    SitesBL sb = new SitesBL();
                    sb.SaveSiteAndGetId("ManageStatus", null, null, 0, 0, 0, null, null, null, ViewBag.UserId, null, DateTime.Now, null, List);
                    res.Status = "success";
                    res.Message = "save successfully";
                }
                else
                {
                    res.Status = "danger";
                    res.Message = "Select Any Site.";
                }

                #endregion


            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }

        [IsLogin(CheckPermission = false)]
        public ActionResult ManageStatusList(string Filter, string Value, DateTime? From, DateTime? To)
        {
            AV_SitesBL sitb = new AV_SitesBL();
            Common.SelectedList sl = new Common.SelectedList();
            ViewBag.WoStatus = sl.Definations("byDefinationType", "WO Status", "Status");

            Filter = (Filter == "wo") ? "ByStatus" : "BySiteCodeWithLayer";

            var rec = sitb.ToList(Filter, Value, null, null, null, null, From.ToString(), To.ToString());
            return PartialView("~/views/Site/_ManageStatusList.cshtml", rec);
        }

        public ActionResult ContactInfo(Int64 Id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactInfo(List<TSS_SiteContact> con, Int64 SiteId, AccessInfo AI)
        {
            Response r = new Response();
            dbDataTable dbdt = new dbDataTable();
            try
            {
                DataTable dt = dbdt.List();

                foreach (var c in con)
                {

                    myDataTable.AddRow(dt, "Value1", SiteId, "Value2", c.Title, "Value3", c.Gender, "Value4", c.FullName, "Value5", c.GateNo,
                                           "Value6", c.ContactNo, "Value7", c.ContactTypeID, "Value8", c.DesignationID, "Value9", c.IsHoldingKeys, "Value15", c.Comment);
                }
                TSS_SiteContactDL scd = new TSS_SiteContactDL();
                scd.Manage("Insert", dt, AI.IsSpecialAccess, AI.AccessDateTime, AI.AccessInstructions);
                r.Status = "success";
                r.Message = "success";
            }
            catch (Exception ex)
            {

                r.Status = "error";
                r.Message = ex.Message;
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult GetContactInfo(long SiteId)
        {
            TSS_SiteContactDL scd = new TSS_SiteContactDL();
            DataTable dt = scd.GetContactInfo("GET_Site_Contact_For_Edit", SiteId);
            DataTable dt2= scd.GetContactInfo("GET_Site_Access_Info", SiteId);
            List<AccessInfo> List_AI =dt2.ToList<AccessInfo>();
            AccessInfo AI = List_AI.FirstOrDefault();
            List<TSS_SiteContact> Contacts = dt.ToList<TSS_SiteContact>();

            var res = new { Contacts = Contacts, AI = AI };

            return Json(res, JsonRequestBehavior.AllowGet); ;
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult DisableServerTimestamp(string SiteId, bool IsActive, string selectedVal)
        {
            Response res = new Response();
            try
            {
                AV_SiteTestLogDL pd = new AV_SiteTestLogDL();
                var result = pd.DisableServerTimestamp("DisableServerTimestamp", SiteId, IsActive, selectedVal);
                return Json(result, JsonRequestBehavior.AllowGet);

                //dbDataTable ddt = new dbDataTable();
                //DataTable List = ddt.List();

                //if (selectedVal != null)
                //{
                //    for (int i = 0; i < selectedVal.Length; i++)
                //    {
                //        myDataTable.AddRow(List, "Value1", SiteId, "Value2", selectedVal[i]);
                //    }
                //    AV_SiteTestLogBL sbl = new AV_SiteTestLogBL();
                //    sbl.RemoveSiteTestLogs("DisableServerTimestamp", SiteId, IsActive, List);
                //    res.Status = "success";
                //    res.Message = "SiteTestLogs removed successfully";
                //}
                //else
                //{
                //    res.Status = "danger";
                //    res.Message = "Select Any Site.";
                //}
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult DisableNIPcis(string SiteId, bool IsActive, string selectedLayers = null, string selectedPcis = null, float? AngleFrom = null, float? AngleTo = null, float? DistanceFrom = null, float? DistanceTo = null)
        {
            Response res = new Response();

            try
            {
                AV_SiteTestLogDL pd = new AV_SiteTestLogDL();
                bool result = true;
                var pcis = selectedLayers.Split(',');
                foreach (var item in pcis)
                {
                    result = pd.DisablePcis("DisablePcis", SiteId, IsActive, item, selectedPcis, AngleFrom, AngleTo, DistanceFrom, DistanceTo);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult RemoveImageofLayers(int layerid, string testname)
        {
            bool result = false;

            try
            {
                //AV_SiteTestSummaryBL sum = new AV_SiteTestSummaryBL();
                //var rec = sum.AV_GetNetworkLayerProcessed(SiteId, BandId, Carrier, NetworkMode);
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //    res.Status = "danger";
                //    res.Message = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult SurveyReport(Int64 SiteSurveyId)
        {
            try
            {
                ViewBag.ApiMapKey = ApiMapKey();
                TSS_SectionBL s = new TSS_SectionBL();
                AD_DefinationBL db = new AD_DefinationBL();
                ViewBag.QuestionType = db.SelectedList("byDefinationType", "Question Type", "-Question Type-");
                var sur = s.SurvayBySite(SiteSurveyId);
                return View(sur);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult CheckList(int id, int siteId)
        {
            ViewBag.SiteSurveyId = id;
            return View();
        }

    }
}