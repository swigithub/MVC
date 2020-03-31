using SWI.Libraries.Common;

using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
    public class AV_GetRoutePlanDL
    {
        public DataTable Get(string filter, string TesterId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetRoutePlan");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FilterOption", filter, "@TesterId", TesterId));
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
