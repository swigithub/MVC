using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SWI.Libraries.Security.DAL
{
    /*----MoB!----*/
  public  class Sec_UserDevicesDL
    {
        public bool Manage(string Filter,decimal DeviceId, decimal UserId, string IMEI, string MAC, string Manufacturer, string Model,bool IsActive,string Password=null,int TranferToId=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManageUserDevice");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@DeviceId", DeviceId, "@UserId", UserId, "@IMEI", IMEI, "@MAC", MAC, "@Manufacturer", Manufacturer, "@Model", Model, "@IsActive", IsActive, "@Password", Password,"@TranferToId", TranferToId));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch(Exception ex)
            {
                DataContext.CancelTransaction(loCommand);
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }


        public DataTable Get(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetUserDevices");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value", Value));
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
