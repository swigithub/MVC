using SWI.Libraries.AD.DAL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AD.BLL
{
    /*----MoB!----*/

    public class AD_FilterQueryBL
    {
        AD_FilterQueryDL dqd = new AD_FilterQueryDL();
        public List<AD_FilterQuery> ToList(string filter, string value = null)
        {
            DataTable dt = dqd.Get(filter, value);
            List<AD_FilterQuery> lst = dt.ToList<AD_FilterQuery>();
            return lst;
        }
    }
}
