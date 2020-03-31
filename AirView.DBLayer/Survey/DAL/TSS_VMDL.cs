using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.DAL
{
    /*----MoB!----*/
    /*----28-08-2017----*/
    public class TSS_VMDL
    {
        public DataTable GetDataTable(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetBands");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@SITEID", Value, "@Filter", filter));
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
