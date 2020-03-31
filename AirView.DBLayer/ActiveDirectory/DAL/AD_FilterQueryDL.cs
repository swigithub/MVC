using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AD.DAL
{
    /*----MoB!----*/
    public class AD_FilterQueryDL
    {

        public DataTable Get(string filter, string value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetFilterQuery");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@value", value));
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
