using SWI.Libraries.AD.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class ReportTestController : Controller
    {
        // GET: ReportTest
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetReport(string Filter)
        {
            try
            {
                AD_DefinationBL db = new AD_DefinationBL();
                return Json(db.ToList(Filter, "Report"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        [HttpGet]
        public ActionResult GetColumn(string reportId)
        {
            try
            {
                AD_DefinationBL db = new AD_DefinationBL();
                var result = db.ToColumnList("NET_LAYER_REPORT", reportId.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult GetReportrecord(string reportFilter, string where,string select,string group)
        {
            
            try
            {
            
                AD_DefinationBL db = new AD_DefinationBL();
                var result = db.ToDefinitionList(reportFilter, where,select, group);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult GetFilters(string Type)
        {
            try
            {
                AD_DefinationBL db = new AD_DefinationBL();
                return Json(db.ToList("byDefinationType", Type), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected override JsonResult Json(object data, string contentType,
           Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
    }
}