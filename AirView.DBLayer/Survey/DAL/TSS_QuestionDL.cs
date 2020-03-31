using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.DAL
{
    /*----MoB!----*/
    /*----06-09-2017----*/
    public class TSS_QuestionDL
    {

        public bool Manage(string Filter, Int64 QuestionId, Int64 SectionId, Int64 QuestionTypeId, string Question, string Description,
                           double Weightage, int SortOrder, bool IsRequired, bool IsNoteRequired, bool IsImageRequired, bool IsBarCodeRequired,
                           bool IsRepeatable, DateTime CreatedOn, Int64 CreatedBy, Int64 UnitSystemId, Int64 UnitTypeId, Int64 UnitId,
                           bool IsActive, bool isVerificationRequired, bool isVideoRequired, bool isAudioRequired, bool isDocumentRequired,int TotalColumn, int TotalRows, bool DynamicRows,bool IsImageDetailRequired,bool IsMultiLocation,string SurveyEntity,string Prefix, DataTable List)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageQuestions");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter,
                                "@QuestionId", QuestionId, "@SectionId", SectionId, "@QuestionTypeId", QuestionTypeId,
                                "@Question", Question, "@Description", Description, "@Weightage", Weightage, "@SortOrder", SortOrder,
                                "@IsRequired", IsRequired, "@IsNoteRequired", IsNoteRequired, "@IsImageRequired", IsImageRequired,
                                "@IsBarCodeRequired", IsBarCodeRequired, "@IsRepeatable", IsRepeatable, "@CreatedBy", CreatedBy,
                                "@UnitSystemId", UnitSystemId, "@UnitTypeId", UnitTypeId, "@UnitId", UnitId, "@IsActive", IsActive,
                                "@isVerificationRequired", isVerificationRequired, "@isVideoRequired", isVideoRequired,
                                "@isAudioRequired", isAudioRequired, "@isDocumentRequired", isDocumentRequired,"@TotalColumn",TotalColumn,"@TotalRows",TotalRows, "@DynamicRows", DynamicRows,
                                "@IsImageDetailRequired", IsImageDetailRequired, "@IsMultiLocation", IsMultiLocation ,"@SurveyEntity",SurveyEntity,"@Prefix",Prefix,"@List", List));
                //"@CreatedOn", CreatedOn,
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

        public DataTable GetDataTable(string filter, string Value = null,int surveyId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_GetQuestions");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", Value, "@surveyId", surveyId));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    }
}
