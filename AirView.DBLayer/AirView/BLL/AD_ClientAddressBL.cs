using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SWI.Libraries.AirView.BLL
{
    /*----Mubashar----*/
    public class AD_ClientAddressBL
    {
        AD_ClientAddressDL cd = new AD_ClientAddressDL();
        public int Manage(string filter, AD_ClientAddress cl)
        {
            return cd.Manage(filter, cl.AddressId, cl.Address, cl.Street, cl.AddressCityId, cl.StateId, cl.CountryId, cl.ZipCode, cl.IsHeadOffice, cl.ClientId,cl.IsActive);
        }
        public AD_ClientAddress Single(string filter, string value = null)
        {
            try
            {
                DataTable dt = cd.Get(filter, value);
                AD_ClientAddress Addres = new AD_ClientAddress();

                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = 0;
                    Addres.AddressId = int.Parse(dt.Rows[i]["AddressId"].ToString());
                    Addres.Address = dt.Rows[i]["Address"].ToString();
                    Addres.Street = dt.Rows[i]["Street"].ToString();
                    Addres.AddressCityId = int.Parse(dt.Rows[i]["CityId"].ToString());
                    Addres.StateId = int.Parse(dt.Rows[i]["StateId"].ToString());
                    Addres.CountryId = int.Parse(dt.Rows[i]["CountryId"].ToString());
                    Addres.ZipCode = int.Parse(dt.Rows[i]["ZipCode"].ToString());
                    Addres.IsHeadOffice = bool.Parse(dt.Rows[i]["IsHeadOffice"].ToString());
                    Addres.ClientId = int.Parse(dt.Rows[i]["ClientId"].ToString());
                    Addres.IsActive = bool.Parse(dt.Rows[i]["IsActive"].ToString());
                
                    return Addres;
                }
                return null;
            }
            catch (Exception) {
                return null;
            }
        }
        public List<AD_ClientAddress> ToList(string filter, string value = null)
        {

            try
            {
                DataTable dt = cd.Get(filter, value);
                List<AD_ClientAddress> Address = dt.ToList<AD_ClientAddress>();
                return Address;
            }
            catch(Exception )
            {
                return null;
            }
        }
    }
}
