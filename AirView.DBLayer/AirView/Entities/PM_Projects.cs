using AirView.DBLayer.Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
  public  class PM_Projects
    {
        public Int64 ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<PM_Workgroup> WorkGroups { get; set; }

    }
    public class PM_Workgroup
    {
        public int WorkgroupId { get; set; }
        public String WorkgroupName { get; set; }
        public List<PM_TaskEntry> Resources { get; set; }
    }
}
