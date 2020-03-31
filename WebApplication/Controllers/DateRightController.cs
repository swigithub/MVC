
using System.Web.Mvc;
using SWI.Libraries.Security.Entities;
using AirView.DBLayer.Security.DAL;
using SWI.Libraries.Security.BLL;
using SWI.Security.Filters;

namespace SWI.Security.Controllers
{
    /*----MoB!----*/

    [IsLogin, ErrorHandling]
    public class DateRightController : Controller
    {

        [IsLogin(CheckPermission =true)]
        public ActionResult New(int Id)
        {
            ViewBag.UserId = Id;

            UserDateRightsBL udrb = new UserDateRightsBL();

            return View(udrb.Single("byUserId", Id.ToString()));
        }

        [HttpPost]
        public ActionResult New(UserDateRights udr)
        {
            UserDateRightsDL urd = new UserDateRightsDL();
          bool res=  urd.Insert_Update(udr.UserId,udr.DaysForward, udr.DaysBack,udr.IsActive);
            if (res)
            {
                TempData["msg_success"] = "save successfully";
                return Redirect(Request.UrlReferrer.ToString());
                //return RedirectToAction("all", "user");
            }
            else
            {
                TempData["msg_error"] = "some error";
                return View(new {id=udr.UserId });

            }


        }


        [IsLogin(Return = "")]
        public ActionResult UserDates(int Id) {

            UserDateRightsBL udrb = new UserDateRightsBL();
            return Json(udrb.Single("byUserId", Id.ToString()), JsonRequestBehavior.AllowGet);
        }

    }
}