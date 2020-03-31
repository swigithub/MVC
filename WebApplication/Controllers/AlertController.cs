using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class AlertController : Controller
    {
        AL_AlertBL model = new AL_AlertBL();
        // GET: Alert
        public ActionResult Index()
        {
            //AL_GetAlertSubscription Temp = new AL_GetAlertSubscription();
            //Temp = model.IsSubscribed("IsSubscribed", 11, 0, 70030, "Project Status Change");
            return View();
        }
    }
}