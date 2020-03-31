using SWI.Libraries.Common;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Template.DAL
{
    public class TMP_GetDashboardDL
    {
        public DataTable GetTb(int ScopeId, int ProjectId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_GetDashboardMap");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@ScopeId", ScopeId, "ProjectId", ProjectId));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
                loCommand.Dispose();
            }
        }
    }

}
