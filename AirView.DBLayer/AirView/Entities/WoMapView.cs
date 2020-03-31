using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class WoMapView
    {
        public WoMapView() {
            MapViewTester = new List<WoMapViewTester>();
        }
        public List<WoMapViewTester> MapViewTester { get; set; }
        public int SiteId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string MarketIcon { get; set; }
        public string SiteCode { get; set; }
        public string WoStatus { get; set; }
        public string WoStatusColor { get; set; }
        public string Client { get; set; }
        public string Market { get; set; }
    
        public string ReceivedOn { get; set; }
        public string SubmittedOn { get; set; }
        public string ScheduledOn { get; set; }
        public string DriveCompletedOn { get; set; }
        public string ReportSubmittedOn { get; set; }
        public string ApprovedOn { get; set; }
    }

    public class WoMapViewTester
    {
        public Int64 TesterId { get; set; }
        public string FullName { get; set; }
        public string Picture { get; set; }
    }

}
