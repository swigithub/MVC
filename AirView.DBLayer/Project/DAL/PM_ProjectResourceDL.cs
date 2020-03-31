using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ibrary.SWI.Project.DAL
{
    /*----MoB!----*/
    /*----15-12-2017----*/
    public class PM_ProjectResourceDL
    {

        public dynamic Manage(string Filter,DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageProjectResources");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List",dt));
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


        public DataTable GetDataTable(string filter, string Value = null, Int64 ProjectId = 0, Int64 TaskId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetProjectResources");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value1", Value));
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
