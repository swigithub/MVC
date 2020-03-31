using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.Model
{
    /*----MoB!----*/
    /*----11-08-2017----*/
    public class TSS_Response
    {
        public TSS_Response()
        {
            TextBoxList = new List<TableResponse>();
            DropDownList = new List<TableResponse>();
            RadioButtonList = new List<TableResponse>();
            CheckBoxList = new List<TableResponse>();
        }
        public Int64 ResponseId { get; set; }
        public Int64 QuestionId { get; set; }
        public Int64 SiteQuestionId { get; set; }
        public string ResponseText { get; set; }
        public string ResponseValue { get; set; }
        public int SortOrder { get; set; }
        public bool IsPassed { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public bool IsGps { get; set; }
        public bool IsActive { get; set; }
        public bool IsChecked { get; set; }
        public Int64 ConditionId { get; set; }
        public Int64 ActionId { get; set; }
        public string Signature {get;set;}
        public bool IsReadOnly { get; set; }
        public string UserValues { get; set; }
        public string SelectedRawResponse { get; set; }
        public List<TableResponse> TextBoxList { get; set; }
        public List<TableResponse> DropDownList { get; set; }
        public List<TableResponse> RadioButtonList { get; set; }
        public List<TableResponse> CheckBoxList { get; set; }
        public List<TableResponse> LatLngList { get; set; }
    }

    public class TableResponse
    {
        public bool IsChecked { get; set; }
        public string ResponseValue { get; set; }
    }
}
