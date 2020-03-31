using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SWI.Security.Filters;
using SWI.Libraries.Security.Entities;

namespace WebApplication.Areas.Fleet.Controllers
{
    
    public class SettingsController : Controller
    {
        // GET: Fleet/FleetSettings
        [IsLogin(CheckPermission = false), ErrorHandling, HandleError]
        public ActionResult Index()
        {
            var id = Session["user"];
            var userId = (LoginInformation)id;
            ViewBag.UserId = userId.UserId;
            return View();
        }
    }
}