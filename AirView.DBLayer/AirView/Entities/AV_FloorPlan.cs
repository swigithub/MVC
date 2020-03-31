using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
 public   class AV_FloorPlan
    {
        public Int64 PlanId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 FloorId { get; set; }
        public string PlanFile { get; set; }
        public string FloorName { get; set; }
        public bool IsActive { get; set; }
    }
}
