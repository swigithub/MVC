using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class AV_BeamTestLog
    {
        public int BTLogId { get; set; }

        public DateTime TimeStamp { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 SectorId { get; set; }
        public Int64 NetworkModeId { get; set; }
        public Int64 ScopeId { get; set; }
        public Int64 BandId { get; set; }
        public Int64 CarrierId { get; set; }
        public Int64 LayerStatusId { get; set; }
        public int BeamId { get; set; }
        public int BeamGroupId { get; set; }
        public string BMColor { get; set; }
        public string BMGColor { get; set; }
        public int PCIId { get; set; }
        public int SSBIndex { get; set; }
        public float NRRSRP0 { get; set; }
        public float NRRSRP1 { get; set; }
        public float NRRSRP2 { get; set; }
        public float NRRSRP3 { get; set; }
        public float NRRSRQ0 { get; set; }
        public float NRRSRQ1 { get; set; }
        public float NRRSRQ2 { get; set; }
        public float NRRSRQ3 { get; set; }
        public float NRRSNR0 { get; set; }
        public float NRRSNR1 { get; set; }
        public float NRRSNR2 { get; set; }
        public float NRRSNR3 { get; set; }

    }

    public class BeamTestLegend
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
        public string Type { get; set; }
    }
    public class RDACounts
    {
        public string RDAEvent { get; set; }
        public int RDACount { get; set; }
    }
    public class OoklaDLSiteLevels
    {
        public string NetworkMode { get; set; }
        public double OoklaDLSiteLevel { get; set; }
    }
}

