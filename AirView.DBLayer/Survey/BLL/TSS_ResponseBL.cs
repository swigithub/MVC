using Library.SWI.Survey.DAL;
using Library.SWI.Survey.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.BLL
{
    /*----MoB!----*/
    /*----07-09-2017----*/
    public class TSS_ResponseBL
    {
        TSS_ResponseDL rd = new TSS_ResponseDL();

        public List<TSS_Response> ToList(string filter, string Value = null) {
            DataTable dt = rd.GetDataTable(filter,Value);
            return dt.ToList<TSS_Response>();
        }
    }
}
