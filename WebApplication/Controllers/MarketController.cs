using SWI.Libraries.Common;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.DAL;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Controllers;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using SWI.Libraries.Security.Entities;
using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.AirView.Entities;
using MoreLinq;

namespace SWI.AirView.Controllers
{
    [IsLogin, ErrorHandling]
    public class MarketController : Controller
    {
        // GET: Market
        public ActionResult Configuration(int Id = 0)
        {
            ViewBag.CityId = Id;
            AD_DefinationBL db = new AD_DefinationBL();
            if (ViewBag.IsAdmin)
            {
                ViewBag.Cities = db.SelectedList("AllCities");
            }
            else
            {
                ViewBag.Cities = db.SelectedList("UserCities", Convert.ToString(ViewBag.UserId));
            }

            Sec_UserBL ubl = new Sec_UserBL();
            ViewBag.Users = ubl.SelectedList("All", null);

            ViewBag.MarketUsers = ubl.SelectedList("ByCityId", Id.ToString());

            ViewBag.NetworkModes = db.SelectedList("NetworkModes", null, "-Select NetworkMode-");


            UserClientsBL ucb = new UserClientsBL();
            ViewBag.UserClients = ucb.SelectedList("byUserId", Convert.ToString(ViewBag.UserId));
            ViewBag.Projects = db.SelectedList("UserProjects", Convert.ToString(ViewBag.UserId));

            ViewBag.ReportTypes = db.SelectedList("ReportTypes", null, "-Report Types-");

            ViewBag.Scopes = db.SelectedList("byDefinationType", "Scope");
            ViewBag.PlotTypes = db.SelectedList("RFPlotTypes");


            return View();
        }
        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult CityUsers(string CityId, string[] Users)
        {
            Response res = new Response();
            foreach (var city in CityId.Split(','))
            {
                try
                {
                    DataTable Data = new DataTable();
                    Data.Columns.AddRange(new DataColumn[2]
                    {
                                    new DataColumn("Id", typeof (int)),
                                    new DataColumn("value", typeof (string))


                    });


                    foreach (var item in Users)
                    {
                        DataRow row;
                        row = Data.NewRow();
                        row["Id"] = Convert.ToInt32(city);
                        row["value"] = item;

                        Data.Rows.Add(row);
                    }

                    UserCityDL uctd = new UserCityDL();
                    uctd.Insert("CityUsers", Convert.ToInt32(city), Data);

                    res.Status = "success";
                    res.Message = "save successfully";
                }
                catch (Exception ex)
                {

                    res.Status = "danger";
                    res.Message = ex.Message;
                }

            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult ReportTemplateView()
        {
            return PartialView();
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult GetWorkOrders(string q)
        {
            AD_DefinationBL db = new AD_DefinationBL();
            return Json(new { items = db.SelectedList("getWorkOrders", q).Select(x => new { text = x.Text, id = x.Value }) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult LoadFiles(string SiteCode)
        {
            var ext = new List<string> { "avx" };
            var dir = Server.MapPath("~/Content/AirViewLogs/TMO/" + SiteCode.ToString());
            //Dictionary<string, string> data = new Dictionary<string, string>();
            List<FileList> data = new List<FileList>();
            bool isDirectoryExists = Directory.Exists(dir);
            if (isDirectoryExists)
            {
                DirectoryInfo d = new DirectoryInfo(dir);//directory/Folder
                FileInfo[] Files = d.GetFiles("*.avx", SearchOption.AllDirectories); //Getting avx files
                foreach (FileInfo file in Files)
                {
                    FileList fList = new FileList();
                    StringBuilder fileName = new StringBuilder();
                    List<string> names = file.Name.Split('_').ToList();
                    for (int i = 0; i < names.Count; i++)
                    {
                        if (names[i] != SiteCode)
                        {
                            if (i != 0)
                            {
                                fileName.Append("_");
                            }
                            fileName.Append(names[i]);
                        }
                        else
                        {
                            break;
                        }
                    }
                    fList.Name = fileName.ToString();
                    fList.Path = file.FullName.Split(new []{ "Content" },StringSplitOptions.None)[1];
                    data.Add(fList);
                }
            }
            return Json(new { items = data.Select(x => new { id = x.Name, text = x.Name , title = "",path=x.Path }).DistinctBy(x=>x.id).ToList() });
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult SaveReportTemplate(long ClientId, long ProjectId, string MarketId, string ReportTemplateName)
        {
            // Check For Existing Report Template
            string Filter = "NewReportTemplate";
            var user = Session["user"] as LoginInformation;
            AV_MarketConfigurationBL mc = new AV_MarketConfigurationBL();
            var response = mc.SaveDataTemplate(Filter, MarketId, 0, ClientId, 0, null, null, ProjectId, ReportTemplateName, null, null, 0, user.UserId);
            if (response == "Success")
            {
                return Json("Saved Successfully");
            }
            else
            {
                return Json(response);
            }


        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult SaveTemplate(long ReportTypeId, long ScopeId,
                                           string TemplateName, string Files,
                                           int ReportTemplateId, MainClass Model, List<string> Keys)
        {
            TemplateName = TemplateName.Replace(' ', '_');
            bool exists = CheckExistence(TemplateName);
            if (!exists)
            {
                ParserController PC = new ParserController();
                var filepath = "";
                string keystring = string.Join(",", Keys.ToArray());
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonData = js.Serialize(Model);
                var user = Session["user"] as LoginInformation;

                DataTable dt = PC.getData(Model, filepath);
                var dtColumns = dt.Columns;
                AV_MarketConfigurationBL mc = new AV_MarketConfigurationBL();
                string Filter = "NewTemplate";
                var response = mc.SaveDataTemplate(Filter, null, ReportTypeId, 0, ScopeId, TemplateName, Files, 0, null, jsonData, keystring, ReportTemplateId, user.UserId);
                bool IsUpdate = false;
                bool IsCreated = mc.CreateDataTemplateTable(IsUpdate, TemplateName, dtColumns);
                if (IsCreated)
                {
                    // Insert Data To Table 
                    //mc.SaveDataToDataBase(dt, TemplateName);

                    return Json("Template Created Successfully");
                }
                else
                {
                    return Json("SomeThing Went Wrong");
                }

            }
            else
            {
                return Json("Template name already exists!");
            }
        }

        public ActionResult RFLegends()
        {
            return PartialView();
        }
        [HttpPost IsLogin(CheckPermission = false)]
        public ActionResult RFLegends(FormCollection frm)
        {
            AV_MarketConfigurationBL mcbl = new AV_MarketConfigurationBL();
            var res = mcbl.SaveRFPlotLegend(frm);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public bool CheckExistence(string TemplateName)
        {
            AV_MarketConfigurationBL mc = new AV_MarketConfigurationBL();
            return mc.checkExistence(TemplateName);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetReportTemplates(string MarketId = "", long ClientId = 0, long ProjectId = 0)
        {
            AV_MarketConfigurationBL db = new AV_MarketConfigurationBL();
            var res = db.SelectedList("getReportTemplates", MarketId, ClientId, ProjectId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetDataTemplates(int id)
        {
            if (id != 0)
            {
                AD_DefinationBL db = new AD_DefinationBL();
                var res = db.SelectedList("getDataTemplates", id.ToString());
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetTemplateById(int id)
        {
            if (id != 0)
            {
                AV_MarketConfigurationBL bl = new AV_MarketConfigurationBL();
                var res = bl.GetTemplateById("GetTemplateById", id);
                return Json((res.Rows[0]["Keys"]).ToString().Split(','), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult UpdateTemplate(MainClass Model, List<string> Keys,
                                           int Value = 0, string TemplateName = "")
        {
            ParserController PC = new ParserController();
            var filepath = "";
            string keystring = string.Join(",", Keys.ToArray());
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(Model);

            DataTable dt = PC.getData(Model, filepath);
            var dtColumns = dt.Columns;
            AV_MarketConfigurationBL mc = new AV_MarketConfigurationBL();
            string Filter = "UpdateTemplate";
            var response = mc.SaveDataTemplate(Filter, null, 0, 0, 0, null, null, 0, null, jsonData, keystring, Value, 0);
            bool IsUpdate = true;
            bool IsCreated = mc.CreateDataTemplateTable(IsUpdate, TemplateName, dtColumns);
            if (IsCreated)
            {
                // Insert Data To Table 
                // mc.SaveDataToDataBase(dt, TemplateName);

                return Json("Template Updated Successfully");
            }
            else
            {
                return Json("SomeThing Went Wrong");
            }
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult DeleteTemplateById(int DataTemplateId)
        {
            AV_MarketConfigurationBL bl = new AV_MarketConfigurationBL();
            string Filter = "DeleteDataTemplateById";
            var res = bl.DeleteDataTemplate(Filter, DataTemplateId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        

        [IsLogin(CheckPermission = false)]
        public bool FileDataParser(long SiteId)
        {
            // Parse File Here
            try
            {
                AV_MarketConfigurationBL bl = new AV_MarketConfigurationBL();

                DataTable WO = bl.GetWorkOrderById("GetWorkOrderById", SiteId);

                long ClientId = Convert.ToInt64(WO.Rows[0]["ClientId"]);
                long ProjectId = Convert.ToInt64(WO.Rows[0]["ProjectId"]);
                long MarketId = Convert.ToInt64(WO.Rows[0]["MarketId"]); ;
                long ScopeId = Convert.ToInt64(WO.Rows[0]["ScopeId"]);
                string ClientPrefix = WO.Rows[0]["ClientPrefix"].ToString();
                string SiteCode = WO.Rows[0]["SiteCode"].ToString();

                DataTable dt = bl.GetDataTemplate("GetDataTemplate", ClientId, ProjectId, MarketId, ScopeId);
                if (dt.Rows.Count> 0)
                {
                    for (int p = 0; p < dt.Rows.Count; p++)
                    {
                        string Files = dt.Rows[p]["Files"].ToString();
                        string Json = dt.Rows[p]["JsonData"].ToString();
                        string TemplateName = dt.Rows[p]["DataTemplateName"].ToString();

                        List<string> ListofFiles = Files.Split(',').ToList();

                        //var dir = Server.MapPath("~/Content/AirViewLogs/" + ClientPrefix + "\\" + SiteCode);
                        var dir = Path.Combine(HttpRuntime.AppDomainAppPath, "Content/AirViewLogs/" + ClientPrefix + "/" + SiteCode);
                        bool isDirectoryExists = Directory.Exists(dir);
                        if (isDirectoryExists)
                        {
                            DirectoryInfo d = new DirectoryInfo(dir);//directory/Folder
                            FileInfo[] directoryFiles = d.GetFiles("*.avx", SearchOption.AllDirectories); //Getting avx files
                            //List<FileInfo> filteredFiles = CheckForAlreadyProcessedFiles(directoryFiles, SiteId);
                            foreach (var file in directoryFiles)
                            {
                                StringBuilder fileName = new StringBuilder();
                                List<string> names = file.Name.Split('_').ToList();
                                for (int i = 0; i < names.Count; i++)
                                {
                                    if (names[i] != SiteCode)
                                    {
                                        if (i != 0)
                                        {
                                            fileName.Append("_");
                                        }
                                        fileName.Append(names[i]);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                foreach (var filename in ListofFiles)
                                {
                                    if (fileName.ToString() == filename)
                                    {

                                        AV_MarketConfigurationBL mc = new AV_MarketConfigurationBL();
                                        string Filter = "CheckFiles";
                                        var processed = mc.CheckFiles(Filter, SiteId, file.Name, TemplateName);
                                        if (!processed)
                                        {
                                            // File is Matched with out Saved Pattern So Parse File Here
                                            MainClass Model = JsonConvert.DeserializeObject<MainClass>(Json);
                                            ParserController PC = new ParserController();
                                            var filepath = file.FullName;
                                            DataTable fileData = PC.GetParserDataFromFile(Model, filepath);
                                            mc.SaveDataToDataBase(fileData, TemplateName, SiteId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //public List<FileInfo> CheckForAlreadyProcessedFiles(FileInfo[] files, long SiteId)
        //{
        //    AV_MarketConfigurationBL bl = new AV_MarketConfigurationBL();
        //    List<FileInfo> filteredFiles = new List<FileInfo>();
        //    string Filter = "CheckFiles";
        //    foreach (var f in files)
        //    {
        //        var processed = bl.CheckFiles(Filter, SiteId, f.Name);
        //        if (!processed)
        //        {
        //            filteredFiles.Add(f);
        //        }
        //    }
        //    return filteredFiles;
        //}
        public class TemplateNameList
        {
            public string Name { get; set; }
        }

        public class FileList
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }


    }
}