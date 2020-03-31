using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
   public class INV_UEIssuesBL
    {
        INV_UEIssuesDL ued = new INV_UEIssuesDL();
        public List<INV_UEIssues> ToList(string filter, string Value = null)
        {
            DataTable dt = ued.Get(filter, Value);
            List<INV_UEIssues> lst = dt.ToList<INV_UEIssues>();
            return lst;
        }
    }
}
