using Library.SWI.Project.DAL;
using Library.SWI.Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using SWI.Libraries.Common;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Project.Model;
using AirView.DBLayer.Project.DAL;

namespace AirView.DBLayer.Project.BLL
{
   public class PM_WorkGroupsBL
    {
        PM_WorkGroupsDAL pg = new PM_WorkGroupsDAL();
        public int Manage(string filter,long PID,string PWorkgroups)
        {
            return pg.Manage(filter,PID,PWorkgroups);
        }
    }
}
