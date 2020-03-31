using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class ErrorController : Controller
    {
        // GET: Project/Error

        public ActionResult Index()
        {
            return View();
        }
    }
}