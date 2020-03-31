using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class DefaultController : Controller
    {
        // GET: Project/Default
        public ActionResult Index()
        {
            return View();
        }
    }
}