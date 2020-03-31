
using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Security.BLL;
using AirView.DBLayer.Security.Entities;
using SWI.AirView.Common;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AD.DAL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SWI.AirView.Controllers
{
    /*----MoB!----*/

    [IsLogin, ErrorHandling]
    public class EquipmentController : Controller
    {
        // GET: Equipment

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult New()
        {
            AD_DefinationBL db = new AD_DefinationBL();
            ViewBag.UETypes = db.SelectedList("byDefinationType", "UE Type", "-Select Device-");

            //TempData["CompanyId"] = Convert.ToString(ViewBag.CompId);
            //ViewBag.UEOwner = Convert.ToString(ViewBag.CompId);

            ClientsBL cBl = new ClientsBL();
            ViewBag.UEOwner = cBl.SelectedList("All", null, "-Select Owner-");


            return View();
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult Edit(Int64 id)
        {
            AD_DefinationBL db = new AD_DefinationBL();
            ViewBag.UETypes = db.SelectedList("byDefinationType", "UE Type", "-select Device-");
            AD_UserEquipmentsBL ueb = new AD_UserEquipmentsBL();

            ClientsBL cBl = new ClientsBL();
            ViewBag.UEOwner = cBl.SelectedList("All", null, "-Select Owner-");

            return View("new", ueb.ToSingle("ById", id.ToString()));
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult New(AD_UserEquipment ue)
        {
            Response res = new Response();
            try
            {
                AD_UserEquipmentsBL ueb = new AD_UserEquipmentsBL();
                ue.IsActive = true;
                if (ue.UEId > 0)
                {
                    ueb.Manage("Update", ue);
                }
                else
                {
                    ueb.Manage("Insert", ue);
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



        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult Paging(int current, int rowCount, string searchPhrase)
        {
            AD_UserEquipmentsBL ueb = new AD_UserEquipmentsBL();
            Int64 count = 0;

            current = (current == 0) ? 1 : current;
            rowCount = (rowCount == 0) ? 5 : rowCount;
            int offset = (current - 1) * rowCount;
            var rec = ueb.Paging(offset, rowCount, searchPhrase, ref count);
            return Json(new { current = current, total = count, rows = rec, rowCount = rowCount }, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult ToList(string filter, string value, string value2, string value3)
        {
            AD_UserEquipmentsBL ueb = new AD_UserEquipmentsBL();

            var rec = ueb.ToList(filter, value, value2, value3);
            return Json(new { data = rec }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Movement()
        {
            string TransType = "";

            if (Request.QueryString["UEId"] != null)
            {
                ViewBag.UEId = Convert.ToInt32(Request.QueryString["UEId"]);
            }

            if (Request.QueryString["UserId"] != null)
            {
                ViewBag.UserId = Request.QueryString["UserId"];
            }

            if (Request.QueryString["UETypeId"] != null)
            {
                ViewBag.UETypeId = Request.QueryString["UETypeId"];
            }


            if (Request.QueryString["UEsts"] != null)
            {
                TransType = Request.QueryString["UEsts"];
            }

            AD_DefinationBL db = new AD_DefinationBL();

            ViewBag.UEStatus = db.SelectedList("byDefinationType", "UE Status", "-Transaction Type-");

            foreach (var v in ViewBag.UEStatus)
            {
                if (v.Text == TransType)
                { ViewBag.UEsts = v.Value; }
            }

            ViewBag.UETypes = db.SelectedList("byDefinationType", "UE Type", "-select-");

            AD_UserEquipmentsBL ueb = new AD_UserEquipmentsBL();
            ViewBag.Equipments = ueb.SelectedList("All", "", "-select-");

            Sec_UserBL ub = new Sec_UserBL();
            ViewBag.Users = ub.SelectedList("ByStatus", true.ToString(), "-Users-");

            ViewBag.IssueToUsers = ub.SelectedList("ByStatus", true.ToString(), "-Users-");

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Movement(List<AD_UEMovement> mov, string UEStatus)
        {
            Response res = new Response();

            try
            {
                AD_UEMovementBL ueb = new AD_UEMovementBL();

                foreach (var item in mov)
                {
                    ueb.Manage(UEStatus, item);
                }

                //------------------------------
                if (UEStatus == "Issue")
                {
                    long UEId = mov.Select(x => x.UEId).Single();
                    long UserId = mov.Select(x => x.UserId).Single();

                    AD_UserEquipmentsBL uebl = new AD_UserEquipmentsBL();
                    AD_UserEquipment uEqu = uebl.ToSingle("ById", UEId.ToString());

                    Sec_UserBL u = new Sec_UserBL();
                    Sec_User usr = u.Single("ById", UserId.ToString());
                    WebConfig wc = new WebConfig();
                    string UeModel = uEqu.Model;
                    string Manufacturer = uEqu.Manufacturer;
                    string SerialNo = uEqu.SerialNo;

                    string AVStoreURL = wc.AppSettings("AVStoreURL");

                    string Url = "<a href=" + AVStoreURL + ">AirView Store</a>";

                    string UserName = usr.UserName;
                    string ToEmail = usr.Email;

                    string Subject = "UE Device Issued";
                    string Body = "<h1>Hi, " + UserName + "</h1>" +
                         "<p>A new device is issued to your account.</p>" +
                         "<table border=" + 1 + " cellpadding=" + 2 + " cellspacing=" + 0 + " width = " + 400 + ">" +
                         "<tr bgcolor='#F5F5F5'><td><strong>Model</strong></td><td>" + UeModel + "</td></tr>" +
                         "<tr bgcolor='#F5F5F5'><td><strong>Manufacturer</strong></td><td>" + Manufacturer + "</td></tr>" +
                         "<tr bgcolor='#F5F5F5'><td><strong>SerialNo</strong></td><td>" + SerialNo + "</td></tr>" +
                         "</table>" +
                         "<p>Please download and install AirView Store by clicking here: " + Url + "</p>";


                    Thread thread = new Thread(() => SendEmail(Subject, ToEmail, Body));
                    thread.Start();

                    //-----------------------------
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

        private void SendEmail(string Subject, string ToEmail, string Body)
        {
            WebConfig wc = new WebConfig();
            string SmtpServer = wc.AppSettings("SmtpServer");
            string SmtpServerPort = wc.AppSettings("SmtpServerPort");
            string FromEmail = wc.AppSettings("FromEmail");
            string FromEmailPassword = wc.AppSettings("FromEmailPassword");

            //string ToEmail = wc.AppSettings("ToEmail");

            FromEmailPassword = Encryption.Decrypt(FromEmailPassword, true);
            Email em = new Email(SmtpServer, Convert.ToInt32(SmtpServerPort), FromEmail, FromEmailPassword);
            try
            {
                if (SmtpServer != null && SmtpServerPort != null && FromEmail != null && FromEmailPassword != null && ToEmail != null)
                {
                    em.SendEmail(Subject, Body, ToEmail);
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }


        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult Devices(string filter, string value)
        {
            if (filter == "Issue")
            {
                AD_UserEquipmentsBL ueb = new AD_UserEquipmentsBL();

                //var rec = ueb.SelectedList("ReturnedDevice", value, null);
                var rec = ueb.SelectedList("AvailableDevices", value, null);
                return Json(rec, JsonRequestBehavior.AllowGet);
            }
            else if (filter == "Transfer")
            {
                AD_UserEquipmentsBL ueb = new AD_UserEquipmentsBL();

                var rec = ueb.SelectedList("IssuedDevice", value, null);
                return Json(rec, JsonRequestBehavior.AllowGet);


            }
            else if (filter == "Return")
            {
                Sec_UserDevicesBL udb = new Sec_UserDevicesBL();
                var rec = udb.SelectedList("byUserId", value, null, "Devices");
                return Json(rec, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult EquipmentStatus(string SerialNo, bool IsActive)
        {
            Response res = new Response();

            try
            {
                AD_UserEquipmentDL ue = new AD_UserEquipmentDL();
                ue.ManageStatus("Set_IsActive", SerialNo, IsActive, null, null, null, null);
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




        [IsLogin(CheckPermission = false)]
        public ActionResult ModuleApps()
        {
            AD_DefinationBL pl = new AD_DefinationBL();
            List<AD_Defination> apps = pl.ToList("byModuleApps", "0");
            ViewBag.Apps = apps;
            return PartialView("~/Views/Equipment/_ModuleApps.cshtml", apps);
        }


        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult UserModuleApps(Int64 UserId)
        {
            Sec_UserSettingsBL ul = new Sec_UserSettingsBL();
            var res = ul.ToList("byUserId", UserId.ToString());

            string appIds = null;
            foreach (var item in res)
            {
                appIds += item.TypeValue + ",";
            }
            ViewBag.appIds = appIds;

            return Json(appIds, JsonRequestBehavior.AllowGet);
        }


        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult UserApps(IEnumerable<string> checkedValues, Int64 UserId, string UEId, Int64 TypeId = 0)
        {
            foreach (var item in checkedValues)
            {
                Sec_UserSettingsDL usd = new Sec_UserSettingsDL();
                usd.Manage("Insert", UserId, TypeId, item.ToString(), UEId);
            }
            var rec = "Success";
            return Json(rec, JsonRequestBehavior.AllowGet);
        }


        [IsLogin(CheckPermission = false)]
        public ActionResult UEIssues()
        {
            return PartialView("~/Views/Equipment/_UEIssues.cshtml");
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult AddUEIssues(Int64 UEId, string Description, Int64 UserId, DateTime Date)
        {
            INV_UEIssuesDL ued = new INV_UEIssuesDL();

            if (Description != null && Description != "")
            {
                var result = ued.Manage("Insert", UEId, Description.Trim(), UserId, Date);
                if (result)
                {
                    var rec = "Success";
                    return Json(rec, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var rec = "Error";
                    return Json(rec, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var rec = "Error";
                return Json(rec, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult UEIssuesList(Int64 UEId)
        {
            INV_UEIssuesBL ueb = new INV_UEIssuesBL();
            List<INV_UEIssues> Issues = ueb.ToList("byUEId", UEId.ToString());
            return Json(Issues, JsonRequestBehavior.AllowGet);
        }
    }
}