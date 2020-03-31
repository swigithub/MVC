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
    /*----08-09-2017----*/
    public class TSS_QuestionLogicDL
    {


        public bool Manage(string Filter,string value, DataTable List)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageQuestionLogics");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@Value",value, "@List", List));
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


        public DataTable GetDataTable(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_GetQuestionLogics");
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


        
            

    }
}
