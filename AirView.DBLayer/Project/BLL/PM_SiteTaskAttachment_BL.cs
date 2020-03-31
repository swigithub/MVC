using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.BLL
{
    public class PM_SiteTaskAttachment_BL
    {
        PM_SiteTaskAttachment_DL dd = new PM_SiteTaskAttachment_DL();
        public List<PM_SiteTaskAttachment> ToList(string filter, string value = null)
        {
            DataTable dt = dd.Get(filter, value);
            List<PM_SiteTaskAttachment> lst = dt.ToList<PM_SiteTaskAttachment>();
            return lst;
        }
    }
}
