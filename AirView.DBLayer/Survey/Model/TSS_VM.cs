using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.Model
{
    /*----MoB!----*/
    /*----11-08-2017----*/
    public class TSS_VM
    {
        public Int64 SiteSurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public Int64 SurveyId { get; set; }
        public string SiteType { get; set; }

        public string ClientPOC { get; set; }
        public string TesterName { get; set; }
        public string TesterId { get; set; }
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public int Sections { get; set; }
        public int Questions { get; set; }
        public Int64 NetworkModeId { get; set; }
        public Int64 ScopeId { get; set; }
        public Int64 BandId { get; set; }
        public Int64 CarrierId { get; set; }
        public Int64 SiteId { get; set; }
        public bool IsActive { get; set; }
        public string ClientPrefix { get; set; }
        public string SiteCode { get; set; }
        public string SectionStatus { get; set; }
    }
}
