using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.Security.Filters;
using System;
using System.Web.Mvc;
using System.Linq;
using SWI.AirView.Common;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;
using SWI.Libraries.Common;
using Newtonsoft.Json.Linq;
using SWI.Libraries.AirView.DAL;
using System.Data;
using SWI.Libraries.AD.Entities;
using WebApplication.Models;
using Library.SWI.Template.Model;
using AirView.DBLayer.Template.BLL;
using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.Template.Model;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Net;
using System.Xml;
using walkDirectory;
using AirView.DBLayer.AirView.Entities;

namespace SWI.AirView.Controllers
{
    [IsLogin, ErrorHandling]
    public class NetLayerStatusController : Controller
    {
        // GET: NetLayerStatus
        #region PRIVATE PROPERTIES
        private TMP_NodeBL NodeBL = new TMP_NodeBL();
        private TMP_NodesPropertiesBL NodesPropertiesBL = new TMP_NodesPropertiesBL();
        private AV_SiteScriptScannerConfigurationsBL SiteScriptScannerConfigBL = new AV_SiteScriptScannerConfigurationsBL();
        private AD_DefinationBL DefinationBL = new AD_DefinationBL();
        private TMP_NodeSettingsBL NodeSettingsBL = new TMP_NodeSettingsBL();
        #endregion
        public ActionResult NewPendingIssue(AV_NetLayerStatus ns)
        {
            Common.SelectedList sl = new Common.SelectedList();
            ViewBag.Reasons = sl.Reasons();

            AV_NetLayerStatusBL nlsb = new AV_NetLayerStatusBL();
            AV_NetLayerStatus nls = nlsb.ToSingle("Get_PendingWithIssue", Decimal.ToInt32(ns.NetworkModeId), Decimal.ToInt32(ns.BandId), Decimal.ToInt32(ns.CarrierId), Decimal.ToInt32(ns.SiteId), null);
            if (nls != null)
            {
                return PartialView("~/views/NetLayerStatus/_NewPendingIssue.cshtml", nls);
            }
            return PartialView("~/views/NetLayerStatus/_NewPendingIssue.cshtml", nls);
        }

