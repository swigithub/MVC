using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_TaskDataEntry
    {
        public Int64 TaskId { get; set; }
        public Int64 ProjectSiteId { get; set; }
        public string FormValue { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64? UserId { get; set; }
    }
}
