

using SWI.Libraries.AD.Entities;
using System.Collections.Generic;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_Sector
    {
        public decimal SectorId { get; set; }
        public string SectorCode { get; set; }
        public decimal NetworkModeId { get; set; }
        public decimal ScopeId { get; set; }
        public decimal BandId { get; set; }

        public string BandWidth { get; set; }
        public decimal CarrierId { get; set; }
        public string Antenna { get; set; }
        public double BeamWidth { get; set; }
        public double Azimuth { get; set; }
        public int PCI { get; set; }
        public decimal SiteId { get; set; }
        public decimal TestStatus { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string MRBTS { get; set; }
        public string CellId { get; set; }
        public int RFHeight { get; set; }
        public int MTilt { get; set; }
        public int ETilt { get; set; }
        public bool isActive { get; set; }
        public double VerticalBeamWidth { get; set; }
        public long SiteClusterId { get; set; }
        public string ClusterId { get; set; }
        public string ClusterName { get; set; }
        public string NetworkId { get; set; }
        public string sectorColor { get; set; }
        public long SurveyId { get; set; }
        public long SiteSurveyId { get; set; }
        public string AntennaDowntilt { get; set; }
        public List<AD_Defination> Bands { get; set; }
        public List<AD_Defination> Carriers { get; set; }

    }
}
