using AirView.DBLayer.Survey.DAL;
using Library.SWI.Survey.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.BLL
{
   public class RequiredActionsBL
    {
        TSS_RequiredActionDL rd = new TSS_RequiredActionDL();

        public List<RequiredActions> ToList(string filter, string Value = null)
        {
            DataTable dt = rd.GetDataTable(filter, Value);
            return dt.ToList<RequiredActions>();
        }

        public int DeleteImage(string actionid)
        {
            DataTable dt = rd.DeleteImage(actionid);
            return 0;
        }
      
    }
}
