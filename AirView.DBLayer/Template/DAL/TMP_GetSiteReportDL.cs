using SWI.Libraries.Common;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Template.DAL
{
    public class TMP_GetSiteReportDL
    {
        public DataSet GetDs(string Filter, int NodeId, int SiteId, int BandId, int CarrierId, int NetworkModeId, int ScopeId, int PlotId, int UserId, string ControlDetailType = null,string ChartType = null, string WhereClause = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_GetSiteReport");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId, "@NodeId", NodeId, "@BandId", BandId, "@CarrierId", CarrierId, "@NetworkModeId", NetworkModeId, "@ScopeId", ScopeId, "@PlotId", PlotId, "@UserId", UserId, "@ControlDetailType", ControlDetailType,"@ChartType", ChartType, "@WhereClause", WhereClause));
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
