using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
  public  class FM_TrackerAlarmConfiguration
    {
       
        public int TrackerId { get; set; }
        public string IMEI { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime  ModifiedOn { get; set; }
        public string AlrmCodes { get; set; }
        public float ThresholdValues { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsNotification { get; set; }
        public bool IsApplied { get; set; }
        public string AssignedUserID { get; set; }
        //public List<FM_AlarmSettingsParam> LstAlarmSettingParam { get; set; }
       
    }

    public class FM_MultipleTrackersAlarmConfiguration
    {
        public int TrackerId { get; set; }
        public string IMEI { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<FM_AlarmConfig> AlarmConfig = new List<FM_AlarmConfig>();
    }

    public class FM_AlarmConfig
    {
        public string AlrmCodes { get; set; }
        public float ThresholdValues { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsNotification { get; set; }
        public bool IsApplied { get; set; }
    }
}
