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
 public   class PM_Report_DL
    {

        public DataTable GetDataTable(string Filter, Int64 projectId,string Where="",string Column="",string Group="",Int64 UserId=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Reports");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", projectId, "@Where", Where, "@Column", Column, "@Group", Group));
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

        public DataTable GetDataTable_Exports(string Filter, Int64 projectId, string Where = "", string Column = "", string Group = "", Int64 UserId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Reports_Exports");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", projectId, "@Where", Where, "@Column", Column, "@Group", Group));
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }



        public DataTable GetDataTableRpt(Int64 ProjectId, string Where = "", string Column = "", string Group = "", string Filter="", DateTime? FromDate=null, DateTime? ToDate = null, string Markets = "", string Tasks="")
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Reports");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@ProjectId", ProjectId, "@Where", Where, "@Column", Column, "@Group", Group, "@Filter", Filter, "@FromDate", FromDate, "@ToDate", ToDate, "@Markets", Markets, "@Tasks", Tasks));
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
