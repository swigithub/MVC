using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_MarketSites
    {
        public decimal RecieverDistance { get; set; }
        public decimal InnerDistance { get; set; }
        public decimal OuterDistance { get; set; }
        public int SectorId { get; set; }
        public int SiteId { get; set; }
        public decimal SrId { get; set; }
        public decimal ClientId { get; set; }
        public decimal RegionId { get; set; }
        public decimal CityId { get; set; }
        public string SiteCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string SectorCode { get; set; }
        public decimal NetworkModeId { get; set; }
        public decimal BandId { get; set; }
        public decimal CarrierId { get; set; }
        public string Antenna { get; set; }
        public double BeamWidth { get; set; }
        public double Azimuth { get; set; }
        public int PCI { get; set; }
        public int RFHeight { get; set; }
        public int MTilt { get; set; }
        public int ETilt { get; set; }
        public int BandWidth { get; set; }
        public string SectorColor { get; set; }
        public string CellId { get; set; }
        public string Band { get; set; }
        public int ScopeId { get; set; }

        public string SiteType { get; set; }
        public string PORCheck { get; set; }
        public string RingId { get; set; }
        public string POR { get; set; }
        public string BandCapable { get; set; }
        public string BandPOR { get; set; }
        public string BandOA { get; set; }
        public string Comments { get; set; }
        public bool IsPOR { get; set; }
        public string FloorPath { get; set; }
    }
}
