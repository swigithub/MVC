using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Security.DAL
{
  public  class Sec_UserProjectsDL
    {
        public bool Manage(Int64 UserId, DataTable data,Int64 Id, string Filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManageUserProjects");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Id", Id, "@Filter",Filter, "@UserId", UserId, "@List", data));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch(Exception ex)
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
