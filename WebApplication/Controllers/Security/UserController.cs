using System;
using System.Web.Mvc;
using SWI.AirView.Common;
using SWI.Libraries.Security.Entities;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.BLL;
using SWI.Security.Filters;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using System.Collections.Generic;
using SWI.AirView.Models;
using System.Data;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.AD.BLL;
using System.Globalization;
using SWI.Libraries.AirView;
using AirView.DBLayer.AirView.BLL;

namespace SWI.Security.Controllers
{
    /*----MoB!----*/
    [IsLogin]
    public class UserController : CommonController
    {

        // GET: User
        [IsLogin(CheckPermission = false)]
        public ActionResult New(string title, string Id = "")
        {
            if (Id == "")
            {
                return RedirectToAction("All");
            }
            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
            Sec_UserBL ud = new Sec_UserBL();
            ViewBag.tit = title;
            ViewBag.Id = 0;
            ViewBag.Reports = sl.User("All");
            ViewBag.ReportTo = ud.ToList("All");

            if (Id != "")
            {
                ViewBag.Id = Convert.ToInt64(Id);
            }

            ClientsBL ub = new ClientsBL();
            ViewBag.Clients2 = sl.Clients("AllRecords");
            NewData();


            return View();
        }
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }


        public ActionResult GetSummryByDate(string currentDate,string id,string[] Markets)
        {
            List<Summary> res = new List<Summary>();
            try
            {
                List<Summary> rs = new List<Summary>();
                ViewBag.UserId = id;
             
                Sec_UserBL obj = new Sec_UserBL();
                var u = obj.Single("ById", id);
                ViewBag.UserName = u.FirstName + " " + u.LastName;
             var a = currentDate.Split('-');
                int i = 0;
                string StartDate = "";
                string EndDate ="";
                foreach(var item in a)
                {
                    if (i == 0)
                    {
                        StartDate = item;
                        i++;
                    }
                    else
                    {
                        EndDate = item;
                    }
                }
                ViewBag.Defultdate = StartDate;
               rs= obj.SummaryList("GetUserSummary", StartDate, EndDate, id);
                if(Markets.Length !=0 || Markets !=null) { 
           foreach(var item in rs)
                {
                    foreach(var itm in Markets)
                    {
                        if(int.Parse(itm) == item.DefinationId)
                        {
                            res.Add(item);
                        }
                    }
                }
                }
                else
                {
                    res = rs;
                }

                return PartialView("~\\Views\\User\\_PartialCalender.cshtml", res);
            }
            catch (Exception ex)
            {

                res = null;
                return Json(res, JsonRequestBehavior.AllowGet);
            }
           // return Json(true, JsonRequestBehavior.AllowGet);
        }
      
