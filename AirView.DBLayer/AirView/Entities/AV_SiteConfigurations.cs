using System;

namespace SWI.Libraries.AirView.Entities
{
  public  class AV_SiteConfigurations
    {
        public Int64 TestId { get; set; }
        public Int64 ClientId { get; set; }
        public Int64 CityId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 RevisionId { get; set; }
        public Int64 TestTypeId { get; set; }
        public Int64 KpiId { get; set; }
        public string KpiValue { get; set; }
        public Int64 TestCatogoryId { get; set; }

        public string TestCategory { get; set; }
        public string TestType { get; set; }
        public string KPI { get; set; }
        public string kpiKey { get; set; }
        public string SortOrder { get; set; }
      

    }
    public class AV_ScriptPreview
    {

        public string Col { get; set; }
        public string Heads { get; set; }
    }
}
