using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
  public  class AV_CLSScheduleVM
    {
        public Int64 SrId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 NetLayerId { get; set; }
        public Int64 RevisionId { get; set; }
        public Int64 EventTypeId { get; set; }
        public string Event { get; set; }
        public string EventValue { get; set; }
        public bool IsValue { get; set; }
        public bool IsL3Enabled { get; set; }
        public string Color { get; set; }
        public string MapColumn { get; set; }
        public string DisplayType { get; set; }
        public string DefinationName { get; set; }
        public string pDefinationName { get; set; }
        public string pDefinationId { get; set; }
        public int SequenceId { get; set; }
        public DateTime ScheduledOn { get; set; }
        public int LayerStatusId { get; set; }
        public decimal NetworkModeId { get; set; }
        public decimal ScopeId { get; set; }
        public decimal BandId { get; set; }
        public decimal CarrierId { get; set; }
        public int TesterId { get; set; }
        public DateTime ReceivedOn { get; set; }
        public DateTime UploadedOn { get; set; }
        public DateTime AssignedOn { get; set; }
        public DateTime CompletedOn { get; set; }
         public DateTime DriveCompletedOn { get; set; }
        public DateTime DownloadedOn { get; set; }
        public DateTime SubmittedOn { get; set; }
        public DateTime AcceptedOn { get; set; }
        public int UploadedById { get; set; }
        public int ScheduledById { get; set; }
        public decimal SubmittedById { get; set; }
        public int AcceptedById { get; set; }
        public decimal Status { get; set; }
        public decimal StatusReason { get; set; }
        public string PendingIssueDesc { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PublishedOn { get; set; }
        public bool isRedrive { get; set; }
        public int redriveTypeId { get; set; }
        public int redriveReasonId { get; set; }
        public string PWoRefID { get; set; }
        public string redriveComments { get; set; }
        public string netLayerObservations { get; set; }
        public string KeyCode { get; set; }

    }
}
