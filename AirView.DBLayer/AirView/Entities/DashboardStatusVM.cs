
namespace SWI.Libraries.AirView.Entities
{
    public class DashboardStatusVM
    {
        public int TotalSites { get; set; }
        public int InProcessSites { get; set; }
        public int CompletedSites { get; set; }
        public int PendingSites { get; set; }
        public int DriveCompleted { get; set; }
        public int PendingWithIssues { get; set; }
        public int InProgress { get; set; }
        public int ReportSubmitted { get; set; }
    }
}
