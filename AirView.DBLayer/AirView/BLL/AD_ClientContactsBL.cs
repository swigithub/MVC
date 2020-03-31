using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SWI.Libraries.AirView.BLL
{
    /*----Mubashar----*/
   public class AD_ClientContactsBL
    {
        AD_ClientContactsDL cd = new AD_ClientContactsDL();
        public int Manage(string filter, AD_ClientContacts cl,DataTable Table)
        {
            return cd.Manage(filter, cl.ContactId, cl.ContactPerson, cl.Designation, cl.Gender, cl.Title, cl.ContactNo, cl.ContactType,cl.IsPrimary,cl.ClientId,cl.UserId,cl.RegionId,cl.ContactCityId,cl.IsActive,cl.ReportToId, Table);
        }
        public AD_ClientContacts Single(string filter, string value = null)
        {
            try
            {
                DataTable dt = cd.Get(filter, value);
                AD_ClientContacts Contact = new AD_ClientContacts();

                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = 0;
                    Contact.ContactId = int.Parse(dt.Rows[i]["ContactId"].ToString());
                    Contact.ContactPerson = dt.Rows[i]["ContactPerson"].ToString();
                    Contact.Designation = dt.Rows[i]["Designation"].ToString();
                    Contact.Gender = dt.Rows[i]["Gender"].ToString();
                    Contact.Title = dt.Rows[i]["Title"].ToString();
                    Contact.ContactNo = int.Parse(dt.Rows[i]["ContactNo"].ToString());
                    Contact.ContactType = dt.Rows[i]["ContactType"].ToString();
                    Contact.IsPrimary = bool.Parse(dt.Rows[i]["IsPrimary"].ToString());
                    Contact.ClientId = int.Parse(dt.Rows[i]["ClientId"].ToString());
                    Contact.UserId = int.Parse(dt.Rows[i]["UserId"].ToString());
                    Contact.RegionId = int.Parse(dt.Rows[i]["RegionId"].ToString());
                    Contact.ContactCityId = int.Parse(dt.Rows[i]["CityId"].ToString());
                    Contact.IsActive = bool.Parse(dt.Rows[i]["IsActive"].ToString());
                    Contact.ReportToId = int.TryParse(dt.Rows[i]["ReportToId"].ToString(), out i) ? (int?)i : null;
                    
                    return Contact;
                }
                return null;
            }
            catch (Exception ex) {
                return null;
            }
        }


        public List<AD_ClientContacts> ToList(string filter, string value = null)
        {

            DataTable dt = cd.Get(filter, value);
            List<AD_ClientContacts> Contacts = dt.ToList<AD_ClientContacts>();
            return Contacts;
        }
        public List<Sec_User> All(string filter, string value = null)
        {

            DataTable dt = cd.Get(filter, value);
            List<Sec_User> Clients = dt.ToList<Sec_User>();
            return Clients;
        }
    }
}
