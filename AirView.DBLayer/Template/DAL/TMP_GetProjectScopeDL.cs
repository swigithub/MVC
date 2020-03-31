using SWI.Libraries.Common;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Template.DAL 
{
    class TMP_GetProjectScopeDL
    {
        public DataSet GetDs(string Filter,int ProjectId, int NodeId, int ScopeId, int UserId, string ControlDetailType = null, string ChartType = null, string WhereClause = null, string CustomQuery = null, string TemplateType = "dashboard", int? SiteId = null, int? BandId = null, int?NetworkModeId = null, int? CarrierId = null, string TableName = null, string ColorColumn = null, string Longitude = null, string Latitude = null, string LSiteIdKML = "''=''", string LBandIdKML = "''=''", string LNetworkModeIdKML = "''=''", string LCarrierKML = "''=''")
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_GetProjectScopeReport");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@ScopeId", ScopeId, "@UserId", UserId, "@NodeId", NodeId , "@ControlDetailType", ControlDetailType, "@ChartType", ChartType, "@WhereClause", WhereClause, "@CustomQuery", CustomQuery, "@SiteId", SiteId, "@BandId", BandId, "@CarrierId", CarrierId, "@NetworkModeId", NetworkModeId, "@TableName", TableName, "@TemplateType", TemplateType, "@Color", ColorColumn, "@Longitude", Longitude, "@Latitude", Latitude, "@LSiteIdKML", LSiteIdKML, "@LBandIdKML", LBandIdKML, "@LNetworkModeIdKML", LNetworkModeIdKML, "@LCarrierKML", LCarrierKML));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
                loCommand.Dispose();
            }
        }
    }
}
