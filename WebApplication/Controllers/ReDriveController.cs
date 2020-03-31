using SWI.AirView.Models;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.Security.Filters;
using System;
using System.Web.Mvc;

namespace SWI.AirView.Controllers
{
    [IsLogin, ErrorHandling]
    public class ReDriveController : Controller
    {
        // GET: ReDrive
        [IsLogin(CheckPermission =false)]
        public ActionResult NewRequest(AV_NetLayerStatus ns)
        {
            Common.SelectedList sl = new Common.SelectedList();

            ViewBag.Reasons = sl.Reasons();
            ViewBag.RedriveTypes = sl.RedriveTypes();

            AV_NetLayerStatusBL nlsb = new AV_NetLayerStatusBL();
            AV_NetLayerStatus nls = nlsb.ToSingle("Get_ReDrive", Decimal.ToInt32(ns.NetworkModeId), Decimal.ToInt32(ns.BandId), Decimal.ToInt32(ns.CarrierId), Decimal.ToInt32(ns.SiteId),null);
            if (nls!=null && nls.redriveTypeId>0)
            {
                return PartialView("~/views/ReDrive/_NewNewRequest.cshtml", nls);
            }
            return PartialView("~/views/ReDrive/_NewNewRequest.cshtml", ns);
        }

        [IsLogin(CheckPermission =false), HttpPost]
        public ActionResult NewRequest(AV_NetLayerStatus ns, string post )
        {
            Response res = new Response();
            try
            {
                ReDriveDL rdd = new ReDriveDL();
                rdd.Manage("Insert",0, Decimal.ToInt32(ns.redriveTypeId), Decimal.ToInt32(ns.redriveReasonId),ns.redriveComments, Decimal.ToInt32(ns.SiteId), Decimal.ToInt32(ns.NetworkModeId), Decimal.ToInt32(ns.BandId), Decimal.ToInt32(ns.CarrierId), Decimal.ToInt32(ns.ScopeId), ViewBag.UserId);
                res.Status = "success";
                res.Message = "save successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}