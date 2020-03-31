using AirView.DBLayer.Security.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Security.BLL
{
   public class Sec_UserSettingsBL
    {
        Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
        public List<Sec_UserSettings> ToList(string filter, string value = null)
        {
            DataTable dt = udl.Get(filter, value);
            List<Sec_UserSettings> lst = dt.ToList<Sec_UserSettings>();
            return lst;
        }
    }
}
