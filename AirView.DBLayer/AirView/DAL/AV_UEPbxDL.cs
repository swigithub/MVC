using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
   public class AV_UEPbxDL
    {

        public DataTable Get(string filter, string value= null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetUEPbx");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value", value));
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


        public bool Manage(string Filter,Int64 UEId,string UEName, string IMEI,bool IsIdle, string @DeviceToken)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageUEPbx");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UEId", UEId, "@UEName", UEName, "@IMEI", IMEI, "@IsIdle", IsIdle, "@DeviceToken", DeviceToken));
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