        public ActionResult Summary(int Id)
        {
            List<Summary> res = new List<Summary>();
            Sec_UserBL obj = new Sec_UserBL();
            var u = obj.Single("ById", Id.ToString());
            ViewBag.UserName = u.FirstName + " " + u.LastName;
            int thisWeekNumber = GetIso8601WeekOfYear(DateTime.Today);
            // 11/11/2013  
            DateTime firstDayOfWeek = FirstDateOfWeek(DateTime.Now.Year, thisWeekNumber, CultureInfo.CurrentCulture);
            // 11/12/2012  
            DateTime firstDayOfLastYearWeek = FirstDateOfWeek(DateTime.Now.Year, thisWeekNumber, CultureInfo.CurrentCulture);
            var StartDate = firstDayOfLastYearWeek.ToString();
         var   EndDate = firstDayOfLastYearWeek.AddDays(7).ToString();
            res = obj.SummaryList("GetUserSummary", StartDate, EndDate, Id.ToString());
            ViewBag.Defultdate = StartDate;
            ViewBag.UserId= Id;
            return View(res);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult Edit(string Id = "")
        {
            ClientsBL cb = new ClientsBL();
            UserClientsBL uchb = new UserClientsBL();
            UserCityBL ucb = new UserCityBL();
            AD_DefinationBL db = new AD_DefinationBL();
            Sec_User user = new Sec_User();
         
            Sec_UserBL ubl = new Sec_UserBL();
            Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
            Sec_PermissionBL pl = new Sec_PermissionBL();
            Sec_UserDefinationTypeBL udt = new Sec_UserDefinationTypeBL();

            ViewBag.Titles = new List<SelectListItem>{
                                                                   new SelectListItem{ Text="Mr.", Value="1"},
                                                                   new SelectListItem{ Text="Mrs.", Value="2"},
                                                                    new SelectListItem{ Text="Miss.", Value="3"},
                                                                   new SelectListItem{ Text="Ms.", Value="4"},
                                                                    new SelectListItem{ Text="Sir.", Value="5"},
                                                                   new SelectListItem{ Text="DR", Value="6"}
                                                                };
            ViewBag.Hide = false;
            ViewBag.Team = false;
            if (Id == Convert.ToString(ViewBag.UserId))
            {
                ViewBag.Hide = true;
                List<OrgChart> rec = ubl.hierarchy("ByCompanyId", Convert.ToString(ViewBag.CompId));
                List<Chart> Data2 = FlatToHierarchy(rec,ViewBag.UserId);
                if(Data2.Count > 0) { ViewBag.Team = true; }
                else { ViewBag.Team =false; }
            }
            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
            Sec_UserBL ud = new Sec_UserBL();
            Sec_User User = ud.Single("ById", Id.ToString());
            ViewBag.UserTitle = User.Title;
            ViewBag.Hide2 = true;
            if (User.ReportToId.ToString() == Convert.ToString(ViewBag.UserId))
            {
                ViewBag.Hide2 = false;
            }
            if(Id == Convert.ToString(ViewBag.UserId))
                ViewBag.Hide2 = false;
            ViewBag.User = User;
            if (User == null)
            {

            }
            if (User.IsAdmin == true)
            {
                ViewBag.Hide = false;
                ViewBag.Hide2 = false;
                ViewBag.Team = true;
            }
            NewData();
            ViewBag.Id = User.CompanyId;
            ViewBag.RoleId = User.RoleId;
            ViewBag.Reports = sl.User("All");
            ViewBag.ReportTo = ud.ToList("All");
            ViewBag.Clients2 = sl.Clients("AllRecords");
            /////permissions
            user = ubl.Single("ById", Id.ToString());
            ///Project
            DataTable Table = udl.GetDataTable("All_Projects", User.ReportToId.ToString(), null, null);
            ViewBag.Projects = Table.ToList<PM_Projects>();
            DataTable Table1 = udl.GetDataTable("UserProjects", Id.ToString(), null, null);
            ViewBag.UserProjects = Table1.ToList<PM_Projects>();
            var r = pl.ToList("byUserId", Id.ToString());
            var d = udt.ToList("GetByUserId", Id.ToString());
            string UDSelected = null;
            foreach (var item in d)
            {
                UDSelected += item.DefinationTypeId + ",";
            }
            ViewBag.DIds = UDSelected;
            string Selected = null;
            foreach (var item in r)
            {
                Selected += item.Id + ",";
            }
            ViewBag.PIds = Selected;
            ViewBag.UId = Id;

            #region user Permissions on tab
            //Clients
            //ViewBag.Clients = cb.ToList("byStatus", User.ReportToId.ToString());
            ViewBag.Clients = cb.ToList("byStatus", "True",User.ReportToId.ToString());
            ViewBag.UserClients = uchb.ToList("byUserId",Id.ToString());
            //Cities
          
            ViewBag.Cities = db.ToList("AllCities");
                //db.ToList("UserCities",User.ReportToId.ToString());
            ViewBag.UserCities = ucb.ToList("byUserId",Id.ToString());
  
            ViewBag.Region = db.RegionsToList(User.ReportToId.ToString());
            //scope
            ViewBag.Scopes = db.ToList("Scopes", User.ReportToId.ToString());
            //ViewBag.Scopes = db.ToList("Scopes");
            ViewBag.UserScopes = db.ToList("UserScopes", Id.ToString());

            ViewBag.Permissions = pl.ToList("byRoleId",user.RoleId.ToString(),User.ReportToId.ToString());

            #endregion
            AD_DefinationTypesBL dtd = new AD_DefinationTypesBL();
            ViewBag.DefinationTypes = dtd.ToList("All", User.ReportToId.ToString());
            
         //User.RoleName = ViewBag.RoleName;
            return View("edit", User);
        }
        private void NewData(string RoleId = "")
        {
            AirView.Common.SelectedList sl2 = new AirView.Common.SelectedList();
            ViewBag.Roles = sl2.Roles();
            if (RoleId != "")
            {
                ViewBag.RoleName = sl2.Roles().Where(x => x.Value == RoleId).FirstOrDefault();
            }
        }
        //, ValidateAntiForgeryToken()
        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult New(Sec_User u, FormCollection dt, string type, string back)
        {
            Response res = new Response();
            try
            {

                string Password = string.Empty;
                if (u.Password != null)
                {
                    Password = Encryption.Encrypt(u.Password, true);

                }

                Sec_UserDL ud = new Sec_UserDL();
                int Id = ud.SaveNew_Update(Convert.ToInt64(u.UserId), Convert.ToInt64(u.RoleId), u.FirstName, u.LastName, u.UserName, Password, u.Email, u.Address, u.Contact, u.homeLatitude, u.homeLongitude, u.Title, u.Gender, u.CompanyId, u.Designation, u.HiringDate, u.ReportToId, u.Color, u.IsManager);

                if (Id > 0)
                {

                    string Picture = null;
                    Picture = UploadImg("/Content/Images/Profile/", "u-" + Id, 150, 150);
                    string Thumb = (!string.IsNullOrEmpty(Picture)) ? UploadImg("/Content/Images/Profile/", "thumb-" + Id, 32, 32) : null;

                    // check if image not uploaded
                    //if (back == "New")
                    //{
                    //    if (Picture == null) { Picture = "/Content/Images/Profile/Default.svg"; }
                    //}

                    if (!string.IsNullOrEmpty(Picture))
                    {
                        ud.Manage("UpdatePicture", Id.ToString(), Picture);
                    }

                    // for home location
                    if (!string.IsNullOrEmpty(Thumb))
                    {
                        Image img = Image.FromFile(Server.MapPath("~" + Thumb));
                        Graphics g = Graphics.FromImage(img);
                        g.DrawImage(Image.FromFile(Server.MapPath("~/Content/Images/Profile/home.png")), 20, 1, 13, 13);//new Point(-50, -50)
                        g.Dispose();
                        img.Save(Server.MapPath("~/Content/Images/Profile/home-" + Id + ".png"), ImageFormat.Png);
                    }
                }

                //TempData["msg_success"] = "Save successfully";
                //if (back == "New" || back == "Edit")
                //{
                //    return RedirectToAction("All");
                //}

                //return RedirectToAction("All");
                res.Status = "success";
                res.Message = "Save Successfully !";
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.ToString();
                return Json(res, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult All()
        {
            string Id = null;
            Sec_UserBL ub = new Sec_UserBL();
            ClientsBL cl = new ClientsBL();
            TempData["CompanyId"] = Id;
            ViewBag.Id = Id;
            var User = Session["user"] as LoginInformation;
            TempData["UserId"] = User.UserId.ToString();
            if (User.IsAdmin == true)
            {
                ViewBag.Client = null;
                if (Id == null)
                {
                    TempData["CompanyId"] = Convert.ToString(ViewBag.CompId);
                    ViewBag.Id = Convert.ToString(ViewBag.CompId);
                    // var Client = cl.Single("ById", Id).ClientName;
                    //ViewBag.Client =Client;
                }
                var Data = ub.ToList("All", Convert.ToString(ViewBag.UserId));
                return View(Data);
            }
            else
            {
                ViewBag.Client = null;
                if (Id == null)
                {
                    TempData["CompanyId"] = Convert.ToString(ViewBag.CompId);
                    ViewBag.Id = Convert.ToString(ViewBag.CompId);
                    // var Client = cl.Single("ById", Id).ClientName;
                    //ViewBag.Client =Client;
                }

                return View(ub.ToList("UserByUserId", Convert.ToString(User.UserId)));
            }
        }



        [HttpPost, ValidateAntiForgeryToken, IsLogin(CheckPermission = false)]
        public bool UpdatePassword(Int64 UserId, string Password)
        {
            try
            {
                Sec_UserDL ud = new Sec_UserDL();
                if (Password != null)
                {
                    Password = Encryption.Encrypt(Password, true);
                    if (!string.IsNullOrEmpty(Password))
                    {
                        return ud.Manage("UpdatePassword", UserId.ToString(), Password);

                    }
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }

        }

        [IsLogin(CheckPermission = false)]
        public bool IsExist(string filter, string value)
        {
            Sec_UserBL ud = new Sec_UserBL();
            Sec_User data = ud.Single(filter, value);
            if (data != null)
            {
                if (data.UserId > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        
        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult ToList(string filter, string value)
        {
            Sec_UserBL ud = new Sec_UserBL();
            List<Sec_User> rec = ud.ToList(filter, value);
            rec = rec.Select(c => { c.Password = ""; return c; }).ToList();
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        
        [IsLogin(CheckPermission = false)]
        public ActionResult Details(int id = 0)
        {
            if (ViewBag.IsAdmin)
            {
                goto data;
            }

            if (ViewBag.UserId != id)
            {
                TempData["msg_error"] = "You have not permission to this panel..";
                return View();
            }
            data:
            Sec_UserBL ud = new Sec_UserBL();
            var data = ud.Single("ById", id.ToString());
            return View(data);
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                Sec_UserDL ud = new Sec_UserDL();
                bool res = ud.Manage("Delete", Id.ToString());
                if (true)
                {
                    TempData["msg_seccess"] = "Delete successfully";

                }
            }
            catch (Exception ex)
            {

                TempData["msg_error"] = ex.Message;
            }

            return RedirectToAction("All");
        }

        [IsLogin(CheckPermission = false)]
        public bool UpdateActiveStatus(int Id, bool status)
        {
            Sec_UserDL ud = new Sec_UserDL();
            return ud.Manage("UpdateStatus", Id.ToString(), status.ToString());
        }
        
        [HttpPost,IsLogin(CheckPermission = false)]
        public ActionResult SelectableList(string filter, string value)
        {
            if (filter == "DevicesUsers")
            {
                Sec_UserDevicesBL udb = new Sec_UserDevicesBL();
                var rec = udb.SelectedList("All", "");
                return Json(rec, JsonRequestBehavior.AllowGet);
            }
            else if (filter == "All")
            {
                Sec_UserBL ub = new Sec_UserBL();
                var rec = ub.SelectedList("ByStatus", true.ToString());
                return Json(rec, JsonRequestBehavior.AllowGet);
            }


            return null;

        }
        
        [IsLogin(CheckPermission = false)]
        [HttpPost]
        public ActionResult record(int current, int rowCount, string searchPhrase, string Id = null)
        {
            Sec_UserBL ub = new Sec_UserBL();
            LoginInformation test = (LoginInformation) Session["user"] ;
            string UserId = test.UserId.ToString();
            string CompanyId = TempData["CompanyId"] as string;
            if (CompanyId == null)
                CompanyId = "0";
            TempData.Keep("CompanyId");
            current = (current == 0) ? 1 : current;
            rowCount = (rowCount == 0) ? 5 : rowCount;

            int offset = (current - 1) * rowCount;
            int TotalRecord = 0;
           
            //var rec = ub.Paging(offset, rowCount, searchPhrase, ref TotalRecord, CompanyId.ToString());
            var rec = ub.Paging(offset, rowCount, searchPhrase, ref TotalRecord, UserId);

            return Json(new { current = current, total = TotalRecord, rows = rec, rowCount = rowCount }, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult hierarchy(string ID="")
        {
            Chart Chart = new Chart();
            Sec_UserBL ub = new Sec_UserBL();
            List<OrgChart> rec = ub.hierarchy("ByCompanyId", Convert.ToString(ViewBag.CompId));
            var parent = rec.Where(x => x.ReportToId == 0).FirstOrDefault();
            var User = rec.Where(x => x.UserId.ToString() == Convert.ToString(ViewBag.UserId)).FirstOrDefault();
            if (ID == "")
            {
                //by Company
                var Data = FlatToHierarchy(rec, parent.UserId);
                Chart.children = Data;
                Chart.text = Text(parent);
                Chart.image = parent.Picture;
                ViewBag.OC = Chart;
            }
            else
            {
                ///////By tEAM
                var Data2 = FlatToHierarchy(rec, User.UserId);
                Chart.children = Data2;
                Chart.text = Text(User);
                Chart.image = User.Picture;
                // ViewBag.Team = Chart;
                ViewBag.OC = Chart;
            }
            return View();
        }
        [IsLogin(CheckPermission = false)]
        private List<Chart> FlatToHierarchy(IEnumerable<OrgChart> list, long parentId = 0)
        {
            return (from i in list
                    where i.ReportToId == parentId
                    select new Chart
                    {
                        image = i.Picture,
                        text = Text(i),
                        children = FlatToHierarchy(list, i.Id)
                    }).ToList();
        }
        [IsLogin(CheckPermission = false)]
        private text Text(OrgChart l)
        {
            text a = new text();
            a.contact = l.Contact;
            a.name = l.FirstName + " " + l.LastName + "???" + "/User/Edit?Id=" + l.Id;
            a.title = l.Designation;
            return a;
        }
    }
}