using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Alert.Controllers
{
    [IsLogin, ErrorHandling, HandleError]
    public class NotificationController : Controller
    {
        // GET: Alert/Notification
        [IsLogin(CheckPermission = false)]
        public ActionResult Index()
        {
            return View();
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult All()
        {
            return View();
        }
    }
}