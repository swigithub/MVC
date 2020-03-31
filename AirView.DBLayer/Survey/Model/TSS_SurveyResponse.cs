using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.Model
{
    public class TSS_SurveyResponse
    {
        public Int64 SiteId { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64 SectionId { get; set; }
        public Int64 QuestionId { get; set; }
        public Int64 ResponseId { get; set; }
        public Int64 IterationId { get; set; }
        public string ResponseText { get; set; }
        public string ResponseValue { get; set; }
        public Int64 pIterationId { get; set; }
        public Int64 MinValue { get; set; }
        public Int64 MaxValue { get; set; }
        public bool IsGps { get; set; }
        public bool IsChecked { get; set; }
        public string Signature { get; set;}

        public string QuestionType { get; set; }

        public int MapZoom { get; set; }
        public double Azimuth { get; set; }

        public string MapImage { get; set; }

        public bool IsSectorLocation { get; set; }



    }
}
