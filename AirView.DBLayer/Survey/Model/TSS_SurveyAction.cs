using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.Model
{
    public class TSS_SurveyAction
    {
        public Int64 SiteId { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64 SectionId { get; set; }
        public Int64 QuestionId { get; set; }
        public Int64 ActionType { get; set; }
        public string ActionValue { get; set; }
        public string Remarks { get; set; }
        public Int64 IterationId { get; set; }
        public Int64 PIterationId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Azimuth { get; set; }

        public string ObjectView { get; set; }
        public string Altitude { get; set; }
        public string GPSAccuracy { get; set; }

    }
}
