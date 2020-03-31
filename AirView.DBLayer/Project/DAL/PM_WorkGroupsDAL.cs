using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.DAL
{
   public class PM_WorkGroupsDAL
    {
        public int Manage(string Filter, Int64 ProjectId, string WorkGroupsIds)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageWorkGroups");
                loCommand = DataContext.StartTransaction(loCommand);
                int result = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@filter", Filter, "@Value", ProjectId, "@Value2", WorkGroupsIds));
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
