using System;


namespace SWI.Libraries.AirView.Entities
{
    public class RegionsVM
    {
        public Int64 ID { get; set; }
        public string RegionName { get; set; }
        public string TesterImage { get; set; }
        public int TotalSites { get; set; }
        public int PendingSites { get; set; }
        public int InProcessSites { get; set; }
        public int CompletedSites { get; set; }
        public int DriveCompleted { get; set; }
        public int PendingWithIssues { get; set; }
        public int DtWoCount { get; set; }
        public int InProgress { get; set; }
        public int ReportSubmited { get; set; }
      
    }
}