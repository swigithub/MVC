
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SWI.AirView.Controllers
{
    /*----MoB!----*/

    [IsLogin, ErrorHandling]
    public class CompanyController : Controller
    {

        #region Edit Region

        [IsLogin(CheckPermission = false)]
        public ActionResult Edit(decimal Id = 0)
        {
            ViewBag.tit = "Edit";
            ViewBag.Id = Id;
            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
          var Clienttypes = sl.Definations("byDefinationType", "Profile Type");
            ViewBag.SelectedClientTypes = Clienttypes;
            ViewBag.SelectedPClient = sl.Clients("AllVendors");
            ClientsBL ud = new ClientsBL();
            AD_Clients Company = ud.Single1("ById", Id.ToString());
            if(Company == null)
            {
                TempData["msg_error"] = "Record Not Exist or You Are Not Authorize to access !";
                return RedirectToAction("All","Client");
            }
            foreach (var item in Clienttypes)
            {
                if (item.Text != "Company" && (item.Value == Company.ClientTypeId.ToString()))
                {
                    return RedirectToAction("Edit","Client", new { Id = Id });
                }
            }
            return View("Edit", Company);
        }

        [HttpPost]
        public ActionResult Edit(AD_Clients cl, List<AD_ClientContacts> con, List<AD_ClientAddress> add)
        {
            Response res = new Response();
            string fname = string.Empty;

            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = cl.ClientPrefix;
                        }
                        fname = Path.Combine(Server.MapPath("~/Content/Images/ClientLogo"), fname + "_logo." + Path.GetExtension(file.FileName));
                        file.SaveAs(fname);
                    }
                }
                int Id = 0;
                ClientsBL rb = new ClientsBL();
                cl.Logo = fname;
                Id = rb.Manage("Update", cl);
                dbDataTable ddt = new dbDataTable();
                //DataTable dtcon = ddt.List();
                //foreach (var item in con)
                //{
                //    myDataTable.AddRow(dtcon, "Value1", item.ContactPerson, "Value2", item.Designation, "Value3", item.Gender, "Value4", item.Title, "Value5", item.ContactNo,
                //                        "Value6", item.ContactType, "Value7", item.IsPrimary, "Value8", Id, "Value9", item.UserId, "Value10", item.RegionId, "Value11", item.CityId, "Value12", item.IsActive, "Value13", item.ReportToId, "Value14",item.ContactId);
                //}
                //AD_ClientContactsDL ccb = new AD_ClientContactsDL();
                //AD_ClientContacts clobj = new AD_ClientContacts();
                //ccb.Manage("UpdateBulk", dtcon);

                DataTable dt = ddt.List();
                foreach (var item in add)
                {
                    if (item.Address != null || item.Address != "")
                    {
                        myDataTable.AddRow(dt, "Value1", item.Address, "Value2", item.Street, "Value3", item.CityId, "Value4", item.StateId, "Value5", item.CountryId,
                                       "Value6", item.ZipCode, "Value7", item.IsHeadOffice, "Value8", Id, "Value9", item.IsActive, "Value10", item.AddressId);
                    }
                }
                AD_ClientAddressDL classaddress = new AD_ClientAddressDL();
                classaddress.Manage("UpdateBulk", dt);

                //res.Status = "success";
                // res.Message = "save successfully";
                TempData["msg_success"] = "Update successfully";
            }
            catch (Exception ex)
            {

                // res.Status = "danger";
                //  res.Message = ex.Message;
                TempData["msg_error"] = ex.Message;
            }
            return RedirectToAction("Edit", new { @Id = cl.ClientId });
            //Json(res, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region New region
        // GET: Client
        public ActionResult NewSingle(string title)
        {
            ViewBag.tit = title;
            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
            ViewBag.SelectedClientTypes = sl.Definations("byDefinationType", "Profile Type");
            ViewBag.SelectedPClient = sl.Clients("AllVendors");
            return View();
        }


        [HttpPost]
        public ActionResult NewSingle(AD_Clients cl, AD_ClientContacts con, AD_ClientAddress claddress)
        {
            Response res = new Response();
            try
            {
                ClientsBL rb = new ClientsBL();
                AD_ClientContactsBL clcon = new AD_ClientContactsBL();
                AD_ClientAddressBL addressbl = new AD_ClientAddressBL();

                int ClientId = 0;
                int ContactId = 0;
                int AddressId = 0;
                if (cl.ClientId > 0 || con.ContactId > 0 || claddress.AddressId > 0)
                {
                    ClientId = rb.Manage("Update", cl);
                    ContactId = clcon.Manage("Update", con, null);
                    AddressId = addressbl.Manage("Update", claddress);
                    res.Status = "success";
                    res.Message = "update successfully";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    

                    ClientId = rb.Manage("Insert", cl);
                    con.ClientId = ClientId;
                    ContactId = clcon.Manage("Insert", con, null);
                    claddress.ClientId = ClientId;
                    AddressId = addressbl.Manage("Insert", claddress);
                    res.Status = "success";
                    res.Message = "save successfully";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult New()
        {
            ClientsBL ub = new ClientsBL();
            var List = ub.ToList("Company");
           
            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
            ViewBag.SelectedClientTypes = sl.Definations("byDefinationType", "Profile Type");
            ViewBag.SelectedPClient = sl.Clients("AllVendors");
            if (List.Count> 0)
                return RedirectToAction("Edit", new { @Id = List[0].ClientId });
            else
            {
                return View();
            }
        }
        
        [HttpPost]
        public ActionResult New(AD_Clients cl, List<AD_ClientContacts> con, List<AD_ClientAddress> add)
        {
            decimal Id = 0;
            Response res = new Response();
            string fname = string.Empty;
            try
            {
                ClientsBL rb = new ClientsBL();
                var List = rb.ToList("Company");
               
                if (List.Count> 0)
                {
                    res.Status = "danger";
                    res.Message = "Company Already Exist !";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                string Extension = "";
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = cl.ClientPrefix;
                        }
                        fname = Path.Combine(Server.MapPath("~/Content/Images/ClientLogo"), fname + "_logo" + Path.GetExtension(file.FileName));
                        file.SaveAs(fname);
                        Extension = Path.GetExtension(file.FileName);
                    }
                }

                cl.Logo = "/Content/Images/ClientLogo/" + cl.ClientPrefix + "_logo" + Extension;
                Id = rb.Manage("Insert", cl);
                dbDataTable ddt = new dbDataTable();
                DataTable dtcon = ddt.List();
                DataTable dt = ddt.List();
                foreach (var item in add)
                {
                    myDataTable.AddRow(dt, "Value1", item.Address, "Value2", item.Street, "Value3", item.CityId, "Value4", item.StateId, "Value5", item.CountryId,
                                        "Value6", item.ZipCode, "Value7", item.IsHeadOffice, "Value8", Id, "Value9", item.IsActive);
                }
                AD_ClientAddressDL claddress = new AD_ClientAddressDL();
                claddress.Manage("Insert", dt);
                res.Status = "success";
                res.Value = Id;
                res.Message = "Save successfully";
                res.Value = Id.ToString();



            }
            catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = ex.Message;

            }

            if (res.Status == "success")
            {
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(res, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion
        #region Address

        [IsLogin(CheckPermission = false)]
        public ActionResult Address(int Id)
        {

            try
            {
                ViewBag.Count = 0;
                SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
                ViewBag.SelectedCountry = sl.Definations("Countries");
                ViewBag.SelectedState = sl.Definations("States");
                ViewBag.Cities = sl.Definations("Cities");
                ViewBag.SelectedPClient = sl.Clients("All");

                AD_ClientAddressBL adress = new AD_ClientAddressBL();
                AD_ClientAddress Address = adress.Single("ById", Id.ToString());
                if (Id != 0)
                {
                    List<AD_ClientAddress> Addresses = adress.ToList("ById", Id.ToString());
                    if (Addresses.Count > 0)
                    {
                        Address.Addresses = Addresses;
                        ViewBag.Count = Addresses.Count;
                    }
                }
                return PartialView("~/Views/Client/_ClientAddress.cshtml", Address);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Client/_ClientAddress.cshtml", null);
            }
        }

        #endregion

        #region ALL,ToList
        public ActionResult All()
        {
            return RedirectToAction("All","Client");
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult ToList(string filter, string value)
        {

            ClientsBL ub = new ClientsBL();
            var rec = ub.ToList(filter, value);

            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}