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
    public class AD_GetQueryResultDL
    {

       
        public DataTable Get(string filter, string value,string value2=null)
        {

           

            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetQueryResult");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", value, "@Value2", value2));
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
    }
}
