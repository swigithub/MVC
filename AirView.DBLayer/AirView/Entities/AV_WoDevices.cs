using System;


namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
   public class AV_WoDevices
    {
        public Int64 WoDeviceId { get; set; }
        public Int64 BandId { get; set; }
        public Int64 CarrierId { get; set; }
        public Int64 DeviceScheduleId { get; set; }
        public Int64 LayerStatusId { get; set; }
        public Int64 SequenceId { get; set; }
        public DateTime DownloadDate { get; set; }
        public bool IsDownlaoded { get; set; }
        public Int64 NetworkId { get; set; }
        public Int64 UserId { get; set; }
        public Int64 UserDeviceId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 ScopeId { get; set; }
        public string WoRefId { get; set; }
        public string TestTypes { get; set; }
    }
}
