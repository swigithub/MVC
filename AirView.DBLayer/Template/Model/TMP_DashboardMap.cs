using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Template.Model
{
    public class TMP_DashboardMap
    {
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
        public string Market { get; set; }
        public string SiteType { get; set; }
        public string Status { get; set; }
        public string Tester { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }
}
