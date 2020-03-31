using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
  public  class ClusterVM
    {
        public Int64 ClusterId { get; set; }
        public string CellFilePath { get; set; }
        public string ClusterName { get; set; }
        public string Scope { get; set; }
        public int ScopeId { get; set; }
        public DateTime? ReceivedOn { get; set; }
        public DateTime? SubmittedOn { get; set; }
        public DateTime? ScheduledOn { get; set; }
        public DateTime? DriveCompletedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }

        public string TesterName { get; set; }
        public int TesterId { get; set; }
        public string RedriveType { get; set; }
        public string RedriveReason { get; set; }
        public string PWoRefID { get; set; }
        public bool isReDrive { get; set; }
        public string SiteCode { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
        public string Region { get; set; }
        public long ClusterStatusId { get; set; }
        public string ClientPrefix { get; set; }

    }
}
