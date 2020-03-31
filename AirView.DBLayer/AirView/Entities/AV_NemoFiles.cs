using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{//
   public class AV_NemoFiles
    {
        public int siteID { get; set; }
        public int sectorID { get; set; }
        public int networkModeID { get; set; }
        public int bandID { get; set; }
        public int carrierID { get; set; }
        public int scopeID { get; set; }
        public string filePath { get; set; }

        public string band { get; set; }
        public string carrier { get; set; }
        public string fileName { get; set; }
        public string networkMode { get; set; }

    
        

    }
}
