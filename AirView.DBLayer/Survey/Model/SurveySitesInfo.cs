using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.Model
{
    public class SurveySitesInfo
    {
        public string SSiteName { get; set; }
        public string ESiteName { get; set; }
        public double Distance { get; set; }
        public double SLatitude { get; set; }
        public double SLongitude { get; set; }
        public double ELatitude { get; set; }
        public double ELongitude { get; set; }
        public string StartSiteId { get; set; }
        public string EndSiteId { get; set; }
        public double EAzimuth { get; set; }

        public string SAddress { get; set; }
        public string EAddress { get; set; }

        public string Image { get; set; }

        public string SColor { get; set; }
        public string EColor { get; set; }
    }
}
