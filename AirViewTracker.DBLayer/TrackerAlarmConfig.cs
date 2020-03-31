using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirViewTracker.DBLayer
{
    public class TrackerAlarmConfig
    {       
        public string TrackerIMEI { get; set; } 
        public string AlrmCodes { get; set; }
        public float ThresholdValues { get; set; }
        public bool IsEnabled { get; set; }        
    }
}
