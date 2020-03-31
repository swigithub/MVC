using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.Model
{
    /*----MoB!----*/
    /*----09-08-2017----*/
    public class TSS_QuestionLogic
    {
        public Int64 LogicId { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64 SectionId { get; set; }
        public Int64 FromQuestionId { get; set; }
        public string FromQuestion { get; set; }
        public string ToQuestionId { get; set; }
        public List<Int64> ToQuestionIdNew { get; set; }
        public string ToQuestion { get; set; }
        public Int64 ConditionId { get; set; }
        public string Condition { get; set; }
        public Int64 ResponseId { get; set; }
        public string Response { get; set; }
        public Int64 ActionId { get; set; }
        public string Action { get; set; }
        public bool IsActive { get; set; }

        









    }
}
