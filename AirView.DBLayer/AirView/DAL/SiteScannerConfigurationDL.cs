using SWI.Libraries.Common;

using System.Data;
using System.Data.SqlClient;


namespace AirView.DBLayer.AirView.DAL
{
 public   class SiteScannerConfigurationDL
    {
        public bool Save(DataTable SiteConfiguration)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_Insert_SiteScannerConfigurations");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@List", SiteConfiguration));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    }
}
