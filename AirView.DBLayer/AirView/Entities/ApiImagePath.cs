
namespace SWI.Libraries.AirView.Entities
{
  public  class ApiImagePath
    {
        public string SiteId { get; set; }
        public string SectorId { get; set; }
        public string NetworkModeId { get; set; }
        public string BandId { get; set; }
        public string CarrierId { get; set; }
        public string ScopeId { get; set; }
        public string TestType { get; set; }
        public string Ping { get; set; }
        public string DL { get; set; }
        public string UL { get; set; }
        public string ImagePath { get; set; }
        public bool isManual { get; set; }

    }
}
