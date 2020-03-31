using System.Web.Mvc;
using SWI.Security.Filters;
using AirView.DBLayer.Fleet.BLL;
using System.Linq;
using AirView.DBLayer.Fleet.Model;
using System.Net;
using System.Collections.Generic;
using System;
using SWI.Libraries.Common;
using System.Web;
using SWI.AirView.Common.ResizeUploadImg;
using System.IO;
using Microsoft.AspNet.SignalR;
using System.Net.Sockets;
using SWI.AirView.Common;
using System.Text;
using System.Threading;

namespace WebApplication.Areas.Fleet.Controllers
{
    [IsLogin, ErrorHandling, HandleError]
    public class FleetReportController : Controller
    {
        // GET: Fleet/FleetReport
        [IsLogin(CheckPermission = false)]
        public ActionResult Index()
        {
            return View();
        }
    }
}