using AirView.DBLayer.Security.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWI.Security.Filters
{
    /*----MoB!----*/
    public class Permission
    {
        private static int PCUserId;
        public static List<Sec_Permission> UnUsedPermission = new List<Sec_Permission>();
        public static bool AllowUrl(string url)
        {
            bool result = false;
            var login_data = HttpContext.Current.Session["user"] as  LoginInformation;
            if (login_data!=null)
            {
                var login_permission = login_data.Permissions.Where(m => m.URL.ToLower().Equals(url.ToLower())).FirstOrDefault();
                var obj= login_data.Permissions.Where(m => m.URL.ToLower().Equals(url.ToLower())).FirstOrDefault();
                if (login_permission != null)
                {
                    result = true;
                   
                }
                // return login_data.Permissions.Any(m=>m.URL.Equals(url));
            }
            
            return result;
        }
        public static Sec_UserProjects AllowProject(long Id)
        {
            Sec_UserProjects result = null;

            var login_data = HttpContext.Current.Session["user"] as LoginInformation;
            if (login_data != null)
            {
                var login_permission = login_data.ProjectPermissions.Where(m => m.ProjectId.Equals(Id)).FirstOrDefault();
                if (login_permission != null)
                {
                   
                    result = login_permission;
                }
                // return login_data.Permissions.Any(m=>m.URL.Equals(url));
            }

            return result;
        }
        public static void AddProject(long Id)
        {
            var login_data = HttpContext.Current.Session["user"] as LoginInformation;
            Sec_UserProjects obj = new Sec_UserProjects();
            obj.ProjectId = Id;
            obj.UserId = login_data.UserId;
            LoginInformation li = new LoginInformation();
            li = login_data;
            li.ProjectPermissions.Add(obj);
            HttpContext.Current.Session["user"] = null;
            HttpContext.Current.Session["user"] = li;
        }
        public static void UpdateEntity(long Id)
        {

            Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
            var login_data = HttpContext.Current.Session["user"] as LoginInformation;
            LoginInformation li = login_data;
            var Pper = udl.GetDataTable("UserProjects", Id.ToString(), null, null);
            List<Sec_UserProjects> UserProjects = Pper.ToList<Sec_UserProjects>();
            li.ProjectPermissions = UserProjects;
            HttpContext.Current.Session["user"] = null;
            HttpContext.Current.Session["user"] = li;
        }
        public static void UpdateSession(string Username)
        {
            Sec_User user = new Sec_User();
            Sec_UserBL ubl = new Sec_UserBL();
            user = ubl.Single("Login", Username);
            if (user != null)
            {

                LoginInformation li = new LoginInformation();

                Sec_PermissionBL pl = new Sec_PermissionBL();
                Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
                var per = pl.ToList("byUserId_ModuleId", user.UserId.ToString(), "AIRIVEW_PORTAL");
                var Pper = udl.GetDataTable("UserProjects", user.UserId.ToString(), null, null);
                var obj = li.set_user_data(user, per, Pper);
                HttpContext.Current.Session["user"] = null;
                HttpContext.Current.Session["user"] = obj;
            }
           
       
        }

        public static bool AllowCode(string code)
        {
            bool result = false;
            var login_data = HttpContext.Current.Session["user"] as LoginInformation;
            var login_permission = login_data.Permissions.Where(m => m.Code.ToLower() == code.ToLower()).FirstOrDefault();
            if (login_permission != null)
            {
                result = true;
            }
            return result;
        }

        public static bool IsLogin()
        {
            bool result = true;
            if (HttpContext.Current.Session["user"]==null)
            {
                result = false;
            }
            return result;
        }


        private List<string> DropController()
        {
            List<string> Controllers = new List<string>();
            Controllers.Add("Base");
            Controllers.Add("Login");
            Controllers.Add("Common");
            Controllers.Add("Error");
            //Controllers.Add("Permission");


            return Controllers;
        }

        private List<string> DropAction()
        {
            List<string> Action = new List<string>();
            Action.Add("IsExist");
            Action.Add("UploadImg");
            Action.Add("Details");
            Action.Add("details");
            Action.Add("getsitesectors");
            Action.Add("Menu");

            
            return Action;
        }

        private List<string> InMenu()
        {
            List<string> noMenuItems = new List<string>();
            noMenuItems.Add("Site");
            return noMenuItems;
        }

        public bool DropControllerExist(string Controller) {
            return DropController().Exists(m => m == Controller);
        }

        public bool InMenuExist(string MenuItem)
        {
            return InMenu().Exists(m => m == MenuItem);
        }

        public bool DropActionExist(string Action)
        {
            return DropAction().Exists(m => m == Action);
        }

       
        public static void ChangeUserId(int sv)
        {
            PCUserId = sv;
        }
        public static int getSharedvalue()
        {
            return PCUserId;
        }

    }
}