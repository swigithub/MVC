
using System;
using System.Drawing;


namespace SWI.Libraries.AirView.Entities
{
    public class SiteReportPlotVM
    {
        public int  LogId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string MarkerImagePath { get; set; }
        public string Carrier { get; set; }
        public string PCI { get; set; }
        public string PciId { get; set; }
        public string TestType { get; set; }
        public double siteLatitude { get; set; }
        public double siteLongitude { get; set; }
        public string pciColor { get; set; }
        public Color plotColor { get; set; }
        public Color rsrpColor { get; set; }
        public Color rsrqColor { get; set; }
        public Color rsnrColor { get; set; }


        public string CarrierColorName { get; set; }
        public string plotColorName { get; set; }
        public string rsrpColorName { get; set; }
        public string rsrqColorName { get; set; }
        public string rsnrColorName { get; set; }
        public string ChColorName { get; set; }


        public bool IsHandover { get; set; }
        public string NetworkMode { get; set; }
        public string Band { get; set; }
        public string serverTimestamp { get; set; }
        public string PlanFile { get; set; }
        public long FloorId { get; internal set; }
    }
}
