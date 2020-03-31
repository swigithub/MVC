using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.Entities;
using SWI.Libraries.AirView.BLL;
using AirView.DBLayer.AirView.BLL;
using System.Drawing.Text;
using System.Drawing;
using SWI.Libraries.AirView.Entities;
using System;
using SWI.Libraries.AD.BLL;

namespace SWI.AirView.Common
{
    public class SelectedList
    {
        /*----MoB!----*/
        public List<SelectListItem> Roles(Int64 SelectedValue=0)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            //Testing TFS
            Sec_RoleBL ub = new Sec_RoleBL(); 
            List<Sec_Role> lst = ub.ToList("All");
            foreach (var item in lst)
            {
                if (item.RoleId== SelectedValue)
                {
                    items.Add(new SelectListItem { Text = item.Name, Value = item.RoleId.ToString(), Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = item.Name, Value = item.RoleId.ToString() });
                }
                
            }
            return items;
        }



        



        public List<SelectListItem> Fonts()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select Font", Value = "0" });
            using (InstalledFontCollection col = new InstalledFontCollection())
            {
                foreach (FontFamily fa in col.Families)
                {
                    items.Add(new SelectListItem { Text = fa.Name, Value = fa.Name });
                }
            }
            return items;
        }

        public List<SelectListItem> DefinationTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_DefinationTypesBL dtb = new AD_DefinationTypesBL();
            var lst = dtb.ToList("All");
            items.Add(new SelectListItem { Text = "-Defination Type-", Value = "0" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationType, Value = item.DefinationTypeId.ToString() });
            }
            return items;
        }
        public List<SelectListItem> UserDefinationTypes(string Userid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            Sec_UserDefinationTypeBL db = new Sec_UserDefinationTypeBL();
            var lst = db.ToListDefinations("GetDefinationTypeByUId", Userid); ;
            items.Add(new SelectListItem { Text = "-Defination Type-", Value = "0" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationType, Value = item.DefinationTypeId.ToString() });
            }
            return items;
        }
        public void UserAssinged_Testers_Devices(int UserId,ref List<SelectListItem> LUsers, ref List<Sec_UserDevices> UserDevices) {
            Sec_UserBL ub = new Sec_UserBL();
            List<Sec_User> Users = new List<Sec_User>();
            ub.UserAssinged_Testers_Devices(UserId, ref Users, ref UserDevices);

            LUsers.Add(new SelectListItem { Text = "Select Tester", Value = "0" });
            foreach (var item in Users)
            {
                LUsers.Add(new SelectListItem { Text = item.FirstName +" "+item.LastName , Value = item.UserId.ToString() });
            }
        }

        public List<SelectListItem> Clients(string filter,string value=null)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            ClientsBL cb = new ClientsBL();
            var lst = cb.ToList(filter,value);
            items.Add(new SelectListItem { Text = "--Select Client--", Value = "0" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.ClientName, Value = item.ClientId.ToString() });
            }
            return items;
        }
        public List<SelectListItem> ClientContacts(string filter, string value = null)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_ClientContactsBL cb = new AD_ClientContactsBL();
            var lst = cb.ToList(filter, value);
            items.Add(new SelectListItem { Text = "--Select Person--", Value = "0" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.ContactPerson, Value = item.ContactId.ToString() });
            }
            return items;
        }
        public List<SelectListItem> Users(string filter, string value = null)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_ClientContactsBL cb = new AD_ClientContactsBL();
            var lst = cb.All(filter, value);
            items.Add(new SelectListItem { Text = "--Select User--", Value = "0" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.FirstName+""+item.LastName, Value = item.UserId.ToString() });
            }
            return items;
        }

        public List<SelectListItem> User(string filter, string value = null)
        {
            List<SelectListItem> items = new List<SelectListItem>();
           Sec_UserBL cb = new Sec_UserBL();
            var lst = cb.ToList(filter, value);
            items.Add(new SelectListItem { Text = "--Select User--", Value = "0" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.FirstName + "" + item.LastName, Value = item.UserId.ToString() });
            }
            return items;
        }

        public List<SelectListItem> Definations(string Filter,string value=null, string Message=null)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_DefinationBL db = new AD_DefinationBL();
            var lst = db.ToList(Filter, value);
          
                items.Add(new SelectListItem { Text = "Select", Value = "0" });

           
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationName, Value = item.DefinationId.ToString() });
            }
            return items;
        }
        public List<SelectListItem> RegionWithParent(string Filter, string value = null, string Message = null)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_DefinationBL db = new AD_DefinationBL();
            var lst = db.ToList(Filter, value);

            items.Add(new SelectListItem { Text = "Select", Value = "0" });


            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationName+" "+item.PDefinationName, Value = item.DefinationId.ToString() });
            }
            return items;
        }

        public List<SelectListItem> Sectors()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_DefinationBL db = new AD_DefinationBL();
            var lst = db.ToList("Sectors");
            items.Add(new SelectListItem { Text = "-Sector-", Value = "" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationName, Value = item.DefinationName });
            }
            return items;
        }
        public List<SelectListItem> Countries()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_DefinationBL db = new AD_DefinationBL();
            var lst = db.ToList("Countries");
            items.Add(new SelectListItem { Text = "All", Value = "0" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationName, Value = item.DefinationId.ToString() });
            }
            return items;
        }
        

        public List<SelectListItem> States()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_DefinationBL db = new AD_DefinationBL();
            var lst = db.ToList("States");
            items.Add(new SelectListItem { Text = "-State-", Value = "0" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationName, Value = item.DefinationId.ToString() });
            }
            return items;
        }

        public List<SelectListItem> Reasons()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_DefinationBL db = new AD_DefinationBL();
            var lst = db.ToList("Reasons");
            items.Add(new SelectListItem { Text = "-Reasons-", Value = "" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationName, Value = item.DefinationId.ToString() });
            }
            return items;
        }


        public List<SelectListItem> RedriveTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_DefinationBL db = new AD_DefinationBL();
            var lst = db.ToList("RedriveTypes");
            items.Add(new SelectListItem { Text = "-Redrive Types-", Value = "" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationName, Value = item.DefinationId.ToString() });
            }
            return items;
        }

        public List<SelectListItem> IssueTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_DefinationBL db = new AD_DefinationBL();
            var lst = db.ToList("IssueTypes");
            items.Add(new SelectListItem { Text = "-Select Issue Type-", Value = "" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationName, Value = item.DefinationId.ToString() });
            }
            return items;
        }

        public List<SelectListItem> Scopes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_DefinationBL db = new AD_DefinationBL();
            var lst = db.ToList("Scopes");
            items.Add(new SelectListItem { Text = "-Scope-", Value = "0" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationName, Value = item.DefinationId.ToString() });
            }
            return items;
        }


        public List<SelectListItem> UsersByRoleId(int Id)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            Sec_UserBL ub = new Sec_UserBL();
            List<Sec_User> lst = ub.ToList("byRoleId", Id.ToString());
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.FirstName+ " "+ item.LastName , Value = item.UserId.ToString() });
            }
            return items;
        }

        public List<SelectListItem> UserDevices(Int64 Id)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            Sec_UserDevicesBL udbl = new Sec_UserDevicesBL();
            var lst = udbl.ToList("byUserId", Id.ToString()).Where(m=>m.IsActive==true).ToList();
            if (lst.Count>1)
            {
                items.Add(new SelectListItem { Text = "All", Value = "0" });
            }
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.Manufacturer + " " + item.Model+" ["+item.IMEI+"]", Value = item.DeviceId.ToString() });
            }
            return items;
        }

        //public List<SelectListItem> ReportTypes()
        //{
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    DefinationBL db = new DefinationBL();
        //    var lst = db.ToList("ReportTypes");
        //    foreach (var item in lst)
        //    {
        //        items.Add(new SelectListItem { Text = item.DefinationName, Value = item.DefinationId.ToString() });
        //    }
        //    return items;
        //}

        public List<SelectListItem> UserCities(List<UserCity> lst)
        {
            List<SelectListItem> items = new List<SelectListItem>();
           
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.CityName, Value = item.CityId.ToString() });
            }
            return items;
        }



        public List<SelectListItem> WoStatus()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            AD_DefinationBL ucb = new AD_DefinationBL();
            var lst = ucb.ToList("WO Status");
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.DefinationName, Value = item.DefinationId.ToString() });
            }
            return items;
        }
    }
}