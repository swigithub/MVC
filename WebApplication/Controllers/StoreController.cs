using AirView.DBLayer.AirView.BLL;
using SWI.AirView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace WebApplication.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult stores()
        {
            Response res = new Response();
            try
            {
               AD_StoreBL wob = new AD_StoreBL();
                var dt = wob.Get(null,"get",null);
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(dt);
                res.Value = JSONString;
                res.Status = "success";
                return Json(new { response = res }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
                return Json(new { response = res }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Search(DateTime date)
        {
            Response res = new Response();
            try
            {
                AD_StoreBL wob = new AD_StoreBL();
                var dt = wob.Get(null, "search", date);
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(dt);
                res.Value = JSONString;
                res.Status = "success";
                return Json(new { response = res }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
                return Json(new { response = res }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}