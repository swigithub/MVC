using System;
using System.Web;
using System.Web.Mvc;
using SWI.Libraries.Security.Entities;
using SWI.Libraries.Security.BLL;
using SWI.Security.Filters;
using SWI.AirView.Common;
using AirView.DBLayer.Security.Entities;
using SWI.Libraries.Security.DAL;

namespace SWI.Security.Controllers
{
    /*----MoB!----*/
    [ErrorHandling]
    public class LoginController : Controller
    {
        //GET: Login
        public ActionResult Index(string tctl, string tact)
        {
            TempData["msg_error"] = null;
            ActionResult temp = null;
            try
            {
                HttpCookie myCookie = Request.Cookies["AirView"];
                if (myCookie != null)
                {
                    Sec_User user = new Sec_User();
                    Sec_UserBL ubl = new Sec_UserBL();
                    user = ubl.Single("Login", Encryption.DecryptSHA256( myCookie.Value));

                    if (user != null)
                    {
                        mySession(user);

                        if (Session["PrevUrl"] != null)
                        {
                            string url = Session["PrevUrl"].ToString();
                            Session["PrevUrl"] = null;
                            return Redirect(url);
                        }
                        else if (!string.IsNullOrEmpty(tctl) && !string.IsNullOrEmpty(tact))
                        {
                            return RedirectToAction(tact, tctl);
                        }
                        else
                        {
                            temp = Redirect(user.DefaultUrl);
                        }

                        return temp;
                    }
                }
            }
            catch (Exception ex)
            {

                TempData["msg_error"] = ex.Message;
            }
            

            ViewBag.TController = tctl;
            ViewBag.TAction = tact;

            return View();
        }
        private void mySession(Sec_User user) {
            if (user != null)
            {

                LoginInformation li = new LoginInformation();

                Sec_PermissionBL pl = new Sec_PermissionBL();
                Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
                var per = pl.ToList("byUserId_ModuleId", user.UserId.ToString(), "AIRIVEW_PORTAL");
                var Pper = udl.GetDataTable("UserProjects", user.UserId.ToString(), null, null);
                Session["user"] = null;
                //System.Web.HttpContext.Current.Application["UserId"] = user.UserId.ToString();
                //System.Web.HttpContext.Current.Application.Add(user.UserId.ToString(), user.UserId.ToString());
                System.Web.HttpContext.Current.Application["User"] = li.set_user_data(user, per, Pper);
                Session.Add("user", li.set_user_data(user, per,Pper));
            }
        }

        [HttpPost]
        public ActionResult Login(FormCollection frm, string tctl, string tact)
        {
            try
            {
                string Remember = frm["Remember"];

                Sec_User user = new Sec_User();
                Sec_UserBL ubl = new Sec_UserBL();
                user = ubl.Single("Login", frm["UserName"]);
                string Password =Encryption.Encrypt(frm["Password"].ToString(),true);
               
                if (user != null)
                {
                    if (user.Password != Password)
                    {
                        TempData["msg_error"] = "Wrong Password Entered.";
                        return RedirectToAction("index");
                    }
                    else
                    {
                        if (Remember != "false")
                        {
                            HttpCookie c = new HttpCookie("AirView");
                            c.Expires = DateTime.Today.AddDays(7);
                            c.Value = Convert.ToString(Encryption.EncryptSHA256( user.UserName));
                            //c.Values.Add("UserName", user.UserName);

                            //c.Value = Convert.ToString(user.UserId);
                            //c.Values.Add("UserName", user.UserName);
                            Response.SetCookie(c);
                        }

                        mySession(user);

                        if (Session["PrevUrl"]!=null)
                        {
                            string url = Session["PrevUrl"].ToString();
                            Session["PrevUrl"] = null;
                            return Redirect(url);
                        }else if (!string.IsNullOrEmpty(tctl) && !string.IsNullOrEmpty(tact))
                        {
                            return RedirectToAction(tact, tctl);
                        }
                        else
                        {                           

                            return Redirect(user.DefaultUrl);

                        }
                    }
                }
                else
                {
                    TempData["msg_error"] = "Wrong Username Entered.";
                    return RedirectToAction("index");
                }
            }
            catch (Exception ex)
            {

                TempData["msg_error"] = ex.Message;
                return RedirectToAction("index");
            }

        }

        public ActionResult Logout()
        {
            HttpCookie temp = Request.Cookies["AirView"];
            if (temp != null)
            {
                temp.Expires = DateTime.Now;
                Response.SetCookie(temp);
            }

            Session["user"] = null;
            Session.Abandon();
           
            return RedirectToAction("index");
        }
    }
}