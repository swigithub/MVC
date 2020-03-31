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
  public  class PM_KPI_DL
    {
        public DataTable GetDataTable(string Filter, string value="0",string SDate="",string EDate="", Int64 Site = 0, Int64 Carrier = 0, Int64 Sector = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Project_KPI");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@value",value,"@StartDate",SDate,"@EndDate",EDate,"@Site",Convert.ToString(Site),"@Carrier", Convert.ToString(Carrier), "@Sector", Convert.ToString(Sector)));
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public bool Manage(string Filter, DataTable list, string UserId="")
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Project_KPI");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", list));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch (Exception ex)
            {
                //  DataContext.CancelTransaction(loCommand);
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    
    }
}
