using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.DAL
{
    public class PM_TrackerGroupsDL
    {
        public bool Manage(string Filter, int ProjectId, int TaskId, string Title, int ParentId, string GroupCode )
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageTrackerGroups");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@TaskId", TaskId, "@Title", Title, "@ParentId", ParentId,"@Groupcode", GroupCode));
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
