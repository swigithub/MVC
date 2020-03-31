using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace SWI.AirView.Controllers
{
    [IsLogin, ErrorHandling]
    public class ScopeTestController : Controller
    {
        // GET: ScopeTest
        public ActionResult New()
        {
            Common.SelectedList sl = new Common.SelectedList();
            ViewBag.Clients = sl.Clients("All");


            AD_DefinationBL db = new AD_DefinationBL();
            if (ViewBag.IsAdmin)
            {
                ViewBag.Cities = db.SelectedList("AllCities");
            }
            else
            {
                ViewBag.Cities = db.SelectedList("UserCities", Convert.ToString(ViewBag.UserId));
            }

            // ViewBag.Cities = sl.Cities(true);
             ViewBag.NetworkModes = db.SelectedList("NetworkModes",null,"-NetworkMode-");
           // ViewBag.NetworkModes = sl.NetworkModes();
            ViewBag.TestTypes = sl.Definations("byDefinationType","Test Types",  "-Select Test Type-");
            ViewBag.Scope = sl.Definations("byDefinationType", "Scope", "-Select Scope-");
            return View();
        }

        [HttpPost]
        public ActionResult New(List<AV_ScopeTests> Tests)
        {
            Response res = new Response();
            try
            {
                DataTable dt = Tests.ToDataTable();
                AV_ScopeTestsDL dtd = new AV_ScopeTestsDL();
                dt.Columns.Remove("ScopeTestId");
                dt.Columns.Remove("Test");
                dt.Columns.Remove("KeyCode");
                dt.Columns.Remove("DisplayText");

                dtd.Insert(dt);
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

        public ActionResult All()
        {
            return View();
        }
    }
}