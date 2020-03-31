using AirView.DBLayer.AirView.DAL;
using SWI.AirView.Common;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace SWI.AirView.Controllers
{
    /*----MoB!----*/
    [IsLogin]
    public class TemplateSettingController : Controller
    {
        // GET: TemplateSetting
        public ActionResult TestSetting()
        {
            SelectedList sl = new SelectedList();

            UserClientsBL ucb = new UserClientsBL();
            ViewBag.UserClients = ucb.SelectedList("byUserId", Convert.ToString(ViewBag.UserId));

            AD_DefinationBL db = new AD_DefinationBL();
            if (ViewBag.IsAdmin)
            {
                ViewBag.Cities = db.SelectedList("AllCities");
            }
            else
            {
                ViewBag.Cities = db.SelectedList("UserCities", Convert.ToString(ViewBag.UserId));
            }

            // ViewBag.Cities = sl.Cities();
            ViewBag.NetworkModes = db.SelectedList("NetworkModes",null,"-Select NetworkMode-");
            //ViewBag.NetworkModes = sl.NetworkModes();
            ViewBag.Bands = db.ToList("Bands");
            ViewBag.Carriers = db.ToList("Carriers");
          
           
            return View();
        }
        [HttpPost]
        public ActionResult TestSetting(FormCollection frm)
        {
            Response res = new Response();
            List<string> cities= frm["CityId"].ToString().Split(',').ToList();
            foreach (var city in cities)
            {
                DataTable TestConfiguration = new DataTable();
                #region Datatable Columns
                TestConfiguration.Columns.AddRange(new DataColumn[10]
                {
                                    new DataColumn("ClientId", typeof (int)),
                                    new DataColumn("CityId", typeof (int)),
                                    new DataColumn("TestTypeId", typeof (int)),
                                    new DataColumn("KpiId", typeof (int)),
                                    new DataColumn("KpiValue", typeof (string)),
                                    new DataColumn("TestCatogoryId", typeof (int)),
                                    new DataColumn("NetworkModeId", typeof (int)),
                                    new DataColumn("BandId", typeof (int)),
                                    new DataColumn("ProjectId", typeof (Int64)),
                                    new DataColumn("BandWidthId",typeof(Int64))
                    //new DataColumn("CarrierId", typeof (int)),
                });
                try
                {
                    #region Form Keys
                    string ClientId = frm["ClientId"].ToString();
                    string CityId = city.ToString();
                    string NetworkModeId = frm["NetworkModeId"].ToString();
                    string BandId = frm["BandId"].ToString();
                    long ProjectId = Convert.ToInt64(frm["ProjectId"]);
                    long BandWidthId = Convert.ToInt64(frm["BandWidthId"]);
                    //  string CarrierId = frm["CarrierId"].ToString();
                    string[] TestCatogoryId = frm["TestCatogoryId"].Split(',');
                    string[] TestTypeIds = frm["TestTypeId"].Split(',');
                    string[] KpiIds = frm["KpiId"].Split(',');
                    string[] KpiValues = frm["KpiValue"].Split(',');
                    #endregion


                    foreach (var tc in TestCatogoryId)
                    {

                        // check if test type exist for this category
                        var TestType = TestTypeIds.Where(m => m.StartsWith(tc)).ToList();
                        if (TestType != null)
                        {
                            foreach (var tt in TestType)
                            {
                                string TestTypeId = tt.Replace(tc + "-", string.Empty);


                                var Kpis = KpiIds.Where(m => m.StartsWith(TestTypeId)).ToList();

                                foreach (var ki in Kpis)
                                {
                                    string KpiId = ki.Replace(TestTypeId + "-", string.Empty);


                                    var value = KpiValues.Where(m => m.StartsWith(KpiId)).FirstOrDefault();
                                    if (value != null)
                                    {
                                        string KpiValue = value.Replace(KpiId + "-", string.Empty);

                                        #region Add Row In TestConfiguration
                                        DataRow row;
                                        row = TestConfiguration.NewRow();
                                        row["ClientId"] = ClientId;
                                        row["CityId"] = CityId;
                                        row["TestTypeId"] = TestTypeId;
                                        row["KpiId"] = KpiId;
                                        row["KpiValue"] = KpiValue;
                                        row["TestCatogoryId"] = tc;
                                        row["NetworkModeId"] = NetworkModeId;
                                        row["BandId"] = BandId;
                                        row["ProjectId"] = ProjectId;
                                        row["BandWidthId"] = BandWidthId;
                                        //  row["CarrierId"] = CarrierId;
                                        TestConfiguration.Rows.Add(row);
                                        #endregion
                                    }
                                }
                            }

                        }
                    }

                    TestConfigurationsDL tcd = new TestConfigurationsDL();
                    tcd.Save(TestConfiguration, ClientId, CityId, NetworkModeId, BandId);

                    //  TempData["msg_success"] = "save successfully";

                    res.Status = "success";
                    res.Message = "save successfully";
                }
                catch (Exception ex)
                {
                    res.Status = "danger";
                    res.Message = ex.Message;
                    // TempData["msg_error"] = ex.Message;
                }
            }
            
            #endregion

            
            return Json(res, JsonRequestBehavior.AllowGet);
           // return RedirectToAction("TestSetting");
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult TestTemplate() {
            AV_GetSettingTemplateBL temp = new AV_GetSettingTemplateBL();
            var Templates = temp.ToList("Template");
            TempData["Templates"] = Templates;



            ViewBag.TestCategory = Templates.GroupBy(test => test.TestCateoryID)
                   .Select(grp => grp.First())
                   .ToList();


            ViewBag.GetTestTypes = new Func<int, List<AV_GetSettingTemplate>>(GetTestTypes);

            ViewBag.GetKpi = new Func<int, List<AV_GetSettingTemplate>>(GetKpi);



            return PartialView("~/views/TemplateSetting/_TestTemplate.cshtml");
        }
        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult Template(int ClientId,int CityId,string Net, string Band)
        {
            AV_GetSettingTemplateBL temp = new AV_GetSettingTemplateBL();
            var rec = temp.ToList("byClinet_City_Net_Band", ClientId.ToString(), CityId.ToString(), Net,Band);
            return Json(rec,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Site(int SiteId, int NetworkModeId, int BandId)
        {
            AV_SiteConfigurationsBL site = new AV_SiteConfigurationsBL();
            var SiteData= site.ToList("GET_Configuration", SiteId.ToString(),  NetworkModeId.ToString(),  BandId.ToString());
            if (SiteData!=null && SiteData.Count>0)
            {
                ViewBag.Values = SiteData;
                ViewBag.SiteId = SiteId;
                var Single = SiteData.FirstOrDefault();
                if (Single!=null)
                {
                    ViewBag.ClientId = Single.ClientId;
                    ViewBag.CityId = Single.CityId;

                    ViewBag.RevisionId = (Single.SiteId > 0) ? Single.RevisionId + 1 : 1;
                    AV_GetSettingTemplateBL temp = new AV_GetSettingTemplateBL();
                    var Templates = temp.ToList("Template");
                    TempData["Templates"] = Templates;
                    ViewBag.TestCategory = Templates.GroupBy(test => test.TestCateoryID)
                           .Select(grp => grp.First())
                           .ToList();

                    ViewBag.GetTestTypes = new Func<int, List<AV_GetSettingTemplate>>(GetTestTypes);
                    ViewBag.GetKpi = new Func<int, List<AV_GetSettingTemplate>>(GetKpi);


                    ViewBag.NetworkModeId = NetworkModeId;
                    ViewBag.BandId = BandId;


                }
                else
                {
                    TempData["msg_error"] = "Site Record Not Found";
                }
                
            }
            else
            {
                TempData["msg_error"] = "Site Record Not Found";
            }
            



            return View();
        }
        [HttpPost]
        public ActionResult Site(FormCollection frm)
        {
            Response res = new Response();
            DataTable TestConfiguration = new DataTable();

            #region DataTable Columns

            TestConfiguration.Columns.AddRange(new DataColumn[10]
            {

                                    new DataColumn("ClientId", typeof (int)),
                                    new DataColumn("CityId", typeof (int)),
                                    new DataColumn("TestTypeId", typeof (int)),
                                    new DataColumn("KpiId", typeof (int)),
                                    new DataColumn("KpiValue", typeof (string)),
                                    new DataColumn("TestCatogoryId", typeof (int)),
                                     new DataColumn("SiteId", typeof (int)),
                                     new DataColumn("RevisionId", typeof (int)),
                                     new DataColumn("NetworkModeId", typeof (int)),
                                     new DataColumn("BandId", typeof (int)),
            });
            #endregion

            try
            {
                #region Form Keys
                string ClientId = frm["ClientId"].ToString();
                string CityId = frm["CityId"].ToString();
                string SiteId = frm["SiteId"].ToString();
                string RevisionId = frm["RevisionId"].ToString();
                string BandId = frm["BandId"].ToString();
                string NetworkModeId = frm["NetworkModeId"].ToString();
                
                string[] TestCatogoryId = frm["TestCatogoryId"].Split(',');
                string[] TestTypeIds = frm["TestTypeId"].Split(',');
                string[] KpiIds = frm["KpiId"].Split(',');
                string[] KpiValues = frm["KpiValue"].Split(',');
                #endregion


                foreach (var tc in TestCatogoryId)
                {

                    // check if test type exist for this category
                    var TestType = TestTypeIds.Where(m => m.StartsWith(tc)).ToList();
                    if (TestType != null)
                    {
                        foreach (var tt in TestType)
                        {
                            string TestTypeId = tt.Replace(tc + "-", string.Empty);


                            var Kpis = KpiIds.Where(m => m.StartsWith(TestTypeId)).ToList();

                            foreach (var ki in Kpis)
                            {
                                string KpiId = ki.Replace(TestTypeId + "-", string.Empty);


                                var value = KpiValues.Where(m => m.StartsWith(KpiId)).FirstOrDefault();
                                if (value != null)
                                {
                                    string KpiValue = value.Replace(KpiId + "-", string.Empty);

                                    #region Add Row In TestConfiguration
                                    DataRow row;
                                    row = TestConfiguration.NewRow();
                                    row["ClientId"] = ClientId;
                                    row["CityId"] = CityId;
                                    row["TestTypeId"] = TestTypeId;
                                    row["KpiId"] = KpiId;
                                    row["KpiValue"] = KpiValue;
                                    row["TestCatogoryId"] = tc;
                                    row["SiteId"] = SiteId;
                                    row["RevisionId"] = RevisionId;
                                    row["NetworkModeId"] = NetworkModeId;
                                    row["BandId"] = BandId;
                                    TestConfiguration.Rows.Add(row);
                                    #endregion
                                }
                            }
                        }

                    }
                }

                SiteConfigurationDL tcd = new SiteConfigurationDL();
                tcd.Save(TestConfiguration);
                res.Status = "success";
                res.Message = "save successfully";
               // TempData["msg_success"] = "save successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
              //  TempData["msg_error"] = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        private List<AV_GetSettingTemplate> GetTestTypes(int TestCateoryID) {
            var template = TempData["Templates"] as List<AV_GetSettingTemplate>;
            if (template!=null)
            {
                return template.Where(m => m.TestCateoryID == TestCateoryID).GroupBy(m => m.TestTypeID)
                   .Select(grp => grp.First())
                   .ToList();
            }else
            {
                return template;
            }
            
        }
        private List<AV_GetSettingTemplate> GetKpi(int TestTypeID)
        {
            var template = TempData["Templates"] as List<AV_GetSettingTemplate>;
            if (template!=null)
            {
                return template.Where(m => m.TestTypeID == TestTypeID).ToList();
            }else
            {
                return template;

            }
        }
        public ActionResult ScannerSetting()
        {
            SelectedList sl = new SelectedList();
            UserClientsBL ucb = new UserClientsBL();
            ViewBag.UserClients = ucb.SelectedList("byUserId", Convert.ToString(ViewBag.UserId));

            AD_DefinationBL db = new AD_DefinationBL();
            if (ViewBag.IsAdmin)
            {
                ViewBag.Cities = db.SelectedList("AllCities");
            }
            else
            {
                ViewBag.Cities = db.SelectedList("UserCities", Convert.ToString(ViewBag.UserId));
            }
            ViewBag.Manufacturer = db.SelectedList("byDefinationType", "ScannerManufacturer", "-Select Manufacturer-");
            ViewBag.ScannerModel = db.ToList("byDefinationType", "ScannerModel");
            ViewBag.ScannerProtocol = db.SelectedList("byDefinationType", "Scanner Protocol", "-Select Protocol-");
            ViewBag.ScannerBand = db.ToList("byDefinationType", "Scanner Band");
            return View();
        }
        [HttpPost]
        public ActionResult ScannerSetting(FormCollection frm)
        {
            Response res = new Response();
            DataTable ScannerConfiguration = new DataTable();
            #region Datatable Columns
            ScannerConfiguration.Columns.AddRange(new DataColumn[10]
            {
                                    new DataColumn("ClientId", typeof (int)),
                                    new DataColumn("CityId", typeof (int)),
                                    new DataColumn("TestTypeId", typeof (int)),
                                    new DataColumn("KpiId", typeof (int)),
                                    new DataColumn("KpiValue", typeof (string)),
                                    new DataColumn("TestCatogoryId", typeof (int)),
                                    new DataColumn("ManufacturerId", typeof (int)),
                                    new DataColumn("ScannerModelId", typeof (int)),
                                    new DataColumn("ProtocolId", typeof (int)),
                                    new DataColumn("ScannerBandId", typeof (int)),
            });
            #endregion

            try
            {
                #region Form Keys
                string ClientId = frm["ClientId"].ToString();
                string CityId = frm["CityId"].ToString();
                string ManufacturerId = frm["ManufacturerId"].ToString();
                string ScannerModelId = frm["ScannerModelId"].ToString();
                string ProtocolId = frm["ProtocolId"].ToString();
                string ScannerBandId = frm["ScannerBandId"].ToString();
                string[] TestCatogoryId = frm["TestCatogoryId"].Split(',');
                string[] TestTypeIds = frm["TestTypeId"].Split(',');
                string[] KpiIds = frm["KpiId"].Split(',');
                string[] KpiValues = frm["KpiValue"].Split(',');
                #endregion


                foreach (var tc in TestCatogoryId)
                {

                    // check if test type exist for this category
                    var TestType = TestTypeIds.Where(m => m.StartsWith(tc)).ToList();
                    if (TestType != null)
                    {
                        foreach (var tt in TestType)
                        {
                            string TestTypeId = tt.Replace(tc + "-", string.Empty);


                            var Kpis = KpiIds.Where(m => m.StartsWith(TestTypeId)).ToList();

                            foreach (var ki in Kpis)
                            {
                                string KpiId = ki.Replace(TestTypeId + "-", string.Empty);


                                var value = KpiValues.Where(m => m.StartsWith(KpiId)).FirstOrDefault();
                                if (value != null)
                                {
                                    string KpiValue = value.Replace(KpiId + "-", string.Empty);

                                    #region Add Row In TestConfiguration
                                    DataRow row;
                                    row = ScannerConfiguration.NewRow();
                                    row["ClientId"] = ClientId;
                                    row["CityId"] = CityId;
                                    row["TestTypeId"] = TestTypeId;
                                    row["KpiId"] = KpiId;
                                    row["KpiValue"] = KpiValue;
                                    row["TestCatogoryId"] = tc;
                                    row["ManufacturerId"] = ManufacturerId;
                                    row["ScannerModelId"] = ScannerModelId;
                                    row["ProtocolId"] = ProtocolId;
                                    row["ScannerBandId"] = ScannerBandId;
                                    ScannerConfiguration.Rows.Add(row);
                                    #endregion
                                }
                            }
                        }

                    }
                }

                ScannerConfigurationsDL tcd = new ScannerConfigurationsDL();
                tcd.Save(ScannerConfiguration);
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
        public ActionResult ScannerTemplate(int SelectedScannerModelId=0)
        {
            AV_GetScannerSettingTemplateBL temp = new AV_GetScannerSettingTemplateBL();
            var Templates = temp.ToList("Template",SelectedScannerModelId.ToString());
            TempData["Templates"] = Templates;

            ViewBag.TestCategory = Templates.GroupBy(test => test.TestCateoryID)
                   .Select(grp => grp.First())
                   .ToList();

            ViewBag.GetTestTypes = new Func<int, List<AV_GetScannerSettingTemplate>>(GetScannerTestTypes);

            ViewBag.GetKpi = new Func<int, List<AV_GetScannerSettingTemplate>>(GetScannerKpi);

            return PartialView("~/views/TemplateSetting/_ScannerTemplate.cshtml");
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult SiteScannerTemplate(int SelectedScannerModelId = 0)
        {
            AV_GetScannerSettingTemplateBL temp = new AV_GetScannerSettingTemplateBL();
            var Templates = temp.ToList("Template",SelectedScannerModelId.ToString());
            TempData["Templates"] = Templates;

            ViewBag.TestCategory = Templates.GroupBy(test => test.TestCateoryID)
                   .Select(grp => grp.First())
                   .ToList();

            ViewBag.GetTestTypes = new Func<int, List<AV_GetScannerSettingTemplate>>(GetScannerTestTypes);

            ViewBag.GetKpi = new Func<int, List<AV_GetScannerSettingTemplate>>(GetScannerKpi);

            return PartialView("~/views/TemplateSetting/_SiteScannerTemplate.cshtml");
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult Scanner(int SiteId, int NetworkModeId, int BandId)
        {
            SelectedList sl = new SelectedList();
            UserClientsBL ucb = new UserClientsBL();
            ViewBag.UserClients = ucb.SelectedList("byUserId", Convert.ToString(ViewBag.UserId));
            AD_DefinationBL db = new AD_DefinationBL();
            if (ViewBag.IsAdmin)
            {
                ViewBag.Cities = db.SelectedList("AllCities");
            }
            else
            {
                ViewBag.Cities = db.SelectedList("UserCities", Convert.ToString(ViewBag.UserId));
            }
            ViewBag.Manufacturer = db.SelectedList("byDefinationType", "ScannerManufacturer", "-Select Manufacturer-");
            ViewBag.ScannerModel = db.ToList("byDefinationType", "ScannerModel");
            ViewBag.ScannerProtocol = db.SelectedList("byDefinationType", "Scanner Protocol", "-Select Protocol-");
            ViewBag.ScannerBand = db.ToList("byDefinationType", "Scanner Band");
            AV_SiteScannerConfigurationsBL site = new AV_SiteScannerConfigurationsBL();
            var SiteData = site.ToList("GET_SiteParams", SiteId.ToString(), NetworkModeId.ToString(), BandId.ToString(),"");
            if (SiteData != null && SiteData.Count > 0)
            {
                var Single = SiteData.FirstOrDefault();
                if (Single != null)
                {
                    ViewBag.ClientId = Single.ClientId;
                    ViewBag.CityId = Single.CityId;
                }
            }
            ViewBag.SiteId = SiteId;
            ViewBag.NetworkModeId = NetworkModeId;
            ViewBag.BandId = BandId;
            //ViewBag.IsExist = false;
            //var siteConfig = site.ToList("GET_Configuration", SiteId.ToString(), NetworkModeId.ToString(), BandId.ToString());
            //if (siteConfig.Count()>0)
            //{
            //    ViewBag.IsExist = true;
            //    ViewBag.Values = siteConfig;
            //    TempData["Templates"] = siteConfig;
            //    ViewBag.TestCategory = siteConfig.GroupBy(test => test.TestCatogoryId)
            //           .Select(grp => grp.First())
            //           .ToList();

            //    ViewBag.GetTestTypes = new Func<int, List<AV_GetScannerSettingTemplate>>(GetScannerTestTypes);
            //    ViewBag.GetKpi = new Func<int, List<AV_GetScannerSettingTemplate>>(GetScannerKpi);
            //}
            return View();
        }
        [IsLogin(CheckPermission = false)]
        [HttpPost]
        public ActionResult Scanner(FormCollection frm)
        {
            Response res = new Response();
            DataTable ScannerConfiguration = new DataTable();

            #region DataTable Columns

            ScannerConfiguration.Columns.AddRange(new DataColumn[14]
            {

                                    new DataColumn("ClientId", typeof (int)),
                                    new DataColumn("CityId", typeof (int)),
                                    new DataColumn("TestTypeId", typeof (int)),
                                    new DataColumn("KpiId", typeof (int)),
                                    new DataColumn("KpiValue", typeof (string)),
                                    new DataColumn("TestCatogoryId", typeof (int)),
                                     new DataColumn("SiteId", typeof (int)),
                                     new DataColumn("RevisionId", typeof (int)),
                                     new DataColumn("NetworkModeId", typeof (int)),
                                     new DataColumn("BandId", typeof (int)),
                                     new DataColumn("ScannerModelId", typeof (int)),
                                     new DataColumn("ManufacturerId", typeof (int)),
                                     new DataColumn("ProtocolId", typeof (int)),
                                     new DataColumn("ScannerBandId", typeof (int)),
            });
            #endregion

            try
            {
                #region Form Keys
                string ClientId = frm["ClientId"].ToString();
                string CityId = frm["CityId"].ToString();
                string SiteId = frm["SiteId"].ToString();
                string BandId = frm["BandId"].ToString();
                string NetworkModeId = frm["NetworkModeId"].ToString();
                string ScannerModelId = frm["ScannerModelId"].ToString();
                string ManufacturerId = frm["ManufacturerId"].ToString();
                string ProtocolId = frm["ProtocolId"].ToString();
                string ScannerBandId = frm["ScannerBandId"].ToString();

                string[] TestCatogoryId = frm["TestCatogoryId"].Split(',');
                string[] TestTypeIds = frm["TestTypeId"].Split(',');
                string[] KpiIds = frm["KpiId"].Split(',');
                string[] KpiValues = frm["KpiValue"].Split(',');
                #endregion


                foreach (var tc in TestCatogoryId)
                {

                    // check if test type exist for this category
                    var TestType = TestTypeIds.Where(m => m.StartsWith(tc)).ToList();
                    if (TestType != null)
                    {
                        foreach (var tt in TestType)
                        {
                            string TestTypeId = tt.Replace(tc + "-", string.Empty);


                            var Kpis = KpiIds.Where(m => m.StartsWith(TestTypeId)).ToList();

                            foreach (var ki in Kpis)
                            {
                                string KpiId = ki.Replace(TestTypeId + "-", string.Empty);


                                var value = KpiValues.Where(m => m.StartsWith(KpiId)).FirstOrDefault();
                                if (value != null)
                                {
                                    string KpiValue = value.Replace(KpiId + "-", string.Empty);

                                    #region Add Row In TestConfiguration
                                    DataRow row;
                                    row = ScannerConfiguration.NewRow();
                                    row["ClientId"] = ClientId;
                                    row["CityId"] = CityId;
                                    row["TestTypeId"] = TestTypeId;
                                    row["KpiId"] = KpiId;
                                    row["KpiValue"] = KpiValue;
                                    row["TestCatogoryId"] = tc;
                                    row["SiteId"] = SiteId;
                                    row["NetworkModeId"] = NetworkModeId;
                                    row["BandId"] = BandId;
                                    row["ScannerModelId"] = ScannerModelId;
                                    row["ManufacturerId"] = ManufacturerId;
                                    row["ProtocolId"] = ProtocolId;
                                    row["ScannerBandId"] = ScannerBandId;
                                    ScannerConfiguration.Rows.Add(row);
                                    #endregion
                                }
                            }
                        }

                    }
                }

                SiteScannerConfigurationDL tcd = new SiteScannerConfigurationDL();
                tcd.Save(ScannerConfiguration);
                res.Status = "success";
                res.Message = "save successfully";
                // TempData["msg_success"] = "save successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
                //  TempData["msg_error"] = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult ScannerTemplate(int ClientId, int CityId, string Manufacturer, string ScannerModel,string SelectedProtocolId,string SelectedBandId)
        {
            AV_GetScannerSettingTemplateBL temp = new AV_GetScannerSettingTemplateBL();
            var rec = temp.ToList("byClinet_City_Net_Band", ClientId.ToString(), CityId.ToString(), Manufacturer, ScannerModel, SelectedProtocolId, SelectedBandId);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult SiteScannerTemplate(int SiteId, int NetworkModeId, int BandId, string Manufacturer, string ScannerModel, string SelectedProtocolId, string SelectedBandId)
        {
            AV_SiteScannerConfigurationsBL site = new AV_SiteScannerConfigurationsBL();
            var rec = site.ToList("GET_Configuration", SiteId.ToString(), NetworkModeId.ToString(), BandId.ToString(),ScannerModel,Manufacturer, SelectedProtocolId, SelectedBandId);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        private List<AV_GetScannerSettingTemplate> GetScannerTestTypes(int TestCateoryID)
        {
            var template = TempData["Templates"] as List<AV_GetScannerSettingTemplate>;
            if (template != null)
            {
                return template.Where(m => m.TestCateoryID == TestCateoryID).GroupBy(m => m.TestTypeID)
                   .Select(grp => grp.First())
                   .ToList();
            }
            else
            {
                return template;
            }

        }

        private List<AV_GetScannerSettingTemplate> GetScannerKpi(int TestTypeID)
        {
            var template = TempData["Templates"] as List<AV_GetScannerSettingTemplate>;
            if (template != null)
            {
                return template.Where(m => m.TestTypeID == TestTypeID).ToList();
            }
            else
            {
                return template;

            }
        }

    }
}