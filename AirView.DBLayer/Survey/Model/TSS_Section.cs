using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.Model
{
    /*----MoB!----*/
    /*----11-08-2017----*/
    public class TSS_Section
    {
        public TSS_Section() {
            Sections = new List<TSS_Section>();
            QuestionList = new List<TSS_Question>();
            Questions = new List<TSS_Question>();

        }

        public Int64 SectionId { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64 PSectionId { get; set; }
        public Int64 SectionPSectionId { get; set; }

        //public Int64 SiteSurveyId { get; set; }
        public Int64 SiteSectionId { get; set; }
        public Int64 SiteQuestionId { get; set; }
        public Int64 QuestionId { get; set; }       
        public string SectionTitle { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 CreatedBy { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalAnswered { get; set; }
        public string StatusId { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public bool IsRepeatable { get; set; }
        public bool IsDeletable { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsInclude { get; set; }
        public string RepeatCount { get; set; }
        public int PIterationID { get; set; }
        public int IterationID { get; set; }
        public string ChildTitle { get; set; }
        public int TemplateSectionId { get; set; }
        public List<TSS_Section> Sections { get; set; }
        public List<TSS_Question> Questions { get; set; }
        public List<TSS_Question> QuestionList { get; set; }

        public List<TSS_Question> ImagesList { get; set; }
        public String Signature { get; set; }
        public int sort { get; set; }
        public string DefinationName { get; set; }
        public string ColorCode { get; set; }

        public string SurveyTitle { get; set; }
        public string SiteCode { get; set; }

        public bool IsSignatureRequired { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string TempSectionTitle { get; set; }

        public bool IsLogicExists { get; set; }

    }
}
