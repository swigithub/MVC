using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.AirView.BLL;
using SWI.Security.Filters;
using SWI.Libraries.Common;
using SWI.AirView.Models;
using SWI.AirView.Common;
using OfficeOpenXml;
using SWI.Libraries.AD.BLL;
using System.Web;
using System.Threading.Tasks;
using System.Threading;
using AirView.DBLayer.Survey.BLL;
using SWI.Libraries.Security.Entities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.AirView.BLL;
using System.Net;
using System.Web.Services;
using AirView.DBLayer.Fleet.BLL;
using AirView.DBLayer.Fleet.Model;
using AirView.DBLayer.Template.BLL;
using SWI.Libraries.Security.DAL;
using Library.SWI.Template.Model;
using System.Web.Configuration;

namespace SWI.AirView.Controllers
{
    [IsLogin, ErrorHandling]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        private DashboardBL loDashboardBL = new DashboardBL();
        private SitesBL loSitesBL = new SitesBL();
        private TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
        public DashboardController() {

        }
      
        public async Task< ActionResult> Index()
        {

            string mapapikey = ApiMapKey();
            ViewBag.ApiMapKey = mapapikey;
            Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
            DataTable Table = udl.GetDataTable("UserProjects", Convert.ToString(ViewBag.UserId), null, null);
            var PList = Table.ToList<PM_Projects>();
            string ProjectsList = "";
            Session["IsSearch"] = false;
            for (int i = 0; i < PList.Count; i++)
            {
                if (i == 0)
                {
                    ProjectsList += PList[i].ProjectId;
                }
                else
                {
                    ProjectsList += "," + PList[i].ProjectId;
                }
            }
            string ModuleKeyCode = "MD_DRIVE_TEST";
            List<TMP_Templates> P_List = new List<TMP_Templates>();
            foreach(var item in PList)
            {
                var i = item.ProjectId;
                var t = TemplatesBL.ToList("IsTemplateExist", ModuleKeyCode).Where(x => x.ProjectId == item.ProjectId).FirstOrDefault();
                if(t != null) P_List.Add(t);
            }
            var ProjectId = P_List?.FirstOrDefault()?.ProjectId ?? 0;
            var templateData = TemplatesBL.ToList("IsTemplateExist", ModuleKeyCode).Where(x => x.ProjectId == ProjectId).FirstOrDefault();
            if (templateData != null)
            {
                return Redirect($"/Project/Template/Dashboard?Id={templateData.TemplateId}&ProjectId={templateData.ProjectId}");
            }

            DashboardVM vm = new DashboardVM();
            try
            {
                HttpCookie myCookie = Request.Cookies["DashboardFilter"];
                if (myCookie != null)
                {
                    Session["Scopes"] = myCookie.Values["Scopes"].ToString();
                    Session["Client"] = (myCookie.Values["Client"] != null) ? myCookie.Values["Client"].ToString() : "0";
                    Session["CountryId"] = (myCookie.Values["CountryId"] != null) ? myCookie.Values["CountryId"].ToString() : "0";
                    Session["Markets"] = (myCookie.Values["Markets"] != null) ? myCookie.Values["Markets"].ToString() : "0";
                    Session["Projects"] = (myCookie.Values["Projects"] != null) ? myCookie.Values["Projects"].ToString() : "0";
                }
                else
                {
                    Session["Scopes"] = "0";
                    Session["Client"] = "0";
                    Session["CountryId"] = "0";
                    Session["Markets"] = "0";
                    Session["Projects"] = "0";
                }

                Session["WoFilter"] = "Total";
                Session["DateFilter"] = "ThisMonth";
                Session["Panel1Option"] = "Market View";
                Session["Panel1Value"] = "0";
                Session["Panel2Option"] = "Drive Tester View";
                Session["Panel2Value"] = "0";
                DateFunctions df = new DateFunctions(DateTime.Now);
                Session["DateFilter"] = "ThisMonth";
                Session["FromDate"] = df.MonthStart;
                Session["ToDate"] = df.MonthEnd;
                
                //Session["Scopes"] = (TempData["Scopes"] == null) ? "0": TempData["Scopes"];
                //Session["Client"] = (TempData["Client"] == null) ? "0": TempData["Client"];
                //Session["CountryId"] = (TempData["CountryId"] == null) ? "0": TempData["CountryId"];

                AD_DefinationBL db = new AD_DefinationBL();
                ViewBag.WoStatus = db.ToList("WO Status");
                ViewBag.UserScopes = db.ToList("UserScopes",Convert.ToString( ViewBag.UserId));
                DataTable tb = udl.GetDataTable("UserProjects", Convert.ToString(ViewBag.UserId), null, null);
                ViewBag.Projects = tb.ToList<PM_Projects>();
                UserClientsBL ucb = new UserClientsBL();
                //ViewBag.UserClients =  ucb.ToList("GetCountries_UserId",Convert.ToString(ViewBag.UserId));
                
                List<UserClients> TempClientList = ucb.ToList("GetCountries_UserId", Convert.ToString(ViewBag.UserId));
                ViewBag.UserClients = TempClientList.ToList();
                ViewBag.UserClientsCountry = TempClientList.Distinct().ToList();
                WebConfig wc = new WebConfig();
                ViewBag.domain = wc.AppSettings("Domain");
                vm = loDashboardBL.GetDashboardVM(Session["WoFilter"].ToString(), Session["DateFilter"].ToString(), Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["ToDate"]), ThisUserMarkets(), Session["Panel1Option"].ToString(), Convert.ToInt64(Session["Panel1Value"]), Session["Panel2Option"].ToString(), Convert.ToInt64(Session["Panel2Value"]), ViewBag.UserId, Session["CountryId"].ToString(), Session["Client"].ToString(), Session["Scopes"].ToString(), Session["Markets"].ToString(), Session["Projects"].ToString());
                GC.Collect();
                GC.WaitForPendingFinalizers();

            }
            catch (Exception ex)
            {
                TempData["msg_error"] = ex.Message;
            }
          
