using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.Model
{
   public class TSS_SectionIteration
    {
        public Int64 SurveyId { get; set; }
        public Int64 SectionId { get; set; }
        public Int64 PSectionId { get; set; }
        public Int64 IterationId { get; set; }
        public Int64 PIterationId { get; set; }
        public Int64 StatusId { get; set; }

    }
}
