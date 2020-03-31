using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.Model
{
    public class TSS_WorkOrderCSV
    {
        public string clusterCode { get; set; }
        public string Region { get; set; }
        public string Market { get; set; }
        public string Client { get; set; }
        public string siteCode { get; set; }
        public string SiteType { get; set; }
        public string SiteClass { get; set; }
        public string siteLatitude { get; set; }
        public string siteLongitude { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Project { get; set; }
        public string Scope { get; set; }
        public string SurveyCode { get; set; }
        public string Survey { get; set; }
        public string ReceivedOn { get; set; }
        public string Checklist { get; set; }
        public string SiteName { get; set; }

    }
}
