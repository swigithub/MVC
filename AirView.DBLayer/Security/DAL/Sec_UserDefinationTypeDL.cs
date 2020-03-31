using AirView.DBLayer.Security.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.Security.BLL
{
    public class Sec_UserDefinationTypeDL
    {
        public DataTable Get(string filter, string value = null, string value2 = "")
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetUserDefinationTypes");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value", value, "@Value2", value2));
            }
            catch (Exception)
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
