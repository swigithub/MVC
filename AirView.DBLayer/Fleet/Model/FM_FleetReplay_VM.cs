using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
    public class FM_FleetReplay_VM
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string IdleTime { get; set; }
        public string ParkingTime { get; set; }
        public string DrivingTime { get; set; }
        public string StartLongitude { get; set; }
        public string EndLongitude { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public string RunningMileage { get; set; }
        public string StartMileage { get; set; }
        public string EndMileage { get; set; }
        public string Status { get; set; }
        public string InEngineOnOff { get; set; }
        public string AlarmCode { get; set; }
        public string AlarmThresholdVal { get; set; }
        public string AlarmCurrentVal { get; set; }
        public string Time { get; set; }
        public string FuelPercent { get; set; }
        public string FuelLiter { get; set; }
        public string AlarmDescription { get; set; }
        public string AlarmState { get; set; }
        public FM_Vehicle LstVehicle { get; set; }
        public FM_TrackerTrip ReplayTrackerTrip { get; set; }

    }
}
