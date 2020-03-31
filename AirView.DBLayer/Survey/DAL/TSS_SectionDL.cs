using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWI.Libraries.Common;
using System.Data.SqlClient;
using System.Data;

namespace Library.SWI.Survey.DAL
{
    /*----MoB!----*/
    /*----28-08-2017----*/
    public class TSS_SectionDL
    {
        public dynamic Manage(string Filter, Int64 SectionId, Int64 PSectionId, Int64 SurveyId, string SectionTitle,
            Int64 PIterationID, Int64 IterationID, string Description, int SortOrder, bool IsActive, DateTime CreatedOn,Int64 CreatedBy,bool isRepeatable,bool isApplicable, bool IsSignatureRequired, DataTable list=null)
        {

            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageSection");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                loCommand = DataContext.StartTransaction(loCommand);

                dynamic id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SectionId", SectionId, "@PSectionId",PSectionId, "@SurveyId", SurveyId, "@SectionTitle", SectionTitle,
                     "@PIterationID", PIterationID, "@IterationID", IterationID, "@Description", Description, "@SortOrder", SortOrder,
                     "@IsActive", IsActive,  "@CreatedBy", CreatedBy , "@IsRepeatable", isRepeatable, "@IsApplicable", isApplicable, "@list", list,"@IsSignatureRequired",IsSignatureRequired));
                //"@CreatedOn", CreatedOn,
                DataContext.EndTransaction(loCommand);
                dynamic result = Convert.ToInt32(loCommand.Parameters["@RETURN_VALUE"].Value);
                return result;
            }
            catch
            {
               // DataContext.CancelTransaction(loCommand);
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

        public DataTable GenerateChildSections(string filter = "GENERATE_CHILD_SITE_SECTIONS", int PSectionId = 0,int RepeatCount = 0, bool IsRepeatable = false,bool IsDeletable = true)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageChildSections");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@PSectionId", PSectionId, "@RepeatCount", RepeatCount, "@IsRepeatable", IsRepeatable,"@IsDeletable",IsDeletable,"@Filter",filter));
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
        public DataTable DeleteSectionsWithChild(string filter, int PSectionId = 0,int SectionId=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageChildSections");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@PSectionId", PSectionId,"@SectionId",SectionId,"@Filter",filter ));
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
