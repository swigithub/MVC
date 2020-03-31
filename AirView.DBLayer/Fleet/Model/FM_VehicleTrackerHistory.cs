using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
    public class FM_VehicleTrackerHistory
    {
        public int VehicleId { get; set; }
        public int UEId { get; set; }
        public DateTime TrackerDate { get; set; }
        public string TypeName { get; set; }
        public string ManuName { get; set; }
        public string IMEI { get; set; }
        public string UENumber { get; set; }

    }
}
