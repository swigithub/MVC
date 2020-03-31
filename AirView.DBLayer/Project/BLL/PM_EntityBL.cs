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
    public class PM_EntityBL
    {
        PM_EntityDL ee = new PM_EntityDL();
        public List<PM_Entity> ToList(string filter, string value = null)
        {

            DataTable dt = ee.Get(filter, value);
            List<PM_Entity> lst = dt.ToList<PM_Entity>();
            return lst;

        }
    }
}
