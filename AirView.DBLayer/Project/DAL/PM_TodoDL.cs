using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Project.DAL
{
    public class PM_TodoDL
    {
        public DataTable GetDataTable(string filter, Int64 ProjectId, Int64 UserId, string WhereClause = "")
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetToDo");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", ProjectId, "@UserId", UserId, "@WhereClause", WhereClause));
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
        public DataSet GetDataset(string filter, Int64 ProjectId, Int64 UserId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetToDo");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", ProjectId, "@UserId", UserId));
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