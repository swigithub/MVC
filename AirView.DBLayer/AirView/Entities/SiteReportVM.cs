using System.Collections.Generic;


namespace SWI.Libraries.AirView.Entities
{
    public class SiteReportVM
    {
        public SiteReportVM()
        {
            TestCategories = new List<TestCategoryVM>();
            PciPlot = new List<SiteReportPlotVM>();
        }
        public SitesVM Site { get; set; }
        public List<TestCategoryVM> TestCategories { get; set; }
        public IEnumerable<SiteReportPlotVM> PciPlot { get; set; }
    }
}
