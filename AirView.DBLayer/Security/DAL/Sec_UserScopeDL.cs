using SWI.Libraries.Common;
using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.Security.DAL
{
    /*----MoB!----*/
    public class Sec_UserScopeDL
    {
        public bool Manage(int UserId, DataTable data)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManageUserScopes");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@UserId", UserId, "@List", data));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    }
}
