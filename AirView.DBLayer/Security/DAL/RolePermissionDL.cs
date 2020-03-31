using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SWI.Libraries.Security.DAL
{
    /*----MoB!----*/
    public class RolePermissionDL
    {

        public bool Insert(int RoleId, DataTable data)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_Insert_RolePsermissions");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result= DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@RoleId", RoleId, "@List", data));
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
