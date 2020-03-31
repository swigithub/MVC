using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.SWI.Survey.Model;

namespace Library.SWI.Survey.DAL
{
    /*----MoB!----*/
    /*----06-09-2017----*/
    public class TSS_SurveyResponseDL
    {
        public bool Manage(string Filter, DataTable List)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageSurveyResponse");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", List));
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
        public int CloneSurvey(Int64 SurveyId, string SurveyTitle,long ClientId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_CreateSurveyClone");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand,
                   "@SurveyTitle", SurveyTitle,
                    "@SurveyId", SurveyId,
                    "@ClientId",ClientId));
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
        public int SaveSingleResponse(Int64 PSiteQuestionId, DataTable responses, DataTable List, Int64 QuestionId,int TotalRows,bool IsFinished=false, string MapImage = "",int MapZoom=17,double Azimuth=0, string base64Image="",string filter = "Save_Single_SurveyResponse")
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageSiteSurveyResponse");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand,
                   "@PSiteQuestionId", PSiteQuestionId,
                    "@Question_Id", QuestionId,
                    "@DynamicRows", TotalRows,
                    "@Filter", filter,
                    "@List", List,
                    "@Responses", responses,
                    "@base64String", base64Image,
                    "@MapImage", MapImage,
                    "@MapZoom", MapZoom,
                    "@AzimuthForMap",Azimuth,
                    "@IsFinished",IsFinished));
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
        public DataTable UpdateSectionStatus(Int64 sectionId, int status =90)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageSiteSurveyResponse");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.Select(DataContext.AddParameters(loCommand,
                    "@SecId", sectionId,
                    "@status",status,
                    "@Filter", "Mark_Section_Status"));
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

        public DataTable DeleteSection(Int64 sectionId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageSection");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.Select(DataContext.AddParameters(loCommand,
                    "@SectionId", sectionId,
                    "@Filter", "Delete_Section_By_Id"));
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
    }
}
