using System.Collections.Generic;


namespace SWI.Libraries.AirView.Entities
{


    public class GetWorkOrderAssignJson
    {
        public GetWorkOrderAssignJson()
        {
            Site = new GetWorkOrderAssignSite();
            SectorList = new List<GetWorkOrderAssignSector>();
        }
        public GetWorkOrderAssignSite Site;
        public List<GetWorkOrderAssignSector> SectorList;

    }
    public  class GetWorkOrderAssign
    {
        public GetWorkOrderAssign() {
            Site = new GetWorkOrderAssignSite();
            SectorList = new List<GetWorkOrderAssignSector>();
            Sector = new GetWorkOrderAssignSector();
        }
       public GetWorkOrderAssignSite Site;
       public GetWorkOrderAssignSector Sector;
       public List<GetWorkOrderAssignSector> SectorList;
        public string Message { get; set; }

    }
    public   class GetWorkOrderAssignSite
    {
        public int SiteId { get; set; }
        public string SiteCode { get; set; }
        public int TesterId { get; set; }
        public string Tester { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int SectorId { get; set; }
        public string Client { get; set; }
        public int ClientId { get; set; }
        public string NetType { get; set; }
        public int NetTypeId { get; set; }
        public int BandId { get; set; }
        public int CarrierId { get; set; }
        public string Carrier { get; set; }
        public string Scope { get; set; }
        public int ScopeId { get; set; }
        public string WoRefId { get; set; }
    }

 public   class GetWorkOrderAssignSector
    {
        public int SectorId { get; set; }
        public string SectorCode { get; set; }
        public string Antenna { get; set; }
        public string BeamWidth { get; set; }
        public string Azimuth { get; set; }
        public string PCI { get; set; }
    }
}
