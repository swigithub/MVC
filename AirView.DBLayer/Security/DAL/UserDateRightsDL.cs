using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;


namespace AirView.DBLayer.Security.DAL
{
  public  class UserDateRightsDL
    {
        /*----MoB!----*/
        public bool Insert_Update(int UserId, int DaysForward, int DaysBack, bool IsActive)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_UserDateRights_Insert_Update");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@UserId", UserId, "@DaysForward",DaysForward, "@DaysBack", DaysBack, "@AssignDate",DateTime.Now, "@IsActive", IsActive));
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


        public DataTable Get(string filter, string value)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_Get_UserDateRights");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value", value));
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
    }
}
