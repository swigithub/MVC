using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_Threshold
    {
        public Int64 Threshold { get; set; }
        public bool IsActive { get; set; }
        public string KPIName { get; set; }
        public string ThresholdTo { get; set; }
        public string KPI { get; set; }
        public Int64 Condition { get; set; }
        public string Threshold_Name { get; set; }
        public string Color { get; set; }
        public Int64 Action { get; set; }
    }
}
