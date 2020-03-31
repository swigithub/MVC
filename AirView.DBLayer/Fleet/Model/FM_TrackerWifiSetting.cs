using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
   public class FM_TrackerWifiSetting
    {
        public string TrackerID { get; set; }
        public bool WifiStatus { get; set; }
        public bool IsAppliedStatus { get; set; }
        public string SSID { get; set; }
        public string WifiPassword { get; set; }
        public bool IsAppliedSSID { get; set; }
        public bool IsAppliedPwd { get; set; }
    }
}
