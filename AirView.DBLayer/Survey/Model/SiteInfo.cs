using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.Model
{
   public class SiteInfo
    {
        public string SiteName { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string SiteId { get; set; }
        public string Address { get; set; }
        public string Color { get; set; }
    }
}
