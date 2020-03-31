using AirView.DBLayer.Security.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.Security.DAL
{
    /*----MoB!----*/
    public class Sec_UserSettingsDL
    {
        public DataTable GetDataTable(string filter, string Value1 = null, string Value2 = null, string Value3 = null, string Value4 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetUserSettings");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value1", Value1, "@Value2", Value2, "@Value3", Value3, "@Value4", Value4));
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

        public DataTable GetUserSettingDataTable(string filter, string Value1 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetUserSettings");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value1"));
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


        public DataTable Get(string filter, string Value1 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetUserSettings");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value1", Value1));
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
        public bool Manage(string Filer,Int64 UserId,Int64 TypeId,string TypeValue,string Value1=null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManageUserSettings");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filer,"@UserId", UserId, "@TypeId", TypeId, "@TypeValue", TypeValue, "@Value1", Value1));
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