            return await Task.Run(() => View(vm));
        }
        [HttpPost, ValidateAntiForgeryToken(),IsLogin(Return = "login")]
        public ActionResult Index(string Countries,string Clients,string Scopes, string Markets,string Projects)
        {
            //TempData["Scopes"] = Scopes;
            //TempData["Client"] = Clients;
            //TempData["CountryId"] = Countries;


            HttpCookie c = new HttpCookie("DashboardFilter");
            c.Values.Add("Scopes", Scopes);
            c.Values.Add("Client", Clients);
            c.Values.Add("CountryId", Countries);
            c.Values.Add("Markets", Markets);
            c.Values.Add("Projects", Projects);
            Response.SetCookie(c);

            return RedirectToAction("Index");
        }

        private string ApiMapKey()
        {
            WebConfig wc = new WebConfig();
            string MapKey = wc.AppSettings("ApiMapKey").ToString();
            return MapKey;
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult DriveRoute(int SiteId, string Scope)
        {
            ViewBag.Scope = Scope;
            ViewBag.SiteId = SiteId;
            AV_DriveRoutesBL drb = new AV_DriveRoutesBL();
            var rec = drb.ToList("BySiteId", SiteId.ToString());
            return View("~/views/Dashboard/_DriveRoute.cshtml", rec);
        }
        public ActionResult Drive(int SiteId, string Scope, string SelectedSiteCode, float Latitude, float Longitude, int SelectedScopeId, string currentClientPrefix, string filter)
        {
            ViewBag.Id = SiteId;
            ViewBag.Scope = Scope;
            ViewBag.SelectedSiteCode = SelectedSiteCode;
            ViewBag.Latitude = Latitude;
            ViewBag.Longitude = Longitude;
            ViewBag.filter = filter;
            ViewBag.SelectedScopeId = SelectedScopeId;
            ViewBag.currentClientPrefix = currentClientPrefix;
            WebConfig wc = new WebConfig();
            ViewBag.domain = wc.AppSettings("Domain");
            ViewBag.GmapsKey = WebConfigurationManager.AppSettings["ApiMapKey"];
            return View();
        }


        [IsLogin(CheckPermission =false), HttpPost]
        public JsonResult DriveRoute(List<Cordinates> cordinates, Int64 SiteId, string SiteCode, string TestType, int ScopeId, Int64 RouteId, string Filter, string ClientPrefix, string Delete, List<Cordinates> PathJson)
        {
            Response res = new Response();
            DirectoryHandler dh = new DirectoryHandler();

            string CompleteKml = null;
            KML km = new KML();
            try
            {
                CompleteKml += km.Open("Routes", "Routes For Site");

                int count = 1;
                #region Existing File Coordinates

                string Path = "/Content/AirViewLogs/" + ClientPrefix + "/" + SiteCode;
                if (dh.FileExist(Server.MapPath("~" + Path + "/route-" + RouteId + ".kml")))
                {
                    string text;
                    var fileStream = new FileStream(Server.MapPath("~" + Path + "/route-" + RouteId + ".kml"), FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        text = streamReader.ReadToEnd();
                    }
                    MyString ms = new MyString();
                    string MyPlacemarks = "";
                    string[] MyPlacemarksSplit;
                    int MyIndex = 5782;
                    if (Delete != "")
                    {
                        MyPlacemarks = ms.BetweenTag(text, "Placemark", "&");
                        MyPlacemarksSplit = MyPlacemarks.Split('&');
                        int counter = 0;

                        foreach (var item in MyPlacemarksSplit)
                        {
                            if (item.Contains(Delete))
                                MyIndex = counter;
                            counter++;


                        }
                    }

                    string Cordinate = ms.BetweenTag(text, "coordinates", "&");
                    string Colors = ms.BetweenTag(text, "color", "&");
                    string[] ColorsArr = Colors.Split('&');
                    //  Cordinate = Cordinate.Replace(",0", ",");
                    //Cordinate = Cordinate.Replace(",-", "-");
                    //Cordinate = Cordinate.Replace("-", ",-");
                    //Cordinate = Cordinate.Replace(",,", ",");
                    string[] Tags = Cordinate.Split('&');
                    string cords; ;
                    string[] Cordinates;

                    for (int i = 0; i < Tags.Length; i++)
                    {
                        if (Delete == "" || (i != MyIndex && MyIndex != 5782))
                        {
                            if (Tags[i].Trim().Length > 0)
                            {
                                cords = null;
                                Cordinates = Tags[i].Split('\n');
                                for (int j = 0; j < Cordinates.Length; j++)
                                {
                                    if (Cordinates[j].Trim().Length > 0)
                                    {
                                        cords += Cordinates[j] + "0\n";
                                    }
                                }
                                CompleteKml += km.Style("LineId" + count, "LineStyle", "color", "FFA9A9A9", "width", "4");
                                CompleteKml += km.Placemark("LineId" + count, "LineId" + count, "LineString", "relative", cords);

                                count++;
                            }
                        }

                    }
                }

                #endregion


                if (cordinates != null || Delete != "")
                {
                    if (Delete == "")
                    {
                        //  foreach (var jk in cordinates)
                        // {
                        string cords = null;

                        string plotColor = "FFA9A9A9"; //(!string.IsNullOrEmpty(r.Color)) ? r.Color.Replace("#", "") :
                        plotColor = "ff" + plotColor.Substring(4, 2) + plotColor.Substring(2, 2) + plotColor.Substring(0, 2);

                        CompleteKml += km.Style("LineId" + count, "LineStyle", "color", plotColor, "width", "4");
                        foreach (var cor in cordinates)
                        {

                            cords += cor.location.lng + "," + cor.location.lat + ",0\n";
                        }
                        CompleteKml += km.Placemark("LineId" + count, "LineId" + count, "LineString", "relative", cords);
                        // CompleteKml += km.Placemarks("LineId" + count, "#LineStyle", "relativeToGround", cords);
                        count++;
                        // }
                    }

                    AV_DriveRoutesBL drb = new AV_DriveRoutesBL();
                    AV_DriveRoutes dr = new AV_DriveRoutes();
                    dr.CreatedDate = DateTime.Now;
                    dr.RouteId = RouteId;
                    dr.SiteId = Convert.ToInt64(SiteId);
                    dr.CreatedBy = ViewBag.UserId;
                    dr.RoutePath = Path;
                    if (TestType.Length > 1)
                    {
                        dr.TestType = TestType.Remove(TestType.Length - 1);
                    }

                    dr.ScopeId = ScopeId;
                    RouteId = drb.Manage(Filter, dr);


                    if (RouteId > 0)
                    {
                        CompleteKml += km.Close();
                        dh.CreateDirectory(Server.MapPath(Path));
                        km.SaveKml2(CompleteKml, "route-" + RouteId, Server.MapPath(Path));
                        if (PathJson != null)
                        {
                            string jsonpath = "~" + Path;
                            string fname = HttpContext.Server.MapPath(jsonpath);
                            if (!Directory.Exists(fname))
                            { // if it doesn't exist, create

                                System.IO.Directory.CreateDirectory(fname);
                            }
                            string json = JsonConvert.SerializeObject(PathJson.ToArray());

                            //write string to file
                            System.IO.File.WriteAllText(fname + "/route-" + RouteId + ".txt", json);
                        }
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    res.Status = "success";
                    if (Delete == "")
                    {
                        res.Message = "Drive Route Planned Successfully.";
                    }
                    else { res.Message = "Drive Route Delete Successfully."; }
                }
                else
                {
                    res.Status = "danger";
                    res.Message = "No Route Selected.";
                }

            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }



        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult SiteFilters(string FilterType, string Filter,string value)
        {
            DashboardVM vm = new DashboardVM();
            try
            {
                Session["IsSearch"] = false;
                if (FilterType == "Status")
                {
                    Session["WoFilter"] = Filter;
                    string SessionData = Session["DateFilter"].ToString();
                    Session["DateFilter"] = SessionData;
                }
                else if (FilterType == "Date")
                {
                    #region DateFilter
                    Session["DateFilter"] = "Total";

                    if (Filter == "Today")
                    {
                        Session["FromDate"] = DateTime.Now;
                        Session["ToDate"] = DateTime.Now; ;
                    }
                    else if (Filter == "TodayNext")
                    {
                        Session["FromDate"] = Convert.ToDateTime(Session["FromDate"].ToString()).AddDays(1);
                        Session["ToDate"] = Convert.ToDateTime(Session["ToDate"].ToString()).AddDays(1);
                    }
                    else if (Filter == "TodayPrev")
                    {
                        Session["FromDate"] = Convert.ToDateTime(Session["FromDate"].ToString()).AddDays(-1);
                        Session["ToDate"] = Convert.ToDateTime(Session["ToDate"].ToString()).AddDays(-1);
                    }
                    else if (Filter == "ThisWeek")
                    {
                        DateFunctions df = new DateFunctions(DateTime.Now, DayOfWeek.Sunday);

                        Session["FromDate"] = df.WeekStart;
                        Session["ToDate"] = df.WeekEnd;
                    }
                    else if (Filter == "WeekPrev")
                    {
                        DateFunctions df = new DateFunctions(Convert.ToDateTime(Session["FromDate"].ToString()).AddDays(-7), DayOfWeek.Sunday);
                        Session["FromDate"] = df.WeekStart;
                        Session["ToDate"] = df.WeekEnd;
                    }
                    else if (Filter == "WeekNext")
                    {
                        DateFunctions df = new DateFunctions(Convert.ToDateTime(Session["ToDate"].ToString()).AddDays(7), DayOfWeek.Sunday);

                        Session["FromDate"] = df.WeekStart;
                        Session["ToDate"] = df.WeekEnd;
                    }

                    else if (Filter == "ThisMonth")
                    {
                        DateFunctions df = new DateFunctions(DateTime.Now);

                        Session["FromDate"] = df.MonthStart;
                        Session["ToDate"] = df.MonthEnd;
                    }
                    else if (Filter == "MonthPrev")
                    {
                        DateFunctions df = new DateFunctions(Convert.ToDateTime(Session["FromDate"].ToString()).AddMonths(-1));
                        Session["FromDate"] = df.MonthStart;
                        Session["ToDate"] = df.MonthEnd;
                    }
                    else if (Filter == "MonthNext")
                    {
                        DateFunctions df = new DateFunctions(Convert.ToDateTime(Session["ToDate"].ToString()).AddMonths(1));
                        Session["FromDate"] = df.MonthStart;
                        Session["ToDate"] = df.MonthEnd;
                    }

                    #endregion

                }

                else if (FilterType == "RegionFilterValue")
                {
                    Session["Panel1Value"] = Filter;
                }
                else if (FilterType == "RegionFilter")
                {
                    Session["Panel1Value"] = 0;
                    Session["Panel1Option"] = Filter;
                }
                else if (FilterType == "Panel2Filter")
                {
                    Session["Panel2Option"] = Filter;
                    Session["Panel2Value"] = "0";
                }
                else if (FilterType == "Panel2Value")
                {
                    Session["Panel2Value"] = Filter;
                }
                else if (FilterType == "Range")
                {
                    Session["DateFilter"] = "DateRange";
                    Session["FromDate"] = Filter;
                    Session["ToDate"] = value;
                }
                else if (FilterType == "Country")
                {
                    Session["CountryId"] = Filter;
                }
                else if (FilterType == "Client")
                {
                    Session["Client"] = Filter;
                }
                else if (FilterType == "Scope")
                {
                    Session["Scopes"] = Filter;
                }
                else if (FilterType == "Market")
                {
                    Session["Markets"] = Filter;
                }
                else if (FilterType == "Projects")
                {
                    Session["Projects"] = Filter;
                }
                vm = loDashboardBL.GetDashboardVM(Session["WoFilter"].ToString(), Session["DateFilter"].ToString(), Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["ToDate"]), ThisUserMarkets(), Session["Panel1Option"].ToString(), Convert.ToInt64(Session["Panel1Value"]), Session["Panel2Option"].ToString(), Convert.ToInt64(Session["Panel2Value"]), ViewBag.UserId, Session["CountryId"].ToString(), Session["Client"].ToString(), Session["Scopes"].ToString(), Session["Markets"].ToString(), Session["Projects"].ToString());

                ViewBag.SiteCount = vm.ClientSites.Sites.Count();

                TempData["SiteGrid"] = vm.ClientSites;
                TempData["Markers"] = vm.ClientSites.Markers;
                TempData["SiteStatuses"] = vm.SiteStatuses;
                vm.SiteStatuses = null;
                vm.ClientSites.Markers = null;

                
                var json = Json(vm, JsonRequestBehavior.AllowGet);

                json.MaxJsonLength = int.MaxValue;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                return json;
            }

            catch { }
            return null;
        }



        [IsLogin(CheckPermission = false, Return = ""), HttpPost]
        public ActionResult SiteGridSearch(string value,int Page=1)
        {

            DashboardVM vm = new DashboardVM();
            try
            {
                Session["IsSearch"] = true;
                int offset = (Page - 1) * 5;
                vm = loDashboardBL.GetDashboardSiteVM(Session["WoFilter"].ToString(), Session["DateFilter"].ToString(), Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["ToDate"]), Session["Panel1Option"].ToString(), Convert.ToInt64(Session["Panel1Value"]), Session["Panel2Option"].ToString(), Convert.ToInt64(Session["Panel2Value"]), ViewBag.UserId, value, offset, 5, Session["CountryId"].ToString());

                ViewBag.SiteCount = vm.Count;
            }
            catch
            {

            }

            return PartialView("~/Views/Dashboard/_SiteGrid.cshtml", vm.ClientSites);
        }




        [IsLogin(CheckPermission = false, Return = ""), HttpPost]
        public ActionResult SiteGridNextPage(int Page)
        {
           
            DashboardVM vm = new DashboardVM();
            try
            {
                int offset = (Page - 1) * 5;
                vm = loDashboardBL.GetDashboardSiteVM(Session["WoFilter"].ToString(), Session["DateFilter"].ToString(), Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["ToDate"]), Session["Panel1Option"].ToString(), Convert.ToInt64(Session["Panel1Value"]), Session["Panel2Option"].ToString(),Convert.ToInt64(Session["Panel2Value"]), ViewBag.UserId, "Dashboard_Sites", offset, 5, Session["CountryId"].ToString(), Session["Client"].ToString(), Session["Scopes"].ToString(), Session["Projects"].ToString());
               
                ViewBag.SiteCount = vm.ClientSites.Sites.Count();
            }
            catch (Exception)
            {

            }

            return PartialView("~/Views/Dashboard/_SiteGrid.cshtml", vm.ClientSites);
        }
       


        [IsLogin(CheckPermission = false, Return = ""), HttpPost, ValidateInput(false)]
        public ActionResult SiteGrid(string RequestFrom="")
        {
            try
            {
                ViewBag.RequestFrom = RequestFrom;
                var sites = TempData["SiteGrid"];
                TempData["SiteGrid"] = null;
                return PartialView("~/Views/Dashboard/_SiteGrid.cshtml", sites);
            }
            catch (Exception)
            {

                return null;
            }
            
           
        }
        
        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult SiteGridSectors(Int64 BandId, Int64 SiteId, Int64 NetworkModId, Int64 ScopeId, Int64 CarrierId,string scope,string RequestFrom)
        {
            
            try
            {
                ViewBag.RequestFrom = RequestFrom;
                List<SectorsVM> lstSectors = loSitesBL.GetSectors("GetSectors", SiteId, NetworkModId, BandId, CarrierId, ScopeId);
                ViewBag.SiteId = SiteId;
                ViewBag.NetworkModeId = NetworkModId;
                ViewBag.BandId = BandId;
                ViewBag.CarrierId = CarrierId;
                ViewBag.ScopeId = ScopeId;

                if (lstSectors.Count>0)
                {
                  var SectorRow=  lstSectors.FirstOrDefault();
                  AV_ScopeTestsBL stb = new AV_ScopeTestsBL();
                  var TestTypes = stb.ToCustomList("byNetwokModeId_ScopeId_ClientId_CityId", SectorRow.ClientId, SectorRow.CityId, NetworkModId, ScopeId);
                  ViewBag.TestTypes = TestTypes;
                }
              return PartialView("~/Views/Dashboard/_SiteGridSectors.cshtml", lstSectors);
            }
            catch (Exception)
            {
                return null;
            }


        }
        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult SiteGridSectorsLeftPanel(Int64 BandId, Int64 SiteId, Int64 NetworkModId, Int64 ScopeId, Int64 CarrierId, string scope, string RequestFrom)
        {
            try
            {
                ViewBag.RequestFrom1 = RequestFrom;
                List<SectorsVM> lstSectors = loSitesBL.GetSectors("GetSectors", SiteId, NetworkModId, BandId, CarrierId, ScopeId);
                ViewBag.SiteId1 = SiteId;
                ViewBag.NetworkModeId1 = NetworkModId;
                ViewBag.BandId1 = BandId;
                ViewBag.CarrierId1 = CarrierId;
                ViewBag.ScopeId1 = ScopeId;

                if (lstSectors.Count > 0)
                {
                    var SectorRow = lstSectors.FirstOrDefault();
                    AV_ScopeTestsBL stb = new AV_ScopeTestsBL();
                    var TestTypes = stb.ToCustomList("byNetwokModeId_ScopeId_ClientId_CityId", SectorRow.ClientId, SectorRow.CityId, NetworkModId, ScopeId);
                    ViewBag.TestTypes1 = TestTypes;
                }
                return PartialView("_getSectors", lstSectors);
            }
            catch (Exception)
            {
                return null;
            }


        }
        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult getStationaryTab(Int64 BandId, Int64 SiteId, Int64 NetworkModId, Int64 ScopeId, Int64 CarrierId, string scope, string RequestFrom)
        {
            try
            {
                ViewBag.RequestFrom1 = RequestFrom;
                List<SectorsVM> lstSectors = loSitesBL.GetSectors("GetSectors", SiteId, NetworkModId, BandId, CarrierId, ScopeId);
                ViewBag.SiteId1 = SiteId;
                ViewBag.NetworkModeId1 = NetworkModId;
                ViewBag.BandId1 = BandId;
                ViewBag.CarrierId1 = CarrierId;
                ViewBag.ScopeId1 = ScopeId;

                if (lstSectors.Count > 0)
                {
                    var SectorRow = lstSectors.FirstOrDefault();
                    AV_ScopeTestsBL stb = new AV_ScopeTestsBL();
                    var TestTypes = stb.ToCustomList("byNetwokModeId_ScopeId_ClientId_CityId", SectorRow.ClientId, SectorRow.CityId, NetworkModId, ScopeId);
                    ViewBag.TestTypes1 = TestTypes;
                }
                return PartialView("_getStationaryTab", lstSectors);
            }
            catch (Exception)
            {
                return null;
            }


        }


        #region Get Route Direction

        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult RouteDirection(Int64 SiteId, string SiteCode, Int64 RouteId, string LineCode, string ClientPrefix)
        {
            Response res = new Response();
            DirectoryHandler dh = new DirectoryHandler();
            string[] LatLngs = null;

            string CompleteKml = null;
            KML km = new KML();
            try
            {
                CompleteKml += km.Open("Routes", "Routes For Site");
                int count = 1;
                #region Existing File Coordinates

                string Path = "/Content/AirViewLogs/" + ClientPrefix + "/" + SiteCode;
                if (dh.FileExist(Server.MapPath("~" + Path + "/route-" + RouteId + ".kml")))
                {
                    string text;
                    var fileStream = new FileStream(Server.MapPath("~" + Path + "/route-" + RouteId + ".kml"), FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        text = streamReader.ReadToEnd();
                    }
                    MyString ms = new MyString();
                    string MyPlacemarks = "";
                    string[] MyPlacemarksSplit;
                    int MyIndex = 5782;
                    if (LineCode != "")
                    {
                        MyPlacemarks = ms.BetweenTag(text, "Placemark", "&");
                        MyPlacemarksSplit = MyPlacemarks.Split('&');
                        int counter = 0;

                        foreach (var item in MyPlacemarksSplit)
                        {
                            if (item.Contains(LineCode))
                                MyIndex = counter;
                            counter++;


                        }
                    }

                    string Cordinate = ms.BetweenTag(text, "coordinates", "&");
                    string Colors = ms.BetweenTag(text, "color", "&");
                    string[] ColorsArr = Colors.Split('&');
                    Cordinate = Cordinate.Replace(",0", ",");
                    string[] Tags = Cordinate.Split('&');
                    string cords; 
                    string[] Cordinates;

                    for (int i = 0; i < Tags.Length; i++)
                    {
                        if (i == MyIndex && MyIndex != 5782)
                        {
                            if (Tags[i].Trim().Length > 0)
                            {
                                cords = null;
                                Cordinates = Tags[i].Split('\n');
                                LatLngs = Cordinates;
                                count++;
                            }
                        }

                    }
                }

                #endregion


                if (LineCode != "")
                {


                    AV_DriveRoutesBL drb = new AV_DriveRoutesBL();
                    AV_DriveRoutes dr = new AV_DriveRoutes();
                    dr.CreatedDate = DateTime.Now;
                    dr.RouteId = RouteId;
                    dr.SiteId = Convert.ToInt64(SiteId);
                    dr.CreatedBy = ViewBag.UserId;
                    dr.RoutePath = Path;


                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    res.Status = "success";
                    res.Message = "Successfully";
                    res.Value = LatLngs;
                }
                else
                {
                    res.Status = "danger";
                    res.Message = "No Route Selected.";
                }

            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region Floor Plan GET and Save

        [IsLogin(CheckPermission = false)]
        public ActionResult FloorPlan(Int64 SiteId, int UserId, string Client,string SiteCode)
        {
            try
            {
                AD_DefinationBL db = new AD_DefinationBL();
                ViewBag.Floors = db.ToList("GetFloors");
                ViewBag.SiteCode = SiteCode;
                ViewBag.Clientprefix = Client.Split('-')[0];
                ViewBag.SiteId = SiteId;
               FloorPlan_DL FloorDL = new FloorPlan_DL();
                DataTable Table = FloorDL.Get("GetById", SiteId);
              ViewBag.Planned = Table.ToList<AV_FloorPlan>();
               return PartialView("~/views/Dashboard/_FloorPlan.cshtml");
            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public JsonResult SaveFloorPlan(string[] Floor,Int64 SiteId,string Clientprefix,string SiteCode, string[] PlanId,bool[] IsActive)
        {
            Response res = new Response();
            try
            {
                dbDataTable dbtd = new dbDataTable();
                DataTable dt = dbtd.List();
                FloorPlan_DL FloorDL = new FloorPlan_DL();
               for (int i = 0; i < HttpContext.Request.Files.Count; i++)
                {
                    #region File Save
                    string Path = "/Content/AirViewLogs/"+ Clientprefix+"/"+ SiteCode;
                    HttpPostedFileBase file = HttpContext.Request.Files[i] as HttpPostedFileBase;
                   string extension = System.IO.Path.GetExtension(file.FileName).ToLower();
                    if (!Directory.Exists(HttpContext.Server.MapPath("~" + Path)))
                    {
                        // if it doesn't exist, create
                        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~" + Path));
                    }
                    var Time = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fname = HttpContext.Server.MapPath("~" + Path+"/"+Floor[i]);
                     HttpContext.Request.Files[i].SaveAs(fname+Time+ extension);
                    myDataTable.AddRow(dt,"Value1",SiteId,"Value2",Convert.ToInt64(Floor[i]),"Value15",Path+"/"+Floor[i]+Time+ extension, "Value3", true,"Value4",PlanId[i]);
                    
                    #endregion
                  
                }
                FloorDL.Save("Save", dt);
                res.Status = "success";
                res.Message = "success";
            }
               catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = ex.Message;
                ExceptionHandling.ErrorLogs(ViewBag.RequestUrl, ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult IsActiveFloorPlan(Int64 PlanId,bool IsActive)
        {
            Response res = new Response();
            try
            {
                FloorPlan_DL FloorDL = new FloorPlan_DL();
            bool  result =  FloorDL.Manage("IsActive", PlanId, IsActive);
                res.Status = "success";
                res.Message = "success";
                res.Value = result;
            }
            catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = ex.Message;
               // ExceptionHandling.ErrorLogs(ViewBag.RequestUrl, ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        #endregion

        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult SiteGridTSSDetails(Int64 SiteSurveyId, string RequestFrom)
        {


            try
            {
                ViewBag.RequestFrom = RequestFrom;
                Library.SWI.Survey.BLL.TSS_SectionBL sb = new Library.SWI.Survey.BLL.TSS_SectionBL();
                var rec = sb.ToList("Sections_by_SiteSurvey_For_Dashboard", SiteSurveyId.ToString()) ;

                return PartialView("~/Views/Dashboard/_SiteGridTSSDetails.cshtml", rec);
            }
            catch (Exception)
            {
                return null;
            }


        }
        [IsLogin(CheckPermission = false)]
        public ActionResult SiteGridCLSDetails(Int64 SiteId,int LayerId,int? ScopeId,string RequestFrom)
        {
            try
            {
                ViewBag.RequestFrom = RequestFrom;
                int User = Convert.ToInt16(ViewBag.UserId);
                Common.SelectedList sl = new Common.SelectedList();
                List<SelectListItem> Users = new List<SelectListItem>();
                List<Sec_UserDevices> UserDevices = new List<Sec_UserDevices>();
                sl.UserAssinged_Testers_Devices(User, ref Users, ref UserDevices);
                ViewBag.Users = Users;
                   ViewBag.UserDevices = UserDevices;

                AV_WoDevicesBL wdb = new AV_WoDevicesBL();
                ViewBag.SelectedLayer = wdb.ToList("BySiteId", SiteId, 0, 0, 0, 0, 0);
                CLS_VMBL abc = new CLS_VMBL();
                List<AV_SiteScript> lstSectors = abc.ToListSectors("GetSectorsCLS", SiteId, LayerId);
                ViewBag.LayerId = LayerId;
                ViewBag.SelectedLayer = wdb.ToList("BySiteId", SiteId, 0, 0, 0, 0, 0);

                return PartialView("~/Views/Dashboard/_SiteGridSectorsCLS.cshtml", lstSectors);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [IsLogin(CheckPermission = false, Return = ""), HttpPost]
        public ActionResult SitesStatuses()
        {
            try
            {
                var SiteStatuses = TempData["SiteStatuses"];
                TempData["SiteStatuses"] = null;
                return PartialView("~/Views/Dashboard/_SitesStatuses.cshtml", SiteStatuses);
            }
            catch (Exception)
            {

                return null;
            }

        }

        [IsLogin(CheckPermission = false, Return = ""), HttpPost]
        public ActionResult SiteLocations()
        {

            try
            {
                var Markers = TempData["Markers"] ;
                TempData["Markers"] = null;
                return PartialView("~/Views/Dashboard/_SiteLocations.cshtml", Markers);

            }
            catch (Exception)
            {
                return null;
            }

        }
       
        [IsLogin(Return = "",CheckPermission =false), HttpPost]
        public ActionResult RegionalSummery(string Filter)
        {
            List<RegionsVM> lstRegions = new List<RegionsVM>();
            try
            {
                Session["Panel1Value"] = 0;
                Session["Panel1Option"] = Filter;
                lstRegions = loDashboardBL.GetPartialRegionalSites(Session["WoFilter"].ToString(), Session["DateFilter"].ToString(), Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["ToDate"]), Session["Panel1Option"].ToString(), ViewBag.UserId, Convert.ToInt64(Session["Panel1Value"]),null,null);

            }
            catch{}
            return PartialView("~/Views/Dashboard/_RegionalSites.cshtml", lstRegions);
        }

        [IsLogin(CheckPermission = false, Return = ""), HttpPost]
        public ActionResult Regions(DashboardVM vm)
        {
            try
            {
                return PartialView("~/Views/Dashboard/_RegionalSites.cshtml", vm.Regions);
            }
            catch (Exception)
            { return null;}
        }

        [IsLogin(CheckPermission = false, Return = ""), HttpPost]
        public ActionResult TesterSummery(string filter)
        {

            List<RegionsVM> lstRegions = new List<RegionsVM>();
            try
            {
                Session["Panel2Option"] = filter;
                Session["Panel2Value"] = 0;

               
                lstRegions = loDashboardBL.GetPartialDriveTesterSites(Session["WoFilter"].ToString(), Session["DateFilter"].ToString(), Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["ToDate"]), Session["Panel2Option"].ToString(), ViewBag.UserId, Convert.ToInt64(Session["Panel2Value"]), Session["Panel1Option"].ToString(), Session["Panel1Value"].ToString());
            }
            catch (Exception)
            {}

            return PartialView("~/Views/Dashboard/_DriveTesterSites.cshtml", lstRegions);
        }

        [IsLogin(CheckPermission = false, Return = ""), HttpPost]
        public ActionResult DriveTester(DashboardVM vm)
        {
            List<RegionsVM> lstRegions = new List<RegionsVM>();
            try
            {
                return PartialView("~/Views/Dashboard/_DriveTesterSites.cshtml", vm.DriveTesterSites);
            }
            catch (Exception)
            {
                return null;
                     
            }
        }

        //[HttpPost]
        //public JsonResult DriveRoute(List<Cordinates> cordinates, Int64 SiteId, string SiteCode, string TestType, int ScopeId, Int64 RouteId, string Filter, string ClientPrefix, string Delete, List<Cordinates> PathJson)
        //{
        //    Response res = new Response();
        //    DirectoryHandler dh = new DirectoryHandler();

        //    string CompleteKml = null;
        //    KML km = new KML();
        //    try
        //    {
        //        CompleteKml += km.Open("Routes", "Routes For Site");

        //        int count = 1;
        //        #region Existing File Coordinates

        //        string Path = "/Content/AirViewLogs/" + ClientPrefix + "/" + SiteCode;
        //        if (dh.FileExist(Server.MapPath("~" + Path + "/route-" + RouteId + ".kml")))
        //        {
        //            string text;
        //            var fileStream = new FileStream(Server.MapPath("~" + Path + "/route-" + RouteId + ".kml"), FileMode.Open, FileAccess.Read);
        //            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        //            {
        //                text = streamReader.ReadToEnd();
        //            }
        //            MyString ms = new MyString();
        //            string MyPlacemarks = "";
        //            string[] MyPlacemarksSplit;
        //            int MyIndex = 5782;
        //            if (Delete != "")
        //            {
        //                MyPlacemarks = ms.BetweenTag(text, "Placemark", "&");
        //                MyPlacemarksSplit = MyPlacemarks.Split('&');
        //                int counter = 0;

        //                foreach (var item in MyPlacemarksSplit)
        //                {
        //                    if (item.Contains(Delete))
        //                        MyIndex = counter;
        //                    counter++;


        //                }
        //            }

        //            string Cordinate = ms.BetweenTag(text, "coordinates", "&");
        //            string Colors = ms.BetweenTag(text, "color", "&");
        //            string[] ColorsArr = Colors.Split('&');
        //            //  Cordinate = Cordinate.Replace(",0", ",");
        //            //Cordinate = Cordinate.Replace(",-", "-");
        //            //Cordinate = Cordinate.Replace("-", ",-");
        //            //Cordinate = Cordinate.Replace(",,", ",");
        //            string[] Tags = Cordinate.Split('&');
        //            string cords; ;
        //            string[] Cordinates;

        //            for (int i = 0; i < Tags.Length; i++)
        //            {
        //                if (Delete == "" || (i != MyIndex && MyIndex != 5782))
        //                {
        //                    if (Tags[i].Trim().Length > 0)
        //                    {
        //                        cords = null;
        //                        Cordinates = Tags[i].Split('\n');
        //                        for (int j = 0; j < Cordinates.Length; j++)
        //                        {
        //                            if (Cordinates[j].Trim().Length > 0)
        //                            {
        //                                cords += Cordinates[j] + "0\n";
        //                            }
        //                        }
        //                        CompleteKml += km.Style("LineId" + count, "LineStyle", "color", "FFA9A9A9", "width", "4");
        //                        CompleteKml += km.Placemark("LineId" + count, "LineId" + count, "LineString", "relative", cords);

        //                        count++;
        //                    }
        //                }

        //            }
        //        }

        //        #endregion


        //        if (cordinates != null || Delete != "")
        //        {
        //            if (Delete == "")
        //            {
        //                //  foreach (var jk in cordinates)
        //                // {
        //                string cords = null;

        //                string plotColor = "FFA9A9A9"; //(!string.IsNullOrEmpty(r.Color)) ? r.Color.Replace("#", "") :
        //                plotColor = "ff" + plotColor.Substring(4, 2) + plotColor.Substring(2, 2) + plotColor.Substring(0, 2);

        //                CompleteKml += km.Style("LineId" + count, "LineStyle", "color", plotColor, "width", "4");
        //                foreach (var cor in cordinates)
        //                {

        //                    cords += cor.location.lng + "," + cor.location.lat + ",0\n";
        //                }
        //                CompleteKml += km.Placemark("LineId" + count, "LineId" + count, "LineString", "relative", cords);
        //                // CompleteKml += km.Placemarks("LineId" + count, "#LineStyle", "relativeToGround", cords);
        //                count++;
        //                // }
        //            }

        //            AV_DriveRoutesBL drb = new AV_DriveRoutesBL();
        //            AV_DriveRoutes dr = new AV_DriveRoutes();
        //            dr.CreatedDate = DateTime.Now;
        //            dr.RouteId = RouteId;
        //            dr.SiteId = Convert.ToInt64(SiteId);
        //            dr.CreatedBy = ViewBag.UserId;
        //            dr.RoutePath = Path;
        //            if (TestType.Length > 1)
        //            {
        //                dr.TestType = TestType.Remove(TestType.Length - 1);
        //            }

        //            dr.ScopeId = ScopeId;
        //            RouteId = drb.Manage(Filter, dr);


        //            if (RouteId > 0)
        //            {
        //                CompleteKml += km.Close();
        //                dh.CreateDirectory(Server.MapPath(Path));
        //                km.SaveKml2(CompleteKml, "route-" + RouteId, Server.MapPath(Path));
        //                if (PathJson != null)
        //                {
        //                    string jsonpath = "~" + Path;
        //                    string fname = HttpContext.Server.MapPath(jsonpath);
        //                    if (!Directory.Exists(fname))
        //                    { // if it doesn't exist, create

        //                        System.IO.Directory.CreateDirectory(fname);
        //                    }
        //                    string json = JsonConvert.SerializeObject(PathJson.ToArray());

        //                    //write string to file
        //                    System.IO.File.WriteAllText(fname + "/route-" + RouteId + ".txt", json);
        //                }
        //            }

        //            GC.Collect();
        //            GC.WaitForPendingFinalizers();

        //            res.Status = "success";
        //            if (Delete == "")
        //            {
        //                res.Message = "Drive Route Planned Successfully.";
        //            }
        //            else { res.Message = "Drive Route Delete Successfully."; }
        //        }
        //        else
        //        {
        //            res.Status = "danger";
        //            res.Message = "No Route Selected.";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        res.Status = "danger";
        //        res.Message = ex.Message;
        //    }

        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        [IsLogin(CheckPermission = false, Return = ""), HttpPost]
        public ActionResult SiteLocationsForSchedule()
        {

            List<SitesVM> lstSites = new List<SitesVM>();
            try
            {
                lstSites = loDashboardBL.GetSiteLocationsForSchedule(Session["WoFilter"].ToString(), Session["DateFilter"].ToString(),Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["ToDate"]),  Session["Panel1Option"].ToString(), Convert.ToInt64(Session["Panel1Value"]), Session["Panel2Option"].ToString(), Convert.ToInt64(Session["Panel2Value"]), ViewBag.UserId, Session["CountryId"].ToString(), Session["Client"].ToString(), Session["Scopes"].ToString(), Session["Markets"].ToString());
            }
            catch (Exception)
            {

            }

            return PartialView("~/Views/Dashboard/_SiteLocations.cshtml", lstSites);
        }


        

        [IsLogin(CheckPermission = false), HttpPost]
        public JsonResult ManageDriveRoute(String Filter, Int64 RouteId, bool IsSelected)
        {
            Response res = new Response();

            try
            {
                AV_DriveRoutesBL drb = new AV_DriveRoutesBL();
                AV_DriveRoutes dr = new AV_DriveRoutes();
                dr.RouteId = RouteId;
                dr.IsSelected = IsSelected;
                int Id = drb.Manage(Filter, dr);
                if (Id > 0)
                {
                    res.Status = "success";
                    res.Message = "Drive Route Planned Successfully.";
                }
                else
                {
                    res.Status = "danger";
                    res.Message = "Record not found.";
                }
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportWorkOrderStatus()
        {
            try
            {
                var grid = new GridView();
                AV_NetLayerReportExportBL rptNtlExp = new AV_NetLayerReportExportBL();
                Int64 UserId = ViewBag.UserId;
                var rec = rptNtlExp.Get(Session["DateFilter"].ToString(), Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["ToDate"]), Session["WoFilter"].ToString(), Session["Panel1Option"].ToString(), Session["Panel1Value"].ToString(), Session["Panel2Option"].ToString(), Session["Panel2Value"].ToString(), "WorkOrderStatus", UserId);

                var rec2 = rec.Select(m => new {m.WO_Ref,m.Client,m.Region,m.Market,m.Site,m.Drive_Tester,m.Received,m.Submitted,m.Scheduled,m.Drive_Completed,m.ReportSubmittedOn,m.Approved,
                    m.Network_Layers,m.Status,m.Description,
                    m.SectorCount,
                    m.LayerCount,
                    m.Distance_from_Site,
                    m.DT_Minutes,
                    m.Site_Completed,
                    m.Site_First_Test,
                    m.SiteType,
                    m.Scope,
                    m.Project,
                    m.CheckList
                }).ToList().ToDataTable();
                MemoryStream memStream;
                using (var package = new ExcelPackage())
                {
                    var Sheet1 = package.Workbook.Worksheets.Add("Report Summary");


                    DataType dt = new DataType();

                    for (int i = 0; i < rec2.Columns.Count; i++)
                    {
                        Sheet1.Cells[1, i + 1].Value = rec2.Columns[i].ColumnName;
                    }

                    for (int i = 0; i < rec2.Rows.Count; i++)
                    {
                        for (int j = 0; j < rec2.Columns.Count; j++)
                        {
                            if ((j + 1) == 7 || (j + 1) == 8 || (j + 1) == 9 || (j + 1) == 8 || (j + 1) == 10 || (j + 1) == 11 || (j + 1) == 12)
                            {
                                Sheet1.Cells[i + 2, j + 1].Style.Numberformat.Format = "m/d/yyyy hh:mm";
                            }
                            if ((j + 1) == 20 || (j + 1) == 21)
                            {
                                Sheet1.Cells[i + 2, j + 1].Style.Numberformat.Format = "m/d/yyyy";
                            }

                            Sheet1.Cells[i + 2, j + 1].Value = dt.GetType(rec2.Rows[i][j].ToString());


                        }
                    }


                    var Markets = rec.GroupBy(l => new { l.Market })
                                                .Select(cl => new
                                                {
                                                    Market = cl.First().Market,
                                                    MarketCount = cl.Count(),
                                                }).ToList();
                    List<AV_NetLayerReportMarktExport> msList = new List<AV_NetLayerReportMarktExport>();
                    AV_NetLayerReportMarktExport ms;
                    #region MarketSummary
                    foreach (var item in Markets)
                    {
                        var temp = rec.Where(m => m.Market == item.Market).GroupBy(l => new { l.Status })
                                                .Select(cl => new
                                                {
                                                    Status = cl.First().Status,
                                                    StatusCount = cl.Count(),
                                                }).ToList();
                        if (temp != null)
                        {
                            ms = new AV_NetLayerReportMarktExport();
                            ms.Market = item.Market;
                            ms.Total = item.MarketCount;

                            var Temp2 = temp.Where(m => m.Status == "Report Submitted").FirstOrDefault();
                            ms.Report_Submitted = (Temp2 != null) ? Temp2.StatusCount : 0;

                            Temp2 = temp.Where(m => m.Status == "Pending Schedule").FirstOrDefault();
                            ms.Pending_Schedule = (Temp2 != null) ? Temp2.StatusCount : 0;

                            Temp2 = temp.Where(m => m.Status == "Approved").FirstOrDefault();
                            ms.Approved = (Temp2 != null) ? Temp2.StatusCount : 0;

                            Temp2 = temp.Where(m => m.Status == "Pending With Issues").FirstOrDefault();
                            ms.Pending_With_Issues = (Temp2 != null) ? Temp2.StatusCount : 0;

                            Temp2 = temp.Where(m => m.Status == "Scheduled").FirstOrDefault();
                            ms.Scheduled = (Temp2 != null) ? Temp2.StatusCount : 0;


                            Temp2 = temp.Where(m => m.Status == "In Progress").FirstOrDefault();
                            ms.In_Progress = (Temp2 != null) ? Temp2.StatusCount : 0;

                            Temp2 = temp.Where(m => m.Status == "Drive Completed").FirstOrDefault();
                            ms.Drive_Completed = (Temp2 != null) ? Temp2.StatusCount : 0;

                            msList.Add(ms);
                        }

                    }
                    var tempdt = msList.ToDataTable();
                    var Sheet2 = package.Workbook.Worksheets.Add("Market Summary");

                    for (int ik = 0; ik < tempdt.Columns.Count; ik++)
                    {
                        Sheet2.Cells[1, ik + 1].Value = tempdt.Columns[ik].ColumnName;
                    }

                    int col = 0, row = 0;
                    for (col = 0; col < tempdt.Rows.Count; col++)
                    {
                        for (row = 0; row < tempdt.Columns.Count; row++)
                        {
                            Sheet2.Cells[col + 2, row + 1].Value = dt.GetType(tempdt.Rows[col][row].ToString());
                        }
                    }

                    Sheet2.Cells[col + 2, 2].Value = msList.Select(m => m.Pending_Schedule).Sum();
                    Sheet2.Cells[col + 2, 3].Value = msList.Select(m => m.Scheduled).Sum();
                    Sheet2.Cells[col + 2, 4].Value = msList.Select(m => m.In_Progress).Sum();
                    Sheet2.Cells[col + 2, 5].Value = msList.Select(m => m.Drive_Completed).Sum();
                    Sheet2.Cells[col + 2, 6].Value = msList.Select(m => m.Pending_With_Issues).Sum();
                    Sheet2.Cells[col + 2, 7].Value = msList.Select(m => m.Report_Submitted).Sum();
                    Sheet2.Cells[col + 2, 8].Value = msList.Select(m => m.Approved).Sum();
                    Sheet2.Cells[col + 2, 9].Value = msList.Select(m => m.Total).Sum();
                    #endregion


                    #region TesterSummary
                    var Tester = rec.GroupBy(l => new { l.Drive_Tester })
                                               .Select(cl => new
                                               {
                                                   Tester = cl.First().Drive_Tester,
                                                   TesterCount = cl.Count(),
                                               }).ToList();
                    msList.Clear();
                    foreach (var item in Tester)
                    {
                        var temp = rec.Where(m => m.Drive_Tester == item.Tester).GroupBy(l => new { l.Status })
                                                .Select(cl => new
                                                {
                                                    Status = cl.First().Status,
                                                    StatusCount = cl.Count(),
                                                }).ToList();
                        if (temp != null)
                        {
                            ms = new AV_NetLayerReportMarktExport();
                            ms.Market = item.Tester;
                            ms.Total = item.TesterCount;

                            var Temp2 = temp.Where(m => m.Status == "Report Submitted").FirstOrDefault();
                            ms.Report_Submitted = (Temp2 != null) ? Temp2.StatusCount : 0;

                            Temp2 = temp.Where(m => m.Status == "Pending Schedule").FirstOrDefault();
                            ms.Pending_Schedule = (Temp2 != null) ? Temp2.StatusCount : 0;

                            Temp2 = temp.Where(m => m.Status == "Approved").FirstOrDefault();
                            ms.Approved = (Temp2 != null) ? Temp2.StatusCount : 0;

                            Temp2 = temp.Where(m => m.Status == "Pending With Issues").FirstOrDefault();
                            ms.Pending_With_Issues = (Temp2 != null) ? Temp2.StatusCount : 0;

                            Temp2 = temp.Where(m => m.Status == "Scheduled").FirstOrDefault();
                            ms.Scheduled = (Temp2 != null) ? Temp2.StatusCount : 0;


                            Temp2 = temp.Where(m => m.Status == "In Progress").FirstOrDefault();
                            ms.In_Progress = (Temp2 != null) ? Temp2.StatusCount : 0;

                            Temp2 = temp.Where(m => m.Status == "Drive Completed").FirstOrDefault();
                            ms.Drive_Completed = (Temp2 != null) ? Temp2.StatusCount : 0;

                            msList.Add(ms);
                        }

                    }
                    tempdt = msList.ToDataTable();
                    var Sheet3 = package.Workbook.Worksheets.Add("Tester Summary");

                    for (int ik = 0; ik < tempdt.Columns.Count; ik++)
                    {
                        var coltemp = Sheet3.Cells[1, ik + 1];
                        if (tempdt.Columns[ik].ColumnName == "Market")
                        {
                            coltemp.Value = "Tester";
                        }
                        else
                        {
                            coltemp.Value = tempdt.Columns[ik].ColumnName;

                        }
                    }

                    col = 0; row = 0;
                    for (col = 0; col < tempdt.Rows.Count; col++)
                    {
                        for (row = 0; row < tempdt.Columns.Count; row++)
                        {
                            Sheet3.Cells[col + 2, row + 1].Value = dt.GetType(tempdt.Rows[col][row].ToString());

                        }
                    }

                    Sheet3.Cells[col + 2, 2].Value = msList.Select(m => m.Pending_Schedule).Sum();
                    Sheet3.Cells[col + 2, 3].Value = msList.Select(m => m.Scheduled).Sum();
                    Sheet3.Cells[col + 2, 4].Value = msList.Select(m => m.In_Progress).Sum();
                    Sheet3.Cells[col + 2, 5].Value = msList.Select(m => m.Drive_Completed).Sum();
                    Sheet3.Cells[col + 2, 6].Value = msList.Select(m => m.Pending_With_Issues).Sum();
                    Sheet3.Cells[col + 2, 7].Value = msList.Select(m => m.Report_Submitted).Sum();
                    Sheet3.Cells[col + 2, 8].Value = msList.Select(m => m.Approved).Sum();
                    Sheet3.Cells[col + 2, 9].Value = msList.Select(m => m.Total).Sum();
                    #endregion

                    memStream = new MemoryStream(package.GetAsByteArray());
                }
                return File(memStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WorkOrderStatus_" + DateTime.Now + ".xlsx");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        public void NetLayerExport()
        {
            try
            {

                var grid = new GridView();

                AV_NetLayerReportExportBL rptNtlExp = new AV_NetLayerReportExportBL();
                Int64 UserId = ViewBag.UserId;
                var rec = rptNtlExp.Get(Session["DateFilter"].ToString(), Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["ToDate"]), Session["WoFilter"].ToString(), Session["Panel1Option"].ToString(), Session["Panel1Value"].ToString(), Session["Panel2Option"].ToString(), Session["Panel2Value"].ToString(), "NetLayerStatus", UserId);
                grid.DataSource = rec.Select(m => new { m.WO_Ref, m.Client, m.Region, m.Market, m.Site, m.Network_Mode, m.Band, m.Carrier, m.Scope, m.Drive_Tester, m.Received, m.Submitted, m.Scheduled, m.Drive_Completed, m.ReportSubmittedOn, m.Approved, m.Status, m.Description, m.SiteType,m.Project });
                grid.DataBind();
                string FileName = "NetLayerStatus_" + DateTime.Now;
                Response.ClearContent();
                Response.Buffer = true;
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
            {

                //   throw;
            }
        }

        [IsLogin(CheckPermission = false, Return = ""), HttpPost]
        private DataTable ThisUserMarkets()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.AddRange(new DataColumn[2]
                                               {
                                                   new DataColumn("Id", typeof (int)),
                                                   new DataColumn("value", typeof (string)),
                                               });
                DataRow r;

                List<UserCity> lstCities = ViewBag.UserCities;
                foreach (var city in lstCities)
                {
                    r = dt.NewRow();
                    r["Id"] = city.CityId;
                    r["value"] = "";
                    dt.Rows.Add(r);
                }
            }
            catch (Exception)
            {

            }

            return dt;
        }

       
        [HttpPost,IsLogin(CheckPermission =false)]
        public ActionResult CurrentDateFilter(string filter) {
            DateFunctions df = new DateFunctions(Convert.ToDateTime(Session["FromDate"])); 

            if (filter== "ThisWeek" || filter == "WeekNext")
            {
                df = new DateFunctions(Convert.ToDateTime(Session["ToDate"]));
            }
            return Json(df, JsonRequestBehavior.AllowGet);
        }


        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult GetRoutePlan(string filter,string TesterId)
        {
            AV_GetRoutePlanBL rpb = new AV_GetRoutePlanBL();
            var rec = rpb.ToList(filter, TesterId);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        #region 
        [NoAsyncTimeout]
        public async Task<ActionResult> Support(string SiteId,string code)
        {
            ViewBag.SiteId = SiteId;
            return await Task.Run(() => View());
        }

        #endregion

       //Create Page for Help
        public ActionResult CreateHelp()
        {
            return View();
        }
        //List Page for Help
        public ActionResult ListHelp()
        {
        
            return View();
        }
        //ListJson for Help List
        //Associated with ListHelp()
        public JsonResult ListHelpJson(int id)
        {
            bool state = false;
            if (id != 0)
            {
                state = true;
            }
            
            AD_HelpBL help = new AD_HelpBL();
            var jsonData = help.List("List_Help", state).ToList();
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        //Request to Publish & Draft Content in Help List
        //Associated with ListHelp()
        public JsonResult ListRow(int FeatureID, int id)
        {
            bool state = false;
            if (id != 0)
            {
                state = true;
            }

            AD_HelpBL help = new AD_HelpBL();
            help.ListRow("ListRow_Help", state, FeatureID);

            return Json("Success!", JsonRequestBehavior.AllowGet);
        }

              

        //It will load ComponentId in create help
        //Associated with CreateHelp()
        [HttpPost]
        public JsonResult HelpComponent()
        {
            AD_HelpBL help = new AD_HelpBL();
            var jsonData = help.Read("Get_Component_list").ToList();
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //It will load ModuleId in create help
        //Associated with CreateHelp()
        [HttpPost]
        public JsonResult HelpModule(int id)
        {
            AD_HelpBL help = new AD_HelpBL();
            var jsonData = help.Read("Get_Module_list", id).ToList();
            List<AD_Help> response = new List<AD_Help>();
            foreach (AD_Help model in jsonData)
            {
                    response.Add(model);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //It will load FeatureId in create help
        //Associated with CreateHelp()
        public JsonResult HelpFeature(int MID, int CID)
        {
            AD_HelpBL help = new AD_HelpBL();
            var jsonData = help.Read("Get_Feature_list", CID, MID).ToList();
            List<AD_Help> response = new List<AD_Help>();
            foreach (AD_Help model in jsonData)
            {

                response.Add(model);

            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditHelpJson() //It will be fired from Jquery ajax call  
        {
            AD_HelpBL help = new AD_HelpBL();
            var jsonData = help.Read("Get_HelpPermissions").ToList();
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHelp([Bind(Include = "Title, ComponentId, ModuleId, FeatureId, Description")] AD_Help post)
        {
            if (ModelState.IsValid)
            {
                AD_HelpBL help = new AD_HelpBL();
                help.Create("Create_Help", post.ComponentId, post.ModuleId, post.FeatureId, post.Title, post.Description, false);
                return RedirectToAction("ListHelp");
            }

            return View(post);
        }
        //Create Page to View Help
        public ActionResult ReadHelp(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AD_HelpBL post = new AD_HelpBL();

            List<AD_Help> data =  post.ReadPost("Read_Help",id).ToList();
            
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        public ActionResult EditHelp(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AD_HelpBL post = new AD_HelpBL();

            List<AD_Help> data = post.ReadPost("Read_Help", id).ToList();

            String code = data[0].Description;

            ViewBag.code = code.Replace("'", "");
            ViewBag.HelpId = data[0].HelpId;
            ViewBag.ComponentId = data[0].ComponentId;
            ViewBag.ComponentName = data[0].ComponentName;
            ViewBag.ModuleId = data[0].ModuleId;
            ViewBag.ModuleName = data[0].ModuleName;
            ViewBag.FeatureId = data[0].FeatureId;
            ViewBag.FeatureName = data[0].FeatureName;

            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        [HttpPost ,ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditHelp([Bind(Include = "HelpId, Title, Description")] AD_Help post)
        {
            if (ModelState.IsValid)
            {
                AD_HelpBL help = new AD_HelpBL();
                help.EditPost("Edit_Help", post.HelpId, post.Title, post.Description);
                return RedirectToAction("ListHelp");
            }

            return View(post);
        }

    }

}