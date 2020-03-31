using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
    public class FM_Tracker_AlarmVal
    {
        public int CurrentSpeed { get; set; }
        public float Temperature { get; set; }
        public float Acceleration { get; set; }
        public float Deceleration { get; set; }
        public float IdleEngineMin { get; set; }
        public float EngineRPM { get; set; }
        public float QuickLaneChange { get; set; }
        public float SharpTurn { get; set; }
        public int FatigueDrivingMin { get; set; }
    }
}
