using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
    public class FM_Tracker_VM
    {
        public List<FM_VehicleTrackerHistory> History { get; set; }
        public FM_Vehicle Vehicle { get; set; }
    }
}
