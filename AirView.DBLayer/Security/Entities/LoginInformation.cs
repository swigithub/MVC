using SWI.Libraries.AirView.Entities;
using SWI.Libraries.AirView.BLL;

using System.Collections.Generic;
using System;
using AirView.DBLayer.Security.Entities;
using System.Data;
using SWI.Libraries.Common;

namespace SWI.Libraries.Security.Entities
{
    /*----MoB!----*/
    public class LoginInformation
    {
        public Int64 UserId { get; set; }
        public Int64 RoleId { get; set; }
        public string Name { get; set; }

        public Int64 CompanyId { get; set; }
        public string Picture { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string DefaultUrl { get; set; }
        public int DaysForward { get; set; }
        public int DaysBack { get; set; }
        public bool IsManager { get; set; }
        public bool IsAdmin { get; set; }
        public List<Sec_Permission> Permissions;
        public List<Sec_UserProjects> ProjectPermissions;
        public List<UserCity> UserCities;

        public LoginInformation set_user_data(Sec_User u, List<Sec_Permission> UserPermission, DataTable  UPDT)
        {
            List<Sec_UserProjects> UserProject = UPDT.ToList<Sec_UserProjects>();
            LoginInformation ld = new LoginInformation();
            ld.UserId =Convert.ToInt64( u.UserId);
            ld.RoleId = Convert.ToInt64(u.RoleId);
            ld.CompanyId = Convert.ToInt64(u.CompanyId); 
            ld.UserName = u.UserName;
            ld.Email = u.Email;
            ld.Name = u.FirstName +" "+u.LastName;
            ld.Picture = u.Picture;
            ld.IsManager = u.IsManager;
            ld.IsAdmin = u.IsAdmin;
            ld.RoleName = u.RoleName;
            ld.DefaultUrl = u.DefaultUrl;
            ld.DaysBack = u.DaysBack;
            ld.DaysForward = u.DaysForward;
            ld.Permissions = UserPermission;
            ld.ProjectPermissions = UserProject;
            UserCityBL ucb = new UserCityBL();
            ld.UserCities = ucb.ToList("byUserId", ld.UserId.ToString());
            return ld;
        }
    }
}
