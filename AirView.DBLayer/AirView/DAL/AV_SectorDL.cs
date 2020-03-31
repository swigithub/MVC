using SWI.Libraries.Common;

using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.DAL
{
  public  class AV_SectorDL
    {

        public DataTable Get(string filter, string SiteId, string NetworkModeId = null, string BandId = null, string CarrierId = null, string ScopeId = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSectors");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@NetworkModeId", NetworkModeId, "@BandId", BandId, "@CarrierId", CarrierId, "@ScopeId", ScopeId, "@SiteId", SiteId));
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


        public bool Manage(DataTable dtSectors, string Filter, string Value1, string Value2)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageSectors");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@SECTORS", dtSectors, "@Filter", Filter, "@Value1", Value1, "@Value2", Value2));
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
