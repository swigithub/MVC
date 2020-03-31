using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Survey.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Survey/Default
        public ActionResult Index()
        {
            return View();
        }
    }
}