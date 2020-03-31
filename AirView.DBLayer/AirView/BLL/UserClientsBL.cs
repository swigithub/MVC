using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SWI.Libraries.AirView.BLL
{
  public  class UserClientsBL
    {

        UserClientsDL ucd = new UserClientsDL();
        public List<UserClients> ToList(string filter, string value = null)
        {
            try
            {
                DataTable dt = ucd.Get(filter, value);
                List<UserClients> lst = dt.ToList<UserClients>();
                return lst;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectedList> SelectedList(string filter, string value = null) {
            return ToList(filter, value).Select(m => new SelectedList { Text = m.ClientName, Value = m.ClientId.ToString() }).ToList();
        }
    }
}
