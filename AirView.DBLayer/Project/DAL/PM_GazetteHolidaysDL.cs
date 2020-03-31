using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.DAL
{
    public class PM_GazetteHolidaysDL
    {
        public dynamic Manages(string Filter, Int64 ProjectId, string Title, DateTime? Date, bool Isoffday)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageGazetteHolidays");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@Title", Title, "@Date", Date, "IsOffday", Isoffday));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable Get(string filter, string value, string value2 = null, string value3 = null, string value4 = null)
        {



            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetGazetteHolidays");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value1", value, "@Value2", value2, "@Value3", value3, "@Value4", value4));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public bool Manage(string DestinationTableName, DataTable Table)
        {
            return DataContext.InsertBulkIntoSQL(DestinationTableName, Table);
        }
    }
}
