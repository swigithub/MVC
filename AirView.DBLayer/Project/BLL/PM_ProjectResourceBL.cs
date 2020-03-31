using ibrary.SWI.Project.DAL;
using Library.SWI.Project.DAL;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Project.BLL
{
    /*----MoB!----*/
    /*----15-12-2017----*/
    public class PM_ProjectResourceBL
    {

        private PM_ProjectResourceDL prd = new PM_ProjectResourceDL();
        public List<PM_ProjectResource> ToList(string Filter, string value1 = null, Int64 ProjectId = 0, Int64 TaskId = 0)
        {
            DataTable dt = prd.GetDataTable(Filter, value1,ProjectId,TaskId);
            return dt.ToList<PM_ProjectResource>();
        }
    }
}