        [HttpPost]
        public ActionResult NewPendingIssue(AV_NetLayerStatus ns, string post)
        {
            Response res = new Response();
            try
            {
                AV_NetLayerStatusDL nlsd = new AV_NetLayerStatusDL();
                nlsd.Manage("Set_PendingWithIssue", Decimal.ToInt32(ns.NetworkModeId), Decimal.ToInt32(ns.BandId), Decimal.ToInt32(ns.CarrierId), Decimal.ToInt32(ns.SiteId), ViewBag.UserId, ns.StatusReason.ToString(), ns.PendingIssueDesc);
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

        public ActionResult NewObservation(AV_NetLayerStatus ns)
        {
            Common.SelectedList sl = new Common.SelectedList();
            ViewBag.Reasons = sl.Reasons();
            return PartialView("~/views/NetLayerStatus/_NewObservation.cshtml", ns);
        }

        [HttpPost]
        public ActionResult NewObservation(AV_NetLayerStatus ns, string post)
        {
            Response res = new Response();
            try
            {
                AV_NetLayerStatusDL nlsd = new AV_NetLayerStatusDL();
                nlsd.Manage("Set_Observation", Decimal.ToInt32(ns.NetworkModeId), Decimal.ToInt32(ns.BandId), Decimal.ToInt32(ns.CarrierId), Decimal.ToInt32(ns.SiteId), ViewBag.UserId, ns.netLayerObservations);
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

        [HttpPost]
        public ActionResult MarkReportSubmit(AV_NetLayerStatus ns)
        {
            Response res = new Response();
            try
            {
                AV_NetLayerStatusDL nlsd = new AV_NetLayerStatusDL();
                nlsd.Manage("Set_Status", Decimal.ToInt32(ns.NetworkModeId), Decimal.ToInt32(ns.BandId), Decimal.ToInt32(ns.CarrierId), Decimal.ToInt32(ns.SiteId), ViewBag.UserId, ns.KeyCode);
                res.Status = "success";
                res.Message = "Mark as reportSubmit successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MarkApproved(AV_NetLayerStatus ns)
        {
            Response res = new Response();
            try
            {
                AV_NetLayerStatusDL nlsd = new AV_NetLayerStatusDL();
                nlsd.Manage("Set_Status", Decimal.ToInt32(ns.NetworkModeId), Decimal.ToInt32(ns.BandId), Decimal.ToInt32(ns.CarrierId), Decimal.ToInt32(ns.SiteId), ViewBag.UserId, ns.KeyCode);
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

        public ActionResult LayerActivation(Int64 SiteId)
        {

            SitesBL sl = new SitesBL();
            var rec = sl.GetSiteBands("Get_All", SiteId);
            AV_SectorBL secb = new AV_SectorBL();
            ViewBag.SiteId = SiteId;
            ViewBag.Sectors = secb.ToList("BySiteId", SiteId.ToString(),null,null,null,null);
            return PartialView("~/views/NetLayerStatus/_LayerActivation.cshtml", rec);
        }


        [HttpPost]
        public ActionResult IsActive(string LayerId, bool Status)
        {
            Response res = new Response();
            try
            {
                AV_NetLayerStatusDL nlsd = new AV_NetLayerStatusDL();
                nlsd.Manage("Set_IsActive", 0, 0, 0, 0, ViewBag.UserId, LayerId, Status.ToString());
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


        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult SectorStatus(Int64 SectorId, bool Status)
        {
            Response res = new Response();
            try
            {
                
                AV_NetLayerStatusDL nlsd = new AV_NetLayerStatusDL();
                AV_SectorDL secd = new AV_SectorDL();
                secd.Manage(null,"Set_isActive", SectorId.ToString(), Status.ToString());
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



        public ActionResult WoStatus(Int64 LayerId)
        {
            AV_NetLayerStatusBL nlsb = new AV_NetLayerStatusBL();
            var rec = nlsb.ToSingle("Get_byLayerStatusId", 0, 0, 0, 0, LayerId.ToString());
            if (rec != null)
            {
                AD_DefinationBL db = new AD_DefinationBL();
                var Status = db.ToList("byDefinationType", "WO Status");
                int Order = Status.Where(m => m.DefinationId == rec.Status).Select(m => m.SortOrder).FirstOrDefault();
                ViewBag.Status = Status.Where(m => m.SortOrder < Order && m.SortOrder > 0).Select(m => new SWI.Libraries.Common.SelectedList { Value = m.DefinationId.ToString(), Text = m.DefinationName }).ToList();

            }
            return PartialView("~/views/NetLayerStatus/_WoStatus.cshtml", rec);
        }
        [HttpPost]
        public ActionResult WoStatus(Int64 LayerStatusId, Int64 Status, string Remarks)
        {
            Response res = new Response();
            try
            {
                AV_NetLayerStatusDL nlsd = new AV_NetLayerStatusDL();
                nlsd.Manage("Status_Movement", 0, 0, 0, 0, ViewBag.UserId, LayerStatusId.ToString(), Status.ToString(), Remarks);
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
        public ActionResult Information(Int64 Id)
        {
            ViewBag.SiteId = Id;
            List<string> Chennels = new List<string>();
            List<string> TestTypes = new List<string>();
            List<GetDirctoryinformation> singdir = new List<GetDirctoryinformation>();
            SitesBL sb = new SitesBL();

            try
            {
                List<BandVM> Bands = sb.GetSiteBands("", Id);
                if (Bands.Count > 0)
                {
                    AV_SectorBL secb = new AV_SectorBL();
                    ViewBag.Sectors = secb.ToList("GetSectors", Id.ToString(), null, null, null, null);
                    ViewBag.Bands = Bands.Select(m => new { m.BandId, m.NetworkMode, m.BandName, m.Carrier, Text = m.NetworkMode + " " + m.BandName + " " + m.Carrier, Value = m.SiteCode + "_" + m.NetworkMode + "_" + m.BandName + "_" + m.Carrier }).ToList();
                    ViewBag.NetworkModeBag = Bands;
                    var band = Bands.FirstOrDefault();
                    var sPath = Server.MapPath("~/Content/AirViewLogs/" + band.ClientPrefix + "\\" + band.SiteCode);
                    ViewBag.FilePath = sPath;
                    if (Directory.Exists(sPath))
                    {
                        FileInfo[] allFiles = new DirectoryInfo(sPath).GetFiles("*.xml", SearchOption.AllDirectories);
                        if (allFiles.Length > 0)
                        {
                            foreach (var file in allFiles)
                            {
                                GetDirctoryinformation info = new GetDirctoryinformation();
                                info.createDate = file.CreationTime.ToString();
                                info.directoryPath = file.FullName;
                                info.fileName = file.Name;
                                info.fileType = file.Extension;
                                info.size = (file.Length / 1024).ToString();
                                singdir.Add(info);
                            }
                        }
                        var fileList = new DirectoryInfo(sPath).GetFiles("*.xml", SearchOption.AllDirectories);
                        myFile mf = new myFile();
                        for (int i = 0; i < fileList.Length; i++)
                        {
                            try
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(fileList[i].FullName);
                                string jsonText = JsonConvert.SerializeXmlNode(doc);
                                JObject json = JObject.Parse(jsonText);
                                //string txt = mf.FileToString(fileList[i].FullName);
                                //JObject json = JObject.Parse(txt);


                                string rssTitle = (string)json["Drive"]["TestType"];

                                foreach (var x in json)
                                {
                                    // string name = x.Key;
                                    JToken value = x.Value;
                                    if (value != null)
                                    {
                                        var sector = value["TestType"];
                                        TestTypes.Add(sector.ToString());
                                    }

                                }

                                var Chennellst = from p in json["Drive"]["message-body"]
                                                 select new { channel = (string)p["channel"], Event = (string)p["event"] };

                                foreach (var item in Chennellst)
                                {
                                    Chennels.Add(item.channel);
                                }
                            }
                            catch(Exception ex) {

                            }

                                
                            
                            
                        }
                        

                      
                    }
                }
            }
            catch (Exception )
            {

                //throw;
            }
            ViewBag.AVXFiles = singdir.ToList();
            ViewBag.Chennels = Chennels.Distinct().ToList();
            ViewBag.TestTypes = TestTypes.Distinct().ToList();
            return View();
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult Information(string Layer = "_", string networkMode = "", string FilePath = "",string File="")
        {
            Response res = new Response();
            try
            {
                List<L3TempModel> Messages = new List<L3TempModel>();
                if (Directory.Exists(FilePath))
                {
                    string fileName = "*.xml";
                    if (File!= "All")
                    {
                        fileName = File;
                    }
                    var fileList = new DirectoryInfo(FilePath).GetFiles(fileName, SearchOption.AllDirectories);
                    int count = 0;
                    foreach (var file in fileList)
                    {
                            myFile mf = new myFile();
                            XmlDocument doc = new XmlDocument();
                            doc.Load(fileList[count].FullName);
                            string jsonText = JsonConvert.SerializeXmlNode(doc);
                            JObject json = JObject.Parse(jsonText);
                            DateFunctions df = new DateFunctions();
                            networkMode = (networkMode == "All") ? "" : networkMode;
                            var rssTitle = json["Drive"]["message-body"];
                            if (rssTitle!=null)
                            {
                                var Chennellst = from p in json["Drive"]["message-body"]
                                                 select new L3TempModel { rat = (string)p["rat"], RadioBearerID = (string)p["RadioBearerID"], Freq = (string)p["Freq"], PCI=(string)p["PCI"], time = (string)p["time"], channel = (string)p["channel"], Event = (string)p["event"], Ticket = (string)p["time"] };
                                Messages.AddRange(Chennellst);
                            }
                        count++;
                    }
                    return Json(Messages.Where(a=>a.rat.ToLower().Contains(networkMode.ToLower())), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    res.Status = "error";
                    res.Message = "file path not found.";
                }
            }
            catch (Exception ex)
            {
                res.Status = "error";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission =false)]
        public ActionResult GetSectors(int SiteId,int NetworkModId,int BandId,int CarrierId,int ScopeId)
        {
            SitesBL loSitesBL = new SitesBL();
            List<SectorsVM> lstSectors = loSitesBL.GetSectors("GetSectors", SiteId, NetworkModId, BandId, CarrierId, ScopeId);
            return Json(lstSectors, JsonRequestBehavior.AllowGet);
        }
        [HttpPost,IsLogin(CheckPermission =false)]
        public ActionResult LayerMessage(string Layer, string Chennel, string FilePath,string Time,string Event)
        {

            Response res = new Response();
            try
            {
                if (Directory.Exists(FilePath))
                {
                    var fileList = new DirectoryInfo(FilePath).GetFiles("*.xml", SearchOption.AllDirectories);
                    int count = 0;
                    foreach (var file in fileList)
                    {
                        if (file.FullName.Contains(Layer))
                        {
                            try
                            {
                                myFile mf = new myFile();
                                XmlDocument doc = new XmlDocument();
                                doc.Load(fileList[count].FullName);
                                string jsonText = JsonConvert.SerializeXmlNode(doc);
                                JObject json = JObject.Parse(jsonText);
                                //string txt = mf.FileToString(fileList[count].FullName);
                                //JObject json = JObject.Parse(txt);

                                JToken rates = json["Drive"]["message-body"].Children().Where(m => (string)m["channel"] == Chennel && (string)m["event"] == Event && (string)m["time"] == Time).FirstOrDefault();
                                //var rec = rates[Chennel];
                                //foreach (var item in rates)
                                //{
                                //    var t = item;
                                //}
                                var rec = rates;
                                var message = rec.ToString();

                                res.Status = "success";
                                res.Message = message;

                                return Json(res, JsonRequestBehavior.AllowGet);
                            }
                            catch {}
                            
                        }
                        count++;
                    }
                   
                }
                else
                {
                    res.Status = "error";
                    res.Message = "file path not found.";
                }


            }
            catch (Exception ex)
            {

                res.Status = "error";
                res.Message = ex.Message;
            }
                return Json(res, JsonRequestBehavior.AllowGet);

        }



        public ActionResult Script(Int64 LayerId,Int64 SiteId, Int64 NetworkModeId, Int64 BandId,string Scope,Int64 CarrierId,int? SiteClusterId=0,int? ScopeId=0)
        {
            
            if (SiteClusterId == 0)
            {
              
               ViewBag.CLS = "";
            }
            else
            {  
              ViewBag.CLS = "CLS";
            }
            ViewBag.ScopeId = ScopeId;
            ViewBag.Scope = Scope;
            //, Int64 BandId, Int64 CarrierId, Int64 ScopeId
            AD_DefinationBL db = new AD_DefinationBL();
            AV_SiteScriptBL ssb = new AV_SiteScriptBL();
            ViewBag.PreviousRecord = ssb.ToList("ByNetLayerStatusId", LayerId.ToString());
            ViewBag.LayerId = LayerId;

            ViewBag.SiteId = SiteId;
            ViewBag.NetworkModeId = NetworkModeId;
            ViewBag.BandId = BandId;
            ViewBag.CarrierId = CarrierId;
            ViewBag.Events = db.ToList("byDefinationType", "Script Events");
            ViewBag.ScriptTypes = db.SelectedList("byDefinationType", "Script Type", null);
            ViewBag.SiteClusterId = SiteClusterId;
            List<AD_Defination> def = new List<AD_Defination>();
            def = db.ToList("byDefinationType", "Script Events");
            def.AddRange(db.ToList("byDefinationType", "Script Type"));
            def.AddRange(db.ToList("byDefinationType", "Script Sub Events"));
            ViewBag.definations = def;


            return View();
           // return PartialView("~/views/NetLayerStatus/_Script.cshtml");
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult GetBC()
        {
            try
            {
                AD_DefinationBL abc = new AD_DefinationBL();
                var Carriers =    abc.ToList("Carriers", "");
                var Bands = abc.ToList("Bands", "");
                var ScannerBand = abc.ToList("byDefinationType", "Scanner Band");
                var ScannerProtocol = abc.ToList("byDefinationType", "Scanner Protocol");
                var ScannerMeasurement = abc.ToList("byScannerModel", "Scanner Measurement Type");
                var Manufacturer = abc.ToList("byDefinationType", "ScannerManufacturer");
                var ScannerModel = abc.ToList("byDefinationType", "ScannerModel");
                return Json(new { Status = "Success", Carriers = Carriers, Bands=Bands, ScannerProtocol = ScannerProtocol, ScannerMeasurement = ScannerMeasurement, ScannerBand=ScannerBand, Manufacturer= Manufacturer, ScannerModel= ScannerModel}, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { Status="error" }, JsonRequestBehavior.AllowGet);
            }
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetOptionIdsByBand(string band="",string search="",string start = "",string next="")
        {
            AD_DefinationBL abc = new AD_DefinationBL();
            var OptionIds = abc.ToList("byDefinationTypeOptionBand", band+","+search+","+start+","+next);
            return Json(OptionIds, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue // Use this value to set your maximum size for all of your Requests
            };
        }
        [IsLogin(CheckPermission =false),HttpPost]
        public ActionResult ScriptPreview(List<AV_ScriptPreview> data,string value2,string value3, string value4, string Head) {
            if (1>0)
            {

                AV_SiteConfigurationsBL site = new AV_SiteConfigurationsBL();
                List<AV_SiteConfigurations> result = new List<AV_SiteConfigurations>();
                var value = "";
                if (data !=null){ 
                foreach(var tm in data)
                {
                    value = value+tm.Col + ',';
                    Head = Head+ tm.Heads + ",";
                }
                    ViewBag.TestTitles = Head;
                result= site.ToList("GET_SCRIPT_SETTINGS", value, value2, value3, value4).Where(m=>m.KpiValue=="15").ToList();
             //foreach(var item in rec)
             //   {
             //       var aa = data.Where(x => x.Col == item.TestId.ToString()).FirstOrDefault();
             //       if (aa != null)
             //       {
             //              HeadTest = Head.Split(',').FirstOrDefault();
             //               item.TestCategory = HeadTest;
             //               var obj = Head.Split(',').Skip(1);
             //               Head= string.Join(",", obj);
                           
             //               result.Add(item);

             //       }
             //       else
             //       {
             //           aa = data.Where(x => x.Col == item.TestTypeId.ToString()).FirstOrDefault();
             //           if (aa != null)
             //           {
             //                   HeadTest = Head.Split(',').FirstOrDefault();
             //                   item.TestCategory = HeadTest;
             //                //   item.TestCategory = aa.Heads;
             //                  result.Add(item);
             //                  // data.Remove(aa);
             //           }
             //       }
               
             //   }
                }
                else
                {
                    result = site.ToList("GET_SCRIPT_SETTINGS", value, value2, value3, value4).Where(m => m.KpiValue == "15").ToList();
                }
                //      AD_DefinationBL db = new AD_DefinationBL();
                //    var rec = db.ToList("GetSettings", value).Where(m=>m.DefinationTypeId==15).ToList();

                return PartialView("~/views/NetLayerStatus/_ScriptPreview.cshtml", result.OrderBy(x=>x.SortOrder));
            }
            return null;
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult ScriptKPI(string value)
        {
            if (value != null)
            {

                AD_DefinationBL db = new AD_DefinationBL();

                var rec = db.ToList("byDefinationType", value);
                //AD_DefinationBL db = new AD_DefinationBL();
                //var rec = db.ToList("GetSettings", value).Where(m=>m.DefinationTypeId==15).ToList();


                return Json(rec, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
       
        [HttpPost]
        public ActionResult Script(Int64 NetLayerId,Int64 SiteId, Int64[] EventTypeId,string[] EventValue, string[] BandReselection, string[] ScannerSelection, string[] ManufacturerSelection, string[] ScannerModelSelection, string[] OptionSelection, string[] OptionIdSelection, bool[] IsValue, bool[] IsL3Enabled,string[] Color,int[] SequenceId,int? SiteClusterId = 0,int? ScopeId=0,string[] TestKPI=null,string[] SrId=null,string[] IsDeleted=null)
        {
            Response res = new Response();
            try
            {
                dbDataTable dbtd = new dbDataTable();
                CustomDataTable ctd = new CustomDataTable();
                DataTable dt = dbtd.Script();
                DataTable cs = dbtd.Script();
                int counter = 1;
                long LayerId = 0;
                for (int i = 0; i < EventValue.Length; i++)
                {
                    //  bool contains = Sequences.Any(z => z == i || i==0);
                    //if(contains == true)
                    //{
                    //    if (i == 0) counter = 1;
                    //    else counter = i;
                    //    myDataTable.AddRow(dt, "Value1", SiteId, "Value2", NetLayerId, "Value3", 0, "Value4", EventTypeId[i], "Value5", EventValue[i], "Value6", IsValue[i], "Value7", IsL3Enabled[i], "Value8", Color[i], "Value8", counter);

                    //}
                    //else
                    //{

                    if (IsL3Enabled[i])
                    {
                        
                            myDataTable.AddRow(dt, "Value1", SiteId, "Value2", NetLayerId, "Value3", 0, "Value4", EventTypeId[i], "Value5", EventValue[i], "Value6", IsValue[i], "Value7", IsL3Enabled[i], "Value8", Color[i], "Value9", SequenceId[i], "Value10", BandReselection[i], "value11", TestKPI[i], "value12", i + 1, "value13", SrId[i], "value14", IsDeleted[i], "value15", (i + 1).ToString(), "value16", "", "value17", "", "value18", "", "value19", "", "value20", "", "value21", "", "value22", "", "value23", "", "value24", "", "value25", "");
                       
                    }
                    else
                    {
                        
                            myDataTable.AddRow(dt, "Value1", SiteId, "Value2", NetLayerId, "Value3", 0, "Value4", EventTypeId[i], "Value5", EventValue[i], "Value6", IsValue[i], "Value7", IsL3Enabled[i], "Value8", Color[i], "Value9", SequenceId[i], "Value10", BandReselection[i], "value11", "", "value12", i + 1, "value13", SrId[i], "value14", IsDeleted[i], "value15", ScannerSelection[i], "value16", ManufacturerSelection[i], "value17", ScannerModelSelection[i], "value18", OptionSelection[i], "value19", OptionIdSelection[i], "value20", "", "value21", "", "value22", "", "value23", "", "value24", "", "value25", "");

                    }

                    //}
                  
                    
                }
                AV_SiteScriptDL ssd = new AV_SiteScriptDL();
                var scripts=  ssd.Manage("Insert", dt);

                if (SiteClusterId != 0)
                {
                    myDataTable.AddRow(cs, "Value1", SiteId, "Value2", NetLayerId, "Value3", 0, "Value4", SiteClusterId, "Value5", NetLayerId, "Value6", ScopeId,"Value7",true);
                   for (int i = 0; i < SequenceId.Length; i++)
                    {
                        if (SequenceId[i] == counter && SiteClusterId != 0)
                        {
                            myDataTable.AddRow(cs, "Value1", SiteId, "Value2", NetLayerId, "Value3", SequenceId[i], "Value4", SiteClusterId,"Value5", NetLayerId,"Value6",ScopeId, "Value7",false);
                            counter++;
                        }
                    }
                    ssd.CLSScript("CLSScripts", cs);
                }
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
        public JsonResult SavedFormInfo(long FormId=0)
        {
           // FormId = !string.IsNullOrEmpty(FormId) ? FormId : "1";
            List<AV_SiteScriptFormEntry> FormItems = NodesPropertiesBL.ToList("ByNodeTypeId", Convert.ToInt64(FormId));

            string FormHTML, ControlTypeName, DataTypeName;
            FormHTML = DataTypeName = ControlTypeName = "";
            int i = 1;

            var controlTypeList = DefinationBL.ToList("byDefinationType", "ControlType");
            var dataTypeList = DefinationBL.ToList("byDefinationType", "DataType");

            var formTypeList = DefinationBL.ToList("byDefinationType", "NodeType").Where(x => x.PDefinationId == 0);

            if (FormId == -1)
            {
                return Json(new
                {
                    FormTypeList = formTypeList.ToList()
                }, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in FormItems)
            {

                var InputValueType = dataTypeList.Where(x => x.DefinationId == Convert.ToInt64(item.DataType)).FirstOrDefault().DefinationName;
                DataTypeName = "<select readonly style='min-width:100px;' class='form-control dropdown DataTypeChange' data-row='" + i + "' id='dataType" + i + "' name='dataType'>";
                foreach (var dataTypeItem in dataTypeList)
                {
                    DataTypeName += dataTypeItem.DefinationId.ToString() == item.DataType ?
                        "<option value='" + dataTypeItem.DefinationId + "' selected='selected'> " + dataTypeItem.DefinationName + " </option> " :
                        "";//"<option value='" + dataTypeItem.DefinationId + "'> " + dataTypeItem.DefinationName + " </option> ";
                }
                DataTypeName += "</select>";

                //ControlTypeName = "<select readonly style='min-width:100px;' class='form-control dropdown' data-row='" + i + "' id='controlType" + i + "' name='controlType'>";
                //foreach (var controlTypeItem in controlTypeList)
                //{
                //    ControlTypeName += controlTypeItem.DefinationId.ToString() == item.ControlType ?
                //        "<option value='" + controlTypeItem.DefinationId + "' selected='selected'> " + controlTypeItem.DefinationName + " </option> " :
                //        "";//"<option value='" + controlTypeItem.DefinationId + "'> " + controlTypeItem.DefinationName + " </option> ";
                //}
                //ControlTypeName += "</select>";






                if (InputValueType.ToLower() == "number" || InputValueType.ToLower() == "decimal")
                {
                    FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + DataTypeName + " </td> <td><input class='form-control' type='number' id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' value='" + item.DefaultValue + "'/></td> <td><input class='form-control' type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                }
                else if (InputValueType.ToLower() == "image")
                {
                    FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td><td> " + DataTypeName + " </td> <td><input class='form-control' readonly type='text' id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' value='" + item.DefaultValue + "'/></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                }
                else if (InputValueType.ToLower() == "date")
                {
                    FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td>   <td> " + DataTypeName + " </td> <td><input class='form-control'  type='date' id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' value='" + item.DefaultValue + "'/></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                }
                else if (InputValueType.ToLower() == "colorpicker")
                {
                    FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td><td> " + DataTypeName + " </td> <td><input class='form-control'  type='color' id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' value='" + item.DefaultValue + "'/></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                }
                else if (InputValueType.ToLower() == "radiobutton")
                {
                    if (item.DefaultValue.ToLower() == "selected")
                    {
                        FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td><td> " + DataTypeName + " </td> <td><select  class='form-control'  id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' '" + item.DefaultValue + "'><option selected value='selected' >Selected</option><option value='unselected' >Unselected</option></select></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";

                    }
                    else
                    {
                        FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td><td> " + DataTypeName + " </td> <td><select class='form-control'   id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' '" + item.DefaultValue + "'><option value='selected' >Selected</option><option selected value='unselected' >Unselected</option></select></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";

                    }
                }

                //else if (InputValueType.ToLower() == "staticdropdown")
                //{
                //    if(item.DefaultValue !="")
                //        {
                //      var SplitedValues = item.DefaultValue.Split(',');
                //        var Options = "";
                //        foreach(var value in SplitedValues)
                //        {
                //           Options += "<option  value='"+ value + "'+ >"+ value+ "</option>";
                //        }

                //        FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input class='form-control' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + ControlTypeName + " </td>   <td> " + DataTypeName + " </td> <td><select class='form-control'   id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' '" + item.DefaultValue + "'>"+ Options + "</select></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                //    }
                //    else
                //    {

                //        FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input class='form-control' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + ControlTypeName + " </td>   <td> " + DataTypeName + " </td> <td><select class='form-control'   id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' '" + item.DefaultValue + "'></select></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                //    }
                //}
                else
                {
                    FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input class='form-control title' required type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td>  <td> " + DataTypeName + " </td> <td><input class='form-control'  type='text' id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' value='" + item.DefaultValue + "'/></td> <td><input class='form-control' type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                }

                
                if (item.Required.ToLower() == "true")
                {
                    FormHTML += "<td> <input type='checkbox' class='require' data-row='" + i + "' checked/>";
                    FormHTML += "<input type='hidden' value='" + item.Required + "' id='require" + i + "' name='required' /> </td> ";
                }
                else
                {
                    FormHTML += "<td> <input type='checkbox' class='require' data-row='" + i + "' value='" + item.Required + "' />";
                    FormHTML += "<input type='hidden' value='" + item.Required + "' id='require" + i + "' name='required' /> </td> ";
                }

               

                FormHTML += "<td><input type='hidden' name='formid' id='formid" + i + "'  value='" + item.FormId + "'    /><input type='hidden' class='deleted' name='isdeleted' value='0' id='isdeleted" + i + "'  /><input class='form-control sortOrderAutoValue' type='number' id='sortOrderInput" + i + "' name='sortOrder' value='" + item.SortOrder + "' max='9999' onkeypress = 'return isNumber(event)'/></td> </tr>";
                i++;
            }

            return Json(new
            {
                Form = FormHTML,
                FormTypeList = formTypeList.ToList()
            });
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult FormBuilderPartial(int FormTypeId, string KeyId)
        {
            ViewBag.DefinationId = !string.IsNullOrEmpty(KeyId) ? KeyId : "0";
            ViewBag.FormId = FormTypeId;
            return PartialView("_SiteScriptFormBuilder");
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetScannerConfigurations(int KeyId,int measurementId,int ManufacturerId=0,int ScannerModelId=0,int ProtocolId=0,int ScannerBandId = 0,string SiteData = "",string SelectedChannels="")
        {
            AV_GetScannerSettingTemplateBL temp = new AV_GetScannerSettingTemplateBL();
            var Templates = temp.ToList("ScannerConfiguration", measurementId.ToString(), KeyId.ToString(), ManufacturerId.ToString(), ScannerModelId.ToString(),ProtocolId.ToString(), ScannerBandId.ToString(),SiteData);
            TempData["Templates"] = Templates;
            ViewBag.GetKpi = new Func<int, List<AV_GetScannerSettingTemplate>>(GetScannerKpi);
            ViewBag.KeyId = KeyId;
            ViewBag.MeasurementId = measurementId;
            ViewBag.SelectedChannels = SelectedChannels;
            return PartialView("_ScannerConfigurations");
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

        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult FormBuilder(string[] title, string[] controlType, string[] dataType, string[] defaultValue, string[] maxLength, string[] required, string[] isAttachments, int[] sortOrder, string formTypeId, string definationTypeId, int[] formid, int[] isdeleted,string[] comment)
        {
            AV_SiteScriptFormEntry nodeProp = new AV_SiteScriptFormEntry();
            nodeProp.NodeTypeId = decimal.Parse(definationTypeId);
            // NodesPropertiesBL.Manage("Delete", nodeProp);
            if (title != null)
            {
                for (int i = 0; i < title.Length; i++)
                {
                    if (title[i] != null && dataType[i] != null && maxLength[i] != null)
                    {
                        nodeProp = new AV_SiteScriptFormEntry
                        {
                            NodeTypeId = decimal.Parse(definationTypeId),
                            Title = title[i],
                            DataType = dataType[i],
                            DefaultValue = defaultValue[i],
                            MaxLength = maxLength[i],
                            Required = required[i] == "true" ? "true" : "false",
                            SortOrder = sortOrder[i],
                            FormId = formid[i],
                            IsDeleted = isdeleted[i]
                          
                        };
                        NodesPropertiesBL.Manage("Insert", nodeProp);
                    }
                }
                return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
            }
            // return RedirectToAction("FormBuilder", "Template");
            return Json(new { Status = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult SaveScannerConfiguration(int[] kpiId, string[] kpiValue, int KeyId = 0, int MeasurementId = 0)
        {
            AV_SiteScriptScannerConfigurations scConfig = new AV_SiteScriptScannerConfigurations();
            try
            {
                if (kpiId != null)
                {
                    for (int i = 0; i < kpiId.Length; i++)
                    {
                            scConfig = new AV_SiteScriptScannerConfigurations
                            {
                                SiteScriptId = KeyId,
                                MeasurementId = MeasurementId,
                                KpiId = kpiId[i],
                                KpiValue = kpiValue[i]
                            };
                            SiteScriptScannerConfigBL.Manage("Insert", scConfig);
                    }
                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

            }
            
            return Json(new { Status = false }, JsonRequestBehavior.AllowGet);
        }

    }
}