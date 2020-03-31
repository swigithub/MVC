using System.Collections.Generic;

namespace SWI.Libraries.AirView.Entities
{
    public class ClientSitesVM
    {
        public ClientSitesVM()
        {
            Sites = new List<SitesVM>();
            Markers = new List<SitesVM>();
        }
        public string Filter { get; set; }

        public List<SitesVM> Sites{ get; set; }
        public IEnumerable<SitesVM> Markers { get; set; }
    }
}