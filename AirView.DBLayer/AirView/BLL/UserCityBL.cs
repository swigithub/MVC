using SWI.Libraries.AirView.Entities;
using SWI.Libraries.AirView.DAL;
using System.Collections.Generic;
using System.Data;


namespace SWI.Libraries.AirView.BLL
{
  public  class UserCityBL
    {

        UserCityDL ucd = new UserCityDL();
        public List<UserCity> ToList(string filter, string value = null)
        {

            DataTable dt = ucd.Get(filter, value);
            List<UserCity> lst = new List<UserCity>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserCity Client = new UserCity();
                    Client.CityId = int.Parse(dt.Rows[i]["CityId"].ToString());
                    Client.CityName = dt.Rows[i]["CityName"].ToString();
                    lst.Add(Client);
                }
            }

            return lst;
        }
    }
}
