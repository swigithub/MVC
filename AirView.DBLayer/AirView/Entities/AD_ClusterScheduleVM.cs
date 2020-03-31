using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
 public  class AD_ClusterScheduleVM
    {
      public Int64 SiteClusterId { get; set; }
        public Int64 LayerStatusId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 ScopeId { get; set; }
        public Int64 TesterId { get; set; }
        public Int64 UserDeviceId { get; set; }
        public Int64 SequenceId { get; set; }
        public Int64 Status { get; set; }
        public bool IsActive { get; set; }
        public Int64 DeviceScheduleId { get; set; }

        //////Device coulumns
        public decimal DeviceId { get; set; }
        public decimal UserId { get; set; }
        public string IMEI { get; set; }
        public string MAC { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }


    }
}
