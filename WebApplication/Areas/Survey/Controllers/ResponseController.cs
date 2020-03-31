using Library.SWI.Survey.BLL;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Survey.Controllers
{
    [IsLogin]
    public class ResponseController : Controller
    {
        // GET: Survey/Response
        [IsLogin(CheckPermission = false)]
        public JsonResult ToList(string Filter, string Value)
        {
            TSS_ResponseBL rb = new TSS_ResponseBL();
            var rec = rb.ToList(Filter, Value);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
    }
}