using System;
using System.Collections.Generic;


namespace SWI.Libraries.AirView.Entities
{
    public class BandVM
    {
        public BandVM()
        {
            Sectors = new List<SectorsVM>();
            PciPlot = new List<SiteReportPlotVM>();
        }
        public Int64 BandId { get; set; }
        public string BandName { get; set; }
        public List<SectorsVM> Sectors { get; set; }
        public string NetworkMode { get; set; }
        public int NetworkModeId { get; set; }
        public string Carrier { get; set; }
        public Int64 CarrierId { get; set; }
        //
        public string Scope { get; set; }
        public int ScopeId { get; set; }
      
        public IEnumerable<SiteReportPlotVM> PciPlot { get; set; }

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
        public long LayerStatusId { get; set; }
        public string ClientPrefix { get; set; }
    }
}
