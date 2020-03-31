using SWI.Libraries.Common;

using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.Security.DAL
{
    /*----MoB!----*/
  public  class Sec_DomainConfigurationDL
    {

        public DataTable Get(string filter=null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetDomainConfigurations");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter));
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
