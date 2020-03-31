using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.Entities;
using SWI.Security.Filters;
using System;
using System.Web.Mvc;

namespace SWI.Security.Controllers
{
    /*----MoB!----*/

    [IsLogin, ErrorHandling]
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Sec_Role r)
        {
            try
            {
                Sec_RoleBL rb = new Sec_RoleBL();
                Int64 RoleId = 0;
                if (r.RoleId > 0)
                {
                    RoleId = rb.Manage("Update", r);
                }
                else
                {
                    RoleId = rb.Manage("Insert", r);
                }

                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["msg_error"] = ex.Message;
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            Sec_RoleBL rl = new Sec_RoleBL();

            return View("New", rl.ToSingle("ById", id.ToString()));
        }

        public ActionResult All()
        {
            Sec_RoleBL rl = new Sec_RoleBL();
            return View(rl.ToList("All"));
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                Sec_RoleBL rbl = new Sec_RoleBL();
                Sec_Role r = new Sec_Role();
                r.RoleId = Id;
                rbl.Manage("Delete", r);
            }
            catch (Exception ex)
            {
                TempData["msg_error"] = ex.Message;
            }

            return RedirectToAction("All");
        }

        [IsLogin(CheckPermission = false, Return = "")]
        public bool UpdateActiveStatus(int Id, bool status)
        {
            try
            {
                Sec_RoleBL rbl = new Sec_RoleBL();
                Sec_Role r = new Sec_Role();
                r.RoleId = Id;
                r.IsActive = status;
                rbl.Manage("UpdateStatus", r);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}