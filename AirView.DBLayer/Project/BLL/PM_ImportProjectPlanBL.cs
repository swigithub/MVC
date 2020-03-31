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
    public class PM_ImportProjectPlanBL
    {
        PM_ImportProjectPlanDL pdl = new PM_ImportProjectPlanDL();
        public bool Manage(string Filter, string value, DataTable dt)
        {
            try
            {
                return pdl.Manage(Filter, value, dt);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public bool ManageImports(string Filter, string value, DataTable dt)
        //{
        //    try
        //    {
        //        return pdl.ManageImports(Filter, value, dt);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        

        public List<PM_ImportProjectPlan> ToList(string Filter, string value = null, DataTable List = null)  //string Filter, string value, DataTable List
        {
            DataTable dt = pdl.GetDataTable(Filter, value, List);
            return dt.ToList<PM_ImportProjectPlan>();
        }
    }
}
