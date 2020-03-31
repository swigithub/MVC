using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.DAL
{
   public class PM_TargetsDL
    {

        public bool Manage(string Filter, string value, DataTable List)
        {
            SqlCommand loCommand = DataContext.OpenConnection();


            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageTargets");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@Value", value, "@List", List));
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

        public DataTable GetDataTable(string filter, string Value1 = null, string Value2 = null, Int64 ProjectId = 0, Int64 TargetId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GenerateDates");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@StartDate", Value1, "@EndDate", Value2));
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



        public DataTable GetDataTableHistory(string filter, string Value1 = null, string Value2 = null, Int64 ProjectId = 0, Int64 MilestoneId = 0, Int64 StageId=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetTargets");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@StartDate", Value1, "@EndDate", Value2, "@ProjectId", ProjectId, "@MilestoneId", MilestoneId, "@StageId", StageId));
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
