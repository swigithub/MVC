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
  public  class AD_Store
    {
        public DataTable Get(int[] app,string filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetUserSettings");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", "Pendding_Request"));
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Search(DateTime? value)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetUserSettings");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", "Reports_By_Date", "@Value1",value));
            }
            catch (Exception ex)
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
