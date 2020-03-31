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
    /*----15-08-2017----*/
    public class TSS_SurveyDocumentDL
    {


        public dynamic Manage(string Filter, Int64 ClientId, Int64 CityId, Int64 @SurveyId, string @SurveyTitle, string @Description, Int64 @CategoryId, Int64 @SubCategoryId, bool @IsActive, Int64 @UnitSystemId, Int64 @CreatedBy,bool IsPublished=false,bool IsGlobal=false)
        {

            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageSurveyDocuments");


                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ClientId", ClientId, "@CityId", CityId, "@SurveyId", SurveyId, "@SurveyTitle", SurveyTitle, "@Description", Description, "@CategoryId", CategoryId,
                                                         "@SubCategoryId", SubCategoryId, "@IsActive", IsActive, "@UnitSystemId", UnitSystemId, "@CreatedBy", CreatedBy,"@IsPublished",IsPublished, "@IsGlobal",IsGlobal));

                DataContext.EndTransaction(loCommand);

                int result = Convert.ToInt32(loCommand.Parameters["@RETURN_VALUE"].Value);
                return result;
            }
            catch
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GetDataTable(string filter, string Value = null, int pageIndex = 0, int pageSize = 0,string IMEI=null,long UserId=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_GetSurveyDocuments");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", Value, "@pageIndex", pageIndex, "@pageSize", pageSize,"@IMEI",IMEI,"@UserId",UserId));
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


        public DataTable Get(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_GetSections");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", Value));
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

        public int CreateWorkOrderDemo(string filter, int SurveyId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_CreateDemoWorkOrder");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand,
                    "@Filter", filter,
                    "@SurveyId", SurveyId));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch
            {
                return 0 ;
            }
        }

        public DataTable GetSectorLocation(Int64 SiteSurveyId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_GetAllSectorLocations");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@SiteSurveyId",SiteSurveyId));
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
