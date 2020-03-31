
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.Security.Filters;
using System.Web.Mvc;
using System;
using SWI.AirView.Common;
using System.Collections.Generic;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using System.Text;
using WebApplication.Models;
using walkDirectory;
using System.IO;
using AirView.DBLayer.AirView.BLL;
using System.Data;
using System.Xml;
using Library.SWI.Survey.BLL;
using AirView.DBLayer.Survey.BLL;
using AirView.DBLayer.Template.BLL;
using System.Linq;
using System.IO.Compression;
using System.Web.Configuration;

namespace WebApplication.Controllers
{
    [IsLogin]
    public class SiteDashboardController : Controller
    {
        // GET: SiteDashboard
        // GET: /SiteDashboard/Index
        private TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
        private SitesBL loSitesBL = new SitesBL();
        public ActionResult Index(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            string ModuleKeyCode = "MD_SITE";
            var ProjectId = TemplatesBL.ToList("GetProjectIdBySiteId", id.ToString()).FirstOrDefault();
            if(ProjectId != null)
            {
                var templateData = TemplatesBL.ToList("IsTemplateExist", ModuleKeyCode).Where(x => x.ProjectId == ProjectId?.ProjectId).FirstOrDefault();
                if (templateData != null && ProjectId != null)
                {
                    return Redirect($"/Project/Template/Dashboard?Id={templateData.TemplateId}&ProjectId={templateData.ProjectId}");
                }
            }

            ViewBag.SiteId = id;
            AD_DefinationBL db = new AD_DefinationBL();
            ViewBag.WoStatus = db.ToList("WO Status");
            WebConfig wc = new WebConfig();
            ViewBag.domain = wc.AppSettings("Domain");
            AV_GetSiteDashboardInfoBL sdb = new AV_GetSiteDashboardInfoBL();
            var rec = sdb.GetDataSet(id, 0, 0, 0, 0, "Dashboard_Site_All");

            ViewBag.PingThroughtputChart = rec.PingThroughtput;
            ViewBag.DLThroughtputChart = rec.DLThroughtput;
            ViewBag.ULThroughtputChart = rec.ULThroughtput;

            ViewBag.GmapsKey = WebConfigurationManager.AppSettings["ApiMapKey"];
            return View(rec);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult SiteGrid(int Page, int SiteId, string filter)
        {
            DashboardVM vm = new DashboardVM();
            DashboardBL loDashboardBL = new DashboardBL();
            try
            {
               
                //SINGLE_SITE
                //SiteDashboard
                int offset = (Page - 1) * 5;
                vm = loDashboardBL.GetDashboardSiteVM(null, null, DateTime.Now, DateTime.Now, null, SiteId, null, 0, ViewBag.UserId, filter, offset, 5, "0");
                ViewBag.Show = "no";
                ViewBag.view = "SiteDashboard";
            
                foreach (var item in vm.ClientSites.Sites)
                {
                    ViewBag.Latitude = item.Latitude;
                    ViewBag.Longitude = item.Longitude;
                    break;
                }

         
                return PartialView("~/Views/Dashboard/_SiteGrid.cshtml", vm.ClientSites);
            }
            catch (System.Exception)
            {

                return null;
            }

        }

        [IsLogin(CheckPermission = false)]
        public ActionResult getLeftPanel(int Page, int SiteId, string filter)
        {
            DashboardVM vm = new DashboardVM();
            ViewBag.SiteIds = SiteId;
            DashboardBL loDashboardBL = new DashboardBL();
            try
            {
                //SINGLE_SITE
                //SiteDashboard
                int offset = (Page - 1) * 5;
                vm = loDashboardBL.GetDashboardSiteVM(null, null, DateTime.Now, DateTime.Now, null, SiteId, null, 0, ViewBag.UserId, filter, offset, 5, "0");
                string scope="";
               

                foreach (var item in vm.ClientSites.Sites)
                {
                    scope = item.Scope;
                }

               
                if (scope == "SSV" || scope == "NI" || scope == "IND")
                {
                   
                    List<BandVM> lstBands = loSitesBL.GetSiteBands("", SiteId);
                    return PartialView("_left_panel_ssv_ni_ind", lstBands);
                }
                else if (scope == "TSS")
                {
                    TSS_VMBL vmb = new TSS_VMBL();
                    var TSS = vmb.ToList("", SiteId.ToString());
                    return PartialView("~/Views/Dashboard/_SiteGridTSS.cshtml", TSS);
                }
                else if (scope == "CLS")
                {

                    CLS_VMBL vmb = new CLS_VMBL();
                    var CLS = vmb.ToList("", SiteId.ToString());
                    return PartialView("~/Views/Dashboard/_SiteGridCLS.cshtml", CLS);
                }
                else
                {
                    return Content("No Scope");
                }
               
            }
            catch (System.Exception)
            {

                return null;
            }
          
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult SingleSiteData(string Filter, Int64 SiteId, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId, string Sector)
        {
            Response r = new Response();
            AV_GetSiteDashboardInfo rec = new AV_GetSiteDashboardInfo();
            try
            {
                AV_GetSiteDashboardInfoBL sdb = new AV_GetSiteDashboardInfoBL();

                if (Filter == "Dashboard_Site_Sector")
                {
                    rec = sdb.GetSectorDataSet(SiteId, NetworkModeId, BandId, CarrierId, ScopeId, Sector, Filter);
                }
                else
                {
                    rec = sdb.GetDataSet(SiteId, NetworkModeId, BandId, CarrierId, ScopeId, Filter);
                    TempData["HandoverStatus"] = rec.HandoverStatus;
                    TempData["MOMTStatus"] = rec.MOMTStatus;
                    TempData["TeamMembers"] = rec.TeamMember;

                    TempData["OoklaResult"] = rec.OoklaTestResult;
                }

                TempData["PingThroughtput"] = rec.PingThroughtput;
                TempData["DLThroughtput"] = rec.DLThroughtput;
                TempData["ULThroughtput"] = rec.ULThroughtput;

                TempData["SiteSectorInfo"] = rec.SiteSectorInfo;

                //TempData["graphDataMTMOSMOSMT"] = rec.GraphDataMTMOSMOSMT;

                r.Status = "success";
                r.Message = "success";
            }
            catch (Exception ex)
            {

                r.Status = "error";
                r.Message = ex.Message;
            }

            return Json(rec, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }



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

        [HttpGet]
        
        public PartialViewResult getDirectioryWithFiles(string getNetworkLayer,string Scope="")
        {
            List<GetDirctoryinformation> singdir=new List<GetDirctoryinformation>();
            //  string absolutePath = Server.MapPath("/Content/AirViewLogs/TMO");
            if (Scope == "TSS")
            {
                string absolutePath = Server.MapPath("~/Content/AirViewLogs/" + getNetworkLayer);
                string path = "";
                if (Directory.Exists(absolutePath))
                {
                    path = absolutePath;
                    DirectoryInfo objDirectoryInfo = new DirectoryInfo(path);
                    FileInfo[] allFiles = objDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
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
                }
                else
                {
                    string SiteCode = getNetworkLayer.Split('/')[1];
                    path = Server.MapPath("~/Content/AirviewLogs/TMO/WEB/" + SiteCode);
                    if (Directory.Exists(path))
                    {
                        DirectoryInfo objDirectoryInfo = new DirectoryInfo(path);
                        FileInfo[] allFiles = objDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
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
                    }
                }
                // Collecting All Files 
              

            }
            else
            {
                string absolutePath = Server.MapPath("~/Content/AirViewLogs/" + getNetworkLayer);
                System.IO.DirectoryInfo files = new System.IO.DirectoryInfo(absolutePath);
                singdir = walkDirectoryByTree.WalkDirectoryTree(files);

            }

            return PartialView("~/Views/SiteDashboard/RenderFilesOfDirectoryBaseOnNetWorklayer.cshtml", singdir);
        }
        [HttpGet]
        [IsLogin(CheckPermission = false)]
        public PartialViewResult GetDirectioryWithFilesAVX(int Id=0)
        {
            List<GetDirctoryinformation> singdir = new List<GetDirctoryinformation>();
            SitesBL sb = new SitesBL();
            List<BandVM> Bands = sb.GetSiteBands("", Id);
            if (Bands.Count()>0)
            {
                var band = Bands.FirstOrDefault();
                var sPath = Server.MapPath("~/Content/AirViewLogs/" + band.ClientPrefix + "\\" + band.SiteCode);
                if (Directory.Exists(sPath))
                {
                    FileInfo[] allFiles = new DirectoryInfo(sPath).GetFiles("*.avx", SearchOption.AllDirectories);
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
                }
            }
            return PartialView("~/Views/SiteDashboard/RenderFilesOfDirectoryBaseOnWorkorder.cshtml", singdir);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult DownloadAttachment(string filesource)
        {
            // Find user by passed id
            // Student student = db.Students.FirstOrDefault(s => s.Id == studentId);



            byte[] fileBytes = System.IO.File.ReadAllBytes(filesource);
            var filename = Path.GetFileName(filesource);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);

        }
       
        
        [HttpGet]
        [IsLogin(CheckPermission = false)]
        public ActionResult getFilesText(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath);

            if (fileExtension == ".json")
            {
                string jsons;
                using (StreamReader r = new StreamReader(filePath))
                {
                    jsons = r.ReadToEnd();

                }
                return Json(new { key = true, fileTxt = jsons, fileType = fileExtension }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            else if (fileExtension == ".xml")
            {
                string rawStringXML = System.IO.File.ReadAllText(filePath);
               
                //XmlDocument xmlDoc = new XmlDocument();
                //StringWriter sw = new StringWriter();
                //xmlDoc.LoadXml(rawStringXML);
                //xmlDoc.Save(sw);
                //string formattedXml = sw.ToString();
                return Json(new { key = true, fileTxt = rawStringXML.ToString(), fileType = fileExtension }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            else if (fileExtension == ".txt")
            {
                string xmlString = System.IO.File.ReadAllText(filePath);
                return Json(new { key = true, fileTxt = xmlString, fileType = fileExtension }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            else if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension==".jpeg" || fileExtension==".mp4" || fileExtension==".mp3" )
            {
                string exactPath = Path.GetFullPath(filePath);
                var spliturl = filePath.Split(new string[] { "Content\\" }, StringSplitOptions.None);
                Uri file = new Uri(filePath);
                // Must end in a slash to indicate folder
                Uri folder = new Uri(spliturl[0]);
                string relativePath =
                Uri.UnescapeDataString(
                    folder.MakeRelativeUri(file)
                        .ToString()
                        .Replace('/', Path.DirectorySeparatorChar)
                    );

                return Json(new { key = true, fileTxt = "\\" + relativePath, fileType = fileExtension }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            else if (fileExtension == ".csv")
            {
                DataTable csvorTsv = csvTsvFile.GetCSVData(filePath);
                ViewBag.Data = csvorTsv;
                ViewBag.title = "Csv";
                return PartialView("_csvOrTsvFile");
            }
            else if (fileExtension == ".tsv")
            {
                DataTable csvorTsv = csvTsvFile.GetTsvData(filePath);
                ViewBag.Data = csvorTsv;
                ViewBag.title = "Tsv";
                return PartialView("_csvOrTsvFile");
            }
            else if (fileExtension == ".kml" || fileExtension == ".zip" || fileExtension == ".nmf" || fileExtension == ".gpx" || fileExtension==".avx" )
            {
                return Json(new { key = true, fileTxt = "", fileType = fileExtension }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }


            return Json(new { key = false, fileTxt = "" }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult HandOverStatus() {

            if (TempData["HandoverStatus"]!=null)
            {
                try
                {
                    var rec = TempData["HandoverStatus"] as List<HandoverStatus>;
                    return PartialView("~/Views/SiteDashboard/_HandOverStatus.cshtml", rec);
                }
                catch 
                {

                    return null;
                }
               
            }
            return null;
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult MOMTStatus()
        {

            if (TempData["MOMTStatus"] != null)
            {
                try
                {
                    var rec = TempData["MOMTStatus"] as List<MOMTStatus>;
                    return PartialView("~/Views/SiteDashboard/_MOMTStatus.cshtml", rec);
                }
                catch
                {

                    return null;
                }

            }
            return null;
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult OoklaResult()
        {

            if (TempData["OoklaResult"] != null)
            {
                try
                {
                    var rec = TempData["OoklaResult"] as List<OoklaTestResult>;
                    return PartialView("~/Views/SiteDashboard/_OoklaResult.cshtml", rec);
                }
                catch
                {

                    return null;
                }

            }
            return null;
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult TeamMembers()
        {

            if (TempData["TeamMembers"] != null)
            {
                try
                {
                    var rec = TempData["TeamMembers"] as List<SiteDashboardTeamMember>;
                    return PartialView("~/Views/SiteDashboard/_TeamMembers.cshtml", rec);
                }
                catch
                {

                    return null;
                }

            }
            return null;
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult PingThroughtput(string Filter)
        {

            if (TempData["PingThroughtput"] != null)
            {
                try
                {
                    var rec = TempData["PingThroughtput"] as List<SiteDashboardThroughtputChart>;
                    ViewBag.type = Filter;
                    return PartialView("~/Views/SiteDashboard/_PingThroughtput.cshtml", rec);
                }
                catch
                {

                    return null;
                }

            }
            return null;
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult GraphMTMOSMOSMT()
        {

            if (TempData["graphDataMTMOSMOSMT"] != null)
            {
                try
                {
                    List<groupByTestType> rec = TempData["graphDataMTMOSMOSMT"] as List<groupByTestType>;
                    return Json(new { key = true, MOSMOMTSMT = rec }, JsonRequestBehavior.AllowGet);
                }
                catch
                {

                    return Json(new { key = false }, JsonRequestBehavior.AllowGet);
                }

            }
            return null;
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult DLThroughtput(string Filter)
        {

            if (TempData["DLThroughtput"] != null)
            {
                try
                {
                    ViewBag.type = Filter;
                    var rec = TempData["DLThroughtput"] as List<SiteDashboardThroughtputChart>;
                    return PartialView("~/Views/SiteDashboard/_DownlinkThroughtput.cshtml", rec);
                }
                catch
                {

                    return null;
                }

            }
            return null;
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult ULThroughtput(string Filter)
        {
           
            if (TempData["ULThroughtput"] != null)
            {
                try
                {
                    ViewBag.type = Filter;
                    var rec = TempData["ULThroughtput"] as List<SiteDashboardThroughtputChart>;
                    return PartialView("~/Views/SiteDashboard/_UplinkThroughtput.cshtml", rec);
                }
                catch
                {

                    return null;
                }

            }
            return null;
        }
    }
}