using System;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_WoTracker
    {
        public Int64 WoTrackerId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 SectorId { get; set; }
        public string SectorCode { get; set; }
        public Int64 NetworkModeId { get; set; }
        public Int64 BandId { get; set; }
        public Int64 CarrierId { get; set; }
        public string WoRefId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Int64 TesterId { get; set; }
        public string TestType { get; set; }
        public string AppVersion { get; set; }
        public string AndroidVersion { get; set; }
        public string TesterFullName { get; set; }
        public string Picture { get; set; }
        public DateTime TrackerTimestamp { get; set; }
        public string Description { get; set; }
        public string IMEI { get; set; }
        
    }
}
