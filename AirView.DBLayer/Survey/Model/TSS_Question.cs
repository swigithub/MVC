using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.Model
{
    /*----MoB!----*/
    /*----11-08-2017----*/
    public class TSS_Question
    {
        public TSS_Question()
        {
            Responses = new List<TSS_Response>();
            ReqActions = new List<RequiredActions>();
            QuestionLogics = new List<TSS_QuestionLogic>();

        }
        public Int64 RowId { get; set; }
        public Int64 QuestionId { get; set; }
        public Int64 SectionId { get; set; }

        public Int64 SiteSectionId { get; set; }
        public Int64 SiteQuestionId { get; set; }
        public Int64 ActionId { get; set; }
        public Int64 QuestionTypeId { get; set; }
        public string QuestionType { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
        public float Weightage { get; set; }
        public int SortOrder { get; set; }
        public bool IsRequired { get; set; }
        public bool IsRepeatable { get; set; }
        public bool IsNoteRequired { get; set; }
        public bool IsImageRequired { get; set; }
        public bool IsBarCodeRequired { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 CreatedBy { get; set; }
        public List<TSS_Response> Responses { get; set; }
        public List<RequiredActions> ReqActions { get; set; }
        public List<TSS_QuestionLogic> QuestionLogics { get; set; }
        public Int64 UnitSystemId { get; set; }
        public Int64 UnitTypeId { get; set; }
        public Int64 UnitId { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerificationRequired { get; set; }
        public bool IsVideoRequired { get; set; }
        public bool IsAudioRequired { get; set; }
        public bool IsDocumentRequired { get; set; }
        public bool IsQuestion { get; set; }
        public bool IsHide { get; set; }
        public int TotalColumn { get; set; }
        public int TotalRows { get; set; }

        public bool DynamicRows { get; set; }
        public int DynamicRowsCount { get; set; }

        public bool IsInclude { get; set; }

        public bool IsImageDetailRequired { get; set; }
        public bool IsMultiLocation { get; set; }
        public string MapImage { get; set; }
        public int MapZoom { get; set; }
        public string SurveyEntity {get;set;}

        public double Azimuth { get; set; }

        public string Prefix { get; set; }

        public bool IsLogicExists { get; set; }
    }
}
