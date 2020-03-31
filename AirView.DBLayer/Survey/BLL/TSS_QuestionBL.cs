using AirView.DBLayer.Survey.BLL;
using Library.SWI.Survey.DAL;
using Library.SWI.Survey.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Library.SWI.Survey.BLL
{
    /*----MoB!----*/
    /*----06-09-2017----*/
    public class TSS_QuestionBL
    {
        TSS_QuestionDL qd = new TSS_QuestionDL();

        public bool Manage(string Filter, TSS_Question q, DataTable dt)
        {
            return qd.Manage(Filter, q.QuestionId, q.SectionId, q.QuestionTypeId, q.Question, q.Description, q.Weightage, q.SortOrder, q.IsRequired, q.IsNoteRequired, q.IsImageRequired, q.IsBarCodeRequired, q.IsRepeatable, q.CreatedOn, q.CreatedBy, q.UnitSystemId, q.UnitTypeId, q.UnitId, q.IsActive, q.IsVerificationRequired, q.IsVideoRequired, q.IsAudioRequired, q.IsDocumentRequired, q.TotalColumn, q.TotalRows, q.DynamicRows,q.IsImageDetailRequired,q.IsMultiLocation,q.SurveyEntity,q.Prefix, dt);
        }

        public List<TSS_Question> ToList(string Filter, string value = null)
        {
            DataTable dt = qd.GetDataTable(Filter, value);
            return dt.ToList<TSS_Question>().OrderBy(x=>x.SortOrder).ToList();
        }

        public void SaveQuestionsOrder(List<TSS_Question> que)
        {
            var dt = new dbDataTable().List();
            foreach (var q in que)
            {
                myDataTable.AddRow(dt, "Value1", q.QuestionId, "Value2", q.SortOrder);
            }
            qd.Manage("Save_Questions_SortOrder",0,0,0,null,null,0,0,false,false,false,false,false,new DateTime(),0,0,0,0,false,false,false,false,false,0,0,false,false,false,"","",dt);
        }
        public List<TSS_Question> GetQuestionsWithOptions(string Filter, int surveyId = 0, string value = null, bool isMapServerPath = false)
        {
            DataTable dt = qd.GetDataTable(Filter, value, surveyId);
            var options = dt.ToList<TSS_Response>();
            var result = new List<TSS_Question>();
            var questionsWithOptions = dt.ToList<TSS_Question>().OrderBy(x=>x.SortOrder).ToList();
            Filter = "GET_QUESIONS_REQUIRED_ACTIONS_BY_SECTIONID";
            DataTable Rdt = qd.GetDataTable(Filter, value, surveyId);
            var requiredAction = Rdt.ToList<RequiredActions>();
            foreach (var item in questionsWithOptions.Select(s => s.SiteQuestionId).Distinct().ToList())
            {
                var itemData = questionsWithOptions.FirstOrDefault(s => s.SiteQuestionId == item);
                if (itemData == null) continue;

                itemData.Responses = options.Where(s => s.SiteQuestionId == item && s.QuestionId == itemData.QuestionId).Distinct().ToList();
                itemData.Responses = options
                    .Where(s => s.SiteQuestionId == item && s.QuestionId == itemData.QuestionId).ToList();

                switch (itemData.QuestionType)
                {
                    case "Table":
                        {
                            foreach (var itemDataResponse in itemData.Responses)
                            {
                                if (itemDataResponse.ResponseValue == "DropDownList")
                                    itemDataResponse.DropDownList = itemDataResponse.UserValues.Split(',').Select(s => new TableResponse() { ResponseValue = s }).ToList();
                                if (itemDataResponse.ResponseValue == "CheckBox")
                                    itemDataResponse.CheckBoxList = itemDataResponse.UserValues.Split(',').Select(s => new TableResponse() { ResponseValue = s }).ToList();
                                if (itemDataResponse.ResponseValue == "RadioButton")
                                    itemDataResponse.RadioButtonList = itemDataResponse.UserValues.Split(',').Select(s => new TableResponse() { ResponseValue = s }).ToList();
                                if (itemDataResponse.ResponseValue == "TextBox")
                                    itemDataResponse.TextBoxList = itemDataResponse.UserValues.Split(',').Select(s => new TableResponse() { ResponseValue = s }).ToList();
                                if (itemDataResponse.ResponseValue == "Map")
                                    itemDataResponse.LatLngList = itemDataResponse.UserValues.Split(',').Select(s => new TableResponse() { ResponseValue = s }).ToList();
                                itemDataResponse.IsChecked = true;
                            }

                            break;
                        }
                    case "DateTime":
                        {
                            foreach (var itemDataResponse in itemData.Responses)
                            {
                                if (itemDataResponse.ResponseValue != "")
                                {
                                    try
                                    {
                                        DateTime date = Convert.ToDateTime(itemDataResponse.ResponseValue);
                                        itemDataResponse.ResponseValue = date.ToString("MM/dd/yyyy hh:mm tt");
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                                break;
                        }
                    case "Bar Code":
                    case "Single Line":
                    case "Multi Line":
                    
                    case "Email":
                    case "Numeric Input":
                        itemData.Responses.ForEach(s => s.IsChecked = true);
                        break;
                }

                //Getting Required Action
                itemData.ReqActions.AddRange(requiredAction.Where(s => s.SiteQuestionId == itemData.SiteQuestionId).ToList());
                foreach (var action in itemData.ReqActions)
                {
                    var temp = action.RequiredAction.Split('/');
                    action.Name = temp.Any() ? temp.Last() : "";
                    switch (action.ActionTypeId)
                    {
                        case 1:
                            action.ActionType = "Notes_Required";
                            break;
                        case 2:
                            if (isMapServerPath)
                                action.RequiredAction = HttpContext.Current.Server.MapPath("~" + action.RequiredAction);
                            action.IsDBExist = true;
                            action.ActionType = "Video_Required";
                            break;
                        case 3:
                            action.ActionType = "Barcode_Required";
                            break;
                        case 4:
                            action.ActionType = "Video_Required";
                            break;
                        case 5:
                            action.ActionType = "Document_Required";
                            break;
                        case 6:
                            action.ActionType = "Audio_Required";
                            break;
                        case 99:
                            action.ActionType = "Signature_Required";
                            break;
                    }

                }
                if (itemData.IsNoteRequired)
                    if (!itemData.ReqActions.Any(s => s.ActionType == "Notes_Required"))
                        itemData.ReqActions.Add(new RequiredActions() { ActionType = "Notes_Required", ActionTypeId = (int)ActionType.Notes });
                if (itemData.IsBarCodeRequired)
                    if (!itemData.ReqActions.Any(s => s.ActionType == "Barcode_Required"))
                        itemData.ReqActions.Add(new RequiredActions() { ActionType = "Barcode_Required", ActionTypeId = (int)ActionType.Barcode });
                result.Add(itemData);
            }

            //foreach (var tssQuestion in result.Where(s => s.QuestionTypeId == 103300
            //                                          || s.QuestionTypeId == 93293
            //                                          || s.QuestionTypeId == 93294
            //                                          || s.QuestionTypeId == 103302
            //                                          || s.QuestionTypeId == 103303
            //                                          || s.QuestionTypeId == 103306).Distinct().ToList())
            //{
            //    tssQuestion.Responses.Add(new TSS_Response()
            //    {
            //        IsChecked = true,
            //        SiteQuestionId = tssQuestion.SiteQuestionId,
            //    });
            //}
            return result;
        }

        public List<TSS_Question> GetQuestionsWithOptionsForDashboard(string Filter, int surveyId = 0, string value = null)
        {
            TSS_SurveyDocumentDL add = new TSS_SurveyDocumentDL();
        var dtsections = add.GetDataTable("SURVEY_SECTIONS_BY_SITESURVEYID_For_Dashboard", Convert.ToString( surveyId), 0, 0);
            var result = new List<TSS_Question>();
            for (int i = 0; i < dtsections.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dtsections.Rows[i]["IsRepeatable"]) == false)
                {



                    value = Convert.ToString(dtsections.Rows[i]["SectionId"]);

                DataTable dt = qd.GetDataTable(Filter, value, surveyId);
               
                var questionsWithOptions = dt.ToList<TSS_Question>();
                foreach (var item in questionsWithOptions.Select(s => s.SiteQuestionId).Distinct().ToList())
                {
                    var itemData = questionsWithOptions.FirstOrDefault(s => s.SiteQuestionId == item);
                    if (itemData == null) continue;
                    result.Add(itemData);
                }
                    }
                }
           

            //foreach (var tssQuestion in result.Where(s => s.QuestionTypeId == 103300
            //                                          || s.QuestionTypeId == 93293
            //                                          || s.QuestionTypeId == 93294
            //                                          || s.QuestionTypeId == 103302
            //                                          || s.QuestionTypeId == 103303
            //                                          || s.QuestionTypeId == 103306).Distinct().ToList())
            //{
            //    tssQuestion.Responses.Add(new TSS_Response()
            //    {
            //        IsChecked = true,
            //        SiteQuestionId = tssQuestion.SiteQuestionId,
            //    });
            //}
            return result;
        }

        public int UpdateCheckListSectionsQuestions(string sections, string questions, string surveyid, string sitesectionids)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "sp_UpdateCheckListSectionsQuestions");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand,
                   "@Sections", sections,
                    "@Questions", questions,
                    "@SiteSurveyID", surveyid,
                    "@SiteSectionIds", sitesectionids
                    ));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public TSS_Question ToSingle(string Filter, string value = null)
        {
            DataTable dt = qd.GetDataTable(Filter, value);
            return dt.ToList<TSS_Question>().FirstOrDefault();
        }

    }
}
