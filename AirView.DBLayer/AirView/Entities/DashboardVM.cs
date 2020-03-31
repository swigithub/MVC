using System.Collections.Generic;


namespace SWI.Libraries.AirView.Entities
{
    public class DashboardVM
    {
        public DashboardVM()
        {
            ClientSites = new ClientSitesVM();
            Regions = new List<RegionsVM>();
            TesterSites = new List<RegionsVM>();
            DriveTesterSites = new List<RegionsVM>();
            SiteStatuses = new DashboardStatusVM();
            lstSiteWO = new List<SitesVM>();
        }
        // public List<SitesVM> Sites { get; set; }
        public ClientSitesVM ClientSites { get; set; }


        public IEnumerable<RegionsVM> Regions { get; set; }
        public IEnumerable<RegionsVM> TesterSites { get; set; }
        public IEnumerable<RegionsVM> DriveTesterSites { get; set; }

        public DashboardStatusVM SiteStatuses { get; set; }
        public List<SitesVM> lstSiteWO { get; set; }
        public int Count { get; set; }

    }
}