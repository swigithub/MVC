using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
    public class FM_Vehicle_Assignment
    {
        public int VehicleId { get; set; }
        public int VehicleAssignmentId { get; set; }
        public int UserId { get; set; }
        public String UserName { get; set; }
        public DateTime DateAssign { get; set; }
        public DateTime DateReturn { get; set; }

        public int TrackerVal { get; set; }
        public string PreviousTracker { get; set; }
        public string NewTracker { get; set; }
    }
}
