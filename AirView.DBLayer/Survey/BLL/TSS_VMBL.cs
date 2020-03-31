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
    /*----06-09-2017----*/
    public class TSS_VMBL
    {
        private TSS_VMDL vmd = new TSS_VMDL();
        public List<TSS_VM> ToList(string Filter, string value = null)
        {
            DataTable dt = vmd.GetDataTable(Filter, value);
            return dt.ToList<TSS_VM>();
        }
    }
}
