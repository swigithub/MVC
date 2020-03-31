using System;


namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_SiteIssueTracker
    {
        public Int64 TrackingId { get; set; }
        public Int64 PTrakingId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 TesterId { get; set; }
        public Int64 NetworkModeId { get; set; }
        public Int64 BandId { get; set; }
        public Int64 CarrierId { get; set; }
        public Int64 ScopeId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime ReportDate { get; set; }
        public string Tester { get; set; }
        public string Picture { get; set; }
        public string ImagePath { get; set; }
        public string IssueType { get; set; }
        public string IssueTypeName { get; set; }
    }
}
