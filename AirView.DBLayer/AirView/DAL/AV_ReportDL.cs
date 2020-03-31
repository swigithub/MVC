using SWI.Libraries.Common;

using System.Data;
using System.Data.SqlClient;

namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
    public class AV_ReportDL
    {
        public DataTable Get(string filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_Report ");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter));
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
