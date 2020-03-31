using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.Model
{
   public class TSS_SiteAttendees
    {

        public Int64 SiteAttendeeId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 SiteSurveyId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Company { get; set; }
        public string Signature { get; set; }

        public string RowId { get; set; }
        public string RowType { get; set; }
    }
}
