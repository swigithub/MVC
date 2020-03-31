using System;
using System.Collections.Generic;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_Site
    {
        public AV_Site() {
            Sectors = new List<AV_Sector>();
        }

        public List<AV_Sector> Sectors;
        public Int64 SiteId { get; set; }
        public Int64 ProjectId { get; set; }
        public string SiteCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double SiteLatitude { get; set; }
        public double SiteLongitude { get; set; }

        public string ClusterCode { get; set; }
        public Int64 ClusterId { get; set; }
        public Int64 ClientId { get; set; }
        public string Client { get; set; }
        public Int64 Region { get; set; }
        public Int64 Market { get; set; }
      
        public string Tester { get; set; }
        public Int64 TesterId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Int64 State { get; set; }
        public DateTime ReportSubmittedOn { get; set; }
        public string StatusName { get; set; }
        public string StatusKeyCode { get; set; }
        public DateTime SubmittedOn { get; set; }
        public DateTime AssignedOn { get; set; }
        public DateTime CompletedOn { get; set; }
        public DateTime ScheduledOn { get; set; }
        public DateTime DriveCompletedOn { get; set; }
        public DateTime DownloadedOn { get; set; }
        public Int64 SubmittedById { get; set; }
        public DateTime TesterAssignedById { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PublishedOn { get; set; }
        public bool IsDownloaded { get; set; }
        public string WoCode { get; set; }
        public string WoRefId { get; set; }
        public string WoRefNo { get; set; } //wo search
        public Int64 CityId { get; set; }
        public DateTime ReceivedOn { get; set; }
        public string SiteAddress { get; set; }
        public Int64 ScopeId { get; set; }
        public string Scope { get; set; }
        public string Scope123 { get; set; }
        public string SiteName { get; set; }
        public Int64 SiteTypeId { get; set; }
        public Int64 SiteClassId { get; set; }
        public string ClientPrefix { get; set; }
        public Int64 SiteType { get; set; }
        public Int64 StateId { get; set; }
        public Int64 RegionId { get; set; }
        public Int64 MarketId { get; set; }
        public Int64 SiteClass { get; set; }
        public Int64 Project { get; set; }
    }
}
