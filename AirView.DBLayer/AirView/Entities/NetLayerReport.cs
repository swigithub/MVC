using AirView.DBLayer.AirView.Entities;
using System;
using System.Collections.Generic;


namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class NetLayerReport
    {
        public NetLayerReport() {
            Azmiuth = new List<Azmiuth>();
        }
        public List<AV_NetLayerReportPlot> PCI_Circles { get; set; }
        public List<AV_NetLayerReportPlot> RsRp_Circles { get; set; }
        public List<AV_NetLayerReportPlot> RSRQ_Circles { get; set; }
        public List<AV_NetLayerReportPlot> CINR_Circles { get; set; }
        public List<AV_NetLayerReportPlot> CW_Circles { get; set; }
        public List<AV_NetLayerReportPlot> CH_Circles { get; set; }
        public List<AV_NetLayerReportPlot> CCW_Circles { get; set; }
        public List<AV_NetLayerReportPlot> CCW_Marker { get; set; }
        public List<AV_NetLayerReportPlot> CW_Marker { get; set; }
        public List<AV_NetLayerReportPlot> CwCcw_Marker { get; set; }
        public List<AV_NetLayerReportPlot> CWDropSignal { get; set; }
        public List<string> PCIs { get; set; }
        public List<string> CwPCIs { get; set; }
        public List<string> CcwPCIs { get; set; }
        public List<string> ServerTimestamp { get; set; }
        public List<Azmiuth> Azmiuth { get; set; }
    }

    public class AV_NetLayerReportPlot
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Color { get; set; }
        public string PCI { get; set; }
        public string TestType { get; set; }
        public string NetworkMode { get; set; }
        public string PlanFile { get; set; }
        public long FloorId { get; set; }
        public string ServerTimestamp { get; set; }
    }
    public class LatLng
    {
        public decimal latitude { get; set; }
    public decimal longitude { get; set; }
}
    public class Azmiuth
    {
        public string RecieverDistance { get; set; }
        public string InnerDistance { get; set; }
        public string OuterDistance { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }
        public string Color { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? nLatitude { get; set; }
        public double? nLongitude { get; set; }
        public string Site { get; set; }
        public string Sector { get; set; }
        public string PCI { get; set; }
        public bool? IsPOR { get; set; }
        public int? NetworkmodeId { get; set; }
        public int? BandId { get; set; }
        public int? CarrierId { get; set; }
        public int? ScopeId { get; set; }
        public int? SectorId { get; set; }
        public int? SiteId { get; set; }
        public string FloorPath { get; set; }

    }
    public class AV_NetLayerReportExport
    {
        public string WO_Ref { get; set; }
        public string Client { get; set; }
        public string Region { get; set; }
        public string Market { get; set; }
        public string Site { get; set; }
        public string Drive_Tester { get; set; }
        public string Network_Mode { get; set; }
        public string Network_Layers { get; set; }
        public string Band { get; set; }
        public string Carrier { get; set; }
        public string Scope { get; set; }
        public string Project { get; set; }
        public DateTime? Received { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? Drive_Completed { get; set; }
        public DateTime? Approved { get; set; }
        public DateTime? ReportSubmittedOn { get; set; }
       
        public string Status { get; set; }
        public string Description { get; set; }
        public int LayerCount { get; set; }
        public int SectorCount { get; set; }
        public double Distance_from_Site { get; set; }
        public double DT_Minutes { get; set; }
        public DateTime? Site_Completed { get; set; }
        public DateTime? Site_First_Test { get; set; }
        public string SiteType { get; set; }
        public string CheckList { get; set; }


    }



    public class AV_NetLayerReportMarktExport
    {
        public string  Market { get; set; }
        public int Pending_Schedule { get; set; }

        public int Scheduled { get; set; }
        public int In_Progress { get; set; }
        public int Drive_Completed { get; set; }
        public int Pending_With_Issues { get; set; }

        public int Report_Submitted { get; set; }
        public int Approved { get; set; }
        public int Total { get; set; }


    }



}
