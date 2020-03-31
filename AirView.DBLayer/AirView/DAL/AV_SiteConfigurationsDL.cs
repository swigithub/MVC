using SWI.Libraries.Common;

using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
    public class AV_SiteConfigurationsDL
    {
        public DataTable Get(string Filter, string Value1=null, string Value2 = null, string Value3 = null, string Value4 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSiteConfiguration");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@Value1", Value1, "@Value2", Value2, "@Value3", Value3, "@Value4", Value4));
            }
            catch
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
