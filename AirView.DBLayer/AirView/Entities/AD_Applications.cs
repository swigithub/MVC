using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
   public class AD_Applications
    {
        public Int64 AppId { get; set; }
        public string AppName { get; set; }
        public string PackageName { get; set; }
        public Int64 ModuleId { get; set; }
        public string Version { get; set; }
        public string AppURL { get; set; }
    }
}
