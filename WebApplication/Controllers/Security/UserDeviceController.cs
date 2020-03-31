using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.Entities;
using SWI.Security.Filters;
using System;
using System.Web.Mvc;

namespace SWI.Security.Controllers
{
    /*----MoB!----*/
    [IsLogin, ErrorHandling]
    public class UserDeviceController : Controller
    {
        // GET: UserDevice
        public ActionResult Index(int Id = 0)
        {
            ViewBag.Id = Id;
            return View();
        }
        [IsLogin(Return = "")]
        public ActionResult New(int id = 0)
        {
            ViewBag.UserId = id;

            return PartialView("~/views/UserDevice/_new.cshtml");
        }

        public ActionResult Edit(int id = 0)
        {
            Sec_UserDevicesBL udbl = new Sec_UserDevicesBL();
            var rec = udbl.Single("byUserId", id.ToString());
            if (rec.DeviceId > 0)
            {
                ViewBag.UserId = rec.UserId;
            }
            return PartialView("~/views/UserDevice/_new.cshtml", rec);
        }

        public string Delete(int id = 0)
        {
            try
            {
                Sec_UserDevicesBL udbl = new Sec_UserDevicesBL();
                Sec_UserDevices dev = new Sec_UserDevices();
                dev.DeviceId = id;
                var rec = udbl.Manage("Delete", dev);
                return rec.ToString();
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
           
        }

        public string Active(int DeviceId,bool Status)
        {
            try
            {
                Sec_UserDevicesBL udbl = new Sec_UserDevicesBL();
                Sec_UserDevices dev = new Sec_UserDevices();
                dev.DeviceId = DeviceId;
                dev.IsActive = Status;
                var rec = udbl.Manage("UpdateStatus", dev);
                return rec.ToString();
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        [HttpPost]
        public string New(string[] DeviceId, int UserId, string[] Manufacturer, string[] Model, string[] IMEI, string[] MAC)
        {
            try
            {
                Sec_UserDevicesBL udb = new Sec_UserDevicesBL();
                Sec_UserDevices dev;
                for (int i = 0; i < Manufacturer.Length; i++)
                {
                    dev = new Sec_UserDevices();
                    if (!string.IsNullOrEmpty(DeviceId[i]))
                    {
                        dev.DeviceId = int.Parse(DeviceId[i]) ;
                    }
                    dev.UserId = UserId;
                    dev.Manufacturer=Manufacturer[i];
                    dev.Model=Model[i];
                    dev.IMEI=IMEI[i];
                    dev.MAC = MAC[i];

                    if (dev.DeviceId>0)
                    {
                        udb.Manage("Update", dev);
                    }
                    else if (udb.Manage("Insert", dev))
                    {
                       // TempData["msg_success"] = "success";

                    }

                }


                return null;
            }
            catch (Exception ex)
            {
               // TempData["msg_error"] = ex.Message;
                return ex.Message;
            }

        }

        public ActionResult byUser(int id = 0)
        {
            ViewBag.UserId = id;
            Sec_UserDevicesBL udbl = new Sec_UserDevicesBL();
            var rec = udbl.ToList("byUserId", id.ToString());
            return PartialView("~/views/UserDevice/_byUser.cshtml", rec);
        }


        
    }
}