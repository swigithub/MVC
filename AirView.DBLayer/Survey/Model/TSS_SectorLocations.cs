using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.Model
{
    public class TSS_SectorLocations
    {
        public Int64 SiteSectionId { get; set; }
        public Int64 SiteQuestionId { get; set; }
        public Double Azimuth { get; set; }
        public string LatLng { get; set; }
    }
}
