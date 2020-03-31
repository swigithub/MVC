using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AV_GetEmailsBL
    {
        AV_GetEmailsDL ed = new AV_GetEmailsDL();

        public List<AV_GetEmails> ToList(string filter, string value)
        {
            DataTable dt = ed.Get( filter,  value);
            return dt.ToList<AV_GetEmails>();
        }
    }
}
