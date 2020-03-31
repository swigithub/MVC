using System;
using System.Web;
using System.Web.Mvc;

namespace SWI.AirView.Controllers
{
    public class ErrorController : Controller
    {
        /*----MoB!----*/
        // GET: Error
        public ActionResult Index(string type)
        {
            if (type=="noScript")
            {
                TempData["msg_error"] = " To use this application JavaScript is required. Enable browser Script.";
            }else
            if (type == "noUrlFound")
            {
                TempData["msg_error"] = " The URL you are trying to access is not found.";
            }

            HttpCookie temp = Request.Cookies["Role_User"];
            if (temp != null)
            {
                temp.Expires = DateTime.Now;
                Response.SetCookie(temp);
            }
            return View();
        }

        public ActionResult Browser()
        {
            TempData["msg_error"] = "This application not for IE.";
            return View("Index");
        }
    }
}