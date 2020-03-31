using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
    public class FM_TrackerTrip
    {
        public DateTime TripStartTime { get; set; }
        public string ReplayTripStartTime { get; set; }
        public string ReplayTripEndTime { get; set; }
        public string TripName { get; set; }
        public string TripDate { get; set; }
        public int IgnitionNo { get; set; }
        public double DrivingTime { get; set; }
        public double IdleTime { get; set; }
        public float AvgSpeed { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxRotation { get; set; }
        public int NoOfAcc { get; set; }
        public int NoOfDec { get; set; }
        public int PktNo { get; set; }
        public double TotalSpeed { get; set; }
        public float CurrentSpeed { get; set; }
        public float CurrentRotation { get; set; }
        public bool IsLogin { get; set; }    
        public DateTime IdleTimeStart { get; set; }
        public DateTime IdleTimeEnd { get; set; }
        public int TripNo { get; set; }
        public string TotalTime { get; set; }
        public float DistanceTravelled { get; set; }
    }
}
