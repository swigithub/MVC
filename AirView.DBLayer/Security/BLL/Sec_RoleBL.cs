using System;
using System.Collections.Generic;

using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.Entities;
using System.Data;

namespace SWI.Libraries.Security.BLL
{
    /*----MoB!----*/
    public class Sec_RoleBL
    {

        Sec_RoleDL rd = new Sec_RoleDL();

        public int Manage(string filter, Sec_Role r)
        {
          return  rd.Manage(filter,r.RoleId,r.Name,r.Description,r.IsActive,r.DefaultUrl);
        }


        public Sec_Role ToSingle(string filter,string Value=null) {

            DataTable dt = rd.GetRoles(filter, Value);
            Sec_Role Role = new Sec_Role();

            if (dt!=null && dt.Rows.Count > 0)
            {
                Role.Name = dt.Rows[0]["Name"].ToString();
                Role.Description = dt.Rows[0]["Description"].ToString();
                Role.RoleId = int.Parse(dt.Rows[0]["RoleId"].ToString());
                Role.DefaultUrl = dt.Rows[0]["DefaultUrl"].ToString();
            }

            return Role;
        }


        public List< Sec_Role> ToList(string filter, string Value = null)
        {

            DataTable dt = rd.GetRoles(filter, Value);
            List<Sec_Role> lstRoles = new List<Sec_Role>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Sec_Role Role = new Sec_Role();
                    Role.RoleId = int.Parse(dt.Rows[i]["RoleId"].ToString());
                    Role.Name = dt.Rows[i]["Name"].ToString();
                    Role.Description = dt.Rows[i]["Description"].ToString();
                    Role.DefaultUrl = dt.Rows[i]["DefaultUrl"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["ModifyDate"].ToString()))
                    {
                        Role.Update_at = DateTime.Parse(dt.Rows[i]["ModifyDate"].ToString());

                    }
                    Role.IsActive = bool.Parse(dt.Rows[i]["IsActive"].ToString());
                    lstRoles.Add(Role);
                }
            }

            return lstRoles;
        }
    }
}
