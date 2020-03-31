using System;


namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_NetLayerStatus
    {
        public int LayerStatusId { get; set; }
        public decimal SiteId { get; set; }
        public decimal NetworkModeId { get; set; }
        public decimal ScopeId { get; set; }
        public decimal BandId { get; set; }
        public decimal CarrierId { get; set; }
        public int TesterId { get; set; }
        public DateTime ReceivedOn { get; set; }
        public DateTime UploadedOn { get; set; }
        public DateTime AssignedOn { get; set; }
        public DateTime CompletedOn { get; set; }
        public DateTime ScheduledOn { get; set; }
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
