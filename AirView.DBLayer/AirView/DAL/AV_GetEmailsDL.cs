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
    public class AV_GetEmailsDL
    {
        public DataTable Get(string filter, string value)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetEmails");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", value));
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
    }
}
