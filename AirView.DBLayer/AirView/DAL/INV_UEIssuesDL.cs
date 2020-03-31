using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.DAL
{
   public class INV_UEIssuesDL
    {
        public DataTable Get(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Inv_GetUEIssues");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", Value));
            }
            catch
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public bool Manage(string Filer, Int64 UEId,  string Description, Int64 UserId = 0,DateTime? Date=null )
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Inv_ManageUEIssues");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filer, "@UEId", UEId, "@Description", Description, "@UserId", UserId, "@Date", Date));
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
