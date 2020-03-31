using System;
using System.Collections.Generic;

namespace SWI.Libraries.AirView.Entities
{
    public class SitesVM
    {
        public SitesVM()
        {
            Sectors = new List<SectorsVM>();
            Bands = new List<BandVM>();
        }
        public Int64 SiteId { get; set; }
        

        public string SiteType { get; set; }
        public string SiteCode { get; set; }
        public string ClusterId { get; set; }
        public string Scope { get; set; }
        public Int64 ScopeId { get; set; }
        //
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Tester { get; set; }
        public int TesterId { get; set; }
        public string Description { get; set; }
        public DateTime SubmittedOn { get; set; }
        public DateTime AssignedOn { get; set; }
        public DateTime CompletedOn { get; set; }
        public DateTime ScheduledOn { get; set; }
        public DateTime ReceivedOn { get; set; }
        public DateTime ReportSubmittedOn { get; set; }
        public string  Market { get; set; }
        public string Region { get; set; }
        public string Client { get; set; }
        public string ClientPrefix { get; set; }
        public Int64 ClientId { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string StatusKeyCode { get; set; }
        public List<SectorsVM> Sectors { get; set; }
        public int ZIndex { get; set; }
        public string MarkerImagePath { get; set; }
        public string MarkerTitle { get; set; }
        public string InfoWindowContent { get; set; }
        public List<BandVM> Bands { get; set; }
        public string WoRefNo { get; set; }
        public string NetworkMode { get; set; }
        public string Band { get; set; }
        public string Carrier { get; set; }
        public string ClientLogo { get; set; }
        public string VendorLogo { get; set; }
        public bool IsDownloaded { get; set; }
        public DateTime DriveCompletedOn { get; set; }
        public bool IsActive { get; set; }
        public Int64 CityId { get; set; }

        public Int64 SiteCount { get; set; }

    }
}